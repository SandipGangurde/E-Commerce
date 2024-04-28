using AutoMapper;
using Business.Contract;
using DAL;
using DAL.DataContract.Contract;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ApplicationModels.Products.Response;
using DataCarrier.ViewModels;
using DataModel.Entities;
using RepositoryOperations.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service
{
    public class ProductsMaster : IProductsMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly IProductsMasterRepository _repo;
        private readonly IGenericRepository<Images> _repoGenImage;
        private readonly IImageHelper _imageHelper;
        private readonly ITransactions _localTransaction;

        public ProductsMaster(ILogger logger, IMapper map, IProductsMasterRepository repo, IGenericRepository<Images> repoGenImage, IImageHelper imageHelper, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _repoGenImage = repoGenImage;
            _imageHelper = imageHelper;
            _localTransaction = transactions;
        }

        public async Task<ApiGetResponseModel<List<ProductsVM>>> GetProductList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<ProductsVM>> response = new ApiGetResponseModel<List<ProductsVM>>();

            try
            {
                var data = await _repo.GetProductList(request, transaction: transaction);
                if (data.IsSuccess && data.Result != null && data.Result.Count > 0)
                {
                    List<ProductsVM> mapresponse = _map.Map<List<ProductsVM>>(data.Result);
                    foreach (var item in mapresponse)
                    {
                        var storeImg = await _repoGenImage.Query("SELECT * FROM Images WHERE TableName = 'Products' and RecordId = " + item.ProductId, transaction: transaction);
                        if (storeImg.Any())
                        {
                            var image = storeImg.FirstOrDefault();
                            item.ImageFilePath = _imageHelper.GetBase64ImageData(image.FilePath);
                        }
                    }
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

        public async Task<ApiGenericResponseModel<ProductsVM>> GetProductById(long productId, IDbTransaction transaction = null)
        {

            ApiGenericResponseModel<ProductsVM> response = new ApiGenericResponseModel<ProductsVM>();
            response.Result = new ProductsVM();
            try
            {
                var data = await _repo.GetProductById(productId, transaction: transaction);
                response.IsSuccess = true;
                var product = _map.Map<ProductsVM>(data.Result);
                product.ImageFilePath = _imageHelper.GetBase64ImageData(product.ImageFilePath);
                response.Result = product;
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

        public async Task<ApiGenericResponseModel<long>> SaveProduct(ProductsVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();

            try
            {
                Products mapmodel = _map.Map<Products>(data);
                var saveResponse = await _repo.SaveProduct(mapmodel, transaction);

                if (saveResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Result = saveResponse.Result;

                    var storeImg = await _repoGenImage.Query("SELECT * FROM Images WHERE TableName = 'Products' and RecordId = " + response.Result, transaction: transaction);

                    if (storeImg.Any())
                    {
                        foreach (var storeImage in storeImg)
                        {
                            _imageHelper.DeleteImage(storeImage.FilePath);
                            await _repoGenImage.ExecuteScalarSP(SqlConstants.SP_DeleteImage, new { ImageId = storeImage.ImageId }, transaction: transaction);
                        }
                    }

                    if (!string.IsNullOrEmpty(data.ImageFilePath))
                    {
                        var image = new Images
                        {
                            ImageId = 0,
                            FileName = response.Result.ToString() + "_Products_" + mapmodel.ProductName,
                            TableName = "Products",
                            FilePath = data.ImageFilePath,
                            RecordId = response.Result,
                        };
                        var saveimage = _imageHelper.SaveImage(image);
                        await _repoGenImage.Insert(saveimage);
                    }
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

        public async Task<ApiGenericResponseModel<bool>> UpdateProduct(ProductsVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                Products mapmodel = _map.Map<Products>(data);
                response = await _repo.UpdateProduct(mapmodel, transaction: localtran);

                var storeImg = await _repoGenImage.Query("SELECT * FROM Images WHERE TableName = 'Products' and RecordId = " + mapmodel.ProductId, transaction: localtran);

                // Check if there's an image file path
                if (!string.IsNullOrEmpty(data.ImageFilePath))
                {
                    var fileName = mapmodel.ProductId.ToString() + "_Products_" + mapmodel.ProductName; // Concatenating strings

                    var image = new Images
                    {
                        ImageId = storeImg.FirstOrDefault()?.ImageId ?? 0, // Default to 0 if not found
                        FileName = fileName,
                        TableName = "Products",
                        FilePath = data.ImageFilePath,
                        RecordId = mapmodel.ProductId
                    };

                    var saveImage = _imageHelper.SaveImage(image);

                    // Decide whether to insert or update based on the presence of existing image
                    if (storeImg.Any())
                    {
                        await _repoGenImage.Update(saveImage);
                    }
                    else
                    {
                        await _repoGenImage.Insert(saveImage);
                    }
                }
                else // If no image file path
                {
                    foreach (var storeImage in storeImg)
                    {
                        _imageHelper.DeleteImage(storeImage.FilePath);
                        await _repoGenImage.ExecuteScalarSP(SqlConstants.SP_DeleteImage, new { ImageId = storeImage.ImageId }, transaction: transaction);
                    }
                }

                //if (transaction == null && localtran != null)
                //    localtran.Commit();
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
        public async Task<ApiGetResponseModel<List<ProductDetailVM>>> GetProductDetailList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<ProductDetailVM>> response = new ApiGetResponseModel<List<ProductDetailVM>>();

            try
            {
                var data = await _repo.GetProductDetailList(request, transaction: transaction);
                if (data.IsSuccess && data.Result != null && data.Result.Count > 0)
                {
                    List<ProductDetailVM> mapresponse = _map.Map<List<ProductDetailVM>>(data.Result);

                    foreach (var item in mapresponse)
                    {
                        var storeImg = await _repoGenImage.Query("SELECT * FROM Images WHERE TableName = 'Products' and RecordId = " + item.ProductId, transaction: transaction);
                        if (storeImg.Any())
                        {
                            var image = storeImg.FirstOrDefault();
                            item.ImageFilePath = _imageHelper.GetBase64ImageData(image.FilePath);
                        }
                    }
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
    }
}
