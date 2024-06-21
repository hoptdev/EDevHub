using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace UserService.Helpers.Attributes
{
    public class SwaggerHeaderAttribute : Attribute
    {
        public string HeaderName { get; }
        public string Description { get; }
        public bool IsRequired { get; }

        public SwaggerHeaderAttribute(string headerName, string? description = null, bool isRequired = false)
        {
            HeaderName = headerName;
            Description = description;
            IsRequired = isRequired;
        }
    }
    public class SwaggerHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            if (context.MethodInfo.GetCustomAttribute(typeof(SwaggerHeaderAttribute)) is SwaggerHeaderAttribute attribute)
            {
                var existingParam = operation.Parameters.FirstOrDefault(p =>
                    p.In == ParameterLocation.Header && p.Name == attribute.HeaderName);
                if (existingParam != null) // remove description from [FromHeader] argument attribute
                {
                    operation.Parameters.Remove(existingParam);
                }

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = attribute.HeaderName,
                    In = ParameterLocation.Header,
                    Description = attribute.Description,
                    Required = attribute.IsRequired,
                    Schema = new OpenApiSchema
                    {
                        Type = "String",
                    }
                });
            }
        }
    }
}
