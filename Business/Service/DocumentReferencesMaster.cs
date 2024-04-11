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
    public class DocumentReferencesMaster : IDocumentReferencesMaster
    {
        private readonly ILogger _logger;
        private readonly IMapper _map;
        private readonly IDocumentReferencesMasterRepository _repo;
        private readonly ITransactions _localTransaction;
        public DocumentReferencesMaster(ILogger logger, IMapper map, IDocumentReferencesMasterRepository repo, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repo = repo;
            _localTransaction = transactions;
        }
        public async Task<ApiGenericResponseModel<DocumentReferencesVM>> GetDocumentReferenceById(long DocumentReferenceID, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<DocumentReferencesVM> response = new ApiGenericResponseModel<DocumentReferencesVM>();
            response.Result = new DocumentReferencesVM();
            try
            {
                var data = await _repo.GetDocumentReferenceById(DocumentReferenceID, transaction: transaction);

                response.IsSuccess = true;
                response.Result = _map.Map<DocumentReferencesVM>(data.Result);
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

        public async Task<ApiGetResponseModel<List<VuDocumentReferences>>> GetDocumentReferenceList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<VuDocumentReferences>> response = new ApiGetResponseModel<List<VuDocumentReferences>>();
            try
            {
                var data = await _repo.GetDocumentReferenceList(request, transaction: transaction);

                if (data.Result != null && data.Result.Count > 0)
                {
                    response.IsSuccess = true;
                    response.Result = data.Result;
                    response.TotalRecords = data.TotalRecords;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Result = default;
                    response.TotalRecords = 0;
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

        public async Task<ApiGenericResponseModel<long>> SaveDocumentReference(DocumentReferencesVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();
            try
            {
                List<DocumentReferences> mapmodel = _map.Map<List<DocumentReferences>>(data);
                response = await _repo.SaveDocumentReference(mapmodel, transaction: localtran);
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

        public async Task<ApiGenericResponseModel<bool>> UpdateDocumentReference(DocumentReferencesVM data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                DocumentReferences mapmodel = _map.Map<DocumentReferences>(data);
                response = await _repo.UpdateDocumentReference(mapmodel, transaction: localtran);
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

        public async Task<ApiGenericResponseModel<bool>> DeleteDocumentReference(string TableName, string PrimaryKeyValue, long DocumentID, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();
            try
            {
                response = await _repo.DeleteDocumentReference(TableName, PrimaryKeyValue, DocumentID, transaction: localtran);

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
