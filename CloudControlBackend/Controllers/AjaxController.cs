using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudControl.Service;
using CloudControl.Model;

namespace CloudControlBackend.Controllers
{
    public class AjaxController : BaseController
    {
        private CloudControlEntities db;
        private ProductService productService;
        private MessageService messageService;
        private FBMembersService fbmembersService;
        private IGMembersService igmembersService;
        private YTMembersService ytmembersService;
        private FBOrderService fborderService;
        private FBOrderlistService fborderlistService;
        private IGOrderService igorderService;
        private YTOrderService ytorderService;
        public AjaxController()
        {
            db = new CloudControlEntities();
            productService = new ProductService();
            messageService = new MessageService();
            fbmembersService = new FBMembersService();
            igmembersService = new IGMembersService();
            ytmembersService = new YTMembersService();
            fborderService = new FBOrderService();
            fborderlistService = new FBOrderlistService();
            igorderService = new IGOrderService();
            ytorderService = new YTOrderService();
        }

        /**** 取得產品成本/售價 *****/
        [HttpPost]
        public JsonResult Productcost(Guid Productid)
        {
            Product product = productService.GetByID(Productid);
            return this.Json(product.Cost);
        }
        /**** 取得產品售價 *****/
        [HttpPost]
        public JsonResult Productprice(Guid Productid)
        {
            Product product = productService.GetByID(Productid);
            return this.Json(product.Price);
        }
        /**** 批量刪除留言 *****/
        [HttpPost]
        public JsonResult AjaxDeleteMessageChecked(Guid[] Messageid)
        {
            if (Messageid != null)
            {
                foreach (Guid thismessageid in Messageid)
                {
                    Message message = messageService.GetByID(thismessageid);
                    messageService.Delete(message);
                }
                messageService.SaveChanges();
            }

            return this.Json("Success");
        }
        /**** 存取訂單編號到Session(全選) ****/
        [HttpPost]
        public JsonResult AjaxStatisticsAllOrder(string[] Ordernumber)
        {
            if(Ordernumber != null)
            {
                foreach (string thisordernumber in Ordernumber)
                {
                    Session["OrderStatistics"] += thisordernumber + ";";
                }
            }
            return this.Json("Success");
        }
        /**** 存取訂單編號到Session(單一) ****/
        [HttpPost]
        public JsonResult AjaxStatisticsOrder(string Ordernumber)
        {
            if (Ordernumber != null)
            {
                Session["OrderStatistics"] += Ordernumber + ";";
            }
            return this.Json("Success");
        }
        /*** 計算成本/售量/利潤 ***/
        [HttpPost]
        public JsonResult AjaxCalcCheckedCost()
        {
            double Cost = 0.0;   //成本
            double Price = 0.0;  //售價
            double Count = 0.0;  //筆數
            string[] OrdernumberList = Session["OrderStatistics"].ToString().Split(';');
            foreach (string ordernumber in OrdernumberList)
            {
                if(ordernumber != "")
                {
                    if (ordernumber.IndexOf("FB") != -1)
                    {
                        FBOrder fborder = fborderService.Get().Where(a => a.FBOrdernumber == ordernumber).FirstOrDefault();
                        Product product = productService.GetByID(fborder.Productid);
                        if (fborder.Istest == true)
                        {
                            Cost += 0.0;
                            Price += 0.0;
                        }
                        else
                        {
                            Cost += Convert.ToInt32(product.Cost * fborder.Count);
                            Price += Convert.ToInt32(product.Price * fborder.Count);
                        }
                    }
                    else if (ordernumber.IndexOf("IG") != -1)
                    {
                        IGOrder igorder = igorderService.Get().Where(a => a.IGOrdernumber == ordernumber).FirstOrDefault();
                        Product product = productService.GetByID(igorder.Productid);
                        if (igorder.Istest == true)
                        {
                            Cost += 0.0;
                            Price += 0.0;
                        }
                        else
                        {
                            Cost += Convert.ToInt32(product.Cost * igorder.Count);
                            Price += Convert.ToInt32(product.Price * igorder.Count);
                        }
                    }
                    else if (ordernumber.IndexOf("YT") != -1)
                    {
                        YTOrder ytorder = ytorderService.Get().Where(a => a.YTOrdernumber == ordernumber).FirstOrDefault();
                        Product product = productService.GetByID(ytorder.Productid);
                        if (ytorder.Istest == true)
                        {
                            Cost += 0.0;
                            Price += 0.0;
                        }
                        else
                        {
                            Cost += Convert.ToInt32(product.Cost * ytorder.Count);
                            Price += Convert.ToInt32(product.Price * ytorder.Count);
                        }
                    }
                    Count++;
                }                
            }
            Session.Remove("OrderStatistics");  // 清除Session的訂單編號
            return this.Json(Cost+";"+Price+";"+(Price-Cost)+";"+ Count);

        }
        /**** 更新訂單 成本/完成人數/死亡人數/補充人數 ***/
        [HttpPost]
        public JsonResult AjaxUpdateCheckedCost(Guid[] FBOrderid)
        {
            foreach(Guid Orderid in FBOrderid)
            {
                FBOrder fborder = fborderService.GetByID(Orderid);
                IEnumerable<FBOrderlist> fborderlist = fborderlistService.Get().Where(a => a.FBOrderid == Orderid);
                fborder.Cost = (fborder.CostRun + fborder.CostAccount) + ((fborder.CostRun + fborder.CostAccount) * (20d/100d));  // 訂單成本為實際成本(運行+帳號) + 20%(預估死亡率)
                fborder.FinishCount = fborderlist.Where(c => c.FBMembers.Isenable == 1).Where(a => a.FBMembers.FBMembersLoginlog.FirstOrDefault().Status != 2).Count();
                fborder.DeathCount = fborderlist.Where(c => c.FBMembers.Isenable == 1).Where(a => a.FBMembers.FBMembersLoginlog.FirstOrDefault().Status == 2).Count();
                /*** 假設訂單完成 ***/
                if(fborder.FBOrderStatus == 2)
                {
                    fborder.ReplenishCount += Convert.ToInt32(fborder.Count - fborder.FinishCount); // 補充人數為訂單數量 - 成功數量
                }
                fborderService.SpecificUpdate(fborder, new string[] { "Cost", "FinishCount", "DeathCount", "ReplenishCount" });                
            }
            fborderService.SaveChanges();
            return this.Json("Success");
        }
        /*** 取得FB產品 ****/
        [HttpPost]
        public JsonResult FBProduct(Guid Categoryid)
        {
            IEnumerable<Product> producttemp = productService.Get().Where(a => a.Categoryid == Categoryid).ToList();
            List<ProductList> List = new List<ProductList>();
            foreach (Product productList in producttemp)
            {
                List.Add(
                    new ProductList
                    {
                        Productid = productList.Productid,
                        Productname = productList.Productname
                    });
            }
            return this.Json(List);
        }

