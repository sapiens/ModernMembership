using System;
using System.Collections.Generic;
using System.Data.Common;
using CavemanTools.Model;
using ModernMembership.Authorization;
using ModernMembership.Authorization.Events;
using ModernMembership.SqlFu.Models;
using SqlFu;
using System.Linq;

namespace ModernMembership.SqlFu
{
  
    public class UserRightsRepository:IRightsGroupsRepository,IUserGroupsRepository,IUserRightsService
    {
        private Func<DbConnection> _getDb;

        public UserRightsRepository(Func<DbConnection> db)
        {
            _getDb = db;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateGroupException">If name and scope match</exception>
        /// <param name="grp"></param>
        public void Add(RightsGroup grp)
        {
           grp.MustNotBeNull();
           using (var db = _getDb())
           {
               var data = new RightsGroupData(grp.GetMemento());
               try
               {
                   db.Insert(data);
               }
               catch (DbException ex)
               {
                   HandleDuplicates(ex);
                   throw;
               }
           }
        }

        private void HandleDuplicates(DbException ex)
        {
            if (ex.Message.Contains(RightsGroupData.UniqueNameIndex))
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
            grp.MustNotBeNull();
            using (var db = _getDb())
            {
                var id = db.GetColumnValue<RightsGroupData, long>(d=>d.Id,d => d.GroupId == grp.Id);
                var data = grp.GetMemento();
                try
                {
                    db.Update(RightsGroupData.Table,
                              new {Id = id, Name = data.Name.Value, Rights = data.Rights.Serialize()});
                }
                catch (DbException ex)
                {
                    HandleDuplicates(ex);
                    throw;
                }
            }
        }

        public RightsGroup GetRightsGroup(Guid id)
        {
            using (var db = _getDb())
            {
                var data=db.Get<RightsGroupData>(d => d.GroupId == id);
                if (data == null) return null;
                return new RightsGroup(data.ToMemento());
            }
        }

        public IEnumerable<RightsGroup> GetRightsGroups(IEnumerable<Guid> ids)
        {
            using (var db = _getDb())
            {
                var data = db.Query<RightsGroupData>(d => ids.Contains(d.GroupId));
                if (data.IsNullOrEmpty()) return Enumerable.Empty<RightsGroup>();
                return data.Select(d=>new RightsGroup(d.ToMemento()));
            }
        }

        /// <summary>
        /// Duplicate id should be ignored
        /// </summary>
        /// <param name="grp"></param>
        public void Add(UserGroup grp)
        {
            grp.MustNotBeNull();
            if (grp.Users.Count()==0) return;
            using (var db = _getDb())
            {
                using (var t = db.BeginTransaction())
                {
                    foreach (var user in grp.Users)
                    {
                        var data = new UserGroupData();
                        data.GroupId = grp.Id;
                        data.MemberId = user;
                       try
                       {
                           db.Insert(data);
                       }
                       catch (DbException ex)
                       {
                           if (!ex.Message.Contains(UserGroupData.UniqueGroupUsersIndex))
                           {
                               throw;
                           }
                           //else we ignore the duplicate
                       }
                    }
                    t.Commit();
                }
            }
        }

        public UserGroup GetUserGroup(Guid groupId)
        {
            using (var db = _getDb())
            {
                var data = db.Query<UserGroupData>(d => d.GroupId == groupId);
                if (data.IsNullOrEmpty())
                {
                    return null;
                }
                return new UserGroup(groupId, data.Select(d => d.MemberId));
            }
        }

        public void Save(params UserGroup[] grps)
        {
            if (grps.Length==0) return;
            using (var db = _getDb())
            {
                using (var t = db.BeginTransaction())
                {
                    var events = grps.SelectMany(d => d.GetGeneratedEvents());
                    foreach (var evnt in events)
                    {
                        HandleAddedUsers(db,evnt as UsersAddedToGroup);
                        HandleRemovedUsers(db,evnt as UsersRemovedFromGroup);
                    }
                    
                    t.Commit();
                }
            }
        }

        static void HandleAddedUsers(DbConnection db, UsersAddedToGroup evnt)
        {
            if (evnt==null) return;
            foreach (var user in evnt.Users)
            {
                try
                {
                    db.Insert(new UserGroupData() { GroupId = evnt.GroupId, MemberId = user });
                }
                catch (DbException ex)
                {
                    if (!ex.Message.Contains(UserGroupData.UniqueGroupUsersIndex))
                    {
                        throw;
                    }
                    //else we ignore the duplicate
                }
                
                
            }
        }

        static void HandleRemovedUsers(DbConnection db, UsersRemovedFromGroup evnt)
        {
            if (evnt==null) return;
            db.DeleteFrom<UserGroupData>(d => d.GroupId == evnt.GroupId && evnt.Users.Contains(d.MemberId));            
        }

        public void Delete(Guid id)
        {
           using (var db = _getDb())
           {
               using (var t = db.BeginTransaction())
               {
                   db.DeleteFrom<RightsGroupData>(d => d.GroupId == id);
                   db.DeleteFrom<UserGroupData>(d => d.GroupId == id);
                   t.Commit();
               }
           }
        }

        public IEnumerable<UserGroup> GetGroupsForUser(Guid userId)
        {
            using (var db = _getDb())
            {
                var groups = db.QueryColumn<UserGroupData, Guid>(d => d.GroupId, d => d.MemberId == userId);
                if (groups.IsNullOrEmpty())
                {
                    return Enumerable.Empty<UserGroup>();
                }

                return db.Query<UserGroupData>(d => groups.Contains(d.GroupId))
                         .GroupBy(d => d.GroupId)
                         .Select(d => new UserGroup(d.Key, d.Select(g => g.MemberId)));
            }
        }

        public IEnumerable<RightsGroup> GetRightsGroupsForUser(Guid userId)
        {
            using (var db = _getDb())
            {
                var data = db.Query<RightsGroupData>(
@"select * from {0} where GroupId in (
select GroupId from {1} where MemberId=@0
)
".ToFormat(RightsGroupData.Table,UserGroupData.Table)
 ,userId
);
                if (data.IsNullOrEmpty()) Enumerable.Empty<RightsGroup>();
                return data.Select(grp => new RightsGroup(grp.ToMemento()));
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
            using (var db = _getDb())
            {
                var sql = @"select * from {0}".ToFormat(RightsGroupData.Table);
                var args = new List<object>();
                if (scope != null)
                {
                    sql += " where Scope=@0";
                    args.Add(scope.Value);
                }
                sql += " order by id";
                var all = db.PagedQuery<RightsGroupData>(skip, take, sql, args.ToArray());
                var result = new PagedResult<RightsGroup>();
                result.Count = all.Count;
                result.Items = all.Items.Select(d => new RightsGroup(d.ToMemento())).ToArray();
                return result;
            }
        }

        public IEnumerable<ScopedRights> GetRights(Guid userId)
        {
           using (var db = _getDb())
           {
               var data = db.Query<RightsGroupData>(
@"select Scope, Rights from {0} where GroupId in (
select GroupId from {1} where MemberId=@0
)
".ToFormat(RightsGroupData.Table, UserGroupData.Table)
 , userId
);
               if (data.IsNullOrEmpty()) Enumerable.Empty<RightsGroup>();
               
               List<int> rights=new List<int>();
               List<ScopedRights> scoped=new List<ScopedRights>();
               foreach (var grp in data.GroupBy(d=>d.Scope))
               {
                   grp.Select(d=>d.Rights.Deserialize<int[]>()).ForEach(r=>rights.AddRange(r));
                   scoped.Add(new ScopedRights(new ScopeId(grp.Key),rights));
                   rights.Clear();
               }
               return scoped;
           }
        }
    }
}