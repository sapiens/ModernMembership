using System;

namespace ModernMembership.Events
{
    public class ExternalMemberDeleted : LocalMemberDeleted
    {
        public ExternalMemberDeleted(Guid id) : base(id)
        {
        }
    }
}