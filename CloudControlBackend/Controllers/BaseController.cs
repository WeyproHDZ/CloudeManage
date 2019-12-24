using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudControl.Model;
using CloudControl.Service;

namespace CloudControlBackend.Controllers
{
    public class BaseController : Controller
    {
        private LimsService limsService;
        protected string container = ConfigurationManager.AppSettings["azure.blob.container"];
        protected string url = ConfigurationManager.AppSettings["azure.blob.url"];

        public BaseController()
        {
            limsService = new LimsService();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.SiteLinks = limsService.Get().OrderBy(p => p.ParentID).ThenBy(s => s.Sort);
            ViewBag.BlobUrl = url + "/" + container + "/";
            base.OnActionExecuted(filterContext);
        }
    }
}