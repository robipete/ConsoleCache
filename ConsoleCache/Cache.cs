using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCache
{
    internal static class Cache
    {
        internal static void Add( string key, string value, CacheModel cm )
        {
            if (cm == null)
                throw new InvalidProgramException("Missing Cache Model param in Cache class."); // this should never happen

            if (!cm.Cache.ContainsKey(key) && cm.CacheMaxSize <= cm.CacheCurrSize) // if key is already there we're just doing an update else if not there and size overflow we remove least used
            {
                var e = cm.Cache.GetEnumerator();
                e.MoveNext(); // move first

                cm.Cache.Remove(e.Current.Key); //overflow, remove oldest which is first by nature. Not really guarenteed but pretty solid.
                
            }
            cm.Cache[key] = value; // update or add new

        }

        internal static string Get(string key, CacheModel cm)
        {
            if (cm == null)
                throw new InvalidProgramException("Missing Cache Model param in Cache class."); // this should never happen

            if (cm.Cache.TryGetValue(key, out string value))
            {
                // here we note the fact the key was looked up and move it to the bottom of the dictionary
                cm.Cache.Remove(key);
                cm.Cache.Add(key, value);
                return value;
            }
            else
                throw new Exception("Key not found.");
        }

        internal static Dictionary<string, string>.Enumerator GetAll(CacheModel cm)
        {
            if (cm == null)
                throw new InvalidProgramException("Missing Cache Model param in Cache class."); // this should never happen
            return cm.Cache.GetEnumerator();

        }
    }
}

