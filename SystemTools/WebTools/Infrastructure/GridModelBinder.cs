﻿using System;
using System.Web.Mvc;

namespace SystemTools.WebTools.Infrastructure
{
    public class GridModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                var request = controllerContext.HttpContext.Request;
                return new GridSettings
                    {
                        IsSearch = bool.Parse(request["_search"] ?? "false"),
                        PageIndex = int.Parse(request["page"] ?? "1"),
                        PageSize = int.Parse(request["rows"] ?? "10"),
                        SortColumn = request["sidx"] ?? "",
                        SortOrder = request["sord"] ?? "asc",
                        Where = Filter.Create(request["filters"] ?? "")
                    };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}