using System;

namespace ModernMembership.Authorization
{
    public interface IUserRightsService
    {
        UserRights GetRights(Guid userId,ScopeId scope);
    }
}