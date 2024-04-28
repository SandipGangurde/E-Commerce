using DataCarrier.ApplicationModels.Common;
using DataCarrier.ApplicationModels.OrderDetails.Request;
using DataCarrier.ApplicationModels.OrderDetails.Response;
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
        Task<ApiGenericResponseModel<PlaceOrderResponse>> SavePlaceOrder(PlaceOrder data, IDbTransaction transaction = null);
        Task<ApiGetResponseModel<List<VuOrderShippingDetails>>> GetOrderShippingDetailsList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> CompleteOrderbyOrderId(long orderId, IDbTransaction transaction = null);
    }
}
