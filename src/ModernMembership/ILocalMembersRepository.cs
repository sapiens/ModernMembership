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

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateEmailException"></exception>
        /// <param name="member"></param>
        void Save(LocalMember member);

        LocalMember GetMember(Guid id);
        LocalMember GetMember(Email email);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="scope">Use ScopeId.None for global scope</param>
        /// <returns></returns>
        LocalMember GetMember(LoginName id, ScopeId scope);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="scope">Use ScopeId.None for global scope</param>
        /// <returns></returns>
        PagedResult<LocalMember> GetMembers(long skip,int take,ScopeId scope);

        void Delete(params Guid[] ids);
    }
    
}