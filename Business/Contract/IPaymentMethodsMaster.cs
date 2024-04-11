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
    public interface IPaymentMethodsMaster
    {
        Task<ApiGetResponseModel<List<PaymentMethodsVM>>> GetPaymentMethodList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<PaymentMethodsVM>> GetPaymentMethodById(long paymentMethodId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SavePaymentMethod(PaymentMethodsVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdatePaymentMethod(PaymentMethodsVM data, IDbTransaction transaction = null);
    }
}
