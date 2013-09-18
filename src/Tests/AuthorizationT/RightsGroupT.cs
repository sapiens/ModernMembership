using System.Linq;
using FluentAssertions;
using ModernMembership;
using ModernMembership.Authorization;
using ModernMembership.Authorization.Events;
using Xunit;
using System;
using System.Diagnostics;

namespace Tests.AuthorizationT
{
    public class RightsGroupT
    {
        private Stopwatch _t = new Stopwatch();


        [Fact]
        public void new_group_requires_id_name()
        {
            var id = Guid.NewGuid();
            var group = new RightsGroup(id, new GroupName("test"));
            group.Id.Should().Be(id);
            group.Name.Value.Should().Be("test");
            group.Scope.Should().BeNull();
        }

        [Fact]
        public void new_group_has_no_rights()
        {
            var group = Setup.SomeRightsGroup();
            group.Rights.Should().BeEmpty();
        }

        [Fact]
        public void new_group_can_have_scope()
        {
            var group = new RightsGroup(Guid.NewGuid(), new GroupName("test"),new ScopeId(Guid.NewGuid()));
            group.Scope.Should().NotBeNull();
        }

        [Fact]
        public void name_change_doesnt_accept_null()
        {
            var grp = Setup.SomeRightsGroup();
            grp.Invoking(g => g.Name = null).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void name_change_generates_proper_event()
        {
            var grp = Setup.RightsGroupFromMemento();
            grp.Name=new GroupName("bla");
            var evnt = grp.GetGeneratedEvents().First().Cast<GroupNameChanged>();
            evnt.Id.Should().Be(grp.Id);
            evnt.Name.Should().Be(grp.Name.Value);
        }


        public static RightsGroup.Memento CreateMemento()
        {
            return new RightsGroup.Memento()
                {
                    Id=Guid.NewGuid(),Name = new GroupName("memento"),Rights = new short[]{1,3},Scope = new ScopeId(Guid.NewGuid())
                };
        }

        [Fact]
        public void init_from_memento_restores_state()
        {
            var memento = CreateMemento();
            var grp = new RightsGroup(memento);
            grp.Id.Should().Be(memento.Id);
            grp.Name.Should().Be(memento.Name);
            grp.Rights.ShouldAllBeEquivalentTo(memento.Rights);
            grp.Scope.Should().Be(memento.Scope);
        }

        [Fact]
        public void init_fails_when_memento_data_is_invalid()
        {
            Assert.Throws<ArgumentException>(() => new RightsGroup(new RightsGroup.Memento()));
        }

        [Fact]
        public void memento_object_contains_group_properties_with_public_getters()
        {
            var grp = Setup.SomeRightsGroup();
            var memento = grp.GetMemento();
            memento.Id.Should().Be(grp.Id);
            memento.Name.Should().Be(grp.Name);
            memento.Rights.ShouldAllBeEquivalentTo(grp.Rights);
            memento.Scope.Should().Be(grp.Scope);
        }

      
        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}