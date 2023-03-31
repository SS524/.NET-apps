using AuthorizationMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationMicroservice.Repository
{
    public interface IAuthorizationRepository
    {
        public User GetUser(string email);
    }
}
