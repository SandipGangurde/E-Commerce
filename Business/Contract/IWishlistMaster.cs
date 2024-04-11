using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contract
{
    public interface IWishlistMaster
    {
        Task<ApiGetResponseModel<List<WishlistVM>>> GetWishlist(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<WishlistVM>> GetWishlistById(long WishlistId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveWishlist(WishlistVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateWishlist(WishlistVM data, IDbTransaction transaction = null);
    }
}
