using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Roomby.API.Rooms.Extensions
{
     public class CustomNameSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || schema?.Properties.Count == 0)
            {
                return;
            }

            var customAttributes = context.Type.GetCustomAttributes(typeof(JsonObjectAttribute), true);
            if (customAttributes.Length == 1)
            {
                JsonObjectAttribute objAttribute = (JsonObjectAttribute)customAttributes[0];
                if (objAttribute != default && objAttribute?.Title?.Length > 0)
                {
                    schema.Title = objAttribute.Title;
                }
            }
        }
    }
}