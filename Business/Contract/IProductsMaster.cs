using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contract
{
    public interface IProductsMaster
    {
        Task<ApiGetResponseModel<List<ProductsVM>>> GetProductList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<ProductsVM>> GetProductById(long productId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveProduct(ProductsVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateProduct(ProductsVM data, IDbTransaction transaction = null);
    }
}
