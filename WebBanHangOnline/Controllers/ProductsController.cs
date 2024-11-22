using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Product
        public ActionResult Index()
        {
            var items = _dbContext.Products.ToList();

            return View(items);
        }

        public ActionResult Detail(string alias ,int id)
        {
            var item = _dbContext.Products.Find(id);
            return View(item);
        }

		public ActionResult ProductCategory(string alias ,int id)
		{
			var items = _dbContext.Products.ToList();

			if (id > 0 )
			{
				items = items.Where(x => x.ProductCategoryId == id).ToList();
			}

            var cate = _dbContext.ProductCategories.Find(id);

            if(cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            ViewBag.CateId = id;

			return View(items);
		}

		public ActionResult Partial_ItemsByCateId()
        {
            var items = _dbContext.Products.Where(x => x.IsHome && x.IsActive).Take(10).ToList();
            return PartialView(items);
        }

		public ActionResult Partial_ProductSales()
		{
			var items = _dbContext.Products.Where(x => x.IsSale && x.IsActive).Take(10).ToList();
			return PartialView(items);
		}
	}
}