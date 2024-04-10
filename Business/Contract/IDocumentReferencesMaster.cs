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
        Task<ApiGetResponseModel<List<VuDocumentReferences>>> getDocumentReferenceList(ApiGetRequestModel request, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<DocumentReferencesVM>> getDocumentReferenceById(long DocumentReferenceID, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<long>> saveDocumentReference(DocumentReferencesVM data, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<bool>> updateDocumentReference(DocumentReferencesVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> deleteDocumentReference(string TableName, string PrimaryKeyValue, long DocumentID, IDbTransaction transaction = null);
    }
}
