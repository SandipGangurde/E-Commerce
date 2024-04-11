﻿using AutoMapper;
using DAL.DataContract.Contract;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using RepositoryOperations.ApplicationModels.Common;
using RepositoryOperations.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class PaymentMethodsMasterRepository : IPaymentMethodsMasterRepository
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<PaymentMethods> _repository;
        private readonly ITransactions _localTransaction;
        private readonly IMapper _map;

        public PaymentMethodsMasterRepository(ILogger logger, IMapper map, IGenericRepository<PaymentMethods> repository, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repository = repository;
            _localTransaction = transactions;
        }

        public async Task<ApiGetResponseModel<List<PaymentMethods>>> GetPaymentMethodList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<PaymentMethods>> response = new ApiGetResponseModel<List<PaymentMethods>>();
            try
            {
                RequestModel searchRequest = _map.Map<RequestModel>(request);
                var data = await _repository.Query(new PaymentMethods(), searchRequest, transaction: transaction);
                string filterQuery = SqlQueryHelper.GenerateQueryForTotalRecordsFound(new PaymentMethods(), searchRequest);
                int totalRecord = (int)await _repository.ScalarDynamicResult(filterQuery, transaction: transaction);
                if (data != null && data.Any())
                {
                    response.IsSuccess = true;
                    response.Result = data.ToList();
                    response.TotalRecords = totalRecord;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Result = default;
                    response.TotalRecords = totalRecord;
                    response.ErrorMessage.Add("No records found");
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                response.IsSuccess = false;
                response.Result = default;
                response.TotalRecords = 0;
                response.ErrorMessage.Add(exception.Message);
            }
            return response;
        }

        public async Task<ApiGenericResponseModel<PaymentMethods>> GetPaymentMethodById(long paymentMethodId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<PaymentMethods> response = new ApiGenericResponseModel<PaymentMethods>();
            response.Result = new PaymentMethods();

            try
            {
                if (paymentMethodId > 0)
                {
                    var data = await _repository.QuerySP(SqlConstants.SP_PaymentMethodsByPaymentMethodId, new { PaymentMethodId = paymentMethodId }, transaction: transaction);
                    if (data != null && data.Any())
                    {
                        response.IsSuccess = true;
                        response.Result = data.FirstOrDefault();
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.ErrorMessage.Add("No records found");
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                response.IsSuccess = false;
                response.ErrorMessage.Add(exception.Message);
                response.Result = default;
            }
            return response;
        }

        public async Task<ApiGenericResponseModel<long>> SavePaymentMethod(PaymentMethods data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();
            IDbTransaction localTransaction = null;

            if (transaction != null)
                localTransaction = transaction;
            else
                localTransaction = _localTransaction.BeginTransaction();

            try
            {
                response.IsSuccess = true;
                response.Result = await _repository.Insert(data, localTransaction);

                if (transaction == null && localTransaction != null)
                    localTransaction.Commit();
            }
            catch (Exception exception)
            {
                if (transaction == null && localTransaction != null)
                    localTransaction.Rollback();

                _logger.Error(exception, exception.Message);

                response.IsSuccess = false;
                response.ErrorMessage.Add(exception.Message);
                response.Result = default;
            }
            return response;
        }

        public async Task<ApiGenericResponseModel<bool>> UpdatePaymentMethod(PaymentMethods data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localTransaction = null;

            if (transaction != null)
                localTransaction = transaction;
            else
                localTransaction = _localTransaction.BeginTransaction();

            try
            {
                response.IsSuccess = true;
                response.Result = await _repository.Update(data, transaction: localTransaction);

                if (transaction == null && localTransaction != null)
                    localTransaction.Commit();
            }
            catch (Exception exception)
            {
                if (transaction == null && localTransaction != null)
                    localTransaction.Rollback();

                _logger.Error(exception, exception.Message);

                response.IsSuccess = false;
                response.ErrorMessage.Add(exception.Message);
                response.Result = default;
            }
            return response;
        }
    }

}
