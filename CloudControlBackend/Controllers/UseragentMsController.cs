using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using CloudControl.Model;
using CloudControl.Service;
using CloudControlBackend;

namespace CloudControlBackend.Controllers
{
    public class UseragentMsController : BaseController
    {
        private CloudControlEntities db;
        private UseragentService useragentService;

        public UseragentMsController()
        {
            db = new CloudControlEntities();
            useragentService = new UseragentService();
        }

        /**** Useragent 新增/刪除/修改 ***/
        [CheckSession(IsAuth = true)]
        // GET: UseragentMs
        public ActionResult Useragent(int p = 1)
        {
            var data = useragentService.Get().OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            ViewBag.Useragent = data.ToPagedList(pageNumber: p, pageSize: 20);
            return View();
        }

        [CheckSession(IsAuth = true)]
        public ActionResult AddUseragent()
        {
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult AddUseragent(Useragent useragent)
        {
            if (TryUpdateModel(useragent, new string[] { "Useragent" }) && ModelState.IsValid)
            {                
                useragentService.Create(useragent);
                useragentService.SaveChanges();
            }
            return RedirectToAction("Useragent");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult DeleteUseragent(int Useragentid)
        {
            Useragent useragent = useragentService.GetByID(Useragentid);
            useragentService.Delete(useragent);
            useragentService.SaveChanges();
            return RedirectToAction("Useragent");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult EditUseragent(int p, int Useragentid)
        {
            ViewBag.pageNumber = p;
            Useragent useragent = useragentService.GetByID(Useragentid);
            return View(useragent);
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult EditUseragent(int Useragentid)
        {
            Useragent useragent = useragentService.GetByID(Useragentid);
            if (TryUpdateModel(useragent, new string[] { "Useragent" }) && ModelState.IsValid)
            {
                useragent.Createdate = dt_tw();
                useragent.Updatedate = dt_tw();
                useragentService.Update(useragent);
                useragentService.SaveChanges();
            }
            return RedirectToAction("Useragent");
        }

        #region --取得台灣時間--
        public DateTime dt_tw()
        {
            DateTime dt = DateTime.Now.ToUniversalTime();
            DateTime dttw = TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time"));
            return dttw;
        }
        #endregion
    }
}