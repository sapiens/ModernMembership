using System;

namespace ModernMembership.Events
{
    public class MemberNameChanged:MemberEvent
    {
        public string Name;

        public MemberNameChanged(Guid id) : base(id)
        {
        }
    }
}