using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Model;

namespace UserMicroservice.Services
{
   public interface IUserServices
    {
        public string AddUserService(User user);
        public string ForgotPasswordService(string email, string password,string key);
        // public string GetUserService(string email, string password);
        public User GetUserService(string email);
    }
}
