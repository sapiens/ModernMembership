using System;
using CavemanTools;
using CavemanTools.Infrastructure;

namespace ModernMembership.Web
{
    public class MemberSessionService:IMemberSessionsService
    {
        private readonly ICacheData _cache;
        private readonly IMemberSessionStorage _storage;
        public static TimeSpan CacheDuration = TimeSpan.FromMinutes(10);


        public MemberSessionService(ICacheData cache,IMemberSessionStorage storage)
        {
            _cache = cache;
            _storage = storage;
        }

        /// <summary>
        /// If the guid is invalid (session expired or not found), it returns null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MemberSessionPrincipal Get(SessionId id)
        {
            var key = GenerateCacheKey(id);
            //var expiration = DateTimeOffset.Now.Add(CacheDuration);
            var session = _cache.Get<SessionStorageData>(key);
            if (session == null)
            {
                //get userdata
                session = _storage.Get(id);
                if (session == null)
                {
                    _cache.Remove(key);
                    return null;
                }
                _cache.Set(key, session, CacheDuration);
            }

            if (session.HasExpired())
            {
                EndSession(id);
                return null;
            }

            if (session.ShouldBeExtended())
            {
                session.ExtendDuration();
                _storage.Update(session);
            }

            return new MemberSessionPrincipal(session);
        }

        /// <summary>
        /// Returns the login session id
        /// </summary>
        /// <param name="data"></param>
        /// <param name="duration">Duration of the session</param>
        /// <param name="sliding">True to automatically extend session when it's close to the end</param>
        /// <returns></returns>
        public SessionId StartSession(MemberSessionData data, TimeSpan duration, bool sliding = true)
        {
            var id = SessionId.NewId();
            var sessionData = new SessionStorageData()
                {
                    MemberInfo = data,Duration = duration, Id = id,IsSliding = sliding,ExpiresOn = DateTime.UtcNow.Add(duration)
                };
            _storage.Add(sessionData);
            return id;
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="id"></param>
        public void EndSession(SessionId id)
        {
            _storage.Delete(id);
            _cache.Remove(GenerateCacheKey(id));
        }


        static string GenerateCacheKey(SessionId id)
        {
            return id + "-auth";
        }
    }
}