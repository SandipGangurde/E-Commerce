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
    public interface IAddressesMasterRepository
    {
        Task<ApiGetResponseModel<List<Addresses>>> GetAddressList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Addresses>> GetAddressById(long addressId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveAddress(Addresses data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateAddress(Addresses data, IDbTransaction transaction = null);
    }
}
