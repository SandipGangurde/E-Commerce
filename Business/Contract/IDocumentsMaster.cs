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

        Task<ApiGetResponseModel<List<DocumentsVM>>> GetDocumentList(ApiGetRequestModel request, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<DocumentsVM>> GetDocumentById(long DocumentId, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<long>> SaveDocument(DocumentsVM data, IDbTransaction transaction = null);

        Task<ApiGenericResponseModel<bool>> UpdateDocument(DocumentsVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteDocument(long DocumentID, IDbTransaction transaction = null);
    }
}
