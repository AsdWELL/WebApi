using Microsoft.AspNetCore.Mvc.Filters;

namespace SportNewsWebApi.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;
            var httpContext = context.HttpContext;

            httpContext.Response.StatusCode = 500;

            httpContext.Response.WriteAsJsonAsync(new
            {
                ActionName = context.ActionDescriptor.DisplayName,
                exception.Message,
                exception.StackTrace
            });

            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }
    }
}
