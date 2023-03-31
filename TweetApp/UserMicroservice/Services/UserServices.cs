using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Exceptions;
using UserMicroservice.Model;
using UserMicroservice.Repository;
using UserMicroservice.Validations;

namespace UserMicroservice.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepo;
        private readonly IValidation _validation;
        public UserServices(IUserRepository userRepo,IValidation validation)
        {
           
            _userRepo = userRepo;
            _validation = validation;
        }
       
        public string AddUserService(User user)
        {
            try
            {
                if (String.IsNullOrEmpty(user.FirstName))
                {
                    return "First name can't be null";
                }
                if (String.IsNullOrEmpty(user.LastName))
                {
                    return "Last name can't be null";
                }

                if (!_validation.EmailValidation(user.Email))
                {
                    return "Please provide a valid email address";
                }
                //if (!_validation.DateValidation(user.DateOfBirth))
                //{
                //    return "Please provide a valid Date of Birth in this format DD-MM-YYYY";
                //}
                if (!_validation.PasswordValidation(user.Password))
                {
                    return "Password length should be greater than 8 and less than 14,must contain one upper case albhabet,one lower case albhabet,one numeric value,once special character";
                }
               if(_userRepo.AddUser(user)== "User is already added")
                {
                    return "User is already added";
                }
                _userRepo.AddUser(user);
                return "You have been successfully added, your secret id is: " + user.SecretKey;
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public string ForgotPasswordService(string email,string password,string key)
        {
            try
            {
                if (_userRepo.ForgotPassword(email,password,key) == "Invalid user")
                {
                    return "Invalid user from Userservices";
                }
                if(_userRepo.ForgotPassword(email,password,key)== "You are providing wrong secret key")
                {
                    return "You are providing wrong secret key";
                }

                    if (!_validation.PasswordValidation(password))
                {
                    return "Password length should be greater than 8 and less than 14,must contain one upper case alphabet,one lower case alphabet,one numeric value,once special character";
                }
                _userRepo.ForgotPassword(email,password,key);
                return "Forgotten password has been updated successfully";
                
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public User GetUserService(string email)
        {
            try
            {
                if(_userRepo.GetUser(email)==null)
                {
                    return null;
                }
                return _userRepo.GetUser(email);
                     
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
