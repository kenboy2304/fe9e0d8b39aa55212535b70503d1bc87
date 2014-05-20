using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Cdnvn.Phim.Entities;
using Cdnvn.Phim.Web.Models;

namespace Cdnvn.Phim.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class CategoryManagerController : Controller
    {
        private readonly FilmContext db = new FilmContext();

        //
        // GET: /Admin/Category/

        public ActionResult Index(int id = 0)
        {
            if (id != 0)
            {
                var cat = db.Categories.Single(c => c.Id == id);
                if (cat != null)
                {
                    cat.Published = !cat.Published;
                    cat.ModifiedDate = DateTime.Now;
                    db.Entry(cat).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            var categories = db.Categories.Where(c => c.CategoryParentId == null);
            return View(categories.OrderBy(c=>c.Order).ToList());
        }

        public ActionResult _Category(List<Category> Checkeds)
        {
            ViewBag.Checked = Checkeds;
            var categories = db.Categories.Where(c => c.CategoryParentId == null);
            return PartialView("_Category", categories.ToList());
        }

        //
        // GET: /Admin/Category/Details/5

        public ActionResult Details(int id = 0)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // GET: /Admin/Category/Create

        public ActionResult Create()
        {
            ViewBag.CategoryParentId = new SelectList(db.Categories, "Id", "Name");
            return View(new Category());
        }

        //
        // POST: /Admin/Category/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.CreatedDate = category.ModifiedDate = DateTime.Now;
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryParentId = new SelectList(db.Categories, "Id", "Name", category.CategoryParentId);
            return View(category);
        }

        //
        // GET: /Admin/Category/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryParentId = new SelectList(db.Categories, "Id", "Name", category.CategoryParentId);
            return View(category);
        }

        //
        // POST: /Admin/Category/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.ModifiedDate = DateTime.Now;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryParentId = new SelectList(db.Categories, "Id", "Name", category.CategoryParentId);
            return View(category);
        }

        //
        // GET: /Admin/Category/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /Admin/Category/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Insert(int size = 10)
        {
            for (int i = 0; i < size; i++)
            {
                db.Categories.Add(new Category
                                      {
                                          Name = "Category "+(i+1),
                                          SEOName = "category-"+(i+1),
                                          CreatedDate = DateTime.Now,
                                          ModifiedDate = DateTime.Now,
                                          Published = true,
                                          Order = 0
                                      });
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteAll()
        {
            foreach (var film in db.Categories)
            {
                db.Categories.Remove(film);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ReInsert(int size = 10)
        {
            foreach (var film in db.Categories)
            {
                db.Categories.Remove(film);
            }
            db.SaveChanges();
            return RedirectToAction("Insert", new { size = size });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}