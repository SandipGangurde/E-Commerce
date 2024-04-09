using Business.Contract;
using Business.Service;
using DAL.DataContract.Contract;
using DAL.Repositories;
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

            //services.AddAuditService(configuration);
        }
    }
}
