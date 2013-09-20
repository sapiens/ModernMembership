using System;
using System.Collections.Generic;
using System.Linq;

namespace ModernMembership.Authorization
{
    public class ScopedRights
    {
        private readonly IEnumerable<short> _rights=new short[0];

        /// <summary>
        /// Should be asigned only to global scope groups. Value can be changed
        /// </summary>
        public static short GlobalAdmin = short.MaxValue - 5;
        /// <summary>
        /// Should be asigned only to scoped groups. Value can be changed
        /// </summary>
        public static short ScopedAdmin = short.MaxValue - 10;

        public ScopedRights(ScopeId scope,IEnumerable<short> rights)
        {
            scope.MustNotBeNull();
            rights.MustNotBeNull();
            Scope = scope;
            _rights = rights.ToArray();
        }

        public ScopeId Scope { get; private set; }
        /// <summary>
        /// Checks if contains any of the specified rights. Global and scoped admin rights are taken into consideration.
        /// </summary>
        /// <param name="rights"></param>
        /// <returns></returns>
        public bool HasRight(params short[] rights)
        {
            var admin = ScopedAdmin;
            if (Scope == ScopeId.Global)
            {
                admin = GlobalAdmin;
            }
            return _rights.Any(r=>rights.Any(d=>d==r) || r==admin);            
        }
    }
}