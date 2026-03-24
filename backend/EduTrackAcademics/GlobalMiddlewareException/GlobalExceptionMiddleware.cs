using System.Net;
using System.Text.Json;
using EduTrackAcademics.Exceptions;

namespace EduTrackAcademics.Aspects
{
	public class GlobalExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<GlobalExceptionMiddleware> _logger;

		public GlobalExceptionMiddleware(RequestDelegate next,
			ILogger<GlobalExceptionMiddleware> logger)
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
			catch (BaseAppException ex)
			{
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = ex.StatusCode;

				var response = new
				{
					success = false,
					message = ex.Message,
					statusCode = ex.StatusCode
				};

				await context.Response.WriteAsync(
					JsonSerializer.Serialize(response));
			}
			catch (System.Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception");

				context.Response.ContentType = "application/json";
				context.Response.StatusCode =
					(int)HttpStatusCode.InternalServerError;

				var response = new
				{
					success = false,
					message = "Internal Server Error",
					statusCode = 500
				};

				await context.Response.WriteAsync(
					JsonSerializer.Serialize(response));
			}
		}
	}
}