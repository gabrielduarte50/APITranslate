using ApiTranslate.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApiTranslate.Infra.CrossCutting;
using ApiTranslate.Domain;
using ApiTranslate.Domain.Interfaces.Service;
using ApiTranslate.Domain.Interfaces.Apis;
using ApiTranslate.Infra.CrossCutting.Apis;
using ApiTranslate.Domain.Interfaces.Repositories;
using ApiTranslate.Infra.CrossCutting.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ApiTranslate
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(opt =>
            {
                opt.LoginPath = "/account/google-login";
            })
             .AddGoogle(opts =>
             {
                 opts.ClientId = "412602187402-606djaeg6im0ng2a4bg9t846v8la6t9c.apps.googleusercontent.com";
                 opts.ClientSecret = "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";
             });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
                {
                   Title = "ApiTranslate", 
                   Version = "v1"
                });
            });
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                //.WithOrigins("https://localhost:4200", "") //Habilita Cors para endpoint expecifico
                .SetIsOriginAllowed(isOriginAllowed: _ => true) //Habilitar para todas as Rotas
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                );
            });
            services.AddScoped<IHapiFhirService, HapiFhirService>();
            services.AddScoped<IHuamiService, HuamiService>();
            services.AddScoped<IHapiFhirRepository, HapiFhirRepository>();
            services.AddScoped<IGoogleApi, GoogleApi>();
            services.AddScoped<IHuamiApi, HuamiApi>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiTranslate V1");
                opt.RoutePrefix = "swagger";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

//            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
