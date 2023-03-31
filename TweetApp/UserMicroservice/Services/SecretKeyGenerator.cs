using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserMicroservice.Services
{
    public class SecretKeyGenerator
    {
        public string GenerateKey()
        {
            Random random = new Random();
            var bytes = new Byte[10];
            random.NextBytes(bytes);

            var keyArray = Array.ConvertAll(bytes, x => x.ToString("X2"));
            var keyStr = String.Concat(keyArray);
            var key = keyStr.ToLower();
            return key;
        }
    }
}
