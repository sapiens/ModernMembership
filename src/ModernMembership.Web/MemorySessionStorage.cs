using System;
using System.Collections.Generic;

namespace ModernMembership.Web
{
    public class MemorySessionStorage:IMemberSessionStorage
    {
        private static object sync = new Object();

        static Dictionary<Guid,SessionStorageData> _list=new Dictionary<Guid, SessionStorageData>();
        public void Add(SessionStorageData data)
        {
            lock (sync)
            {
                _list.Add(data.Id,data);
            }            
        }

        public SessionStorageData Get(Guid id)
        {
            SessionStorageData result=null;
            _list.TryGetValue(id, out result);
            return result;
        }

        public void Update(SessionStorageData data)
        {
            
        }

        public void Delete(Guid id)
        {
            lock (sync)
            {
                _list.Remove(id);
            }
        }
    }
}