using System;
using FluentAssertions;
using ModernMembership.Web;
using Xunit;

namespace Tests.Web
{
    public class AnonymousMemberSessionTests
    {
        private CavemanMemberSession _sut;

        public AnonymousMemberSessionTests()
        {
            _sut = new CavemanMemberSession();
        }

        [Fact]
        public void is_not_authenticated()
        {
            _sut.IsAuthenticated.Should().BeFalse();
        }

        [Fact]
        public void has_no_name_set()
        {
            _sut.Name.Should().BeNullOrEmpty();
        }

        [Fact]
        public void user_id_is_empty()
        {
            _sut.MemberId.Should().Be(Guid.Empty);
        }

        [Fact]
        public void session_id_is_empty()
        {
            _sut.Info.Should().BeNull();
        }


    }
}