using System;
using DomainBus.Concepts;

namespace ModernMembership.Events
{
    public class MemberEvent:AbstractEvent
    {
        public Guid MemberId;

        public MemberEvent(Guid id)
        {
            MemberId = id;
        }
    }
}