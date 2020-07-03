using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SGS.API.Base
{
    public class HandlerOptions : DelegatingHandler
    {
        private static readonly string[] Methods ={ "GET", "PUT", "POST", "PATCH", "DELETE", "HEAD", "TRACE" };

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public HandlerOptions(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            _logger = loggerFactory.CreateLogger<HandlerOptions>();
        }

        public async Task Invoke(HttpContext context)
        {

            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "*");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            context.Response.Headers.Add("Access-Control-Max-Age", "86400");


            if (context.Request.Method.Contains("OPTIONS"))
            {
                await context.Response.WriteAsync("Ok", Encoding.UTF8).ConfigureAwait(true);
            }
            else
            {      
                await _next.Invoke(context);
            }
        }



        //protected override Task<HttpResponseMessage> SendAsync(
        //HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    // Create the response.
        //    var response = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new StringContent("Hello!")
        //    };

        //    _logger.LogInformation("Handling OPTIONS XXXX METHOD: " + request.Method.ToString());

        //    // Note: TaskCompletionSource creates a task that does not contain a delegate.
        //    var tsc = new TaskCompletionSource<HttpResponseMessage>();
        //    tsc.SetResult(response);   // Also sets the task state to "RanToCompletion"
        //    return tsc.Task;
        //}


        private enum ResponseAction
        {
            UseOriginal,
            RetrieveMethods,
            ReturnUnauthorized
        }

        //private static ResponseAction GetResponseAction(
        //    HttpRequestMessage request, HttpResponseMessage response)
        //{
        //    if (request.Method != HttpMethod.Options)
        //        return ResponseAction.UseOriginal;

        //    if (response.StatusCode != HttpStatusCode.MethodNotAllowed)
        //        return ResponseAction.UseOriginal;

        //    return ResponseAction.RetrieveMethods;
        //}
    }
}
