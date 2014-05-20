using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cdnvn.Phim.Web.Models;

namespace Cdnvn.Phim.Web.Controllers
{
    public class HomeController : Controller
    {
        private FilmContext db = new FilmContext();
        public ActionResult _HeadMeta(string title ="", string des ="", string keyword="", string imageUrl ="", string url ="" )
        {
            var setting = GenericModel.GetGeneric<SettingModels>(db.Settings);
            var data = new MetaTagModel
                           {
                               Name = "Phim Cơ Đốc",
                               Title = !string.IsNullOrWhiteSpace(title)?title+" | "+setting.WebName:setting.WebName,
                               Description = !string.IsNullOrWhiteSpace(des) ? des : setting.MetaDescription,
                               Keyword = !string.IsNullOrWhiteSpace(keyword) ? keyword : setting.MetaKeyword,
                               ImageUrl = !string.IsNullOrWhiteSpace(imageUrl) ? imageUrl : setting.LogoUrl,
                               Url =string.Format("http://{0}", Request.Url.Host)+url,
                               FacebookId = ConfigurationManager.AppSettings["AppId"],
                               GoogleId = ConfigurationManager.AppSettings["GoogleId"],
                           };
            return PartialView("_HeadMeta", data);
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Name";
            return View();

        }

        public ActionResult Film(string keyword)
        {
            return View();
        }


        public ActionResult Category(string[] keyword)
        {
            var key = "";
            for (var i = keyword.Length-1; i >= 0; i--)
            {
                if (!string.IsNullOrWhiteSpace(keyword[i]))
                {
                    key = keyword[i];
                    break;
                }
            }
            ViewBag.Current = key;
            return View((object)key);
        }
    }
}
