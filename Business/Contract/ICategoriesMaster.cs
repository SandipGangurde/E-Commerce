using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contract
{
    public interface ICategoriesMaster
    {
        Task<ApiGetResponseModel<List<CategoriesVM>>> getCategoryList(ApiGetRequestModel request, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<CategoriesVM>> getCategoryById(long CategoryId, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<long>> saveCategory(CategoriesVM data, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<bool>> updateCategory(CategoriesVM data, IDbTransaction transaction = null);

    }
}
