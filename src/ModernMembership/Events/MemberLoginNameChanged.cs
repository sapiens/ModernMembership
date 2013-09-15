using System;

namespace ModernMembership.Events
{
    public class MemberLoginNameChanged:MemberEvent
    {
        public string NewName { get; set; }

        public MemberLoginNameChanged(Guid id,string name) : base(id)
        {
            NewName = name;
        }
    }
}