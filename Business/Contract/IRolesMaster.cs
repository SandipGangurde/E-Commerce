using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contract
{
    public interface IRolesMaster
    {
        Task<ApiGetResponseModel<List<RoleVM>>> GetRoleList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<RoleVM>> GetRoleById(long roleId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveRole(RoleVM role, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateRole(RoleVM role, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteRole(long roleId, IDbTransaction transaction = null);
    }
}
