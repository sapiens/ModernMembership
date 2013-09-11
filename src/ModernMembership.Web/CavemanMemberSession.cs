using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using ModernMembership.Authorization;

namespace ModernMembership.Web
{
    public class CavemanMemberSession:IIdentity
    {
        private readonly IEnumerable<ScopedRights> _rights=new ScopedRights[0];
        public Guid MemberId { get; private set; }

        public SessionData Info { get; private set; }

        /// <summary>
        /// Authentication type
        /// </summary>
        public const string Type = "Caveman";

        /// <summary>
        /// Creates an anonymous session
        /// </summary>
        public CavemanMemberSession()
        {
            IsAuthenticated = false;
            Scope = ScopeId.Global;
        }

        
        public CavemanMemberSession(SessionStorageData session)
        {
            session.MustNotBeNull();
            Info = session;
            _rights = session.MemberInfo.Rights;
           
            MemberId = session.MemberInfo.MemberId;
            Name = session.MemberInfo.MemberName;
            IsAuthenticated = true;
            Scope = ScopeId.Global;
        }

        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        /// <returns>
        /// The name of the user on whose behalf the code is running.
        /// </returns>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        /// <returns>
        /// The type of authentication used to identify the user.
        /// </returns>
        public string AuthenticationType
        {
            get { return Type; }
        }

        /// <summary>
        /// Gets a value that indicates whether the user has been authenticated.
        /// </summary>
        /// <returns>
        /// true if the user was authenticated; otherwise, false.
        /// </returns>
        public bool IsAuthenticated
        {
            get; private set;
        }

        Dictionary<string, object> _other = new Dictionary<string, object>();

        /// <summary>
        /// Checks if member has at least one of the specified rights in the active scope.
        /// Globally scoped rights are always verified first.
        /// </summary>
        /// <param name="rights">Collection of rights</param>
        /// <returns></returns>
        public bool HasRight(params short[] rights)
        {
            return GetValidGroups().Any(sr=>sr.HasRight(rights));
        }


        IEnumerable<ScopedRights> GetValidGroups()
        {
            if (Scope==null) throw new InvalidOperationException("Scope is undefined. For global scope, use ScopeId.Global");
            if (Scope.Equals(ScopeId.Global)) return _rights.Where(g => g.Scope.Equals(ScopeId.Global));
            return _rights.Where(g => g.Scope.Equals(ScopeId.Global) || Scope.Equals(g.Scope));
        }

        /// <summary>
        /// Gets a dictionary where you can store other information about the user
        /// </summary>
        public IDictionary<string, object> OtherData
        {
            get { return _other; }
        }

        /// <summary>
        /// Default is ScopeId.Global
        /// </summary>
        public ScopeId Scope { get; set; }
    }
}