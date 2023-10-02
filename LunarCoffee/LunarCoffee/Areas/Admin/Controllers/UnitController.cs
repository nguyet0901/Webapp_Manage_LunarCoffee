using LunarCoffee.Models;
using LunarCoffee.Models.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LunarCoffee.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UnitController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Unit
        public ActionResult Index(string SearchText, int?page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Unit> items = db.Unit.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(SearchText))
            {
                items = items.Where(x => x.Name.Contains(SearchText));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Add(Unit model)
        {
            if (ModelState.IsValid)
            {
                if (db.Unit.Any(e => e.Name == model.Name))
                {
                    ModelState.AddModelError(nameof(model.Name), "Đơn vị đã tồn tại");
                    return View(model);
                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Name = model.Name;
                
                db.Unit.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit (int id)
        {
            var item = db.Unit.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit (Unit model)
        {
            if (ModelState.IsValid)
            {
                if (db.Unit.Any(e => e.Name == model.Name && e.Id != model.Id))
                {
                    ModelState.AddModelError(nameof(model.Name), "Đơn vị đã tồn tại");
                    return View(model);
                }
                db.Unit.Attach(model);
                model.ModifiedDate = DateTime.Now;
                model.Name = model.Name;
                db.Entry(model).Property(x => x.Name).IsModified = true;
                db.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
                db.Entry(model).Property(x => x.ModifieddBy).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete (int id)
        {
            var item = db.Unit.Find(id);
            if (item != null)
            {
                var DeleteItem = db.Unit.Attach(item);
                db.Unit.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }    
            return Json(true);

        }

        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = db.Unit.Find(Convert.ToInt32(item));
                        db.Unit.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}