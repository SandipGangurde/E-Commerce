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
    public class CustomersMaster : ICustomersMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly ICustomersMasterRepository _repo;
        private readonly ITransactions _localTransaction;

        public CustomersMaster(ILogger logger, IMapper map, ICustomersMasterRepository repo, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _localTransaction = transactions;
        }

        public async Task<ApiGetResponseModel<List<CustomersVM>>> GetCustomerList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<CustomersVM>> response = new ApiGetResponseModel<List<CustomersVM>>();

            try
            {
                var data = await _repo.GetCustomerList(request, transaction);

                if (data.IsSuccess && data.Result != null)
                {
                    List<CustomersVM> mapresponse = _map.Map<List<CustomersVM>>(data.Result);
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

        public async Task<ApiGenericResponseModel<CustomersVM>> GetCustomerById(long customerId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<CustomersVM> response = new ApiGenericResponseModel<CustomersVM>();

            try
            {
                var data = await _repo.GetCustomerById(customerId, transaction);

                if (data.IsSuccess && data.Result != null)
                {
                    CustomersVM mapresponse = _map.Map<CustomersVM>(data.Result);
                    response.Result = mapresponse;
                }
                else
                {
                    response.Result = null;
                    response.ErrorMessage.Add("No records found");
                }
                response.IsSuccess = true;
            }
            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                response.IsSuccess = false;
                response.Result = null;
                response.ErrorMessage.Add(exception.Message);
            }

            return response;
        }

        public async Task<ApiGenericResponseModel<long>> SaveCustomer(CustomersVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();

            try
            {
                Customers mapmodel = _map.Map<Customers>(data);
                var saveResponse = await _repo.SaveCustomer(mapmodel, transaction);

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

        public async Task<ApiGenericResponseModel<bool>> UpdateCustomer(CustomersVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();

            try
            {
                Customers mapmodel = _map.Map<Customers>(data);
                var saveResponse = await _repo.UpdateCustomer(mapmodel, transaction);

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
    }
}
