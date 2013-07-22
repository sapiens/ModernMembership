using System;
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
        PagedResult<UserGroup> GetAll(int skip, int take, ScopeId scope);
    }

   
}