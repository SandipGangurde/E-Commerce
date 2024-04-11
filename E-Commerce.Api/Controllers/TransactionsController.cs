using Business.Contract;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsMaster _transactionMaster;
        public TransactionsController(ITransactionsMaster transactionMaster)
        {
            _transactionMaster = transactionMaster;
        }

        [HttpPost("getTransactionList")]
        [ProducesResponseType(typeof(ApiGetResponseModel<List<TransactionsVM>>), (int)HttpStatusCode.OK)]
        public async Task<ApiGetResponseModel<List<TransactionsVM>>> GetTransactionList([FromBody] ApiGetRequestModel request)
        {
            return await _transactionMaster.GetTransactionList(request);
        }

        [HttpPost("getTransactionById")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<TransactionsVM>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<TransactionsVM>> GetTransactionById([FromBody] GetByIdVM request)
        {
            return await _transactionMaster.GetTransactionById(request.Id);
        }


        [HttpPost("saveTransaction")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<long>> SaveTransaction([FromBody] TransactionsVM data)
        {
            return await _transactionMaster.SaveTransaction(data, transaction: null);
        }

        [HttpPost("updateTransaction")]
        [ProducesResponseType(typeof(ApiGenericResponseModel<bool>), (int)HttpStatusCode.OK)]
        public async Task<ApiGenericResponseModel<bool>> UpdateTransaction([FromBody] TransactionsVM data)
        {
            return await _transactionMaster.UpdateTransaction(data, transaction: null);
        }
    }
}
