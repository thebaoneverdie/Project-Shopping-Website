using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var items = dbContext.Orders.OrderByDescending(x => x.CreatedDate).ToList();
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
			ViewBag.PageSize = pageSize;
			ViewBag.Page = pageNumber;
			return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult View(int id)
        {
            var item = dbContext.Orders.Find(id);
            return View(item);
        }

        public ActionResult Partial_SanPham(int id)
        {
            var items = dbContext.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(items);
        }


        [HttpPost]
        public ActionResult UpdateTrangThai(int id, int trangthai)
        {
            var item = dbContext.Orders.Find(id);
            if (item != null)
            {
                dbContext.Orders.Attach(item);
                item.PaymentType = trangthai;
                dbContext.Entry(item).Property(x => x.PaymentType).IsModified = true;
                dbContext.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
			return Json(new { message = "UnSuccess", Success = false });
		}
    }
}