using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Model;

namespace UserMicroservice.Repository
{
   public interface IUserRepository
    {
        public string AddUser(User user);
        public string ForgotPassword(string email,string password,string key);
        //public string GetUser(string email, string password);
        public User GetUser(string email);
    }
}
