using DataCarrier.ApplicationModels.Common;
using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataContract.Contract
{
    public interface ICategoriesMasterRepository
    {
        Task<ApiGetResponseModel<List<Categories>>> getCategoryList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Categories>> getCategoryById(long CategoryId, IDbTransaction transaction = null);
        //Task<ApiGenericResponseModel<Categories>> getCategoryByCode(string CategoryCode, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> saveCategory(Categories data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> updateCategory(Categories data, IDbTransaction transaction = null);
    }
}
