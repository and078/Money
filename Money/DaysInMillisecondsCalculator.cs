using System;
using System.Collections.Generic;
using System.Text;

namespace Money
{
    internal class DaysInMillisecondsCalculator
    {
        private const long firstDay = 82800000; //23:00 1.01.1970
        private const long msPerDay = 86400000;
        public static long FirstDay { get; } = 82800000;
        public static long MsPerDay { get; } = 86400000;
        private static long DateNowInMilliseconds ()
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime now = DateTime.UtcNow;

            long timestamp = (long)(now.Subtract(epoch).TotalMilliseconds);
            return timestamp;
        }
        //let today = (Math.trunc((Date.now() - someDate) / msPerDay)) * msPerDay + someDate (JS)
        public static long today = (long)((float)(DateNowInMilliseconds() - firstDay) / msPerDay) * msPerDay + firstDay;

        public static long weekAgo = today - (msPerDay * 6);
    }
}
