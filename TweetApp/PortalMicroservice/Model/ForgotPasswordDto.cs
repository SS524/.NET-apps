using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalMicroservice.Model
{
    public class ForgotPasswordDto
    {
        
        public string Email { get; set; }
        public string Password { get; set; }
        public string SecretKey { get; set; }
    }
}
