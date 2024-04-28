using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataContract.Contract
{
    public interface IUserRolesMasterRepository
    {
        Task<ApiGetResponseModel<List<UserRole>>> GetUserRoleList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<UserRole>> GetUserRoleById(long userRoleId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveUserRole(UserRole userRole, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateUserRole(UserRole userRole, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteUserRole(long userRoleId, IDbTransaction transaction = null);

        Task<ApiGetResponseModel<List<VuUserRole>>> GetUserRoleDetailList(ApiGetRequestModel request, IDbTransaction transaction = null);
    }
}
