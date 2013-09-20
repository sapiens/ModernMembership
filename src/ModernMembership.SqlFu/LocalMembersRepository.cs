using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using CavemanTools.Model;
using CavemanTools.Model.ValueObjects;
using ModernMembership.SqlFu.Models;
using SqlFu;
using System.Linq;

namespace ModernMembership.SqlFu
{
    public class LocalMembersRepository:ILocalMembersRepository
    {
        private readonly Func<DbConnection> _getDb;

        private const string Table = LocalMemberData.Table;

        public LocalMembersRepository(Func<DbConnection> cnxFactory)
        {
            _getDb = cnxFactory;
            cnxFactory.MustNotBeNull();
        }

        public IDictionary<MemberStatus, int> GetStats()
        {
            using (var db = _getDb())
            {
                var all = db.Query<dynamic>("select Status,count(id) as Total from {0} group by status".ToFormat(Table));
                return all.ToDictionary(d => (MemberStatus)d.Status, d => (int)d.Total);
            }               
        }

        /// <summary>
        /// </summary>
        /// <exception cref="DuplicateMemberIdException"></exception>
        /// <exception cref="DuplicateLoginNameException"></exception>
        /// <exception cref="DuplicateEmailException"></exception>
        /// <param name="member"></param>
        public void Add(LocalMember member)
        {
            member.MustNotBeNull();
            var data = new Models.LocalMemberData(member.GetMemento());
            using (var db = _getDb())
            {
               try
               {
                   db.Insert(data);
               }
               catch (DbException ex)
               {
                   HandleDuplicates(ex, false);
                   throw;
               }
            }
        }

       void HandleDuplicates(DbException ex,bool updateMode)
        {
            if (!updateMode)
            {
                if (ex.Message.Contains(LocalMemberData.UniqueIdIndex))
                {
                    throw new DuplicateMemberIdException();
                }
            }

           if (ex.Message.Contains(LocalMemberData.UniqueEmailIndex))
           {
               throw new DuplicateEmailException();
           }

           if (ex.Message.Contains(LocalMemberData.UniqueNameIndex))
           {
               throw new DuplicateLoginNameException();
           }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateLoginNameException"></exception>
        /// <exception cref="DuplicateEmailException"></exception>
        /// <param name="member"></param>
        public void Save(LocalMember member)
        {
            member.MustNotBeNull();
            var data = new Models.LocalMemberData(member.GetMemento());
            using (var db = _getDb())
            {
               try
               {
                   db.Update<LocalMemberData>(data, d => d.MemberId == member.Id);
               }
               catch (DbException ex)
               {
                   HandleDuplicates(ex, true);
                   throw; 
               }
            }
        }

        public LocalMember GetMember(Guid id)
        {
            return GetByCriteria(d => d.MemberId == id);
        }

        LocalMember GetByCriteria(Expression<Func<LocalMemberData, bool>> criteria)
        {
            using (var db = _getDb())
            {
                var data = db.Get(criteria);
                if (data == null)
                {
                    return null;
                }
                return new LocalMember(data.ToMemento());
            }
        }

        public LocalMember GetMember(Email email)
        {
            email.MustNotBeNull();
            return GetByCriteria(d => d.Email == email.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="scope">Use ScopeId.Global for global scope. Null to ignore the scope</param>
        /// <returns></returns>
        public LocalMember GetMember(LoginName id, ScopeId scope = null)
        {
            id.MustNotBeNull();
            if (scope == null)
            {
                return GetByCriteria(d => d.Name == id.Value);
            }
            return GetByCriteria(d => d.Name == id.Value && d.Scope == scope.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="scope">Use ScopeId.Global for global scope.Null to ignore scope</param>
        /// <returns></returns>
        public PagedResult<LocalMember> GetMembers(long skip, int take, ScopeId scope = null)
        {
            using (var db = _getDb())
            {
                var sql = @"select * from {0}".ToFormat(Table);
                var args = new List<object>();
                if (scope != null)
                {
                    sql += " where Scope=@0";
                    args.Add(scope.Value);
                }
                sql += " order by id";
                var all = db.PagedQuery<LocalMemberData>(skip, take, sql, args.ToArray());
                var result = new PagedResult<LocalMember>();
                result.Count = all.Count;
                result.Items = all.Items.Select(d => new LocalMember(d.ToMemento())).ToArray();
                return result;
            }
            
        }

        public void Delete(params Guid[] ids)
        {
           if (ids.Length==0) return;
            using (var db = _getDb())
            {
                db.DeleteFrom<LocalMemberData>(d=>ids.Contains(d.MemberId));
            }
        }

        /// <summary>
        /// Deletes local members who didn't activate their account in the specified period
        /// </summary>
        /// <param name="interval"></param>
        public void PurgeUnactivatedMembers(TimeSpan interval)
        {
            var comparisonDate = DateTime.UtcNow.Subtract(interval);
            using (var db = _getDb())
            {
                db.DeleteFrom<Models.LocalMemberData>(user=>user.Status == MemberStatus.NeedsActivation && user.RegisteredOn<comparisonDate);
            }            
        }
    }
}