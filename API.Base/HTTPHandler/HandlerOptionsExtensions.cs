using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGS.API.Base
{
    public static  class HandlerOptionsExtensions
    {

        public static IApplicationBuilder UseMiddlewareOptions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HandlerOptions>();
        }
    }
}
