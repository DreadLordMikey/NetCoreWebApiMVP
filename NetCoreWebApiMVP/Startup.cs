using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using System.IO;
using System.Reflection;
using System;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace NetCoreWebApiMVP
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
         services.AddControllers();

         services.AddApiVersioning(option => {
            option.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            option.ReportApiVersions = true;
            option.ApiVersionReader = new UrlSegmentApiVersionReader();
            option.AssumeDefaultVersionWhenUnspecified = true;
            option.UseApiBehavior = false;
         });

         services.AddMvcCore().AddNewtonsoftJson().AddApiExplorer();

         ConfigureSwaggerService(services);
      }

      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseHttpsRedirection();

         app.UseRouting();         

         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });

         app.UseSwagger(c => c.RouteTemplate = "api-docs/{documentName}/swagger.json");
         app.UseSwaggerUI(c =>
         {
            c.RoutePrefix = "help";
            c.SwaggerEndpoint("../api-docs/v1/swagger.json", "PT Web API V1");
            c.SwaggerEndpoint("../api-docs/v2/swagger.json", "PT Web API V2");
         });
      }

      private void ConfigureSwaggerService(IServiceCollection services)
      {
         services.AddVersionedApiExplorer(options =>
         {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
            options.SubstitutionFormat = "'v'VVV";
         });

         services.AddApiVersioning(o =>
         {
            o.ReportApiVersions = true;
            o.AssumeDefaultVersionWhenUnspecified = true;
         });

         services.AddSwaggerGen(options =>
         {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
               options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }

            options.EnableAnnotations();

            options.OperationFilter<SwaggerDefaultValues>();

            options.IncludeXmlComments(GetXmlDocumentationFilePath());
         });
      }

      private string GetXmlDocumentationFilePath()
      {
         var basePath = AppContext.BaseDirectory;
         var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
         return Path.Combine(basePath, fileName);
      }

      private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
      {
         var info = new OpenApiInfo()
         {
            Title = $"PT Web API {description.ApiVersion}",
            Version = description.ApiVersion.ToString(),
            Description = "Physical Therapy Web API"
         };

         if (description.IsDeprecated)
         {
            info.Description += " This API version has been deprecated.";
         }

         return info;
      }
   }
}
