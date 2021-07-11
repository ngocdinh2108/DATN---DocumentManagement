using DocumentManagement.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DocumentManagement.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", area = "Admin" }));
            }
            base.OnActionExecuting(filterContext);
        }

        protected void SetAlert(string message, string type)
        {
            TempData["Message"] = message;
            if(type == "success")
            {
                TempData["Type"] = "success";
            }
            else if (type == "warning")
            {
                TempData["Type"] = "warning";
            }
            else if (type == "danger")
            {
                TempData["Type"] = "danger";
            }
            else
            {
                TempData["Type"] = "info";
            }
        }
    }
}