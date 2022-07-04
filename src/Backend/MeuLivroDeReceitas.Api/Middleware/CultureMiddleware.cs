using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Api.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IList<string> _idiomasSuportados = new List<string>
    {
        "pt",
        "en"
    };

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var cultura = new CultureInfo("pt");

        if (context.Request.Headers.ContainsKey("Accept-Language"))
        {
            var linguagem = context.Request.Headers["Accept-Language"].ToString();
            
            if (_idiomasSuportados.Any(c => c.Equals(linguagem)))
            {
                cultura = new CultureInfo(linguagem);
            }
        }

        CultureInfo.CurrentCulture = cultura;
        CultureInfo.CurrentUICulture = cultura;

        await _next(context);
    }
}
