﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudControl.Model;
using CloudControl.Service;
using System.IO;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace CloudControlBackend.Controllers
{
    public class FBApiController : BaseController
    {
        private FBMembersService fbmembersService;
        private FBMembersLoginlogService fbmembersloginlogService;
        private FBOrderService fborderService;
        private FBOrderlistService fborderlistService;
        private FBVMStatusService fbvmstatusService;
        private FBVMLogService fbvmlogService;
        private CategoryProductService categoryproductService;
        private ProductService productService;
        private CategoryMessageSevice categorymessageService;
        private MessageService messageService;
        private UseragentService useragentService;
        public FBApiController()
        {
            fbmembersService = new FBMembersService();
            fbmembersloginlogService = new FBMembersLoginlogService();
            fborderService = new FBOrderService();
            fborderlistService = new FBOrderlistService();
            fbvmstatusService = new FBVMStatusService();
            fbvmlogService = new FBVMLogService();
            categoryproductService = new CategoryProductService();
            productService = new ProductService();
            categorymessageService = new CategoryMessageSevice();
            messageService = new MessageService();
            useragentService = new UseragentService();
        }
        int Now = (int)(DateTime.Now - new DateTime(1970, 1, 1).AddHours(-8)).TotalSeconds;      //目前時間總秒數
        string txt_filepath = @"C:\Users\wadmin\Desktop\CloudControl_order.txt";    // 問題回報txt位置
        #region --Facebook文章讚--
        /*** 要FB文章讚的訂單 ***/
        [HttpGet]
        public JsonResult GetFBOrder_ArticleLike(string Id)
        {
            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("0c020482-d76a-4213-b021-f8db0fe96489"));
                FBOrder fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 1).FirstOrDefault();
                if (fborder == null)
                {
                    fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 0).FirstOrDefault();
                }
                if (fborder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    fborder.FBOrderStatus = 1;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] FBOrderArray = new string[4];
                    FBOrderArray[0] = fborder.FBOrdernumber;
                    FBOrderArray[1] = fborder.Remains.ToString();
                    FBOrderArray[2] = fborder.Url;
                    FBOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
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
        /**** 更新FB文章讚的訂單 ***/
        public JsonResult UpdateFBOrder_ArticleLike(string Id, string FBOrdernumber , string status = "failed")
        {
            if(Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {                    
                    fborder.FBOrderStatus = 2;  //狀態改已成功
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                else
                {
                    fborder.FBOrderStatus = 3;  //狀態改失敗
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Facebook文章愛心--
        /*** 要FB文章愛心的訂單 ***/
        [HttpGet]
        public JsonResult GetFBOrder_ArticleLove(string Id)
        {

            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("6c5425d0-5362-4fa9-8c6e-dfb929877b93"));
                FBOrder fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 1).FirstOrDefault();
                if (fborder == null)
                {
                    fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 0).FirstOrDefault();
                }
                if (fborder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    fborder.FBOrderStatus = 1;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] FBOrderArray = new string[4];
                    FBOrderArray[0] = fborder.FBOrdernumber;
                    FBOrderArray[1] = fborder.Remains.ToString();
                    FBOrderArray[2] = fborder.Url;
                    FBOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
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
        /**** 更新FB文章愛心的訂單 ***/
        public JsonResult UpdateFBOrder_ArticleLove(string Id, string FBOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    fborder.FBOrderStatus = 2;  //狀態改已成功
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                else
                {
                    fborder.FBOrderStatus = 3;  //狀態改失敗
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Facebook文章哈--
        /*** 要FB文章哈的訂單 ***/
        [HttpGet]
        public JsonResult GetFBOrder_ArticleHaha(string Id)
        {

            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("6b1e8bd2-8dbb-4282-89df-b509be5ff361"));
                FBOrder fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 1).FirstOrDefault();
                if (fborder == null)
                {
                    fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 0).FirstOrDefault();
                }
                if (fborder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    fborder.FBOrderStatus = 1;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] FBOrderArray = new string[4];
                    FBOrderArray[0] = fborder.FBOrdernumber;
                    FBOrderArray[1] = fborder.Remains.ToString();
                    FBOrderArray[2] = fborder.Url;
                    FBOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
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
        /**** 更新FB文章哈的訂單 ***/
        public JsonResult UpdateFBOrder_ArticleHaha(string Id, string FBOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    fborder.FBOrderStatus = 2;  //狀態改已成功
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                else
                {
                    fborder.FBOrderStatus = 3;  //狀態改失敗
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Facebook文章哇--
        /*** 要FB文章愛心的訂單 ***/
        [HttpGet]
        public JsonResult GetFBOrder_ArticleWow(string Id)
        {

            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("f9dd03ee-ecd7-4514-94c6-2ea7d72d931c"));
                FBOrder fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 1).FirstOrDefault();
                if (fborder == null)
                {
                    fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 0).FirstOrDefault();
                }
                if (fborder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    fborder.FBOrderStatus = 1;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] FBOrderArray = new string[4];
                    FBOrderArray[0] = fborder.FBOrdernumber;
                    FBOrderArray[1] = fborder.Remains.ToString();
                    FBOrderArray[2] = fborder.Url;
                    FBOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
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
        /**** 更新FB文章哇的訂單 ***/
        public JsonResult UpdateFBOrder_ArticleWow(string Id, string FBOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    fborder.FBOrderStatus = 2;  //狀態改已成功
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                else
                {
                    fborder.FBOrderStatus = 3;  //狀態改失敗
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Facebook文章嗚--
        /*** 要FB文章嗚的訂單 ***/
        [HttpGet]
        public JsonResult GetFBOrder_ArticleCry(string Id)
        {

            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("e592d2a8-a1a7-4471-a648-0547c7a46cdd"));
                FBOrder fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 1).FirstOrDefault();
                if (fborder == null)
                {
                    fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 0).FirstOrDefault();
                }
                if (fborder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    fborder.FBOrderStatus = 1;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] FBOrderArray = new string[4];
                    FBOrderArray[0] = fborder.FBOrdernumber;
                    FBOrderArray[1] = fborder.Remains.ToString();
                    FBOrderArray[2] = fborder.Url;
                    FBOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
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
        /**** 更新FB文章嗚的訂單 ***/
        public JsonResult UpdateFBOrder_ArticleCry(string Id, string FBOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    fborder.FBOrderStatus = 2;  //狀態改已成功
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                else
                {
                    fborder.FBOrderStatus = 3;  //狀態改失敗
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Facebook文章怒--
        /*** 要FB文章嗚的訂單 ***/
        [HttpGet]
        public JsonResult GetFBOrder_ArticleAngry(string Id)
        {

            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("bffb9389-46f0-4bf4-a6ab-d4dcf77435c7"));
                FBOrder fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 1).FirstOrDefault();
                if (fborder == null)
                {
                    fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 0).FirstOrDefault();
                }
                if (fborder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    fborder.FBOrderStatus = 1;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] FBOrderArray = new string[4];
                    FBOrderArray[0] = fborder.FBOrdernumber;
                    FBOrderArray[1] = fborder.Remains.ToString();
                    FBOrderArray[2] = fborder.Url;
                    FBOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
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
        /**** 更新FB文章怒的訂單 ***/
        public JsonResult UpdateFBOrder_ArticleAngry(string Id, string FBOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    fborder.FBOrderStatus = 2;  //狀態改已成功
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                else
                {
                    fborder.FBOrderStatus = 3;  //狀態改失敗
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Facebook文章退讚--
        /**** 要FB文章退讚的訂單 ***/
        [HttpGet]
        public JsonResult GetFBOrder_RegainArticleLike(string Id)
        {
            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("0c020482-d76a-4213-b021-f8db0fe96489"));
                FBOrder fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 5).FirstOrDefault();
                if (fborder == null)
                {
                    fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 4).FirstOrDefault();
                }
                if (fborder != null)
                {
                    /*** 將訂單狀態改為收回中 ***/
                    fborder.FBOrderStatus = 5;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] FBOrderArray = new string[4];
                    FBOrderArray[0] = fborder.FBOrdernumber;
                    FBOrderArray[1] = fborder.FBOrderlist.Count().ToString();
                    FBOrderArray[2] = fborder.Url;
                    return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
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
        /**** 更新FB文章退讚的訂單 ***/
        public JsonResult UpdateFBOrder_RegainArticleLike(string Id, string FBOrdernumber, string status)
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    fborder.FBOrderStatus = 6;  //狀態改已收回
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --Facebook留言--
        /*** 要FB留言的訂單 ***/
        [HttpGet]
        public JsonResult GetFBOrder_FacebookMessage(string Id)
        {

            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("f686d184-884c-4aa7-9f26-f8118ba7f990"));
                FBOrder fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 1).FirstOrDefault();
                if (fborder == null)
                {
                    fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 0).FirstOrDefault();
                }
                if (fborder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    fborder.FBOrderStatus = 1;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] FBOrderArray = new string[4];
                    FBOrderArray[0] = fborder.FBOrdernumber;
                    FBOrderArray[1] = fborder.Remains.ToString();
                    FBOrderArray[2] = fborder.Url;
                    FBOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
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
        /**** 更新FB留言的訂單 ***/
        public JsonResult UpdateFBOrder_FacebookMessage(string Id, string FBOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    fborder.FBOrderStatus = 2;  //狀態改已成功
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                else
                {
                    fborder.FBOrderStatus = 3;  //狀態改失敗
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --粉絲專頁讚--
        /*** 要FB粉絲專頁讚的訂單 ****/
        [HttpGet]
        public JsonResult GetFBOrder_FansLike(string Id)
        {
            if(Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("b93e5ee4-f946-4bb0-ad6a-8f379e704802"));
                FBOrder fborder = fborderService.Get().Where(a => a.Productid == product.Productid).Where(x => x.FBOrderStatus == 1).OrderBy(o => o.Createdate).FirstOrDefault();
                if(fborder == null)
                {
                    fborder = fborderService.Get().Where(a => a.Productid == product.Productid).Where(x => x.FBOrderStatus == 0).OrderBy(o => o.Createdate).FirstOrDefault();
                }

                if(fborder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    fborder.FBOrderStatus = 1;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] FBOrderArray = new string[4];
                    FBOrderArray[0] = fborder.FBOrdernumber;
                    FBOrderArray[1] = fborder.Remains.ToString();
                    FBOrderArray[2] = fborder.Url;
                    FBOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
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
        /**** 更新FB粉絲專頁的訂單 ***/
        public JsonResult UpdateFBOrder_FansLike(string Id, string FBOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    fborder.FBOrderStatus = 2;  //狀態改已成功
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                else
                {
                    fborder.FBOrderStatus = 3;  //狀態改失敗
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --嵌入式留言--
        /*** 要FB嵌入式留言的訂單 ***/
        [HttpGet]
        public JsonResult GetFBOrder_EmbedMessage(string Id)
        {

            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("07408390-5f81-451a-9193-a93faaed1825"));
                FBOrder fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 1).FirstOrDefault();
                if (fborder == null)
                {
                    fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 0).FirstOrDefault();
                }
                if (fborder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    fborder.FBOrderStatus = 1;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] FBOrderArray = new string[4];
                    FBOrderArray[0] = fborder.FBOrdernumber;
                    FBOrderArray[1] = fborder.Remains.ToString();
                    FBOrderArray[2] = fborder.Url;
                    FBOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
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
        /**** 更新FB嵌入式留言的訂單 ***/
        public JsonResult UpdateFBOrder_EmbedMessage(string Id, string FBOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    fborder.FBOrderStatus = 2;  //狀態改已成功
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                else
                {
                    fborder.FBOrderStatus = 3;  //狀態改失敗
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --個人頁面追蹤--
        /*** 要FB個人頁面追蹤的訂單 ***/
        [HttpGet]
        public JsonResult GetFBOrder_Follow(string Id)
        {

            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("e16dfe59-2789-4598-9310-6334e5e7803c"));
                FBOrder fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 1).FirstOrDefault();
                if (fborder == null)
                {
                    fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 0).FirstOrDefault();
                }
                if (fborder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    fborder.FBOrderStatus = 1;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] FBOrderArray = new string[4];
                    FBOrderArray[0] = fborder.FBOrdernumber;
                    FBOrderArray[1] = fborder.Remains.ToString();
                    FBOrderArray[2] = fborder.Url;
                    FBOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
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
        /**** 更新FB個人頁面追蹤的訂單 ***/
        public JsonResult UpdateFBOrder_Follow(string Id, string FBOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    fborder.FBOrderStatus = 2;  //狀態改已成功
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                else
                {
                    fborder.FBOrderStatus = 3;  //狀態改失敗
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --直播--
        /*** 要FB直播的訂單 ***/
        [HttpGet]
        public JsonResult GetFBOrder_Live(string Id)
        {
            if (Id == "CloudControl_order")
            {
                Product product = productService.GetByID(Guid.Parse("78da7b19-f424-4efa-9691-45268100188d"));
                FBOrder fborder = fborderService.Get().OrderBy(a => a.Createdate).Where(x => x.Productid == product.Productid).Where(c => c.FBOrderStatus == 0).FirstOrDefault();
                if (fborder != null)
                {
                    /*** 將訂單狀態改為進行中 ***/
                    fborder.FBOrderStatus = 1;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    /*** 傳送訂單 ***/
                    string[] FBOrderArray = new string[4];
                    FBOrderArray[0] = fborder.FBOrdernumber;
                    FBOrderArray[1] = fborder.Remains.ToString();
                    FBOrderArray[2] = fborder.Url;
                    FBOrderArray[3] = (Now + 3600).ToString();
                    return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
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
        /**** 更新FB直播的訂單 ***/
        public JsonResult UpdateFBOrder_Live(string Id, string FBOrdernumber, string status = "failed")
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 抓該訂單
                if (status == "success")
                {
                    fborder.FBOrderStatus = 2;  //狀態改已成功
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                else
                {
                    fborder.FBOrderStatus = 3;  //狀態改失敗
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                }
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }             

        /***** 取得有資源群組的訂單 *****/
        [HttpGet]
        public JsonResult GetFBOrder_ResourceGroup(string Id)
        {
            if(Id == "CloudControl_order")
            {
                IEnumerable<FBOrder> fborders = fborderService.Get().Where(a => a.IsResourceGroup == true).OrderByDescending(o => o.Createdate);
                string[] FBOrderArray = new string[fborders.Count()];
                int i = 0;
                foreach (FBOrder fborder in fborders)
                {
                    FBOrderArray[i] = fborder.FBOrdernumber;
                    i++;
                }
                return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);                
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        /**** 刪除訂單資源群組 ****/
        [HttpGet]
        public JsonResult DeleteFBOrder_ResourceGroup(string Id, string FBOrdernumber)
        {
            if(Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();
                fborder.IsResourceGroup = false;
                fborderService.SpecificUpdate(fborder, new string[] { "IsResourceGroup" });
                fborderService.SaveChanges();               
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        /******* 取得直播網址 *****/
        [HttpGet]
        public JsonResult GetFBOrder_LiveUrl(string Id, string FBOrdernumber)
        {
            if(Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();
                return this.Json(fborder.Url, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        /***** 取得Useragent ******/
        public JsonResult GetFBMember_Useragent(string Id, string Account)
        {
            if(Id == "CloudControl_order")
            {
                FBMembers fbmember = fbmembersService.Get().Where(a => a.FB_Account == Account).FirstOrDefault();
                return this.Json(fbmember.Useragent, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }        

        /**** 抓取直播會員 ****/
        [HttpGet]
        public JsonResult GetLiveFBMember(string Id, int number, string FBOrdernumber)
        {
            if(Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 撈訂單
                Product product = productService.GetByID(fborder.Productid);    // 撈訂單的產品
                IEnumerable<FBMembers> fbmembers = fbmembersService.Get().Where(a => a.Lastdate <= Now).Where(p => p.Productid == product.Productid).Where(c => c.FBMembersLoginlog.FirstOrDefault().Status != 2).Where(d => d.Isdocker == 0).OrderBy(r => r.Lastdate);
                List<get_livelist> FBLiveList = new List<get_livelist>();
                if(fbmembers.Count() > number)
                {
                    foreach(FBMembers member in fbmembers.Take(number))
                    {
                        FBLiveList.Add(
                            new get_livelist
                            {
                                Account = member.FB_Account
                            }
                        );
                        /*** 更新下次互惠時間 ****/
                        member.Lastdate = Now + 20;
                        /*** 更新Docker狀態【0 : 關閉 , 1: 打開】 ****/
                        member.Isdocker = 1;
                        fbmembersService.SpecificUpdate(member, new string[] { "Lastdate", "Isdocker" });
                    }
                    fbmembersService.SaveChanges();
                    return this.Json(FBLiveList, JsonRequestBehavior.AllowGet);
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
        /**** 抓取直播會員詳細資料 ****/
        [HttpGet]
        public JsonResult GetLiveFBMemberDetail(string Id, string Account)
        {
            if (Id == "CloudControl_order")
            {
                FBMembers fbmember = fbmembersService.Get().Where(a => a.FB_Account == Account).FirstOrDefault();
                string Cookie = "";
                if(fbmember.Cookie != null)
                {
                    Cookie = fbmember.Cookie.Replace("True", "true").Replace("False", "false").Replace("name", "Name").Replace("value", "Value").Replace("path", "Path").Replace("domain", "Domain").Replace("secure", "Secure").Replace("httpOnly", "IsHttpOnly").Replace("expiry", "Expiry");
                    Cookie = Cookie.Replace("'", @"""");     // 將' 取代成 "
                }
                List<get_member> AccountDetail = new List<get_member>();
                AccountDetail.Add(
                    new get_member()
                    {
                        Memberid = fbmember.FBMemberid,
                        Account = fbmember.FB_Account,
                        Password = fbmember.FB_Password,
                        Useragent_phone = fbmember.Useragent,
                        Cookie = Cookie,
                    }
                );
                return this.Json(AccountDetail, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        /**** 更新直播會員 ***/
        [HttpPost]
        public JsonResult UpdateLiveFBMember(string Id, string FBOrdernumber, string Memberid, string Cookie, int AccountStatus)
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 撈訂單
                IEnumerable<FBOrderlist> old_fborderlist = fborderlistService.Get().Where(a => a.FBOrderid == fborder.FBOrderid);   // 撈完成人數
                FBMembers fbmembers = fbmembersService.GetByID(Guid.Parse(Memberid));   // 撈會員
                /*** 0 : 帳號需驗證，1 : 帳密輸入錯誤 or 密碼更改，2 : 成功執行，3 : 找不到讚的位置***/
                if (AccountStatus == 0)
                {
                    /**** 如果會員被鎖，則剩餘人數加回去 ****/
                    foreach(FBOrderlist orderlist in old_fborderlist)
                    {
                        if(orderlist.FBMemberid == Guid.Parse(Memberid))
                        {
                            fborder.Remains += 1;
                            fborderService.SpecificUpdate(fborder, new string[] { "Remains" });                           
                        }
                        fborderService.SaveChanges();
                    }
                    /*** 更新會員紀錄 ***/
                    FBMembersLoginlog fbmemebrsloginlog = fbmembersloginlogService.Get().Where(a => a.FBMemberid == fbmembers.FBMemberid).FirstOrDefault();
                    fbmemebrsloginlog.Status = 2;
                    fbmembersloginlogService.SpecificUpdate(fbmemebrsloginlog, new string[] { "Stauts" });
                    fbmembersloginlogService.SaveChanges();
                    return this.Json("Success");
                }
                else if (AccountStatus == 1)
                {
                    /*** 更新會員紀錄 ***/
                    FBMembersLoginlog fbmemebrsloginlog = fbmembersloginlogService.Get().Where(a => a.FBMemberid == fbmembers.FBMemberid).FirstOrDefault();
                    fbmemebrsloginlog.Status = 2;
                    fbmembersloginlogService.SpecificUpdate(fbmemebrsloginlog, new string[] { "Stauts" });
                    fbmembersloginlogService.SaveChanges();
                    return this.Json("Success");
                }
                else if (AccountStatus == 2)
                {
                    /*** 更新會員的Cookie *****/
                    fbmembers.Cookie = Cookie;
                    fbmembersService.SpecificUpdate(fbmembers, new string[] { "Cookie" });
                    fbmembersService.SaveChanges();                    
                    /*** 改訂單剩餘人數 ***/
                    fborder.Remains -= 1;
                    fborderService.SpecificUpdate(fborder, new string[] { "Remains" });
                    fborderService.SaveChanges();
                    FBOrderlist fborderlist = new FBOrderlist();
                    fborderlist.FBOrderlistid = Guid.NewGuid();
                    fborderlist.FBMemberid = Guid.Parse(Memberid);
                    fborderlist.FBAccount = fbmembers.FB_Account;
                    fborderlist.FBOrderid = fborder.FBOrderid;
                    fborderlist.Createdate = dt_tw();
                    fborderlist.Updatedate = dt_tw();
                    fborderlistService.Create(fborderlist);
                    fborderlistService.SaveChanges();
                    return this.Json("Success");
                }
                else
                {
                    /**** 寫入TXT檔 *****/
                    using (StreamWriter sw = new StreamWriter(txt_filepath, true))
                    {
                        sw.Write("FBLive:" + Memberid);
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
        #region --要帳號--
        [HttpGet]
        public JsonResult GetFBMember(string Id, int number, string FBOrdernumber)
        {
            if(Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 撈訂單
                IEnumerable<FBOrderlist> fborderlist = fborderlistService.Get().Where(a => a.FBOrder.FBOrdernumber == FBOrdernumber);   // 撈訂單的完成列表
                IEnumerable<FBOrder> old_fborder = fborderService.Get().Where(c => c.Productid == fborder.Productid).Where(a => a.FBOrdernumber != FBOrdernumber).Where(x => x.Url == fborder.Url);  // 撈所有訂單裡網址為此訂單及產品為此訂單的資料
                Product product = productService.GetByID(fborder.Productid);    // 撈此訂單所需的產品
                List<get_old_member> MemberList = new List<get_old_member>();                
                /**** 先排除這張訂單的完成訂單裡的人 ****/
                if(fborderlist != null)
                {
                    foreach(FBOrderlist list in fborderlist)
                    {
                        string Account = Regex.Replace(list.FBMembers.FB_Account, @"[^a-z||A-Z||@||.||0-9]", "").Replace(" ", "");         // 保留A-Z、a-z、0-9、小老鼠、小數點，其餘取代空值
                        MemberList.Add(
                        new get_old_member
                        {
                            Account = Account
                        });
                    }
                }
                /*** 將同網址的訂單的完成訂單裡的人排除掉 ****/
                if(old_fborder != null)
                {
                    foreach (FBOrder thisold_fborder in old_fborder)
                    {
                        IEnumerable<FBOrderlist> old_fborderlist = fborderlistService.Get().Where(a => a.FBOrderid == thisold_fborder.FBOrderid);                 
                        foreach (FBOrderlist thisold_fborderlist in old_fborderlist)
                        {
                            string Account = Regex.Replace(thisold_fborderlist.FBMembers.FB_Account, @"[^a-z||A-Z||@||.||0-9]", "").Replace(" ", "");         // 保留A-Z、a-z、0-9、小老鼠、小數點，其餘取代空值
                            MemberList.Add(
                                new get_old_member
                                {
                                    Account = Account
                                }
                            );
                        }
                    }
                }
                List<get_member> AccountList = new List<get_member>();

                //IQueryable<FBMembers> FBMembers = fbmembersService.Get().Where(a => a.Lastdate <= 0).Where(c => c.Productid == product.Productid).Where(x => x.FBMembersLoginlog.FirstOrDefault().Status != 2).Where(x => x.Productid == fborder.Productid);
                IQueryable<FBMembers> FBMembers = fbmembersService.Get().Where(x => x.FBMembersLoginlog.FirstOrDefault().Status != 2).Where(c => c.Productid == product.Productid).Where(x => x.Productid == fborder.Productid).OrderBy(r => Guid.NewGuid());
                if (FBMembers != null)
                {                    
                    foreach (FBMembers Member in FBMembers)
                    {
                        bool used = false;
                        int loop;
                        for (loop = 0; loop < MemberList.Count(); loop++)
                        {
                            string Account = Regex.Replace(Member.FB_Account, @"[^a-z||A-Z||@||.||0-9]", "").Replace(" ", "");         // 保留A-Z、a-z、0-9、小老鼠、小數點，其餘取代空值
                            if (Account.Contains(MemberList[loop].Account))
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
                            if (product.Productid == Guid.Parse("f686d184-884c-4aa7-9f26-f8118ba7f990") || product.Productid == Guid.Parse("07408390-5f81-451a-9193-a93faaed1825"))
                            {
                                /**** 撈取該訂單的留言類別 ****/
                                Guid CategortMessageid = categorymessageService.GetByID(fborder.Categoryid).Categoryid;
                                /*** 撈取該留言類別底下的留言 ***/
                                int messageConunt = messageService.Get().Where(a => a.Categoryid == fborder.Categoryid).Count();    // 該留言類別底下的留言數量
                                IEnumerable<Message> message = messageService.Get().Where(a => a.Categoryid == fborder.Categoryid).OrderBy(o => Guid.NewGuid());

                                AccountList.Add(
                                    new get_member
                                    {
                                        Memberid = Member.FBMemberid,
                                        Account = Member.FB_Account,
                                        Password = Member.FB_Password,
                                        Name = Member.FB_Name,
                                        Useragent_phone = Member.Useragent,
                                        Cookie = Cookie,
                                        Duedate = (Now + 3600),
                                        Message = message.FirstOrDefault().MessageName,
                                        UserDataUrl = Member.UserDataUrl,
                                        Mega_Account = Member.Mega_Account,
                                        Mega_Password = Member.Mega_Password
                                    }
                                );
                            }
                            else
                            {
                                AccountList.Add(
                                    new get_member
                                    {
                                        Memberid = Member.FBMemberid,
                                        Account = Member.FB_Account,
                                        Password = Member.FB_Password,
                                        Name = Member.FB_Name,
                                        Useragent_phone = Member.Useragent,
                                        Cookie = Cookie,
                                        Duedate = (Now + 3600),
                                        UserDataUrl = Member.UserDataUrl,
                                        Mega_Account = Member.Mega_Account,
                                        Mega_Password = Member.Mega_Password
                                    }
                                );
                            }
                        }
                        else
                        {
                            used = false;
                        }
                        
                    }
                    //return this.Json(AccountList, JsonRequestBehavior.AllowGet);
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
                            FBMembers Member = fbmembersService.GetByID(entity.Memberid);
                            Member.Lastdate = Now + 650000;
                            fbmembersService.SpecificUpdate(Member, new string[] { "Lastdate" });
                        }
                        fbmembersService.SaveChanges();
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
        public JsonResult UpdateFBMember(string Id, string FBOrdernumber, string Memberid, string Cookie, string UserDataUrl, string Mega_Account, string Mega_Password, int AccountStatus)
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();
                FBMembers fbmembers = fbmembersService.GetByID(Guid.Parse(Memberid));

                /*** 是否舊Cookie 【0:舊Cookie 1:更新Cookie】 *****/
                fbmembers.Isnew = 1;
                fbmembersService.SpecificUpdate(fbmembers, new string[] { "Isnew" });
                fbmembersService.SaveChanges();
                /**** 執行成本 *****/
                Product product = productService.GetByID(fborder.Productid);
                /*** 0 : 帳號需驗證，1 : 帳密輸入錯誤 or 密碼更改，2 : 成功執行，3 : 找不到讚的位置***/
                if(AccountStatus == 0)
                {
                    /*** 更新會員紀錄 ***/
                    FBMembersLoginlog fbmemebrsloginlog = fbmembersloginlogService.Get().Where(a => a.FBMemberid == fbmembers.FBMemberid).FirstOrDefault();
                    if (fbmemebrsloginlog != null)
                    {
                        fbmemebrsloginlog.Status = 2;
                        fbmembersloginlogService.SpecificUpdate(fbmemebrsloginlog, new string[] { "Status" });
                    }
                    else
                    {
                        FBMembersLoginlog newlog = new FBMembersLoginlog();
                        newlog.FBMemberid = fbmembers.FBMemberid;
                        newlog.Createdate = DateTime.Now;
                        newlog.Status = 2;
                        fbmembersloginlogService.Create(newlog);
                    }
                    fbmembersloginlogService.SaveChanges();
                    /***** 新增完成名單 ****/
                    FBOrderlist fborderlist = new FBOrderlist();
                    fborderlist.FBOrderlistid = Guid.NewGuid();
                    fborderlist.FBMemberid = Guid.Parse(Memberid);
                    fborderlist.FBAccount = fbmembers.FB_Account;
                    fborderlist.FBOrderid = fborder.FBOrderid;
                    fborderlist.Createdate = dt_tw();
                    fborderlist.Updatedate = dt_tw();
                    fborderlistService.Create(fborderlist);
                    fborderlistService.SaveChanges();
                    /**** 更新成本 *****/
                    fborder.CostRun += Convert.ToDouble(fborder.ProductCost); //運行成本
                    fborder.CostAccount += Convert.ToDouble(fbmembers.AccountCost); //帳號成本
                    fborderService.SpecificUpdate(fborder, new string[] { "CostRun", "CostAccount" });
                    fborderService.SaveChanges();
                    return this.Json("Success");                
                }
                else if(AccountStatus == 1)
                {
                    /*** 更新會員紀錄 ***/
                    FBMembersLoginlog fbmemebrsloginlog = fbmembersloginlogService.Get().Where(a => a.FBMemberid == fbmembers.FBMemberid).FirstOrDefault();
                    if (fbmemebrsloginlog != null)
                    {
                        fbmemebrsloginlog.Status = 1;
                        fbmembersloginlogService.SpecificUpdate(fbmemebrsloginlog, new string[] { "Status" });
                    }
                    else
                    {
                        FBMembersLoginlog newlog = new FBMembersLoginlog();
                        newlog.FBMemberid = fbmembers.FBMemberid;
                        newlog.Createdate = DateTime.Now;
                        newlog.Status = 1;
                        fbmembersloginlogService.Create(newlog);
                    }
                    fbmembersloginlogService.SaveChanges();
                    /**** 新增完成名單 *****/
                    FBOrderlist fborderlist = new FBOrderlist();
                    fborderlist.FBOrderlistid = Guid.NewGuid();
                    fborderlist.FBMemberid = Guid.Parse(Memberid);
                    fborderlist.FBAccount = fbmembers.FB_Account;
                    fborderlist.FBOrderid = fborder.FBOrderid;
                    fborderlist.Createdate = dt_tw();
                    fborderlist.Updatedate = dt_tw();
                    fborderlistService.Create(fborderlist);
                    fborderlistService.SaveChanges();
                    /**** 更新成本 *****/
                    fborder.CostRun += Convert.ToDouble(fborder.ProductCost); //運行成本
                    fborder.CostAccount += Convert.ToDouble(fbmembers.AccountCost); //帳號成本
                    fborderService.SpecificUpdate(fborder, new string[] { "CostRun", "CostAccount" });
                    fborderService.SaveChanges();
                    return this.Json("Success");
                }
                else if (AccountStatus == 2)
                {
                    /*** 更新會員的Cookie *****/
                    fbmembers.Cookie = Cookie;
                    /*** 更新會員的UserData Url *****/
                    fbmembers.UserDataUrl = UserDataUrl;
                    /*** 更新會員的Mega_Account、Mega_Password ***/
                    fbmembers.Mega_Account = Mega_Account;
                    fbmembers.Mega_Password = Mega_Password;
                    fbmembersService.SpecificUpdate(fbmembers, new string[] { "Cookie", "UserDataUrl", "Mega_Account", "Mega_Password" });
                    fbmembersService.SaveChanges();
                    /*** 更新會員紀錄 ***/
                    FBMembersLoginlog fbmemebrsloginlog = fbmembersloginlogService.Get().Where(a => a.FBMemberid == fbmembers.FBMemberid).FirstOrDefault();
                    if (fbmemebrsloginlog != null)
                    {
                        fbmemebrsloginlog.Status = 1;
                        fbmembersloginlogService.SpecificUpdate(fbmemebrsloginlog, new string[] { "Status" });
                    }
                    else
                    {
                        FBMembersLoginlog newlog = new FBMembersLoginlog();
                        newlog.FBMemberid = fbmembers.FBMemberid;
                        newlog.Createdate = DateTime.Now;
                        newlog.Status = 1;
                        fbmembersloginlogService.Create(newlog);
                    }
                    fbmembersloginlogService.SaveChanges();
                    /*** 改訂單剩餘人數 ***/
                    fborder.Remains -= 1;
                    /**** 更新成本 *****/
                    fborder.CostRun += Convert.ToDouble(fborder.ProductCost); //運行成本
                    fborderService.SpecificUpdate(fborder, new string[] { "CostRun", "Remains" });
                    fborderService.SaveChanges();
                    /***** 新增完成名單 ****/
                    FBOrderlist fborderlist = new FBOrderlist();
                    fborderlist.FBOrderlistid = Guid.NewGuid();
                    fborderlist.FBMemberid = Guid.Parse(Memberid);
                    fborderlist.FBAccount = fbmembers.FB_Account;
                    fborderlist.FBOrderid = fborder.FBOrderid;
                    fborderlist.Createdate = dt_tw();
                    fborderlist.Updatedate = dt_tw();
                    fborderlistService.Create(fborderlist);
                    fborderlistService.SaveChanges();
                    return this.Json("Success");
                }
                else
                {
                    /*** 更新會員的Cookie *****/
                    fbmembers.Cookie = Cookie;
                    /*** 更新會員的UserData Url *****/
                    fbmembers.UserDataUrl = UserDataUrl;
                    /*** 更新會員的Mega_Account、Mega_Password ***/
                    fbmembers.Mega_Account = Mega_Account;
                    fbmembers.Mega_Password = Mega_Password;
                    fbmembersService.SpecificUpdate(fbmembers, new string[] { "Cookie", "UserDataUrl", "Mega_Account", "Mega_Password" });
                    fbmembersService.SaveChanges();
                    return this.Json("Success");
                }
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --取得收回的帳號名單--
        [HttpGet]
        public JsonResult GetFBRegainMember(string Id, string FBOrdernumber)
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 取得訂單
                IEnumerable<FBOrderlist> fborderlist = fborderlistService.Get().Where(a => a.FBOrderid == fborder.FBOrderid);
                List<get_member> AccountList = new List<get_member>();
                int fborderlistCount = fborderlist.Count();

                foreach (FBOrderlist fbordertemp in fborderlist)
                {
                    string Cookie = fbordertemp.FBMembers.Cookie.Replace("True", "true").Replace("False", "false").Replace("name", "Name").Replace("value", "Value").Replace("path", "Path").Replace("domain", "Domain").Replace("secure", "Secure").Replace("httpOnly", "IsHttpOnly").Replace("expiry", "Expiry");
                    Cookie = Cookie.Replace("'", @"""");     // 將' 取代成 "
                    AccountList.Add(
                        new get_member
                        {
                            Memberid = Guid.Parse(fbordertemp.FBMemberid.ToString()),
                            Account = fbordertemp.FBAccount,
                            Password = fbordertemp.FBMembers.FB_Password,
                            Useragent_phone = fbordertemp.FBMembers.Useragent,
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
        public JsonResult UpdateRegainFBMember(string Id, string FBOrdernumber, string Memberid, string Cookie, int AccountStatus)
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();
                FBMembers fbmembers = fbmembersService.GetByID(Guid.Parse(Memberid));
                /*** 0 : 帳號需驗證，1 : 帳密輸入錯誤 or 密碼更改，2 : 成功執行，3 : 找不到讚的位置***/
                if (AccountStatus == 0)
                {
                    /**** 寫入TXT檔 *****/
                    using (StreamWriter sw = new StreamWriter(txt_filepath, true))
                    {
                        sw.Write("CloudControl訂單問題回報 訂單編號:" + fborder.FBOrdernumber + "會員帳號:" + fbmembers.FB_Account + "登入有問題(帳號需驗證)");
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
                        sw.Write("CloudControl訂單問題回報 訂單編號:" + fborder.FBOrdernumber + "會員帳號:" + fbmembers.FB_Account + "登入有問題(密碼更改or帳密錯誤)");
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
                        sw.Write("CloudControl訂單問題回報 訂單編號:" + fborder.FBOrdernumber + "會員帳號:" + fbmembers.FB_Account + "執行成功");
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
                        sw.Write("CloudControl訂單問題回報 訂單編號:" + fborder.FBOrdernumber + " 會員帳號:" + fbmembers.FB_Account + "找不到執行的位置");
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
        


        /****************************更新版****************************/
        #region --取得訂單--
        [HttpGet]
        public JsonResult GetFBOrder(string Id)
        {
            Guid Productid = Guid.Parse("f9dd03ee-ecd7-4514-94c6-2ea7d72d931c");
            List<OrderStatus> OrderStatus = new List<Controllers.OrderStatus>();
            if(Id == "CloudControl_order")
            {
                /**** 運行中訂單 ***/
                IEnumerable<FBOrder> running_fborders = fborderService.Get().Where(p => p.Productid == Productid).Where(a => a.FBOrderStatus == 1).OrderBy(o => o.Createdate).ToList();
                if(running_fborders != null)
                {
                    foreach (FBOrder running_fborder in running_fborders)
                    {
                        bool Status = true;
                        /**** 虛擬機運行狀況 ***/
                        IEnumerable<FBVMLog> fbvmlogs = fbvmlogService.Get().Where(a => a.FBOrderid == running_fborder.FBOrderid);
                        foreach (FBVMLog fbvmlog in fbvmlogs)
                        {
                            if (fbvmlog.Status == "Running")
                            {
                                Status = false;
                            }
                        }

                        if (Status == true)
                        {                            
                            OrderStatus.Add(
                                new Controllers.OrderStatus()
                                {
                                    Status = "Success",
                                    Productid = Guid.Parse(running_fborder.Productid.ToString()),
                                    OrderDetail = new OrderDetail()
                                    {
                                        FBOrdernumber = running_fborder.FBOrdernumber,
                                        Remain = Convert.ToInt32(running_fborder.Remains)
                                    }
                                }
                            );
                        }
                        if(OrderStatus.Count() > 0)
                        {
                            return this.Json(OrderStatus, JsonRequestBehavior.AllowGet);
                        }                        
                    }
                }                
                /**** 等待中訂單 ***/
                IEnumerable<FBOrder> waiting_fborders = fborderService.Get().Where(a => a.FBOrderStatus == 0).Where(p => p.Productid == Productid).OrderBy(o => o.Createdate).ToList();
                if(waiting_fborders != null)
                {
                    foreach (FBOrder waiting_fborder in waiting_fborders)
                    {
                        bool Status = false;
                        foreach (FBOrder running_fborder in running_fborders)
                        {
                            if(waiting_fborder.Productid == running_fborder.Productid)
                            {
                                Status = true;
                            }
                        }
                        if(Status == false)
                        {
                            //waiting_fborder.FBOrderStatus = 0; // <-- 測試 -->
                            waiting_fborder.FBOrderStatus = 1;
                            fborderService.SpecificUpdate(waiting_fborder, new string[] { "FBOrderStatus" });
                            fborderService.SaveChanges();
                            OrderStatus.Add(
                                new Controllers.OrderStatus()
                                {
                                    Status = "Success",
                                    Productid = Guid.Parse(waiting_fborder.Productid.ToString()),
                                    OrderDetail = new OrderDetail()
                                    {
                                        FBOrdernumber = waiting_fborder.FBOrdernumber,
                                        Remain = Convert.ToInt32(waiting_fborder.Remains)
                                    }
                                }
                            );
                            break;
                        }
                    }
                    if (OrderStatus.Count() > 0)
                    {
                        return this.Json(OrderStatus.Take(1), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        OrderStatus.Add(
                            new Controllers.OrderStatus()
                            {
                                Status = "no_order"
                            }
                        );
                        return this.Json(OrderStatus, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    OrderStatus.Add(
                        new Controllers.OrderStatus()
                        {
                            Status = "no_order"
                        }
                    );
                    return this.Json(OrderStatus, JsonRequestBehavior.AllowGet);
                }                                              
            }
            else
            {
                OrderStatus.Add(
                    new Controllers.OrderStatus()
                    {
                        Status = "Error"
                    }
                );
                return this.Json(OrderStatus, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --更新訂單--
        [HttpGet]
        public JsonResult UpdateFBOrder(string Id, string FBOrdernumber, string Status = "failed")
        {
            List<UpdateData> UpdateData = new List<Controllers.UpdateData>();
            if (Id == "CloudControl_order")
            {
                if(Status == "Success")
                {
                    FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();
                    fborder.FBOrderStatus = 2;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    UpdateData.Add(
                        new Controllers.UpdateData()
                        {
                            Status = "Success"
                        }
                    );
                    return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();
                    fborder.FBOrderStatus = 3;
                    fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                    fborderService.SaveChanges();
                    UpdateData.Add(
                        new Controllers.UpdateData()
                        {
                            Status = "Success"
                        }
                    );
                    return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                UpdateData.Add(
                    new Controllers.UpdateData()
                    {
                        Status = "Error"
                    }
                );
                return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --取得訂單網址--
        /******* 取得直播網址 *****/
        [HttpGet]
        public JsonResult GetFBOrderUrl(string Id, string FBOrdernumber)
        {
            List<OrderUrlStatus> OrderUrlStatus = new List<Controllers.OrderUrlStatus>();
            List<OrderUrl> OrderUrl = new List<Controllers.OrderUrl>();        
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();
                OrderUrl.Add(
                    new Controllers.OrderUrl()
                    {
                        Url = fborder.Url
                    }
                );
                OrderUrlStatus.Add(
                    new Controllers.OrderUrlStatus()
                    {
                        Status = "Success",
                        OrderUrl = OrderUrl
                    }
                );
                return this.Json(OrderUrlStatus, JsonRequestBehavior.AllowGet);
            }
            else
            {
                OrderUrlStatus.Add(
                    new Controllers.OrderUrlStatus()
                    {
                        Status = "Error",
                        OrderUrl = OrderUrl
                    }
                );
                return this.Json(OrderUrlStatus, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --取得帳號--
        [HttpGet]
        public JsonResult GetFBAccount(string Id, int number, string FBOrdernumber)
        {
            List<AccountStatus> AccountStatus = new List<Controllers.AccountStatus>();
            List<GetAccount> AccountList = new List<GetAccount>();
            if(Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();   // 撈訂單
                Product product = productService.GetByID(fborder.Productid);    // 撈產品
                /**** 直播訂單 ****/
                if(fborder.Productid == Guid.Parse("78da7b19-f424-4efa-9691-45268100188d"))
                {
                    IEnumerable<FBMembers> fbmembers = fbmembersService.Get().Where(a => a.Productid == product.Productid).Where(x => x.FBMembersLoginlog.FirstOrDefault().Status != 2).Where(d => d.Isdocker == 0).Where(x => x.Lastdate <= Now).OrderBy(o => o.Lastdate);
                    if (fbmembers != null)
                    {
                        if(fbmembers.Count() >= number)
                        {                            
                            foreach (FBMembers fbmember in fbmembers.Take(number))
                            {
                                /**** 加上下次使用時間 ***/
                                fbmember.Lastdate = Now + 0;
                                fbmembersService.SpecificUpdate(fbmember, new string[] { "Lastdate" });
                                AccountList.Add(
                                    new Controllers.GetAccount()
                                    {
                                        Memberid = fbmember.FBMemberid.ToString()
                                    }
                                );
                            }
                            fbmembersService.SaveChanges();
                            AccountStatus.Add(
                                new Controllers.AccountStatus()
                                {
                                    Status = "Success",
                                    List = AccountList
                                }
                            );
                            return this.Json(AccountStatus, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            AccountStatus.Add(
                                new Controllers.AccountStatus()
                                {
                                    Status = "no_account"
                                }
                            );

                            /*** 將訂單改為等待中，重新排隊 ***/
                            fborder.FBOrderStatus = 0;
                            fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                            fborderService.SaveChanges();
                            return this.Json(AccountStatus, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        AccountStatus.Add(
                            new Controllers.AccountStatus()
                            {
                                Status = "no_account"
                            }
                        );
        
                        /*** 將訂單改為等待中，重新排隊 ***/
                        fborder.FBOrderStatus = 0;
                        fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                        fborderService.SaveChanges();
                        return this.Json(AccountStatus, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {                    
                    IEnumerable<FBOrderlist> fborderlist = fborderlistService.Get().Where(a => a.FBOrder.FBOrdernumber == FBOrdernumber);   // 撈訂單的完成列表
                    IEnumerable<FBOrder> old_fborder = fborderService.Get().Where(c => c.Productid == fborder.Productid).Where(a => a.FBOrdernumber != FBOrdernumber).Where(x => x.Url == fborder.Url);  // 撈所有訂單裡網址為此訂單及產品為此訂單的資料
                    List<GetOldAccount> MemberList = new List<GetOldAccount>();
                    /**** 先排除這張訂單的完成訂單裡的人 ****/
                    if (fborderlist != null)
                    {
                        foreach (FBOrderlist list in fborderlist)
                        {
                            MemberList.Add(
                            new GetOldAccount
                            {
                                Memberid = Guid.Parse(list.FBMemberid.ToString())
                            });
                        }
                    }

                    /*** 將同網址的訂單的完成訂單裡的人排除掉 ****/
                    if (old_fborder != null)
                    {
                        foreach (FBOrder thisold_fborder in old_fborder)
                        {
                            IEnumerable<FBOrderlist> old_fborderlist = fborderlistService.Get().Where(a => a.FBOrderid == thisold_fborder.FBOrderid);
                            foreach (FBOrderlist thisold_fborderlist in old_fborderlist)
                            {
                                MemberList.Add(
                                    new GetOldAccount
                                    {
                                        Memberid = Guid.Parse(thisold_fborderlist.FBMemberid.ToString())
                                    }
                                );
                            }
                        }
                    }
                    /*** 撈人 **/                                        
                    IQueryable<FBMembers> FBMembers = fbmembersService.Get().Where(x => x.FBMembersLoginlog.FirstOrDefault().Status != 2).Where(a => a.Isnew == 1).Where(x => x.Productid == fborder.Productid).OrderBy(r => Guid.NewGuid());
                    if (FBMembers != null)
                    {
                        List<GetAccount> AccountTemp = new List<GetAccount>();
                        foreach (FBMembers fbmember in FBMembers)
                        {
                            bool used = false;
                            int loop;
                            for (loop = 0; loop < MemberList.Count(); loop++)
                            {
                                if (fbmember.FBMemberid.Equals(MemberList[loop].Memberid))
                                {
                                    used = true;
                                }
                            }

                            if (used == false)
                            {
                                AccountList.Add(
                                    new GetAccount()
                                    {
                                        Memberid = fbmember.FBMemberid.ToString()
                                    }
                                );
                            }                            
                        }
                        if (AccountList.Count() >= number)
                        {
                            foreach (GetAccount entity in AccountList.Take(number))
                            {
                                AccountTemp.Add(
                                    new GetAccount()
                                    {
                                        Memberid = entity.Memberid
                                    }
                                );
                                Guid FBMemberid = Guid.Parse(entity.Memberid);
                                FBMembers member = fbmembersService.GetByID(FBMemberid);
                                member.Lastdate = Now + 0;
                                fbmembersService.SpecificUpdate(member, new string[] { "Lastdate" });
                            }
                            

                            fbmembersService.SaveChanges();
                            AccountStatus.Add(
                                new Controllers.AccountStatus()
                                {
                                    Status = "Success",
                                    List = AccountTemp
                                }
                            );
                            return this.Json(AccountStatus.Take(number), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            AccountStatus.Add(
                                new Controllers.AccountStatus()
                                {
                                    Status = "no_account"
                                }
                            );

                            /*** 將訂單改為等待中，重新排隊 ***/
                            fborder.FBOrderStatus = 0;
                            fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                            fborderService.SaveChanges();
                            return this.Json(AccountStatus, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        AccountStatus.Add(
                            new Controllers.AccountStatus()
                            {
                                Status = "no_account"
                            }
                        );
                        /*** 將訂單改為等待中，重新排隊 ***/
                        fborder.FBOrderStatus = 0;
                        fborderService.SpecificUpdate(fborder, new string[] { "FBOrderStatus" });
                        fborderService.SaveChanges();
                        return this.Json(AccountStatus, JsonRequestBehavior.AllowGet);
                    }                
                }
            }
            else
            {
                AccountStatus.Add(
                    new Controllers.AccountStatus()
                    {
                        Status = "Error"
                    }
                );
                return this.Json(AccountStatus, JsonRequestBehavior.AllowGet);
            }
            
        }
        #endregion
        #region --取得帳號詳細資料--
        [HttpGet]
        public JsonResult GetFBAccountDetail(string Id, string Memberid, string FBOrdernumber)
        {
            List<AccountDetailStatus> AccountDetailStatus = new List<Controllers.AccountDetailStatus>();
            List<GetAccountDetail> AccountDetailList = new List<GetAccountDetail>();
            if(Id == "CloudControl_order")
            {
                Guid FBMemberid = Guid.Parse(Memberid);
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();
                FBMembers fbmember = fbmembersService.GetByID(FBMemberid);
                Product product = productService.GetByID(fborder.Productid);
                /*** 留言訂單 ***/
                if (fborder.Productid == Guid.Parse("f686d184-884c-4aa7-9f26-f8118ba7f990") || fborder.Productid == Guid.Parse("07408390-5f81-451a-9193-a93faaed1825"))
                {
                    /**** 撈訂單的留言類別 ****/
                    Guid CategortMessageid = categorymessageService.GetByID(fborder.Categoryid).Categoryid;
                    /*** 撈留言類別底下的留言 ***/
                    int messageConunt = messageService.Get().Where(a => a.Categoryid == fborder.Categoryid).Count();    // 該留言類別底下的留言數量
                    Message[] message = messageService.Get().Where(a => a.Categoryid == fborder.Categoryid).ToArray();
                    Random crandom = new Random();
                    int rand = crandom.Next(0, messageConunt - 1);
                    AccountDetailList.Add(
                        new GetAccountDetail()
                        {
                            Account = fbmember.FB_Account,
                            Memberid = fbmember.FBMemberid,
                            Password = fbmember.FB_Password,
                            Name = fbmember.FB_Name,
                            Cookie = fbmember.Cookie,
                            Message = message[rand].MessageName,
                            Useragent_phone = fbmember.Useragent
                        }
                    );
                    AccountDetailStatus.Add(
                        new Controllers.AccountDetailStatus()
                        {
                            Status = "Success",
                            AccountDetailList = AccountDetailList
                        }
                    );
                    return this.Json(AccountDetailStatus, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    AccountDetailList.Add(
                        new GetAccountDetail()
                        {
                            Account = fbmember.FB_Account,
                            Memberid = fbmember.FBMemberid,
                            Password = fbmember.FB_Password,
                            Name = fbmember.FB_Name,                           
                            Cookie = fbmember.Cookie,
                            Useragent_phone = fbmember.Useragent
                        }
                    );
                    AccountDetailStatus.Add(
                        new Controllers.AccountDetailStatus()
                        {
                            Status = "Success",
                            AccountDetailList = AccountDetailList
                        }
                    );
                    return this.Json(AccountDetailStatus, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                AccountDetailStatus.Add(
                    new Controllers.AccountDetailStatus()
                    {
                        Status = "Error"
                    }
                );
                return this.Json(AccountDetailStatus, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --更新會員--
        [HttpPost]
        public JsonResult UpdateFBAccount(string Id, string FBOrdernumber, string Memberid, string Facebookid, string Cookie, string UserDataUrl, int AccountStatus)
        {
            List<UpdateData> UpdateData = new List<Controllers.UpdateData>();
            if(Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();
                FBMembers fbmembers = fbmembersService.GetByID(Guid.Parse(Memberid));
                /*** AccountStatus 回傳狀態【0 : 帳號需驗證，1 : 帳密輸入錯誤 or 密碼更改，2 : 成功執行，3 : 找不到讚的位置】***/
                /*** 是否舊Cookie 【0:舊Cookie 1:更新Cookie】，更新會員FB連結 *****/

                /*** 如果找不到讚的位置 ***/
                if (AccountStatus != 3)
                {
                    fbmembers.Isnew = 1;
                }
                else
                {
                    fbmembers.Isnew = 0;
                }                
                fbmembers.Facebooklink = "https://www.facebook.com/profile.php?id=" + Facebookid;
                fbmembersService.SpecificUpdate(fbmembers, new string[] { "Isnew", "Facebooklink" });
                fbmembersService.SaveChanges();
                
                if (AccountStatus == 0)
                {
                    /*** 更新會員紀錄 ***/
                    FBMembersLoginlog fbmemebrsloginlog = fbmembersloginlogService.Get().Where(a => a.FBMemberid == fbmembers.FBMemberid).FirstOrDefault();
                    if (fbmemebrsloginlog != null)
                    {
                        fbmemebrsloginlog.Status = 2;
                        fbmembersloginlogService.SpecificUpdate(fbmemebrsloginlog, new string[] { "Status" });
                    }
                    else
                    {
                        FBMembersLoginlog newlog = new FBMembersLoginlog();
                        newlog.FBMemberid = fbmembers.FBMemberid;
                        newlog.Createdate = DateTime.Now;
                        newlog.Status = 2;
                        fbmembersloginlogService.Create(newlog);
                    }
                    fbmembersloginlogService.SaveChanges();
                    /**** 更新成本 *****/
                    fborder.CostRun += Convert.ToDouble(fborder.ProductCost); //運行成本
                    fborder.CostAccount += Convert.ToDouble(fbmembers.AccountCost); //帳號成本
                    fborderService.SpecificUpdate(fborder, new string[] { "CostRun", "CostAccount" });
                    fborderService.SaveChanges();
                    /**** 更新完成列表 ****/
                    FBOrderlist fborderlist = new FBOrderlist();
                    fborderlist.FBOrderlistid = Guid.NewGuid();
                    fborderlist.FBMemberid = Guid.Parse(Memberid);
                    fborderlist.FBAccount = fbmembers.FB_Account;
                    fborderlist.FBOrderid = fborder.FBOrderid;
                    fborderlist.Createdate = dt_tw();
                    fborderlist.Updatedate = dt_tw();
                    fborderlistService.Create(fborderlist);
                    fborderlistService.SaveChanges();
                }
                else if(AccountStatus == 1)
                {
                    /*** 更新會員紀錄 ***/
                    FBMembersLoginlog fbmemebrsloginlog = fbmembersloginlogService.Get().Where(a => a.FBMemberid == fbmembers.FBMemberid).FirstOrDefault();
                    if (fbmemebrsloginlog != null)
                    {
                        fbmemebrsloginlog.Status = 2;
                        fbmembersloginlogService.SpecificUpdate(fbmemebrsloginlog, new string[] { "Status" });
                    }
                    else
                    {
                        FBMembersLoginlog newlog = new FBMembersLoginlog();
                        newlog.FBMemberid = fbmembers.FBMemberid;
                        newlog.Createdate = DateTime.Now;
                        newlog.Status = 2;
                        fbmembersloginlogService.Create(newlog);
                    }
                    fbmembersloginlogService.SaveChanges();
                    /**** 更新完成列表 ****/
                    FBOrderlist fborderlist = new FBOrderlist();
                    fborderlist.FBOrderlistid = Guid.NewGuid();
                    fborderlist.FBMemberid = Guid.Parse(Memberid);
                    fborderlist.FBAccount = fbmembers.FB_Account;
                    fborderlist.FBOrderid = fborder.FBOrderid;
                    fborderlist.Createdate = dt_tw();
                    fborderlist.Updatedate = dt_tw();
                    fborderlistService.Create(fborderlist);
                    fborderlistService.SaveChanges();
                }
                else if(AccountStatus == 2)
                {
                    /*** 更新會員的Cookie *****/
                    fbmembers.Cookie = Cookie;
                    fbmembersService.SpecificUpdate(fbmembers, new string[] { "Cookie" });
                    fbmembersService.SaveChanges();                    
                    /*** 改訂單剩餘人數 ***/
                    fborder.Remains -= 1;
                    /**** 更新成本 *****/
                    fborder.CostRun += Convert.ToDouble(fborder.ProductCost); //運行成本
                    fborderService.SpecificUpdate(fborder, new string[] { "CostRun", "Remains" });
                    fborderService.SaveChanges();
                    /**** 更新完成列表 ****/
                    FBOrderlist fborderlist = new FBOrderlist();
                    fborderlist.FBOrderlistid = Guid.NewGuid();
                    fborderlist.FBMemberid = Guid.Parse(Memberid);
                    fborderlist.FBAccount = fbmembers.FB_Account;
                    fborderlist.FBOrderid = fborder.FBOrderid;
                    fborderlist.Createdate = dt_tw();
                    fborderlist.Updatedate = dt_tw();
                    fborderlistService.Create(fborderlist);
                    fborderlistService.SaveChanges();
                }
                               
                UpdateData.Add(
                    new Controllers.UpdateData()
                    {
                        Status = "Success"
                    }
                );
                return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                UpdateData.Add(
                    new Controllers.UpdateData()
                    {
                        Status = "Error"
                    }
                );
                return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --創建VMLog--
        /******* 創建VMLog ******/
        [HttpPost]
        public JsonResult CreatFBVMLog(string Id, string VMName, string FBOrdernumber)
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();
                FBVMLog fbvmlog = new FBVMLog();
                fbvmlog.FBVMid = Guid.NewGuid();
                fbvmlog.FBOrderid = fborder.FBOrderid;
                fbvmlog.PC_Name = VMName;
                fbvmlog.Status = "Running";
                fbvmlogService.Create(fbvmlog);
                fbvmlogService.SaveChanges();
                return this.Json(fbvmlog.FBVMid, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --更新VMLog--
        [HttpGet]
        public JsonResult UpdateVMLog(string Id, Guid VMid)
        {
            List<UpdateData> UpdateData = new List<Controllers.UpdateData>();
            if(Id == "CloudControl_order")
            {
                FBVMLog fbvmlog = fbvmlogService.GetByID(VMid);
                fbvmlog.Status = "Success";
                fbvmlogService.SpecificUpdate(fbvmlog, new string[] { "Status" });
                fbvmlogService.SaveChanges();
                UpdateData.Add(
                    new Controllers.UpdateData()
                    {
                        Status = "Success"
                    }
                );
                return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                UpdateData.Add(
                    new Controllers.UpdateData()
                    {
                        Status = "Error"
                    }
                );
                return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --運行VM--
        [HttpGet]
        public JsonResult RunVMLog(string Id, string VMName)
        {
            UpdateData UpdateData = new Controllers.UpdateData();
            if(Id == "CloudControl_order")
            {
                FBVMLog fbvmlog = fbvmlogService.Get().Where(a => a.PC_Name == VMName).FirstOrDefault();
                if(fbvmlog != null)
                {
                    fbvmlog.Status = "Running";
                    fbvmlogService.SpecificUpdate(fbvmlog, new string[] { "Status" });
                    fbvmlogService.SaveChanges();
                    UpdateData.Status = "Success";
                }
                else
                {
                    UpdateData.Status = "Error";
                }
            }
            else
            {
                UpdateData.Status = "Error";
            }
            return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region --取得有資源群組的訂單--
        /***** 取得有資源群組的訂單 *****/
        [HttpGet]
        public JsonResult GetFBOrderResourceGroup(string Id)
        {
            if (Id == "CloudControl_order")
            {
                IEnumerable<FBOrder> fborders = fborderService.Get().Where(a => a.IsResourceGroup == true).OrderByDescending(o => o.Createdate);
                string[] FBOrderArray = new string[fborders.Count()];
                int i = 0;
                foreach (FBOrder fborder in fborders)
                {
                    FBOrderArray[i] = fborder.FBOrdernumber;
                    i++;
                }
                return this.Json(FBOrderArray, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --刪除訂單資源群組--
        /**** 刪除訂單資源群組 ****/
        [HttpGet]
        public JsonResult DeleteFBOrderResourceGroup(string Id, string FBOrdernumber)
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();
                fborder.IsResourceGroup = false;
                fborderService.SpecificUpdate(fborder, new string[] { "IsResourceGroup" });
                fborderService.SaveChanges();
                return this.Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --查看訂單狀態--
        [HttpGet]
        public JsonResult CheckFBOrderStatus(string Id, string FBOrdernumber)
        {
            if (Id == "CloudControl_order")
            {
                FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == FBOrdernumber).FirstOrDefault();
                return this.Json(fborder.FBOrderStatus, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --驗證帳號--
        [HttpGet]
        public JsonResult AuthFBMember(string Id)
        {
            List<AuthAccount> Auth = new List<AuthAccount>();
            if(Id == "CloudControl_order")
            {
                List<GetAccountLink> LinkList = new List<GetAccountLink>();
                IEnumerable<FBMembers> FBMembers = fbmembersService.GetNoDel();
                foreach(FBMembers FBMember in FBMembers)
                {
                    LinkList.Add(
                        new GetAccountLink()
                        {
                            Account = FBMember.FB_Account,
                            Link = FBMember.Facebooklink
                        }
                    );
                }
                Auth.Add(
                    new AuthAccount()
                    {
                        Status = "Success",
                        LinkList = LinkList
                    }
                );
                return this.Json(Auth, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Auth.Add(
                    new AuthAccount()
                    {
                        Status = "Error"
                    }
                );
                return this.Json(Auth, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult UpdateAuthFBMember(string Id, string Account, int Status)
        {
            List<UpdateData> UpdateData = new List<UpdateData>();
            if(Id == "CloudControl_order")
            {
                FBMembers fbmember = fbmembersService.GetNoDel().Where(a => a.FB_Account.Contains(Account)).FirstOrDefault();
                FBMembersLoginlog fbmemberloiglog = fbmembersloginlogService.Get().Where(a => a.FBMemberid == fbmember.FBMemberid).FirstOrDefault();
                if (fbmemberloiglog != null)
                {
                    fbmemberloiglog.Status = Status;
                    fbmembersloginlogService.SpecificUpdate(fbmemberloiglog, new string[] { "Status" });
                }
                else
                {
                    FBMembersLoginlog newlog = new FBMembersLoginlog();
                    newlog.FBMemberid = fbmember.FBMemberid;
                    newlog.Createdate = DateTime.Now;
                    newlog.Status = Status;
                    fbmembersloginlogService.Create(newlog);
                }
                fbmembersloginlogService.SaveChanges();
                UpdateData.Add(
                    new Controllers.UpdateData()
                    {
                        Status = "Success"
                    }
                );
                return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                UpdateData.Add(
                    new Controllers.UpdateData()
                    {
                        Status = "Error"
                    }
                );
                return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
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
        #region --測試API--
        [HttpGet]
        public JsonResult Test_Api(string Account)
        {
            FBMembers FBMember = fbmembersService.GetNoDel().Where(a => a.FB_Account.Contains(Account)).FirstOrDefault();
            FBMember.FB_Account = FBMember.FB_Account.Trim();
            fbmembersService.SpecificUpdate(FBMember, new string[] { "FB_Account" });
            fbmembersService.SaveChanges();
            return this.Json("Success", JsonRequestBehavior.AllowGet);
        }
        #endregion     
        #region --更新Cookie--
        [HttpGet]
        public JsonResult GetFBAccount_OldCookie(string Id)
        {
            List<AccountStatus> AccountStatus = new List<Controllers.AccountStatus>();
            Guid LiveId = Guid.Parse("78DA7B19-F424-4EFA-9691-45268100188D");
            if(Id == "CloudControl_order")
            {               
                List<GetAccount> AccountList = new List<GetAccount>();
                FBMembers fbmembers = fbmembersService.GetNoDel().Where(i => i.Isenable == 2).Where(a => a.Isnew == 0).Where(c => c.Productid != LiveId).Where(x => x.FBMembersLoginlog.FirstOrDefault().Status != 2).FirstOrDefault();
                if(fbmembers != null)
                {
                    AccountList.Add(
                        new GetAccount()
                        {
                            Memberid = fbmembers.FBMemberid.ToString()
                        }
                    );
                    
                                                   
                    AccountStatus.Add(
                        new Controllers.AccountStatus()
                        {
                            Status = "Success",
                            List = AccountList
                        }
                    );
                    return this.Json(AccountStatus, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    AccountStatus.Add(
                        new Controllers.AccountStatus()
                        {
                            Status = "Error"
                        }
                    );
                    return this.Json(AccountStatus, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                AccountStatus.Add(
                    new Controllers.AccountStatus()
                    {
                        Status = "Error"
                    }
                );
                return this.Json(AccountStatus, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetFBAccountDatail_OldCookie(string Id, string Memberid)
        {
            List<AccountDetailStatus> AccountDetailStatus = new List<Controllers.AccountDetailStatus>();
            if(Id == "CloudControl_order")
            {
                List<GetAccountDetail> GetAccountDetail = new List<Controllers.GetAccountDetail>();
                Guid FBMemberid = Guid.Parse(Memberid);
                FBMembers fbmember = fbmembersService.GetByID(FBMemberid);                
                GetAccountDetail.Add(
                    new Controllers.GetAccountDetail()
                    {
                        Account = fbmember.FB_Account,
                        Memberid = fbmember.FBMemberid,
                        Password = fbmember.FB_Password,
                        Useragent_phone = fbmember.Useragent
                    }
                );
                AccountDetailStatus.Add(
                    new Controllers.AccountDetailStatus()
                    {
                        Status = "Success",
                        AccountDetailList = GetAccountDetail
                    }
                );
                return this.Json(AccountDetailStatus, JsonRequestBehavior.AllowGet);
            }
            else
            {
                AccountDetailStatus.Add(
                    new Controllers.AccountDetailStatus()
                    {
                        Status = "Error"
                    }
                );
                return this.Json(AccountDetailStatus, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult UpdateFBAccount_Link(string Id, string Facebooklink)
        {
            List<UpdateData> UpdateData = new List<Controllers.UpdateData>();
            if(Id == "CloudControl_order")
            {
                FBMembers fbmember = fbmembersService.Get().Where(a => a.Facebooklink == Facebooklink).FirstOrDefault();
                fbmember.Isnew = 1;
                fbmembersService.SpecificUpdate(fbmember, new string[] {"Isnew" });
                fbmembersService.SaveChanges();
                FBMembersLoginlog fbmemberloiglog = fbmembersloginlogService.Get().Where(a => a.FBMemberid == fbmember.FBMemberid).FirstOrDefault();
                if (fbmemberloiglog != null)
                {
                    fbmemberloiglog.Status = 2;
                    fbmembersloginlogService.SpecificUpdate(fbmemberloiglog, new string[] { "Status" });
                }
                else
                {
                    FBMembersLoginlog newlog = new FBMembersLoginlog();
                    newlog.FBMemberid = fbmember.FBMemberid;
                    newlog.Createdate = DateTime.Now;
                    newlog.Status = 2;
                    fbmembersloginlogService.Create(newlog);
                }
                fbmembersloginlogService.SaveChanges();
                UpdateData.Add(
                    new Controllers.UpdateData()
                    {
                        Status = "Success"
                    }
                );
                return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                UpdateData.Add(
                    new Controllers.UpdateData()
                    {
                        Status = "Error"
                    }
                );
                return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateFBAccount_NewCookie(string Id, string Memberid, string Cookie, string Facebookid, int Status)
        {
            List<UpdateData> UpdateData = new List<Controllers.UpdateData>();
            if(Id == "CloudControl_order")
            {
                FBMembers fbmember = fbmembersService.GetByID(Guid.Parse(Memberid));
                fbmember.Cookie = Cookie;
                fbmember.Facebooklink = "https://www.facebook.com/profile.php?id=" + Facebookid;
                fbmember.Isnew = 1;
                fbmembersService.SpecificUpdate(fbmember, new string[] { "Cookie", "Isnew" });
                fbmembersService.SaveChanges();
                FBMembersLoginlog fbmemberloiglog = fbmembersloginlogService.Get().Where(a => a.FBMemberid == fbmember.FBMemberid).FirstOrDefault();
                if (fbmemberloiglog != null)
                {
                    fbmemberloiglog.Status = Status;
                    fbmembersloginlogService.SpecificUpdate(fbmemberloiglog, new string[] { "Status" });
                }
                else
                {
                    FBMembersLoginlog newlog = new FBMembersLoginlog();
                    newlog.FBMemberid = fbmember.FBMemberid;
                    newlog.Createdate = DateTime.Now;
                    newlog.Status = Status;
                    fbmembersloginlogService.Create(newlog);
                }
                fbmembersloginlogService.SaveChanges();
                UpdateData.Add(
                    new Controllers.UpdateData()
                    {
                        Status = "Success"
                    }
                );
                return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                UpdateData.Add(
                    new Controllers.UpdateData()
                    {
                        Status = "Error"
                    }
                );
                return this.Json(UpdateData, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --辦Facebook帳號專區--
        public JsonResult GetUseragent(string Id)
        {
            if (Id == "CC_Create_Facebook")
            {
                int useragent_count = useragentService.Get().Count();
                Random crandom = new Random();
                int rand = crandom.Next(0, useragent_count - 1);
                Useragent[] useragent = useragentService.Get().ToArray();
                return this.Json(useragent[rand].User_agent, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        #region --新增帳號專區--
        /**** 新增FB帳號 *****/
        [HttpPost]
        public JsonResult InsertFBMember(string Id, string Account, string Password, string Name, string Facebookid, string Cookie, string Productid, string Isenable)
        {
            if (Id == "CloudControl_Insert")
            {
                FBMembers fbmember = new FBMembers();
                fbmember.Isenable = int.Parse(Isenable);
                fbmember.FBMemberid = Guid.NewGuid();
                /***** 
                   (1) 文章(讚)
                   (2) 文章(愛心)
                   (3) 文章(哇)
                   (4) 文章(哈)
                   (5) 文章(嗚)
                   (6) 文章(怒)
                   (7) 文章留言
                   (8) 嵌入式留言
                   (9) 粉絲專頁讚
                   (10) 個人追蹤
                   (11) 直播
                ****/
                switch (Productid)
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
                    default:
                        fbmember.Productid = Guid.Parse("e16dfe59-2789-4598-9310-6334e5e7803c");
                        break;
                }
                fbmember.Facebooklink = "https://www.facebook.com/profile.php?id=" + Facebookid;
                fbmember.AccountCost = 8;
                fbmember.Cookie = Cookie;
                fbmember.Createdate = dt_tw();
                fbmember.FB_Account = Account;
                fbmember.FB_Password = Password;
                fbmember.FB_Name = Name;
                fbmember.Updatedate = dt_tw();      
                fbmember.Lastdate = ((int)(dt_tw() - new DateTime(1970, 1, 1)).TotalSeconds) - 28800;      // 總秒數
                int useragent_count = useragentService.Get().Count();
                Random crandom = new Random();
                int rand = crandom.Next(0, useragent_count - 1);
                Useragent[] useragent = useragentService.Get().ToArray();
                fbmember.Useragent = useragent[rand].User_agent;
                FBMembersLoginlog fbmemberloginlog = new FBMembersLoginlog();
                fbmemberloginlog.FBMemberid = fbmember.FBMemberid;
                fbmemberloginlog.Status = 1;
                fbmemberloginlog.Createdate = DateTime.Now;
                fbmember.FBMembersLoginlog.Add(fbmemberloginlog);
                fbmembersService.Create(fbmember);
                fbmembersService.SaveChanges();
                return this.Json("Success");
            }
            else
            {
                return this.Json("Error");
            }
        }

        /***** 新增FB直播帳號 ****/
        [HttpPost]
        public JsonResult InsertLiveFBMember(string Id, string Account, string Password, string Name, string Facebookid, string Cookie, string Isenable)
        {
            if (Id == "CloudControl_Insert")
            {
                string Useragent;
                FBMembers fbmember = new FBMembers();
                fbmember.Isenable = int.Parse(Isenable);
                fbmember.FBMemberid = Guid.NewGuid();
                fbmember.Productid = Guid.Parse("78da7b19-f424-4efa-9691-45268100188d");
                fbmember.Facebooklink = "https://www.facebook.com/profile.php?id=" + Facebookid;
                fbmember.Cookie = Cookie;
                fbmember.Createdate = DateTime.Now;
                fbmember.FB_Account = Account;
                fbmember.FB_Password = Password;
                fbmember.FB_Name = Name;
                fbmember.Updatedate = DateTime.Now;
                fbmember.Lastdate = ((int)(dt_tw() - new DateTime(1970, 1, 1)).TotalSeconds) - 28800;      // 總秒數                
                Useragent = GetUseragent();
                fbmember.Useragent = Useragent;
                FBMembersLoginlog fbmemberloginlog = new FBMembersLoginlog();
                fbmemberloginlog.FBMemberid = fbmember.FBMemberid;
                fbmemberloginlog.Status = 1;
                fbmemberloginlog.Createdate = DateTime.Now;
                fbmember.FBMembersLoginlog.Add(fbmemberloginlog);
                fbmembersService.Create(fbmember);
                fbmembersService.SaveChanges();
                return this.Json("Success");
            }
            else
            {
                return this.Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region --取得電腦版Useragent--
        public string GetUseragent()
        {
            string[] Useragent = new string[10];
            Useragent[0] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
            Useragent[1] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.121 Safari/537.36";
            Useragent[2] = "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.121 Safari/537.36";
            Useragent[3] = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
            Useragent[4] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.121 Safari/537.36";
            Useragent[5] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/67.0";
            Useragent[6] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:61.0) Gecko/20100101 Firefox/61.0";
            Useragent[7] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0";
            Useragent[8] = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0";
            Useragent[9] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0";
            Random crandom = new Random();
            int rand = crandom.Next(0, 9);
            return Useragent[rand];
        }
        #endregion
    }
    public class get_old_member
    {
        public string Account { get; set; }
    }

    public class get_livelist
    {
        public string Account { get; set; }
    }
    public class get_member
    {
        public Guid Memberid { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
        public string Useragent_phone { get; set; }
        public string Cookie { get; set; }
        public int Duedate { get; set; }
        public string Name { get; set; }
        public string UserDataUrl { get; set; }
        public string Mega_Account { get; set; }
        public string Mega_Password { get; set; }
    }
    public class GetOldAccount
    {
        public Guid Memberid { get; set; }
    }
    public class GetAccount
    {
        public string Memberid { get; set; }
    }
    public class AccountStatus
    {
        public string Status { get; set; }
        public List<GetAccount> List { get; set; }
    }
    public class GetAccountDetail
    {
        public Guid Memberid { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string Useragent_phone { get; set; }
        public string Cookie { get; set; }
        public string UserDataUrl { get; set; }
        public string Mega_Account { get; set; }
        public string Mega_Password { get; set; }
    }
    public class AccountDetailStatus
    {
        public string Status { get; set; }
        public List<GetAccountDetail> AccountDetailList { get; set; }
    }
    public class OrderDetail
    {
        public string FBOrdernumber { get; set; }
        public int Remain { get; set; }
    }
    public class OrderStatus
    {
        public string Status { get; set; }
        public Guid Productid { get; set; }
        public OrderDetail OrderDetail { get; set; }
    }
    public class UpdateData
    {
        public string Status { get; set; }
    }
    public class OrderUrlStatus
    {
        public string Status { get; set; }
        public List<OrderUrl> OrderUrl { get; set; }
    }

    public class OrderUrl
    {
        public string Url { get; set; }
    }

    public class AuthAccount
    {
        public string Status { get; set; }
        public List<GetAccountLink> LinkList { get; set; }
    }

    public class GetAccountLink
    {
        public string Account { get; set; }
        public string Link { get; set; }
    }
}


/*
-----FBProductId-----
個人追蹤 (Y)	e16dfe59-2789-4598-9310-6334e5e7803c	
文章(怒) (Y)	bffb9389-46f0-4bf4-a6ab-d4dcf77435c7	
文章(嗚) (Y)	e592d2a8-a1a7-4471-a648-0547c7a46cdd	
文章(哇) (Y)	f9dd03ee-ecd7-4514-94c6-2ea7d72d931c
文章(哈) (Y)	6b1e8bd2-8dbb-4282-89df-b509be5ff361	
文章(愛心) (Y)	6c5425d0-5362-4fa9-8c6e-dfb929877b93
直播	78da7b19-f424-4efa-9691-45268100188d
嵌入式留言 (Y)	07408390-5f81-451a-9193-a93faaed1825	
文章留言 (Y)	f686d184-884c-4aa7-9f26-f8118ba7f990	
粉絲專頁讚 (Y)	b93e5ee4-f946-4bb0-ad6a-8f379e704802
文章(讚) (Y)	0c020482-d76a-4213-b021-f8db0fe96489
*/
