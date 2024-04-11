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
    public class CategoriesMaster : ICategoriesMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly ICategoriesMasterRepository _repo;
        private readonly ITransactions _localTransaction;
        public CategoriesMaster(ILogger logger, IMapper map, ICategoriesMasterRepository repo, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _localTransaction = transactions;
        }


        public async Task<ApiGenericResponseModel<CategoriesVM>> GetCategoryById(long categoryId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<CategoriesVM> response = new ApiGenericResponseModel<CategoriesVM>();
            response.Result = new CategoriesVM();
            try
            {
                var data = await _repo.GetCategoryById(categoryId, transaction: transaction);
                response.IsSuccess = true;
                response.Result = _map.Map<CategoriesVM>(data.Result);
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

        public async Task<ApiGetResponseModel<List<CategoriesVM>>> GetCategoryList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<CategoriesVM>> response = new ApiGetResponseModel<List<CategoriesVM>>();
            try
            {
                var data = await _repo.GetCategoryList(request, transaction: transaction);
                if (data.Result != null)
                {
                    if (data.Result.Count > 0)
                    {
                        List<CategoriesVM> mapresponse = _map.Map<List<CategoriesVM>>(data.Result);
                        response.Result = mapresponse;
                        response.TotalRecords = data.TotalRecords;
                    }
                    else
                    {
                        response.Result = default;
                        response.TotalRecords = data.TotalRecords;
                        response.ErrorMessage.Add("No Records Found");
                    }
                }
                else
                {
                    response.Result = default;
                    response.TotalRecords = 0;
                    response.ErrorMessage.Add("No Records Found");
                }
                response.IsSuccess = true;
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
        public async Task<ApiGenericResponseModel<long>> SaveCategory(CategoriesVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                Categories mapmodel = _map.Map<Categories>(data);
                response = await _repo.SaveCategory(mapmodel, transaction: localtran);

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
        public async Task<ApiGenericResponseModel<bool>> UpdateCategory(CategoriesVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                Categories mapmodel = _map.Map<Categories>(data);
                response = await _repo.UpdateCategory(mapmodel, transaction: localtran);

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
