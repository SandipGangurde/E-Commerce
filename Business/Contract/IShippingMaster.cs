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
    public interface IShippingMaster
    {
        Task<ApiGetResponseModel<List<ShippingVM>>> GetShippingList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<ShippingVM>> GetShippingById(long shippingId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveShipping(ShippingVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateShipping(ShippingVM data, IDbTransaction transaction = null);
    }
}
