using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Interface.Interfaces;
using DPD.HumanResources.Utilities.Utils;
using Microsoft.IdentityModel.Tokens;

namespace DPD.HR.Infrastructure.WebApi.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private readonly IUserRepository _userRepository;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger,
            IUserRepository userRepository)
        {
            _next = next;
            _logger = logger;
            _userRepository = userRepository;
        }

        /// <summary>
        /// this function to handle request and respond in backend system
        /// </summary>
        /// <param name="httpContext"></param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                //getting jwt from request
                var jwt = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                //extracting email value from token
                var userEmail = ValidateToken(jwt ?? "");

                //getting user by email from database
                var user = await _userRepository.UserByEmail(userEmail ?? "");
                // if it's match by condition and then add value id user and name user to request
                if (user?.Id is not null && user.Id != Guid.Empty)
                {
                    httpContext.Items["UserId"] = user.Id;
                    httpContext.Items["UserName"] = user.Fullname;
                }

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// this function to validate token and then extracting value from token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static string? ValidateToken(string token)
        {
            if (token == "")
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.SecretKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userEmail = jwtToken.Claims.First(x => x.Type == "email").Value;

                return userEmail;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// handling request or respond if it's may occur the error and logging to the file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errorResponse = new TemplateApi<UserDto>(null, null, "Đã xảy ra lỗi !", false, 0, 0, 0, 0);

            _logger.LogError("Đã xảy ra lỗi - {exception}", exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}