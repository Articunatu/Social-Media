using SM.WebApi.Endpoints;

namespace SM.WebApi.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void MapApiEndpoints(this WebApplication app)
        {
            app.MapAuthenticationEndpoints();
            app.MapCommentEndpoints();
            app.MapPostEndpoints();
            app.MapReactionEndpoints();
            app.MapUserEndpoints();
        }
    }
}
