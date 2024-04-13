using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class SqlConstants
    {
        public const string SP_CategoriesByCategoryCode = "SP_CountryByCountryCode";


        public const string SP_CategoriesByCategoryId = "SP_CategoriesByCategoryId";


        public const string SP_DocumentByDocumentId = "SP_DocumentByDocumentId";
        public const string SP_DateRangeFilter = "SP_DateRangeFilter";
        public const string SP_DocumentByPrimaryKey = "SP_DocumentByPrimaryKey";
        public const string SP_DocumentReferenceByPrimaryKey = "SP_DocumentReferenceByPrimaryKey";
        public const string SP_DocumentReferenceByDocumentReferenceID = "SP_DocumentReferenceByDocumentReferenceID";
        public const string SP_DocumentReferenceByDocumentID = "SP_DocumentReferenceByDocumentID";
        public const string SP_DocumentReferenceByDocumentReferenceByPrimary = "SP_DocumentReferenceByDocumentReferenceByPrimary";
        public const string SP_DeleteDocumentReference = "SP_DeleteDocumentReference";
        public const string SP_DeleteDocument = "SP_DeleteDocument";



        public const string SP_ProductsByProductId = "SP_ProductsByProductId";
        public const string SP_OrdersByOrderId = "SP_OrdersByOrderId";
        public const string SP_OrderDetailsById = "SP_OrderDetailsById";
        public const string SP_AddressesByAddressId = "SP_AddressesByAddressId";
        public const string SP_CartItemsByCartItemId = "SP_CartItemsByCartItemId";
        public const string SP_PaymentMethodsByPaymentMethodId = "SP_PaymentMethodsByPaymentMethodId";
        public const string SP_ReviewsByReviewId = "SP_ReviewsByReviewId";
        public const string SP_DiscountsByDiscountsId = "SP_DiscountsByDiscountsId";
        public const string SP_ShippingByShippingId = "SP_ShippingByShippingId";
        public const string SP_TransactionsByTransactionId = "SP_TransactionsByTransactionId";
        public const string SP_WishlistByWishlistId = "SP_WishlistByWishlistId";
        
        
        
        public const string SP_UserByUserId = "SP_UserByUserId";
        public const string SP_ImageByImageId = "SP_ImageByImageId";
        public const string SP_DeleteImage = "SP_DeleteImage";
        public const string SP_RoleByRoleId = "SP_RoleByRoleId";
        public const string SP_DeleteRole = "SP_DeleteRole";
        public const string SP_UserRoleByUserRoleId = "SP_UserRoleByUserRoleId";
        public const string SP_DeleteUserRole = "SP_DeleteUserRole";
        public const string SP_GetUserDetailsByEmail = "SP_GetUserDetailsByEmail";
        public const string SP_UserDetailByEmail = "SP_UserDetailByEmail";
        public const string SP_DeleteCartItemByCartItemId = "SP_DeleteCartItemByCartItemId";




    }
}
