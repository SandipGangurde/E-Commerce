using AutoMapper;
using Business.Contract;
using DAL.DataContract.Contract;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using RepositoryOperations.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service
{
    public class PaymentMethodsMaster : IPaymentMethodsMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly IPaymentMethodsMasterRepository _repo;
        private readonly ITransactions _localTransaction;

        public PaymentMethodsMaster(ILogger logger, IMapper map, IPaymentMethodsMasterRepository repo, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _localTransaction = transactions;
        }

        public async Task<ApiGetResponseModel<List<PaymentMethodsVM>>> GetPaymentMethodList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<PaymentMethodsVM>> response = new ApiGetResponseModel<List<PaymentMethodsVM>>();

            try
            {
                var data = await _repo.GetPaymentMethodList(request, transaction);

                if (data.IsSuccess && data.Result != null && data.Result.Count > 0)
                {
                    List<PaymentMethodsVM> mapresponse = _map.Map<List<PaymentMethodsVM>>(data.Result);
                    response.Result = mapresponse;
                    response.TotalRecords = data.TotalRecords;
                }
                else
                {
                    response.Result = null;
                    response.TotalRecords = 0;
                    response.ErrorMessage.Add("No records found");
                }
                response.IsSuccess = true;
            }
            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                response.IsSuccess = false;
                response.Result = null;
                response.TotalRecords = 0;
                response.ErrorMessage.Add(exception.Message);
            }

            return response;
        }

        public async Task<ApiGenericResponseModel<PaymentMethodsVM>> GetPaymentMethodById(long paymentMethodId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<PaymentMethodsVM> response = new ApiGenericResponseModel<PaymentMethodsVM>();

            try
            {
                var data = await _repo.GetPaymentMethodById(paymentMethodId, transaction);

                if (data.IsSuccess && data.Result != null)
                {
                    PaymentMethodsVM mapresponse = _map.Map<PaymentMethodsVM>(data.Result);
                    response.Result = mapresponse;
                }
                else
                {
                    response.Result = null;
                    response.ErrorMessage.Add("No records found");
                }
                response.IsSuccess = true;
            }
            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                response.IsSuccess = false;
                response.Result = null;
                response.ErrorMessage.Add(exception.Message);
            }

            return response;
        }

        public async Task<ApiGenericResponseModel<long>> SavePaymentMethod(PaymentMethodsVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();

            try
            {
                PaymentMethods mapmodel = _map.Map<PaymentMethods>(data);
                var saveResponse = await _repo.SavePaymentMethod(mapmodel, transaction);

                if (saveResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Result = saveResponse.Result;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = default;
                    response.ErrorMessage.AddRange(saveResponse.ErrorMessage);
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                response.IsSuccess = false;
                response.Result = default;
                response.ErrorMessage.Add(exception.Message);
            }

            return response;
        }

        public async Task<ApiGenericResponseModel<bool>> UpdatePaymentMethod(PaymentMethodsVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();

            try
            {
                PaymentMethods mapmodel = _map.Map<PaymentMethods>(data);
                var saveResponse = await _repo.UpdatePaymentMethod(mapmodel, transaction);

                if (saveResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Result = saveResponse.Result;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = default;
                    response.ErrorMessage.AddRange(saveResponse.ErrorMessage);
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                response.IsSuccess = false;
                response.Result = default;
                response.ErrorMessage.Add(exception.Message);
            }

            return response;
        }
    }

}
