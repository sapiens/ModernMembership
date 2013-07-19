using System;
using System.Collections.Generic;

namespace ModernMembership.Authorization
{
    
    public class UserRights
    {
        public Guid UserId { get; private set; }
        public bool HasRight(short right)
        {
            throw new NotImplementedException();            
        }
        
    }
}