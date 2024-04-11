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
    public class UsersMaster : IUsersMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly IUsersMasterRepository _repo;
        private readonly ITransactions _localTransaction;
        public UsersMaster(ILogger logger, IMapper map, IUsersMasterRepository repo, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _localTransaction = transactions;
        }

        public async Task<ApiGenericResponseModel<UsersVM>> GetUserById(long UserId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<UsersVM> response = new ApiGenericResponseModel<UsersVM>();
            response.Result = new UsersVM();
            try
            {
                var data = await _repo.GetUserById(UserId, transaction: transaction);
                response.IsSuccess = true;
                response.Result = _map.Map<UsersVM>(data.Result);
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

        public async Task<ApiGetResponseModel<List<UsersVM>>> GetUserList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<UsersVM>> response = new ApiGetResponseModel<List<UsersVM>>();
            try
            {
                var data = await _repo.GetUserList(request, transaction: transaction);
                if (data.Result != null)
                {
                    if (data.Result.Count > 0)
                    {
                        List<UsersVM> mapresponse = _map.Map<List<UsersVM>>(data.Result);
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
        public async Task<ApiGenericResponseModel<long>> SaveUser(UsersVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                Users mapmodel = _map.Map<Users>(data);
                response = await _repo.SaveUser(mapmodel, transaction: localtran);

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
        public async Task<ApiGenericResponseModel<bool>> UpdateUser(UsersVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                Users mapmodel = _map.Map<Users>(data);
                response = await _repo.UpdateUser(mapmodel, transaction: localtran);

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
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                response = await _repo.DeleteUser(userId, transaction: localtran);

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
