using SimpleBlog2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using SimpleBlog2.Models;
using SimpleBlog2.Infrastructure;

namespace SimpleBlog2.Controllers
{
    public class PostsController:Controller
    {
       

        public ActionResult index() {

            return View();
        }
    }
}