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
    public interface ICustomersMasterRepository
    {
        Task<ApiGetResponseModel<List<Customers>>> GetCustomerList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Customers>> GetCustomerById(long customerId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveCustomer(Customers data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateCustomer(Customers data, IDbTransaction transaction = null);
    }
}
