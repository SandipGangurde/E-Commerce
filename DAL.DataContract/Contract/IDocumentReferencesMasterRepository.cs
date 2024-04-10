using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataContract.Contract
{
    public interface IDocumentReferencesMasterRepository
    {
        Task<ApiGetResponseModel<List<VuDocumentReferences>>> getDocumentReferenceList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<DocumentReferences>> getDocumentReferenceById(long DocumentReferenceID, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> saveDocumentReference(List<DocumentReferences> data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> updateDocumentReference(DocumentReferences data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<DocumentReferences>> getDocumentReferenceByDocId(long DocumentID, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<DocumentReferences>> getDocumentReferenceByPrimarykey(string PrimaryKeyValue, string TableName, long DocumentID, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> deleteDocumentReference(string TableName, string PrimaryKeyValue, long DocumentID, IDbTransaction transaction = null);
    }
}
