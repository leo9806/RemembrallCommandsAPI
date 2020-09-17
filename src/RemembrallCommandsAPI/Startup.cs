using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using RemembrallCommandsAPI.Models;

namespace RemembrallCommandsAPI {
    public class Startup {

        //  providing us with the fabric to read config data from appsettings.json
        public IConfiguration Configuration { get; }
        public Startup (IConfiguration configuration) =>
            Configuration = configuration;

        public void ConfigureServices (IServiceCollection services) {
            var builder = new NpgsqlConnectionStringBuilder ();
            builder.ConnectionString =
                Configuration.GetConnectionString ("PostgreSqlConnection");
            builder.Username = Configuration["UserID"];
            builder.Password = Configuration["Password"];

            // register our CommandContext class as a solution-wide DBContext and we point it to the
            // connection string
            services.AddDbContext<CommandContext> (opt => opt.UseNpgsql ((builder.ConnectionString)));

            services.AddControllers ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env, CommandContext context) {
            context.Database.Migrate ();
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