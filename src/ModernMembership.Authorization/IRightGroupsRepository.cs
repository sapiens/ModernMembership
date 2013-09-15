using System;
using CavemanTools.Model;

namespace ModernMembership.Authorization
{
    public interface IRightGroupsRepository
    {
        void Add(RightsGroup group);
        void Save(RightsGroup group);
        void Delete(Guid id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="scope">Null to ignore scope</param>
        /// <returns></returns>
        PagedResult<RightsGroup> GetGroups(long skip,int take,ScopeId scope=null);
    }
}