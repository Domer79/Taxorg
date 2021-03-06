﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SystemTools;
using SystemTools.ConfigSections;
using SystemTools.Extensions;
using SystemTools.Interfaces;
using SystemTools.WebTools.Helpers;
using SystemTools.WebTools.Infrastructure;
using DataRepository.Infrastructure;
using TaxOrg.Controllers;
using TaxorgRepository;
using TaxorgRepository.Models;
using TaxorgRepository.Repositories;
using WebSecurity;

namespace TaxOrg
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ApplicationCustomizer.RegisterErrorLog(ErrorLog.SaveError);
            ApplicationCustomizer.AppVersion = TaxorgTools.AppVersion;
            ContextInfo.ContextInfoCollection.Add(typeof(TaxorgContext));

            #region Enable Security

            ApplicationCustomizer.Security = Security.Instance;
            ControllerBuilder.Current.SetControllerFactory(new SecurityControllerFactory());
            Security.Instance.SetAccessTypes<SecurityAccessType>();
            ApplicationCustomizer.EnableSecurity = true;
            ApplicationCustomizer.SecurityConnectionString = ApplicationSettings.SecurityConnectionString;
            ApplicationCustomizer.EnableSecurityAdminPanel = true;
            ControllerCollection.Assemblies.Add(typeof(OrgController).Assembly);
            ControllerHelper.ControllerCollection.Add<MvcFileUploader.MvcFileUploadController>();

            #endregion
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            exception.SaveError();
            Server.ClearError();
            try
            {
//                Context.Response.Cookies.Add(new HttpCookie("exception", exception.SerializeToString()));
                Session["errorObject"] = exception;
            }
            catch
            {
                Context.Response.Cookies.Add(new HttpCookie("exception", exception.SerializeToString()));
            }
            ApplicationCustomizer.IsError = true;
            Response.RedirectToRoute(new { AdditionalConfiguration.Instance.ErrorPage.Controller, AdditionalConfiguration.Instance.ErrorPage.Action });
        }

        protected void Session_Start()
        {
            var user = ((UserIdentity) Security.Instance.Principal.Identity).User;
            Session["User"] = user;
            SessionRepository.AddNewSession(Session.SessionID, user);
            ApplicationCustomizer.CurrentSessionId = Session.SessionID;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Security.RenewContext();
        }
    }
}
