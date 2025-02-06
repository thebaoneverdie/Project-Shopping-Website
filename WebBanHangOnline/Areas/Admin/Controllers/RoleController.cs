using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
	[Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
		// GET: Admin/Role
		private ApplicationDbContext dbContext = new ApplicationDbContext();
		// GET: Role
		public ActionResult Index()
		{
			var items = dbContext.Roles.ToList();
			return View(items);
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IdentityRole model)
		{
			if(ModelState.IsValid)
			{
				var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));
				roleManager.Create(model);
				return RedirectToAction("Index");
			}
			 return View(model);
		}

		/// <summary>
		/// Chua them view cho chuc nang Edit
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult Edit(int id)
		{
			var item = dbContext.Roles.Find(id);
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(IdentityRole model)
		{
			if (ModelState.IsValid)
			{
				var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));
				roleManager.Update(model);
				return RedirectToAction("Index");
			}
			return View(model);
		}
	}
}