using System;
using System.Collections.Generic;
using ModernMembership.Authorization;
using SqlFu;
using SqlFu.DDL;

namespace ModernMembership.SqlFu.Models
{
    [Table(Table,CreationOptions = IfTableExists.Ignore)]
    [Index("GroupId",IsUnique = true)]
    [Index("Name,Scope",IsUnique = true,Name = UniqueNameIndex)]
    internal class RightsGroupData
    {
        internal const string Table="RightsGroups";
        internal const string UniqueNameIndex="uk_group_name";
        
        public long Id { get; set; }
        public Guid GroupId { get; set; }
        [ColumnOptions(Size = "50")]
        public string Name { get; set; }
        public Guid Scope { get; set; }
        public string Rights { get; set; }

        public RightsGroupData()
        {
            
        }

        public RightsGroupData(RightsGroup.Memento memento)
        {
            GroupId = memento.Id;
            Name = memento.Name.Value;
            Scope = memento.Scope.Value;
            Rights = memento.Rights.Serialize();
        }

        public RightsGroup.Memento ToMemento()
        {
            var memento = new RightsGroup.Memento();
            memento.Id = GroupId;
            memento.Name=new GroupName(Name);
            memento.Rights = Rights.Deserialize<short[]>();
            memento.Scope=new ScopeId(Scope);
            return memento;
        }
    }

    [Table(Table,CreationOptions = IfTableExists.Ignore)]
    [Index("GroupId,MemberId",IsUnique = true,Name = UniqueGroupUsersIndex)]
    internal class UserGroupData
    {
        internal const string Table = "UsersGroups";
        internal const string UniqueGroupUsersIndex = "uk_group_users";
        public long Id { get; set; }
        public Guid GroupId { get; set; }
        public Guid MemberId { get; set; }
    }
}