using DataCarrier.ApplicationModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using Business.Contract;
using DataCarrier.ViewModels;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesMaster _categorymaster;
        public CategoriesController(ICategoriesMaster categorymaster)
        {
            _categorymaster = categorymaster;
        }

        [HttpPost("getCategoryList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<CategoriesVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<CategoriesVM>>> GetAllCountries([FromBody] ApiGetRequestModel request)
        {
            return await _categorymaster.getCategoryList(request);
        }

        [HttpPost("getCategoryById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<CategoriesVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<CategoriesVM>> GetCategoryById([FromBody] GetByIdVM request)
        {
            return await _categorymaster.getCategoryById(request.Id);
        }


        [HttpPost("saveCategory")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveCategory([FromBody] CategoriesVM data)
        {
            return await _categorymaster.saveCategory(data, transaction: null);
        }

        [HttpPost("updateCategory")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateCategory([FromBody] CategoriesVM data)
        {
            return await _categorymaster.updateCategory(data, transaction: null);
        }


    }
}
