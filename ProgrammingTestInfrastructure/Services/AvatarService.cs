using Microsoft.Extensions.Configuration;
using ProgrammingTestInfrastructure.Interfaces;
using ProgrammingTestInfrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProgrammingTestInfrastructure.Services
{
    public class AvatarService : IAvatarService
    {
        private IConfiguration _configuration;

        public AvatarService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        
        public string GetAvatarUrl(string identifier)
        {
            string result = "";
            
            using (var db = new DataContext())
            {
                var dbImage = db.Images.Where(i => i.Id == identifier.Last()).FirstOrDefault();
                if (dbImage != null)
                {
                    result = dbImage.Url;
                }
            }

            return result;
        }

        public string GetDicebearAvatarUrl(string identifier)
        {
            string result = "";

            var imageRequestUrl = $"{_configuration["TestImagesUrl"]}{identifier.Last()}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(imageRequestUrl).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;

                var responseObject = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

                if (responseObject != null)
                {
                    result = responseObject["url"].ToString();
                }
            }

            return result;
        }
    }
}
