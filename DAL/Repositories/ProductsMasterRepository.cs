

using AutoMapper;
using DAL.DataContract.Contract;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using RepositoryOperations.ApplicationModels.Common;
using RepositoryOperations.Interfaces;
using Serilog;
using System.Data;

namespace DAL.Repositories
{
    public class ProductsMasterRepository : IProductsMasterRepository
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<Products> _repository;
        private readonly ITransactions _localTransaction;
        private readonly IMapper _map;

        public ProductsMasterRepository(ILogger logger, IMapper map, IGenericRepository<Products> repository, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repository = repository;
            _localTransaction = transactions;
        }

        public async Task<ApiGetResponseModel<List<Products>>> GetProductList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<Products>> response = new ApiGetResponseModel<List<Products>>();
            try
            {
        
                RequestModel searchRequest = _map.Map<RequestModel>(request);
                var data = await _repository.Query(new Products(), searchRequest, transaction: transaction);
                string FilterQuery = SqlQueryHelper.GenerateQueryForTotalRecordsFound(new Products(), searchRequest);
                int totalRecord = (int)await _repository.ScalarDynamicResult(FilterQuery, transaction: transaction);
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


        public async Task<ApiGenericResponseModel<Products>> GetProductById(long productId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<Products> response = new ApiGenericResponseModel<Products>();
            response.Result = new Products();

            try
            {
                if (productId > 0)
                {
                    var data = await _repository.QuerySP(SqlConstants.SP_ProductsByProductId, new { ProductId = productId }, transaction: transaction);
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

        public async Task<ApiGenericResponseModel<long>> SaveProduct(Products data, IDbTransaction transaction = null)
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

        public async Task<ApiGenericResponseModel<bool>> UpdateProduct(Products data, IDbTransaction transaction = null)
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
