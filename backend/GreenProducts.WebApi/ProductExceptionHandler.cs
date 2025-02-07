using GreenProducts.Core.Products;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GreenProducts.WebApi;

/// <summary>
/// Maps domain exceptions to problem details.
/// This enabled using exceptions as a way of communicating and mapping domain errors to responses.
/// If I was to do a bigger service, I would use some results-based implementation instead of exceptions.
/// </summary>
/// <param name="problemDetailsService">Service for writing problem details to the output stream.</param>
public class ProductExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int? statusCode = exception switch
        {
            ProductValidationException => StatusCodes.Status400BadRequest,
            ProductNotFoundException => StatusCodes.Status404NotFound,
            BadHttpRequestException => StatusCodes.Status400BadRequest,
            _ => null
        };

        // Fallback to default handling if the error is not an exception we can deal with
        if (!statusCode.HasValue) return false;

        var details = string.IsNullOrWhiteSpace(exception.InnerException?.Message)
            ? exception.Message
            : $"{exception.Message} {exception.InnerException.Message}";

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = "An error occurred",
            Type = exception.GetType().Name,
            Detail = details
        };
        httpContext.Response.StatusCode = problemDetails.Status.Value;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            Exception = exception,
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}