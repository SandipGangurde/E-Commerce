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
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistMaster _wishlistMaster;
        public WishlistController(IWishlistMaster wishlistMaster)
        {
            _wishlistMaster = wishlistMaster;
        }

        [HttpPost("wetWishlist")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<WishlistVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<WishlistVM>>> GetWishlist([FromBody] ApiGetRequestModel request)
        {
            return await _wishlistMaster.GetWishlist(request);
        }

        [HttpPost("getWishlistById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<WishlistVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<WishlistVM>> GetWishlistById([FromBody] GetByIdVM request)
        {
            return await _wishlistMaster.GetWishlistById(request.Id);
        }


        [HttpPost("saveWishlist")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveWishlist([FromBody] WishlistVM data)
        {
            return await _wishlistMaster.SaveWishlist(data, transaction: null);
        }

        [HttpPost("updateWishlist")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateWishlist([FromBody] WishlistVM data)
        {
            return await _wishlistMaster.UpdateWishlist(data, transaction: null);
        }
    }
}
