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
    public interface IUsersMaster
    {
        Task<ApiGetResponseModel<List<UsersVM>>> GetUserList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<UsersVM>> GetUserById(long UserId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveUser(UsersVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateUser(UsersVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteUser(long UserId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<VuUserDetails>> GetUserDetailByEmail(string email, IDbTransaction transaction = null);

    }
}
