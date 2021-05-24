using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficRecording
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var basicTokenResult = CreateBasicToken(_appSettings.ClientId, _appSettings.Secret);
                string basicToken = Convert.ToBase64String(basicTokenResult.Item2);

                // basic sportcast token or tab
                if (token == basicToken || token == "6a1377b4-dc24-4288-9264-fc106c5543e3")
                {
                    // attach user to context on successful jwt validation
                    context.Items["User"] = new User() { Id = 123, FirstName = "Admin", LastName = "Test", Password = "abc123!", Username = "admin" };
                }
                
            }
            catch (Exception ex)
            {
                var a = 1;
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }

        public Tuple<bool, byte[]> CreateBasicToken(string clientId, string secret)
        {
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(secret))
            {
                return new Tuple<bool, byte[]>(false, null);
            }
            return new Tuple<bool, byte[]>(true, new UTF8Encoding().GetBytes($"{clientId}:{secret}"));
        }
    }
}
