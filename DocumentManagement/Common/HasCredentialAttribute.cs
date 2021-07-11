using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Common
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            List<string> privilegeLevels = (List<string>)HttpContext.Current.Session[CommonConstants.CREDENTIALS_SESSION];

            if (privilegeLevels.Contains(this.RoleID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Areas/Admin/Views/Shared/Unauthorized.cshtml"
            };
        }
    }
}