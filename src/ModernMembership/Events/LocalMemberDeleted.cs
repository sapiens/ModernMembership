using System;

namespace ModernMembership.Events
{
    public class LocalMemberDeleted:MemberEvent
    {
        public LocalMemberDeleted(Guid id) : base(id)
        {
        }
    }
}