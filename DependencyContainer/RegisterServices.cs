using Business.Contract;
using Business.Service;
using DAL.DataContract.Contract;
using DAL.Repositories;
using DataModel.Entities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryOperations.IoC;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyContainer
{
    public static class RegisterServices
    {
        public static void RegisterProjectService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(Log.Logger);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.RegisterServices(configuration);

            services.AddTransient<ICategoriesMasterRepository, CategoriesMasterRepository>();
            services.AddTransient<ICategoriesMaster, CategoriesMaster>();
            services.AddTransient<IDocumentsMasterRepository, DocumentMasterRepository>();
            services.AddTransient<IDocumentsMaster, DocumentsMaster>();
            services.AddTransient<IDocumentReferencesMasterRepository, DocumentReferencesMasterRepository>();
            services.AddTransient<IDocumentReferencesMaster, DocumentReferencesMaster>();

            services.AddTransient<IProductsMasterRepository, ProductsMasterRepository>();
            services.AddTransient<IProductsMaster, ProductsMaster>();
            services.AddTransient<IOrdersMasterRepository, OrdersMasterRepository>();
            services.AddTransient<IOrdersMaster, OrdersMaster>();
            services.AddTransient<IOrderDetailsMasterRepository, OrderDetailsMasterRepository>();
            services.AddTransient<IOrderDetailsMaster, OrderDetailsMaster>();
            services.AddTransient<ICustomersMasterRepository, CustomersMasterRepository>();
            services.AddTransient<ICustomersMaster, CustomersMaster>();
            services.AddTransient<IAddressesMasterRepository, AddressesMasterRepository>();
            services.AddTransient<IAddressesMaster, AddressesMaster>();
            services.AddTransient<IPaymentMethodsMasterRepository, PaymentMethodsMasterRepository>();
            services.AddTransient<IPaymentMethodsMaster, PaymentMethodsMaster>(); 
            services.AddTransient<ICartItemsMasterRepository, CartItemsMasterRepository>();
            services.AddTransient<ICartItemsMaster, CartItemsMaster>();
            services.AddTransient<IReviewsMasterRepository, ReviewsMasterRepository>();
            services.AddTransient<IReviewsMaster, ReviewsMaster>();
            services.AddTransient<IDiscountsMasterRepository, DiscountsMasterRepository>();
            services.AddTransient<IDiscountsMaster, DiscountsMaster>(); 
            services.AddTransient<ITransactionsMasterRepository, TransactionsMasterRepository>();
            services.AddTransient<ITransactionsMaster, TransactionsMaster>();
            services.AddTransient<IShippingMasterRepository, ShippingMasterRepository>();
            services.AddTransient<IShippingMaster, ShippingMaster>();
            services.AddTransient<IWishlistMasterRepository, WishlistMasterRepository>();
            services.AddTransient<IWishlistMaster, WishlistMaster>();

            //services.AddAuditService(configuration);
        }
    }
}
