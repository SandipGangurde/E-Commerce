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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesMaster _roleMaster;
        public RolesController(IRolesMaster rolesMaster)
        {
            _roleMaster = rolesMaster;
        }

        [HttpPost("getRoleList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<RoleVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<RoleVM>>> GetRoleList([FromBody] ApiGetRequestModel request)
        {
            return await _roleMaster.GetRoleList(request);
        }

        [HttpPost("getRoleById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<RoleVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<RoleVM>> GetRoleById([FromBody] GetByIdVM request)
        {
            return await _roleMaster.GetRoleById(request.Id);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("saveRole")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveRole([FromBody] RoleVM data)
        {
            return await _roleMaster.SaveRole(data, transaction: null);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("updateRole")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateRole([FromBody] RoleVM data)
        {
            return await _roleMaster.UpdateRole(data, transaction: null);
        }
    }
}
