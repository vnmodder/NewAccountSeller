﻿namespace AccountSeller.Infrastructure.HttpClientHelper
{
    public static class HttpClientUtils
    {
        private const string TimeoutPropertyKey = "RequestTimeout";

        public static void SetTimeout(this HttpRequestMessage request, TimeSpan? timeout)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Options.TryAdd(TimeoutPropertyKey, timeout);
        }

        public static TimeSpan? GetTimeout(this HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Options.TryGetValue<TimeSpan>(new HttpRequestOptionsKey<TimeSpan>(TimeoutPropertyKey), out var value) && value is TimeSpan timeout)
            {

                return timeout;
            }

            return null;
        }
    }
}
