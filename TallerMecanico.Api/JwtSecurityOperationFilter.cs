using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TallerMecanico.Api;

public sealed class JwtSecurityOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var controller = context.MethodInfo.DeclaringType;
        var allowsAnonymous = context.MethodInfo.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
            || controller?.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) == true;
        var requiresAuthorization = context.MethodInfo.IsDefined(typeof(AuthorizeAttribute), inherit: true)
            || controller?.IsDefined(typeof(AuthorizeAttribute), inherit: true) == true;

        if (!requiresAuthorization || allowsAnonymous)
            return;

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }] = Array.Empty<string>()
        });
    }
}
