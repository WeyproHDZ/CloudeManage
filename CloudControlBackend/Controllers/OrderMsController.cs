using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using CloudControlBackend;
using CloudControl.Service;
using CloudControl.Model;

namespace CloudControlBackend.Controllers
{
    public class OrderMsController : BaseController
    {
        private CloudControlEntities db;
        private FBMembersService fbmembersService;
        private FBOrderService fborderService;
        private FBOrderlistService fborderlistService;
        private IGMembersService igmembersService;
        private IGOrderService igorderService;
        private IGOrderlistService igorderlistService;
        private YTMembersService ytmembersService;
        private YTOrderService ytorderService;
        private YTOrderlistService ytorderlistService;
        private CategoryProductService categoryproductService;
        private ProductService productService;
        private CategoryMessageSevice categorymessageService;
        private MessageService messageService;
        public OrderMsController()
        {
            db = new CloudControlEntities();
            fbmembersService = new FBMembersService();
            fborderService = new FBOrderService();
            fborderlistService = new FBOrderlistService();
            igmembersService = new IGMembersService();
            igorderService = new IGOrderService();
            igorderlistService = new IGOrderlistService();
            ytmembersService = new YTMembersService();
            ytorderService = new YTOrderService();
            ytorderlistService = new YTOrderlistService();
            categoryproductService = new CategoryProductService();
            productService = new ProductService();
            categorymessageService = new CategoryMessageSevice();
            messageService = new MessageService();
        }

