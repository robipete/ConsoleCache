using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCache
{
    internal class CacheService
    {
        /*
        The Dictionary generic class provides a mapping from a set of keys to a set of values. 
        Each addition to the dictionary consists of a value and its associated key. 
        Retrieving a value by using its key is very fast, close to O(1) because the Dictionary class is implemented as a hash table.

            And for the Add or Remove function:

        If Count is less than the capacity, this method approaches an O(1) operation. Remove by key is O(1).
        If the capacity must be increased to accommodate the new element, this method becomes an O(n) operation, where n is Count.
         */
        CacheModel cm = null;
        private int maxParams = 5; // ok that is an 
        private int minParams = 1; // ok that is an 
        const string getall = "GETALL";
        const string get = "GET";
        const string getadd = "ADD";
        const string gethelp = "HELP";
        private string[] verbs = { getall, get, getadd, gethelp };
        internal string Initialize(string input)
        {

            int size;
            if (int.TryParse(input, out size))
            {
                cm = new CacheModel(size); // parsable size as int
                return "OK";
            }
            else
                throw new InvalidOperationException("Must specify cache size as an int before continuing. No spaces. One number."); // bad input

        }

        internal string ProcessCommand(string input)
        {

            if (cm == null)
                throw new InvalidOperationException("Cache not properly initialized."); // it should be impossible to hit this but... just in case

            var command = input.Split(' ');

            if (command.Length >= minParams && command.Length <= maxParams) // ok that is rather arbitrary but suppse someone types a paragraph?
                switch (command[0])
                {
                    case getall:
                        return command.Length == 1 ? DoGetAll() : throw new Exception("Invalid number of arguements."); // length should be passed in, this is cheating
                    case get:
                        return command.Length == 2 ? DoGet(command[1]) : throw new Exception("Invalid number of arguements.");
                    case getadd:
                        return command.Length == 3 ? DoAdd(command[1], command[2]) : throw new Exception("Invalid number of arguements.");
                    case gethelp:
                        return command.Length == 1 ? DoHelp() : throw new Exception("Invalid number of arguements.");


                    case "OK":
                        return command.Length == 1 ? DoOk() : throw new Exception("Invalid number of arguements.");



                    default:
                        throw new InvalidOperationException("Invalid verb");
                }
            else
                throw new InvalidOperationException("Invalid number of operands.");

        }

        private string DoOk()
        {
            return "Hey that's my line!";
        }

        private string DoHelp()
        {
            // could I do some reflection magic to dynamically make this list?
            foreach (string s in verbs)
                CacheController.CacheWriteLn(s);
            return "OK";
        }

        private string DoAdd(string k, string v)
        {
            Cache.Add(k, v, cm);
            return "OK";
        }

        private string DoGet(string k)
        {
            return Cache.Get(k, cm);
        }
        private string DoGetAll()
        {
            var e = Cache.GetAll(cm);
            while (e.MoveNext())
            {
                Console.Write(e.Current.Key);
                Console.Write(" : ");
                Console.WriteLine(e.Current.Value);
            }

            return "OK";
        }
    }
}
