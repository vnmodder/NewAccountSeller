namespace AccountSeller.Infrastructure
{
    public static class SettingConstants
    {
        public static class SettingKey
        {
            public const string EVENT_TYPE = "EventType";
        }

        public static class DatabaseSettings
        {
            /// <summary>
            /// Default database command time out from seconds.
            /// <br></br>
            /// Unit: second.
            /// </summary>
            public const int TIMEOUT_FROM_SECONDS = 120;

            /// <summary>
            /// Default max attempts to login failed with an account.
            /// <br></br>
            /// Unit: attempts times.
            /// </summary>
            public const int MAX_ATTEMPTS_WRONG_PASSWORD = 10;

            public const int MAX_YEAR_LOCK_ACCOUNT = 10;
        }

        public static class HTTPSettings
        {
            /// <summary>
            /// Max HTTP request time out from seconds.
            /// <br></br>
            /// Unit: second.
            /// </summary>
            public const int MAX_HTTP_REQUEST_TIME_OUT = 30;
        }
    }
}