using CavemanTools.Model.ValueObjects;
using ModernMembership;
using Ploeh.AutoFixture;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;
using System.Linq;

namespace Tests.Repositories
{
   public class MemoryLocalMembershipActions:LocalMembershipActions,IDisposable
   {
       protected override ILocalMembersRepository GetRepository()
       {
           return new LocalMembersMemoryRepository();
       }

       /// <summary>
       /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
       /// </summary>
       public void Dispose()
       {
           (_sut as LocalMembersMemoryRepository).Dispose();
       }
   }
    
    
    public abstract class LocalMembershipActions
    {
        private Stopwatch _t = new Stopwatch();
        protected ILocalMembersRepository _sut;

        protected LocalMembershipActions()
        {
            _sut = GetRepository();
        }

        protected abstract ILocalMembersRepository GetRepository();

        [Fact]
        public void get_by_id_an_added_member()
        {
            var member = Setup.ALocalMember();
            _sut.Add(member);
            var m2 = _sut.GetMember(member.Id);
            member.Name.Should().Be(m2.Name);

        }
        
        [Fact]
        public void get_by_name_igoring_scope_an_added_member()
        {
            var member = Setup.ALocalMember();
            _sut.Add(member);
            var m2 = _sut.GetMember(member.Name);
            member.Id.Should().Be(m2.Id);
        }

        [Fact]
        public void get_by_name_and_scope_an_added_member()
        {
            var somedata = Setup.ALocalMember();
            _sut.Add(somedata);
            var member = Setup.ALocalMember(false,"heelo");
           _sut.Add(member);
            var m2 = _sut.GetMember(member.Name,member.Scope);
            member.Id.Should().Be(m2.Id);
        }

        [Fact]
        public void get_by_name_a_nonexistant_member_returns_null()
        {
            _sut.GetMember(new LoginName("bla")).Should().Be(null);
        }
        
        [Fact]
        public void get_by_name_a_nonexistant_member_in_scope_returns_null()
        {
            var member = Setup.ALocalMember(false);
            _sut.Add(member);
            _sut.GetMember(member.Name,ScopeId.Global).Should().Be(null);
        }
        
        [Fact]
        public void get_by_id_a_nonexistant_member_returns_null()
        {
            _sut.GetMember(Guid.NewGuid()).Should().Be(null);
        }
        
        [Fact]
        public void get_by_email_a_nonexistant_member_returns_null()
        {
            _sut.GetMember(new Email("test@example.com")).Should().Be(null);
        }

        [Fact]
        public void get_by_email()
        {
            var member = Setup.ALocalMember();
            _sut.Add(member);
            var m2 = _sut.GetMember(member.Email);
            member.Id.Should().Be(m2.Id);
        }

        [Fact]
        public void update_an_existing_member()
        {
            var member = Setup.ALocalMember();
            _sut.Add(member);
            var m2 = _sut.GetMember(member.Id);
            m2.DisplayName = "haha";
            _sut.Save(m2);
            var m3 = _sut.GetMember(member.Id);
            m3.DisplayName.Should().Be("haha");
        }

        [Fact]
        public void delete_members()
        {
            var member = Setup.ALocalMember();
            _sut.Add(member);
            _sut.Delete(member.Id);
            _sut.GetMember(member.Id).Should().BeNull();
        }

        [Fact]
        public void get_paged_member_list()
        {
            var member1 = Setup.ALocalMember(true,"member1");
            var member2 = Setup.ALocalMember(true,"member2");
            _sut.Add(member1);
            _sut.Add(member2);

            var members = _sut.GetMembers(0, 2);
            members.Count.Should().Be(2);
            members.Items.Any(m => m.Id == member2.Id).Should().BeTrue();
        }

        
        [Fact]
        public void when_adding_member_duplicate_id_throws()
        {
            var member = Setup.ALocalMember();
            _sut.Add(member);
            _sut.Invoking(s => s.Add(member)).ShouldThrow<DuplicateMemberIdException>();
        }

        [Fact]
        public void when_adding_member_duplicate_name_throws()
        {
            var member = Setup.ALocalMember();
            _sut.Add(member);
            var m2 = new LocalMember(new LocalMember.Memento()
                {
                    Email = new Email("bla@hi.com"),
                    LoginId = member.Name,
                    Password = Setup.APassword.Hash,
                    Scope = ScopeId.Global
                });
            _sut.Invoking(s => s.Add(m2)).ShouldThrow<DuplicateLoginNameException>();
        }
        
        [Fact]
        public void when_adding_member_duplicate_email_throws()
        {
            var member = Setup.ALocalMember();
            _sut.Add(member);
            var m2 = new LocalMember(new LocalMember.Memento()
            {
                Email = member.Email,
                LoginId = new LoginName("bla1234"),
                Password = Setup.APassword.Hash,
                Scope = ScopeId.Global
            });

            _sut.Invoking(s => s.Add(m2)).ShouldThrow<DuplicateEmailException>();
        }

        [Fact]
        public void update_member_with_duplicate_name_throws()
        {
            var member = Setup.ALocalMember();
            _sut.Add(member);
            _sut.Add(Setup.ALocalMember(name:"aha12"));

            var old = _sut.GetMember(member.Id);
            old.ChangeName(new LoginName("aha12"));
            _sut.Invoking(s => s.Save(old)).ShouldThrow<DuplicateLoginNameException>();
        }

      [Fact]
        public void update_member_with_duplicate_email_throws()
        {
            var member = Setup.ALocalMember();
            var member2 = Setup.ALocalMember();
          
            _sut.Add(member);
            _sut.Add(member2);

            var old = _sut.GetMember(member.Id);
            old.ChangeEmail(member2.Email);
            _sut.Invoking(s => s.Save(old)).ShouldThrow<DuplicateEmailException>();
        }

        [Fact]
        public void get_stats()
        {
            var member = Setup.ALocalMember();
            var member2 = Setup.ALocalMember();
            member2.Status=MemberStatus.Deleted;
            _sut.Add(member);
            _sut.Add(member2);

            var stats = _sut.GetStats();
            stats[LocalMember.DefaultStatus].Should().Be(1);
            stats[MemberStatus.Deleted].Should().Be(1);
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}