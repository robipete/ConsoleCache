using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ConsoleCache
{
    internal static class CacheWorker
    {
        private static string mcmmsg = "Missing Cache Model param in Cache class.";

        internal static void Add( string key, string value, CacheModel cm )
        {


            if (cm == null)
                throw new InvalidProgramException(mcmmsg); // this should never happen

            if (!cm.Cache.ContainsKey(key) && cm.CacheMaxSize <= cm.CacheCurrSize) // if key is already there we're just doing an update else if not there and size overflow we remove least used
            {
                var e = cm.Cache.GetEnumerator();
                e.MoveNext(); // move first
                cm.AgeCache(e.Current.Key);
                cm.AddCache(key, value); // add new forced at end
                               
            }
            else
                cm.Cache[key] = value; // update or add new, update not considered as a cache GET so don't reorder. Case is rapidly changing values rarely accessed?

        }

        internal static string Get(string key, CacheModel cm)
        {
            if (cm == null)
                throw new InvalidProgramException(mcmmsg); // this should never happen

            if (cm.Cache.TryGetValue(key, out string value))
            {
                // here we note the fact the key was looked up and we move it to the bottom of the dictionary
                cm.AgeCache(key);
                cm.AddCache(key, value); // re-add existing KV forced new at end
                return value;
            }
            else
                throw new Exception("NOTFOUND");
        }

        internal static Dictionary<string, string>.Enumerator GetAll(CacheModel cm)
        {
            if (cm == null)
                throw new InvalidProgramException(mcmmsg); // this should never happen
            return cm.Cache.GetEnumerator();

        }
    }
}

