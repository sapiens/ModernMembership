using System;
using System.Collections.Generic;
using System.Linq;
using CavemanTools.Model;

namespace ModernMembership.Authorization
{
    public class UserGroupsMemoryRepository:IUserGroupsRepository,IRightsGroupsRepository,IUserRightsService
    {
        List<RightsGroup> _rights=new List<RightsGroup>();
        List<UserGroup> _users=new List<UserGroup>();
        
        private object _syncR = new Object();
        private object _syncU=new Object();

        public void Add(UserGroup @group)
        {
            if (@group.Users.Count()==0) return;
            lock (_syncU)
            {
               if (_users.Any(d=>d.Id==@group.Id)) return;
                _users.Add(@group);
            }
            
        }

        public UserGroup GetUserGroup(Guid groupId)
        {
            return _users.Find(d => d.Id == groupId);
        }

        public void Save(params UserGroup[] @group)
        {
            lock (_syncU)
            {
                _users.RemoveAll(u => @group.Any(g=>u.Id==g.Id));
                _users.AddRange(@group);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateGroupException">If name and scope match</exception>
        /// <param name="grp"></param>
        public void Add(RightsGroup grp)
        {
            lock (_syncR)
            {
                HandleDuplicate(grp);
                _rights.Add(grp);
            }
        }

        private void HandleDuplicate(RightsGroup grp,bool edit=false)
        {
            IEnumerable<RightsGroup> items = _rights;
            if (edit)
            {
                items = items.Where(d => d.Id != grp.Id);
            }
            if (items.Any(d => d.Name.Equals(grp.Name) && d.Scope.Equals(grp.Scope)))
            {
                throw new DuplicateGroupException();
            }
        }


        ///<summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateGroupException"></exception>
        public void Save(RightsGroup grp)
        {
            lock (_syncR)
            {
                HandleDuplicate(grp,true);
                _rights.RemoveAll(d => d.Id == grp.Id);
                _rights.Add(grp);
            }
        }

        public RightsGroup GetRightsGroup(Guid id)
        {
            return _rights.Find(d => d.Id == id);
        }

        public IEnumerable<RightsGroup> GetRightsGroupsForUser(Guid userId)
        {

            var ids = _users.Where(d => d.Users.Contains(userId)).Select(u => u.Id);
            return _rights.Where(r => ids.Contains(r.Id)).ToArray();
        }

        public IEnumerable<RightsGroup> GetRightsGroups(IEnumerable<Guid> ids)
        {
            return _rights.Where(d => ids.Contains(d.Id)).ToArray();
        }

        public void Delete(Guid id)
        {
            lock (_syncR)
            {
                _rights.RemoveAll(d => d.Id == id);
            }

            lock (_syncU)
            {
                _users.RemoveAll(d => d.Id == id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="scope">Null to ignore scope</param>
        /// <returns></returns>
        public PagedResult<RightsGroup> GetPaged(long skip, int take, ScopeId scope = null)
        {
            IEnumerable<RightsGroup> items = _rights;
            if (scope != null)
            {
                items = items.Where(s => s.Scope.Equals(scope));
            }
            var rezult = new PagedResult<RightsGroup>();
            rezult.Count = items.Count();
            rezult.Items = items.Skip((int)skip).Take(take).ToArray();
            return rezult;
        }

        public IEnumerable<UserGroup> GetGroupsForUser(Guid userId)
        {
            return _users.Where(d => d.Users.Contains(userId)).ToArray();
            
        }

        public IEnumerable<ScopedRights> GetRights(Guid userId)
        {
            var scoped = new Dictionary<ScopeId, List<int>>();
            foreach (var grp in GetRightsGroupsForUser(userId))
            {
                List<int> cRights;
                if (!scoped.TryGetValue(grp.Scope, out cRights))
                {
                    cRights=new List<int>();
                    scoped[grp.Scope] = cRights;
                }
                cRights.AddRange(grp.Rights);
            }

            foreach (var sr in scoped)
            {
                yield return new ScopedRights(sr.Key,sr.Value);
            }
        }
    }
}