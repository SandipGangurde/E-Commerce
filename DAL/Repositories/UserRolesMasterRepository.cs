using AutoMapper;
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
    public class UserRolesMasterRepository : IUserRolesMasterRepository
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<UserRole> _repository;
        private readonly ITransactions _localTransaction;
        private readonly IMapper _map;

        public UserRolesMasterRepository(ILogger logger, IMapper map, IGenericRepository<UserRole> repository, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repository = repository;
            _localTransaction = transactions;
        }

        public async Task<ApiGenericResponseModel<UserRole>> GetUserRoleById(long userRoleId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<UserRole> response = new ApiGenericResponseModel<UserRole>();
            response.Result = new UserRole();

            try
            {
                if (userRoleId > 0)
                {
                    var data = await _repository.QuerySP(SqlConstants.SP_UserRoleByUserRoleId, new { UserRoleId = userRoleId }, transaction: transaction);
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

        public async Task<ApiGetResponseModel<List<UserRole>>> GetUserRoleList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<UserRole>> response = new ApiGetResponseModel<List<UserRole>>();
            try
            {
                RequestModel searchRequest = _map.Map<RequestModel>(request);
                var data = await _repository.Query(new UserRole(), searchRequest);
                string FilterQuery = SqlQueryHelper.GenerateQueryForTotalRecordsFound(new UserRole(), searchRequest);
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

        public async Task<ApiGenericResponseModel<long>> SaveUserRole(UserRole data, IDbTransaction transaction = null)
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

        public async Task<ApiGenericResponseModel<bool>> UpdateUserRole(UserRole data, IDbTransaction transaction = null)
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

        public async Task<ApiGenericResponseModel<bool>> DeleteUserRole(long userRoleId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            try
            {
                if (userRoleId > 0)
                {
                    int delUserRoleStatus = (int)await _repository.ExecuteScalarSP(SqlConstants.SP_DeleteUserRole, new { UserRoleId = userRoleId }, transaction: transaction);
                    if (delUserRoleStatus > 0)
                    {
                        /*Implement delete logic for the user role.*/
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
