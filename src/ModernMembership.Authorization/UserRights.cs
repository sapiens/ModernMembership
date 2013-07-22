using System;
using System.Collections.Generic;

namespace ModernMembership.Authorization
{
    public interface IUserRights
    {
        Guid UserId { get; }
        bool HasRight(short right);
        ScopeId Scope { get; }
    }

    public class UserRights : IUserRights
    {
        public Guid UserId { get; private set; }
        public bool HasRight(short right)
        {
            throw new NotImplementedException();            
        }

        public ScopeId Scope { get; private set; }
    }
}