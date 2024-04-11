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
        Task<ApiGetResponseModel<List<CategoriesVM>>> GetCategoryList(ApiGetRequestModel request, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<CategoriesVM>> GetCategoryById(long categoryId, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<long>> SaveCategory(CategoriesVM data, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<bool>> UpdateCategory(CategoriesVM data, IDbTransaction transaction = null);

    }
}
