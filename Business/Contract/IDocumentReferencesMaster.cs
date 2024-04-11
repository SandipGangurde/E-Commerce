using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contract
{
    public interface IDocumentReferencesMaster
    {
        Task<ApiGetResponseModel<List<VuDocumentReferences>>> GetDocumentReferenceList(ApiGetRequestModel request, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<DocumentReferencesVM>> GetDocumentReferenceById(long DocumentReferenceID, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<long>> SaveDocumentReference(DocumentReferencesVM data, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<bool>> UpdateDocumentReference(DocumentReferencesVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteDocumentReference(string TableName, string PrimaryKeyValue, long DocumentID, IDbTransaction transaction = null);
    }
}
