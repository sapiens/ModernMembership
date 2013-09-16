using System;
using System.Collections.Generic;
using System.Linq;
using CavemanTools.Model;
using CavemanTools.Model.ValueObjects;

namespace ModernMembership
{
    public class LocalMembersMemoryRepository:ILocalMembersRepository,IDisposable
    {
        Dictionary<Guid,LocalMember> _members=new Dictionary<Guid, LocalMember>();

        private object _sync = new Object();

        /// <summary>
        /// </summary>
        /// <exception cref="DuplicateMemberIdException"></exception>
        /// <exception cref="DuplicateLoginNameException"></exception>
        /// <exception cref="DuplicateEmailException"></exception>
        /// <param name="member"></param>
        public void Add(LocalMember member)
        {
            lock (_sync)
            {
                if (_members.ContainsKey(member.Id))
                {
                    throw new DuplicateMemberIdException();
                }

                HandleDuplicateName(member);

                HandleDuplicateEmail(member);

                _members.Add(member.Id, member);
            }            
        }

        private void HandleDuplicateName(LocalMember member,bool updateMode=false)
        {
            IEnumerable<LocalMember> list = _members.Values;
            if (updateMode)
            {
                list = list.Where(d => d.Id != member.Id);
            }
            if (list.Any(d => d.Name.Equals(member.Name)))
            {
                throw new DuplicateLoginNameException();
            }
        }
        
        private void HandleDuplicateEmail(LocalMember member,bool updateMode=false)
        {
            IEnumerable<LocalMember> list = _members.Values;
            if (updateMode)
            {
                list = list.Where(d => d.Id != member.Id);
            }
            if (list.Any(d => d.Email.Equals(member.Email)))
            {
                throw new DuplicateEmailException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateEmailException"></exception>
        /// <param name="member"></param>
        public void Save(LocalMember member)
        {
            lock (_sync)
            {
                HandleDuplicateName(member,true);
                HandleDuplicateEmail(member,true);
                _members[member.Id] = member;
            }
        }

        public LocalMember GetMember(Guid id)
        {
            LocalMember result=null;
            _members.TryGetValue(id, out result);
            return result;
        }

        public LocalMember GetMember(Email email)
        {
            return _members.Values.Where(m => m.Email.Equals(email)).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="scope">Use ScopeId.Global for global scope</param>
        /// <returns></returns>
        public LocalMember GetMember(LoginName id, ScopeId scope=null)
        {
            var members = _members.Values.Where(d => d.Name.Equals(id));
            if (scope != null)
            {
                members = members.Where(d => d.Scope.Equals(scope));
            }
            return members.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="scope">Use ScopeId.Global for global scope. Null to ignore scope</param>
        /// <returns></returns>
        public PagedResult<LocalMember> GetMembers(long skip, int take, ScopeId scope=null)
        {
            var result = new PagedResult<LocalMember>();
            IEnumerable<LocalMember> items = _members.Values;
            if (scope != null) 
            {
                items = items.Where(d => d.Scope.Equals(scope));
            }
            result.Count = items.Count();
            items=items.Skip((int) skip).Take(take);
            result.Items = items.ToArray();
            return result;
        }

        public void Delete(params Guid[] ids)
        {
            lock (_sync)
            {
                foreach (var id in ids) _members.Remove(id);
            }
        }

        /// <summary>
        /// Deletes local members who didn't activate their account in the specified period
        /// </summary>
        /// <param name="interval"></param>
        public void PurgeUnactivatedMembers(TimeSpan interval)
        {
            lock (_sync)
            {
                var now = DateTime.UtcNow;
                var to_delete =
                    _members.Values.Where(
                        d => d.Status == MemberStatus.NeedsActivation && (now.Subtract(d.RegisteredOn) > interval))
                        .Select(d=>d.Id)
                        .ToArray();
                to_delete.ForEach(d=>_members.Remove(d));
            }
        }

        public IDictionary<MemberStatus, int> GetStats()
        {
            return _members.Values.GroupBy(d => d.Status).ToDictionary(d => d.Key, d => d.Count());
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _members.Clear();
            _members = null;
        }
    }
}