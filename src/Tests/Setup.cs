using System;
using CavemanTools;
using CavemanTools.Model.ValueObjects;
using CavemanTools.Web;
using ModernMembership;
using ModernMembership.Authorization;
using ModernMembership.Web;
using Ploeh.AutoFixture;

namespace Tests
{
    public class Setup
    {
        static Fixture _fixture=new Fixture();
        public static Fixture GetAutoFixture()
        {
            return _fixture;
        }

        public static Guid AFixedId=Guid.NewGuid();
        
        public static SessionId AFixedSessionId=SessionId.NewId();

        public static ScopeId ARandomScope=new ScopeId(Guid.NewGuid());


        public static ExternalMember AnExternalMember()
        {
            return new ExternalMember(Guid.NewGuid(), new ExternalMemberId("fdb", Guid.NewGuid().ToString()), ScopeId.Global,DateTime.UtcNow);
        }

        public static UserGroup UserGroupWithARandomUser()
        {
            var ug = new UserGroup(Guid.NewGuid(),new[]{Guid.NewGuid()});
            return ug;
        }

        public static LocalMember.Memento AMemento()
        {
            var m = new LocalMember.Memento();
            m.Id = Guid.NewGuid();
            m.Name = new LoginName("valid-login-name");
            m.Status = MemberStatus.Locked;
            m.Email = new Email("bla@me.com");
            m.Password = new PasswordHash("hash",Salt.Generate()).ToString();
            m.Scope = ScopeId.Global;
            m.RegisteredOn = DateTime.UtcNow;
            return m;
        }
        public static LocalMember ALocalMember(bool globalScope=true,string name="")
        {
            if (name.IsNullOrEmpty())
            {
                name = "test"+Guid.NewGuid().ToString().Substring(4);
            }
            return new LocalMember(Guid.NewGuid(), new LoginName(name), new PasswordHash("bla",Salt.Generate()), new Email("bla{0}@yahoo.com".ToFormat(Guid.NewGuid().ToString().Substring(4))), globalScope ? ScopeId.Global : new ScopeId(Guid.NewGuid()));
        }

        public static MemberSessionPrincipal AnonymousMemberSession()
        {
            return new MemberSessionPrincipal();
        }

        static SetupUserRights userRights=new SetupUserRights();


        public static SetupUserRights UserRights
        {
            get { return userRights; }
        }

        public static Email AFixedEmail
        {
            get
            {
                return new Email("test@example.com");
            }
        }

        public static SetupPassword APassword
        {
            get
            {
                return new SetupPassword();
            }
        }

        public static RightsGroup SomeRightsGroup(string name="test")
        {
            return new RightsGroup(Guid.NewGuid(),new GroupName(name));
        }

        public static RightsGroup RightsGroupFromMemento()
        {
            return new RightsGroup(new RightsGroup.Memento()
                {
                    Id=Guid.NewGuid(),Name = new GroupName("test")
                });
        }
    }

    public class SetupUserRights
    {
        public ScopedRights GlobalAdminRights=new ScopedRights(ScopeId.Global,new[]{ScopedRights.GlobalAdmin});
        
        public ScopedRights ScopedAdminRights=new ScopedRights(Setup.ARandomScope,new[]{ScopedRights.ScopedAdmin});
        public const int Right1 = 1;
        public const int Right2 = 3;
        public ScopedRights FixedRights= new ScopedRights(new ScopeId(Setup.AFixedId), new int[]{Right1,Right2});
        
    }



    public class SetupPassword
    {
        public string FixedValue = "pwd";
        public Salt salt = Salt.Generate();
        public PasswordHash FixedHash
        {
            get
            {
               return new PasswordHash(FixedValue, salt);
            }
        }
    }
}