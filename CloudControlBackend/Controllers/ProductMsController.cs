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
    public class ProductMsController : BaseController
    {
        private CloudControlEntities db;
        private CategoryProductService categoryproductService;
        private ProductService productService;

        public ProductMsController()
        {
            db = new CloudControlEntities();
            categoryproductService = new CategoryProductService();
            productService = new ProductService();
        }
        // GET: ProductMs
        /**** 產品類別 新增/刪除/修改 ****/
        [CheckSession(IsAuth = true)]
        public ActionResult CategoryProduct(int p = 1)
        {
            var data = categoryproductService.Get().OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            ViewBag.CategoryProduct = data.ToPagedList(pageNumber: p, pageSize: 20);
            return View();
        }

        [CheckSession(IsAuth = true)]
        public ActionResult AddCategoryProduct()
        {
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult AddCategoryProduct(CategoryProduct categoryproduct)
        {
            if (TryUpdateModel(categoryproduct, new string[] { "Categoryname" }) && ModelState.IsValid)
            {
                categoryproduct.Categoryid = Guid.NewGuid();
                categoryproduct.Createdate = dt_tw();
                categoryproduct.Updatedate = dt_tw();
                categoryproductService.Create(categoryproduct);
                categoryproductService.SaveChanges();
            }
            return RedirectToAction("CategoryProduct");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult DeleteCategoryProduct(Guid Categoryid)
        {
            CategoryProduct categoryproduct = categoryproductService.GetByID(Categoryid);
            categoryproductService.Delete(categoryproduct);
            categoryproductService.SaveChanges();
            return RedirectToAction("CategoryProduct");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult EditCategoryProduct(Guid Categoryid, int p)
        {
            ViewBag.pageNumber = p;
            CategoryProduct categoryproduct = categoryproductService.GetByID(Categoryid);
            return View(categoryproduct);
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult EditCategoryProduct(Guid Categoryid)
        {
            CategoryProduct categoryproduct = categoryproductService.GetByID(Categoryid);
            if (TryUpdateModel(categoryproduct, new string[] { "Categoryname" }) && ModelState.IsValid)
            {
                categoryproductService.Update(categoryproduct);
                categoryproductService.SaveChanges();
            }
            return RedirectToAction("CategoryProduct");
        }

        /**** 產品 查詢/新增/刪除/修改 *****/
        [CheckSession(IsAuth = true)]
        public ActionResult Product(int p = 1)
        {
            var data = productService.Get().OrderByDescending(o => o.Createdate);
            ViewBag.pageNumber = p;
            ViewBag.Product = data.ToPagedList(pageNumber: p, pageSize: 20);
            /**** 類別選單 *****/
            CategoryProductDropDownList();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult Product(Guid? Categoryid, int p = 1)
        {
            if(Categoryid != null)
            {
                var data = productService.Get().Where(a => a.Categoryid == Categoryid).OrderByDescending(o => o.Createdate);
                ViewBag.pageNumber = p;
                ViewBag.Product = data.ToPagedList(pageNumber: p, pageSize: 20);
            }
            else
            {
                var data = productService.Get().OrderByDescending(o => o.Createdate);
                ViewBag.pageNumber = p;
                ViewBag.Product = data.ToPagedList(pageNumber: p, pageSize: 20);
            }

            /**** 類別選單 *****/
            CategoryProductDropDownList();
            return View();
        }
        [CheckSession(IsAuth = true)]
        public ActionResult AddProduct()
        {
            /**** 類別選單 *****/
            CategoryProductDropDownList();
            return View();
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult AddProduct(Product product)
        {
            if (TryUpdateModel(product, new string[] { "Categoryid" ,"Productname", "BreakTime", "Price", "Cost" }) && ModelState.IsValid)
            {
                product.Productid = Guid.NewGuid();
                product.Createdate = dt_tw();
                product.Updatedate = dt_tw();
                productService.Create(product);
                productService.SaveChanges();
            }
            return RedirectToAction("Product");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult DeleteProduct(Guid Productid)
        {
            Product product = productService.GetByID(Productid);
            productService.Delete(product);
            productService.SaveChanges();
            return RedirectToAction("Product");
        }

        [HttpGet]
        [CheckSession(IsAuth = true)]
        public ActionResult EditProduct(Guid Productid, int p)
        {
            ViewBag.pageNumber = p;
            Product product = productService.GetByID(Productid);
            /**** 類別選單 ****/
            CategoryProductDropDownList(product);
            return View(product);
        }

        [HttpPost]
        [CheckSession(IsAuth = true)]
        public ActionResult EditProduct(Guid Productid)
        {
            Product product = productService.GetByID(Productid);
            if (TryUpdateModel(product, new string[] { "BreakTime", "Price", "Cost" }) && ModelState.IsValid)
            {
                productService.Update(product);
                productService.SaveChanges();
            }
            return RedirectToAction("Product");
        }
        #region --產品類別選單--
        private void CategoryProductDropDownList(Object selectMember = null)
        {
            var querys = categoryproductService.Get();

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