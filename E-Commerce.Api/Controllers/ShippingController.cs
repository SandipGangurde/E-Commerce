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
    public class ShippingController : ControllerBase
    {
        private readonly IShippingMaster _shippingMaster;
        public ShippingController(IShippingMaster shippingMaster)
        {
            _shippingMaster = shippingMaster;
        }

        [HttpPost("getShippingList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<ShippingVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<ShippingVM>>> GetShippingList([FromBody] ApiGetRequestModel request)
        {
            return await _shippingMaster.GetShippingList(request);
        }

        [HttpPost("getShippingById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<ShippingVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<ShippingVM>> GetShippingById([FromBody] GetByIdVM request)
        {
            return await _shippingMaster.GetShippingById(request.Id);
        }


        [HttpPost("saveShipping")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveShipping([FromBody] ShippingVM data)
        {
            return await _shippingMaster.SaveShipping(data, transaction: null);
        }

        [HttpPost("updateShipping")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateShipping([FromBody] ShippingVM data)
        {
            return await _shippingMaster.UpdateShipping(data, transaction: null);
        }
    }
}
