using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repositories;
using Repositories.Interfaces;
using Repositories.Models;
using Repositories.Repositories;
using Services.Interfaces;
using Services.Services;

namespace RepositoryPresentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("MyStore"));
            services.AddTransient<ApplicationDbContext>();
            services.AddMvc();

            services.AddScoped<ITransactionManager, TransactionManager>();
            services.AddScoped<IProductsRepository, ProductRepositoryEmpty>();
            services.AddScoped<IProductsRepositoryWithLogic, ProductRepositoryWithLogic>();
            services.AddScoped<IRepository<Product>, ProductRepositoryNoDbContextControl>();
            services.AddScoped<IATMRepository, ATMRepository>();

            //services.AddScoped<IProductService, ProductServiceUsingDbSet>();
            services.AddScoped<IProductService, ProductServiceUsingRepositoryLogic>();
            services.AddScoped<IATMService, ATMService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
