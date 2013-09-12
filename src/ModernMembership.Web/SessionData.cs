using System;

namespace ModernMembership.Web
{
    public class SessionData
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsSliding { get; set; }
        public DateTime ExpiresOn { get; set; }
        
    }
}