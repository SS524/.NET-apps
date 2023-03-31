using System;
using System.Linq;
using UserMicroservice.Context;
using UserMicroservice.Exceptions;
using UserMicroservice.Model;
using UserMicroservice.Services;

namespace UserMicroservice.Repository
{
    public class UserRepository : IUserRepository
    {
        
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }
       
        public string AddUser(User user)
        {
            try
            {
                foreach (var u in _context.UserTbl.ToList())
                {
                    if (u.Email == user.Email)
                    {
                        return "User is already added";
                    }
                }
                user.SecretKey = new SecretKeyGenerator().GenerateKey();
                _context.Add(user);
                _context.SaveChanges();
                return "You have been successfully added,your secret id is: "+user.SecretKey;
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public string ForgotPassword(string email,string password,string key)
        {
            try
            {
                var user_obj=new User();
                foreach (var u in _context.UserTbl.ToList())
                {
                    if (u.Email == email)
                    {
                        user_obj = u;
                        break;
                    }
                }
               // var user_obj = users.Find(u => u.Email == email).FirstOrDefault();
                if (user_obj.Email!=email)
                {
                    return "This user does not exist";
                }
                if (user_obj.SecretKey != key)
                {
                    return "You are providing wrong secret key";
                }
                _context.UserTbl.Find(user_obj.Id).Password = password;
                _context.SaveChanges();
                //user_obj.Password = password;
                //users.FindOneAndUpdate<User>(Builders<User>.Filter.Eq("Email", email),
                //    Builders<User>.Update.Set("Password", user_obj.Password));
                return "Password has been rest successfully";
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public User GetUser(string email)
        {
            try
            {
                foreach (var u in _context.UserTbl.ToList())
                {
                    if (u.Email == email)
                    {
                        return u;
                    }
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
