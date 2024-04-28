using Business.Contract;
using Business.Service;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ApplicationModels.OrderDetails.Request;
using DataCarrier.ApplicationModels.OrderDetails.Response;
using DataCarrier.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsMaster _orderDetailMaster;
        public OrderDetailsController(IOrderDetailsMaster OrderDetailMaster)
        {
            _orderDetailMaster = OrderDetailMaster;
        }

        [HttpPost("getOrderDetailList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<OrderDetailsVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<OrderDetailsVM>>> GetAllOrderDetailList([FromBody] ApiGetRequestModel request)
        {
            return await _orderDetailMaster.GetOrderDetailList(request);
        }

        [HttpPost("getOrderDetailById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<OrderDetailsVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<OrderDetailsVM>> GetOrderDetailById([FromBody] GetByIdVM request)
        {
            return await _orderDetailMaster.GetOrderDetailById(request.Id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("saveOrderDetail")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveOrderDetail([FromBody] OrderDetailsVM data)
        {
            return await _orderDetailMaster.SaveOrderDetail(data, transaction: null);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("UpdateOrderDetail")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateOrderDetail([FromBody] OrderDetailsVM data)
        {
            return await _orderDetailMaster.UpdateOrderDetail(data, transaction: null);
        }

        [HttpPost("savePlaceOrder")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<PlaceOrderResponse>> SavePlaceOrder([FromBody] PlaceOrder data)
        {
            return await _orderDetailMaster.SavePlaceOrder(data, transaction: null);
        }

        [HttpPost("getOrderShippingDetailsList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<OrderShippingDetailsVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<OrderShippingDetailsVM>>> GetOrderShippingDetailsList([FromBody] ApiGetRequestModel request)
        {
            return await _orderDetailMaster.GetOrderShippingDetailsList(request);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("completeOrderbyOrderId")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> CompleteOrderbyOrderId([FromBody] GetByIdVM request)
        {
            return await _orderDetailMaster.CompleteOrderbyOrderId(request.Id);
        }
    }
}
