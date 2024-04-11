using AutoMapper;
using DataCarrier.ApplicationModels.Common;
using DataCarrier.ViewModels;
using DataModel.Entities;
using RepositoryOperations.ApplicationModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.AutomapperProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /*Entities to View Model Mapping*/
            CreateMap<ApiGetRequestModel, RequestModel>();
            CreateMap<ApplicationModels.Common.MultiSearchInColumn, RepositoryOperations.ApplicationModels.Common.MultiSearchInColumn>();
            CreateMap<ApplicationModels.Common.DateRangeFilter, RepositoryOperations.ApplicationModels.Common.DateRangeFilter>();
            CreateMap<Users, UsersVM>();
            CreateMap<Role, RoleVM>();
            CreateMap<UserRole, UserRoleVM>();
            CreateMap<Images, ImagesVM>();
            CreateMap<Categories, CategoriesVM>();
            CreateMap<Documents, DocumentsVM>();
            CreateMap<DocumentReferences, DocumentReferencesVM>();
            CreateMap<Products, ProductsVM>();
            CreateMap<Orders, OrdersVM>();
            CreateMap<OrderDetails, OrderDetailsVM>();
            CreateMap<Addresses, AddressesVM>();
            CreateMap<PaymentMethods, PaymentMethodsVM>();
            CreateMap<CartItems, CartItemsVM>();
            CreateMap<Reviews, ReviewsVM>();
            CreateMap<Discounts, DiscountsVM>();
            CreateMap<Transactions, TransactionsVM>();
            CreateMap<Shipping, ShippingVM>();
            CreateMap<Wishlist, WishlistVM>();

            /*View Model to Entities Mapping*/
            CreateMap<RequestModel, ApiGetRequestModel>();
            CreateMap<RepositoryOperations.ApplicationModels.Common.MultiSearchInColumn, ApplicationModels.Common.MultiSearchInColumn>();
            CreateMap<RepositoryOperations.ApplicationModels.Common.DateRangeFilter, ApplicationModels.Common.DateRangeFilter>();
            CreateMap<UsersVM, Users>();
            CreateMap<RoleVM, Role>();
            CreateMap<RoleVM, Role>();
            CreateMap<UserRoleVM, UserRole>();
            CreateMap<CategoriesVM, Categories>();
            CreateMap<DocumentsVM, Documents>();
            CreateMap<DocumentReferencesVM, DocumentReferences>();
            CreateMap<ProductsVM, Products>();
            CreateMap<OrdersVM, Orders>();
            CreateMap<OrderDetailsVM, OrderDetails>();
            CreateMap<AddressesVM, Addresses>();
            CreateMap<PaymentMethodsVM, PaymentMethods>();
            CreateMap<CartItemsVM, CartItems>();
            CreateMap<ReviewsVM, Reviews>();
            CreateMap<DiscountsVM, Discounts>();
            CreateMap<TransactionsVM, Transactions>();
            CreateMap<ShippingVM, Shipping>();
            CreateMap<WishlistVM, Wishlist>();

            /*View Model to View Model*/
            //CreateMap<PortCallIssuedPdaAndWorkFlowVM, ExportPdfDocumentVM>();

        }
    }
}
