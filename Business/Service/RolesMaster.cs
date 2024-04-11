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
    public class RolesMaster : IRolesMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly IRolesMasterRepository _repo;
        private readonly ITransactions _localTransaction;

        public RolesMaster(ILogger logger, IMapper map, IRolesMasterRepository repo, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _localTransaction = transactions;
        }

        public async Task<ApiGenericResponseModel<RoleVM>> GetRoleById(long roleId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<RoleVM> response = new ApiGenericResponseModel<RoleVM>();
            response.Result = new RoleVM();

            try
            {
                var data = await _repo.GetRoleById(roleId, transaction: transaction);
                response.IsSuccess = true;
                response.Result = _map.Map<RoleVM>(data.Result);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                response.IsSuccess = false;
                response.ErrorMessage.Add(exception.Message);
            }
            return response;
        }

        public async Task<ApiGetResponseModel<List<RoleVM>>> GetRoleList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<RoleVM>> response = new ApiGetResponseModel<List<RoleVM>>();
            try
            {
                var data = await _repo.GetRoleList(request, transaction: transaction);
                if (data.Result != null)
                {
                    if (data.Result.Count > 0)
                    {
                        List<RoleVM> mapResponse = _map.Map<List<RoleVM>>(data.Result);
                        response.Result = mapResponse;
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
                response.ErrorMessage.Add(exception.Message);
            }
            return response;
        }

        public async Task<ApiGenericResponseModel<long>> SaveRole(RoleVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();
            IDbTransaction localTransaction = null;

            if (transaction != null)
                localTransaction = transaction;
            else
                localTransaction = _localTransaction.BeginTransaction();

            try
            {
                Role mapModel = _map.Map<Role>(data);
                response = await _repo.SaveRole(mapModel, transaction: localTransaction);

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

        public async Task<ApiGenericResponseModel<bool>> UpdateRole(RoleVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localTransaction = null;

            if (transaction != null)
                localTransaction = transaction;
            else
                localTransaction = _localTransaction.BeginTransaction();

            try
            {
                Role mapModel = _map.Map<Role>(data);
                response = await _repo.UpdateRole(mapModel, transaction: localTransaction);

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

        public async Task<ApiGenericResponseModel<bool>> DeleteRole(long roleId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localTransaction = null;

            if (transaction != null)
                localTransaction = transaction;
            else
                localTransaction = _localTransaction.BeginTransaction();

            try
            {
                response = await _repo.DeleteRole(roleId, transaction: localTransaction);

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
