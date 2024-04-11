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
    public class DocumentsMaster : IDocumentsMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly IDocumentsMasterRepository _repo;
        private readonly ITransactions _localTransaction;
        public DocumentsMaster(ILogger logger, IMapper map, IDocumentsMasterRepository repo, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _localTransaction = transactions;
        }

        public async Task<ApiGenericResponseModel<DocumentsVM>> GetDocumentById(long DocumentId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<DocumentsVM> response = new ApiGenericResponseModel<DocumentsVM>();
            response.Result = new DocumentsVM();
            try
            {
                var data = await _repo.GetDocumentById(DocumentId, transaction: transaction);
                DocumentsVM result = _map.Map<DocumentsVM>(data.Result);
                //Byte[] bytes = File.ReadAllBytes(data.Result.FilePath);
                Byte[] bytes = data.Result.FileData;
                result.FileBaseData = Convert.ToBase64String(bytes);

                response.IsSuccess = true;
                response.Result = result;
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

        public async Task<ApiGetResponseModel<List<DocumentsVM>>> GetDocumentList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<DocumentsVM>> response = new ApiGetResponseModel<List<DocumentsVM>>();
            try
            {
                var data = await _repo.GetDocumentList(request, transaction: transaction);
                if (data.Result != null)
                {
                    if (data.Result.Count > 0)
                    {
                        List<DocumentsVM> mapresponse = _map.Map<List<DocumentsVM>>(data.Result);
                        response.Result = mapresponse;
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
                response.Result = default;
                response.TotalRecords = 0;
                response.ErrorMessage.Add(exception.Message);
            }
            return response;
        }

        public async Task<ApiGenericResponseModel<long>> SaveDocument(DocumentsVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                List<Documents> mapmodel = _map.Map<List<Documents>>(data);
                response = await _repo.SaveDocument(mapmodel, transaction: localtran);

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

        public async Task<ApiGenericResponseModel<bool>> UpdateDocument(DocumentsVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                List<Documents> mapmodel = _map.Map<List<Documents>>(data);
                response = await _repo.UpdateDocument(mapmodel, transaction: localtran);

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

        public async Task<ApiGenericResponseModel<bool>> DeleteDocument(long DocumentID, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();
            try
            {
                response = await _repo.DeleteDocument(DocumentID, transaction: localtran);

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
