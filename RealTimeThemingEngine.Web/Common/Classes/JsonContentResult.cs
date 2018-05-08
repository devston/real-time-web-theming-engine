﻿using System.Net;
using System.Web.Mvc;

namespace RealTimeThemingEngine.Web.Common.Classes
{
    public class JsonContentResult : PartialViewResult
    {
        private string _content;
        private HttpStatusCode _statusCode;
        private string _statusDescription;

        public JsonContentResult(string content,
                                HttpStatusCode statusCode = HttpStatusCode.OK,
                                string statusDescription = null)
        {
            _content = content;
            _statusCode = statusCode;
            _statusDescription = statusDescription;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = (int)_statusCode;

            if (_statusDescription != null)
            {
                response.StatusDescription = _statusDescription;
            }

            if (_content != null)
            {
                context.HttpContext.Response.Write(_content);
            }

            context.HttpContext.Response.ContentType = "text/json";
        }
    }
}