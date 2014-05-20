using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using Cdnvn.Phim.Web.Models;

namespace Cdnvn.Phim.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            Database.SetInitializer(new InitSecurityDb());
                            context.Database.Initialize(true);
                            // Create the SimpleMembership database without Entity Framework migration schema

                            // ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                        else
                        {
                            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }

    }
    public class InitSecurityDb : DropCreateDatabaseIfModelChanges<UsersContext>
    {
        protected override void Seed(UsersContext context)
        {

            WebSecurity.InitializeDatabaseConnection("DefaultConnection",
               "UserProfile", "UserId", "UserName", autoCreateTables: true);
            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            string[] rs = new[] { "Admin", "Manager", "User" };

            foreach (var r in rs)
            {
                if (!roles.RoleExists(r))
                {
                    roles.CreateRole(r);
                }
            }
            if (membership.GetUser("admin", false) == null)
            {
                membership.CreateUserAndAccount("admin@cdnvn.com", "p@sswo!D");
                roles.AddUsersToRoles(new[] { "admin@cdnvn.com" }, rs);
            }
            var check = true;
            foreach (var res in roles.GetRolesForUser("admin@cdnvn.com"))
            {
                if (res.Contains("Admin"))
                {
                    check = false;
                }
            }
            if (!check)
            {
                roles.AddUsersToRoles(new[] { "admin@cdnvn.com" }, new[] { "Admin" });
            }
        }
    }
}
