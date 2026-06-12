namespace SM.Application.Behaviors;

public interface ICacheableQuery
{
    TimeSpan? CacheDuration => null;
}
