using DotNetInterview.Application.Items.Exceptions;
using DotNetInterview.Domain.Exceptions;

namespace DotNetInterview.API;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ItemNotFoundException exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            var response = new
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = exception.Message,
                Status = context.Response.StatusCode,
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (PriceOutOfRangeException exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var response = new
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = exception.Message,
                Status = context.Response.StatusCode,
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (QuantityOutOfRangeException exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var response = new
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = exception.Message,
                Status = context.Response.StatusCode,
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (VariationAlreadyExistsException exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var response = new
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = exception.Message,
                Status = context.Response.StatusCode,
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = new
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Title = "Internal Server Error",
                Status = context.Response.StatusCode,
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
