using Business.Contract;
using DataCarrier.ApplicationModels.Auth.Request;
using DataCarrier.ApplicationModels.Auth.Response;
using DataCarrier.ApplicationModels.Common;
using DataModel.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class AuthController : ControllerBase
    {
        private readonly IUsersMaster _userMaster;
        public AuthController(IUsersMaster userMaster)
        {
            _userMaster = userMaster;
        }
        //[HttpPost("login")]
        //public async Task<ApiGenericResponseModel<LoginResponseModel>> Login([FromBody] LoginRequestModel payload)
        //{
        //    var apiResponse = new ApiGenericResponseModel<LoginResponseModel>();

        //    try
        //    {

                
        //        var user = await _userMaster.GetUserList.GetUserByEmailAsync(payload.Email);

        //        // Check if the user exists
        //        if (user != null)
        //        {
        //            // Check if the user's email is confirmed
        //            //var isEmailConfirmed = await _unitOfWork.ConfirmationToken.IsEmailConfirmedAsync(user.UserID);

        //            if (!user.IsEmailConfirmed)
        //            {
        //                // User's email is not confirmed
        //                apiResponse.Success = false;
        //                apiResponse.Message = "Please confirm your email address to log in.";
        //                return apiResponse;
        //            }

        //            // Check if the password is correct
        //            if (_helper.VerifyPassword(payload.Password, user.PasswordHash))
        //            {

        //                // Generate JWT token
        //                var jwtToken = _helper.GenerateJwtToken(user);

        //                var response = new LoginResponseVM();
        //                response.Token = jwtToken;
        //                response.UserID = user.UserID;
        //                response.Username = user.UserName;
        //                response.UserRole = user.RoleName;
        //                // Return the token in the response
        //                apiResponse.Success = true;
        //                apiResponse.Result = response;
        //            }
        //            else
        //            {
        //                // Invalid password
        //                apiResponse.Success = false;
        //                apiResponse.Message = "Invalid password.";
        //            }
        //        }
        //        else
        //        {
        //            // User not found
        //            apiResponse.Success = false;
        //            apiResponse.Message = "User not found.";
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        apiResponse.Success = false;
        //        apiResponse.Message = ex.Message;
        //        Logger.Instance.Error("SQL Exception:", ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        apiResponse.Success = false;
        //        apiResponse.Message = ex.Message;
        //        Logger.Instance.Error("Exception:", ex);
        //    }

        //    return apiResponse;
        //}


        //[HttpPost("register")]
        //public async Task<ApiResponse<string>> Register([FromBody] User user)
        //{
        //    var apiResponse = new ApiResponse<string>();

        //    try
        //    {
        //        // Begin a transaction
        //        _unitOfWork.BeginTransaction();

        //        // Find the user by email
        //        var getUser = await _unitOfWork.Users.GetUserByEmailAsync(user.Email);

        //        if (getUser == null)
        //        {
        //            // Convert hashed password
        //            user.PasswordHash = _helper.HashPassword(user.PasswordHash);

        //            // Add user to the database
        //            var userID = await _unitOfWork.Users.AddAsync(user);

        //            // Generate confirmation token
        //            var token = _helper.GenerateEmailConfirmationToken();

        //            if (userID != null)
        //            {
        //                // Save confirmation token to database
        //                await _unitOfWork.ConfirmationToken.AddAsync(new ConfirmationTokens { Token = token, UserID = int.Parse(userID), ExpiryDateTime = DateTime.UtcNow.AddHours(24) });
        //                // Save UserRole for new User
        //                var userRole = new UserRole()
        //                {
        //                    UserID = int.Parse(userID),
        //                    RoleID = (int)UserRoleEnum.User,
        //                    UserRolesID = 0,
        //                    IsActive = true,
        //                };
        //                await _unitOfWork.UserRoles.AddAsync(userRole);
        //            }

        //            // Commit the transaction
        //            _unitOfWork.CommitTransaction();
        //            apiResponse.Message = "Account created successfully. Verify email and login.";
        //            apiResponse.Success = true;
        //            apiResponse.Result = userID;

        //            var emailConfig = this._helper.EmailConfiguration();
        //            var name = user.FirstName + " " + user.LastName;
        //            var email = new EmailVM()
        //            {
        //                Name = name,
        //                Email = user.Email,
        //                Subject = "Registration",
        //                HtmlBody = EmailTemplates.GetAccountCreationEmailTemplate(emailConfig.ApplicationName, name, token)
        //            };

        //            // Send email
        //            bool emailSent = await _unitOfWork.EmailSender.SendEmailAsync(email, emailConfig);
        //        }
        //        else
        //        {
        //            apiResponse.Success = false;
        //            apiResponse.Message = "User already exists.";
        //            Logger.Instance.Error("User already exists with email: " + user.Email);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Roll back the transaction in case of an error
        //        _unitOfWork.RollbackTransaction();

        //        apiResponse.Success = false;
        //        apiResponse.Message = "An error occurred.";
        //        Logger.Instance.Error("Exception:", ex);
        //    }

        //    return apiResponse;
        //}


        //[HttpPost("emailVerification")]
        //public async Task<ApiResponse<LoginResponseVM>> EmailVerification([FromBody] EmailVerificationVM payload)
        //{
        //    var apiResponse = new ApiResponse<LoginResponseVM>();

        //    try
        //    {
        //        // Find the user by username or email
        //        var user = await _unitOfWork.Users.GetUserByEmailAsync(payload.Email);

        //        // Check if the user exists
        //        if (user != null)
        //        {
        //            // Check if the user's email is confirmed
        //            //var isEmailConfirmed = await _unitOfWork.ConfirmationToken.IsEmailConfirmedAsync(user.UserID);

        //            if (!user.IsEmailConfirmed)
        //            {
        //                await _unitOfWork.ConfirmationToken.DeleteExpiredTokensAsync();


        //                if (user.Token == payload.Token)
        //                {
        //                    var updateUser = await _unitOfWork.Users.GetByIdAsync(user.UserID);
        //                    updateUser.IsEmailConfirmed = true;
        //                    updateUser.IsUserActive = true;
        //                    await _unitOfWork.Users.UpdateAsync(updateUser);
        //                    await _unitOfWork.ConfirmationToken.DeleteAsync(user.Token);

        //                    var emailConfig = this._helper.EmailConfiguration();
        //                    var name = user.FirstName + " " + user.LastName;
        //                    var email = new EmailVM()
        //                    {
        //                        Name = name,
        //                        Email = user.Email,
        //                        Subject = "Email Verification Successful",
        //                        HtmlBody = EmailTemplates.GetGeneralEmailTemplate(emailConfig.ApplicationName, name, "Email has been verified successfully.")
        //                    };

        //                    // Send email
        //                    bool emailSent = await _unitOfWork.EmailSender.SendEmailAsync(email, emailConfig);

        //                    // Email verification successful
        //                    apiResponse.Success = true;
        //                    apiResponse.Message = "Email has been verified successfully.";
        //                    return apiResponse;
        //                }
        //                else
        //                {
        //                    // Invalid token
        //                    apiResponse.Success = false;
        //                    apiResponse.Message = "Invalid token.";
        //                    return apiResponse;
        //                }
        //            }
        //            else
        //            {
        //                // Invalid token
        //                apiResponse.Success = false;
        //                apiResponse.Message = "This email has already been verified.";
        //                return apiResponse;
        //            }
        //        }
        //        else
        //        {
        //            // User not found
        //            apiResponse.Success = false;
        //            apiResponse.Message = "User not found.";
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        apiResponse.Success = false;
        //        apiResponse.Message = ex.Message;
        //        Logger.Instance.Error("SQL Exception:", ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        apiResponse.Success = false;
        //        apiResponse.Message = ex.Message;
        //        Logger.Instance.Error("Exception:", ex);
        //    }

        //    return apiResponse;
        //}
    }
}
