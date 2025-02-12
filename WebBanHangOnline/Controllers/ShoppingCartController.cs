using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null && cart.Items.Any())
			{
				ViewBag.CheckCart = cart;
			}
			return View();
			
        }

		public ActionResult CheckOut()
		{
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null && cart.Items.Any())
			{
                ViewBag.CheckCart = cart;
			}
			return View();
		}

		public ActionResult CheckOutSuccess()
		{
			return View();
		}

		public ActionResult Partial_Item_Payment()
		{
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null && cart.Items.Any())
			{
				return PartialView(cart.Items);
			}
			return PartialView();
		}

		public ActionResult Partial_Item_Cart()
		{
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null && cart.Items.Any())
			{
				return PartialView(cart.Items);
			}
			return PartialView();
		}

		public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if(cart != null )
            {
				return Json(new {Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
			}
            return Json(new {Count = 0 }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Partial_CheckOut()
        {
            return PartialView() ;
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CheckOut(OrderViewModel req)
		{
			var code = new { Success = false, Code = -1 };

			if (!ModelState.IsValid)
			{
				return Json(new { Success = false, Message = "Vui lòng điền đầy đủ thông tin!" });
			}

			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null && cart.Items.Any())
			{
				Order order = new Order
				{
					CustomerName = req.CustomerName,
					Phone = req.Phone,
					Address = req.Address,
					Email = req.Email,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					CreatedBy = req.Phone,
					PaymentType = req.PaymentType,
					Code = "DH" + new Random().Next(1000, 9999),
					TotalAmount = cart.Items.Sum(x => x.Price * x.Quantity).ToString(),
					OrderDetails = cart.Items.Select(x => new OrderDetail
					{
						ProductId = x.ProductId,
						Quantity = x.Quantity,
						Price = x.Price
					}).ToList()
				};

				dbContext.Orders.Add(order);
				dbContext.SaveChanges();

				// Xử lý nội dung email
				var strSanPham = "";
				var thanhtien = decimal.Zero;
				var TongTien = decimal.Zero;

				foreach (var sp in cart.Items)
				{
					strSanPham += "<tr>";
					strSanPham += "<td>" + sp.ProductName + "</td>";
					strSanPham += "<td>" + sp.Quantity + "</td>";
					strSanPham += "<td>" + WebBanHangOnline.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
					strSanPham += "</tr>";
					thanhtien += sp.Price * sp.Quantity;
				}
				TongTien = thanhtien;

				// Gửi mail cho khách hàng
				string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
				contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code)
												 .Replace("{{SanPham}}", strSanPham)
												 .Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"))
												 .Replace("{{TenKhachHang}}", order.CustomerName)
												 .Replace("{{Phone}}", order.Phone)
												 .Replace("{{Email}}", req.Email)
												 .Replace("{{DiaChiNhanHang}}", order.Address)
												 .Replace("{{ThanhTien}}", WebBanHangOnline.Common.Common.FormatNumber(thanhtien, 0))
												 .Replace("{{TongTien}}", WebBanHangOnline.Common.Common.FormatNumber(TongTien, 0));
				WebBanHangOnline.Common.Common.SendMail("TBShop", "Đơn hàng #" + order.Code, contentCustomer, req.Email);

				// Gửi mail cho admin
				string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
				contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code)
										   .Replace("{{SanPham}}", strSanPham)
										   .Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"))
										   .Replace("{{TenKhachHang}}", order.CustomerName)
										   .Replace("{{Phone}}", order.Phone)
										   .Replace("{{Email}}", req.Email)
										   .Replace("{{DiaChiNhanHang}}", order.Address)
										   .Replace("{{ThanhTien}}", WebBanHangOnline.Common.Common.FormatNumber(thanhtien, 0))
										   .Replace("{{TongTien}}", WebBanHangOnline.Common.Common.FormatNumber(TongTien, 0));
				WebBanHangOnline.Common.Common.SendMail("TBShop", "Đơn hàng mới #" + order.Code, contentAdmin, ConfigurationManager.AppSettings["EmailAdmin"]);

				// Xóa giỏ hàng
				cart.ClearCart();
				Session["Cart"] = cart;

				// Kiểm tra nếu là AJAX request
				if (Request.IsAjaxRequest())
				{
					return PartialView("Partial_CheckOut");
				}

				return RedirectToAction("CheckOutSuccess");
			}

			return Json(code);
		}


		[HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias = checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công!", code = 1, Count = cart.Items.Count };
            }
            return Json(code);
        }

		[HttpPost]
		public ActionResult Update(int id, int quantity)
		{
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null)
			{
				cart.UpdateQuantity(id,quantity);
				return Json(new { Success = true });
			}
			return Json(new { Success = false });
		}

		[HttpPost]
        public ActionResult Delete(int id)
        {
			var code = new { Success = false, msg = "", code = -1, Count = 0 };

			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null)
			{
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
					code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
				}

			}

			return Json(code);  
		}


		[HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if(cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
			return Json(new { Success = false });
		}


    }
}