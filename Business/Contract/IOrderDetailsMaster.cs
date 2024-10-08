﻿using DataCarrier.ApplicationModels.Common;
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

namespace Business.Contract
{
    public interface IOrderDetailsMaster
    {
        Task<ApiGetResponseModel<List<OrderDetailsVM>>> GetOrderDetailList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<OrderDetailsVM>> GetOrderDetailById(long orderDetailId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveOrderDetail(OrderDetailsVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateOrderDetail(OrderDetailsVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<PlaceOrderResponse>> SavePlaceOrder(PlaceOrder data, IDbTransaction transaction = null);
        Task<ApiGetResponseModel<List<OrderShippingDetailsVM>>> GetOrderShippingDetailsList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> CompleteOrderbyOrderId(long orderId, IDbTransaction transaction = null);
    }
}
