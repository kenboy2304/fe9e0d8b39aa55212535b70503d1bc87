using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Cdnvn.Phim.Web.Models
{
    interface IUserRepository : IDisposable
    {
        IPagedList<UserProfile> GetUsers(int page, int size);
        UserProfile GetStudentByID(int studentId);
        void Save();
    }
}
