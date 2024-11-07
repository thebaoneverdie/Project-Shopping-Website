using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
		private ApplicationDbContext _dbContext = new ApplicationDbContext();
		// GET: Admin/Products
		public ActionResult Index(int? page)
        {
			IEnumerable<Product> items = _dbContext.Products.OrderByDescending(x => x.Id);

			var pageSize = 5;

			if (page == null)
			{
				page = 1;
			}

			var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			items = items.ToPagedList(pageIndex, pageSize);
			ViewBag.Page = pageIndex;
			ViewBag.PageSize = pageSize;

			return View(items);
        }

		public ActionResult Add()
		{
			ViewBag.ProductCategory = new SelectList(_dbContext.ProductCategories.ToList(),"Id","Title");
			return View();
		}
    }
}