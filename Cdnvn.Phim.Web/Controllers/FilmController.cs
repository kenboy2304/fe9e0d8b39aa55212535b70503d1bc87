using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cdnvn.Phim.Entities;
using Cdnvn.Phim.Web.Models;
using PagedList;

namespace Cdnvn.Phim.Web.Controllers
{
    public class FilmController : Controller
    {
        private FilmContext db = new FilmContext();
        //
        // GET: /Film/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _NewsFilm(string category = "", int page = 1, int size = 9)
        {
            if (page <= 0) page = 1;
            var films = db.Films.Where(f => f.Published);
            if (!String.IsNullOrWhiteSpace(category))
                films = films.Where(f => f.Categories.Any(c => c.SEOName.Equals(category)));
            return PartialView(films.OrderByDescending(f => f.Id).ToPagedList(page, size));
        }
        private void SEO()
        {
            ViewBag.Title = "Name";
        }

        public ActionResult _RelatedFilms(string category = "")
        {
            var films = db.Films.Where(f => f.Published && f.Categories.Any(c => c.SEOName.Equals(category)));
            return PartialView("_RelatedFilms", films.OrderByDescending(f => f.Id).ToPagedList(1, 10));
        }

        public JsonResult _RelatedFilmAjax(string category = "", int skin = 10, int size = 10)
        {
            var films = db.Films.Where(f => f.Published && f.Categories.Any(c => c.SEOName.Equals(category)))
                .OrderByDescending(f => f.Id).Skip(skin).Take(size).Select(f => new
                                                                            {
                                                                                Name = f.Name,
                                                                                SEOName = f.SEOName,
                                                                                ImageUrl = f.ImageUrl
                                                                            });
            if (string.IsNullOrWhiteSpace(category))
            {
                films = db.Films.Where(f => f.Published)
                .OrderByDescending(f => f.Id).Skip(skin).Take(size).Select(f => new
                {
                    Name = f.Name,
                    SEOName = f.SEOName,
                    ImageUrl = f.ImageUrl
                });
            }
            return Json(films.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ErrorFilm(string SEName, int order)
        {
            var filmpart = db.FilmParts.Single(p => p.Order == order && p.SEOName.StartsWith(SEName));
            filmpart.isError = true;
            db.Entry(filmpart).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilmDetails(string keyword = "")
        {
            ViewBag.Title = "Detail";
            var film = db.Films.Single(f => f.SEOName.Equals(keyword));
            return View(film);
        }

    }
}
