using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusteriKazan.Web.Attributes
{
    public class LoginControl : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var user = filterContext.HttpContext.Session["KullaniciAdi"];
                if (user == null)
                    filterContext.Result = new RedirectResult(string.Format("~/Home/Index", filterContext.HttpContext.Request.Url.AbsolutePath));
                var token = filterContext.HttpContext.Session["AccessToken"];
                if (token == null)
                    filterContext.Result = new RedirectResult(string.Format("~/Home/Index", filterContext.HttpContext.Request.Url.AbsolutePath));
                DateTime tokenExpireDate = (DateTime)filterContext.HttpContext.Session["TokenEndDate"];
                if(tokenExpireDate < DateTime.Now)
                {
                    filterContext.Result = new RedirectResult(string.Format("~/Home/Index", filterContext.HttpContext.Request.Url.AbsolutePath));
                }

            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult(string.Format("~/Home/Index", filterContext.HttpContext.Request.Url.AbsolutePath));
            }
            
        }
    }
}