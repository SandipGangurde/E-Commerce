using Business.Contract;
using DataCarrier.ApplicationModels.Common;
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
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodsMaster _paymentMethodsMaster;
        public PaymentMethodsController(IPaymentMethodsMaster paymentMethodsMaster)
        {
            _paymentMethodsMaster = paymentMethodsMaster;
        }

        [HttpPost("getPaymentMethodList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<PaymentMethodsVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<PaymentMethodsVM>>> GetPaymentMethodList([FromBody] ApiGetRequestModel request)
        {
            return await _paymentMethodsMaster.GetPaymentMethodList(request);
        }

        [HttpPost("getPaymentMethodById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<PaymentMethodsVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<PaymentMethodsVM>> GetPaymentMethodById([FromBody] GetByIdVM request)
        {
            return await _paymentMethodsMaster.GetPaymentMethodById(request.Id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("savePaymentMethod")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SavePaymentMethod([FromBody] PaymentMethodsVM data)
        {
            return await _paymentMethodsMaster.SavePaymentMethod(data, transaction: null);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("updatePaymentMethod")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdatePaymentMethod([FromBody] PaymentMethodsVM data)
        {
            return await _paymentMethodsMaster.UpdatePaymentMethod(data, transaction: null);
        }
    }
}
