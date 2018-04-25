using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace EShop.Attributes
{
    public class CustomAuthorization : AuthorizeAttribute
    {
        public string LoginPage { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
                filterContext.HttpContext.Response.Redirect(LoginPage + "?ReturnUrl=" + returnUrl);
            }
            base.OnAuthorization(filterContext);
        }
    }
}