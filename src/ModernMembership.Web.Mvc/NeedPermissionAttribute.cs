using System;
using System.Linq;
using System.Web.Mvc;

namespace ModernMembership.Web.Mvc
{
    /// <summary>
    /// Modern Membership only
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple = true)]
    public class NeedPermissionAttribute:FilterAttribute,IAuthorizationFilter
    {
        private int[] _rights;

       
        /// <summary>
        /// The authorization succeeds if user has any of the specified rights
        /// </summary>
        /// <param name="rights"></param>
        public NeedPermissionAttribute(params int[] rights)
        {
            if (rights.Length==0) throw new InvalidOperationException("At least a right needs to be specified!");
            _rights = rights;
        }

      public void OnAuthorization(AuthorizationContext filterContext)
      {
          var dt = filterContext.HttpContext.User.GetMemberSession();
          
          if (!dt.HasRight(_rights))
            {
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    filterContext.Result = new HttpStatusCodeResult(403, "You are not authorized to view this page");    
                }
                
            }
           
        }
    }
}