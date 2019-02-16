using LudoGameEngine;
using LudoWebAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
namespace LudoWebAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvcCore();
            services.AddMvcCore().AddApiExplorer();
            services.AddSingleton<IGameContainer, GameContainer>();
            services.AddSingleton<IStatsContainter, StatsContainer>();
            services.AddTransient<IDiece, Diece>();
            //services.AddScoped<ILudoGame, LudoGame>(); //IF i add this to singeleton i can't create more than one Ludogame
            //and this code won't compile if its scopped because an singleton depends on it. Igamcontainer
            //services.AddSingleton<LudoController>(); Ask the teacher about this :)

            services.AddSwaggerGen(c =>
            {
                c.DescribeAllEnumsAsStrings();
                c.DescribeAllParametersInCamelCase();
                c.SwaggerDoc("v1", new Info { Title = "Ludo API" });
                var xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + @"LudoWebAPI.xml";
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ludo API");
            });
        }
    }
}
