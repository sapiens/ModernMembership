using System;

namespace ModernMembership.Events
{
    public class MemberEmailChanged:MemberEvent
    {
        public string NewEmail;
        public MemberEmailChanged(Guid id,string email) : base(id)
        {
            NewEmail = email;
        }
    }
}