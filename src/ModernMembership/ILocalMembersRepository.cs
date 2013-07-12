using System;
using CavemanTools.Model;
using CavemanTools.Model.ValueObjects;

namespace ModernMembership
{
    public interface ILocalMembersRepository
    {
        /// <summary>
        /// </summary>
        /// <exception cref="DuplicateMemberIdException"></exception>
        /// <exception cref="DuplicateLoginIdException"></exception>
        /// <exception cref="DuplicateEmailException"></exception>
        /// <param name="member"></param>
        void Add(LocalMember member);

        LocalMember GetMember(Guid id);
        LocalMember GetMember(Email email);
        LocalMember GetMember(LoginName id);

        PagedResult<LocalMember> GetMembers(long skip,int take);
    }
    
}