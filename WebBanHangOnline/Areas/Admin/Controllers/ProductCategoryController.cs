﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
		private ApplicationDbContext _dbConnect = new ApplicationDbContext();
		// GET: Admin/ProductCategory
		public ActionResult Index()
        {
			var items = _dbConnect.ProductCategories;

			return View(items);
        }

		
		public ActionResult Add()
        {
            
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(ProductCategory model)
		{
			if (ModelState.IsValid)
			{
				model.CreatedDate = DateTime.Now;
				model.ModifiedDate = DateTime.Now;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Alias);
				_dbConnect.ProductCategories.Add(model);
				_dbConnect.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);
		}


		public ActionResult Edit(int id)
		{
			var item = _dbConnect.ProductCategories.Find(id);
			return View(item);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ProductCategory model)
		{
			if (ModelState.IsValid)
			{
				model.ModifiedDate = DateTime.Now;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Alias);
				_dbConnect.ProductCategories.Attach(model);
				_dbConnect.Entry(model).State = System.Data.Entity.EntityState.Modified;
				_dbConnect.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);

		}


		[HttpPost]
		public ActionResult Delete(int id)
		{
			var item = _dbConnect.ProductCategories.Find(id);
			if (item != null)
			{
				_dbConnect.ProductCategories.Remove(item);
				_dbConnect.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = true });
		}


	}
}