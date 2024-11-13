using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
		private ApplicationDbContext _dbContext = new ApplicationDbContext();
		// GET: Admin/ProductImages
		public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = _dbContext.ProductImages.Where(x => x.ProductId == id).ToList();
            return View(items);
        }

        [HttpPost]
        public ActionResult AddImage(int productId, string url)
        {
            _dbContext.ProductImages.Add(new Models.EF.ProductImage {
                ProductId = productId,
                Image = url,
                IsDefault = true
            }); 
            _dbContext.SaveChanges();
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbContext.ProductImages.Find(id);
            _dbContext.ProductImages.Remove(item);
            _dbContext.SaveChanges();
            return Json(new {success = true});
        }
    }
}