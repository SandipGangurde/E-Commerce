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
        Task<ApiGetResponseModel<List<Documents>>> GetDocumentsListByPrimaryKey(long PrimaryKey, string TableName, IDbTransaction transaction = null);
        Task<ApiGetResponseModel<List<Documents>>> GetDocumentList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Documents>> GetDocumentById(long DocumentId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveDocument(List<Documents> data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateDocument(List<Documents> data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteDocument(long DocumentID, IDbTransaction transaction = null);
    }
}
