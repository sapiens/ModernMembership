using System;
using System.Data.Common;
using ModernMembership.Web;
using SqlFu;
using SessionData = ModernMembership.SqlFu.Models.SessionData;

namespace ModernMembership.SqlFu
{
    public class SessionStorage:IMemberSessionStorage
    {
        private Func<DbConnection> _getDb;

        public SessionStorage(Func<DbConnection> db)
        {
            db.MustNotBeNull();
            _getDb = db;
        }
        
        public void Add(SessionStorageData data)
        {
            data.MustNotBeNull();
            using (var db = _getDb())
            {
                db.Insert(new SessionData(data));
            }
        }

        public SessionStorageData Get(Guid id)
        {
            using (var db = _getDb())
            {
                var data = db.GetColumnValue<SessionData, string>(d => d.Data, d => d.SessionId == id);
                if (data == null)
                {
                    return null;
                }
                return data.Deserialize<SessionStorageData>();
            }
        }

        public void Update(SessionStorageData data)
        {
            data.MustNotBeNull();
            using (var db = _getDb())
            {
                db.Update<SessionData>()
                  .Set(d => d.Data, data.Serialize())
                  .Set(d => d.ExpiresOn, data.ExpiresOn)
                  .Where(d => d.SessionId == data.Id)
                  .Execute();
            }
        }

        public void Delete(Guid id)
        {
            using (var db = _getDb())
            {
                db.DeleteFrom<SessionData>(d => d.SessionId == id);
            }
        }
    }
}