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
    public class ReviewsMaster : IReviewsMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly IReviewsMasterRepository _repo;
        private readonly ITransactions _localTransaction;

        public ReviewsMaster(ILogger logger, IMapper map, IReviewsMasterRepository repo, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _localTransaction = transactions;
        }

        public async Task<ApiGetResponseModel<List<ReviewsVM>>> GetReviewList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<ReviewsVM>> response = new ApiGetResponseModel<List<ReviewsVM>>();

            try
            {
                var data = await _repo.GetReviewList(request, transaction);

                if (data.IsSuccess && data.Result != null && data.Result.Count > 0)
                {
                    List<ReviewsVM> mapresponse = _map.Map<List<ReviewsVM>>(data.Result);
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

        public async Task<ApiGenericResponseModel<ReviewsVM>> GetReviewById(long reviewId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<ReviewsVM> response = new ApiGenericResponseModel<ReviewsVM>();

            try
            {
                var data = await _repo.GetReviewById(reviewId, transaction);

                if (data.IsSuccess && data.Result != null)
                {
                    ReviewsVM mapresponse = _map.Map<ReviewsVM>(data.Result);
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

        public async Task<ApiGenericResponseModel<long>> SaveReview(ReviewsVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();

            try
            {
                Reviews mapmodel = _map.Map<Reviews>(data);
                var saveResponse = await _repo.SaveReview(mapmodel, transaction);

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

        public async Task<ApiGenericResponseModel<bool>> UpdateReview(ReviewsVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();

            try
            {
                Reviews mapmodel = _map.Map<Reviews>(data);
                var saveResponse = await _repo.UpdateReview(mapmodel, transaction);

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
