using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Linq;

namespace CAM.WebAPI.Extensions
{
    public static class HttpContextAccessorExtensionMethods
    {
        public static string[] GetUserLanguages(this IHttpContextAccessor accessor)
        {
            return accessor.HttpContext.Request.HttpContext.Request.GetTypedHeaders()
                .AcceptLanguage
                ?.OrderByDescending(x => x.Quality ?? 1)
                .Select(x => x.Value.ToString())
                .ToArray() ?? Array.Empty<string>();
        }

        /// <summary>
        /// Extension method to get the CultureInfo from the current user making the request.
        /// </summary>
        /// <param name="accessor">IHttpContextAccessor of the request.</param>
        /// <returns>The current user's CultureInfo. If an error occurs it returns the default en-Us CultureInfo.</returns>
        public static CultureInfo GetUserCulture(this IHttpContextAccessor accessor)
        {
            try
            {
                var cultureString = accessor.HttpContext.Request.HttpContext.Request.GetTypedHeaders()
                        .AcceptLanguage?.OrderByDescending(x => x.Quality ?? 1)
                        .Select(x => x.Value.ToString()).First();

                return new CultureInfo(cultureString);
            }
            catch (Exception)
            {
                return new CultureInfo("en-Us");
            }
        }
    }
}
