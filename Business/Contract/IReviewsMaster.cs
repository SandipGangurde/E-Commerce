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

    public interface IReviewsMaster
    {
        Task<ApiGetResponseModel<List<ReviewsVM>>> GetReviewList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<ReviewsVM>> GetReviewById(long addressId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveReview(ReviewsVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateReview(ReviewsVM data, IDbTransaction transaction = null);
    }
}
