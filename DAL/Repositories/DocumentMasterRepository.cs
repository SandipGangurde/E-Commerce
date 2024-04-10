using AutoMapper;
using DAL.DataContract.Contract;
using DataCarrier.ApplicationModels.Common;
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
    public class DocumentMasterRepository : IDocumentsMasterRepository
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<Documents> _repository;
        private readonly ITransactions _localTransaction;
        private readonly IMapper _map;

        public DocumentMasterRepository(ILogger logger, IMapper map, IGenericRepository<Documents> repository, ITransactions transactions)
        {
            _logger = logger;
            _map = map;
            _repository = repository;
            _localTransaction = transactions;
        }

        public async Task<ApiGetResponseModel<List<Documents>>> getDocumentsListByPrimaryKey(long PrimaryKey, string TableName, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<Documents>> response = new ApiGetResponseModel<List<Documents>>();
            response.Result = new List<Documents>();

            try
            {
                if (PrimaryKey > 0)
                {
                    var data = await _repository.QuerySP(SqlConstants.SP_DocumentByPrimaryKey, new { PrimaryKey = PrimaryKey, TableName = TableName }, transaction: transaction);
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

        public async Task<ApiGenericResponseModel<Documents>> getDocumentById(long DocumentId, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<Documents> response = new ApiGenericResponseModel<Documents>();
            response.Result = new Documents();

            try
            {
                if (DocumentId > 0)
                {
                    var data = await _repository.QuerySP(SqlConstants.SP_DocumentByDocumentId, new { DocumentId = DocumentId }, transaction: transaction);
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

        public async Task<ApiGetResponseModel<List<Documents>>> getDocumentList(ApiGetRequestModel request, IDbTransaction transaction = null)
        {
            ApiGetResponseModel<List<Documents>> response = new ApiGetResponseModel<List<Documents>>();
            try
            {
                RequestModel searchRequest = _map.Map<RequestModel>(request);
                var data = await _repository.Query(new Documents(), searchRequest);
                string FilterQuery = SqlQueryHelper.GenerateQueryForTotalRecordsFound(new Documents(), searchRequest);
                int TotalRecord = (int)await _repository.ScalarDynamicResult(FilterQuery, transaction: transaction);
                if (data != null && data.Any())
                {
                    response.IsSuccess = true;
                    response.Result = data.ToList();
                    response.TotalRecords = TotalRecord;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Result = default;
                    response.TotalRecords = TotalRecord;
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

        public async Task<ApiGenericResponseModel<long>> saveDocument(List<Documents> data, IDbTransaction transaction = null)
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

        public async Task<ApiGenericResponseModel<bool>> updateDocument(List<Documents> data, IDbTransaction transaction = null)
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
                for (int i = 0; i < data.Count; i++)
                {
                    response.Result = await _repository.Update(data[i], transaction: localtran);
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

        public async Task<ApiGenericResponseModel<bool>> deleteDocument(long DocumentID, IDbTransaction transaction = null)
        {
            ApiGenericResponseModel<bool> response = new ApiGenericResponseModel<bool>();
            try
            {
                if (DocumentID > 0)
                {

                    int delDocStatus = (int)await _repository.ExecuteScalarSP(SqlConstants.SP_DeleteDocument, new { DocumentID = DocumentID }, transaction: transaction);
                    if (delDocStatus > 0)
                    {
                        /*Implement delete logic for the file from the local folder.*/
                    }
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
