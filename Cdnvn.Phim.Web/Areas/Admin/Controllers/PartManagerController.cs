using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Cdnvn.Phim.Entities;
using Cdnvn.Phim.Web.Models;

namespace Cdnvn.Phim.Web.Areas.Admin.Controllers
{
    public class PartManagerController : Controller
    {
        private readonly FilmContext db = new FilmContext();

        //
        // GET: /Admin/PartManager/

        public ActionResult Index()
        {
            var filmparts = db.FilmParts.Include(f => f.Film);
            return View(filmparts.ToList());
        }

        //
        // GET: /Admin/PartManager/Details/5

        public ActionResult Details(int id = 0)
        {
            FilmPart filmpart = db.FilmParts.Find(id);
            if (filmpart == null)
            {
                return HttpNotFound();
            }
            return View(filmpart);
        }

        //
        // GET: /Admin/PartManager/Create

        public ActionResult Create()
        {
            ViewBag.FilmId = new SelectList(db.Films, "Id", "ImageUrl");
            return View();
        }

        //
        // POST: /Admin/PartManager/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FilmPart filmpart)
        {
            if (ModelState.IsValid)
            {
                db.FilmParts.Add(filmpart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FilmId = new SelectList(db.Films, "Id", "ImageUrl", filmpart.FilmId);
            return View(filmpart);
        }

        //
        // GET: /Admin/PartManager/Edit/5

        public ActionResult Edit(int id = 0)
        {
            FilmPart filmpart = db.FilmParts.Find(id);
            if (filmpart == null)
            {
                return HttpNotFound();
            }
            ViewBag.FilmId = new SelectList(db.Films, "Id", "ImageUrl", filmpart.FilmId);
            return View(filmpart);
        }

        //
        // POST: /Admin/PartManager/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FilmPart filmpart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filmpart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FilmId = new SelectList(db.Films, "Id", "ImageUrl", filmpart.FilmId);
            return View(filmpart);
        }

        //
        // GET: /Admin/PartManager/Delete/5

        public ActionResult Delete(int id = 0)
        {
            FilmPart filmpart = db.FilmParts.Find(id);
            if (filmpart == null)
            {
                return HttpNotFound();
            }
            return View(filmpart);
        }

        //
        // POST: /Admin/PartManager/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FilmPart filmpart = db.FilmParts.Find(id);
            db.FilmParts.Remove(filmpart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private ActionResult DeleteAll()
        {
            foreach (var filmPart in db.FilmParts)
            {
                db.FilmParts.Remove(filmPart);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}