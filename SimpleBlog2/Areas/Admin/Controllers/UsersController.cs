using SimpleBlog2.Areas.Admin.ViewModels;
using SimpleBlog2.Infrastructure;
using SimpleBlog2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleBlog2.Areas.Admin.Controllers
{
    [SelectedTab("users")]
    public class UsersController : Controller
    {
        //
        // GET: /Admin/Users/

        public ActionResult Index()
        {
            return View(
                new UsersIndex() { Users=Database.Session.QueryOver<User>().List()}
                );
         
        }

        public ActionResult New()
        {


            List<RoleCheckBox> roles = new List<RoleCheckBox>();

            var query = Database.Session.QueryOver<Role>().List();
            
            foreach (Role r in query)
            {
                roles.Add(new RoleCheckBox() { 
                    Id=r.Id,
                    Name=r.Name,
                    isChecked=false
                });

            }


            return View(new UsersNew()
            {
                Roles =roles

            });
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult New(UsersNew form)
        {
            if (Database.Session.QueryOver<User>().Where(x => x.Username == form.Username).RowCount() > 0)
            {
                ModelState.AddModelError("username", "Usernames must me unique");
            }


            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var user = new User()
            {
                Username = form.Username,
                Email = form.Email
            };

            
            user.SetPassword(form.Password);
            user.Roles = SyncRoles(form.Roles);

            Database.Session.Save(user);
    
            return RedirectToAction("index");

        }


        private List<Role> SyncRoles(List<RoleCheckBox> checkBoxes)
        {
            var selectedRoles = new List<Role>();
            List<Role> roles = new List<Role>();

            foreach (var role in Database.Session.Query<Role>())
            {
                var checkBox = checkBoxes.Single(p => p.Id == role.Id);
                checkBox.Name = role.Name;

                if (checkBox.isChecked)
                {
                    selectedRoles.Add(role);
                }

            }



            foreach (var r in selectedRoles)
            {
                roles.Add(r);
            }

            return roles;

            


        }

        public ActionResult Edit(int id)
        {
           var user = Database.Session.Load<User>(id);


           List<RoleCheckBox> roles = new List<RoleCheckBox>();

           var query = Database.Session.QueryOver<Role>().List();
            
           foreach (Role r in query)
           {
                roles.Add(new RoleCheckBox() { 
                    Id=r.Id,
                    Name=r.Name,
                    isChecked=false
                });

           }

           foreach (var role in user.Roles)
           {
               var selectedRole = roles.Single(p => p.Id == role.Id);
               selectedRole.isChecked = true;
           }

          
           
            return View(new UsersEdit() {Username=user.Username,Email=user.Email ,Roles=roles});
        }


        [HttpPost]
        public ActionResult Edit(int id,UsersEdit form)
        {
            if (Database.Session.QueryOver<User>().Where(x => x.Username == form.Username).Where(p=>p.Id!=id).RowCount() > 0)
            {
                ModelState.AddModelError("username", "Usernames must me unique");
            }


            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var user = new User()
            {
                Id=id,
                Username = form.Username,
                Email = form.Email
            };


            user.SetPassword("");
            
            user.Roles = SyncRoles(form.Roles);

            Database.Session.Update(user);

            return RedirectToAction("index");

        }

        public ActionResult ResetPassword(int id)
        {

            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(new UsersResetPassword() { Username = user.Username});
        }

        [HttpPost]
        public ActionResult ResetPassword(UsersResetPassword form,int id)
        {

            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.SetPassword(form.Password);


            if (!ModelState.IsValid)
            {
                return View(form);
            }

            Database.Session.Update(user);

            return RedirectToAction("index");
        }


        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            Database.Session.Delete(user);


            return RedirectToAction("index");
        }

    }
}
