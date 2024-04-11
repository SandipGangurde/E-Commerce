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
    public interface IOrdersMaster
    {
        Task<ApiGetResponseModel<List<OrdersVM>>> GetOrderList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<OrdersVM>> GetOrderById(long orderId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveOrder(OrdersVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateOrder(OrdersVM data, IDbTransaction transaction = null);
    }
}
