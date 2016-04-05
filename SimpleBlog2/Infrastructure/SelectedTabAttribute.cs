using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleBlog2.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SelectedTabAttribute:ActionFilterAttribute
    {
        private readonly string _selecetTab;

        public SelectedTabAttribute(string selectedTab) {
            _selecetTab = selectedTab;

        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.SelectedTab = _selecetTab;
        }
    
    }
}