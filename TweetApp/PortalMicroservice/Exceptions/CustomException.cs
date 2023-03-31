using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalMicroservice.Exceptions
{
    [Serializable]
    public class CustomException:Exception
    {
        public CustomException()
        {

        }
        public CustomException(string message) : base(String.Format($" Exception Occured: {message}"))
        {

        }
    }
}
