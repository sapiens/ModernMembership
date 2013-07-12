using System;

namespace ModernMembership
{
    public class MemberSessionInfo
    {
        private readonly Guid _id;
        private readonly string _displayName;

        public MemberSessionInfo(Guid id,MemberSessionInfoStatus state,MemberStatus memberStatus)
        {
            Status = state;
            _id = id;
            MemberStatus = memberStatus;
        }

        public MemberSessionInfo(Guid id,string displayName)
        {
            _id = id;
            _displayName = displayName;
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

    }

    public enum MemberSessionInfoStatus
    {
        Authenticated,
        PasswordFailed,
        MemberInactive
    }


}