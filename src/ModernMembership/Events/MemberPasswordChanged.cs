using System;

namespace ModernMembership.Events
{
    public class MemberPasswordChanged:MemberEvent
    {
        public MemberPasswordChanged(Guid id) : base(id)
        {
        }
    }
}