using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalMicroservice.Validation
{
   public interface IValidations
    {
        public bool DateValidation(string date);
        public bool EmailValidation(string email);
        public bool PasswordValidation(string password);
    }
}
