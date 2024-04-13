using Business.Contract;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRolesMaster _userRoleMaster;
        public UserRolesController(IUserRolesMaster userRolesMaster)
        {
            _userRoleMaster = userRolesMaster;
        }

        [HttpPost("getUserRoleList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<UserRoleVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<UserRoleVM>>> GetUserRoleList([FromBody] ApiGetRequestModel request)
        {
            return await _userRoleMaster.GetUserRoleList(request);
        }

        [HttpPost("getUserRoleById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<UserRoleVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<UserRoleVM>> GetUserRoleById([FromBody] GetByIdVM request)
        {
            return await _userRoleMaster.GetUserRoleById(request.Id);
        }

        [HttpPost("saveUserRole")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveUserRole([FromBody] UserRoleVM data)
        {
            return await _userRoleMaster.SaveUserRole(data, transaction: null);
        }

        [HttpPost("updateUserRole")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateUserRole([FromBody] UserRoleVM data)
        {
            return await _userRoleMaster.UpdateUserRole(data, transaction: null);
        }
    }
}
