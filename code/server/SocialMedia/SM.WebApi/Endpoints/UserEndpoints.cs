using MediatR;

namespace SM.WebApi.Endpoints;

public class UserEndpoints
{
    public static void Endpoint1(ISender sender, HttpContext http)
    {
        Guid userId = http.User.;
        sender.Send();
    }
}
