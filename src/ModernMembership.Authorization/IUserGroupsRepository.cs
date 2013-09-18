using System;
using System.Collections.Generic;

namespace ModernMembership.Authorization
{
    public interface IUserGroupsRepository
    {
        void Add(UserGroup group);
        UserGroup GetUserGroup(Guid groupId);
        void Save(params UserGroup[] group);
        void Delete(Guid grpId);
       
        //IEnumerable<UserGroup> GetUserGroups(IEnumerable<Guid> ids);
        IEnumerable<UserGroup> GetGroupsForUser(Guid userId);
        IEnumerable<RightsGroup> GetRightsGroupsForUser(Guid userId);
    }
}