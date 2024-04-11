using AutoMapper;
using DAL.DataContract.Contract;
using DAL;
using DataCarrier.ApplicationModels.Common;
using DataModel.Entities;
using RepositoryOperations.ApplicationModels.Common;
using RepositoryOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Business.Service
{
    public class RolesMasterRepository : IRolesMasterRepository
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<Role> _repository;
        private readonly ITransactions _localTransaction;
        private readonly IMapper _map;

        public RolesMasterRepository(ILogger logger, IMapper map, IGenericRepository<Role> repository, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repository = repository;
            _localTransaction = transactions;
        }

        public async Task<ApiGenericResponseModel<Role>> GetRoleById(long roleId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<Role> response = new ApiGenericResponseModel<Role>();
            response.Result = new Role();

            try
            {
                if (roleId > 0)
                {
                    var data = await _repository.QuerySP(SqlConstants.SP_RoleByRoleId, new { RoleId = roleId }, transaction: transaction);
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

        public async Task<ApiGetResponseModel<List<Role>>> GetRoleList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<Role>> response = new ApiGetResponseModel<List<Role>>();
            try
            {
                RequestModel searchRequest = _map.Map<RequestModel>(request);
                var data = await _repository.Query(new Role(), searchRequest);
                string FilterQuery = SqlQueryHelper.GenerateQueryForTotalRecordsFound(new Role(), searchRequest);
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

        public async Task<ApiGenericResponseModel<long>> SaveRole(Role data, IDbTransaction transaction = null)
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

        public async Task<ApiGenericResponseModel<bool>> UpdateRole(Role data, IDbTransaction transaction = null)
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

        public async Task<ApiGenericResponseModel<bool>> DeleteRole(long roleId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            try
            {
                if (roleId > 0)
                {
                    int delRoleStatus = (int)await _repository.ExecuteScalarSP(SqlConstants.SP_DeleteRole, new { RoleId = roleId }, transaction: transaction);
                    if (delRoleStatus > 0)
                    {
                        /*Implement delete logic for the role.*/
                    }
                    response.Result = true;
                }
                else
                {
                    response.Result = false;
                    response.IsSuccess = true;
                    response.ErrorMessage.Add("No records found");
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
    }

}
