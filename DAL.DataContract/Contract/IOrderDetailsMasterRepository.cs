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
    public interface IOrderDetailsMasterRepository
    {
        Task<ApiGetResponseModel<List<OrderDetails>>> GetOrderDetailList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<OrderDetails>> GetOrderDetailById(long orderDetailId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveOrderDetail(OrderDetails data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateOrderDetail(OrderDetails data, IDbTransaction transaction = null);
    }
}
