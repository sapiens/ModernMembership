using System;
using CavemanTools.Model;

namespace ModernMembership.Authorization
{
    public interface IRightGroupsRepository
    {
        void Add(RightsGroup group);
        void Save(RightsGroup group);
        void Delete(Guid id);

        PagedResult<RightsGroup> GetGroups(long skip,int take,ScopeId id=null);
    }
}