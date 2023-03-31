using AuthorizationMicroservice.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthorizationMicroservice.Repository
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private IConfiguration Configuration;

        Uri baseAddress;
        HttpClient client;
        public AuthorizationRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            baseAddress = new Uri(Configuration["Links:UserMicroservice"]);
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        public User GetUser(string email)
        {
            try
            {
                User user_obj = new User();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress +"/"+ email).Result;
                if (response.IsSuccessStatusCode)
                {

                    string data = response.Content.ReadAsStringAsync().Result;
                    user_obj = JsonConvert.DeserializeObject<User>(data);

                    return user_obj;
                }
                return user_obj;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
