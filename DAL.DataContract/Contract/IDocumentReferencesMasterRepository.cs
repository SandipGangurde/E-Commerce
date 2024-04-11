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
        Task<ApiGetResponseModel<List<VuDocumentReferences>>> GetDocumentReferenceList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<DocumentReferences>> GetDocumentReferenceById(long DocumentReferenceID, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveDocumentReference(List<DocumentReferences> data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateDocumentReference(DocumentReferences data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<DocumentReferences>> GetDocumentReferenceByDocId(long DocumentID, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<DocumentReferences>> GetDocumentReferenceByPrimarykey(string PrimaryKeyValue, string TableName, long DocumentID, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteDocumentReference(string TableName, string PrimaryKeyValue, long DocumentID, IDbTransaction transaction = null);
    }
}
