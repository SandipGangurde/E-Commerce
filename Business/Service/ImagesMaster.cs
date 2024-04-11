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
using static System.Net.Mime.MediaTypeNames;

namespace Business.Service
{
    public class ImagesMaster : IImagesMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly IImagesMasterRepository _repo;
        private readonly ITransactions _localTransaction;

        public ImagesMaster(ILogger logger, IMapper map, IImagesMasterRepository repo, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _localTransaction = transactions;
        }

        public async Task<ApiGenericResponseModel<ImagesVM>> GetImageById(long imageId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<ImagesVM> response = new ApiGenericResponseModel<ImagesVM>();
            response.Result = new ImagesVM();

            try
            {
                var data = await _repo.GetImageById(imageId, transaction: transaction);
                response.IsSuccess = true;
                response.Result = _map.Map<ImagesVM>(data.Result);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                response.IsSuccess = false;
                response.ErrorMessage.Add(exception.Message);
            }
            return response;
        }

        public async Task<ApiGetResponseModel<List<ImagesVM>>> GetImageList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<ImagesVM>> response = new ApiGetResponseModel<List<ImagesVM>>();
            try
            {
                var data = await _repo.GetImageList(request, transaction: transaction);
                if (data.Result != null)
                {
                    if (data.Result.Count > 0)
                    {
                        List<ImagesVM> mapResponse = _map.Map<List<ImagesVM>>(data.Result);
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

        public async Task<ApiGenericResponseModel<long>> SaveImage(ImagesVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();
            IDbTransaction localTransaction = null;

            if (transaction != null)
                localTransaction = transaction;
            else
                localTransaction = _localTransaction.BeginTransaction();

            try
            {
                Images mapModel = _map.Map<Images>(data);
                response = await _repo.SaveImage(mapModel, transaction: localTransaction);

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

        public async Task<ApiGenericResponseModel<bool>> UpdateImage(ImagesVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localTransaction = null;

            if (transaction != null)
                localTransaction = transaction;
            else
                localTransaction = _localTransaction.BeginTransaction();

            try
            {
                Images mapModel = _map.Map<Images>(data);
                response = await _repo.UpdateImage(mapModel, transaction: localTransaction);

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

        public async Task<ApiGenericResponseModel<bool>> DeleteImage(long imageId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localTransaction = null;

            if (transaction != null)
                localTransaction = transaction;
            else
                localTransaction = _localTransaction.BeginTransaction();

            try
            {
                response = await _repo.DeleteImage(imageId, transaction: localTransaction);

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
