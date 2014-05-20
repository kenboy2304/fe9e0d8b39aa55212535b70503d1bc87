using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using PagedList;

namespace Cdnvn.Phim.Web.Models
{
    public class UserRepository:IUserRepository, IDisposable
    {
        private UsersContext context;

        public UserRepository(UsersContext context)
        {
            this.context = context;
        }

        public IPagedList<UserProfile> GetUsers(int page, int size)
        {
           return  context.UserProfiles.OrderByDescending(u=>u.UserId).ToPagedList(page, size);
        }

        public UserProfile GetStudentByID(int studentId)
        {
            return context.UserProfiles.Single(f => f.UserId.Equals(studentId));
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}