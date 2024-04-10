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
    public interface IDocumentsMaster
    {

        Task<ApiGetResponseModel<List<DocumentsVM>>> getDocumentList(ApiGetRequestModel request, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<DocumentsVM>> getDocumentById(long DocumentId, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<long>> saveDocument(DocumentsVM data, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<bool>> updateDocument(DocumentsVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> deleteDocument(long DocumentID, IDbTransaction transaction = null);
    }
}
