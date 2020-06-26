using Akirs.client.DL;
using Akirs.client.repository;
using Akirs.client.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class UserRepository : Repository<AspNetUser>, IUserRepository
    {
        public UserRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        public AspNetUser ChangePassword(string UserName, string password)
        {
            var ret = PlutoContext.AspNetUsers.Where(f => f.UserName == UserName).FirstOrDefault();
            if(ret != null)
            {
                var user = PlutoContext.AspNetUsers.Where(f => f.ItbId == ret.ItbId).FirstOrDefault();
                
                return user;
            }
            return null;
        }
        public AspNetUser ValidateUser(string UserName, string password)
        {
            var ret = PlutoContext.AspNetUsers.Where(f => f.UserName == UserName && f.PasswordHash == password).FirstOrDefault();
            return ret;
        }

        public AspNetUser ValidateEnrollment(string EnrollmentID, string password)
        {
            var ret = PlutoContext.AspNetUsers.Where(f => f.EnrollmentID == EnrollmentID && f.PasswordHash == password).FirstOrDefault();
            return ret;
        }
        public IEnumerable<AspNetUser> GetUsers(string EnrollId)
        {
            var ret = PlutoContext.AspNetUsers.Where(p => p.EnrollmentID == EnrollId).ToList();
            return ret;
        }
        public AspNetUser GetByUserName(string UserName)
        {
            var ret = PlutoContext.AspNetUsers.Where(p => p.UserName == UserName).FirstOrDefault();
            return ret;
        }

        public AspNetUser GetByUserEmail(string Email)
        {
            var ret = PlutoContext.AspNetUsers.Where(p => p.Email  == Email).FirstOrDefault();
            return ret;
        }
        public List<AspNetUser> GetById(int Itbid)
        {
            var ret = PlutoContext.AspNetUsers.Where(p => p.ItbId == Itbid).ToList();
            return ret;
        }
        public AspNetUser GetUserById(int Itbid)
        {
            var ret = PlutoContext.AspNetUsers.Where(p => p.ItbId == Itbid).FirstOrDefault();
            return ret;
        }
        public short GetCountByUserName(string UserName)
        {
            var ret = PlutoContext.AspNetUsers.Where(p => p.UserName == UserName).Count();
            return (short) ret;
        }
        public short GetCountByEmail(string Email)
        {
            var ret = PlutoContext.AspNetUsers.Where(p => p.Email == Email).Count();
            return (short)ret;
        }

        public bool ResetPassword(string email, string passwordHash)
        {
            var ret = PlutoContext.AspNetUsers.Where(p => p.Email == email).FirstOrDefault();
            //update pasword with password hash
            try
            {
                ret.PasswordHash = passwordHash;
                PlutoContext.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }

           
          //  return false;
        }

        public AspNetUser GetUser(string EnrollId)
        {
            var det = PlutoContext.AspNetUsers.Where(p => p.EnrollmentID == EnrollId).FirstOrDefault();
            return det;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}