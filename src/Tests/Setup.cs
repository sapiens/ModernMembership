using System;
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

        public static Guid AnId=Guid.NewGuid();

        public static ScopeId AScope=new ScopeId(Guid.NewGuid());


        public static MemberSessionPrincipal AnonymousMemberSession()
        {
            return new MemberSessionPrincipal();
        }

        static SetupUserRights userRights=new SetupUserRights();


        public static SetupUserRights UserRights
        {
            get { return userRights; }
        }

        public static Email SomeEmail
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

        public static RightsGroup CreateRightsGroup()
        {
            return new RightsGroup(Guid.NewGuid(),new GroupName("test"));
        }

        public static RightsGroup CreateExistingRightsGroup()
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
        
        public ScopedRights ScopedAdminRights=new ScopedRights(Setup.AScope,new[]{ScopedRights.ScopedAdmin});
        public const short Right1 = 1;
        public const short Right2 = 3;
        public ScopedRights SomeRights= new ScopedRights(new ScopeId(Setup.AnId), new short[]{Right1,Right2});
        
    }



    public class SetupPassword
    {
        public string Value = "pwd";
        public PasswordHash Hash
        {
            get
            {
                var str = new CavemanHashStrategy();
                return new PasswordHash(str.Hash(Value, "salt"), "salt");
            }
        }
    }
}