using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeManager
{
    public static class Extension
    {
        public static DateTime FromUnixTimestamp(this double timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
        }

        public static DateTime FromUnixTimestamp(this long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(timestamp);
        }

        public static long ToUnixTimestamp(this DateTime date)
        {
            return (long)(date - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;
        }

        public static T SingleOrNew<T>(this IEnumerable<T> query) where T : new()
        {
            try
            {
                return query.Single();
            }
            catch (InvalidOperationException)
            {
                return new T();
            }
        }

    }
}