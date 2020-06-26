using Akirs.client.DL;
using Akirs.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.repository
{
    public interface IUserRepository : IRepository<AspNetUser>
    {
        IEnumerable<AspNetUser> GetUsers(string EnrollId);

        AspNetUser GetUser(string EnrollId);
        AspNetUser GetByUserName(string UserName);
        AspNetUser ValidateUser(string Email, string password);
        AspNetUser ValidateEnrollment(string EnrollmentID, string password);
        List<AspNetUser> GetById(int Itbid);
        AspNetUser GetUserById(int Itbid);
        AspNetUser GetByUserEmail(string Email);
        short GetCountByUserName(string UserName);
        short GetCountByEmail(string Email);
        AspNetUser ChangePassword(string UserName, string password);

        bool ResetPassword(string email, string passwordHash);       
       
    }
}
