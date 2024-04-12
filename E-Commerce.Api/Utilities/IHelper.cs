using DataCarrier.ApplicationModels.Common;
using DataCarrier.ApplicationModels.Common.Email;
using DataCarrier.ViewModels;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace E_Commerce.Api.Utilities
{
    public interface IHelper
    {
        EmailConfiguration EmailConfiguration();
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string passwordHash);
        public string GenerateJwtToken(VuUserDetails user);
        public ClaimsPrincipal ValidateJwtToken(string jwtToken);
        //public bool ValidateJwtToken(string token);
        public string GenerateEmailConfirmationToken(int length = 32);
    }
}
