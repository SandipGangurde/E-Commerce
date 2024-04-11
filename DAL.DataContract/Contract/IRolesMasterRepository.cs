using DataCarrier.ApplicationModels.Common;
using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataContract.Contract
{
    public interface IRolesMasterRepository
    {
        Task<ApiGetResponseModel<List<Role>>> GetRoleList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Role>> GetRoleById(long roleId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveRole(Role role, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateRole(Role role, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteRole(long roleId, IDbTransaction transaction = null);
    }
}
