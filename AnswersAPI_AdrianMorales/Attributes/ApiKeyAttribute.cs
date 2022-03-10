using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

namespace AnswersAPI_AdrianMorales.Attributes
{
    // Decoraciones, como las anotaciones en Spring Boot
    [AttributeUsage(validOn: AttributeTargets.All)]
    public sealed class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        // Estamos creando un atributo que luego usaremos como decoración para nuestros 
        // controllers e inyectarle el mecanismo de seguridad por ApiKey para darle
        // una capa de seguridad a nuestros end points

        private const string NombreDelApiKey = "ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(NombreDelApiKey,out var ApiSalida))
            {

                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "No se ha incluido una API Key"
                };
                return;
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            
            var apikey = appSettings.GetValue<string>(NombreDelApiKey);

            if (!apikey.Equals(ApiSalida))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "La API Key suministrada no es la correcta."
                };
                return;
            }

            await next();

        }

    }
}
