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
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemsMaster _cartItemsmaster;
        public CartItemsController(ICartItemsMaster cartItemsmaster)
        {
            _cartItemsmaster = cartItemsmaster;
        }

        [HttpPost("getCartItemList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<CartItemsVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<CartItemsVM>>> GetCartItemList([FromBody] ApiGetRequestModel request)
        {
            return await _cartItemsmaster.GetCartItemList(request);
        }

        [HttpPost("getCartItemById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<CartItemsVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<CartItemsVM>> GetCartItemById([FromBody] GetByIdVM request)
        {
            return await _cartItemsmaster.GetCartItemById(request.Id);
        }


        [HttpPost("saveCartItem")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveCartItem([FromBody] CartItemsVM data)
        {
            return await _cartItemsmaster.SaveCartItem(data, transaction: null);
        }

        [HttpPost("updateCartItem")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateCartItem([FromBody] CartItemsVM data)
        {
            return await _cartItemsmaster.UpdateCartItem(data, transaction: null);
        }
    }
}
