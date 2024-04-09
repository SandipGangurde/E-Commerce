﻿using AutoMapper;
using DAL.DataContract.Contract;
using DataCarrier.ApplicationModels.Common;
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
    public class CategoriesMasterRepository : ICategoriesMasterRepository
    {

        private readonly ILogger _logger;
        private readonly IGenericRepository<Categories> _repository;
        private readonly ITransactions _localTransaction;
        private readonly IMapper _map;

        public CategoriesMasterRepository(ILogger logger, IMapper map, IGenericRepository<Categories> repository, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repository = repository;
            _localTransaction = transactions;
        }

        //public async Task<ApiGenericResponseModel<Categories>> getCategoryByCode(string CountryCode, IDbTransaction transaction = null)
        //{
        //    ApiGenericResponseModel<Categories> response = new ApiGenericResponseModel<Categories>();
        //    response.Result = new Categories();

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(CategoryCode))
        //        {
        //            var data = await _repository.QuerySP(SqlConstants.SP_CategoryByCategoryCode, new { CountryCode = CountryCode }, transaction: transaction);
        //            if (data != null && data.Any())
        //            {
        //                response.IsSuccess = true;
        //                response.Result = data.FirstOrDefault();
        //            }
        //            else
        //            {
        //                response.IsSuccess = true;
        //                response.ErrorMessage.Add("No records found");
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.Error(exception, exception.Message);
        //        response.IsSuccess = false;
        //        response.ErrorMessage.Add(exception.Message);
        //        response.Result = default;
        //    }
        //    return response;
        //}

        public async Task<ApiGenericResponseModel<Categories>> getCategoryById(long CategoryId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<Categories> response = new ApiGenericResponseModel<Categories>();
            response.Result = new Categories();

            try
            {
                if (CategoryId > 0)
                {
                    var data = await _repository.QuerySP(SqlConstants.SP_CategoriesByCategoryId, new { CategoryId = CategoryId }, transaction: transaction);
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

        public async Task<ApiGetResponseModel<List<Categories>>> getCategoryList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<Categories>> response = new ApiGetResponseModel<List<Categories>>();
            try
            {
                RequestModel searchRequest = _map.Map<RequestModel>(request);
                var data = await _repository.Query(new Categories(), searchRequest, transaction: transaction);
                string FilterQuery = SqlQueryHelper.GenerateQueryForTotalRecordsFound(new Categories(), searchRequest);
                int TotalRecord = (int)await _repository.ScalarDynamicResult(FilterQuery, transaction: transaction);
                if (data != null && data.Any())
                {
                    response.IsSuccess = true;
                    response.Result = data.ToList();
                    response.TotalRecords = TotalRecord;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Result = default;
                    response.TotalRecords = TotalRecord;
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

        public async Task<ApiGenericResponseModel<long>> saveCategory(Categories data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                response.IsSuccess = true;
                response.Result = await _repository.Insert(data, localtran);

                if (transaction == null && localtran != null)
                    localtran.Commit();
            }
            catch (Exception exception)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                _logger.Error(exception, exception.Message);

                response.IsSuccess = false;
                response.ErrorMessage.Add(exception.Message);
                response.Result = default;
            }
            return response;
        }

        public async Task<ApiGenericResponseModel<bool>> updateCategory(Categories data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                response.IsSuccess = true;
                response.Result = await _repository.Update(data, transaction: localtran);

                if (transaction == null && localtran != null)
                    localtran.Commit();

            }
            catch (Exception exception)
            {
                if (transaction == null && localtran != null)
                    localtran.Rollback();

                _logger.Error(exception, exception.Message);

                response.IsSuccess = false;
                response.ErrorMessage.Add(exception.Message);
                response.Result = default;
            }
            return response;
        }
    }
}
