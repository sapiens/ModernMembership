using System;
using System.Collections.Generic;
using CavemanTools.Model;

namespace ModernMembership.Authorization
{
    public interface IUserGroupsRepository
    {
        void Add(UserGroup group);
        UserGroup Get(Guid groupId);
        void Save(params UserGroup[] group);
        void Delete(Guid grpId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="scope">Use ScopeId.None for global scope</param>
        /// <returns></returns>
        PagedResult<UserGroup> GetPaged(int skip, int take, ScopeId scope);

        IEnumerable<UserGroup> GetGroups(IEnumerable<Guid> ids);
        IEnumerable<UserGroup> GetGroupsForUser(Guid userId);
    }

   
}