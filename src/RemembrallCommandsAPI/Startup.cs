using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RemembrallCommandsAPI {
    public class Startup {

        //  providing us with the fabric to read config data from appsettings.json
        public IConfiguration Configuration { get; }
        public Startup (IConfiguration configuration) =>
            Configuration = configuration;

        public void ConfigureServices (IServiceCollection services) {
            // register our CommandContext class as a solution-wide DBContext and we point it to the
            // connection string
            services.AddDbContext<CommandContext> (opt => opt.UseNpgsql (Configuration.GetConnectionString ("PostgreSqlConnection")));

            services.AddControllers ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseRouting ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}