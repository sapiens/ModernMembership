using System;

namespace ModernMembership
{
    public class DuplicateMemberIdException:Exception
    {
        public DuplicateMemberIdException():base("There is already a member wiht that id. Maybe an idempotency issue?")
        {
            
        }
    }
}