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
    public class ReviewsController : BaseApiController
    {
        private readonly IReviewsMaster _reviewMaster;
        public ReviewsController(IReviewsMaster reviewMaster)
        {
            _reviewMaster = reviewMaster;
        }

        [HttpPost("getReviewList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<ReviewsVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<ReviewsVM>>> GetReviewList([FromBody] ApiGetRequestModel request)
        {
            return await _reviewMaster.GetReviewList(request);
        }

        [HttpPost("getReviewById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<ReviewsVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<ReviewsVM>> GetReviewById([FromBody] GetByIdVM request)
        {
            return await _reviewMaster.GetReviewById(request.Id);
        }


        [HttpPost("saveReview")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveReview([FromBody] ReviewsVM data)
        {
            return await _reviewMaster.SaveReview(data, transaction: null);
        }

        [HttpPost("updateReview")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateReview([FromBody] ReviewsVM data)
        {
            return await _reviewMaster.UpdateReview(data, transaction: null);
        }
    }
}
