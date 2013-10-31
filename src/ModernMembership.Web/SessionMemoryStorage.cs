using System;
using System.Collections.Generic;
using CavemanTools;

namespace ModernMembership.Web
{
    public class SessionMemoryStorage:IMemberSessionStorage
    {
        private static object sync = new Object();

        static Dictionary<SessionId,SessionStorageData> _list=new Dictionary<SessionId, SessionStorageData>();
        public void Add(SessionStorageData data)
        {
            lock (sync)
            {
                _list.Add(data.Id,data);
            }            
        }

        public SessionStorageData Get(SessionId id)
        {
            SessionStorageData result=null;
            _list.TryGetValue(id, out result);
            return result;
        }

        public void Update(SessionStorageData data)
        {
            lock (sync)
            {
                _list[data.Id] = data;
            }
        }

        public void Delete(SessionId id)
        {
            lock (sync)
            {
                _list.Remove(id);
            }
        }
    }
}