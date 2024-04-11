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
    public interface IUsersMasterRepository
    { 
        Task<ApiGetResponseModel<List<Users>>> GetUserList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Users>> GetUserById(long UserId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveUser(Users data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateUser(Users data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteUser(long UserId, IDbTransaction transaction = null);
    }
}
