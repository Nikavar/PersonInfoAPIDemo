using PersonInfo.Model.Models;
using PersonInfo.Model.Models.Enums;
using PersonInfo.Service.Interfaces;
using PersonInfo.Service.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PersonInfo.Service
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }
        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            var stackTrace = string.Empty;
            string message;

            var exceptionType = exception.GetType();

            if (exceptionType == typeof(NotFoundException))
            {
                message = exception.Message;
                status = HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(AlreadyExistsException))
            {
                status = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = nameof(HttpStatusCode.InternalServerError);
                stackTrace = exception.StackTrace;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = (int)status,
                Message = message
            }.ToString());
        }
    }
}
