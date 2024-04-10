using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using E_Commerce.Api.Utilities;

namespace E_Commerce.Api.Filter
{
    public class AuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {
        #region ===[ Private Members ]=============================================================

        private readonly string _apiKey;
        private readonly string _apiKeySecondary;
        private readonly bool _canUseSecondaryApiKey;
        private readonly IConfiguration _configuration;

        private readonly IHelper _helper;


        #endregion

        #region ===[ Constructor ]=================================================================

        public AuthorizationFilterAttribute(IConfiguration configuration, IHelper helper)
        {
            _apiKey = configuration["SecretKeys:ApiKey"];
            _apiKeySecondary = configuration["SecretKeys:ApiKeySecondary"];
            _canUseSecondaryApiKey = configuration["SecretKeys:UseSecondaryKey"] == "True";
            _configuration = configuration;
            _helper = helper;
        }

        #endregion

        #region ===[ Public Methods ]==============================================================

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            #region ============ old implementaition =======================
            /*
            var apiKeyHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            //var authController = new Controllers.AuthController();

            if (apiKeyHeader.Any())
            {
                var keys = new List<string>
                {
                    _apiKey
                };

                if (_canUseSecondaryApiKey)
                {
                    keys.AddRange(_apiKeySecondary.Split(','));
                }

                if (keys.FindIndex(x => x.Equals(apiKeyHeader, StringComparison.OrdinalIgnoreCase)) == -1)
                {
                    //context.Result = authController.NotAuthorized();
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                //context.Result = authController.NotAuthorized();
                context.Result = new UnauthorizedResult();
            }
            */
            #endregion ============ old implementaition =======================

            #region ============= JWT token Implementation ==========================
            var jwtToken = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(jwtToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Validate the JWT token
            if (!_helper.ValidateJwtToken(jwtToken))
            {
                context.Result = new UnauthorizedResult();
            }
            #endregion ============= JWT token Implementation ==========================
        }

        #endregion
    }
}
