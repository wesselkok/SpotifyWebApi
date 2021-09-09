using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace SpotifyApp.Core.Base.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Check if IEnumerable is null or contains no elements
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> input)
        {
            return input == null || !input.Any();
        }

        /// <summary>
        /// Foreach on IEnumerable
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

        /// <summary>
        /// Convert NameValueCollection to QueryString
        /// </summary>
        public static string ToQueryString(this NameValueCollection nvc)
        {
            // Create list of parameters (encoded)
            List<string> parameters = new List<string>();
            nvc.AllKeys.ForEach(x => nvc.GetValues(x).ForEach(y => parameters.Add(string.Format("{0}={1}", HttpUtility.UrlEncode(x), HttpUtility.UrlEncode(y)))));

            // Return
            return "?" + string.Join("&", parameters);
        }
    }
}
