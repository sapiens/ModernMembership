using System.Linq;
using ModernMembership.Authorization;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.Repositories
{
    
    public class MemoryRightsGroups:RightsGroups
    {
        protected override IRightsGroupsRepository GetRepo()
        {
            return new UserGroupsMemoryRepository();
        }
    }

    public abstract class RightsGroups
    {
        private Stopwatch _t = new Stopwatch();
        private IRightsGroupsRepository _sut;

        public RightsGroups()
        {
            _sut = GetRepo();
        }

        protected abstract IRightsGroupsRepository GetRepo();

        [Fact]
        public void add_get_group()
        {
            var grp = Setup.SomeRightsGroup();
            _sut.Add(grp);
            var g2 = _sut.GetRightsGroup(grp.Id);
            g2.Name.Should().Be(grp.Name);

        }

        [Fact]
        public void adding_group_with_same_name_and_scope_throws()
        {
            var grp = Setup.SomeRightsGroup();
            _sut.Add(grp);
            var grp2 = new RightsGroup(Guid.NewGuid(), grp.Name, grp.Scope);
            _sut.Invoking(s => s.Add(grp2)).ShouldThrow<DuplicateGroupException>();
        }

        [Fact]
        public void get_nonexisting_group_returns_null()
        {
            _sut.GetRightsGroup(Guid.NewGuid()).Should().BeNull();
        }

        [Fact]
        public void delete()
        {
            var grp = Setup.SomeRightsGroup();
            _sut.Add(grp);
            _sut.Delete(grp.Id);
            _sut.GetRightsGroup(grp.Id).Should().BeNull();
        }

        [Fact]
        public void save_group()
        {
            var grp = Setup.SomeRightsGroup();
            _sut.Add(grp);
            var g2 = _sut.GetRightsGroup(grp.Id);
            g2.Name=new GroupName("changed");
            _sut.Save(g2);
            var g3 = _sut.GetRightsGroup(grp.Id);
            g3.Name.Value.Should().Be("changed");
        }

        [Fact]
        public void saving_group_with_duplicate_name_and_scope_throws()
        {
            var grp = Setup.SomeRightsGroup();
            _sut.Add(grp);

            var grp2 = Setup.SomeRightsGroup("test2");
            _sut.Add(grp2);
            
            var g2 = _sut.GetRightsGroup(grp2.Id);
            g2.Name = grp.Name;
            _sut.Invoking(s => s.Save(g2)).ShouldThrow<DuplicateGroupException>();
        }

        [Fact]
        public void get_paged_groups()
        {
           
            _sut.Add(Setup.SomeRightsGroup("tt1"));
            _sut.Add(Setup.SomeRightsGroup("tt2"));
            _sut.Add(Setup.SomeRightsGroup("tt3"));

            var grps = _sut.GetPaged(1, 2);
            grps.Count.Should().Be(3);
            grps.Items.Any(s => s.Name.Value == "tt2").Should().BeTrue();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}