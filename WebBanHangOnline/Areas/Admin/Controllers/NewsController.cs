using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Admin/News
        public ActionResult Index()
        {
            var items = _dbContext.News.OrderByDescending(x => x.Id).ToList();
            return View(items);
        }

        public ActionResult Add()
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(News model)
        {
            if(ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.CategoryId = 3;
                model.Alias  = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Alias);
                _dbContext.News.Add(model);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);

        }
		public ActionResult Edit(int id)
		{
            var item = _dbContext.News.Find(id);
			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(News model)
		{
			if (ModelState.IsValid)
			{
				model.ModifiedDate = DateTime.Now;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Alias);
				_dbContext.News.Attach(model);
                _dbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
				_dbContext.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);

		}
		[HttpPost]
		public ActionResult Delete(int id)
        {
            var item = _dbContext.News.Find(id);
            if(item != null)
            {
                _dbContext.News.Remove(item);
                _dbContext.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = true });
		}
		[HttpPost]
		public ActionResult IsActive(int id)
		{
			var item = _dbContext.News.Find(id);
			if (item != null)
			{
				item.IsActive = !item.IsActive;
				_dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
				_dbContext.SaveChanges();
				return Json(new { success = true, isActive = item.IsActive });
			}
			return Json(new { success = false });
		}
		[HttpPost]
		public ActionResult DeleteAll(string ids)
		{
			if(!string.IsNullOrEmpty(ids))
			{
				var items = ids.Split(',');
				if(items != null && items.Any()) 
				{
					foreach(var item in items)
					{
						var obj = _dbContext.News.Find(Convert.ToInt32(item));
						_dbContext.News.Remove(obj);
						_dbContext.SaveChanges();
					}
				}
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}
	}
}