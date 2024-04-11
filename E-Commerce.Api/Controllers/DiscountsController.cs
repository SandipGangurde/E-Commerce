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
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountsMaster _discountMaster;
        public DiscountsController(IDiscountsMaster discountMaster)
        {
            _discountMaster = discountMaster;
        }

        [HttpPost("getDiscountList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<DiscountsVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<DiscountsVM>>> GetDiscountList([FromBody] ApiGetRequestModel request)
        {
            return await _discountMaster.GetDiscountList(request);
        }

        [HttpPost("getDiscountById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<DiscountsVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<DiscountsVM>> GetDiscountById([FromBody] GetByIdVM request)
        {
            return await _discountMaster.GetDiscountById(request.Id);
        }


        [HttpPost("saveDiscount")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveDiscount([FromBody] DiscountsVM data)
        {
            return await _discountMaster.SaveDiscount(data, transaction: null);
        }

        [HttpPost("updateDiscount")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateDiscount([FromBody] DiscountsVM data)
        {
            return await _discountMaster.UpdateDiscount(data, transaction: null);
        }
    }
}
