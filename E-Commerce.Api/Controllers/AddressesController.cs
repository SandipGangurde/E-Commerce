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
    public class AddressesController : ControllerBase
    {
        private readonly IAddressesMaster _addressesmaster;
        public AddressesController(IAddressesMaster addressesmaster)
        {
            _addressesmaster = addressesmaster;
        }

        [HttpPost("getAddressList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<AddressesVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<AddressesVM>>> GetAddressList([FromBody] ApiGetRequestModel request)
        {
            return await _addressesmaster.GetAddressList(request);
        }

        [HttpPost("getAddressById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<AddressesVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<AddressesVM>> GetAddressById([FromBody] GetByIdVM request)
        {
            return await _addressesmaster.GetAddressById(request.Id);
        }


        [HttpPost("saveAddress")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveAddress([FromBody] AddressesVM data)
        {
            return await _addressesmaster.SaveAddress(data, transaction: null);
        }

        [HttpPost("updateAddress")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateAddress([FromBody] AddressesVM data)
        {
            return await _addressesmaster.UpdateAddress(data, transaction: null);
        }
    }
}
