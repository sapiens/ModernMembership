using System;
using System.Collections.Generic;
using CavemanTools.Model;

namespace ModernMembership.Authorization
{
    public interface IRightsGroupsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateGroupException">If name and scope match</exception>
        /// <param name="group"></param>
        void Add(RightsGroup group);
        ///<summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateGroupException"></exception>
        void Save(RightsGroup group);
        RightsGroup GetRightsGroup(Guid id);
        IEnumerable<RightsGroup> GetRightsGroups(IEnumerable<Guid> ids);

        void Delete(Guid id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="scope">Null to ignore scope</param>
        /// <returns></returns>
        PagedResult<RightsGroup> GetPaged(long skip,int take,ScopeId scope=null);
    }
}