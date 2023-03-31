using AuthorizationMicroservice.Model;
using AuthorizationMicroservice.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationMicroservice.Provider
{
    public class AuthorizationProvider
    {
        private readonly IAuthorizationRepository _authrepo;
        public AuthorizationProvider(IAuthorizationRepository authrepo)
        {
            _authrepo = authrepo;
        }
        public bool ValidateUser(LoginModel logInModel)
        {
            try
            {
                User obj;
                obj = _authrepo.GetUser(logInModel.Email);
                if (obj == null)
                {
                    return false;
                }
                else if (obj.Password != logInModel.Password)
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
