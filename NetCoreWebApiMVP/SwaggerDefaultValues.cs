using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Linq;

namespace NetCoreWebApiMVP
{
   /// <summary>
   /// Provides the operation filter to use when generating Swagger documentation.
   /// </summary>
   /// <seealso cref="Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter" />
   public class SwaggerDefaultValues : IOperationFilter
   {
      /// <summary>  
      /// Applies the filter to the specified operation using the given context.  
      /// </summary>  
      /// <param name="operation">The operation to apply the filter to.</param>  
      /// <param name="context">The current operation filter context.</param>  
      public void Apply(OpenApiOperation operation, OperationFilterContext context)
      {
         if (operation.Parameters == null)
         {
            return;
         }

         var apiVersionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "api-version");
         if (apiVersionParameter != null)
         {
            operation.Parameters.Remove(apiVersionParameter);
         }

         foreach (var parameter in operation.Parameters)
         {
            var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
            var routeInfo = description.RouteInfo;

            if (parameter.Description == null)
            {
               parameter.Description = description.ModelMetadata?.Description;
            }

            if (routeInfo == null)
            {
               continue;
            }

            parameter.Required |= !routeInfo.IsOptional;
         }
      }
   }

}
