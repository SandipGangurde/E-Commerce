using AutoMapper;
using DAL.DataContract.Contract;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using Microsoft.AspNetCore.Http.Extensions;
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
    public class UsersMasterRepository : IUsersMasterRepository
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<Users> _repo;
        private readonly IGenericRepository<VuUserDetails> _repoUd;
        private readonly IGenericRepository<UserRole> _repoUr;
        private readonly ITransactions _localTransaction;
        private readonly IMapper _map;

        public UsersMasterRepository(ILogger logger, IMapper map, IGenericRepository<Users> repo, IGenericRepository<VuUserDetails> repoUd, IGenericRepository<UserRole> repoUr, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _repoUd = repoUd;
            _repoUr = repoUr;
            _localTransaction = transactions;
        }
       

        public async Task<ApiGenericResponseModel<Users>> GetUserById(long UserId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<Users> response = new ApiGenericResponseModel<Users>();
            response.Result = new Users();

            try
            {
                if (UserId > 0)
                {
                    var data = await _repo.QuerySP(SqlConstants.SP_UserByUserId, new { UserId = UserId }, transaction: transaction);
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

        public async Task<ApiGetResponseModel<List<Users>>> GetUserList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<Users>> response = new ApiGetResponseModel<List<Users>>();
            try
            {
                RequestModel searchRequest = _map.Map<RequestModel>(request);
                var data = await _repo.Query(new Users(), searchRequest);
                string FilterQuery = SqlQueryHelper.GenerateQueryForTotalRecordsFound(new Users(), searchRequest);
                int TotalRecord = (int)await _repo.ScalarDynamicResult(FilterQuery, transaction: transaction);
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

        public async Task<ApiGenericResponseModel<long>> SaveUser(Users data, IDbTransaction transaction = null)
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
                var userId = await _repo.Insert(data, localtran);
                var userRole = new UserRole
                {
                    UserRoleId = 0,
                    UserId = userId,
                    RoleId = 2,
                };
                var UserRoleID = await _repoUr.Insert(userRole, localtran);

                response.Result = userId;

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

        public async Task<ApiGenericResponseModel<bool>> UpdateUser(Users data, IDbTransaction transaction = null)
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
                response.Result = await _repo.Update(data, transaction: localtran);

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

        public async Task<ApiGenericResponseModel<bool>> DeleteUser(long userId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            try
            {
                if (userId > 0)
                {


                    int delDocStatus = (int)await _repo.ExecuteScalarSP(SqlConstants.SP_DeleteDocument, new { UserId = userId }, transaction: transaction);
                    if (delDocStatus > 0)
                    {
                        /*Implement delete logic for the file from the local folder.*/
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
        public async Task<ApiGenericResponseModel<VuUserDetails>> GetUserDetailByEmail(string email, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<VuUserDetails> response = new ApiGenericResponseModel<VuUserDetails>();
            try
            {
                if (email != null)
                {
                    var data = await _repoUd.QuerySP(SqlConstants.SP_UserDetailByEmail, new { Email = email}, transaction: transaction);
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
                else
                {
                    
                    response.Result = null;
                    response.IsSuccess = false;
                    response.ErrorMessage.Add("Email is not valid");
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
