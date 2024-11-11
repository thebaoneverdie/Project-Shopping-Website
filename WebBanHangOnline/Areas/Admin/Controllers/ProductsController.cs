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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(Product model, List<string> Images, List<int> rDefault)
		{
			if (ModelState.IsValid)
			{
				if(Images != null && Images.Count > 0)
				{
					for(int i = 0; i < Images.Count; i++)
					{
						if(i + 1 == rDefault[0])
						{
							model.Image = Images[i];
							model.ProductImage.Add(new ProductImage
							{
								ProductId = model.Id,
								Image = Images[i],
								IsDefault = true
							});
						}
						else
						{
							model.ProductImage.Add(new ProductImage
							{
								ProductId = model.Id,
								Image = Images[i],
								IsDefault = false
							});
						}
					}
				}
				model.CreatedDate = DateTime.Now;
				model.ModifiedDate = DateTime.Now;
				if (string.IsNullOrEmpty(model.SeoTitle))
				{
					model.SeoTitle = model.Title;
				}
					if (string.IsNullOrEmpty(model.Alias))
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
				_dbContext.Products.Add(model);
				_dbContext.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.ProductCategory = new SelectList(_dbContext.ProductCategories.ToList(), "Id", "Title");
			return View(model);
		}


		public ActionResult Edit(int id)
		{
			ViewBag.ProductCategory = new SelectList(_dbContext.ProductCategories.ToList(), "Id", "Title");
			var item = _dbContext.Products.Find(id);
			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Product model)
		{
			if (ModelState.IsValid)
			{
				model.ModifiedDate = DateTime.Now;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Alias);
				_dbContext.Products.Attach(model);
				_dbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
				_dbContext.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);

		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			var item = _dbContext.Products.Find(id);
			if (item != null)
			{
				_dbContext.Products.Remove(item);
				_dbContext.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = true });
		}

	}
}