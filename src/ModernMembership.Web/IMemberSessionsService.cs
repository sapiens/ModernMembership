using System;
using System.Security.Principal;

namespace ModernMembership.Web
{
    public interface IMemberSessionsService
    {
        /// <summary>
        /// If the guid is invalid (session expired or not found), it returns null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IIdentity Get(Guid id);

        /// <summary>
        /// Returns the login session id
        /// </summary>
        /// <param name="data"></param>
        /// <param name="valability">Duration of the session</param>
        /// <returns></returns>
     //   Guid StartSession(AuthenticationData data, TimeSpan valability);
        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="id"></param>
        void EndSession(Guid id);
    }
}