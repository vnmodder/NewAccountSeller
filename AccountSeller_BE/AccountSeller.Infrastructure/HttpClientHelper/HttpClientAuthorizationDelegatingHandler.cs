using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace AccountSeller.Infrastructure.HttpClientHelper
{
    public class HttpClientAuthorizationDelegatingHandler
        : DelegatingHandler
    {
        private const string AUTHORIZATION = "Authorization";
        private const string ACCESS_TOKEN = "access_token";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers[AUTHORIZATION];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request?.Headers.Add(AUTHORIZATION, new List<string>() { authorizationHeader });
            }

            var token = await GetTokenAsync().ConfigureAwait(false);

            if (token != null && request != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private async Task<string> GetTokenAsync()
        {

            return await _httpContextAccessor.HttpContext.GetTokenAsync(ACCESS_TOKEN).ConfigureAwait(false);
        }
    }
}
