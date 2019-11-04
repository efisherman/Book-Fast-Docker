﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookFast.Files.Controllers;
using BookFast.Files.Mappers;
using BookFast.SeedWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookFast.Files
{
    public class Startup
    {
        private const string apiTitle = "Book Fast Files API";
        private const string apiVersion = "v1";

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(configuration);
            services.AddAuthorizationPolicies();

            services.AddSecurityContext();
            services.AddAndConfigureMvc();

            services.AddApplicationInsightsTelemetry(configuration);

            services.AddSwashbuckle(configuration, apiTitle, apiVersion, "BookFast.Files.xml");

            var modules = new List<ICompositionModule>
                          {
                              new Business.Composition.CompositionModule(),
                              new Data.Composition.CompositionModule()
                          };

            foreach (var module in modules)
            {
                module.AddServices(services, configuration);
            }

            services.AddScoped<IFileAccessMapper, FileAccessMapper>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseSecurityContext();
            app.UseMvc();

            app.UseSwagger(apiTitle, apiVersion);
        }
    }
}
