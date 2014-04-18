using System;
using System.Security.Principal;
using System.Web;
using CavemanTools;


namespace ModernMembership.Web
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the user identity
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static MemberSessionPrincipal GetMemberSession(this IPrincipal user)
        {
            if (user.Identity.AuthenticationType == MemberSessionPrincipal.Type)
            {
                return user.Identity as MemberSessionPrincipal;
            }
            throw new NotSupportedException("Only ModernMembership identity is supported");
        }

        /// <summary>
        /// Checks is the current user has any of the specified rights
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rights"></param>
        /// <returns></returns>
        public static bool HasRight(this IPrincipal user, params int[] rights)
        {
            var session = user.GetMemberSession();
            return session.HasRight(rights);
        }

        /// <summary>
        /// Sets the scope for authorization purposes
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="scopeId"></param>
        public static void SetAuthScope(this IPrincipal principal,ScopeId scopeId)
        {
            if (scopeId == null) throw new ArgumentNullException("scopeId");
            var usr = principal.GetMemberSession();
            usr.Scope = scopeId;
        }

        /// <summary>
        ///  Sets authenticated cookie
        ///  </summary>
        /// <param name="response"></param>
        /// <param name="id">Session id</param>
        /// <param name="valability">Null means browser session</param>
        public static void SetSessionCookie(this HttpResponseBase response, SessionId id, TimeSpan? valability = null)
        {
            MemberSessionModule.SetAuthCookie(response.Cookies,id,valability: valability);
        }

        /// <summary>
        /// Clears the login cookie.
        /// </summary>
        /// <param name="response"></param>
        public static void Logout(this HttpResponseBase response)
        {
            MemberSessionModule.DestroyAuthCookie(response.Cookies);
        }
            
    }
}