using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
namespace RegAppMVC
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/*.css"));

            bundles.Add(new ScriptBundle("~/Scripts/jQuery").Include("~/Scripts/jquery-{version}.js", "~/Scripts/jquery.unobtrusive-ajax.js"));
        }
    }
}