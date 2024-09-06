using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestTaskMixvel.Filters
{
    /// <summary>
    /// Application exception filter.
    /// </summary>
    public class AppExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<AppExceptionFilter> _logger;

        /// <summary>
        /// Application exception filter constructor.
        /// </summary>
        /// <param name="logger">Class loger.</param>
        public AppExceptionFilter(ILogger<AppExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handle exception method.
        /// </summary>
        /// <param name="context">Exception context.</param>
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            using var streamReader = new StreamReader(context.HttpContext.Request.Body);
            var requestBody = await streamReader.ReadToEndAsync();
            _logger.LogError($@"{nameof(OnExceptionAsync)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. {nameof(context.HttpContext.Request.Body)}={requestBody}, {
                nameof(context.Exception.Message)}={context.Exception.Message}, {nameof(context.Exception.StackTrace)}={context.Exception.StackTrace},.");

            context.Result = new JsonResult(null)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}
