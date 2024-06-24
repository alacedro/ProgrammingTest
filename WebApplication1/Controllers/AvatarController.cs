using Microsoft.AspNetCore.Mvc;
using ProgrammingTestInfrastructure.Interfaces;
using ProgrammingTestInfrastructure.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvatarController : Controller
    {
        private IAvatarService _avatarService;
        private IConfiguration _configuration;


        public AvatarController(IConfiguration configuration, IAvatarService avatarService)
        {
            _avatarService = avatarService;
            _configuration = configuration;

        }

        private bool IsLastCharBetween1and5(string identifier)
        {
            var regex = new Regex("[12345]");
            return !string.IsNullOrEmpty(identifier) ? regex.IsMatch(identifier.Last().ToString()) : false;
        }

        private bool IsLastCharBetween6and9(string identifier)
        {
            var regex = new Regex("[6789]");
            return !string.IsNullOrEmpty(identifier) ? regex.IsMatch(identifier.Last().ToString()) : false;
        }

        private bool HasVowel(string identifier)
        {
            var vowelsRegex = new Regex("[aeiou]");
            return vowelsRegex.IsMatch(identifier);
        }

        private bool HasNonAlphanumeric(string identifier)
        {
            var nonAlphaRegex = new Regex(@"\W|_");
            return nonAlphaRegex.IsMatch(identifier);
        }

        [HttpGet(Name = "GetAvatar")]
        public AvatarResponse Get(string userIdentifier = null)
        {
            AvatarResponse avatarResponse = new AvatarResponse();
            
            int lastDigit;
            string format = _configuration["DicebearImagesUrlFormat"];
            string defaultUrl = string.Format(format, "default");


            //Case last character is [6,7,8,9]
            if (string.IsNullOrEmpty(userIdentifier))
            {
                avatarResponse.Url = defaultUrl;
            } 
            else if (IsLastCharBetween6and9(userIdentifier))
            {
                avatarResponse.Url = _avatarService.GetDicebearAvatarUrl(userIdentifier);

            }
            //Case last character is [1,2,3,4,5]
            else if (IsLastCharBetween1and5(userIdentifier))
            {
                avatarResponse.Url = _avatarService.GetAvatarUrl(userIdentifier);
            }
            //Case contains vowel
            else if (!string.IsNullOrEmpty(format) && HasVowel(userIdentifier))
            {
                char lastChar = userIdentifier.Last();
                avatarResponse.Url = string.Format(format, lastChar.ToString());
            }
            //Case contains non-alphanumeric
            else if (!string.IsNullOrEmpty(format) && HasNonAlphanumeric(userIdentifier))
            {
                Random r = new Random();
                int random = r.Next(5) + 1;
                avatarResponse.Url = string.Format(format, random);
            }
            else
            {
                avatarResponse.Url = defaultUrl;
            }

            return avatarResponse;

        }
    }
}
