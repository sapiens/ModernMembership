using System;
using CavemanTools;

namespace ModernMembership.Web
{
    public interface IMemberSessionStorage
    {
        void Add(SessionStorageData data);
        SessionStorageData Get(SessionId id);
        void Update(SessionStorageData data);
        void Delete(SessionId id);
    }
}