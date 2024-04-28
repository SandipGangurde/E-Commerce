using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contract
{
    public interface IUserRolesMaster
    {
        Task<ApiGetResponseModel<List<UserRoleVM>>> GetUserRoleList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<UserRoleVM>> GetUserRoleById(long userRoleId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveUserRole(UserRoleVM userRole, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateUserRole(UserRoleVM userRole, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteUserRole(long userRoleId, IDbTransaction transaction = null);
        Task<ApiGetResponseModel<List<VuUserRole>>> GetUserRoleDetailList(ApiGetRequestModel request, IDbTransaction transaction = null);
    }
}
