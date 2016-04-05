using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace SimpleBlog2
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/admin/styles").
                Include("~/content/styles/admin.css").
                Include("~/content/styles/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/admin/scripts").
               Include("~/content/scripts/jquery-1.11.2.min.js").
               Include("~/content/scripts/form.js"));

            bundles.Add(new StyleBundle("~/styles").
                Include("~/content/styles/site.css").
                Include("~/content/styles/bootstrap.css"));
        }
    }
}