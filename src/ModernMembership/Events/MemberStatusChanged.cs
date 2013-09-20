using System;

namespace ModernMembership.Events
{
    public class MemberStatusChanged:MemberEvent
    {
        public MemberStatus Status { get; set; }
        public bool IsLocal { get; set; }

        public MemberStatusChanged(Guid id,MemberStatus status,bool local=true) : base(id)
        {
            Status = status;
            IsLocal = local;
        }
    }
}