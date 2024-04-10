using DataCarrier.ApplicationModels.Common;
using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataContract.Contract
{
    public interface IDocumentsMasterRepository
    {
        Task<ApiGetResponseModel<List<Documents>>> getDocumentsListByPrimaryKey(long PrimaryKey, string TableName, IDbTransaction transaction = null);
        Task<ApiGetResponseModel<List<Documents>>> getDocumentList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Documents>> getDocumentById(long DocumentId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> saveDocument(List<Documents> data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> updateDocument(List<Documents> data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> deleteDocument(long DocumentID, IDbTransaction transaction = null);
    }
}
