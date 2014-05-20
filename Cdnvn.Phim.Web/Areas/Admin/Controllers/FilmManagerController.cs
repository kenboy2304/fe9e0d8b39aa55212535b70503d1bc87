using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Cdnvn.Phim.Entities;
using Cdnvn.Phim.Web.Models;
using PagedList;

namespace Cdnvn.Phim.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class FilmManagerController : Controller
    {
        private readonly FilmContext db = new FilmContext();

        //
        // GET: /Admin/Film/

        public ActionResult Index(int page = 1, int size = 10)
        {
            if (page == 0) page = 1;
            var films = db.Films.OrderByDescending(f => f.Id).ToPagedList(page, size);
            return View(films);
        }

        //
        // GET: /Admin/Film/Error

        public ActionResult Error(int page = 1, int size = 10)
        {
            if (page == 0) page = 1;
            var films = db.Films.Where(f=>f.FilmParts.Any(p=>p.isError)).OrderByDescending(f => f.Id).ToPagedList(page, size);
            return View(films);
        }

        public ActionResult ErrorDetails(int id)
        {
            var film = db.Films.Single(f=>f.Id==id);
            return View(film);
        }
        
        [HttpPost]
        public ActionResult PartEdit(int id, string yId)
        {
            var part = db.FilmParts.Single(f => f.Id == id);
            part.YoutubeId = yId;
            part.isError = false;
            db.Entry(part).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ErrorDetails", new {id = part.FilmId});
        }

        //
        // GET: /Admin/Film/Create

        public ActionResult Create()
        {
            return View(new Film());
        }

        //
        // POST: /Admin/Film/Create

        private List<Category> GetCategory(int[] cat)
        {
            var categories = new List<Category>();
            if (cat.Any())
            {
                foreach (var id in cat)
                {
                    var ct = db.Categories.Single(c => c.Id == id);
                    categories.Add(ct);
                }
            }
            return categories;
        }

        private IEnumerable<string> RemoveSpace(string[] strs)
        {
            var strings = new List<string>();
            foreach (var str in strs)
            {
                if (str.Length > 0)
                    strings.Add(str);
            }
            return strings.ToArray();
        }

        private List<FilmPart> GetPart(string[] part, string url)
        {
            var partList = new List<FilmPart>();
            if (part == null) return new List<FilmPart>();
            var i = 0;
            foreach (var p in RemoveSpace(part))
            {
                i++;
                partList.Add(new FilmPart
                                  {
                                      CreatedDate = DateTime.Now,
                                      ModifiedDate = DateTime.Now,
                                      Published = true,
                                      SEOName = url + "-phan-" + i,
                                      YoutubeId = p,
                                      Order = i
                                  });
            }
            return partList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Film film, int[] Cat, string[] part)
        {
            if (ModelState.IsValid)
            {
                film.CreatedDate = film.ModifiedDate = DateTime.Now;
                film.Categories = GetCategory(Cat);
                film.FilmParts = GetPart(part, film.SEOName);
                if (part.Length > 0)
                    film.ImageUrl = string.Format("http://i1.ytimg.com/vi/{0}/mqdefault.jpg", part[0]);
                db.Films.Add(film);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(film);
        }

        public ActionResult ViewNumber(int[] data)
        {
            var r = "";
            foreach (var i in data)
            {
                r = r + i + ", ";
            }
            return Content("xem");
        }

        //
        // GET: /Admin/Film/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        //
        // POST: /Admin/Film/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, int[] Cat, string[] part, int[] pId)
        {
            var film = db.Films.Single(f => f.Id == Id);
            TryUpdateModel(film);
            if (ModelState.IsValid)
            {
                film.Categories.Clear();
                film.Categories = GetCategory(Cat);
                if (part.Length > 0)
                    film.ImageUrl = string.Format("http://i1.ytimg.com/vi/{0}/mqdefault.jpg", part[0]);
                db.Entry(film).State = EntityState.Modified;
                db.SaveChanges();
                UpdateParts(part, pId, film.Id, film.SEOName);
                return RedirectToAction("Index");
            }
            return View(film);
        }
        private void UpdateParts(string[] parts, int[] pId, int fId, string url)
        {
            if (pId == null) return;
            var tap = 0;
            for (var i = 0; i < parts.Count(); i++)
            {
                if (pId[i] == 0 && parts[i] != "")
                {
                    tap++;
                    db.FilmParts.Add(new FilmPart
                                         {
                                             FilmId = fId,
                                             CreatedDate = DateTime.Now,
                                             ModifiedDate = DateTime.Now,
                                             Published = true,
                                             SEOName = url + "-phan-" + tap,
                                             Order = tap,
                                             YoutubeId = parts[i]
                                         });
                }
                if (pId[i] != 0 && parts[i] == "")
                {
                    var del = db.FilmParts.Find(pId[i]);
                    db.FilmParts.Remove(del);
                }
                if (pId[i] != 0 && parts[i] != "")
                {
                    tap++;
                    var update = db.FilmParts.Find(pId[i]);
                    update.ModifiedDate = DateTime.Now;
                    update.YoutubeId = parts[i];
                    update.SEOName = url + "-phan-" + tap;
                    update.Order = tap;
                    db.Entry(update).State = EntityState.Modified;
                    
                }
            }
            db.SaveChanges();
        }


        //
        // GET: /Admin/Film/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        //
        // POST: /Admin/Film/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Film film = db.Films.Find(id);
            db.Films.Remove(film);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Insert(int size = 100)
        {
            string[] parts = new[] { "D04_wN-ffzQ", "w7nCoyB59NQ", "dY64HO_hTZo" };

            for (int i = 0; i < size; i++)
            {
                db.Films.Add(new Film
                {
                    Name = "Film " + (i + 1),
                    SEOName = "film-" + (i + 1),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Published = true,
                    FilmParts = GetPart(parts, "film-" + (i + 1)),
                    ImageUrl = string.Format("http://i1.ytimg.com/vi/{0}/mqdefault.jpg", parts[0]),
                    Categories = db.Categories.ToList(),
                    Order = 0
                });
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteAll()
        {
            foreach (var film in db.Films)
            {
                db.Films.Remove(film);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ReInsert(int size = 100)
        {
            foreach (var film in db.Films)
            {
                db.Films.Remove(film);
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