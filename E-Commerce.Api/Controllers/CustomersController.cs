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
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersMaster _customersMaster;
        public CustomersController(ICustomersMaster customersMaster)
        {
            _customersMaster = customersMaster;
        }

        [HttpPost("getCustomerList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<CustomersVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<CustomersVM>>> GetAllCustomerList([FromBody] ApiGetRequestModel request)
        {
            return await _customersMaster.GetCustomerList(request);
        }

        [HttpPost("getCustomerById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<CustomersVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<CustomersVM>> GetCustomerById([FromBody] GetByIdVM request)
        {
            return await _customersMaster.GetCustomerById(request.Id);
        }


        [HttpPost("saveCustomer")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveCustomer([FromBody] CustomersVM data)
        {
            return await _customersMaster.SaveCustomer(data, transaction: null);
        }

        [HttpPost("updateCustomer")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateCustomer([FromBody] CustomersVM data)
        {
            return await _customersMaster.UpdateCustomer(data, transaction: null);
        }
    }
}
