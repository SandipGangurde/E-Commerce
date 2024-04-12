using Business.Contract;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ApplicationModels.Products.Response;
using DataCarrier.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsMaster _productmaster;
        public ProductsController(IProductsMaster productmaster)
        {
            _productmaster = productmaster;
        }

        [HttpPost("getProductList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<ProductsVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<ProductsVM>>> GetAllProductList([FromBody] ApiGetRequestModel request)
        {
            return await _productmaster.GetProductList(request);
        }

        [HttpPost("getProductById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<ProductsVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<ProductsVM>> GetProductById([FromBody] GetByIdVM request)
        {
            return await _productmaster.GetProductById(request.Id);
        }


        [HttpPost("saveProduct")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveProduct([FromBody] ProductsVM data)
        {
            return await _productmaster.SaveProduct(data, transaction: null);
        }

        [HttpPost("updateProduct")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateProduct([FromBody] ProductsVM data)
        {
            return await _productmaster.UpdateProduct(data, transaction: null);
        }

        [HttpPost("getProductDetailList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<ProductDetailVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<ProductDetailVM>>> GetProductDetailList([FromBody] ApiGetRequestModel request)
        {
            return await _productmaster.GetProductDetailList(request);
        }
    }
}
