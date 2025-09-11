using System.Net;
using System.Text.RegularExpressions;

namespace SM.WebApi.Extensions;

public static class StatusCodeExtensions
{
    public static string ToTitle(this HttpStatusCode status)
    {
        return Regex.Replace(status.ToString(), "(\\B[A-Z])", " $1");
    }
}
