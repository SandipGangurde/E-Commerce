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
        Task<ApiGetResponseModel<List<Categories>>> GetCategoryList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Categories>> GetCategoryById(long categoryId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveCategory(Categories data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateCategory(Categories data, IDbTransaction transaction = null);
    }
}
