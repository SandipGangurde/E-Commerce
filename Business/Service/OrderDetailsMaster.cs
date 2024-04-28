using AutoMapper;
using Business.Contract;
using DAL.DataContract.Contract;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ApplicationModels.OrderDetails.Request;
using DataCarrier.ApplicationModels.OrderDetails.Response;
using DataCarrier.ViewModels;
using DataModel.Entities;
using Microsoft.AspNetCore.Server.IISIntegration;
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
    public class OrderDetailsMaster : IOrderDetailsMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly IOrderDetailsMasterRepository _repo;
        private readonly IOrdersMasterRepository _ordersMasterRepository;
        private readonly IAddressesMasterRepository _userAddressRepo;
        private readonly IProductsMasterRepository _productRepo;
        private readonly IShippingMasterRepository _shippingRepo;
        private readonly ITransactions _localTransaction;

        public OrderDetailsMaster(ILogger logger, IMapper map, IAddressesMasterRepository userAddressRepo, IOrderDetailsMasterRepository repo, IOrdersMasterRepository ordersMasterRepository, IProductsMasterRepository productRepo, IShippingMasterRepository shippingRepo, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _ordersMasterRepository = ordersMasterRepository;
            _userAddressRepo = userAddressRepo;
            _productRepo = productRepo;
            _shippingRepo = shippingRepo;
            _localTransaction = transactions;
        }

        public async Task<ApiGetResponseModel<List<OrderDetailsVM>>> GetOrderDetailList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<OrderDetailsVM>> response = new ApiGetResponseModel<List<OrderDetailsVM>>();

            try
            {
                var data = await _repo.GetOrderDetailList(request, transaction: transaction);
                if (data.IsSuccess && data.Result != null && data.Result.Count > 0)
                {
                    List<OrderDetailsVM> mapresponse = _map.Map<List<OrderDetailsVM>>(data.Result);
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

        public async Task<ApiGenericResponseModel<OrderDetailsVM>> GetOrderDetailById(long orderDetailId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<OrderDetailsVM> response = new ApiGenericResponseModel<OrderDetailsVM>();
            response.Result = new OrderDetailsVM();

            try
            {
                var data = await _repo.GetOrderDetailById(orderDetailId, transaction: transaction);
                response.IsSuccess = true;
                response.Result = _map.Map<OrderDetailsVM>(data.Result);
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

        public async Task<ApiGenericResponseModel<long>> SaveOrderDetail(OrderDetailsVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();

            try
            {
                OrderDetails mapmodel = _map.Map<OrderDetails>(data);
                var saveResponse = await _repo.SaveOrderDetail(mapmodel, transaction);

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

        public async Task<ApiGenericResponseModel<bool>> UpdateOrderDetail(OrderDetailsVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                OrderDetails mapmodel = _map.Map<OrderDetails>(data);
                response = await _repo.UpdateOrderDetail(mapmodel, transaction: localtran);

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

        public async Task<ApiGenericResponseModel<PlaceOrderResponse>> SavePlaceOrder(PlaceOrder data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<PlaceOrderResponse> response = new ApiGenericResponseModel<PlaceOrderResponse>();

            try
            {
                response = await _repo.SavePlaceOrder(data, transaction: transaction);
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

        public async Task<ApiGetResponseModel<List<OrderShippingDetailsVM>>> GetOrderShippingDetailsList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<OrderShippingDetailsVM>> response = new ApiGetResponseModel<List<OrderShippingDetailsVM>>();

            try
            {
                var data = await _repo.GetOrderShippingDetailsList(request, transaction: transaction);
                if (data.IsSuccess && data.Result != null && data.Result.Count > 0)
                {
                    List<OrderShippingDetailsVM> mapresponse = _map.Map<List<OrderShippingDetailsVM>>(data.Result);
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

        public async Task<ApiGenericResponseModel<bool>> CompleteOrderbyOrderId(long orderId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            response.Result = false;

            try
            {
                var data = await _repo.CompleteOrderbyOrderId(orderId, transaction: transaction);
                response.IsSuccess = true;
                response.Result = data.Result;
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
