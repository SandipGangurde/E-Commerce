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
    public interface IDiscountsMaster
    {
        Task<ApiGetResponseModel<List<DiscountsVM>>> GetDiscountList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<DiscountsVM>> GetDiscountById(long discountId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveDiscount(DiscountsVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateDiscount(DiscountsVM data, IDbTransaction transaction = null);
    }
}
