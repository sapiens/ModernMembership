using System;
using ModernMembership.Web;
using SqlFu;
using SqlFu.DDL;

namespace ModernMembership.SqlFu.Models
{
    [Table(Table,CreationOptions = IfTableExists.DropIt)]
    [Index("SessionId")]
    internal class SessionData
    {
        internal const string Table = "MemberSessions";

        public long Id { get; set; }
        public Guid SessionId { get; set; }
        public string Data { get; set; }
        public DateTime ExpiresOn { get; set; }

        public SessionData()
        {
            
        }

        public SessionData(SessionStorageData data)
        {
            SessionId = data.Id;
            Data = data.Serialize();
            ExpiresOn = data.ExpiresOn;
        }

        //public SessionStorageData ToObject()
        //{
        //    return Data.Deserialize<SessionStorageData>();
        //}
    }
}