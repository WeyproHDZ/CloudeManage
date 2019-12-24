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
    public class IGApiController : BaseController
    {
        private IGMembersService igmembersService;
        private IGMembersLoginlogService igmembersloginlogService;
        private IGOrderService igorderService;
        private IGOrderlistService igorderlistService;
        private CategoryProductService categoryproductService;
        private ProductService productService;
        private CategoryMessageSevice categorymessageService;
        private MessageService messageService;

        public IGApiController()
        {
            igmembersService = new IGMembersService();
            igmembersloginlogService = new IGMembersLoginlogService();
            igorderService = new IGOrderService();
            igorderlistService = new IGOrderlistService();
            categoryproductService = new CategoryProductService();
            productService = new ProductService();
            categorymessageService = new CategoryMessageSevice();
            messageService = new MessageService();
        }

        int Now = (int)(DateTime.Now - new DateTime(1970, 1, 1).AddHours(-8)).TotalSeconds;      //目前時間總秒數
        //string txt_filepath = @"C:\Users\wadmin\Desktop\CloudControl_order.txt";    // 問題回報txt位置


        // GET: IGApi
        #region --Instagram文章讚--
        /*** 要IG文章讚的訂單 ***/
        [HttpGet]
        public JsonResult GetIGOrder_ArticleLike(string Id)
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {                
                Product product = productService.GetByID(Guid.Parse("5ac21bfd-9aff-483a-8c28-efa9ad74761b"));
                IGOrder igorder = igorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.IGOrderStatus == 1).FirstOrDefault();
                if (igorder == null)
                {
                    igorder = igorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.IGOrderStatus == 0).FirstOrDefault();
                }
                if (igorder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    igorder.IGOrderStatus = 1;
                    igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
                    igorderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] IGOrderArray = new string[4];
                    IGOrderArray[0] = igorder.IGOrdernumber;
                    IGOrderArray[1] = igorder.Remains.ToString();
                    IGOrderArray[2] = igorder.Url;
                    IGOrderArray[3] = (Now + 3600).ToString();
                    keyValues = new Dictionary<string, string>
                    {
                        { "IGOrdernumber" , IGOrderArray[0]},
                        { "Remains" , IGOrderArray[1]},
                        { "Url" , IGOrderArray[2]}
                    };
                    return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    keyValues = new Dictionary<string, string>
                    {
                        { "Status" , "no_order" }
                    };                
                    return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        /**** 更新IG文章讚的訂單 ***/
        public JsonResult UpdateIGOrder_ArticleLike(string Id, string IGOrdernumber, string status = "failed")
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {
                IGOrder igorder = igorderService.Get().Where(a => a.IGOrdernumber == IGOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    igorder.IGOrderStatus = 2;  //狀態改已成功
                    igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
                    igorderService.SaveChanges();
                }
                else
                {
                    igorder.IGOrderStatus = 3;  //狀態改失敗
                    igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
                    igorderService.SaveChanges();
                }
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Success" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Instagram收回文章讚--
        /***** 要IG收回文章讚的訂單 ***/
        [HttpGet]
        public JsonResult GetIGOrder_RegainArticleLike(string Id)
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {                
                Product product = productService.GetByID(Guid.Parse("5ac21bfd-9aff-483a-8c28-efa9ad74761b"));
                IGOrder igorder = igorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.IGOrderStatus == 5).FirstOrDefault();
                if (igorder == null)
                {
                    igorder = igorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.IGOrderStatus == 4).FirstOrDefault();
                }
                if (igorder != null)
                {
                    /*** 將訂單狀態改為收回中 ***/
                    igorder.IGOrderStatus = 5;
                    igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
                    igorderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] IGOrderArray = new string[4];
                    IGOrderArray[0] = igorder.IGOrdernumber;
                    IGOrderArray[1] = igorder.Count.ToString();
                    IGOrderArray[2] = igorder.Url;

                    keyValues = new Dictionary<string, string>
                    {
                        { "IGOrdernumber" , IGOrderArray[0] },
                        { "Count" , IGOrderArray[1] },
                        { "Url" , IGOrderArray[2] }
                    };
                    return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    keyValues = new Dictionary<string, string>
                    {
                        { "Status" , "no_order" }
                    };
                    return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        /**** 更新IG收回文章讚的訂單 ***/
        public JsonResult UpdateIGOrder_RegainArticleLike(string Id, string IGOrdernumber, string status)
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {
                IGOrder igorder = igorderService.Get().Where(a => a.IGOrdernumber == IGOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    igorder.IGOrderStatus = 6;  //狀態改已收回
                    igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
                    igorderService.SaveChanges();
                }
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Success" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Instagram留言--
        /*** 要IG留言的訂單 ***/
        [HttpGet]
        public JsonResult GetIGOrder_InstagramMessage(string Id)
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("f5993cef-2a79-4413-bdbc-20a841f0c91e"));
                IGOrder igorder = igorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.IGOrderStatus == 1).FirstOrDefault();
                if (igorder == null)
                {
                    igorder = igorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.IGOrderStatus == 0).FirstOrDefault();
                }
                if (igorder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    igorder.IGOrderStatus = 1;
                    igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
                    igorderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] IGOrderArray = new string[4];
                    IGOrderArray[0] = igorder.IGOrdernumber;
                    IGOrderArray[1] = igorder.Remains.ToString();
                    IGOrderArray[2] = igorder.Url;
                    IGOrderArray[3] = (Now + 3600).ToString();

                    keyValues = new Dictionary<string, string>
                    {
                        { "IGOrdernumber", IGOrderArray[0] },
                        { "Remains", IGOrderArray[1] },
                        { "Url", IGOrderArray[2] }
                    };
                    return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    keyValues = new Dictionary<string, string>
                    {
                        { "Status" , "no_order" }
                    };
                    return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        /**** 更新IG留言的訂單 ***/
        public JsonResult UpdateIGOrder_InstagramMessage(string Id, string IGOrdernumber, string status = "failed")
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {
                IGOrder igorder = igorderService.Get().Where(a => a.IGOrdernumber == IGOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    igorder.IGOrderStatus = 2;  //狀態改已成功
                    igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
                    igorderService.SaveChanges();
                }
                else
                {
                    igorder.IGOrderStatus = 3;  //狀態改失敗
                    igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
                    igorderService.SaveChanges();
                }
                keyValues = new Dictionary<string, string>
                {
                    { "Status", "Success" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status", "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Instagram粉絲追蹤--
        /*** 要IG粉絲追蹤的訂單 ***/
        [HttpGet]
        public JsonResult GetIGOrder_Follow(string Id)
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {                
                Product product = productService.GetByID(Guid.Parse("2422e90a-86c9-4e5c-b119-3623140cdc5c"));
                IGOrder igorder = igorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.IGOrderStatus == 1).FirstOrDefault();
                if (igorder == null)
                {
                    igorder = igorderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.IGOrderStatus == 0).FirstOrDefault();
                }
                if (igorder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    igorder.IGOrderStatus = 1;
                    igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
                    igorderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] IGOrderArray = new string[4];
                    IGOrderArray[0] = igorder.IGOrdernumber;
                    IGOrderArray[1] = igorder.Remains.ToString();
                    IGOrderArray[2] = igorder.Url;
                    IGOrderArray[3] = (Now + 3600).ToString();

                    keyValues = new Dictionary<string, string>
                    {
                        { "IGOrdernumber", IGOrderArray[0] },
                        { "Remains", IGOrderArray[1] },
                        { "Url", IGOrderArray[2] }
                    };
                    return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    keyValues = new Dictionary<string, string>
                    {
                        { "Status" , "no_order" }
                    };
                    return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        /**** 更新IG粉絲追蹤的訂單 ***/
        public JsonResult UpdateIGOrder_Follow(string Id, string IGOrdernumber, string status = "failed")
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {
                IGOrder igorder = igorderService.Get().Where(a => a.IGOrdernumber == IGOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    igorder.IGOrderStatus = 2;  //狀態改已成功
                    igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
                    igorderService.SaveChanges();
                }
                else
                {
                    igorder.IGOrderStatus = 3;  //狀態改失敗
                    igorderService.SpecificUpdate(igorder, new string[] { "IGOrderStatus" });
                    igorderService.SaveChanges();
                }
                keyValues = new Dictionary<string, string>
                {
                    { "Status", "Success" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status", "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --要帳號--
        [HttpGet]
        public JsonResult GetIGMember(string Id, int number, string IGOrdernumber)
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {
                IGOrder igorder = igorderService.Get().Where(a => a.IGOrdernumber == IGOrdernumber).FirstOrDefault();   // 撈該訂單
                IEnumerable<IGOrderlist> igorderlist = igorderlistService.Get().Where(a => a.IGOrder.IGOrdernumber == IGOrdernumber);   // 撈該訂單的完成列表
                IEnumerable<IGOrder> old_igorder = igorderService.Get().Where(c => c.Productid == igorder.Productid).Where(a => a.IGOrdernumber != IGOrdernumber).Where(x => x.Url == igorder.Url);  // 撈所有訂單裡網址為此訂單及產品為此訂單的資料
                Product product = productService.GetByID(igorder.Productid);    // 撈此訂單所需的產品
                List<get_old_member> MemberList = new List<get_old_member>();
                /**** 先排除這張訂單的完成訂單裡的人 ****/
                if (igorderlist != null)
                {
                    foreach (IGOrderlist list in igorderlist)
                    {
                        MemberList.Add(
                        new get_old_member
                        {
                            Memberid = Guid.Parse(list.IGMemberid.ToString())
                        });
                    }
                }

                /*** 將同網址的訂單的完成訂單裡的人排除掉 ****/
                if (old_igorder != null)
                {
                    foreach (IGOrder thisold_igorder in old_igorder)
                    {
                        IEnumerable<IGOrderlist> old_igorderlist = igorderlistService.Get().Where(a => a.IGOrderid == thisold_igorder.IGOrderid);
                        foreach (IGOrderlist thisold_igorderlist in old_igorderlist)
                        {
                            MemberList.Add(
                                new get_old_member
                                {
                                    Memberid = Guid.Parse(thisold_igorderlist.IGMemberid.ToString())
                                }
                            );
                        }
                    }
                }

                List<get_member> AccountList = new List<get_member>();
                IEnumerable<IGMembers> IGMembers = igmembersService.Get().Where(a => a.Lastdate <= Now).Where(x => x.IGMembersLoginlog.OrderByDescending(c => c.Createdate).FirstOrDefault().Status != 2).OrderBy(o => o.Lastdate);    // 撈可用時間小於現在以及驗證狀態不是需驗證的會員
                if (IGMembers != null)
                {
                    foreach (IGMembers Member in IGMembers)
                    {
                        bool used = false;
                        int loop;
                        for (loop = 0; loop < MemberList.Count(); loop++)
                        {
                            if (Member.IGMemberid == MemberList[loop].Memberid)
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
                                Cookie = Cookie.Replace(@"""", "");    // 將" 取代成空白  
                                Cookie = Cookie.Replace('\\', '\"');   // 將\\ 取代成\"
                            }
                            /**** 判斷這張單是否是留言訂單 ****/
                            if (product.Productid == Guid.Parse("f5993cef-2a79-4413-bdbc-20a841f0c91e"))
                            {
                                /**** 撈取該訂單的留言類別 ****/
                                Guid CategortMessageid = categorymessageService.GetByID(igorder.Categoryid).Categoryid;
                                /*** 撈取該留言類別底下的留言 ***/
                                int messageConunt = messageService.Get().Where(a => a.Categoryid == igorder.Categoryid).Count();    // 該留言類別底下的留言數量
                                Message[] message = messageService.Get().Where(a => a.Categoryid == igorder.Categoryid).ToArray();
                                Random crandom = new Random();
                                int rand = crandom.Next(0, messageConunt - 1);
                                AccountList.Add(
                                    new get_member
                                    {
                                        Memberid = Member.IGMemberid,
                                        Account = Member.IG_Account,
                                        Password = Member.IG_Password,
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
                                        Memberid = Member.IGMemberid,
                                        Account = Member.IG_Account,
                                        Password = Member.IG_Password,
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
                        keyValues = new Dictionary<string, string>
                        {
                            { "Status" , "no_member" }
                        };
                        return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        //Memberid = Member.IGMemberid,
                        //Account = Member.IG_Account,
                        //Password = Member.IG_Password,
                        //Useragent_phone = Member.Useragent,
                        //Cookie = Cookie,
                        //Duedate = (Now + 3600),
                        foreach (get_member entity in AccountList.Take(number))
                        {
                            /*** 將此會員更新下次互惠時間 ****/
                            IGMembers Member = igmembersService.GetByID(entity.Memberid);
                            Member.Lastdate = Now + 120;
                            igmembersService.SpecificUpdate(Member, new string[] { "Lastdate" });

                            keyValues = new Dictionary<string, string>
                            {
                                { "Memberid", entity.Memberid.ToString() },
                                { "Account", entity.Account },
                                { "Password", entity.Password },
                                { "Useragent", entity.Useragent_phone },
                                { "Cookie", entity.Cookie }
                            };
                        }
                        igmembersService.SaveChanges();
                        return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    keyValues = new Dictionary<string, string>
                    {
                        { "Status" , "no_member" }
                    };
                    return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --更新會員--
        [HttpPost]
        public JsonResult UpdateIGMember(string Id, string IGOrdernumber, string Memberid, string Cookie, int AccountStatus)
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {
                IGOrder igorder = igorderService.Get().Where(a => a.IGOrdernumber == IGOrdernumber).FirstOrDefault();
                IGMembers igmembers = igmembersService.GetByID(Guid.Parse(Memberid));
                /*** 0 : 帳號有問題，1 : 成功執行，2 : 找不到執行的位置***/
                if (AccountStatus == 0)
                {
                    /*** 將會員寫入登入紀錄裡 ****/
                    IGMembersLoginlog igmembersloginlog = new IGMembersLoginlog();
                    igmembersloginlog.Createdate = dt_tw();
                    igmembersloginlog.IGMemberid = Guid.Parse(Memberid);
                    igmembersloginlog.Status = 2;   // 【 0:未驗證 , 1:已驗證 , 2:需驗證 】
                    igmembersloginlogService.Create(igmembersloginlog);
                    igmembersloginlogService.SaveChanges();
                    /**** 寫入TXT檔 *****/
                    //using (StreamWriter sw = new StreamWriter(txt_filepath, true))
                    //{
                    //    sw.Write("CloudControl訂單問題回報 訂單編號:" + igorder.IGOrdernumber + "會員帳號:" + igmembers.IG_Account + "登入有問題");
                    //    sw.Write(Environment.NewLine);
                    //    sw.Write(dt_tw());
                    //    sw.Write(Environment.NewLine);
                    //}
                    keyValues = new Dictionary<string, string>
                    {
                        { "Status" , "Success" }
                    };
                    return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                }
                else if (AccountStatus == 1)
                {
                    /*** 更新該會員的Cookie *****/
                    igmembers.Cookie = Cookie;
                    igmembersService.SpecificUpdate(igmembers, new string[] { "Cookie" });
                    igmembersService.SaveChanges();
                    /**** 寫入登入表裡 *****/
                    IGMembersLoginlog igmembersloginlog = new IGMembersLoginlog();
                    igmembersloginlog.IGMemberid = Guid.Parse(Memberid);
                    igmembersloginlog.Status = 1;
                    igmembersloginlog.Createdate = dt_tw();
                    igmembersloginlogService.Create(igmembersloginlog);
                    igmembersloginlogService.SaveChanges();
                    /*** 改訂單剩餘人數 ***/
                    igorder.Remains -= 1;                    
                    igorderService.SpecificUpdate(igorder, new string[] { "Remains" });
                    igorderService.SaveChanges();
                    IGOrderlist igorderlist = new IGOrderlist();
                    igorderlist.IGOrderlistid = Guid.NewGuid();
                    igorderlist.IGMemberid = Guid.Parse(Memberid);
                    igorderlist.IGAccount = igmembers.IG_Account;
                    igorderlist.IGOrderid = igorder.IGOrderid;
                    igorderlist.Createdate = dt_tw();
                    igorderlist.Updatedate = dt_tw();
                    igorderlistService.Create(igorderlist);
                    igorderlistService.SaveChanges();
                    keyValues = new Dictionary<string, string>
                    {
                        { "Status" , "Success" }
                    };
                    return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    /**** 寫入TXT檔 *****/
                    //using (StreamWriter sw = new StreamWriter(txt_filepath, true))
                    //{
                    //    sw.Write("CloudControl訂單問題回報 訂單編號:" + igorder.IGOrdernumber + "會員帳號:" + igmembers.IG_Account + "找不到執行的位置");
                    //    sw.Write(Environment.NewLine);
                    //    sw.Write(dt_tw());
                    //    sw.Write(Environment.NewLine);
                    //}
                    keyValues = new Dictionary<string, string>
                    {
                        { "Status" , "Success" }
                    };
                    return this.Json(keyValues, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --取得收回的帳號列表--
        [HttpGet]
        public JsonResult GetIGRegainMember(string Id, string IGOrdernumber)
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {
                IGOrder igorder = igorderService.Get().Where(a => a.IGOrdernumber == IGOrdernumber).FirstOrDefault();
                IEnumerable<IGOrderlist> igorderlist = igorderlistService.Get().Where(a => a.IGOrderid == igorder.IGOrderid);
                List<get_member> AccountList = new List<get_member>();

                foreach(IGOrderlist igordertemp in igorderlist)
                {
                    string Cookie = igordertemp.IGMembers.Cookie.Replace("True", "true").Replace("False", "false").Replace("name", "Name").Replace("value", "Value").Replace("path", "Path").Replace("domain", "Domain").Replace("secure", "Secure").Replace("httpOnly", "IsHttpOnly").Replace("expiry", "Expiry");
                    Cookie = Cookie.Replace(@"""", "");    // 將" 取代成空白  
                    Cookie = Cookie.Replace('\\', '\"');   // 將\\ 取代成\"
                    AccountList.Add(
                        new get_member
                        {
                            Memberid = Guid.Parse(igordertemp.IGMemberid.ToString()),
                            Account = igordertemp.IGAccount,
                            Password = igordertemp.IGMembers.IG_Password,
                            Useragent_phone = igordertemp.IGMembers.Useragent,
                            Cookie = Cookie
                        }
                    );

                    keyValues = new Dictionary<string, string>
                    {
                        { "Memberid", igordertemp.IGMemberid.ToString() },
                        { "Account", igordertemp.IGAccount },
                        { "Password", igordertemp.IGMembers.IG_Password },
                        { "Useragent", igordertemp.IGMembers.Useragent },
                        { "Cookie", Cookie }
                    };
                }
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --更新收回的帳號--
        [HttpPost]
        public JsonResult UpdateIGRegainMember(string Id, string IGOrdernumber, string Memberid, string Cookie, int AccountStatus)
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {
                IGOrder igorder = igorderService.Get().Where(a => a.IGOrdernumber == IGOrdernumber).FirstOrDefault();
                IGMembers igmembers = igmembersService.GetByID(Guid.Parse(Memberid));
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Success" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --查看訂單狀態--
        public JsonResult CheckIGOrderStatus(string Id, string IGOrdernumber)
        {
            var keyValues = new Dictionary<string, string>();
            if (Id == "CloudControl_order")
            {
                IGOrder igorder = igorderService.Get().Where(a => a.IGOrdernumber == IGOrdernumber).FirstOrDefault();
                keyValues = new Dictionary<string, string>
                {
                    { "IGOrderStatus" , igorder.IGOrderStatus.ToString() }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
            else
            {
                keyValues = new Dictionary<string, string>
                {
                    { "Status" , "Error" }
                };
                return this.Json(keyValues, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion







        #region --測試用API--
        [HttpGet]
        public JsonResult Test_Api(string Id)
        {
            if (Id == "taepsit")
            {
                IEnumerable<IGMembers> igmembers = igmembersService.Get().Where(a => a.Isenable == 0);
                foreach (IGMembers member in igmembers)
                {
                    IGMembersLoginlog igmembersloginlog = new IGMembersLoginlog();
                    igmembersloginlog.Createdate = dt_tw();
                    igmembersloginlog.Status = 2;
                    igmembersloginlog.IGMemberid = member.IGMemberid;
                    igmembersloginlogService.Create(igmembersloginlog);
                }
                igmembersloginlogService.SaveChanges();
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
            DateTime dttw = dt.AddHours(8);
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