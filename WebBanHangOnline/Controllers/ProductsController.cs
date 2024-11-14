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
            return View();
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