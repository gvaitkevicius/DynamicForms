using System.Globalization;
using System.IO;
using System.IO.Compression;
using DynamicForms.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using Rotativa.AspNetCore;

namespace DynamicForms
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

            services.AddDbContext<JSgi>(op => op
                .UseSqlServer(Configuration.GetConnectionString("PlayConect")));

            //adicionando o serviço de cookies na aplicação
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "DynamicForms";
                    //se o usuário não estiver logado, ele é redirecionado para a página de login
                    options.LoginPath = new PathString("/Acesso/Login");
                    options.AccessDeniedPath = new PathString("/Acesso/SemAcesso");
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });

            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });

            // Compressão do response
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(opt => opt.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddHttpContextAccessor();
            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Error");
                app.UseHsts();
            }

            //Forcando formato de data padrão
            var ptbr = "pt-BR";
            var supportedCultures = new[] { new CultureInfo(ptbr) };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: ptbr, uiCulture: ptbr),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // For the wwwroot folder
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documentos\")),
                RequestPath = "/Documentos"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\CenariosTestes\")),
                RequestPath = "/Estoque"
            });

            app.UseCookiePolicy();

            // Compressão do response
            app.UseResponseCompression();

            app.UseMvc(routes =>
            {
                routes.MapAreaRoute(
                    name: "plugandplay",
                    areaName: "plugandplay",
                    template: "PlugAndPlay/{controller}/{action}");

                routes.MapAreaRoute(
                    name: "sgi",
                    areaName: "sgi",
                    template: "SGI/{controller}/{action}");

                routes.MapAreaRoute(
                    name: "apiinterface",
                    areaName: "apiinterface",
                    template: "apiinterface/{controller}/{action}");

                routes.MapAreaRoute(
                    name: "apiotimizador",
                    areaName: "apiotimizador",
                    template: "apiotimizador/{controller}/{action}");
                
                routes.MapAreaRoute(
                                    name: "apiestoque",
                                    areaName: "apiestoque",
                                    template: "apiestoque/{controller}/{action}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Acesso}/{action=Login}/{id?}");

            });

            // var webRootPath = env.WebRootPath;
            // call rotativa conf passing env to get web root path
            RotativaConfiguration.Setup(env);

        }
    }
}



/*
 webconfig


    <?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <handlers>
      <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
    </handlers>
    <aspNetCore processPath=".\DynamicForms.exe" arguments="" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" />
  </system.webServer>
</configuration>
<!--ProjectGuid: 9720ac56-cbf4-4b05-b501-68522411b327-->
     
     */
