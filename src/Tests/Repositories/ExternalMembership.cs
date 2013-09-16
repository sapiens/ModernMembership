using System.Collections.Generic;
using System.Linq;
using ModernMembership;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.Repositories
{
    public class MemoryExternalmembership:ExternalMembership
    {
        protected override IExternalMembersRepository GetRepository()
        {
            return new ExternalMembersMemoryRepository();
        }
    }

    public abstract class ExternalMembership
    {
        private Stopwatch _t = new Stopwatch();
        protected IExternalMembersRepository _sut;

        public ExternalMembership()
        {
            _sut = GetRepository();
        }


        protected abstract IExternalMembersRepository GetRepository();

        [Fact]
        public void get_added_member()
        {
            var member = Setup.AnExternalMember();
            _sut.Add(member);
            var m2 = _sut.GetMember(member.Id);
            m2.ExternalId.Should().Be(member.ExternalId);
        }

        [Fact]
        public void get_nonexisting_member_returns_null()
        {
            _sut.GetMember(Guid.NewGuid()).Should().BeNull();

        }

        [Fact]
        public void adding_member_with_same_id_throws()
        {
            var member = Setup.AnExternalMember();
            _sut.Add(member);
            var m2 = new ExternalMember(member.Id, ExternalMemberId.RandomTestValue(), ScopeId.Global);
            _sut.Invoking(s => s.Add(m2)).ShouldThrow<DuplicateMemberIdException>();
        }

        [Fact]
        public void adding_member_with_same_externalId_throws()
        {
            var member = Setup.AnExternalMember();
            _sut.Add(member);
            var m2 = new ExternalMember(Guid.NewGuid(), member.ExternalId, ScopeId.Global);
            _sut.Invoking(s => s.Add(m2)).ShouldThrow<DuplicateExternalIdException>();
        }

        [Fact]
        public void get_member_by_external_id()
        {
            var member = Setup.AnExternalMember();
            _sut.Add(member);
            var m2 = _sut.GetMember(member.ExternalId);
            m2.Id.Should().Be(member.Id);
        }

        [Fact]
        public void get_nonexisting_member_by_externalid_returns_null()
        {
            _sut.GetMember(ExternalMemberId.RandomTestValue()).Should().BeNull();
        }

        [Fact]
        public void update_member()
        {
            var member = Setup.AnExternalMember();
            _sut.Add(member);

            var m2 = _sut.GetMember(member.Id);
            m2.DisplayName = "test";
            _sut.Save(m2);
            var m3 = _sut.GetMember(member.Id);
            m3.DisplayName.Should().Be("test");
        }

        [Fact]
        public void get_members()
        {
            _sut.Add(Setup.AnExternalMember());
            _sut.Add(new ExternalMember(Guid.NewGuid(), ExternalMemberId.RandomTestValue(), Setup.AScope));
            var member = Setup.AnExternalMember();
            _sut.Add(member);
            var data = _sut.GetMembers(1, 6);
            data.Count.Should().Be(3);
            data.Items.Any(d => d.Id == member.Id).Should().BeTrue();
        }

        [Fact]
        public void get_members_filtered_by_scope()
        {
            _sut.Add(Setup.AnExternalMember());
            _sut.Add(Setup.AnExternalMember()); 
            _sut.Add(new ExternalMember(Guid.NewGuid(),ExternalMemberId.RandomTestValue(),Setup.AScope));

            var all = _sut.GetMembers(0, 10, ScopeId.Global);
            all.Count.Should().Be(2);
        }


        [Fact]
        public void delete_members()
        {
            var member = Setup.AnExternalMember();
            _sut.Add(member);

            _sut.Delete(member.Id);

            _sut.GetMember(member.Id).Should().BeNull();
        }

        [Fact]
        public void get_stats()
        {
            _sut.Add(Setup.AnExternalMember());
            _sut.Add(Setup.AnExternalMember());
            _sut.Add(new ExternalMember(Guid.NewGuid(), ExternalMemberId.RandomTestValue(), Setup.AScope,status:MemberStatus.Locked));

            var result = new Dictionary<MemberStatus,int>(_sut.GetStats());
            result[ExternalMember.DefaultStatus].Should().Be(2);
            result[MemberStatus.Locked].Should().Be(1);
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}