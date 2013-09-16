using System;
using CavemanTools.Model;

namespace ModernMembership
{
    public interface IExternalMembersRepository:IMembershipStats
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateMemberIdException"></exception>
        /// <exception cref="DuplicateExternalIdException"></exception>
        /// <param name="member"></param>
        void Add(ExternalMember member);

        void Save(ExternalMember member);

        ExternalMember GetMember(Guid id);
        ExternalMember GetMember(ExternalMemberId id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="scope">Use null to ignore scope</param>
        /// <returns></returns>
        PagedResult<ExternalMember> GetMembers(long skip, int take, ScopeId scope = null);
        void Delete(params Guid[] ids);

     
    }
}