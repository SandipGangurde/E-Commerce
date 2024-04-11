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
    public interface IPaymentMethodsMasterRepository
    {
        Task<ApiGetResponseModel<List<PaymentMethods>>> GetPaymentMethodList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<PaymentMethods>> GetPaymentMethodById(long paymentMethodId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SavePaymentMethod(PaymentMethods data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdatePaymentMethod(PaymentMethods data, IDbTransaction transaction = null);
    }
}
