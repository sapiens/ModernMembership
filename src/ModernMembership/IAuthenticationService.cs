using CavemanTools.Web;

namespace ModernMembership
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="pwd"></param>
        /// <param name="hasher">Default is CavemanHashStrategy</param>
        /// <returns></returns>
        MemberSessionInfo Authenticate(string loginId, string pwd, IHashPassword hasher = null);
    }
}