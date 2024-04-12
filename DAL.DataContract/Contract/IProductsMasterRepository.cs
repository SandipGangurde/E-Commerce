using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataContract.Contract
{
    public interface IProductsMasterRepository
    {
        Task<ApiGetResponseModel<List<Products>>> GetProductList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Products>> GetProductById(long productId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveProduct(Products data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateProduct(Products data, IDbTransaction transaction = null);
        Task<ApiGetResponseModel<List<VuProductDetails>>> GetProductDetailList(ApiGetRequestModel request, IDbTransaction transaction = null);
    }
}
