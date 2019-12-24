using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudControl.Model;
using CloudControl.Service;
using System.IO;


namespace CloudControlBackend.Controllers
{
    public class YTApiController : BaseController
    {
        private YTMembersService ytmembersService;
        private YTMembersLoginlogService ytmembersloginlogService;
        private YTOrderService ytorderService;
        private YTOrderlistService ytorderlistService;
        private CategoryProductService categoryproductService;
        private ProductService productService;
        private CategoryMessageSevice categorymessageService;
        private MessageService messageService;

        public YTApiController()
        {
            ytmembersService = new YTMembersService();
            ytmembersloginlogService = new YTMembersLoginlogService();
            ytorderService = new YTOrderService();
            ytorderlistService = new YTOrderlistService();
            categoryproductService = new CategoryProductService();
            productService = new ProductService();
            categorymessageService = new CategoryMessageSevice();
            messageService = new MessageService();
        }

        int Now = (int)(DateTime.Now - new DateTime(1970, 1, 1).AddHours(-8)).TotalSeconds;      //目前時間總秒數
        string txt_filepath = @"C:\Users\Jessie\Desktop\CloudControl_order.txt";    // 問題回報txt位置
                                                                                    // GET: YTApi
        #region --Youtube影片喜歡--
        /*** 要YT影片喜歡的訂單 ***/
        [HttpGet]
        public JsonResult GetYTOrder_VideoLike(string Id)
        {

            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("e06a7760-f8e8-4d45-8457-5734ae3c034b"));
                YTOrder ytorder = ytorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.YTOrderStatus == 1).FirstOrDefault();
                if (ytorder == null)
                {
                    ytorder = ytorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.YTOrderStatus == 0).FirstOrDefault();
                }
                if (ytorder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    ytorder.YTOrderStatus = 1;
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] YTOrderArray = new string[4];
                    YTOrderArray[0] = ytorder.YTOrdernumber;
                    YTOrderArray[1] = ytorder.Remains.ToString();
                    YTOrderArray[2] = ytorder.Url;
                    YTOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(YTOrderArray, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json("目前沒有訂單", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        /**** 更新YT影片喜歡的訂單 ***/
        public JsonResult UpdateYTOrder_VideoLike(string Id, string YTOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                YTOrder ytorder = ytorderService.Get().Where(a => a.YTOrdernumber == YTOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    ytorder.YTOrderStatus = 2;  //狀態改已成功
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                }
                else
                {
                    ytorder.YTOrderStatus = 3;  //狀態改失敗
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Youtube影片不喜歡--
        public JsonResult GetYTOrder_VideoDisLike(string Id)
        {
            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("6698e003-de20-40d6-a47c-12ff4d0c2edf"));
                YTOrder ytorder = ytorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.YTOrderStatus == 1).FirstOrDefault();
                if (ytorder == null)
                {
                    ytorder = ytorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.YTOrderStatus == 0).FirstOrDefault();
                }
                if (ytorder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    ytorder.YTOrderStatus = 1;
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] YTOrderArray = new string[4];
                    YTOrderArray[0] = ytorder.YTOrdernumber;
                    YTOrderArray[1] = ytorder.Remains.ToString();
                    YTOrderArray[2] = ytorder.Url;
                    YTOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(YTOrderArray, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json("目前沒有訂單", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        /**** 更新YT影片不喜歡的訂單 ***/
        public JsonResult UpdateYTOrder_VideoDisLike(string Id, string YTOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                YTOrder ytorder = ytorderService.Get().Where(a => a.YTOrdernumber == YTOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    ytorder.YTOrderStatus = 2;  //狀態改已成功
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                }
                else
                {
                    ytorder.YTOrderStatus = 3;  //狀態改失敗
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Youtube影片收回喜歡--
        /*** 要YT影片收回喜歡的訂單 ***/
        [HttpGet]
        public JsonResult GetYTOrder_VideoRegainLike(string Id)
        {

            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("e06a7760-f8e8-4d45-8457-5734ae3c034b"));
                YTOrder ytorder = ytorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.YTOrderStatus == 5).FirstOrDefault();
                if (ytorder == null)
                {
                    ytorder = ytorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.YTOrderStatus == 4).FirstOrDefault();
                }
                if (ytorder != null)
                {
                    /*** 將訂單狀態改為收回中 ***/
                    ytorder.YTOrderStatus = 5;
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] YTOrderArray = new string[4];
                    YTOrderArray[0] = ytorder.YTOrdernumber;
                    YTOrderArray[1] = ytorder.Count.ToString();
                    YTOrderArray[2] = ytorder.Url;
                    return this.Json(YTOrderArray, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json("目前沒有訂單", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        /**** 更新YT影片收回喜歡的訂單 ***/
        public JsonResult UpdateYTOrder_VideoRegainLike(string Id, string YTOrdernumber, string status)
        {
            if (Id == "CloudControl_order")
            {
                YTOrder ytorder = ytorderService.Get().Where(a => a.YTOrdernumber == YTOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    ytorder.YTOrderStatus = 6;  //狀態改已收回
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Youtube影片收回不喜歡--
        /*** 要YT影片收回不喜歡的訂單 ***/
        public JsonResult GetYTOrder_VideoRegainDisLike(string Id)
        {
            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("6698e003-de20-40d6-a47c-12ff4d0c2edf"));
                YTOrder ytorder = ytorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.YTOrderStatus == 5).FirstOrDefault();
                if (ytorder == null)
                {
                    ytorder = ytorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.YTOrderStatus == 4).FirstOrDefault();
                }
                if (ytorder != null)
                {
                    /*** 將訂單狀態改為收回中 ***/
                    ytorder.YTOrderStatus = 5;
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] YTOrderArray = new string[4];
                    YTOrderArray[0] = ytorder.YTOrdernumber;
                    YTOrderArray[1] = ytorder.Remains.ToString();
                    YTOrderArray[2] = ytorder.Url;
                    return this.Json(YTOrderArray, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json("目前沒有訂單", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        /**** 更新YT影片不喜歡的訂單 ***/
        public JsonResult UpdateYTOrder_VideoRegainDisLike(string Id, string YTOrdernumber, string status)
        {
            if (Id == "CloudControl_order")
            {
                YTOrder ytorder = ytorderService.Get().Where(a => a.YTOrdernumber == YTOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    ytorder.YTOrderStatus = 6;  //狀態改已收回
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Youtube留言--
        /*** 要YT留言的訂單 ***/
        [HttpGet]
        public JsonResult GetYTOrder_YoutubeMessage(string Id)
        {

            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("205548ac-b02c-42a1-8e43-1ce78f451dd8"));
                YTOrder ytorder = ytorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.YTOrderStatus == 1).FirstOrDefault();
                if (ytorder == null)
                {
                    ytorder = ytorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.YTOrderStatus == 0).FirstOrDefault();
                }
                if (ytorder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    ytorder.YTOrderStatus = 1;
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] YTOrderArray = new string[4];
                    YTOrderArray[0] = ytorder.YTOrdernumber;
                    YTOrderArray[1] = ytorder.Remains.ToString();
                    YTOrderArray[2] = ytorder.Url;
                    YTOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(YTOrderArray, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json("目前沒有訂單", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        /**** 更新YT留言的訂單 ***/
        public JsonResult UpdateYTOrder_YoutubeMessage(string Id, string YTOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                YTOrder ytorder = ytorderService.Get().Where(a => a.YTOrdernumber == YTOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    ytorder.YTOrderStatus = 2;  //狀態改已成功
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                }
                else
                {
                    ytorder.YTOrderStatus = 3;  //狀態改失敗
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Youtube訂閱--
        /*** 要YT訂閱的訂單 ***/
        [HttpGet]
        public JsonResult GetYTOrder_Subscription(string Id)
        {
            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("10cca5b9-6353-436b-8c5e-1150492c89cc"));
                YTOrder ytorder = ytorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.YTOrderStatus == 1).FirstOrDefault();
                if (ytorder == null)
                {
                    ytorder = ytorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.YTOrderStatus == 0).FirstOrDefault();
                }
                if (ytorder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    ytorder.YTOrderStatus = 1;
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] YTOrderArray = new string[4];
                    YTOrderArray[0] = ytorder.YTOrdernumber;
                    YTOrderArray[1] = ytorder.Remains.ToString();
                    YTOrderArray[2] = ytorder.Url;
                    YTOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(YTOrderArray, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return this.Json("目前沒有訂單", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        /**** 更新YT訂閱的訂單 ***/
        public JsonResult UpdateYTOrder_Subscription(string Id, string YTOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                YTOrder ytorder = ytorderService.Get().Where(a => a.YTOrdernumber == YTOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    ytorder.YTOrderStatus = 2;  //狀態改已成功
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                }
                else
                {
                    ytorder.YTOrderStatus = 3;  //狀態改失敗
                    ytorderService.SpecificUpdate(ytorder, new string[] { "YTOrderStatus" });
                    ytorderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --要帳號--
        public JsonResult GetYTMember(string Id, int number, string YTOrdernumber)
        {
            if (Id == "CloudControl_order")
            {
                YTOrder ytorder = ytorderService.Get().Where(a => a.YTOrdernumber == YTOrdernumber).FirstOrDefault();   // 撈該訂單
                IEnumerable<YTOrderlist> ytorderlist = ytorderlistService.Get().Where(a => a.YTOrder.YTOrdernumber == YTOrdernumber);   // 撈該訂單的完成列表
                IEnumerable<YTOrder> old_ytorder = ytorderService.Get().Where(c => c.Productid == ytorder.Productid).Where(a => a.YTOrdernumber != YTOrdernumber).Where(x => x.Url == ytorder.Url);  // 撈所有訂單裡網址為此訂單及產品為此訂單的資料
                Product product = productService.GetByID(ytorder.Productid);    // 撈此訂單所需的產品
                List<get_old_member> MemberList = new List<get_old_member>();
                /**** 先排除這張訂單的完成訂單裡的人 ****/
                if (ytorderlist != null)
                {
                    foreach (YTOrderlist list in ytorderlist)
                    {
                        MemberList.Add(
                        new get_old_member
                        {
                            Memberid = Guid.Parse(list.YTMemberid.ToString())
                        });
                    }
                }

                /*** 將同網址的訂單的完成訂單裡的人排除掉 ****/
                if (old_ytorder != null)
                {
                    foreach (YTOrder thisold_ytorder in old_ytorder)
                    {
                        IEnumerable<YTOrderlist> old_ytorderlist = ytorderlistService.Get().Where(a => a.YTOrderid == thisold_ytorder.YTOrderid);
                        foreach (YTOrderlist thisold_ytorderlist in old_ytorderlist)
                        {
                            MemberList.Add(
                                new get_old_member
                                {
                                    Memberid = Guid.Parse(thisold_ytorderlist.YTMemberid.ToString())
                                }
                            );
                        }
                    }
                }

                List<get_member> AccountList = new List<get_member>();
                IEnumerable<YTMembers> YTMembers = ytmembersService.Get().Where(a => a.Lastdate <= Now).Where(x => x.YTMembersLoginlog.OrderByDescending(c => c.Createdate).FirstOrDefault().Status != 2).OrderBy(o => o.Lastdate);    // 撈可用時間小於現在以及驗證狀態不是需驗證的會員
                if (YTMembers != null)
                {
                    foreach (YTMembers Member in YTMembers)
                    {
                        bool used = false;
                        int loop;
                        for (loop = 0; loop < MemberList.Count(); loop++)
                        {
                            if (Member.YTMemberid == MemberList[loop].Memberid)
                            {
                                used = true;
                            }
                        }

                        if (used == false)
                        {
                            string Cookie = "";
                            if (Member.Cookie != null)
                            {
                                Cookie = Member.Cookie.Replace("True", "true").Replace("False", "false").Replace("name", "Name").Replace("value", "Value").Replace("path", "Path").Replace("domain", "Domain").Replace("secure", "Secure").Replace("httpOnly", "IsHttpOnly").Replace("expiry", "Expiry");
                                Cookie = Cookie.Replace("'", @"""");     // 將' 取代成 "
                            }
                            /**** 判斷這張單是否是留言訂單 ****/
                            if (product.Productid == Guid.Parse("205548ac-b02c-42a1-8e43-1ce78f451dd8"))
                            {
                                /**** 撈取該訂單的留言類別 ****/
                                Guid CategortMessageid = categorymessageService.GetByID(ytorder.Categoryid).Categoryid;
                                /*** 撈取該留言類別底下的留言 ***/
                                int messageConunt = messageService.Get().Where(a => a.Categoryid == ytorder.Categoryid).Count();    // 該留言類別底下的留言數量
                                Message[] message = messageService.Get().Where(a => a.Categoryid == ytorder.Categoryid).ToArray();
                                Random crandom = new Random();
                                int rand = crandom.Next(0, messageConunt - 1);
                                AccountList.Add(
                                    new get_member
                                    {
                                        Memberid = Member.YTMemberid,
                                        Account = Member.YT_Account,
                                        Password = Member.YT_Password,
                                        Useragent_phone = Member.Useragent,
                                        Cookie = Cookie,
                                        Message = message[rand].MessageName
                                    }
                                );
                                used = true;
                            }
                            else
                            {
                                AccountList.Add(
                                    new get_member
                                    {
                                        Memberid = Member.YTMemberid,
                                        Account = Member.YT_Account,
                                        Password = Member.YT_Password,
                                        Useragent_phone = Member.Useragent,
                                        Cookie = Cookie,
                                    }
                                );
                                used = true;
                            }
                        }
                    }
                    /*** 可用人數小於該訂單所需人數 ****/
                    if (AccountList.Count() < number)
                    {
                        return this.Json("數量不足", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        foreach (get_member entity in AccountList.Take(number))
                        {
                            /*** 將此會員更新下次互惠時間 ****/
                            YTMembers Member = ytmembersService.GetByID(entity.Memberid);
                            Member.Lastdate = Now + 120;
                            ytmembersService.SpecificUpdate(Member, new string[] { "Lastdate" });
                        }
                        ytmembersService.SaveChanges();
                        return this.Json(AccountList.Take(number), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return this.Json("數量不足", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --更新會員--
        [HttpPost]
        public JsonResult UpdateYTMember(string Id, string YTOrdernumber, string Memberid, string Cookie, int AccountStatus)
        {
            if (Id == "CloudControl_order")
            {
                YTOrder ytorder = ytorderService.Get().Where(a => a.YTOrdernumber == YTOrdernumber).FirstOrDefault();
                YTMembers ytmembers = ytmembersService.GetByID(Guid.Parse(Memberid));
                /*** 0 : 帳號需驗證，1 : 帳密輸入錯誤 or 密碼更改，2 : 成功執行，3 : 找不到讚的位置***/
                if (AccountStatus == 0)
                {
                    /*** 將會員寫入會員紀錄裡 ***/
                    YTMembersLoginlog ytmembersloginlog = new YTMembersLoginlog();
                    ytmembersloginlog.Createdate = dt_tw();
                    ytmembersloginlog.YTMemberid = Guid.Parse(Memberid);
                    ytmembersloginlog.Status = 2;   // 【 0:未驗證 , 1:已驗證 , 2:需驗證 】
                    ytmembersloginlogService.Create(ytmembersloginlog);
                    ytmembersloginlogService.SaveChanges();
                    /**** 寫入TXT檔 *****/
                    using (StreamWriter sw = new StreamWriter(txt_filepath, true))
                    {
                        sw.Write("CloudControl訂單問題回報 訂單編號:" + ytorder.YTOrdernumber + "會員帳號:" + ytmembers.YT_Account + "登入有問題(帳號需驗證)");
                        sw.Write(Environment.NewLine);
                        sw.Write(dt_tw());
                        sw.Write(Environment.NewLine);
                    }
                    return this.Json("Success");
                }
                else if (AccountStatus == 1)
                {
                    /**** 寫入TXT檔 *****/
                    using (StreamWriter sw = new StreamWriter(txt_filepath, true))
                    {
                        sw.Write("CloudControl訂單問題回報 訂單編號:" + ytorder.YTOrdernumber + "會員帳號:" + ytmembers.YT_Account + "登入有問題(密碼更改or帳密錯誤)");
                        sw.Write(Environment.NewLine);
                        sw.Write(dt_tw());
                        sw.Write(Environment.NewLine);
                    }
                    return this.Json("Success");
                }
                else if (AccountStatus == 2)
                {
                    /*** 更新該會員的Cookie *****/
                    ytmembers.Cookie = Cookie;
                    ytmembersService.SpecificUpdate(ytmembers, new string[] { "Cookie" });
                    ytmembersService.SaveChanges();
                    /**** 寫入登入表裡 ***/
                    YTMembersLoginlog ytmembersloginlog = new YTMembersLoginlog();
                    ytmembersloginlog.YTMemberid = Guid.Parse(Memberid);
                    ytmembersloginlog.Createdate = dt_tw();
                    ytmembersloginlog.Status = 1;
                    ytmembersloginlogService.Create(ytmembersloginlog);
                    ytmembersloginlogService.SaveChanges();
                    /*** 改訂單剩餘人數 ***/
                    ytorder.Remains -= 1;
                    ytorderService.SpecificUpdate(ytorder, new string[] { "Remains", "Cookie" });
                    ytorderService.SaveChanges();
                    YTOrderlist ytorderlist = new YTOrderlist();
                    ytorderlist.YTOrderlistid = Guid.NewGuid();
                    ytorderlist.YTMemberid = Guid.Parse(Memberid);
                    ytorderlist.YTAccount = ytmembers.YT_Account;
                    ytorderlist.YTOrderid = ytorder.YTOrderid;
                    ytorderlist.Createdate = dt_tw();
                    ytorderlist.Updatedate = dt_tw();
                    ytorderlistService.Create(ytorderlist);
                    ytorderlistService.SaveChanges();
                    return this.Json("Success");
                }
                else
                {
                    /**** 寫入TXT檔 *****/
                    using (StreamWriter sw = new StreamWriter(txt_filepath, true))
                    {
                        sw.Write("CloudControl訂單問題回報 訂單編號:" + ytorder.YTOrdernumber + " 會員帳號:" + ytmembers.YT_Account + "找不到執行的位置");
                        sw.Write(Environment.NewLine);
                        sw.Write(dt_tw());
                        sw.Write(Environment.NewLine);
                    }
                    return this.Json("Success");
                }
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --取得收回的帳號列表--
        public JsonResult GetYTRegainMember(string Id, string YTOrdernumber)
        {
            if (Id == "CloudControl_order")
            {
                YTOrder ytorder = ytorderService.Get().Where(a => a.YTOrdernumber == YTOrdernumber).FirstOrDefault();
                IEnumerable<YTOrderlist> ytorderlist = ytorderlistService.Get().Where(a => a.YTOrderid == ytorder.YTOrderid);
                List<get_member> AccountList = new List<get_member>();

                foreach (YTOrderlist ytordertemp in ytorderlist)
                {
                    string Cookie = ytordertemp.YTMembers.Cookie.Replace("True", "true").Replace("False", "false").Replace("name", "Name").Replace("value", "Value").Replace("path", "Path").Replace("domain", "Domain").Replace("secure", "Secure").Replace("httpOnly", "IsHttpOnly").Replace("expiry", "Expiry");
                    Cookie = Cookie.Replace("'", @"""");     // 將' 取代成 " 

                    AccountList.Add(
                        new get_member
                        {
                            Memberid = Guid.Parse(ytordertemp.YTMemberid.ToString()),
                            Account = ytordertemp.YTAccount,
                            Password = ytordertemp.YTMembers.YT_Password,
                            Useragent_phone = ytordertemp.YTMembers.Useragent,
                            Cookie = Cookie
                        }
                    );
                }
                return this.Json(AccountList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --更新收回的帳號--
        [HttpPost]
        public JsonResult UpdateYTRegainMember(string Id, string YTOrdernumber, string Memberid, string Cookie, int AccountStatus)
        {
            if (Id == "CloudControl_order")
            {
                YTOrder ytorder = ytorderService.Get().Where(a => a.YTOrdernumber == YTOrdernumber).FirstOrDefault();
                YTMembers ytmembers = ytmembersService.GetByID(Guid.Parse(Memberid));
                /*** 0 : 帳號需驗證，1 : 帳密輸入錯誤 or 密碼更改，2 : 成功執行，3 : 找不到讚的位置***/
                if (AccountStatus == 0)
                {
                    /**** 寫入TXT檔 *****/
                    using (StreamWriter sw = new StreamWriter(txt_filepath, true))
                    {
                        sw.Write("CloudControl訂單問題回報 訂單編號:" + ytorder.YTOrdernumber + "會員帳號:" + ytmembers.YT_Account + "登入有問題(帳號需驗證)");
                        sw.Write(Environment.NewLine);
                        sw.Write(dt_tw());
                        sw.Write(Environment.NewLine);
                    }
                    return this.Json("Success");
                }
                else if (AccountStatus == 1)
                {
                    /**** 寫入TXT檔 *****/
                    using (StreamWriter sw = new StreamWriter(txt_filepath, true))
                    {
                        sw.Write("CloudControl訂單問題回報 訂單編號:" + ytorder.YTOrdernumber + "會員帳號:" + ytmembers.YT_Account + "登入有問題(密碼更改or帳密錯誤)");
                        sw.Write(Environment.NewLine);
                        sw.Write(dt_tw());
                        sw.Write(Environment.NewLine);
                    }
                    return this.Json("Success");
                }
                else if (AccountStatus == 2)
                {
                    /**** 寫入TXT檔 *****/
                    using (StreamWriter sw = new StreamWriter(txt_filepath, true))
                    {
                        sw.Write("CloudControl訂單問題回報 訂單編號:" + ytorder.YTOrdernumber + "會員帳號:" + ytmembers.YT_Account + "執行成功");
                        sw.Write(Environment.NewLine);
                        sw.Write(dt_tw());
                        sw.Write(Environment.NewLine);
                    }
                    return this.Json("Success");
                }
                else
                {
                    /**** 寫入TXT檔 *****/
                    using (StreamWriter sw = new StreamWriter(txt_filepath, true))
                    {
                        sw.Write("CloudControl訂單問題回報 訂單編號:" + ytorder.YTOrdernumber + " 會員帳號:" + ytmembers.YT_Account + "找不到執行的位置");
                        sw.Write(Environment.NewLine);
                        sw.Write(dt_tw());
                        sw.Write(Environment.NewLine);
                    }
                    return this.Json("Success");
                }
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --查看訂單狀態--
        public JsonResult CheckYTOrderStatus(string Id, string YTOrdernumber)
        {
            if (Id == "CloudControl_order")
            {
                YTOrder ytorder = ytorderService.Get().Where(a => a.YTOrdernumber == YTOrdernumber).FirstOrDefault();
                return this.Json(ytorder.YTOrderStatus, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion








        #region --測試用API--
        [HttpGet]
        public JsonResult Test_Api(string Id)
        {
            if(Id == "taepsit")
            {
                IEnumerable<YTMembers> ytmembers = ytmembersService.Get().Where(a => a.Isenable == 0);
                foreach(YTMembers member in ytmembers)
                {
                    YTMembersLoginlog ytmembersloginlog = new YTMembersLoginlog();
                    ytmembersloginlog.Createdate = dt_tw();
                    ytmembersloginlog.Status = 2;
                    ytmembersloginlog.YTMemberid = member.YTMemberid;
                    ytmembersloginlogService.Create(ytmembersloginlog);
                }
                ytmembersloginlogService.SaveChanges();
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --取得台灣時間--
        public DateTime dt_tw()
        {
            DateTime dt = DateTime.Now.ToUniversalTime();
            DateTime dttw = TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time"));
            return dttw;
        }
        #endregion
        public class get_member
        {
            public Guid Memberid { get; set; }
            public string Account { get; set; }
            public string Password { get; set; }
            public string Message { get; set; }
            public string Useragent_phone { get; set; }
            public string Cookie { get; set; }
        }
    }

    
}