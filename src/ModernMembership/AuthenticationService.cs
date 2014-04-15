using CavemanTools;
using CavemanTools.Web;

namespace ModernMembership
{
    public class AuthenticationService:IAuthenticationService
    {
        private readonly ILocalMembersRepository _repository;

        public AuthenticationService(ILocalMembersRepository repository)
        {
            _repository = repository;
        }

        public MemberSessionInfo Authenticate(string loginId, string pwd, ScopeId scope = null)
        {
            if (scope == null) scope = ScopeId.Global;
            var member = _repository.GetMember(new LoginName(loginId),scope);
            if (member == null)
            {
                return null;
            }

            if (!member.Password.IsValidPassword(pwd))
            {
                return new MemberSessionInfo(member.Id,MemberSessionInfoStatus.PasswordFailed,member.Status,scope);
            }

            if (member.Status != MemberStatus.Active)
            {
                return new MemberSessionInfo(member.Id,MemberSessionInfoStatus.MemberInactive,member.Status,scope);
            }

            return new MemberSessionInfo(member.Id,member.DisplayName,scope);
        }
    }
}