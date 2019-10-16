

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Trippin_Wansch
{
    class User_Request
    {
        List<User> allUser;
        private static readonly HttpClient client = new HttpClient() { BaseAddress = new Uri("https://services.odata.org/TripPinRESTierService/(S(gfyapivcxr5wmab2lxogm3ac))/") };

        public User_Request()
        {        }

        public void readJson()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("config.json").Build();
            var jsonInput = config["jsonFile"];
            StreamReader r = new StreamReader(jsonInput);
            var json = r.ReadToEnd();
            allUser = JsonConvert.DeserializeObject<List<User>>(json);
            
        }

        public async Task compareUser()
        {
           
            foreach(var item in allUser)
            {
                HttpResponseMessage response = await client.GetAsync($"People('{item.UserName}')");
                Console.WriteLine(response.StatusCode.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    HttpResponseMessage postResponse = await client.PostAsync("People",
                           new StringContent(System.Text.Json.JsonSerializer.Serialize(new
                           {
                               item.UserName,
                               item.FirstName,
                               item.LastName,
                               Emails = new List<string> { item.Email },
                               AddressInfo = new List<object>{
                                    new
                                    {
                                        item.Address,
                                        City = new
                                        {
                                            Name = item.CityName,
                                            CountryRegion = item.Country,
                                            Region = item.Country
                                        }
                                    }
                               }
                           }), Encoding.UTF8, "application/json"));
                    Console.WriteLine("New added User: "+item.UserName);
                }
                else
                {
                    Console.WriteLine("User already exists");
                }
            }
        }
    }
}
