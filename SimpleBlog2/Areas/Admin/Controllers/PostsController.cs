using SimpleBlog2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using SimpleBlog2.Models;
using SimpleBlog2.Areas.Admin.ViewModels;
using SimpleBlog2.Controllers;

namespace SimpleBlog2.Areas.Admin.Controllers
{

    [Authorize(Roles="admin")]
    [SelectedTab("posts")]
    public class PostsController : Controller
    {
        //
        // GET: /Admin/Posts/

        public ActionResult Index(int page=1)
        {
            //var total

            var totalPostCount = Database.Session.Query<Post>().Count();

            var currentPosts = Database.Session.Query<Post>()
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * postsPerPage)
                .Take(postsPerPage)
                .ToList();


            return View(new PostsIndex()
            {
                Posts = new PageData<Post>(currentPosts, totalPostCount, page, postsPerPage)
            });

                
                
        }

        public ActionResult New()
        {

            var tags = Database.Session.Query<Tag>().ToList();

            return View("form", new PostsForm() { isNew = true, Tags = Database.Session.
                Query<Tag>().
                Select(tag => new TagCheckBox() { 
                    Id=tag.Id,
                    Name=tag.Name,
                    isChecked=false
                }).ToList() });
            
        }

        public ActionResult Edit(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
            {
                return HttpNotFound();

            }


            return View("form",
                new PostsForm()
                {
                    isNew = false,
                    postId = post.Id,
                    Content = post.Content,
                    Slug = post.Slug,
                    Title = post.Title,
                    Tags = Database.Session.Query<Tag>().Select(tag => new TagCheckBox() { 
                    
                        Id=tag.Id,
                        Name=tag.Name,
                        isChecked=post.Tags.Contains(tag)

                    }).ToList()
                });

        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Form(PostsForm form)
        {
            var post=new Post();

            form.isNew = form.postId == null;

            if (!ModelState.IsValid)
            {
                return View(form);
            }

            if (form.isNew)
            {
                post = new Post()
                {
                    CreatedAt = DateTime.UtcNow,
                    User = AuthController.User
                };
            }
            else
            {
                post = Database.Session.Load<Post>(form.postId);
                post.UpdatedAt = DateTime.UtcNow;
                if (post == null)
                    return HttpNotFound();
            }

            post.Title = form.Title;
            post.Slug = form.Slug;
            post.Content = form.Content;


            Database.Session.SaveOrUpdate(post);
            return RedirectToAction("index");


        }



        [HttpPost]
        public ActionResult Trash(int id)
        {
            var post = Database.Session.Load<Post>(id);

            if (post == null)
            {
                return HttpNotFound();
            }
            post.DeletedAt = DateTime.UtcNow;

            Database.Session.SaveOrUpdate(post);
            return RedirectToAction("index");
                


        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            

            var post = Database.Session.Load<Post>(id);

            if (post == null)
            {
                return HttpNotFound();
            }
           
            Database.Session.Delete(post);
            return RedirectToAction("index");

        }


        [HttpPost]
        public ActionResult Restore(int id)
        {
            var post = Database.Session.Load<Post>(id);

            if (post == null)
            {
                return HttpNotFound();
            }
            post.DeletedAt = null;

            Database.Session.SaveOrUpdate(post);
            return RedirectToAction("index");



        }
        private static int postsPerPage = 5;

        public User Auth { get; set; }
    }
}
