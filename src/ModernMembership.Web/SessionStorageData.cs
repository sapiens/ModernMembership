using System;

namespace ModernMembership.Web
{
    public class SessionStorageData : SessionData
    {
        public MemberSessionData MemberInfo { get; set; }

        public bool HasExpired()
        {
            return DateTime.UtcNow > ExpiresOn;
        }

        public bool ShouldBeExtended()
        {
            return Sliding && (ExpiresOn.Subtract(DateTime.UtcNow) < Duration.Multiply(.5f));
        }

        public void ExtendDuration()
        {
            ExpiresOn = ExpiresOn.Add(Duration);
        }
    }
}