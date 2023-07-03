using FluentValidation.Results;

namespace Common.Behaviour;

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionHandlerMiddleware(RequestDelegate next) => this.next = next;

    public async Task Invoke(HttpContext context /* other dependencies */)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Log issues and handle exception response
        if (exception.GetType() != typeof(FluentValidation.ValidationException))
        {
            var result = JsonSerializer.Serialize(new { isSuccess = false, error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(result);
        }

        var errors = ((FluentValidation.ValidationException)exception).Errors;
        var validationFailures = errors as ValidationFailure[] ?? errors.ToArray();
        if (validationFailures.Any())
        {
            var failures = validationFailures.Select(x => new
            {
                Property = x.PropertyName.Replace("Data.", string.Empty),
                Error = x.ErrorMessage.Replace("Data ", string.Empty)
            });
            var result = Result.Failure(failures.ToList<object>());
            var resultJson = JsonSerializer.Serialize(result);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(resultJson);
        }
        else
        {
            var result = Result.Failure(exception?.Message ?? string.Empty);
            var resultJson = JsonSerializer.Serialize(result);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(resultJson);
        }
    }
}