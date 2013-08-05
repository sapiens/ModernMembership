using System;
using CavemanTools.Model;

namespace ModernMembership
{
    public interface IExternalMembersRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateMemberIdException"></exception>
        /// <exception cref="DuplicateExternalIdException"></exception>
        /// <param name="member"></param>
        void Add(ExternalMember member);

        void Save(ExternalMember member);
        ExternalMember GetMember(ExternalMemberId id);

        PagedResult<ExternalMember> GetMembers(int skip, int take);
        void Delete(params Guid[] ids);
    }
}