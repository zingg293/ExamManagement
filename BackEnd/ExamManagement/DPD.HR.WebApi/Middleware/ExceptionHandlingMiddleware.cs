using System.Net;
using System.Text.Json;
using DPD.HR.Infrastructure.WebApi.Middleware.Exceptions;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities;
using DPD.HumanResources.Utilities.Utils;
using KeyNotFoundException = DPD.HR.Infrastructure.WebApi.Middleware.Exceptions.KeyNotFoundException;
using NotImplementedException = DPD.HR.Infrastructure.WebApi.Middleware.Exceptions.NotImplementedException;
using UnauthorizedAccessException = DPD.HR.Infrastructure.WebApi.Middleware.Exceptions.UnauthorizedAccessException;

namespace DPD.HR.Infrastructure.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// this function like a global handle exception
        /// if it's have any the error then logging to the file and make a proper request status
        /// </summary>
        /// <param name="httpContext"></param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new TemplateApi<UserDto>(null, null, "Đã xảy ra lỗi !", false, 0, 0, 0, 0);

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(BadRequestException))
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                response.StatusCode = (int)HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(KeyNotFoundException))
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            _logger.LogError("Đã xảy ra lỗi - {exception}", exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
