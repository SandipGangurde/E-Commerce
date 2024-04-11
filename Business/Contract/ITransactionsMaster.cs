using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contract
{
    public interface ITransactionsMaster
    {
        Task<ApiGetResponseModel<List<TransactionsVM>>> GetTransactionList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<TransactionsVM>> GetTransactionById(long transactionId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveTransaction(TransactionsVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateTransaction(TransactionsVM data, IDbTransaction transaction = null);
    }
}
