using Business.Contract;
using Business.Service;
using DataCarrier.ApplicationModels.Auth.Request;
using DataCarrier.ApplicationModels.Auth.Response;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using E_Commerce.Api.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class AuthController : ControllerBase
    {
        private readonly IUsersMaster _userMaster;
        private readonly IHelper _helper;
        public AuthController(IUsersMaster userMaster, IHelper helper)
        {
            _userMaster = userMaster;
            _helper = helper;
        }
        [HttpPost("login")]
        public async Task<ApiGenericResponseModel<LoginResponseModel>> Login([FromBody] LoginRequestModel payload)
        {
            var apiResponse = new ApiGenericResponseModel<LoginResponseModel>();

            try
            {
                var user = await _userMaster.GetUserDetailByEmail(payload.Email);

                // Check if the user exists
                if (user != null && user.Result != null)
                {
                    

                    // Check if the password is correct
                    if (_helper.VerifyPassword(payload.Password, user.Result.PasswordHash))
                    {

                        
                        // Generate JWT token
                        var jwtToken = _helper.GenerateJwtToken(user.Result);

                        var response = new LoginResponseModel();
                        response.Token = jwtToken;
                        response.UserId = user.Result.UserId;
                        response.Email = user.Result.Email;
                        response.UserRole = user.Result.RoleName;
                        response.FullName = user.Result.FirstName + " " + user.Result.LastName;
                        // Return the token in the response
                        apiResponse.IsSuccess = true;
                        apiResponse.Result = response;
                    }
                    else
                    {
                        // Invalid password
                        apiResponse.IsSuccess = false;
                        apiResponse.ErrorMessage.Add("Invalid password.");
                    }
                }
                else
                {
                    // User not found
                    apiResponse.IsSuccess = false;
                    apiResponse.ErrorMessage.Add("Invalid Username.");
                }
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessage.Add(ex.Message);
            }

            return apiResponse;
        }

        
        [HttpPost("register")]
        public async Task<ApiGenericResponseModel<long>> Register([FromBody] UsersVM request)
        {
            var apiResponse = new ApiGenericResponseModel<long>();
            try
            {
                if (request != null)
                {
                    var user = await _userMaster.GetUserDetailByEmail(request.Email);

                    if (user.Result == null)
                    {
                        // Convert hashed password
                        request.PasswordHash = _helper.HashPassword(request.PasswordHash);
                        request.IsUserActive = true;
                        apiResponse =  await _userMaster.SaveUser(request);

                    }
                    else
                    {
                        apiResponse.IsSuccess = false;
                        apiResponse.ErrorMessage.Add("User already exists.");
                    }

                }
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessage.Add(ex.Message);
            }

            return apiResponse;

        }

        /*
        [HttpPost("emailVerification")]
        public async Task<ApiResponse<LoginResponseVM>> EmailVerification([FromBody] EmailVerificationVM payload)
        {
            var apiResponse = new ApiResponse<LoginResponseVM>();

            try
            {
                // Find the user by username or email
                var user = await _unitOfWork.Users.GetUserByEmailAsync(payload.Email);

                // Check if the user exists
                if (user != null)
                {
                    // Check if the user's email is confirmed
                    //var isEmailConfirmed = await _unitOfWork.ConfirmationToken.IsEmailConfirmedAsync(user.UserID);

                    if (!user.IsEmailConfirmed)
                    {
                        await _unitOfWork.ConfirmationToken.DeleteExpiredTokensAsync();


                        if (user.Token == payload.Token)
                        {
                            var updateUser = await _unitOfWork.Users.GetByIdAsync(user.UserID);
                            updateUser.IsEmailConfirmed = true;
                            updateUser.IsUserActive = true;
                            await _unitOfWork.Users.UpdateAsync(updateUser);
                            await _unitOfWork.ConfirmationToken.DeleteAsync(user.Token);

                            var emailConfig = this._helper.EmailConfiguration();
                            var name = user.FirstName + " " + user.LastName;
                            var email = new EmailVM()
                            {
                                Name = name,
                                Email = user.Email,
                                Subject = "Email Verification Successful",
                                HtmlBody = EmailTemplates.GetGeneralEmailTemplate(emailConfig.ApplicationName, name, "Email has been verified successfully.")
                            };

                            // Send email
                            bool emailSent = await _unitOfWork.EmailSender.SendEmailAsync(email, emailConfig);

                            // Email verification successful
                            apiResponse.Success = true;
                            apiResponse.Message = "Email has been verified successfully.";
                            return apiResponse;
                        }
                        else
                        {
                            // Invalid token
                            apiResponse.Success = false;
                            apiResponse.Message = "Invalid token.";
                            return apiResponse;
                        }
                    }
                    else
                    {
                        // Invalid token
                        apiResponse.Success = false;
                        apiResponse.Message = "This email has already been verified.";
                        return apiResponse;
                    }
                }
                else
                {
                    // User not found
                    apiResponse.Success = false;
                    apiResponse.Message = "User not found.";
                }
            }
            catch (SqlException ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                Logger.Instance.Error("SQL Exception:", ex);
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                Logger.Instance.Error("Exception:", ex);
            }

            return apiResponse;
        }

        [HttpPost("Get")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<GetUserDetailVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<GetUserDetailVM>> GetUserDetailByEmail()
        {
            return await _userMaster.GetUserDetailByEmail("admin@admin.com");
        }
        */
    }
}
