using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationMicroservice.Authenticate
{
    public interface ITokenGenerator
    {
       public string GenerateToken(string email);
    }
}
