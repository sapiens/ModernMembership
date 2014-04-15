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
        /// <param name="scope">If null scope is ignored</param>
        /// <returns></returns>
        MemberSessionInfo Authenticate(string loginId, string pwd, ScopeId scope = null);
    }
}