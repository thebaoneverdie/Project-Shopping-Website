using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
		ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: Admin/Category
        public ActionResult Index()
        {
            var items = _dbConnect.Categories;
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            if(ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                _dbConnect.Categories.Add(model);
                _dbConnect.SaveChanges();
				return RedirectToAction("Index");
			}
            return View(model);
        }

        public ActionResult Edit(int id)
        {
			var item = _dbConnect.Categories.Find(id);
			if (item == null)
			{
				return HttpNotFound();
			}
			return View(item); // Truyền đối tượng item vào View
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Category model)
		{
			if (ModelState.IsValid)
			{
                _dbConnect.Categories.Attach(model);
				model.ModifiedDate = DateTime.Now;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);

                _dbConnect.Entry(model).Property(x => x.Title).IsModified = true;
				_dbConnect.Entry(model).Property(x => x.Description).IsModified = true;
				_dbConnect.Entry(model).Property(x => x.Alias).IsModified = true;
				_dbConnect.Entry(model).Property(x => x.SeoDescription).IsModified = true;
				_dbConnect.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
				_dbConnect.Entry(model).Property(x => x.SeoTitle).IsModified = true;
				_dbConnect.Entry(model).Property(x => x.Position).IsModified = true;
				_dbConnect.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
				_dbConnect.Entry(model).Property(x => x.ModifiedBy).IsModified = true;

				_dbConnect.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(model);
		}

		public ActionResult Delete(int id)
		{
			var item = _dbConnect.Categories.Find(id);
			if(item != null)
			{
				_dbConnect.Categories.Remove(item);
				_dbConnect.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}
	}
}