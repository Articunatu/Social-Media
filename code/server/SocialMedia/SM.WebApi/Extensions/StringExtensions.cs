namespace SM.WebApi.Extensions;

public static class StringExtensions
{
    public static Guid ConvertToGuid(this string stringId)
    {
        if (Guid.TryParse(stringId, out var guidId))
            return guidId;
        throw new ArgumentException();
    }
}
