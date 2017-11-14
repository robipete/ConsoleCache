using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

        internal void AgeCache(string key) // needed this for fast reordering on GETs, key passed in meant to be the first but works in general
        {
            // OK this would/could be more efficient if I could just peel off the last n - 1 elements of the underlying structure as a dictionary!
            //cache = cache.Skip<Dictionary<string, string>>(1);
            //cache = (IEnumerable<Dictionary<string, string>>)cache.TakeLast<Dictionary<string,string>>(cache.Count - 1);
            cache = cache.Where(s => s.Key != key).ToDictionary(s => s.Key, s => s.Value); // excising the first element, a delete by key just leaves the slot open in the dictionary and messes up our ordering with the next add
           // cache.Add(key, value); // drop newly gotten entry to the end
        }

        internal void AddCache(string key, string value) // needed this for fast reordering on GETs, key passed in meant to be the first but works in general
        {
            // OK this would/could be more efficient if I could just peel off the last n - 1 elements of the underlying structure as a dictionary!
            //cache = cache.Skip<Dictionary<string, string>>(1);
            //cache = (IEnumerable<Dictionary<string, string>>)cache.TakeLast<Dictionary<string,string>>(cache.Count - 1);
            //cache = cache.Where(s => s.Key != key).ToDictionary(s => s.Key, s => s.Value); // excising the first element, a delete by key just leaves the slot open in the dictionary and messes up our ordering with the next add
            cache.Add(key, value); // drop newly gotten entry to the end
        }
    }
}
