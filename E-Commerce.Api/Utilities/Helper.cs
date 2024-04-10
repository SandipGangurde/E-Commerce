using DataCarrier.ApplicationModels.Common;
using DataCarrier.ApplicationModels.Common.Email;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce.Api.Utilities
{
    public class Helper : IHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public Helper(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        }

       

        public EmailConfiguration EmailConfiguration()
        {
            var emailConfig = _configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            return emailConfig;
        }


        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedInputBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashedInputString = BitConverter.ToString(hashedInputBytes).Replace("-", "").ToLower();

                // Compare the hashed input password with the stored password hash
                return hashedInputString.Equals(passwordHash);
            }
        }

        public string GenerateJwtToken(RequestUserDetails user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["SecretKeys:ApiKey"];

            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.RoleName),
                // Add more claims as needed
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1), // Token expiration time
                //Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["SecretKeys:ApiKey"];
            var key = Encoding.ASCII.GetBytes(secretKey);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // Set to true to validate the issuer
                    ValidateAudience = false, // Set to true to validate the audience
                    ValidIssuer = "http://localhost:4200", // Set to your backend API server URL
                    ValidAudience = "YourAudience", // Specify the expected audience
                    ClockSkew = TimeSpan.Zero // Adjust the acceptable clock skew
                }, out SecurityToken validatedToken);

                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                // Token expired
                return false;
            }
            catch (SecurityTokenInvalidIssuerException)
            {
                // Invalid issuer
                return false;
            }
            catch (SecurityTokenInvalidAudienceException)
            {
                // Invalid audience
                return false;
            }
            catch (SecurityTokenValidationException)
            {
                // Token validation failed
                return false;
            }
            catch (Exception)
            {
                // Other exceptions
                return false;
            }
        }

        public string GenerateEmailConfirmationToken(int length = 32)
        {
            // Generate a random token
            byte[] tokenBytes = new byte[length];
            using (var rngProvider = new RNGCryptoServiceProvider())
            {
                rngProvider.GetBytes(tokenBytes);
            }

            // Convert the token bytes to a string
            string token = Convert.ToBase64String(tokenBytes);
            // Replace characters that are not URL-safe
            token = token.Replace("+", "-").Replace("/", "_").Replace("=", "");

            return token;
        }
    }
}
