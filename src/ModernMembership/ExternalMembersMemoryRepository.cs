using System;
using System.Collections.Generic;
using System.Linq;
using CavemanTools.Model;

namespace ModernMembership
{
    public class ExternalMembersMemoryRepository:IExternalMembersRepository
    {
        List<ExternalMember> _users=new List<ExternalMember>();
        private object _sync = new Object();

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateMemberIdException"></exception>
        /// <exception cref="DuplicateExternalIdException"></exception>
        /// <param name="member"></param>
        public void Add(ExternalMember member)
        {
            lock (_sync)
            {
               
                foreach (var u in _users)
                {
                    if (u.Id == member.Id) throw new DuplicateMemberIdException();
                    if (u.ExternalId == member.ExternalId) throw new DuplicateExternalIdException();
                }
                
                _users.Add(member);
            }            
        }

        public void Save(ExternalMember member)
        {
            lock (_sync)
            {
                var user = GetMember(member.Id);
                if (user != null)
                {
                    _users.Remove(user);
                    _users.Add(member);
                }
            }
        }

        public ExternalMember GetMember(Guid id)
        {
            return _users.Find(d => d.Id == id);
        }

        public ExternalMember GetMember(ExternalMemberId id)
        {
            return _users.Find(d => d.ExternalId.Equals(id));
        }

        public PagedResult<ExternalMember> GetMembers(long skip, int take, ScopeId scope = null)
        {
            var result = new PagedResult<ExternalMember>();
            IEnumerable<ExternalMember> items = _users;
            if (scope != null)
            {
                items = items.Where(d => d.Scope.Equals(scope));
            }
            result.Count = items.Count();
            result.Items = items.Skip((int) skip).Take(take).ToArray();
            return result;
        }

        public void Delete(params Guid[] ids)
        {
            lock (_sync)
            {
                _users.RemoveAll(d => ids.Contains(d.Id));
            }
        }

        public IDictionary<MemberStatus, int> GetStats()
        {
            return _users.GroupBy(d => d.Status)
                         .ToDictionary(d => d.Key, d => d.Count());

        }
    }
}