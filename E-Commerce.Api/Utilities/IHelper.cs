using DataCarrier.ApplicationModels.Common;
using DataCarrier.ApplicationModels.Common.Email;
using static System.Net.Mime.MediaTypeNames;

namespace E_Commerce.Api.Utilities
{
    public interface IHelper
    {
        EmailConfiguration EmailConfiguration();
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string passwordHash);
        public string GenerateJwtToken(RequestUserDetails user);
        public bool ValidateJwtToken(string token);
        public string GenerateEmailConfirmationToken(int length = 32);
    }
}
