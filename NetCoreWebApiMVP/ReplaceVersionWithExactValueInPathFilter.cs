using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetCoreWebApiMVP
{
   public class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
   {
      public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
      {
         var paths = new OpenApiPaths();

         var version = swaggerDoc.Info.Version;

         foreach (var path in swaggerDoc.Paths)
         {
            var key = path.Key.Replace("{version}", version)
                              .Replace("{version:apiVersion}", version);
            
            paths.Add(key, path.Value);
         }

         swaggerDoc.Paths = paths;
      }
   }
}
