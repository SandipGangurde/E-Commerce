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
    public interface IDiscountsMasterRepository
    {
        Task<ApiGetResponseModel<List<Discounts>>> GetDiscountList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Discounts>> GetDiscountById(long discountId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveDiscount(Discounts data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateDiscount(Discounts data, IDbTransaction transaction = null);
    }
}
