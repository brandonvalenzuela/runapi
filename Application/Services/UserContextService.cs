using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public int GetUserId()
    {
        // Obtiene el HttpContext actual
        var httpContext = _httpContextAccessor.HttpContext;

        /*if (httpContext == null || httpContext.User?.Identity?.IsAuthenticated != true)
        {
            throw new UnauthorizedAccessException("No hay un usuario autenticado en el contexto.");
        }*/
        if (httpContext == null || httpContext.User?.Identity?.IsAuthenticated != true)
        {
            #if DEBUG
                return 1; // For testing in local dev
            #else
                throw new UnauthorizedAccessException("No hay un usuario autenticado...");
            #endif
        }

        // Buscar un claim "UserId", "sub" o "nameidentifier", dependiendo de cómo guardes el ID
        // Ejemplo: si guardas el ID en el claim "UserId"
        var userIdClaim = httpContext.User.FindFirst("UserId");

        if (userIdClaim == null)
        {
            throw new InvalidOperationException("El claim 'UserId' no está presente en el token o no se encuentra.");
        }

        // Convierte el valor del claim a entero
        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            throw new InvalidOperationException("El valor del claim 'UserId' no es un número válido.");
        }

        return userId;
    }

    public string GetUserName()
    {
        throw new NotImplementedException();
    }

    public string GetUserEmail()
    {
        throw new NotImplementedException();
    }
}