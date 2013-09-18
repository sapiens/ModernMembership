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

        public void Add(UserGroup @group)
        {
            throw new NotImplementedException();
            
        }

        public UserGroup GetUserGroup(Guid groupId)
        {
            throw new NotImplementedException();
        }

        public void Save(params UserGroup[] @group)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateGroupException">If name and scope match</exception>
        /// <param name="group"></param>
        public void Add(RightsGroup @group)
        {
            lock (_syncR)
            {
                HandleDuplicate(@group);
                _rights.Add(@group);
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
        public void Save(RightsGroup @group)
        {
            lock (_syncR)
            {
                HandleDuplicate(@group,true);
                _rights.RemoveAll(d => d.Id == @group.Id);
                _rights.Add(@group);
            }
        }

        public RightsGroup GetRightsGroup(Guid id)
        {
            return _rights.Find(d => d.Id == id);
        }

        public IEnumerable<RightsGroup> GetRightsGroupsForUser(Guid userId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public IEnumerable<ScopedRights> GetRights(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}