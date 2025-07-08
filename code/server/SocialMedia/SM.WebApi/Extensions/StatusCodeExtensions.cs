using SM.Domain.Shared;
using System.Text.RegularExpressions;

namespace SM.WebApi.Extensions;

public static class StatusCodeExtensions
{
    public static string ToTitle(this StatusCode status)
    {
        return Regex.Replace(status.ToString(), "(\\B[A-Z])", " $1");
    }
}
