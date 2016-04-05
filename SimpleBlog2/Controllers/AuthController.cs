using SimpleBlog2.Models;
using SimpleBlog2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SimpleBlog2.Controllers
{
    public class AuthController : Controller
    {

        private static string UserKey = "SimpleBlog.User";

        public ActionResult logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToRoute("home");
        }
       
        public ActionResult login()
        {
            return View(new AuthLogin(){Test="test value from login get method"});
        }

        [HttpPost]
        public ActionResult login(AuthLogin form,string returnUrl)
        {

            var user = Database.Session.QueryOver<User>().Where(p => p.Username == form.UserName).SingleOrDefault();

            //if (user == null)
            //{
            //    new User().FakeHash();
            //    return View(form);
            //}

            //if (user == null || !user.CheckPasword(form.Password))
            //{
            //    ModelState.AddModelError("username", "Username or password is incorrect");
            //}

            if (!ModelState.IsValid)
            {
                return View(form);
            }



            FormsAuthentication.SetAuthCookie(form.UserName, true);


            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToRoute("home");
            }

        }


        public static User User {

            get
            {
                if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return null;
                }


                var user = System.Web.HttpContext.Current.Items[UserKey] as User;

                if (user == null)
                {
                    user = Database.Session.QueryOver<User>().Where(p => p.Username == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();

                    if (user == null)
                    {
                        return null;
                        
                    }
                    System.Web.HttpContext.Current.Items[UserKey] = user;
                }

                return user;

            }
        }


      
    }
}
