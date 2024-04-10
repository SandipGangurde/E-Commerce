using AutoMapper;
using DAL.DataContract.Contract;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using RepositoryOperations.ApplicationModels.Common;
using RepositoryOperations.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DocumentReferencesMasterRepository : IDocumentReferencesMasterRepository
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<DocumentReferences> _repository;
        private readonly ITransactions _localTransaction;
        private readonly IMapper _map;

        public DocumentReferencesMasterRepository(ILogger logger, IMapper map, IGenericRepository<DocumentReferences> repository, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repository = repository;
            _localTransaction = transactions;
        }
        public async Task<ApiGenericResponseModel<DocumentReferences>> getDocumentReferenceById(long DocumentReferenceID, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<DocumentReferences> response = new ApiGenericResponseModel<DocumentReferences>();
            response.Result = new DocumentReferences();

            try
            {
                if (DocumentReferenceID > 0)
                {
                    var data = await _repository.QuerySP(SqlConstants.SP_DocumentReferenceByDocumentReferenceID, new { DocumentReferenceID = DocumentReferenceID }, transaction: transaction);
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
        public async Task<ApiGenericResponseModel<DocumentReferences>> getDocumentReferenceByDocId(long DocumentID, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<DocumentReferences> response = new ApiGenericResponseModel<DocumentReferences>();
            response.Result = new DocumentReferences();

            try
            {
                if (DocumentID > 0)
                {
                    var data = await _repository.QuerySP(SqlConstants.SP_DocumentReferenceByDocumentID, new { DocumentID = DocumentID }, transaction: transaction);
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
        public async Task<ApiGenericResponseModel<DocumentReferences>> getDocumentReferenceByPrimarykey(string PrimaryKeyValue, string TableName, long DocumentID, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<DocumentReferences> response = new ApiGenericResponseModel<DocumentReferences>();
            response.Result = new DocumentReferences();

            try
            {
                if (!string.IsNullOrEmpty(PrimaryKeyValue))
                {
                    var data = await _repository.QuerySP(SqlConstants.SP_DocumentReferenceByDocumentReferenceByPrimary, new { PrimaryKeyValue = PrimaryKeyValue, TableName = TableName, DocumentID = DocumentID }, transaction: transaction);
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
        public async Task<ApiGetResponseModel<List<DocumentReferences>>> getDocumentReferenceListByPrimaryKey(long PrimaryKey, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<DocumentReferences>> response = new ApiGetResponseModel<List<DocumentReferences>>();
            response.Result = new List<DocumentReferences>();

            try
            {
                if (PrimaryKey > 0)
                {
                    var data = await _repository.QuerySP(SqlConstants.SP_DocumentReferenceByPrimaryKey, new { PrimaryKey = PrimaryKey }, transaction: transaction);
                    if (data != null && data.Any())
                    {
                        response.IsSuccess = true;
                        response.Result = data.ToList();
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
        public async Task<ApiGetResponseModel<List<VuDocumentReferences>>> getDocumentReferenceList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<VuDocumentReferences>> response = new ApiGetResponseModel<List<VuDocumentReferences>>();
            try
            {
                RequestModel searchRequest = _map.Map<RequestModel>(request);
                string sql = SqlQueryHelper.GenerateQueryFromEntity(new VuDocumentReferences(), searchRequest);
                var data = await _repository.QueryDynamicResult(sql, transaction: transaction);
                string FilterQuery = SqlQueryHelper.GenerateQueryForTotalRecordsFound(new VuDocumentReferences(), searchRequest);
                FilterQuery = sql.Split("ORDER BY")[0];
                FilterQuery = FilterQuery.Replace("*", "COUNT(1)");
                int TotalRecord = (int)await _repository.ScalarDynamicResult(FilterQuery, transaction: transaction);
                if (data != null && data.Any())
                {
                    var config = new MapperConfiguration(cfg => { });
                    var mapper = config.CreateMapper();

                    var result = mapper.Map<List<VuDocumentReferences>>(data.ToList());

                    response.IsSuccess = true;
                    response.Result = result;
                    response.TotalRecords = TotalRecord;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Result = null;
                    response.TotalRecords = TotalRecord;
                    response.ErrorMessage.Add("No records found");
                }
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
        public async Task<ApiGenericResponseModel<long>> saveDocumentReference(List<DocumentReferences> data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<long> response = new ApiGenericResponseModel<long>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                response.IsSuccess = true;
                for (int i = 0; i < data.Count; i++)
                {
                    response.Result = await _repository.Insert(data[i], localtran);
                }

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
        public async Task<ApiGenericResponseModel<bool>> updateDocumentReference(DocumentReferences data, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            IDbTransaction localtran = null;

            if (transaction != null)
                localtran = transaction;
            else
                localtran = _localTransaction.BeginTransaction();

            try
            {
                response.IsSuccess = true;
                response.Result = await _repository.Update(data, transaction: localtran);

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
        public async Task<ApiGenericResponseModel<bool>> deleteDocumentReference(string TableName, string PrimaryKeyValue, long DocumentID, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            try
            {
                if (!string.IsNullOrEmpty(PrimaryKeyValue) && !string.IsNullOrEmpty(TableName) && DocumentID > 0)
                {
                    await _repository.ExecuteScalarSP(SqlConstants.SP_DeleteDocumentReference, new { TableName = TableName, PrimaryKeyValue = PrimaryKeyValue.ToString(), DocumentID = DocumentID }, transaction: transaction);
                    response.IsSuccess = true;
                    response.Result = true;
                }
                else
                {
                    response.Result = false;
                    response.IsSuccess = true;
                    response.ErrorMessage.Add("No records found");
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
    }
}
