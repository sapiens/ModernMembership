using System;
using CavemanTools.Model;

namespace ModernMembership.Authorization
{
    public interface IUserGroupsRepository
    {
        //void AddUsersToGroup(Guid grpId, params Guid[] userIds);
        //void MakeUserPartOfGroups(Guid userId, params Guid[] groupIds);

        //void RemoveUserFromGroup(Guid userId, Guid grpId);
        UserGroup Get(Guid groupId);
        void Save(params UserGroup[] group);
        void Delete(Guid grpId);
    }

    public interface IQueryUserGroups
    {
        //PagedResult<UserGroup> 
    }
}