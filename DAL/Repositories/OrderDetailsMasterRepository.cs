using AutoMapper;
using DAL.DataContract.Contract;
using DAL.Helpers;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ApplicationModels.OrderDetails.Request;
using DataCarrier.ApplicationModels.OrderDetails.Response;
using DataCarrier.ViewModels;
using DataModel.Entities;
using RepositoryOperations.ApplicationModels.Common;
using RepositoryOperations.Interfaces;
using RepositoryOperations.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL.Repositories
{
    public class OrderDetailsMasterRepository : IOrderDetailsMasterRepository
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<OrderDetails> _repository;
        private readonly IGenericRepository<Users> _userRepo;
        private readonly IGenericRepository<Addresses> _addressRepo;
        private readonly IGenericRepository<Orders> _orderRepo;
        private readonly IGenericRepository<OrderDetails> _orderDetailsRepo;
        private readonly IGenericRepository<Products> _productsRepo;
        private readonly IGenericRepository<Shipping> _shippingRepo;
        private readonly IGenericRepository<VuOrderShippingDetails> _orderShippingDetail;
        private readonly IGenericRepository<CartItems> _cartRepo;
        private readonly ITransactions _localTransaction;
        private readonly IMapper _map;

        public OrderDetailsMasterRepository(
            ILogger logger, 
            IMapper map, 
            IGenericRepository<OrderDetails> repository,
            IGenericRepository<Users> userRepo,
            IGenericRepository<Addresses> addressRepo,
            IGenericRepository<Orders> orderRepo,
            IGenericRepository<OrderDetails> orderDetailsRepo,
            IGenericRepository<Products> productsRepo,
            IGenericRepository<Shipping> shippingRepo,
            IGenericRepository<VuOrderShippingDetails> orderShippingDetail,
            IGenericRepository<CartItems> cartRepo,
            ITransactions transactions
        ){
            _logger = logger;
            _map = map;
            _repository = repository;
            _userRepo = userRepo;
            _addressRepo = addressRepo;
            _orderRepo = orderRepo;
            _orderDetailsRepo = orderDetailsRepo;
            _productsRepo = productsRepo;
            _shippingRepo = shippingRepo;
            _orderShippingDetail = orderShippingDetail;
            _cartRepo = cartRepo;
            _localTransaction = transactions;
        }

        public async Task<ApiGetResponseModel<List<OrderDetails>>> GetOrderDetailList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<OrderDetails>> response = new ApiGetResponseModel<List<OrderDetails>>();
            try
            {
                RequestModel searchRequest = _map.Map<RequestModel>(request);
                var data = await _repository.Query(new OrderDetails(), searchRequest, transaction: transaction);
                string filterQuery = SqlQueryHelper.GenerateQueryForTotalRecordsFound(new OrderDetails(), searchRequest);
                int totalRecord = (int)await _repository.ScalarDynamicResult(filterQuery, transaction: transaction);
                if (data != null && data.Any())
                {
                    response.IsSuccess = true;
                    response.Result = data.ToList();
                    response.TotalRecords = totalRecord;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Result = default;
                    response.TotalRecords = totalRecord;
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

        public async Task<ApiGenericResponseModel<OrderDetails>> GetOrderDetailById(long orderDetailId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<OrderDetails> response = new ApiGenericResponseModel<OrderDetails>();
            response.Result = new OrderDetails();

            try
            {
                if (orderDetailId > 0)
                {
                    var data = await _repository.QuerySP(SqlConstants.SP_OrderDetailsById, new { OrderDetailsId = orderDetailId }, transaction: transaction);
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

        public async Task<ApiGenericResponseModel<long>> SaveOrderDetail(OrderDetails data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();
            IDbTransaction localTransaction = null;

            if (transaction != null)
                localTransaction = transaction;
            else
                localTransaction = _localTransaction.BeginTransaction();

            try
            {
                response.IsSuccess = true;
                response.Result = await _repository.Insert(data, localTransaction);

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

        public async Task<ApiGenericResponseModel<bool>> UpdateOrderDetail(OrderDetails data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localTransaction = null;

            if (transaction != null)
                localTransaction = transaction;
            else
                localTransaction = _localTransaction.BeginTransaction();

            try
            {
                response.IsSuccess = true;
                response.Result = await _repository.Update(data, transaction: localTransaction);

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

        public async Task<ApiGenericResponseModel<PlaceOrderResponse>> SavePlaceOrder(PlaceOrder data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<PlaceOrderResponse> response = new ApiGenericResponseModel<PlaceOrderResponse>();
            IDbTransaction localTransaction = null;

            if (transaction != null)
                localTransaction = transaction;
            else
                localTransaction = _localTransaction.BeginTransaction();

            try
            {
                // save order
                var ordermodel = new Orders()
                {
                    OrderId = 0,
                    UserId = data.UserId,
                    OrderDate = DateTime.Now,
                    IsCompleted = false,
                    TotalAmount = data.TotalAmount,
                };
                var orderId = await _orderRepo.Insert(ordermodel, localTransaction);
                // save orderDetails
                foreach (var productItem in data.ProductItems)
                {
                    OrderDetails orderDetailsModel = new OrderDetails()
                    {
                        OrderDetailId = 0,
                        OrderId = orderId,
                        ProductId = productItem.ProductId,
                        Quantity = productItem.Quantity,
                        UnitPrice = productItem.UnitPrice,
                        DiscountApplied = productItem.DiscountApplied,
                        TotalPrice = productItem.TotalPrice
                    };
                    await _orderDetailsRepo.Insert(orderDetailsModel, localTransaction);

                    var getProduct = await _productsRepo.QuerySP(SqlConstants.SP_ProductsByProductId, new { ProductId = productItem.ProductId }, transaction: localTransaction);
                    if (getProduct != null && getProduct.Any())
                    {
                        var udpateProduct = getProduct.FirstOrDefault();
                        if (udpateProduct != null)
                        {
                            // update product stockQuantity
                            udpateProduct.StockQuantity = udpateProduct.StockQuantity - productItem.Quantity;
                            await _productsRepo.Update(udpateProduct, localTransaction);
                        }

                    }

                }
                // Generate a random tracking number
                Random random = new Random();
                string trackingNumber = "TC-" + data.UserId + random.Next(100000, 999999);

                // save shipping details
                Shipping shippingmodel = new Shipping()
                {
                    ShippingId = 0,
                    OrderId = orderId,
                    ShippedDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    ShippingMethod = "Standard Shipping",
                    ShippingAddress = $"{data.Address.PostalCode} {data.Address.AddressLine1}, {data.Address?.City}, {data.Address?.State}, {data.Address?.Country}",
                    TrackingNumber = trackingNumber
                };

                var shippingId = await _shippingRepo.Insert(shippingmodel, localTransaction);

                PlaceOrderResponse placeOrderResponse = new PlaceOrderResponse()
                {
                    ShippingId = shippingId,
                    OrderId = orderId,
                    TrackingNumber = trackingNumber,
                };

                foreach (var cartItemId in data.CartItemIds)
                {
                    await _cartRepo.ExecuteScalarSP(SqlConstants.SP_DeleteCartItemByCartItemId, new { CartItemId = cartItemId }, transaction: localTransaction);
                }

                response.Result = placeOrderResponse;
                response.IsSuccess = true;

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

        public async Task<ApiGetResponseModel<List<VuOrderShippingDetails>>> GetOrderShippingDetailsList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<VuOrderShippingDetails>> response = new ApiGetResponseModel<List<VuOrderShippingDetails>>();
            try
            {
                RequestModel searchRequest = _map.Map<RequestModel>(request);
                var data = await _orderShippingDetail.Query(new VuOrderShippingDetails(), searchRequest, transaction: transaction);
                string filterQuery = SqlQueryHelper.GenerateQueryForTotalRecordsFound(new VuOrderShippingDetails(), searchRequest);
                int totalRecord = (int)await _orderShippingDetail.ScalarDynamicResult(filterQuery, transaction: transaction);
                if (data != null && data.Any())
                {
                    response.IsSuccess = true;
                    response.Result = data.ToList();
                    response.TotalRecords = totalRecord;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Result = default;
                    response.TotalRecords = totalRecord;
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

        public async Task<ApiGenericResponseModel<bool>> CompleteOrderbyOrderId(long orderId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            response.Result = false;
            IDbTransaction localTransaction = null;

            if (transaction != null)
                localTransaction = transaction;
            else
                localTransaction = _localTransaction.BeginTransaction();
            try
            {
                if (orderId > 0)
                {
                    var orderData = await _orderRepo.Query("SELECT * FROM Orders WHERE OrderId = " + orderId, transaction: localTransaction);
                    if (orderData != null && orderData.Any())
                    {
                        response.IsSuccess = true;
                        var order = orderData.FirstOrDefault();
                        order.IsCompleted = true;
                        response.Result = await _orderRepo.Update(order, transaction: localTransaction);

                        var shippingData = await _shippingRepo.Query("SELECT * FROM Shipping WHERE OrderId = " + order.OrderId, transaction: localTransaction);

                        // Check if there's an image file path
                        if (shippingData.Any())
                        {
                            var shipping = shippingData.FirstOrDefault();
                            shipping.DeliveryDate = DateTime.Now;
                            await _shippingRepo.Update(shipping, transaction: localTransaction);
                        }
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Result = false;
                        response.ErrorMessage.Add("No records found");
                    }
                }
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
