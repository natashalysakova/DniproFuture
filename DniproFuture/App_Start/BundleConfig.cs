using System.Web;
using System.Web.Optimization;

namespace DniproFuture
{
    public class BundleConfig
    {
        // Дополнительные сведения о Bundling см. по адресу http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/animation").Include(
                         "~/Scripts/cbpAnimatedHeader.js"));

            bundles.Add(new ScriptBundle("~/bundles/agency").Include(
            "~/Scripts/agency.js"));
            bundles.Add(new ScriptBundle("~/agency/classie").Include(
                        "~/Scripts/classie.js"));

    //        <!-- jQuery -->
    //<script src="js/jquery.js"></script>

    //<!-- Bootstrap Core JavaScript -->
    //<script src="js/bootstrap.min.js"></script>

    //<!-- Plugin JavaScript -->
    //<script src="http://cdnjs.cloudflare.com/ajax/libs/jquery-easing/1.3/jquery.easing.min.js"></script>
    //<script src="js/classie.js"></script>
    //<script src="js/cbpAnimatedHeader.js"></script>

    //<!-- Contact Form JavaScript -->
    //<script src="js/jqBootstrapValidation.js"></script>
    //<script src="js/contact_me.js"></script>

    //<!-- Custom Theme JavaScript -->
    //<script src="js/agency.js"></script>
            
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/css/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/css/agency.css"));
            bundles.Add(new StyleBundle("~/Content/flags").Include("~/Content/css/flags.css"));
            bundles.Add(new StyleBundle("~/Content/customBootstrap").Include("~/Content/css/customBootstrap.css"));




        }
    }
}