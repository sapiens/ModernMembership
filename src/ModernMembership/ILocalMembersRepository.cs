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
        /// <exception cref="DuplicateLoginNameException"></exception>
        /// <exception cref="DuplicateEmailException"></exception>
        /// <param name="member"></param>
        void Add(LocalMember member);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateLoginNameException"></exception>
        /// <exception cref="DuplicateEmailException"></exception>
        /// <param name="member"></param>
        void Save(LocalMember member);

        LocalMember GetMember(Guid id);
        LocalMember GetMember(Email email);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="scope">Use ScopeId.Global for global scope. Null to ignore the scope</param>
        /// <returns></returns>
        LocalMember GetMember(LoginName id, ScopeId scope=null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="scope">Use ScopeId.Global for global scope.Null to ignore scope</param>
        /// <returns></returns>
        PagedResult<LocalMember> GetMembers(long skip,int take,ScopeId scope=null);

        void Delete(params Guid[] ids);
    }
    
}