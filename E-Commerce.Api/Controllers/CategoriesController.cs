using DataCarrier.ApplicationModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using Business.Contract;
using DataCarrier.ViewModels;
using E_Commerce.Api.Filter;
using Microsoft.AspNetCore.Authorization;
using DataModel.Entities;

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
        public async Task<ApiGetResponseModel<List<CategoriesVM>>> GetAllCategoryList([FromBody] ApiGetRequestModel request)
        {
            return await _categorymaster.GetCategoryList(request);
        }
        
        [HttpPost("getCategoryById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<CategoriesVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<CategoriesVM>> GetCategoryById([FromBody] GetByIdVM request)
        {
            return await _categorymaster.GetCategoryById(request.Id);
        }
        
        [HttpPost("saveCategory")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveCategory([FromBody] CategoriesVM data)
        {
            return await _categorymaster.SaveCategory(data, transaction: null);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("updateCategory")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateCategory([FromBody] CategoriesVM data)
        {
            return await _categorymaster.UpdateCategory(data, transaction: null);
        }

    }
}
