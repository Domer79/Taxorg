using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SystemTools.Extensions;
using SystemTools.WebTools.Infrastructure;
using DataRepository.Exceptions;
using DataRepository.Infrastructure;
using MvcFileUploader;
using MvcFileUploader.Models;
using TaxOrg.Tools;
using TaxorgRepository;
using TaxorgRepository.Exceptions;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;
using SystemTools;
using TaxorgRepository.Tools;
using Organization = TaxOrg.Tools.Organization;

namespace TaxOrg.Controllers
{
    public class UploaderController : Controller
    {
        //
        // GET: /MvcUploaderTest/Index

        public ActionResult Index(bool? inline, string ui = "bootstrap")
        {
            if (TaxorgTools.IsMaintenance)
                return RedirectToAction("Maintenance", "Org");

            return View(inline);
        }

        public ActionResult UploadFile(int? entityId) // optionally receive values specified with Html helper
        {
            // here we can send in some extra info to be included with the delete url 
            var statuses = new List<ViewDataUploadFileResult>();
            for (var i = 0; i < Request.Files.Count; i++)
            {
                string excelPath = null;
                try
                {
                    var st = FileSaver.StoreFile(x =>
                    {
                        x.File = Request.Files[i];
                        //note how we are adding an additional value to be posted with delete request
                        //and giving it the same value posted with upload
                        x.DeleteUrl = Url.Action("DeleteFile", new {entityId = entityId});
                        x.StorageDirectory = Server.MapPath("~/Content/uploads");
                        x.UrlPrefix = "/Content/uploads"; // this is used to generate the relative url of the file
//                        x.UrlPrefix = ApplicationCustomizer.ExcelFilePath;
                        // this is used to generate the relative url of the file
//                        x.StorageDirectory = ApplicationCustomizer.ExcelFilePath;


                        //overriding defaults
                        x.FileName = Request.Files[i].FileName; // default is filename suffixed with filetimestamp
                        x.ThrowExceptions = false;
                        //default is false, if false exception message is set in error property
                    });

                    statuses.Add(st);
                    excelPath = st.FullPath;

                    try
                    {
                        FileSystemRepository.FileSave(Request.Files[i], excelPath);
                    }
                    catch (Exception e)
                    {
                        e.SaveError();
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                            e.GetErrorMessage() + ". " + st.error);
                    }

                }
                catch (Exception e)
                {
                    e.SaveError();
                }

                try
                {
                    if (excelPath == null)
                        throw new InvalidOperationException(
                            "��������� ������ �� ����� ���������� �����. ���� � ����� �� ���������������!");

                    TaxorgTools.CheckSaveTaxAccess();

                    using (var csvReader = new CsvReader<Organization>(excelPath, (e, row) =>
                    {
                        Buges.SaveBugRow(row, e.Message);
                        e.SaveError();
                    }))
                    {
                        var taxRepository = new TaxRepository();
                        var firstOrg = csvReader.First();
                        taxRepository.DeletePeriod(firstOrg.Date);

                        foreach (var organization in csvReader)
                        {
                            taxRepository.SaveTaxToDb(organization.Inn, organization.TaxCode, organization.TaxName,
                                organization.Date,
                                organization.Tax);
                        }

                    }
                }
                catch (Exception e)
                {
                    e.SaveError();
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }

            //statuses contains all the uploaded files details (if error occurs then check error property is not null or empty)
            //todo: add additional code to generate thumbnail for videos, associate files with entities etc

            //adding thumbnail url for jquery file upload javascript plugin
            statuses.ForEach(x => x.thumbnailUrl = x.url + "?width=80&height=80");
            // uses ImageResizer httpmodule to resize images from this url

            //setting custom download url instead of direct url to file which is default
            statuses.ForEach(x => x.url = Url.Action("Index", "Org"));


            //server side error generation, generate some random error if entity id is 13
            if (entityId == 13)
            {
                var rnd = new Random();
                statuses.ForEach(x =>
                {
                    //setting the error property removes the deleteUrl, thumbnailUrl and url property values
                    x.error = rnd.Next(0, 2) > 0
                        ? "We do not have any entity with unlucky Id : '13'"
                        : String.Format("Your file size is {0} bytes which is un-acceptable", x.size);
                    //delete file by using FullPath property
                    if (System.IO.File.Exists(x.FullPath)) System.IO.File.Delete(x.FullPath);
                });
            }

            var viewresult = Json(new {files = statuses});
            //for IE8 which does not accept application/json
            if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
                viewresult.ContentType = "text/javascript";

            return viewresult;
        }

