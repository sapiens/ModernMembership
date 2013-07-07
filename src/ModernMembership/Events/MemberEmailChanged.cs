using System;

namespace ModernMembership.Events
{
    public class MemberEmailChanged:MemberEvent
    {
        public string Email;
        public MemberEmailChanged(Guid id,string email) : base(id)
        {
            Email = email;
        }
    }
}