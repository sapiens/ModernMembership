using System;

namespace ModernMembership.Events
{
    public class MemberStatusChanged:MemberEvent
    {
        public MemberStatus Status { get; set; }

        public MemberStatusChanged(Guid id,MemberStatus status) : base(id)
        {
            Status = status;
        }
    }
}