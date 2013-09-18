using System;
using System.Collections.Generic;

namespace ModernMembership.Authorization
{
    public interface IUserRightsService
    {
        IEnumerable<ScopedRights> GetRights(Guid userId);        
    }

   
}