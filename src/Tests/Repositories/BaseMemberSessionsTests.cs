using System.Linq;
using CavemanTools;
using ModernMembership.Authorization;
using ModernMembership.Web;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace Tests.Repositories
{
    public abstract class BaseMemberSessionsTests
    {
        private Stopwatch _t = new Stopwatch();
        private IMemberSessionStorage _sut;
        private SessionStorageData _data;

        public BaseMemberSessionsTests()
        {
            _sut = GetStorage();

            _data = new SessionStorageData()
                {
                    Id=SessionId.NewId(),
                    ExpiresOn = DateTime.UtcNow,
                    MemberInfo = new MemberSessionData()
                        {
                            MemberId = Setup.AFixedId,
                            MemberName = "a name",
                            Rights = Enumerable.Empty<ScopedRights>()

                        }
                };
        }

        protected abstract IMemberSessionStorage GetStorage();

        [Fact]
        public void get_added_session()
        {
            _sut.Add(_data);
            _sut.Get(_data.Id).MemberInfo.MemberName.Should().Be("a name");
        }

        [Fact]
        public void get_nonexisting_session_returns_null()
        {
            _sut.Get(SessionId.NewId()).Should().BeNull();
        }

        [Fact]
        public void update_session()
        {
            _sut.Add(_data);
            var s2 = _sut.Get(_data.Id);
            s2.IsSliding = true;
            _sut.Update(s2);

            _sut.Get(s2.Id).IsSliding.Should().BeTrue();
        }

        [Fact]
        public void delete_session()
        {
            _sut.Add(_data);
            _sut.Delete(_data.Id);
            _sut.Get(_data.Id).Should().BeNull();
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}