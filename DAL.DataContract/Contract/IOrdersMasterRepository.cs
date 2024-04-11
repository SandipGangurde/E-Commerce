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
    public interface IOrdersMasterRepository
    {
        Task<ApiGetResponseModel<List<Orders>>> GetOrderList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Orders>> GetOrderById(long orderId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveOrder(Orders data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateOrder(Orders data, IDbTransaction transaction = null);
    }
}
