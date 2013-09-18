using System;

namespace ModernMembership.Authorization
{
    public class DuplicateGroupException:Exception
    {
        public DuplicateGroupException():base("A group name must be unique in scope")
        {
            
        }
    }
}