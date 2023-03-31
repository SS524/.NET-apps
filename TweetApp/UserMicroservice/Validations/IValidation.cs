using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserMicroservice.Validations
{
   public interface IValidation
    {
        public bool DateValidation(string date);
        public bool EmailValidation(string email);
        public bool PasswordValidation(string password);

    }
}
