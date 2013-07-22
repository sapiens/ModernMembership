using System;

namespace ModernMembership.Authorization
{
    public interface IUserRightsService
    {
        IUserRights GetRights(Guid userId,ScopeId scope);
    }
}