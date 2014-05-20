using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cdnvn.Phim.Entities;
using Cdnvn.Phim.Web.Models;

namespace Cdnvn.Phim.Web.Controllers
{
    
    public class CategoryController : Controller
    {
        private FilmContext db = new FilmContext();
        //
        // GET: /Category/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _CategoryLayout(string current="")
        {
            ViewBag.CategoryCurrent = current;
            IQueryable<Category> cats = db.Categories.Where(f => f.Published).OrderBy(c=>c.Order);
            return PartialView("_CategoryMenu", cats);
        }
        public ActionResult CategoryDetails(string[] keyword)
        {
            var key = "";
            for (var i = keyword.Length - 1; i >= 0; i--)
            {
                if (!string.IsNullOrWhiteSpace(keyword[i]))
                {
                    key = keyword[i];
                    break;
                }
            }
            var category = db.Categories.Single(c => c.SEOName.Equals(key));

            return View(category);
        }
    }
}
