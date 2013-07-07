using System;

namespace ModernMembership.Events
{
    public class LocalMemberAdded:MemberEvent
    {
        public string LoginId;
        public string DisplayName;
        public string Email;
        public MemberStatus Status;
        
        public LocalMemberAdded(Guid id) : base(id)
        {
        }
    }
}