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
    public interface IWishlistMasterRepository
    {
        Task<ApiGetResponseModel<List<Wishlist>>> GetWishlist(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<Wishlist>> GetWishlistById(long wishlistId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveWishlist(Wishlist data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateWishlist(Wishlist data, IDbTransaction transaction = null);
    }
}
