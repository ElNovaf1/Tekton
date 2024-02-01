using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekton.Infrastructure.Interfaces;
using Tekton.Infrastructure.Repositories.Decorators;
using Tekton.Infrastructure.Repositories;
using Tekton.Infrastructure;
using Tekton.Service.Interfaces;
using Tekton.Service;

namespace Tekton.UnitTests.Helpers
{
    public class DependencyHelper
    {
        public ServiceProvider GetServiceProvider() {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IProductService, ProductService>();
            serviceCollection.AddSingleton<ProductService>();
            serviceCollection.AddScoped<DbFactory>();
            serviceCollection.AddScoped<IWorkUnit, WorkUnit>();

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var connectionString = config.GetConnectionString("sqlConnection");
            serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            serviceCollection.AddDbContext<TektonContext>(options =>
                options.UseSqlServer(connectionString));
            serviceCollection.AddScoped<Func<TektonContext>>((provider) => () => provider.GetService<TektonContext>());
            serviceCollection.AddTransient<IProductService, ProductService>();
            serviceCollection.AddTransient<IStatusService, StatusService>();
            serviceCollection.AddTransient<IHttpRepository, HttpRepository>();
            serviceCollection.AddTransient<IDiscountService, DiscountService>();
            serviceCollection.AddSingleton<IConfiguration>(config);
            serviceCollection.AddScoped<DbFactory>();
            serviceCollection.AddScoped<IWorkUnit, WorkUnit>();
            serviceCollection.AddScoped(typeof(Repository<>));
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(RepositoryCacheDecorator<>));
            serviceCollection.AddMemoryCache();
            return serviceCollection.BuildServiceProvider();
        }
    }
}
