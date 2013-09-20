using System;
using System.Collections.Generic;

namespace ModernMembership.Authorization
{
    public interface IUserGroupsRepository
    {
        /// <summary>
        /// Duplicate id should be ignored. Empty groups are not saved
        /// </summary>
        /// <param name="group"></param>
        void Add(UserGroup group);
        UserGroup GetUserGroup(Guid groupId);
        void Save(params UserGroup[] group);
        void Delete(Guid grpId);
       
        //IEnumerable<UserGroup> GetUserGroups(IEnumerable<Guid> ids);
        IEnumerable<UserGroup> GetGroupsForUser(Guid userId);
        IEnumerable<RightsGroup> GetRightsGroupsForUser(Guid userId);
    }
}