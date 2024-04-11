using Business.Contract;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersMaster _ordersmaster;
        public OrdersController(IOrdersMaster ordersmaster)
        {
            _ordersmaster = ordersmaster;
        }

        [HttpPost("getOrderList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<OrdersVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<OrdersVM>>> GetAllCountries([FromBody] ApiGetRequestModel request)
        {
            return await _ordersmaster.GetOrderList(request);
        }

        [HttpPost("getOrderById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<OrdersVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<OrdersVM>> GetOrderById([FromBody] GetByIdVM request)
        {
            return await _ordersmaster.GetOrderById(request.Id);
        }


        [HttpPost("saveOrder")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveOrder([FromBody] OrdersVM data)
        {
            return await _ordersmaster.SaveOrder(data, transaction: null);
        }

        [HttpPost("updateOrder")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateOrder([FromBody] OrdersVM data)
        {
            return await _ordersmaster.UpdateOrder(data, transaction: null);
        }
    }
}
