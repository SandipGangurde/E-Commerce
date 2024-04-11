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
    public interface ICartItemsMasterRepository
    {
        Task<ApiGetResponseModel<List<CartItems>>> GetCartItemList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<CartItems>> GetCartItemById(long cartItemId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveCartItem(CartItems data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateCartItem(CartItems data, IDbTransaction transaction = null);
    }
}
