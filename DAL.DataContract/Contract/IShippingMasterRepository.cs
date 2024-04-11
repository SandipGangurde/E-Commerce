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
    public interface IShippingMasterRepository
    {
        Task<ApiGetResponseModel<List<Shipping>>> GetShippingList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Shipping>> GetShippingById(long shippingId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveShipping(Shipping data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateShipping(Shipping data, IDbTransaction transaction = null);
    }
}
