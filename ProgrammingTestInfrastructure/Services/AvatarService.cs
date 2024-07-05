using Microsoft.Extensions.Configuration;
using ProgrammingTestInfrastructure.Interfaces;
using ProgrammingTestInfrastructure.Models;
using System.Text.Json;

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

        public async Task<string> GetDicebearAvatarUrl(string identifier)
        {
            string result = "";

            var imageRequestUrl = $"{_configuration["TestImagesUrl"]}{identifier.Last()}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(imageRequestUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var responseObject = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

                if (responseObject != null)
                {
                    result = responseObject["url"].ToString() ?? "";
                }
            }

            return result;
        }
    }
}
