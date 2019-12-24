using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using CloudControlBackend;
using CloudControl.Model;
using CloudControl.Service;

namespace CloudControlBackend.Controllers
{  
    public class MessageMsController : BaseController
    {
        private CloudControlEntities db;
        private CategoryMessageSevice categorymessageService;
        private MessageService messageService;

        public MessageMsController()
        {
            db = new CloudControlEntities();
            categorymessageService = new CategoryMessageSevice();
            messageService = new MessageService();
        }

        /***** 留言類別 查詢/新增/刪除/修改 *****/
        [CheckSession(IsAuth = true)]
        // GET: MessageMs
        public ActionResult CategoryMessage(int p = 1)
        {
            var data = categorymessageService.Get().OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            ViewBag.CategoryMessage = data.ToPagedList(pageNumber: p, pageSize: 20);
            return View();
        }

        [CheckSession(IsAuth = true)]
        public ActionResult AddCategoryMessage()
        {
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult AddCategoryMessage(CategoryMessage categorymessage)
        {
            if (TryUpdateModel(categorymessage, new string[] { "Categoryname" }) && ModelState.IsValid)
            {
                categorymessage.Categoryid = Guid.NewGuid();
                categorymessage.Createdate = dt_tw();
                categorymessage.Updatedate = dt_tw();
                categorymessageService.Create(categorymessage);
                categorymessageService.SaveChanges();
            }
            return RedirectToAction("CategoryMessage");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult DeleteCategoryMessage(Guid Categoryid)
        {
            CategoryMessage categorymessage = categorymessageService.GetByID(Categoryid);
            categorymessageService.Delete(categorymessage);
            categorymessageService.SaveChanges();
            return RedirectToAction("CategoryMessage");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult EditCategoryMessage(Guid Categoryid, int p)
        {
            CategoryMessage categorymessage = categorymessageService.GetByID(Categoryid);
            ViewBag.pageNumber = p;
            return View(categorymessage);
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult EditCategoryMessage(Guid Categoryid)
        {
            CategoryMessage categorymessage = categorymessageService.GetByID(Categoryid);
            if (TryUpdateModel(categorymessage, new string[] { "Categoryname" }) && ModelState.IsValid)
            {
                categorymessageService.Update(categorymessage);
                categorymessageService.SaveChanges();
            }
            return RedirectToAction("CategoryMessage");
        }

        /***** 留言 新增/刪除/修改 ****/
        [HttpGet]
        [CheckSession(IsAuth = true)]
        // GET: MessageMs
        public ActionResult Message(int p = 1)
        {                    
            var data = messageService.Get().OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            ViewBag.Message = data.ToPagedList(pageNumber: p, pageSize: 20);
            /*** 留言類別選單 ***/
            CategoryMessageDropDownList();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult Message(Guid? Categoryid, int p = 1)
        {
            if(Categoryid != null)
            {
                var data = messageService.Get().Where(a => a.Categoryid == Categoryid).OrderByDescending(o => o.Createdate);
                ViewBag.pageNumber = p;
                ViewBag.Message = data.ToPagedList(pageNumber: p, pageSize: 20);
            }
            else
            {
                var data = messageService.Get().OrderByDescending(o => o.Createdate);
                ViewBag.pageNumber = p;
                ViewBag.Message = data.ToPagedList(pageNumber: p, pageSize: 20);
            }

            /*** 留言類別選單 ***/
            CategoryMessageDropDownList();
            return View();
        }

        [CheckSession(IsAuth = true)]
        public ActionResult AddMessage()
        {
            /**** 留言類別選單 ****/
            CategoryMessageDropDownList();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult AddMessage(string MessageName, Guid? Categoryid)
        {
            string[] MessageName_array;
            MessageName_array = MessageName.Split(Environment.NewLine.ToCharArray());
            if(Categoryid != null)
            {
                foreach (string message in MessageName_array)
                {
                    if(message != "")
                    {
                        Message Message = new Message();
                        Message.Messageid = Guid.NewGuid();
                        Message.Categoryid = Categoryid;
                        Message.MessageName = message;
                        Message.Createdate = dt_tw();
                        Message.Updatedate = dt_tw();
                        messageService.Create(Message);
                    }           
                }
                messageService.SaveChanges();
            }
            return RedirectToAction("Message");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult DeleteMessage(Guid Messageid)
        {
            Message message = messageService.GetByID(Messageid);
            messageService.Delete(message);
            messageService.SaveChanges();
            return RedirectToAction("Message");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult EditMessage(Guid Messageid, int p)
        {
            ViewBag.pageNumber = p;
            Message message = messageService.GetByID(Messageid);
            /**** 留言類別選單 ****/
            CategoryMessageDropDownList(message);
            return View(message);
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult EditMessage(Guid Messageid)
        {
            Message message = messageService.GetByID(Messageid);
            if (TryUpdateModel(message, new string[] { "MessageName", "Categoryid" }) && ModelState.IsValid)
            {
                messageService.Update(message);
                messageService.SaveChanges();
            }
            return RedirectToAction("Message");
        }
        #region --留言類別選單--
        private void CategoryMessageDropDownList(Object selectMember = null)
        {
            var querys = categorymessageService.Get();

            ViewBag.Categoryid = new SelectList(querys, "Categoryid", "Categoryname", selectMember);
        }
        #endregion
        #region --取得台灣時間--
        public DateTime dt_tw()
        {
            DateTime dt = DateTime.Now.ToUniversalTime();
            DateTime dttw = dt.AddHours(8);
            return dttw;
        }
        #endregion
    }
}