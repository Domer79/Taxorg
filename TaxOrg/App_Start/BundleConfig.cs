using System.Web.Optimization;

namespace TaxOrg
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/jquery").Include(
                "~/scripts/jquery-2.1.3.js"
                ));

            bundles.Add(new ScriptBundle("~/jqgrid").Include(
                "~/scripts/jquery.jqGrid.src.js"
                ));

            bundles.Add(new ScriptBundle("~/jquery-ui").Include(
                "~/scripts/jquery-ui-1.11.1.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/jqgridlocale").Include(
                "~/scripts/i18n/grid.locale-ru.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/mvcfileupload/blueimp").Include(
                    "~/Scripts/mvcfileupload/blueimp/jquery.ui.widget.js",
                    "~/Scripts/mvcfileupload/blueimp/tmpl.min.js",
                    "~/Scripts/mvcfileupload/blueimp/load-image.min.js",
                    "~/Scripts/mvcfileupload/blueimp/canvas-to-blob.min.js",
                    "~/Scripts/mvcfileupload/blueimp/jquery.iframe-transport.js",
                    "~/Scripts/mvcfileupload/blueimp/jquery.fileupload.js",
                    "~/Scripts/mvcfileupload/blueimp/jquery.fileupload-process.js",
                    "~/Scripts/mvcfileupload/blueimp/jquery.fileupload-image.js",
                    "~/Scripts/mvcfileupload/blueimp/jquery.fileupload-validate.js",
                    "~/Scripts/mvcfileupload/blueimp/jquery.fileupload-ui.js"
//                    "~/Scripts/mvcfileupload/blueimp/jquery.fileupload-jquery-ui.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/mvcfilepload/vendor").Include(
                    "~/scripts/mvcfilepload/vendor/bootstrap.min.js",
                    "~/scripts/mvcfilepload/vendor/jquery.ui.widget.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/modernizr").Include(
                    "~/scripts/modernizr-2.6.2.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/bootstrap").Include(
//                    "~/Scripts/mvcfileupload/vendor/bootstrap.min.js"
                    "~/Scripts/bootstrap-3.3.4-dist/js/bootstrap.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/blueimp").Include(
                    "~/Content/mvcfileupload/blueimp/jquery.fileupload.css",
                    "~/Content/mvcfileupload/blueimp/jquery.fileupload-ui.css"
                ));

            #region Темы jQuery-UI

//            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
//                "~/Content/jquery-ui.css",
//                "~/Content/jquery-ui.structure.css",
//                "~/Content/themes/base/jquery-ui.theme.css",
//                "~/Content/jquery.jqGrid/ui.jqgrid.css"
//                ));
            bundles.Add(new StyleBundle("~/Content/themes/black-tie/css").Include(
                "~/Content/jquery-ui.css",
                "~/Content/jquery-ui.structure.css",
                "~/Content/themes/black-tie/jquery-ui.theme.css",
                "~/Content/jquery.jqGrid/ui.jqgrid.css"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/blitzer/css").Include(
                "~/Content/jquery-ui.css",
                "~/Content/jquery-ui.structure.css",
                "~/Content/themes/blitzer/jquery-ui.theme.css",
                "~/Content/jquery.jqGrid/ui.jqgrid.css"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/smoothness/css").Include(
                "~/Content/jquery-ui.css",
                "~/Content/jquery-ui.structure.css",
                "~/Content/themes/smoothness/jquery-ui.theme.css",
                "~/Content/jquery.jqGrid/ui.jqgrid.css"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/redmond/css").Include(
                "~/Content/jquery-ui.css",
                "~/Content/jquery-ui.structure.css",
                "~/Content/themes/redmond/jquery-ui.theme.css",
                "~/Content/jquery.jqGrid/ui.jqgrid.css"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/ui-lightness/css").Include(
                "~/Content/jquery-ui.css",
                "~/Content/jquery-ui.structure.css",
                "~/Content/themes/ui-lightness/jquery-ui.theme.css",
                "~/Content/jquery.jqGrid/ui.jqgrid.css"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/overcast/css").Include(
                "~/Content/jquery-ui.css",
                "~/Content/jquery-ui.structure.css",
                "~/Content/themes/overcast/jquery-ui.overcast.css",
                "~/Content/jquery.jqGrid/ui.jqgrid.css"
                ));

            #endregion

            bundles.Add(new StyleBundle("~/bootstrap/css").Include(
                "~/Content/bootstrap-3.3.4-dist/css/bootstrap-theme.css",
                "~/Content/bootstrap-3.3.4-dist/css/bootstrap.css"
                ));
        }
    }
}