        // GET: OrderMs
        /*** FB訂單管理 查詢/新增/修改/刪除/完成名單/重做/收回/補充 ****/
        [CheckSession(IsAuth = true)]
        public ActionResult FBOrder(int p = 1)
        {
            var data = fborderService.Get().OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            /*** FB產品選單 ***/
            FBProductDropDownList();
            ViewBag.FBOrder = data.ToPagedList(pageNumber: p, pageSize: 20);
            ViewBag.FBOrder_total = fborderService.Get().OrderByDescending(o => o.Createdate);
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult FBOrder(Guid? Productid, int Status = 7, int p = 1)
        {
            var data = fborderService.Get();
            /*** 產品篩選 ****/
            if(Productid != null)
            {
                data = data.Where(a => a.Productid == Productid);
            }
            /**** 訂單狀態篩選 ****/
            switch (Status)
            {
                case 0:
                    data = data.Where(a => a.FBOrderStatus == 0);
                    break;
                case 1:
                    data = data.Where(a => a.FBOrderStatus == 1);
                    break;
                case 2:
                    data = data.Where(a => a.FBOrderStatus == 2);
                    break;
                case 3:
                    data = data.Where(a => a.FBOrderStatus == 3);
                    break;
                case 4:
                    data = data.Where(a => a.FBOrderStatus == 4);
                    break;
                case 5:
                    data = data.Where(a => a.FBOrderStatus == 5);
                    break;
                case 6:
                    data = data.Where(a => a.FBOrderStatus == 6);
                    break;
                default:
                    break;
            }
            ViewBag.pageNumber = p;
            data = data.OrderByDescending(o => o.Createdate);
            /*** FB產品選單 ***/
            FBProductDropDownList();
            ViewBag.FBOrder = data.ToPagedList(pageNumber: p, pageSize: 20);
            ViewBag.FBOrder_total = fborderService.Get().OrderByDescending(o => o.Createdate);
            return View();
        }

        [CheckSession(IsAuth = true)]
        public ActionResult AddFBOrder()
        {
            /**** FB產品選單 ****/
            FBProductDropDownList();
            /***** 留言類別選單 ***/
            CategoryMessageDropDownList();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult AddFBOrder(FBOrder fborder)
        {
            if (TryUpdateModel(fborder, new string[] { "Url", "Count", "Productid", "Categoryid", "Istest" }) && ModelState.IsValid)
            {
                fborder.FBOrderid = Guid.NewGuid();
                fborder.Createdate = dt_tw();
                fborder.Updatedate = dt_tw();
                fborder.Remains = fborder.Count;
                fborder.FBOrdernumber = "FBOrder" + dt_tw().ToString("yyyyMMddHHmmssfff");
                fborder.IsResourceGroup = true;
                fborderService.Create(fborder);
                fborderService.SaveChanges();
            }                 
            return RedirectToAction("FBOrder");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult DeleteFBOrder(Guid FBOrderid)
        {
            FBOrder fborder = fborderService.GetByID(FBOrderid);
            fborderService.Delete(fborder);
            fborderService.SaveChanges();
            return RedirectToAction("FBOrder");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult EditFBOrder(Guid FBOrderid, int p)
        {
            ViewBag.pageNumber = p;
            FBOrder fborder = fborderService.GetByID(FBOrderid);
            /**** FB產品選單 ****/
            FBProductDropDownList(fborder);
            /***** 留言類別選單 ***/
            CategoryMessageDropDownList(fborder);
            return View(fborder);
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult EditFBOrder(Guid FBOrderid)
        {
            FBOrder fborder = fborderService.GetByID(FBOrderid);

            if (TryUpdateModel(fborder, new string[] { "Url", "Count", "Productid", "FBOrderStatus", "Categoryid", "Istest"}) && ModelState.IsValid)
            {
                if(fborder.FBOrderStatus == 2 || fborder.FBOrderStatus == 3)
                {
                    /*** 將完成名單的會員Docker關閉 ****/
                    IEnumerable<FBMembers> FBMembers = fbmembersService.Get().Where(a => a.Isdocker == 1);
                    foreach (FBMembers FBMember in FBMembers)
                    {
                        if (FBMember.FBOrderlist.Where(a => a.FBOrderid == FBOrderid) != null)
                        {
                            FBMember.Isdocker = 0;
                            fbmembersService.SpecificUpdate(FBMember, new string[] { "Isdocker" });
                        }
                    }
                    fbmembersService.SaveChanges();
                }
                fborderService.Update(fborder);               
                fborderService.SaveChanges();
            }
            return RedirectToAction("FBOrder");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult FBOrderlist(Guid FBOrderid, int p = 1)
        {
            var data = fborderlistService.Get().Where(a => a.FBOrderid == FBOrderid).OrderByDescending(o => o.FBMembers.FB_Name);
            ViewBag.pageNumber = p;
            ViewBag.FBOrderid = FBOrderid;
            ViewBag.FBOrderlist = data.ToPagedList(pageNumber: p, pageSize: 20);
            return View();
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult FBOrderrework(Guid FBOrderid, int p)
        {
            FBOrder fborder = fborderService.GetByID(FBOrderid);
            fborder.FBOrderStatus = 0;  // 將訂單改為等待中
            fborder.Remains = fborder.Count; // 剩餘數量改為下單數量
            /*** 將完成名單的會員Docker關閉 ****/
            IEnumerable<FBMembers> FBMembers = fbmembersService.Get().Where(a => a.Isdocker == 1);
            foreach (FBMembers FBMember in FBMembers)
            {
                if (FBMember.FBOrderlist.Where(a => a.FBOrderid == FBOrderid) != null)
                {
                    FBMember.Isdocker = 0;
                    fbmembersService.SpecificUpdate(FBMember, new string[] { "Isdocker" });
                }
            }
            fbmembersService.SaveChanges();
            fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus", "Remains" });
            fborderService.SaveChanges();
            return RedirectToAction("FBOrder", new { p = p });
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult FBOrderregain(Guid FBOrderid, int p, Guid Productid)
        {
            FBOrder fborder = fborderService.GetByID(FBOrderid);
            fborder.FBOrderStatus = 4;  //將訂單改為等待收回
            fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
            fborderService.SaveChanges();
            return RedirectToAction("FBOrder", new { p = p });
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult FBOrdersupplement(Guid FBOrderid, int p)
        {
            FBOrder fborder = fborderService.GetByID(FBOrderid);
            fborder.FBOrderStatus = 0;  // 將訂單改為等待中
            fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
            fborderService.SaveChanges();
            return RedirectToAction("FBOrder", new { p = p });
        }
        /****** IG訂單 查詢/新增/刪除/修改/完成名單/重做/收回 *****/
        [CheckSession(IsAuth = true)]
        public ActionResult IGOrder(int p = 1)
        {
            var data = igorderService.Get().OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            /*** Instagram產品選單 ***/
            IGProductDropDownList();
            ViewBag.IGOrder = data.ToPagedList(pageNumber: p, pageSize: 20);
            ViewBag.IGOrder_total = igorderService.Get().OrderByDescending(o => o.Createdate);
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult IGOrder(Guid? Productid, int Status = 7, int p = 1)
        {
            var data = igorderService.Get();
            /**** 產品篩選 ****/
            if(Productid != null)
            {
                data = data.Where(a => a.Productid == Productid);
            }
            /**** 訂單狀態篩選 ****/
            switch (Status)
            {
                case 0:
                    data = data.Where(a => a.IGOrderStatus == 0);
                    break;
                case 1:
                    data = data.Where(a => a.IGOrderStatus == 1);
                    break;
                case 2:
                    data = data.Where(a => a.IGOrderStatus == 2);
                    break;
                case 3:
                    data = data.Where(a => a.IGOrderStatus == 3);
                    break;
                case 4:
                    data = data.Where(a => a.IGOrderStatus == 4);
                    break;
                case 5:
                    data = data.Where(a => a.IGOrderStatus == 5);
                    break;
                case 6:
                    data = data.Where(a => a.IGOrderStatus == 6);
                    break;
                default:                    
                    break;
            }
            data = data.OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            ViewBag.Status = Status;
            /*** Instagram產品選單 ***/
            IGProductDropDownList();
            ViewBag.IGOrder = data.ToPagedList(pageNumber: p, pageSize: 20);
            ViewBag.IGOrder_total = igorderService.Get().OrderByDescending(o => o.Createdate);
            return View();
        }

        [CheckSession(IsAuth = true)]
        public ActionResult AddIGOrder()
        {
            /**** Instagram產品選單 ***/
            IGProductDropDownList();
            /*** 留言類別選單 ****/
            CategoryMessageDropDownList();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult AddIGOrder(IGOrder igorder)
        {
            if (TryUpdateModel(igorder, new string[] { "Url", "Count", "Productid", "Categoryid", "Istest" }) && ModelState.IsValid)
            {
                igorder.IGOrderid = Guid.NewGuid();
                igorder.Createdate = dt_tw();
                igorder.Updatedate = dt_tw();
                igorder.Remains = igorder.Count;
                igorder.IGOrdernumber = "IGOrder" + dt_tw().ToString("yyyyMMddHHmmssfff");
                igorderService.Create(igorder);
                igorderService.SaveChanges();
            }
            return RedirectToAction("IGOrder");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult DeleteIGOrder(Guid IGOrderid)
        {
            IGOrder igorder = igorderService.GetByID(IGOrderid);
            igorderService.Delete(igorder);
            igorderService.SaveChanges();
            return RedirectToAction("IGOrder");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult EditIGOrder(int p , Guid IGOrderid)
        {
            ViewBag.pageNumber = p;
            IGOrder igorder = igorderService.GetByID(IGOrderid);
            /***** Instagram產品選單 *****/
            IGProductDropDownList(igorder);
            /**** 留言類別選單 *****/
            CategoryMessageDropDownList(igorder);
            return View(igorder);
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult EditIGOrder(Guid IGOrderid)
        {
            IGOrder igorder = igorderService.GetByID(IGOrderid);

            if (TryUpdateModel(igorder, new string[] { "Url", "Count", "Productid", "IGOrderStatus", "Categoryid", "Istest" }) && ModelState.IsValid)
            {
                /*** 訂單不是進行中，將完成名單的會員Docker關閉 ****/
                if (igorder.IGOrderStatus != 1)
                {
                    IEnumerable<IGMembers> IGMembers = igmembersService.Get();
                    IEnumerable<IGOrderlist> IGOrderlist = igorderlistService.Get().Where(a => a.IGOrderid == IGOrderid);
                    foreach (IGOrderlist list in IGOrderlist)
                    {
                        foreach (IGMembers IGMember in IGMembers)
                        {
                            if (IGMember.IGMemberid == list.IGMemberid)
                            {
                                IGMember.Isdocker = 0;  // 關閉Docker 【0 : 關閉, 1 : 開啟】
                                igmembersService.SpecificUpdate(IGMember, new string[] { "Isdocker" });
                            }
                        }
                    }
                    igmembersService.SaveChanges();
                }
                igorderService.Update(igorder);
                igorderService.SaveChanges();
            }
            return RedirectToAction("IGOrder");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult IGOrderlist(Guid IGOrderid, int p = 1)
        {
            var data = igorderlistService.Get().Where(a => a.IGOrderid == IGOrderid).OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            ViewBag.IGOrderlist = data.ToPagedList(pageNumber: p, pageSize: 20);
            return View();
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult IGOrderrework(Guid IGOrderid , int p)
        {
            IGOrder igorder = igorderService.GetByID(IGOrderid);
            igorder.IGOrderStatus = 0;          // 訂單狀態改為等待中
            /**** 更新完成列表裡的帳號的Docker狀態 ****/
            IEnumerable<IGMembers> IGMembers = igmembersService.Get();
            IEnumerable<IGOrderlist> IGOrderlist = igorderlistService.Get().Where(a => a.IGOrderid == IGOrderid);
            foreach (IGOrderlist list in IGOrderlist)
            {
                foreach (IGMembers IGMember in IGMembers)
                {
                    if (IGMember.IGMemberid == list.IGMemberid)
                    {
                        IGMember.Isdocker = 0;  // 關閉Docker 【0 : 關閉, 1 : 開啟】
                        igmembersService.SpecificUpdate(IGMember, new string[] { "Isdocker" });
                    }
                }
            }
            igmembersService.SaveChanges();

            igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
            igorderService.SaveChanges();
            return RedirectToAction("IGOrder", new { p = p });
        }

        [HttpGet]
        public ActionResult IGOrderregain(Guid IGOrderid, int p)
        {
            IGOrder igorder = igorderService.GetByID(IGOrderid);
            igorder.IGOrderStatus = 4;   //訂單狀態改為收回等待中

            igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
            igorderService.SaveChanges();
            return RedirectToAction("IGOrder", new { p = p });
        }
        /****** YT訂單 查詢/新增/刪除/修改/完成名單/重做/收回 *****/
        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult YTOrder(int p = 1)
        {
            var data = ytorderService.Get().OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            /**** Youtube產品選單 ****/
            YTProductDropDownList();
            ViewBag.YTOrder = data.ToPagedList(pageNumber: p, pageSize: 20);
            ViewBag.YTOrder_total = ytorderService.Get().OrderByDescending(o => o.Createdate);
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult YTOrder(Guid? Productid, int Status = 7, int p = 1)
        {
            var data = ytorderService.Get();
            /*** 產品篩選 ****/
            if(Productid != null)
            {
                data = data.Where(a => a.Productid == Productid);
            }
            /**** 訂單狀態篩選 ****/
            switch (Status)
            {
                case 0:
                    data = data.Where(a => a.YTOrderStatus == 0);
                    break;
                case 1:
                    data = data.Where(a => a.YTOrderStatus == 1);
                    break;
                case 2:
                    data = data.Where(a => a.YTOrderStatus == 2);
                    break;
                case 3:
                    data = data.Where(a => a.YTOrderStatus == 3);
                    break;
                case 4:
                    data = data.Where(a => a.YTOrderStatus == 4);
                    break;
                case 5:
                    data = data.Where(a => a.YTOrderStatus == 5);
                    break;
                case 6:
                    data = data.Where(a => a.YTOrderStatus == 6);
                    break;
                default:
                    break;                
            }
            ViewBag.pageNumber = p;
            ViewBag.Status = Status;
            data = data.OrderByDescending(o => o.Createdate);
            /**** Youtube產品選單 ****/
            YTProductDropDownList();
            ViewBag.YTOrder = data.ToPagedList(pageNumber: p, pageSize: 20);
            ViewBag.YTOrder_total = ytorderService.Get().OrderByDescending(o => o.Createdate);
            return View();
        }

        [CheckSession(IsAuth = true)]
        public ActionResult AddYTOrder()
        {
            /**** Youtube產品選單 ****/
            YTProductDropDownList();
            /**** 留言類別選單 ****/
            CategoryMessageDropDownList();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult AddYTOrder(YTOrder ytorder)
        {
            if (TryUpdateModel(ytorder, new string[] { "Url", "Count", "Productid", "Categoryid", "Istest" }) && ModelState.IsValid)
            {
                ytorder.YTOrderid = Guid.NewGuid();
                ytorder.Createdate = dt_tw();
                ytorder.Updatedate = dt_tw();
                ytorder.Remains = ytorder.Count;
                ytorder.YTOrdernumber = "YTOrder" + dt_tw().ToString("yyyyMMddHHmmssfff");
                ytorderService.Create(ytorder);
                ytorderService.SaveChanges();
            }
            return RedirectToAction("YTOrder");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult DeleteYTOrder(Guid YTOrderid)
        {
            YTOrder ytorder = ytorderService.GetByID(YTOrderid);
            ytorderService.Delete(ytorder);
            ytorderService.SaveChanges();
            return RedirectToAction("YTOrder");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult EditYTOrder(int p, Guid YTOrderid)
        {
            ViewBag.pageNumber = p;
            YTOrder ytorder = ytorderService.GetByID(YTOrderid);
            /**** Youtube產品選單 ****/
            YTProductDropDownList(ytorder);
            /**** 留言類別選單 ****/
            CategoryMessageDropDownList(ytorder);
            return View(ytorder);
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult EditYTOrder(Guid YTOrderid)
        {
            YTOrder ytorder = ytorderService.GetByID(YTOrderid);
            if (TryUpdateModel(ytorder, new string[] { "Url", "Count", "Productid", "YTOrderStatus", "Categoryid", "Istest" }) && ModelState.IsValid)
            {
                /*** 訂單不是進行中，將完成名單的會員Docker關閉 ****/
                if (ytorder.YTOrderStatus != 1)
                {
                    IEnumerable<YTMembers> YTMembers = ytmembersService.Get();
                    IEnumerable<YTOrderlist> YTOrderlist = ytorderlistService.Get().Where(a => a.YTOrderid == YTOrderid);
                    foreach (YTOrderlist list in YTOrderlist)
                    {
                        foreach (YTMembers YTMember in YTMembers)
                        {
                            if (YTMember.YTMemberid == list.YTMemberid)
                            {
                                YTMember.Isdocker = 0;  // 關閉Docker 【0 : 關閉, 1 : 開啟】
                                ytmembersService.SpecificUpdate(YTMember, new string[] { "Isdocker" });
                            }
                        }
                    }
                    ytmembersService.SaveChanges();
                }
                ytorderService.Update(ytorder);
                ytorderService.SaveChanges();
            }
            return RedirectToAction("YTOrder");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult YTOrderlist(Guid YTOrderid, int p = 1)
        {
            var data = ytorderlistService.Get().Where(a => a.YTOrderid == YTOrderid).OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            ViewBag.YTOrderlist = data.ToPagedList(pageNumber: p, pageSize: 20);
            return View();
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult YTOrderrework(Guid YTOrderid, int p)
        {
            YTOrder ytorder = ytorderService.GetByID(YTOrderid);
            ytorder.YTOrderStatus = 0;           // 訂單狀態改為等待中
            /**** 更新完成列表裡的帳號的Docker狀態 ****/
            IEnumerable<YTMembers> YTMembers = ytmembersService.Get();
            IEnumerable<YTOrderlist> YTOrderlist = ytorderlistService.Get().Where(a => a.YTOrderid == YTOrderid);
            foreach (YTOrderlist list in YTOrderlist)
            {
                foreach (YTMembers YTMember in YTMembers)
                {
                    if (YTMember.YTMemberid == list.YTMemberid)
                    {
                        YTMember.Isdocker = 0;  // 關閉Docker 【0 : 關閉, 1 : 開啟】
                        ytmembersService.SpecificUpdate(YTMember, new string[] { "Isdocker" });
                    }
                }
            }
            ytmembersService.SaveChanges();

            ytorderService.SpecificUpdate(ytorder, new string[] {"YTOrderStatus" });
            ytorderService.SaveChanges();
            return RedirectToAction("YTOrder", new { p = p });
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult YTOrderregain(Guid YTOrderid, int p)
        {
            YTOrder ytorder = ytorderService.GetByID(YTOrderid);
            ytorder.YTOrderStatus = 4;           // 訂單狀態改為等待收回中
            ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
            ytorderService.SaveChanges();
            return RedirectToAction("YTOrder", new { p = p });
        }

        #region --留言類別選單--
        private void CategoryMessageDropDownList(Object selectMember = null)
        {
            var querys = categorymessageService.Get();

            ViewBag.Categoryid = new SelectList(querys, "Categoryid", "Categoryname", selectMember);
        }
        #endregion
        #region --Facebook產品選單--
        private void FBProductDropDownList(Object selectMember = null)
        {
            Guid FBCategory = categoryproductService.Get().Where(a => a.Categoryname == "Facebook").FirstOrDefault().Categoryid;
            var querys = productService.Get().Where(a => a.Categoryid == FBCategory);

            ViewBag.Productid = new SelectList(querys, "Productid", "Productname", selectMember);
        }
        #endregion

        #region --Instagram產品選單--
        private void IGProductDropDownList(Object selectMember = null)
        {
            Guid IGCategory = categoryproductService.Get().Where(a => a.Categoryname == "Instagram").FirstOrDefault().Categoryid;
            var querys = productService.Get().Where(a => a.Categoryid == IGCategory);

            ViewBag.Productid = new SelectList(querys, "Productid", "Productname", selectMember);
        }
        #endregion

        #region --Youtube產品選單--
        private void YTProductDropDownList(Object selectMember = null)
        {
            Guid YTCategory = categoryproductService.Get().Where(a => a.Categoryname == "Youtube").FirstOrDefault().Categoryid;
            var querys = productService.Get().Where(a => a.Categoryid == YTCategory);

            ViewBag.Productid = new SelectList(querys, "Productid", "Productname", selectMember);
        }
        #endregion
        #region --類別產品選單--
        //private void CategoryProductDropDownList(Object selectMember = null)
        //{
        //    var querys = categoryproductService.Get();

        //    ViewBag.Categoryid = new SelectList(querys, "Categoryid", "Categoryname", selectMember);
        //}
        #endregion
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