using System.Security.Principal;
using System.Web;
using System;
using System.Web.Configuration;
using CavemanTools;

namespace ModernMembership.Web
{
    public class MemberSessionModule:IHttpModule
    {
        private readonly Func<IMemberSessionsService> _service;

        public static string CookieName = "ModernSession";
        public static string CookiePath = "/";
        public static string CookieDomain = "";
        
        public static string LoginRedirect = "";
        public const string WebConfigLoginRedirectKey = "Membership.LoginPage";
        
        
        public MemberSessionModule(Func<IMemberSessionsService> service)
        {
            _service = service;
            LoginRedirect = WebConfigurationManager.AppSettings[WebConfigLoginRedirectKey];
         }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += context_AuthenticateRequest;
           
            context.EndRequest += context_EndRequest;
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            if (LoginRedirect.IsNullOrEmpty()) return;
            var context = sender.As<HttpApplication>().Context;
            if (context.Response.StatusCode != 401)
                return;
            context.Response.Redirect(LoginRedirect, false);
        }

        void context_AuthenticateRequest(object sender, System.EventArgs e)
        {
            var ctx = sender.As<HttpApplication>().Context;

            var cookie = ctx.Request.Cookies[CookieName];

            var id = GetSessionId(cookie);

            if (id != null) //we found session id in cookie
            {
                var session = _service().Get(id.Value);
                
                if (session != null)//there is a non expired session
                {
                    if (UpdateCookieValability(session.Info, cookie))
                    {
                        SetCookie(ctx.Response.Cookies,cookie);
                    }

                    ctx.User=new GenericPrincipal(session,new string[0]);
                    return;
                }
                else
                {
                    DestroyAuthCookie(ctx.Response.Cookies);
                }
            }
            //anonymous user
            ctx.User = new GenericPrincipal(new MemberSessionPrincipal(), new string[0]);
        }

        /// <summary>
        /// Returns true if cookie was updated
        /// </summary>
        /// <param name="session"></param>
        /// <param name="ck"></param>
        /// <returns></returns>
        static bool UpdateCookieValability(SessionData session,HttpCookie ck)
        {
           if (session.IsSliding && session.ExpiresOn>ck.Expires)
            {
                ck.Expires = session.ExpiresOn;
                return true;
            }

            return false;
        }

        static SessionId? GetSessionId(HttpCookie ck)
        {
            if (ck != null)
            {
                SessionId result;
                if (SessionId.TryParse(ck.Value, out result))
                {
                    return result;
                }
            }
            return null;
        }

       
        static void SetCookie(HttpCookieCollection cookies, HttpCookie ck)
        {
            ck.Path = CookiePath;
            if (!CookieDomain.IsNullOrEmpty())
            {
                ck.Domain = CookieDomain;
            }
            cookies.Attach(ck);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookies">Response cookies</param>
        /// <param name="id">Session id</param>
        /// <param name="valability">Null means browser session</param>
        public static void SetAuthCookie(HttpCookieCollection cookies, SessionId id, TimeSpan? valability = null)
        {
            var ck = new HttpCookie(CookieName, id.ToString());

            if (valability != null)
            {
                ck.Expires = DateTime.UtcNow.Add(valability.Value);
            }
            SetCookie(cookies, ck);
        }


        public static void DestroyAuthCookie(HttpCookieCollection resp)
        {
            var ck = new HttpCookie(CookieName, "");
            ck.Expires = new DateTime(2012, 1, 1);
            SetCookie(resp, ck);
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}