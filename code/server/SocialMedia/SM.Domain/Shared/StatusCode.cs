namespace SM.Domain.Shared;

public enum StatusCode
{
    Ok = 200,
    NoContent = 204,
    Validation = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    Unexpected = 500
}
