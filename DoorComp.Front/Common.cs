using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;

namespace DoorComp.Front
{
    public class Common
    {
        private static ObjectCache cache = MemoryCache.Default;
        private const int cacheSeconds = 240;
        private static object cacheLock = new object();

        public static void ExpireDoor(string DoorID)
        {
            string key = string.Format("Door:{0}", DoorID);
            if (cache.Contains(key))
            {
                lock(cacheLock)
                {
                    if (cache.Contains(key))
                        cache.Remove(key);
                }
            }

        }
    }
}