        /**** 確認FB數量 ***/
        [HttpGet]
        public JsonResult AjaxCheckFBMembersNumber()
        {
            Guid CategoryId = Guid.Parse("9f268158-09b1-4176-9088-a4a4af63d389");
            IEnumerable<Product> ProductList = productService.Get().Where(a => a.Categoryid == CategoryId).OrderBy(o => o.Orders);
            
            List<ProductNumber> List = new List<ProductNumber>();
            foreach(Product Product in ProductList)            
            {
                int Count = fbmembersService.Get().Where(n => n.Isnew == 1).Where(a => a.FBMembersLoginlog.FirstOrDefault().Status != 2).Where(p => p.Productid == Product.Productid).Count();                
                int PrepCount = fbmembersService.GetNoDel().Where(n => n.Isnew == 1).Where(i => i.Isenable == 2).Where(a => a.FBMembersLoginlog.FirstOrDefault().Status != 2).Where(p => p.Productid == Product.Productid).Count();
                int Death = fbmembersService.Get().Where(a => a.FBMembersLoginlog.FirstOrDefault().Status == 2).Where(p => p.Productid == Product.Productid).Count();
                List.Add(
                    new ProductNumber()
                    {
                        Productname = Product.Productname,
                        Productcount = Count,
                        Productprepcount = PrepCount,
                        Productdeathcount = Death
                    }
                );
                
            }
            return this.Json(List, JsonRequestBehavior.AllowGet);
        }

        /*** 批量刪除FB會員 ****/
        public JsonResult AjaxDeleteFBMembersChecked(Guid[] FBMemberid)
        {
            if (FBMemberid != null)
            {
                foreach (Guid thismemberid in FBMemberid)
                {
                    FBMembers fbmember = fbmembersService.GetByID(thismemberid);
                    fbmember.Isenable = 0;
                    fbmembersService.SpecificUpdate(fbmember, new string[] { "Isenable" });
                }
                fbmembersService.SaveChanges();
            }

            return this.Json("Success");
        }

        /****** 設定FB會員產品 *****/
        public JsonResult AjaxSetFBMembersProduct(Guid[] FBMemberid, Guid Productid)
        {
            if(FBMemberid != null && Productid != null)
            {
                foreach(Guid thismemberid in FBMemberid)
                {
                    FBMembers fbmember = fbmembersService.GetByID(thismemberid);
                    fbmember.Productid = Productid;
                    fbmembersService.SpecificUpdate(fbmember, new string[] { "Productid" });               
                }
                fbmembersService.SaveChanges();                
            }

            return this.Json("Success");
        }

        /*** 批量FB會員轉正式 ****/
        public JsonResult AjaxToOfficialFBMembersChecked(Guid[] FBMemberid)
        {
            if (FBMemberid != null)
            {
                foreach (Guid thismemberid in FBMemberid)
                {
                    FBMembers fbmember = fbmembersService.GetByID(thismemberid);
                    fbmember.Isenable = 1;
                    fbmembersService.SpecificUpdate(fbmember, new string[] { "Isenable" });
                }
                fbmembersService.SaveChanges();
            }

            return this.Json("Success");
        }

        /*** 批量刪除IG會員 ****/
        public JsonResult AjaxDeleteIGMembersChecked(Guid[] IGMemberid)
        {
            if (IGMemberid != null)
            {
                foreach (Guid thismemberid in IGMemberid)
                {
                    IGMembers igmember = igmembersService.GetByID(thismemberid);
                    igmember.Isenable = 0;
                    igmembersService.SpecificUpdate(igmember, new string[] { "Isenable" });
                }
                igmembersService.SaveChanges();
            }

            return this.Json("Success");
        }

        /*** 批量刪除YT會員 ****/
        public JsonResult AjaxDeleteYTMembersChecked(Guid[] YTMemberid)
        {
            if (YTMemberid != null)
            {
                foreach (Guid thismemberid in YTMemberid)
                {
                    YTMembers ytmember = ytmembersService.GetByID(thismemberid);
                    ytmember.Isenable = 0;
                    ytmembersService.SpecificUpdate(ytmember, new string[] { "Isenable" });
                }
                ytmembersService.SaveChanges();
            }

            return this.Json("Success");
        }

    }
    public class ProductList
    {
        public Guid Productid { get; set; }
        public string Productname { get; set; }
    }

    public class ProductNumber
    {
        public string Productname { get; set; }
        public int Productcount { get; set; }
        public int Productprepcount { get; set; }
        public int Productdeathcount { get; set; }
    }
}