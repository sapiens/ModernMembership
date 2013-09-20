using System;
using SqlFu;
using SqlFu.DDL;

namespace ModernMembership.SqlFu.Models
{
    [Table(Table,CreationOptions = IfTableExists.DropIt)]
    [Index("MemberId", IsUnique = true, Name = UniqueIdIndex)]
    [Index("ExternalId,Scope", IsUnique = true, Name = UniqueNameIndex)]
    public class ExternalMemberData
    {
        public const string UniqueIdIndex = "uk_external_id";
        public const string UniqueNameIndex = "uk_external_name";
        
        public const string Table = "ExternalMembers";

        public long Id { get; set; }
        public Guid MemberId { get; set; }
        [ColumnOptions(Size = "300")]
        public string ExternalId { get; set; }
        public Guid Scope { get; set; }
        public DateTime RegisteredOn { get; set; }
        public MemberStatus Status { get; set; }
        [ColumnOptions(IsNullable = true)]
        public string DisplayName { get; set; }

        public ExternalMemberData()
        {
            
        }

        public ExternalMemberData(ExternalMember member)
        {
            MemberId = member.Id;
            ExternalId = member.ExternalId.ToString();
            Scope = member.Scope.Value;
            RegisteredOn = member.RegisteredOn;
            Status = member.Status;
            DisplayName = member.DisplayName;
        }

        public ExternalMember ToMember()
        {
            return new ExternalMember(MemberId,ExternalMemberId.FromString(ExternalId),new ScopeId(Scope),RegisteredOn,DisplayName,Status);
        }

    }
}