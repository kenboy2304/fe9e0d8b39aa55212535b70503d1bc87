using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Cdnvn.Phim.Web.Filters;
using Cdnvn.Phim.Web.Models;
using PagedList;
using WebMatrix.WebData;

namespace Cdnvn.Phim.Web.Areas.Admin.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AccountManagerController : Controller
    {
        private readonly UsersContext uDb = new UsersContext();
        private SimpleMembershipProvider membershipProvider = (SimpleMembershipProvider)Membership.Provider;
        private readonly SimpleRoleProvider roleProvider = (SimpleRoleProvider)Roles.Provider;
        //
        // GET: /Admin/Account/

        public ActionResult Index(int page=1,int size =10)
        {
           
            return View(uDb.UserProfiles.OrderByDescending(u => u.UserId).ToPagedList(page, size));
        }

        //
        // GET: /Admin/Account/Create
        [InitializeSimpleMembership]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Account/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [InitializeSimpleMembership]
        public ActionResult Create(RegisterModel register, string[] roles=null)
        {
            if(ModelState.IsValid)
            {
                WebSecurity.CreateUserAndAccount(register.UserName, register.Password);
                roleProvider.AddUsersToRoles(new[]{register.UserName},roles );
                return RedirectToAction("Index");
            }
            return View(register);
        }

        //
        // GET: /Admin/Account/Edit/5
        [InitializeSimpleMembership]
        public ActionResult Edit(string username)
        {
            return View((object)username);
        }

        //
        // POST: /Admin/Account/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [InitializeSimpleMembership]
        public ActionResult Edit(string username, string[] roles)
        {
            var rolesExist =  roleProvider.GetRolesForUser(username);
            roleProvider.RemoveUsersFromRoles(new[]{username},rolesExist);
            roleProvider.AddUsersToRoles(new[] { username }, roles);
            
            return RedirectToAction("Index");
        }

        [InitializeSimpleMembership]
        public ActionResult SetPassword(string username)
        {
            return View((object)username);
        }

        [HttpPost]
        [InitializeSimpleMembership]
        [ValidateAntiForgeryToken]
        public ActionResult SetPassword(string username,string password)
        {
            var token = WebSecurity.GeneratePasswordResetToken(username);
            WebSecurity.ResetPassword(token, password);
            return RedirectToAction("Index");
        }

        //
        // GET: /Admin/Account/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Account/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        protected override void Dispose(bool disposing)
        {
            uDb.Dispose();
            base.Dispose(disposing);
        }
    }
}
