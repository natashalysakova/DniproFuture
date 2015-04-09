using System.Web;
using System.Web.Optimization;

namespace DniproFuture
{
    public class BundleConfig
    {
        // Дополнительные сведения о Bundling см. по адресу http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.unobtrusive*","~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/animation").Include("~/Scripts/cbpAnimatedHeader.js"));

            bundles.Add(new ScriptBundle("~/bundles/agency").Include("~/Scripts/agency.js"));
            bundles.Add(new ScriptBundle("~/agency/classie").Include("~/Scripts/classie.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include("~/Scripts/moment.min.js", "~/Scripts/moment-with-locales.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/dateTimePicker").Include("~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include("~/Scripts/admin.js"));

            bundles.Add(new ScriptBundle("~/bundles/lightboxJS").Include("~/Scripts/ekko-lightbox.js"));

            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include("~/Scripts/tinymce/tinymce.min.js"));


            
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/css/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/css/agency.css"));
            bundles.Add(new StyleBundle("~/Content/flags").Include("~/Content/css/flags.css"));
            bundles.Add(new StyleBundle("~/Content/customBootstrap").Include("~/Content/css/customBootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/lightboxCSS").Include("~/Content/css/ekko-lightbox.min.css"));
            bundles.Add(new StyleBundle("~/Content/datetimecss").Include("~/Content/css/bootstrap-datetimepicker.min.css"));




        }
    }
}