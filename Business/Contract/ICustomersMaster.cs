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
    public interface ICustomersMaster
    {
        Task<ApiGetResponseModel<List<CustomersVM>>> GetCustomerList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<CustomersVM>> GetCustomerById(long customerId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveCustomer(CustomersVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateCustomer(CustomersVM data, IDbTransaction transaction = null);
    }
}
