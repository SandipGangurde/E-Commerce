﻿using AutoMapper;
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
    public class WishlistMaster : IWishlistMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly IWishlistMasterRepository _repo;
        private readonly ITransactions _localTransaction;

        public WishlistMaster(ILogger logger, IMapper map, IWishlistMasterRepository repo, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _localTransaction = transactions;
        }

        public async Task<ApiGetResponseModel<List<WishlistVM>>> GetWishlist(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<WishlistVM>> response = new ApiGetResponseModel<List<WishlistVM>>();

            try
            {
                var data = await _repo.GetWishlist(request, transaction: transaction);
                if (data.IsSuccess && data.Result != null && data.Result.Count > 0)
                {
                    List<WishlistVM> mapresponse = _map.Map<List<WishlistVM>>(data.Result);
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

        public async Task<ApiGenericResponseModel<WishlistVM>> GetWishlistById(long wishlistId, IDbTransaction transaction = null)
        {

            ApiGenericResponseModel<WishlistVM> response = new ApiGenericResponseModel<WishlistVM>();
            response.Result = new WishlistVM();
            try
            {
                var data = await _repo.GetWishlistById(wishlistId, transaction: transaction);
                response.IsSuccess = true;
                response.Result = _map.Map<WishlistVM>(data.Result);
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

        public async Task<ApiGenericResponseModel<long>> SaveWishlist(WishlistVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();

            try
            {
                Wishlist mapmodel = _map.Map<Wishlist>(data);
                var saveResponse = await _repo.SaveWishlist(mapmodel, transaction);

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

        public async Task<ApiGenericResponseModel<bool>> UpdateWishlist(WishlistVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                Wishlist mapmodel = _map.Map<Wishlist>(data);
                response = await _repo.UpdateWishlist(mapmodel, transaction: localtran);

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
