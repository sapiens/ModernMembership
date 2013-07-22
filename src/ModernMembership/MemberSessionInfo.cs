using System;

namespace ModernMembership
{
    public class MemberSessionInfo
    {
        private readonly Guid _id;
        private readonly string _displayName;

        public MemberSessionInfo(Guid id,MemberSessionInfoStatus state,MemberStatus memberStatus,ScopeId scope)
        {
            Status = state;
            _id = id;
            MemberStatus = memberStatus;
            Scope = scope;
        }

        /// <summary>
        /// COnstructor for member authenticated
        /// </summary>
        /// <param name="id"></param>
        /// <param name="displayName"></param>
        public MemberSessionInfo(Guid id,string displayName,ScopeId scope)
        {
            Scope = scope;
            _id = id;
            _displayName = displayName;
            MemberStatus=MemberStatus.Active;
        }

        public Guid Id
        {
            get { return _id; }
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public MemberSessionInfoStatus Status { get; private set; }
        public MemberStatus MemberStatus { get; private set; }
        public ScopeId Scope { get; private set; }
    }

    public enum MemberSessionInfoStatus
    {
        Authenticated,
        PasswordFailed,
        MemberInactive
    }


}