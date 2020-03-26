using System;

namespace WorkTimer.Api.Extensions {
    public static class DateTimeExtensions {

        private static DateTimeOffset UnixTimeStampStart => new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static long ToUnixEpochDate(this DateTime date) {
            return (long)Math.Round((date.ToUniversalTime() - UnixTimeStampStart).TotalSeconds);
        }
    }
}
