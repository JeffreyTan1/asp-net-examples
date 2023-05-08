using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using AdminWebsite.Models;

namespace AdminWebsite.Filters;

public class AuthorizeCustomerAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if(context.ActionDescriptor.EndpointMetadata.Any(x => x is AllowAnonymousAttribute))
           return;

        var loggedInState = context.HttpContext.Session.GetString("loggedInState");
        if (loggedInState == null)
            context.Result = new RedirectToActionResult("Index", "AdminLogin", null);
    }
}
