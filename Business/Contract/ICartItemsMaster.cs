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
    public interface ICartItemsMaster
    {
        Task<ApiGetResponseModel<List<CartItemsVM>>> GetCartItemList(ApiGetRequestModel request, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<CartItemsVM>> GetCartItemById(long cartItemId, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<long>> SaveCartItem(CartItemsVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> UpdateCartItem(CartItemsVM data, IDbTransaction transaction = null);
        Task<ApiGenericResponseModel<bool>> DeleteCartItemByCartItemId(long cartItemId, IDbTransaction transaction = null);
        Task<ApiGetResponseModel<List<CartItemDetailsVM>>> GetCartItemDetail(ApiGetRequestModel request, IDbTransaction transaction = null);
    }
}
