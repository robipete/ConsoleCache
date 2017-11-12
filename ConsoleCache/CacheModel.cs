using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCache
{
    internal class CacheModel
    {
        /*
        The Dictionary generic class provides a mapping from a set of keys to a set of values. 
        Each addition to the dictionary consists of a value and its associated key. 
        Retrieving a value by using its key is very fast, close to O(1), because the Dictionary class is implemented as a hash table.

            And for the Add or Remove function:

        If Count is less than the capacity, this method approaches an O(1) operation. Remove by key is O(1).
        If the capacity must be increased to accommodate the new element, this method becomes an O(n) operation, where n is Count.
         */
        private Dictionary<string, string> cache;
        private int cacheMaxSize;
        internal int CacheMaxSize => cacheMaxSize;
        internal int CacheCurrSize => cache != null ? cache.Count : -1; // only hit the count if we're instantiated
        internal Dictionary<string, string> Cache => cache;

        internal CacheModel()
        {
            cache = CacheModelFactory(10); // default the size to 10
        }

        internal CacheModel(int size)
        {
            cache = CacheModelFactory(size); // set desired size 
        }

        private Dictionary<string, string> CacheModelFactory(int size)
        {
            var c = new Dictionary<string, string>(size);
            cacheMaxSize = size;
            return c;
        }

    }
}
