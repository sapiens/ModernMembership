using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using CavemanTools.Model;
using ModernMembership.SqlFu.Models;
using SqlFu;

namespace ModernMembership.SqlFu
{
    public class ExternalMembersRepository:IExternalMembersRepository
    {
        private Func<DbConnection> _getDb;

        private const string Table = ExternalMemberData.Table;

        public ExternalMembersRepository(Func<DbConnection> cnx)
        {
            cnx.MustNotBeNull();
            _getDb = cnx;
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
        /// 
        /// </summary>
        /// <exception cref="DuplicateMemberIdException"></exception>
        /// <exception cref="DuplicateExternalIdException"></exception>
        /// <param name="member"></param>
        public void Add(ExternalMember member)
        {
            member.MustNotBeNull();
            var data = new ExternalMemberData(member);
            using (var db = _getDb())
            {
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

        void HandleDuplicates(DbException ex)
        {
            if (ex.Message.Contains(ExternalMemberData.UniqueIdIndex))
            {
                throw new DuplicateMemberIdException();
            }

            if (ex.Message.Contains(ExternalMemberData.UniqueNameIndex))
            {
                throw new DuplicateExternalIdException();
            }
        }

        public void Save(ExternalMember member)
        {
            member.MustNotBeNull();            
            using (var db = _getDb())
            {
                db.Update<ExternalMemberData>()
                  .Set(d => d.Status, member.Status)
                  .Set(d => d.DisplayName, member.DisplayName)
                  .Where(d => d.MemberId == member.Id)
                  .Execute();
            }
        }

        public ExternalMember GetMember(Guid id)
        {
            return GetByCriteria(m => m.MemberId == id);
        }

        public ExternalMember GetMember(ExternalMemberId id)
        {
            id.MustNotBeNull();
            return GetByCriteria(d => d.ExternalId == id.ToString());
        }
        ExternalMember GetByCriteria(Expression<Func<ExternalMemberData, bool>> criteria)
        {
            using (var db = _getDb())
            {
                var data = db.Get(criteria);
                if (data == null)
                {
                    return null;
                }
                return data.ToMember();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="scope">Use null to ignore scope</param>
        /// <returns></returns>
        public PagedResult<ExternalMember> GetMembers(long skip, int take, ScopeId scope = null)
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
                var all = db.PagedQuery<ExternalMemberData>(skip, take, sql, args.ToArray());
                var result = new PagedResult<ExternalMember>();
                result.Count = all.Count;
                result.Items = all.Items.Select(d => d.ToMember()).ToArray();
                return result;
            }
        }

        public void Delete(params Guid[] ids)
        {
            if (ids.Length == 0) return;
            using (var db = _getDb())
            {
                db.DeleteFrom<ExternalMemberData>(d => ids.Contains(d.MemberId));
            }
        }
    }
}