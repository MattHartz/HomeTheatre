﻿using System.Web;
using System.Web.Optimization;
using React;

namespace HomeTheatre
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            bundles.Add(new ScriptBundle("~/Scripts/templates")
                .IncludeDirectory("~/Content/scripts/templates", "*.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .IncludeDirectory("~/Content/css", "*.css"));
        }
    }
}
