using System;
using CavemanTools;

namespace ModernMembership.Web
{
    public class SessionData
    {
        public SessionId Id { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsSliding { get; set; }
        public DateTime ExpiresOn { get; set; }
        
    }
}