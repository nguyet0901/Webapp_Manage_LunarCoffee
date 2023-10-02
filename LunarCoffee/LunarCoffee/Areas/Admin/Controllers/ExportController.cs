using LunarCoffee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using LunarCoffee.Models.EntityFramework;

namespace LunarCoffee.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Export
        public ActionResult Index(int? page)
        {
            var items = db.Exports.OrderByDescending(x => x.CreatedDate).ToList();
            if(page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult View (int id)
        {
            var item = db.Exports.Find(id);
            return View(item);
        }

        public ActionResult Partial_SanPham (int id)
        {
            var items = db.ExportDetais.Where(x => x.ExportId == id).ToList();
            return PartialView(items);
        }

        [HttpPost]
        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = db.Exports.Find(id);
            if (item != null)
            {
                db.Exports.Attach(item);
                var dtNow = DateTime.Now;
                item.IsExport = trangthai;
                item.DateExport = dtNow;
                db.Entry(item).Property(x => x.IsExport).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });
        }

    }
}