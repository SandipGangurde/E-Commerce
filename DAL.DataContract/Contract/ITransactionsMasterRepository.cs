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
    public interface ITransactionsMasterRepository
    {
        Task<ApiGetResponseModel<List<Transactions>>> GetTransactionList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Transactions>> GetTransactionById(long transactionId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveTransaction(Transactions data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateTransaction(Transactions data, IDbTransaction transaction = null);
    }
}
