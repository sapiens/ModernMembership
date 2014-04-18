using System;
using System.Collections.Generic;
using System.Linq;

namespace ModernMembership.Authorization
{
    public class ScopedRights
    {
        private readonly IEnumerable<int> _rights=new int[0];

        /// <summary>
        /// Should be asigned only to global scope groups. Value can be changed
        /// </summary>
        public static int GlobalAdmin = int.MaxValue - 5;
        /// <summary>
        /// Should be asigned only to scoped groups. Value can be changed
        /// </summary>
        public static int ScopedAdmin = int.MaxValue - 10;

        private bool _isGlobalAdmin;

        public ScopedRights(ScopeId scope,IEnumerable<int> rights)
        {
            scope.MustNotBeNull();
            rights.MustNotBeNull();
            Scope = scope;
            _rights = rights.ToArray();
            _isGlobalAdmin = rights.Any(r => r == GlobalAdmin);
        }

        public ScopeId Scope { get; private set; }
        /// <summary>
        /// Checks if contains any of the specified rights. Global and scoped admin rights are taken into consideration.
        /// </summary>
        /// <param name="rights"></param>
        /// <returns></returns>
        public bool HasRight(params int[] rights)
        {
            if (_isGlobalAdmin) return true;
            var admin = ScopedAdmin;
            if (Scope == ScopeId.Global)
            {
                admin = GlobalAdmin;
            }
            return _rights.Any(r=>rights.Any(d=>d==r) || r==admin);            
        }
    }
}