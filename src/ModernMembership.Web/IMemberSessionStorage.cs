using System;

namespace ModernMembership.Web
{
    public interface IMemberSessionStorage
    {
        void Add(SessionStorageData data);
        SessionStorageData Get(Guid id);
        void Update(SessionStorageData data);
        void Delete(Guid id);
    }
}