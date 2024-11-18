using Microsoft.AspNetCore.Mvc;

namespace DPD.HR.Infrastructure.WebApi.Permission
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(string permission)
        : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { permission };
        }
    }
}
