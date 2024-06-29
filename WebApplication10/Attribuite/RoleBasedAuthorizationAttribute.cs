using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

public class RoleBasedAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _allowedRoles;

    public RoleBasedAuthorizationAttribute(params string[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Get the token from cookies or headers
        string token = context.HttpContext.Request.Cookies["user-access-token"];

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Extract role claims from the decoded token
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            if (string.IsNullOrEmpty(roleClaim))
            {
                context.Result = new ForbidResult();
                return;
            }

            // Check if user's role is allowed
            if (!_allowedRoles.Contains(roleClaim))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
        catch (Exception)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
