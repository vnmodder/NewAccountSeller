namespace AccountSeller.Infrastructure.HttpClientHelper
{
    public class HttpClientRequestIdDelegatingHandler
       : DelegatingHandler
    {
        private const string X_REQUEST_ID = "x-requestid";

        public HttpClientRequestIdDelegatingHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request != null && (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put) && !request.Headers.Contains(X_REQUEST_ID))
            {
                request.Headers.Add(X_REQUEST_ID, Guid.NewGuid().ToString());
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
