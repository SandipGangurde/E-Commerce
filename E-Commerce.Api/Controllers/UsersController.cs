using Business.Contract;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using E_Commerce.Api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersMaster _userMaster;
        private readonly IHelper _helper;
        public UsersController(IUsersMaster userMaster, IHelper helper)
        {
            _userMaster = userMaster;
            _helper = helper;
        }

        [HttpPost("getUserList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<UsersVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<UsersVM>>> GetUserList([FromBody] ApiGetRequestModel request)
        {
            return await _userMaster.GetUserList(request);
        }

        [HttpPost("getUserById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<UsersVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<UsersVM>> GetUserById([FromBody] GetByIdVM request)
        {
            return await _userMaster.GetUserById(request.Id);
        }

        [HttpPost("saveUser")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveUser([FromBody] UsersVM data)
        {
            data.PasswordHash = _helper.HashPassword(data.PasswordHash);
            return await _userMaster.SaveUser(data, transaction: null);
        }
        
        [HttpPost("updateUser")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateUser([FromBody] UsersVM data)
        {
            
            var existingUser = await _userMaster.GetUserById(data.UserId);
            if(data.PasswordHash != existingUser.Result.PasswordHash)
            {
                data.PasswordHash = _helper.HashPassword(data.PasswordHash);
            }

            return await _userMaster.UpdateUser(data, transaction: null);
        }
    }
}
