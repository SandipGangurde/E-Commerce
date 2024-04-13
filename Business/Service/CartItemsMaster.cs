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
    public class CartItemsMaster : ICartItemsMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly ICartItemsMasterRepository _repo;
        private readonly IUsersMasterRepository _repoUser;
        private readonly ITransactions _localTransaction;

        public CartItemsMaster(
            ILogger logger, 
            IMapper map, 
            ICartItemsMasterRepository repo, 
            IUsersMasterRepository repoUser, 
            ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _repoUser = repoUser;
            _localTransaction = transactions;
        }

        public async Task<ApiGetResponseModel<List<CartItemsVM>>> GetCartItemList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<CartItemsVM>> response = new ApiGetResponseModel<List<CartItemsVM>>();

            try
            {
                var data = await _repo.GetCartItemList(request, transaction);

                if (data.IsSuccess && data.Result != null && data.Result.Count > 0)
                {
                    List<CartItemsVM> mapresponse = _map.Map<List<CartItemsVM>>(data.Result);
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

        public async Task<ApiGenericResponseModel<CartItemsVM>> GetCartItemById(long cartId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<CartItemsVM> response = new ApiGenericResponseModel<CartItemsVM>();

            try
            {
                var data = await _repo.GetCartItemById(cartId, transaction);

                if (data.IsSuccess && data.Result != null)
                {
                    CartItemsVM mapresponse = _map.Map<CartItemsVM>(data.Result);
                    response.Result = mapresponse;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Result = null;
                    response.ErrorMessage.Add("No records found");
                }
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

        public async Task<ApiGenericResponseModel<long>> SaveCartItem(CartItemsVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();

            try
            {
                ApiGetRequestModel request = new ApiGetRequestModel();
                var filterCols = new List<MultiSearchInColumn>();

                var userId = new MultiSearchInColumn()
                {
                    ColumnName = "userId",
                    ColumnValue = data.UserId.ToString(),
                };
                filterCols.Add(userId);

                var productId = new MultiSearchInColumn()
                {
                    ColumnName = "productId",
                    ColumnValue = data.ProductId.ToString(),
                };
                filterCols.Add(productId);
                request.ListOfMultiSearchColumn = filterCols;

                var cartResult = await _repo.GetCartItemList(request);
                if(cartResult.Result == null)
                {
                    CartItems mapmodel = _map.Map<CartItems>(data);
                    var saveResponse = await _repo.SaveCartItem(mapmodel, transaction);

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
                else 
                {
                    response.IsSuccess = false;
                    response.Result = default;
                    response.ErrorMessage.Add("Product is already in the cart");
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

        public async Task<ApiGenericResponseModel<bool>> UpdateCartItem(CartItemsVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();

            try
            {
                CartItems mapmodel = _map.Map<CartItems>(data);
                var saveResponse = await _repo.UpdateCartItem(mapmodel, transaction);

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

        public async Task<ApiGetResponseModel<List<CartItemDetailsVM>>> GetCartItemDetail(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<CartItemDetailsVM>> response = new ApiGetResponseModel<List<CartItemDetailsVM>>();

            try
            {
                var data = await _repo.GetCartItemDetail(request, transaction);

                if (data.IsSuccess && data.Result != null && data.Result.Count > 0)
                {
                    List<CartItemDetailsVM> mapresponse = _map.Map<List<CartItemDetailsVM>>(data.Result);
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
        public async Task<ApiGenericResponseModel<bool>> DeleteCartItemByCartItemId(long cartItemId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();
            try
            {
                response = await _repo.DeleteCartItemByCartItemId(cartItemId, transaction: localtran);

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
