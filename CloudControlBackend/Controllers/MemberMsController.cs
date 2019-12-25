using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using CloudControl.Model;
using CloudControl.Service;
using CloudControlBackend;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.Entity;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace CloudControlBackend.Controllers
{
    public class MemberMsController : BaseController
    {
        private CloudControlEntities db;
        private FBMembersService fbmembersService;
        private FBMembersLoginlogService fbmembersloginlogService;
        private IGMembersService igmembersService;
        private IGMembersLoginlogService igmembersloginlogService;
        private YTMembersService ytmembersService;
        private YTMembersLoginlogService ytmembersloginlogService;
        private UseragentService useragentService;
        private ProductService productService;

        public MemberMsController()
        {
            db = new CloudControlEntities();
            fbmembersService = new FBMembersService();
            fbmembersloginlogService = new FBMembersLoginlogService();
            igmembersService = new IGMembersService();
            igmembersloginlogService = new IGMembersLoginlogService();           
            ytmembersService = new YTMembersService();
            ytmembersloginlogService = new YTMembersLoginlogService();
            useragentService = new UseragentService();
            productService = new ProductService();
        }
        // GET: MemberMs
        /**** FB會員 查詢/新增/刪除/修改/匯入帳號/轉正式 *****/
        [CheckSession(IsAuth = true)]
        public ActionResult FBMembers(int p = 1)
        {
            //var data = fbmembersService.GetNoDel().OrderByDescending(o => o.Createdate);
            var data = fbmembersService.Get().OrderByDescending(o => o.FBOrderlist.Count());
            ViewBag.pageNumber = p;
            ViewBag.FBMembers = data.ToPagedList(pageNumber: p, pageSize: 100);
            //ViewBag.FBMembers = data;

            //ViewBag.Check = db.FBMembers.Include(x => x.FBMembersLoginlog.OrderByDescending(o => o.Createdate).FirstOrDefault().Status == 1).Count();
            ViewBag.Check = fbmembersService.Get().Include(a => a.FBMembersLoginlog).Where(a => a.FBMembersLoginlog.FirstOrDefault().Status == 1).Count();
            ViewBag.Times = fbmembersService.Get().Include(a => a.FBMembersLoginlog).Where(a => a.FBMembersLoginlog.FirstOrDefault().Status == 2).Count();
            /**** FB 產品選單 ***/
            FBProductDropDownList();
            /**** 預備人選 ****/
            ViewBag.ReservedNumber = fbmembersService.GetNoDel().Where(a => a.Isenable == 2).Count();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult FBMembers(string Account, Guid? Productid, int p = 1, int Isenable = 3, int Status = 3)
        {
            var data = fbmembersService.GetNoDel();
            /**** 帳號篩選 ***/
            if(Account != null)
            {
                data = data.Where(a => a.FB_Account.Contains(Account));
            }
            /**** 產品篩選 ****/
            if(Productid != null)
            {
                data = data.Where(a => a.Productid == Productid);
            }
            /**** 帳號型態篩選 ****/
            if(Isenable != 3)
            {
                data = data.Where(a => a.Isenable == Isenable);
            }
            /**** 驗證狀態 ****/
            if(Status != 3)
            {
                data = data.Where(a => a.FBMembersLoginlog.FirstOrDefault().Status == Status);
            }
            data = data.OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            ViewBag.FBMembers = data.ToPagedList(pageNumber: p, pageSize: 100);
            ViewBag.Account = Account;
            ViewBag.Check = fbmembersService.Get().Include(a => a.FBMembersLoginlog).Where(a => a.FBMembersLoginlog.FirstOrDefault().Status == 1).Count();

            
            ViewBag.Times = fbmembersService.Get().Include(a => a.FBMembersLoginlog).Where(a => a.FBMembersLoginlog.FirstOrDefault().Status == 2).Count();
            /**** FB 產品選單 ***/
            FBProductDropDownList();
            /**** 預備人選 ****/
            ViewBag.ReservedNumber = fbmembersService.GetNoDel().Where(a => a.Isenable == 2).Count();
            return View();
        }

        [CheckSession(IsAuth = true)]
        public ActionResult AddFBMembers()
        {
            /**** FB 產品選單 ***/
            FBProductDropDownList();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult AddFBMembers(FBMembers fbmember)
        {
            if (TryUpdateModel(fbmember, new string[] { "FB_Account", "FB_Password", "Facebooklink", "FB_Name", "Isenable", "Productid" }) && ModelState.IsValid)
            {
                fbmember.FBMemberid = Guid.NewGuid();
                fbmember.FB_Account = Regex.Replace(fbmember.FB_Account, @"[^a-z||A-Z||@||.||0-9]", "");         // 保留A-Z、a-z、0-9、小老鼠、小數點，其餘取代空值
                fbmember.Createdate = dt_tw();
                fbmember.Updatedate = dt_tw();
                fbmember.Lastdate = ((int)(dt_tw() - new DateTime(1970, 1, 1)).TotalSeconds) - 28800;      // 總秒數
                fbmember.Isenable = 1;
                /*** 隨機抓取Useragent ***/
                int useragentCount = useragentService.Get().Count();
                Useragent[] useragent = useragentService.Get().ToArray();
                Random crand = new Random();
                int rand = crand.Next(0, useragentCount - 1);
                fbmember.Useragent = useragent[rand].User_agent;
                /**** 將會員寫進會員登入紀錄裡，預設狀態為0 【0 : 未驗證 , 1 : 已驗證 , 2 : 需驗證】 ****/
                FBMembersLoginlog fbmembersloginlog = new FBMembersLoginlog();
                fbmembersloginlog.FBMemberid = fbmember.FBMemberid;
                fbmembersloginlog.Createdate = fbmember.Createdate;
                fbmembersloginlog.Status = 0;
                fbmember.FBMembersLoginlog.Add(fbmembersloginlog);
                /**** End Memberloginrecord ****/
                fbmembersService.Create(fbmember);
                fbmembersService.SaveChanges();
            }

            return RedirectToAction("FBMembers");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult DeleteFBMembers(Guid FBMemberid, int p)
        {
            FBMembers fbmember = fbmembersService.GetByID(FBMemberid);
            fbmember.Isenable = 0;
            fbmembersService.SpecificUpdate(fbmember, new string[] { "Isenable" });
            fbmembersService.SaveChanges();
            return RedirectToAction("FBMembers");
        }
        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult EditFBMembers(int p , Guid FBMemberid)
        {
            ViewBag.pageNumber = p;
            FBMembers fbmember = fbmembersService.GetByID(FBMemberid);
            /**** FB 產品選單 ***/
            FBProductDropDownList(fbmember);
            return View(fbmember);
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult EditFBMembers(Guid FBMemberid, int Status)
        {
            FBMembersLoginlog fbmembersloginlog = new FBMembersLoginlog();
            FBMembers fbmember = fbmembersService.GetByID(FBMemberid);
            if (TryUpdateModel(fbmember, new string[] { "FB_Account", "FB_Password", "Facebooklink", "FB_Name", "Productid"}) && ModelState.IsValid)
            {
                fbmember.FB_Account = Regex.Replace(fbmember.FB_Account, @"[^a-z||A-Z||@||.||0-9]", "");         // 保留A-Z、a-z、0-9、小老鼠、小數點，其餘取代空值
                fbmembersService.Update(fbmember);                
                fbmembersService.SaveChanges();

                fbmembersloginlog.Status = Status;
                fbmembersloginlog.FBMemberid = FBMemberid;
                fbmembersloginlog.Createdate = DateTime.Now;
                fbmembersloginlogService.Create(fbmembersloginlog);
                fbmembersloginlogService.SaveChanges();
            }

            return RedirectToAction("FBMembers");
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult UploadFBMembers(HttpPostedFileBase file)
        {
            if (file != null)
            {
                var filename = Path.GetFileName(file.FileName);
                var filetype = Path.GetExtension(file.FileName).ToLower();
                string newfileName = dt_tw().ToString("yyyyMMddHHmmssff") + filetype;
                var path = Path.Combine(Server.MapPath("~/FileUpload/FB"), newfileName);
                file.SaveAs(path);
                import_FBMemberstoSQL(path);
            }
            return RedirectToAction("FBMembers");
        }

        public void import_FBMemberstoSQL(string path)
        {
            IWorkbook workBook;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                workBook = new XSSFWorkbook(fs);
            }

            var sheet = workBook.GetSheet("工作表1");
            for (var i = 1; i <= sheet.LastRowNum; i++)
            {
                var j = sheet.LastRowNum;
                if (sheet.GetRow(i) != null)
                {                    
                    FBMembers fbmember = new FBMembers();
                    if (sheet.GetRow(i).GetCell(0).ToString() != "" && sheet.GetRow(i).GetCell(0).ToString() != null)
                    {

                        var test = sheet.GetRow(i).GetCell(3).ToString();
                        fbmember.FB_Account = Regex.Replace(sheet.GetRow(i).GetCell(0).ToString(), @"[^a-z||A-Z||@||.||0-9]", "");         // 保留A-Z、a-z、0-9、小老鼠、小數點，其餘取代空值
                        fbmember.FB_Password = sheet.GetRow(i).GetCell(1).ToString();
                        fbmember.FB_Name = sheet.GetRow(i).GetCell(2).ToString();
                        fbmember.Facebooklink = sheet.GetRow(i).GetCell(3).ToString();
                        fbmember.Cookie = sheet.GetRow(i).GetCell(4).ToString();
                        switch (sheet.GetRow(i).GetCell(5).ToString())
                        {
                            case "1":
                                fbmember.Productid = Guid.Parse("0c020482-d76a-4213-b021-f8db0fe96489");
                                break;
                            case "2":
                                fbmember.Productid = Guid.Parse("6c5425d0-5362-4fa9-8c6e-dfb929877b93");
                                break;
                            case "3":
                                fbmember.Productid = Guid.Parse("f9dd03ee-ecd7-4514-94c6-2ea7d72d931c");
                                break;
                            case "4":
                                fbmember.Productid = Guid.Parse("6b1e8bd2-8dbb-4282-89df-b509be5ff361");
                                break;
                            case "5":
                                fbmember.Productid = Guid.Parse("e592d2a8-a1a7-4471-a648-0547c7a46cdd");
                                break;
                            case "6":
                                fbmember.Productid = Guid.Parse("bffb9389-46f0-4bf4-a6ab-d4dcf77435c7");
                                break;
                            case "7":
                                fbmember.Productid = Guid.Parse("f686d184-884c-4aa7-9f26-f8118ba7f990");
                                break;
                            case "8":
                                fbmember.Productid = Guid.Parse("07408390-5f81-451a-9193-a93faaed1825");
                                break;
                            case "9":
                                fbmember.Productid = Guid.Parse("b93e5ee4-f946-4bb0-ad6a-8f379e704802");
                                break;
                            case "10":
                                fbmember.Productid = Guid.Parse("e16dfe59-2789-4598-9310-6334e5e7803c");
                                break;
                            default:
                                fbmember.Productid = Guid.Parse("78da7b19-f424-4efa-9691-45268100188d");
                                break;
                        }

                        if(sheet.GetRow(i).GetCell(6).ToString() == "1")
                        {
                            fbmember.Isenable = 1;
                        }
                        else if(sheet.GetRow(i).GetCell(6).ToString() == "2")
                        {
                            fbmember.Isenable = 2;
                        }

                        fbmember.FBMemberid = Guid.NewGuid();
                        fbmember.Createdate = dt_tw();
                        fbmember.Updatedate = dt_tw();
                        fbmember.Lastdate = ((int)(dt_tw() - new DateTime(1970, 1, 1)).TotalSeconds) - 28800;
                        /*** 隨機指派手機版Useragent ***/
                        int useragent_phone = useragentService.Get().Count();
                        Useragent[] useragent = useragentService.Get().ToArray();
                        Random rnd = new Random();
                        int rnd_useragent = rnd.Next(1, useragent_phone-1);
                        /*** End Useragent ***/
                        fbmember.Useragent = useragent[rnd_useragent].User_agent;
                        /*** 將帳號寫入登入紀錄裡，預設狀態為0 【0 : 未驗證 , 1 : 已驗證 , 2 : 需驗證】 ***/
                        FBMembersLoginlog fbmembersloginlog = new FBMembersLoginlog();
                        fbmembersloginlog.FBMemberid = fbmember.FBMemberid;
                        fbmembersloginlog.Createdate = dt_tw();
                        fbmembersloginlog.Status = 0;
                        fbmember.FBMembersLoginlog.Add(fbmembersloginlog);
                        /*** End LoginLog ***/
                        fbmembersService.Create(fbmember);
                    }
                }
            }
            fbmembersService.SaveChanges();
        }

        /**** IG會員 查詢/新增/刪除/修改/匯入帳號 *****/
        [CheckSession(IsAuth = true)]
        public ActionResult IGMembers(int p = 1)
        {
            var data = igmembersService.Get().OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            ViewBag.IGMembers = data.ToPagedList(pageNumber: p, pageSize: 20);
            ViewBag.Check = igmembersService.Get().Where(a => a.IGMembersLoginlog.OrderByDescending(o => o.Createdate).FirstOrDefault().Status == 1).Count();
            ViewBag.Times = igmembersService.Get().Where(a => a.IGMembersLoginlog.OrderByDescending(o => o.Createdate).FirstOrDefault().Status == 2).Count();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult IGMembers(string Account, int p = 1)
        {
            if (Account != null)
            {
                var data = igmembersService.Get().Where(a => a.IG_Account.Contains(Account)).OrderByDescending(o => o.Createdate);
                ViewBag.pageNumber = p;
                ViewBag.IGMembers = data.ToPagedList(pageNumber: p, pageSize: 20);
                ViewBag.Account = Account;
            }
            else
            {
                var data = igmembersService.Get().OrderByDescending(o => o.Createdate);
                ViewBag.pageNumber = p;
                ViewBag.IGMembers = data.ToPagedList(pageNumber: p, pageSize: 20);
            }
            ViewBag.Check = igmembersService.Get().Where(a => a.IGMembersLoginlog.OrderByDescending(o => o.Createdate).FirstOrDefault().Status == 1).Count();
            ViewBag.Times = igmembersService.Get().Where(a => a.IGMembersLoginlog.OrderByDescending(o => o.Createdate).FirstOrDefault().Status == 2).Count();
            return View();
        }

        [CheckSession(IsAuth = true)]
        public ActionResult AddIGMembers()
        {
            Guid IGProductid = Guid.Parse("6dc4c05e-f230-4d5b-ba0f-1a8a5aaf39c0");
            //var rows = productService.Get().Where(a => a.Categoryid == IGProductid).ToList();
            /**** IG 產品選單 ***/
            IGProductDropDownList();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult AddIGMembers(IGMembers igmember)
        {
            if (TryUpdateModel(igmember, new string[] { "IG_Account", "IG_Password", "Instagramlink", "IG_Name", "Isenable", "Productid" }) && ModelState.IsValid)
            {
                igmember.IGMemberid = Guid.NewGuid();
                igmember.IG_Account = Regex.Replace(igmember.IG_Account, @"[^a-z||A-Z||@||.||0-9]", "");         // 保留A-Z、a-z、0-9、小老鼠、小數點，其餘取代空值
                igmember.Createdate = dt_tw();
                igmember.Updatedate = dt_tw();
                igmember.Lastdate = ((int)(dt_tw() - new DateTime(1970, 1, 1)).TotalSeconds) - 28800;      // 總秒數
                igmember.Isenable = 1;
                /*** 隨機抓取Useragent ***/
                int useragentCount = useragentService.Get().Count();
                Useragent[] useragent = useragentService.Get().ToArray();
                Random crand = new Random();
                int rand = crand.Next(0, useragentCount - 1);
                igmember.Useragent = useragent[rand].User_agent;
                /**** 將會員寫進會員登入紀錄裡，預設狀態為0 【0 : 未驗證 , 1 : 已驗證 , 2 : 需驗證】 ****/
                IGMembersLoginlog igmemberloginlog = new IGMembersLoginlog();
                igmemberloginlog.IGMemberid = igmember.IGMemberid;
                igmemberloginlog.Createdate = igmember.Createdate;
                igmemberloginlog.Status = 0;
                igmember.IGMembersLoginlog.Add(igmemberloginlog);
                /**** End Memberloginrecord ****/
                igmembersService.Create(igmember);
                igmembersService.SaveChanges();
            }

            return RedirectToAction("IGMembers");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult DeleteIGMembers(Guid IGMemberid, int p)
        {
            IGMembers igmember = igmembersService.GetByID(IGMemberid);
            igmember.Isenable = 0;
            igmembersService.SpecificUpdate(igmember, new string[] { "Isenable" });
            igmembersService.SaveChanges();
            return RedirectToAction("IGMembers");
        }
        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult EditIGMembers(int p, Guid IGMemberid)
        {
            ViewBag.pageNumber = p;
            IGMembers igmember = igmembersService.GetByID(IGMemberid);
            /**** IG 產品選單 ***/
            IGProductDropDownList(igmember);
            return View(igmember);
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult EditIGMembers(Guid IGMemberid, int Status)
        {
            IGMembersLoginlog igmembersloginlog = new IGMembersLoginlog();
            IGMembers igmember = igmembersService.GetByID(IGMemberid);
            if (TryUpdateModel(igmember, new string[] { "IG_Account", "IG_Password", "Instagramlink", "IG_Name", "Productid" }) && ModelState.IsValid)
            {
                igmember.IG_Account = Regex.Replace(igmember.IG_Account, @"[^a-z||A-Z||@||.||0-9]", "");         // 保留A-Z、a-z、0-9、小老鼠、小數點，其餘取代空值
                igmembersService.Update(igmember);
                igmembersService.SaveChanges();

                igmembersloginlog.IGMemberid = IGMemberid;
                igmembersloginlog.Status = Status;
                igmembersloginlog.Createdate = DateTime.Now;
                igmembersloginlogService.Create(igmembersloginlog);
                igmembersloginlogService.SaveChanges();
            }

            return RedirectToAction("IGMembers");
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult UploadIGMembers(HttpPostedFileBase file)
        {
            if (file != null)
            {
                var filename = Path.GetFileName(file.FileName);
                var filetype = Path.GetExtension(file.FileName).ToLower();
                string newfileName = dt_tw().ToString("yyyyMMddHHmmssff") + filetype;
                var path = Path.Combine(Server.MapPath("~/FileUpload/IG"), newfileName);
                file.SaveAs(path);
                import_IGMemberstoSQL(path);
            }
            return RedirectToAction("IGMembers");
        }

        public void import_IGMemberstoSQL(string path)
        {
            IWorkbook workBook;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                workBook = new XSSFWorkbook(fs);
            }

            var sheet = workBook.GetSheet("工作表1");
            for (var i = 1; i <= sheet.LastRowNum; i++)
            {
                var j = sheet.LastRowNum;
                if (sheet.GetRow(i) != null)
                {
                    IGMembers igmember = new IGMembers();
                    try
                    //if (sheet.GetRow(i).GetCell(0).ToString() != "" && sheet.GetRow(i).GetCell(0).ToString() != null)
                    {
                        try
                        {
                            if (sheet.GetRow(i).GetCell(0).ToString() != "" && sheet.GetRow(i).GetCell(0).ToString() != null)
                            {
                                igmember.IG_Account = Regex.Replace(sheet.GetRow(i).GetCell(0).ToString(), @"[^a-z||A-Z||@||.||0-9]", "");         // 保留A-Z、a-z、0-9、小老鼠、小數點，其餘取代空值
                            }
                        }
                        catch { }
                        try
                        {
                            if (sheet.GetRow(i).GetCell(1).ToString() != "" && sheet.GetRow(i).GetCell(1).ToString() != null)
                            {
                                igmember.IG_Password = sheet.GetRow(i).GetCell(1).ToString();
                            }
                        }
                        catch { }                        
                        try
                        {
                            if (sheet.GetRow(i).GetCell(2).ToString() != "" && sheet.GetRow(i).GetCell(2).ToString() != null)
                            {
                                igmember.IG_Name = sheet.GetRow(i).GetCell(2).ToString();
                            }
                        }
                        catch { } 
                        try
                        {
                            if (sheet.GetRow(i).GetCell(3).ToString() != "" && sheet.GetRow(i).GetCell(3).ToString() != null)
                            {
                                igmember.Instagramlink = sheet.GetRow(i).GetCell(3).ToString();
                            }
                        }
                        catch { }
                        try
                        {
                            if (sheet.GetRow(i).GetCell(4).ToString() != "" && sheet.GetRow(i).GetCell(4).ToString() != null)
                            {
                                igmember.Cookie = sheet.GetRow(i).GetCell(4).ToString();
                            }
                        }
                        catch { }
                        
                        igmember.IGMemberid = Guid.NewGuid();
                        igmember.Createdate = dt_tw();
                        igmember.Updatedate = dt_tw();
                        igmember.Isenable = 1;
                        igmember.Lastdate = ((int)(dt_tw() - new DateTime(1970, 1, 1)).TotalSeconds) - 28800;
                        /*** 隨機指派手機版Useragent ***/
                        int useragent_phone = useragentService.Get().Count();
                        Useragent[] useragent = useragentService.Get().ToArray();
                        Random rnd = new Random();
                        int rnd_useragent = rnd.Next(1, useragent_phone - 1);
                        /*** End Useragent ***/
                        igmember.Useragent = useragent[rnd_useragent].User_agent;

                        /**** 將會員寫進會員登入紀錄裡，預設狀態為0 【0 : 未驗證 , 1 : 已驗證 , 2 : 需驗證】 ****/
                        IGMembersLoginlog igmembersloginlog = new IGMembersLoginlog();
                        igmembersloginlog.IGMemberid = igmember.IGMemberid;
                        igmembersloginlog.Createdate = dt_tw();
                        igmembersloginlog.Status = 0;
                        igmember.IGMembersLoginlog.Add(igmembersloginlog);
                        /**** End LoginLog ***/
                        igmembersService.Create(igmember);
                    }
                    catch { }
                }
            }
            igmembersService.SaveChanges();
        }
        /**** YT會員 查詢/新增/刪除/修改/匯入帳號 ****/
        [CheckSession(IsAuth = true)]
        public ActionResult YTMembers(int p = 1)
        {
            var data = ytmembersService.Get().OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            ViewBag.YTMembers = data.ToPagedList(pageNumber: p, pageSize: 20);
            ViewBag.Check = ytmembersService.Get().Where(a => a.YTMembersLoginlog.OrderByDescending(o => o.Createdate).FirstOrDefault().Status == 1).Count();
            ViewBag.Times = ytmembersService.Get().Where(a => a.YTMembersLoginlog.OrderByDescending(o => o.Createdate).FirstOrDefault().Status == 2).Count();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult YTMembers(string Account, int p = 1)
        {
            if (Account != null)
            {
                var data = ytmembersService.Get().Where(a => a.YT_Account.Contains(Account)).OrderByDescending(o => o.Createdate);
                ViewBag.pageNumber = p;
                ViewBag.YTMembers = data.ToPagedList(pageNumber: p, pageSize: 20);
                ViewBag.Account = Account;
            }
            else
            {
                var data = ytmembersService.Get().OrderByDescending(o => o.Createdate);
                ViewBag.pageNumber = p;
                ViewBag.YTMembers = data.ToPagedList(pageNumber: p, pageSize: 20);
            }
            ViewBag.Check = ytmembersService.Get().Where(a => a.YTMembersLoginlog.OrderByDescending(o => o.Createdate).FirstOrDefault().Status == 1).Count();
            ViewBag.Times = ytmembersService.Get().Where(a => a.YTMembersLoginlog.OrderByDescending(o => o.Createdate).FirstOrDefault().Status == 2).Count();
            return View();
        }

        [CheckSession(IsAuth = true)]
        public ActionResult AddYTMembers()
        {
            /**** YT 產品選單 ***/
            YTProductDropDownList();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult AddYTMembers(YTMembers ytmember)
        {
            if (TryUpdateModel(ytmember, new string[] { "YT_Account", "YT_Password", "Youtubelink", "YT_Name", "Isenable", "Productid" }) && ModelState.IsValid)
            {
                ytmember.YTMemberid = Guid.NewGuid();
                ytmember.YT_Account = Regex.Replace(ytmember.YT_Account, @"[^a-z||A-Z||@||.||0-9]", "");         // 保留A-Z、a-z、0-9、小老鼠、小數點，其餘取代空值
                ytmember.Createdate = dt_tw();
                ytmember.Updatedate = dt_tw();
                ytmember.Lastdate = ((int)(dt_tw() - new DateTime(1970, 1, 1)).TotalSeconds) - 28800;      // 總秒數                
                ytmember.Isenable = 1;
                /*** 隨機抓取Useragent ***/
                int useragentCount = useragentService.Get().Count();
                Useragent[] useragent = useragentService.Get().ToArray();
                Random crand = new Random();
                int rand = crand.Next(0, useragentCount - 1);
                ytmember.Useragent = useragent[rand].User_agent;
                /**** 將會員寫進會員登入紀錄裡，預設狀態為0 【0 : 未驗證 , 1 : 已驗證 , 2 : 需驗證】 ****/
                YTMembersLoginlog ytmembersloginlog = new YTMembersLoginlog();
                ytmembersloginlog.YTMemberid = ytmember.YTMemberid;
                ytmembersloginlog.Createdate = dt_tw();
                ytmembersloginlog.Status = 0;
                ytmember.YTMembersLoginlog.Add(ytmembersloginlog);
                /**** End Memberloginrecord ****/
                ytmembersService.Create(ytmember);
                ytmembersService.SaveChanges();
            }

            return RedirectToAction("YTMembers");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult DeleteYTMembers(Guid YTMemberid, int p)
        {
            YTMembers ytmember = ytmembersService.GetByID(YTMemberid);
            ytmember.Isenable = 0;
            ytmembersService.SpecificUpdate(ytmember, new string[] { "Isenable" });
            ytmembersService.SaveChanges();
            return RedirectToAction("YTMembers");
        }
        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult EditYTMembers(int p, Guid YTMemberid)
        {
            ViewBag.pageNumber = p;
            YTMembers ytmember = ytmembersService.GetByID(YTMemberid);
            /**** YT 產品選單 ***/
            YTProductDropDownList(ytmember);
            return View(ytmember);
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult EditYTMembers(Guid YTMemberid, int Status)
        {
            YTMembersLoginlog ytmembersloginlog = new YTMembersLoginlog();
            YTMembers ytmember = ytmembersService.GetByID(YTMemberid);
            if (TryUpdateModel(ytmember, new string[] { "YT_Account", "YT_Password", "Youtubelink", "YT_Name", "Productid" }) && ModelState.IsValid)
            {
                ytmember.YT_Account = Regex.Replace(ytmember.YT_Account, @"[^a-z||A-Z||@||.||0-9]", "");         // 保留A-Z、a-z、0-9、小老鼠、小數點，其餘取代空值
                ytmembersService.Update(ytmember);
                ytmembersService.SaveChanges();

                ytmembersloginlog.YTMemberid = YTMemberid;
                ytmembersloginlog.Status = Status;
                ytmembersloginlog.Createdate = DateTime.Now;
                ytmembersloginlogService.Create(ytmembersloginlog);
                ytmembersloginlogService.SaveChanges();
            }
            return RedirectToAction("YTMembers");
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult UploadYTMembers(HttpPostedFileBase file)
        {
            if (file != null)
            {
                var filename = Path.GetFileName(file.FileName);
                var filetype = Path.GetExtension(file.FileName).ToLower();
                string newfileName = dt_tw().ToString("yyyyMMddHHmmssff") + filetype;
                var path = Path.Combine(Server.MapPath("~/FileUpload/YT"), newfileName);
                file.SaveAs(path);
                import_YTMemberstoSQL(path);
            }
            return RedirectToAction("YTMembers");
        }

        public void import_YTMemberstoSQL(string path)
        {
            IWorkbook workBook;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                workBook = new XSSFWorkbook(fs);
            }

            var sheet = workBook.GetSheet("工作表1");
            for (var i = 1; i <= sheet.LastRowNum; i++)
            {
                var j = sheet.LastRowNum;
                if (sheet.GetRow(i) != null)
                {
                    YTMembers ytmember = new YTMembers();
                    if (sheet.GetRow(i).GetCell(0).ToString() != "" && sheet.GetRow(i).GetCell(0).ToString() != null)
                    {
                        var test = sheet.GetRow(i).GetCell(3).ToString();
                        ytmember.YT_Account = Regex.Replace(sheet.GetRow(i).GetCell(0).ToString(), @"[^a-z||A-Z||@||.||0-9]", "");         // 保留A-Z、a-z、0-9、小老鼠、小數點，其餘取代空值
                        ytmember.YT_Password = sheet.GetRow(i).GetCell(1).ToString();
                        ytmember.YT_Name = sheet.GetRow(i).GetCell(2).ToString();
                        ytmember.Youtubelink = sheet.GetRow(i).GetCell(3).ToString();
                        ytmember.Cookie = sheet.GetRow(i).GetCell(4).ToString();
                        ytmember.YTMemberid = Guid.NewGuid();
                        ytmember.Createdate = dt_tw();
                        ytmember.Updatedate = dt_tw();
                        ytmember.Isenable = 1;
                        ytmember.Lastdate = ((int)(dt_tw() - new DateTime(1970, 1, 1)).TotalSeconds) - 28800;
                        /*** 隨機指派手機版Useragent ***/
                        int useragent_phone = useragentService.Get().Count();
                        Useragent[] useragent = useragentService.Get().ToArray();
                        Random rnd = new Random();
                        int rnd_useragent = rnd.Next(1, useragent_phone - 1);
                        /*** End Useragent ***/
                        ytmember.Useragent = useragent[rnd_useragent].User_agent;
                        /**** 將會員寫進會員登入紀錄裡，預設狀態為0 【0 : 未驗證 , 1 : 已驗證 , 2 : 需驗證】 ****/
                        YTMembersLoginlog ytmembersloginlog = new YTMembersLoginlog();
                        ytmembersloginlog.YTMemberid = ytmember.YTMemberid;
                        ytmembersloginlog.Createdate = dt_tw();
                        ytmembersloginlog.Status = 0;
                        ytmember.YTMembersLoginlog.Add(ytmembersloginlog);
                        /**** End LoginLog ****/
                        ytmembersService.Create(ytmember);
                    }
                }
            }
            ytmembersService.SaveChanges();
        }

        #region --FB產品類別選單--
        private void FBProductDropDownList(Object selectMember = null)
        {
            Guid FBCategoryid = Guid.Parse("9f268158-09b1-4176-9088-a4a4af63d389");
            var querys = productService.Get().Where(a => a.Categoryid == FBCategoryid).ToList();

            ViewBag.Productid = new SelectList(querys, "Productid", "Productname", selectMember);
        }
        #endregion

        #region --IG產品類別選單--
        private void IGProductDropDownList(Object selectMember = null)
        {
            Guid IGCategroyid = Guid.Parse("6dc4c05e-f230-4d5b-ba0f-1a8a5aaf39c0");
            var querys = productService.Get().Where(a => a.Categoryid == IGCategroyid).ToList();

            ViewBag.Productid = new SelectList(querys, "Productid", "Productname", selectMember);
        }
        #endregion

        #region --YT產品類別選單--
        private void YTProductDropDownList(Object selectMember = null)
        {
            Guid YTCategoryid = Guid.Parse("5653a5c3-102b-4dc6-a9ee-2d9e6c6c4f18");
            var querys = productService.Get().Where(a => a.Categoryid == YTCategoryid).ToList();

            ViewBag.Productid = new SelectList(querys, "Productid", "Productname", selectMember);
        }
        #endregion

        #region --取得台灣時間--
        public DateTime dt_tw()
        {
            DateTime dt = DateTime.Now;
            DateTime dttw = dt.AddHours(8);
            return dttw;
        }
        #endregion
    }
}