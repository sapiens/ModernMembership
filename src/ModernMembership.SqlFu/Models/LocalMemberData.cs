using System;
using CavemanTools;
using CavemanTools.Model.ValueObjects;
using CavemanTools.Web;
using SqlFu;
using SqlFu.DDL;

namespace ModernMembership.SqlFu.Models
{
    [Table(Table,CreationOptions = IfTableExists.Ignore)]
    [Index("MemberId",IsUnique = true,Name = UniqueIdIndex)]
    [Index("Name,Scope",IsUnique = true,Name = UniqueNameIndex)]
    [Index("Email,Scope",IsUnique = true,Name = UniqueEmailIndex)]
    internal class LocalMemberData
    {
        public const string UniqueIdIndex="uk_local_id";
        public const string UniqueNameIndex="uk_local_name";
        public const string UniqueEmailIndex="uk_local_email";

        public const string Table = "LocalMembers";

        public long Id { get; set; }
        public Guid MemberId { get; set; }
        [ColumnOptions(Size = "75")]
        public string Name { get; set; }
        [ColumnOptions(Size = "250")]
        public string Email { get; set; }
        public Guid Scope { get; set; }
        public DateTime RegisteredOn { get; set; }
        public MemberStatus Status { get; set; }
        [ColumnOptions(IsNullable = true)]
        public string DisplayName { get; set; }
        public string PasswordHash { get; set; }

        public LocalMemberData()
        {
            
        }

        public LocalMemberData(LocalMember.Memento member)
        {
            MemberId = member.Id;
            Name = member.Name.Value;
            Email = member.Email.Value;
            Scope = member.Scope.Value;
            RegisteredOn = member.RegisteredOn;
            Status = member.Status;
            DisplayName = member.DisplayName;
            PasswordHash = member.Password;
        }

        public LocalMember.Memento ToMemento()
        {
            var memento = new LocalMember.Memento();
            memento.Id = MemberId;
            memento.Name=new LoginName(Name);
            memento.Email=new Email(Email);
            memento.Scope=new ScopeId(Scope);
            memento.RegisteredOn = RegisteredOn;
            memento.Status = Status;
            memento.DisplayName = DisplayName;
            memento.Password = PasswordHash;
            return memento;
        }
    }
}