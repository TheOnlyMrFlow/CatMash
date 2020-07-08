using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatMash.Middlewares;
using CatMash.Models;
using CatMash.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace CatMash
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<CatMashDatabaseSettings>(
                Configuration.GetSection(nameof(CatMashDatabaseSettings)));


            services.AddSingleton<ICatMashDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<CatMashDatabaseSettings>>().Value);

            services.Configure<Secrets>(
                Configuration.GetSection(nameof(Secrets)));

            services.AddSingleton<ISecrets>(sp =>
                sp.GetRequiredService<IOptions<Secrets>>().Value);

            services.AddSingleton<CatService>();

            services.AddCors(); 


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().WithHeaders("content-type")
            );

            app.UseWhen(   
                context => context.Request.Path.StartsWithSegments("/cats/admin"),                
                appBuilder =>
                {
                    appBuilder.UseAdminCheck();
                });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

