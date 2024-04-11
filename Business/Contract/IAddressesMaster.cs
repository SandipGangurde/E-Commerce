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
    public interface IAddressesMaster
    {
        Task<ApiGetResponseModel<List<AddressesVM>>> GetAddressList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<AddressesVM>> GetAddressById(long addressId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveAddress(AddressesVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateAddress(AddressesVM data, IDbTransaction transaction = null);
    }
}
