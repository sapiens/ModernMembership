﻿using System;

namespace ModernMembership.Web
{
    public interface IMemberSessionsService
    {
        /// <summary>
        /// If the guid is invalid (session expired or not found), it returns null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CavemanMemberSession Get(Guid id);

        /// <summary>
        /// Returns the login session id
        /// </summary>
        /// <param name="data"></param>
        /// <param name="duration">Duration of the session</param>
        /// <param name="sliding">True to automatically extend session when it's close to the end</param>
        /// <returns></returns>
        Guid StartSession(MemberSessionData data, TimeSpan duration,bool sliding=true);
        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="id"></param>
        void EndSession(Guid id);
    }
}