        public ActionResult UploadAllFiles(bool? inline, string ui = "bootstrap")
        {
            return View(inline);
        }

        [HttpPost]
        public ActionResult UploadAllFiles(int? entityId)
        {
            // here we can send in some extra info to be included with the delete url 
            var statuses = new List<ViewDataUploadFileResult>();
            for (var i = 0; i < Request.Files.Count; i++)
            {
                string excelPath = null;
                try
                {
                    var st = FileSaver.StoreFile(x =>
                    {
                        x.File = Request.Files[i];
                        //note how we are adding an additional value to be posted with delete request
                        //and giving it the same value posted with upload
                        x.DeleteUrl = Url.Action("DeleteFile", new { entityId = entityId });
                        x.StorageDirectory = Server.MapPath("~/Content/uploads");
                        x.UrlPrefix = "/Content/uploads"; // this is used to generate the relative url of the file
                        //                        x.UrlPrefix = ApplicationCustomizer.ExcelFilePath;
                        // this is used to generate the relative url of the file
                        //                        x.StorageDirectory = ApplicationCustomizer.ExcelFilePath;


                        //overriding defaults
                        x.FileName = Request.Files[i].FileName; // default is filename suffixed with filetimestamp
                        x.ThrowExceptions = false;
                        //default is false, if false exception message is set in error property
                    });

                    statuses.Add(st);
                    excelPath = st.FullPath;

                    try
                    {
                        st.name = FileSystemRepository.FileSave(Request.Files[i], excelPath).ToString();
                    }
                    catch (Exception e)
                    {
                        e.SaveError();
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                            e.GetErrorMessage() + ". " + st.error);
                    }

                }
                catch (Exception e)
                {
                    e.SaveError();
                }
            }

            //statuses contains all the uploaded files details (if error occurs then check error property is not null or empty)
            //todo: add additional code to generate thumbnail for videos, associate files with entities etc

            //adding thumbnail url for jquery file upload javascript plugin
            statuses.ForEach(x => x.thumbnailUrl = x.url + "?width=80&height=80");
            // uses ImageResizer httpmodule to resize images from this url

            //setting custom download url instead of direct url to file which is default
//            statuses.ForEach(x => x.name = );


            //server side error generation, generate some random error if entity id is 13
            if (entityId == 13)
            {
                var rnd = new Random();
                statuses.ForEach(x =>
                {
                    //setting the error property removes the deleteUrl, thumbnailUrl and url property values
                    x.error = rnd.Next(0, 2) > 0
                        ? "We do not have any entity with unlucky Id : '13'"
                        : String.Format("Your file size is {0} bytes which is un-acceptable", x.size);
                    //delete file by using FullPath property
                    if (System.IO.File.Exists(x.FullPath)) System.IO.File.Delete(x.FullPath);
                });
            }

            var viewresult = Json(new { files = statuses });
            //for IE8 which does not accept application/json
            if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
                viewresult.ContentType = "text/javascript";

            return viewresult;
        }

        //here i am receving the extra info injected
        [HttpPost] // should accept only post
        public ActionResult DeleteFile(int? entityId, string fileUrl)
        {
            var filePath = Server.MapPath("~" + fileUrl);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            var viewresult = Json(new {error = String.Empty});
            //for IE8 which does not accept application/json
            if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
                viewresult.ContentType = "text/plain";

            return viewresult; // trigger success
        }

        public ActionResult DownloadFile(string fileUrl, string mimetype)
        {
            var filePath = Server.MapPath("~" + fileUrl);

            if (System.IO.File.Exists(filePath))
                return File(filePath, mimetype);
            else
            {
                return new HttpNotFoundResult("File not found");
            }
        }

        public ActionResult DownloadFile2(int idFile)
        {
            var file = FileSystemRepository.GetObjects().First(fs => fs.IdFileSystem == idFile);
            return File(FileSystemRepository.GetStreamFromData(idFile), file.ContentType);
        }
    }
}