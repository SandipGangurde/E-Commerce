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
    public interface IReviewsMasterRepository
    {
        Task<ApiGetResponseModel<List<Reviews>>> GetReviewList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Reviews>> GetReviewById(long reviewId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveReview(Reviews data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateReview(Reviews data, IDbTransaction transaction = null);
    }
}
