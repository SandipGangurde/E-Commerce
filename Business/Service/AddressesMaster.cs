using AutoMapper;
using Business.Contract;
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

namespace Business.Service
{
    public class AddressesMaster : IAddressesMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly IAddressesMasterRepository _repo;
        private readonly ITransactions _localTransaction;

        public AddressesMaster(ILogger logger, IMapper map, IAddressesMasterRepository repo, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _localTransaction = transactions;
        }

        public async Task<ApiGetResponseModel<List<AddressesVM>>> GetAddressList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<AddressesVM>> response = new ApiGetResponseModel<List<AddressesVM>>();

            try
            {
                var data = await _repo.GetAddressList(request, transaction: transaction);
                if (data.IsSuccess && data.Result != null && data.Result.Count>0)
                {
                    List<AddressesVM> mapresponse = _map.Map<List<AddressesVM>>(data.Result);
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

        public async Task<ApiGenericResponseModel<AddressesVM>> GetAddressById(long addressId, IDbTransaction transaction = null)
        {

            ApiGenericResponseModel<AddressesVM> response = new ApiGenericResponseModel<AddressesVM>();
            response.Result = new AddressesVM();
            try
            {
                var data = await _repo.GetAddressById(addressId, transaction: transaction);
                response.IsSuccess = true;
                response.Result = _map.Map<AddressesVM>(data.Result);
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

        public async Task<ApiGenericResponseModel<long>> SaveAddress(AddressesVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();

            try
            {
                Addresses mapmodel = _map.Map<Addresses>(data);
                var saveResponse = await _repo.SaveAddress(mapmodel, transaction);

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

        public async Task<ApiGenericResponseModel<bool>> UpdateAddress(AddressesVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                Addresses mapmodel = _map.Map<Addresses>(data);
                response = await _repo.UpdateAddress(mapmodel, transaction: localtran);

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
