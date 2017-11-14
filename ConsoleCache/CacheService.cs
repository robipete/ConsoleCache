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
        private const char space = ' ';
        private const string size = "SIZE";
        private const string ok = "OK";
        private const string getall = "GETALL";
        private const string get = "GET";
        private const string set = "SET";
        private const string gethelp = "HELP";
        private const string exit = "EXIT";
        private string[] verbs = { getall, get, set, gethelp, exit }; // list of available commands
        const string errmsg = "ERROR";

        internal string Initialize(string input)
        {
            var command = input.Split(space); 
            if (command.Length == 2 && command[0].Equals(size) && int.TryParse(command[1], out int x))
            {
                cm = new CacheModel(x); // parsable size as int
                return size + " " + ok;
            }
            else
                throw new InvalidOperationException(errmsg); // bad input

        }

        internal string ProcessCommand(string input)
        {

            if (cm == null)
                throw new InvalidOperationException("Cache not properly initialized."); // it should be impossible to hit this but... just in case

            var command = input.Split(space);

            if (command.Length >= minParams && command.Length <= maxParams) // ok that is rather arbitrary but suppose someone types a paragraph?
                switch (command[0])
                {
                    case getall:    // getall useful for debugging! could disable for release builds...
                        return command.Length == 1 ? DoGetAll() : throw new Exception( errmsg ); // TODO: length should be passed into this method, this is cheating
                    case get:
                        return command.Length == 2 ? DoGet(command[1]) : throw new Exception( errmsg );
                    case set:
                        return command.Length == 3 ? DoAdd(command[1], command[2]) : throw new Exception( errmsg );


                    case gethelp:
                        return command.Length == 1 ? DoHelp() : throw new Exception( errmsg ); // open for expansion here...


                    case ok:
                        return command.Length == 1 ? DoOk() : throw new Exception( errmsg );



                    default:
                        throw new InvalidOperationException("Invalid verb. Type 'HELP' for a list of valid commands.");
                }
            else
                throw new Exception( errmsg );

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
            return ok;
        }

        private string DoAdd(string k, string v)
        {
            CacheWorker.Add(k, v, cm);
            return set + space + ok;
        }

        private string DoGet(string k)
        {
            return "GOT" + space + CacheWorker.Get(k, cm);
        }

        private string DoGetAll()
        {
            var e = CacheWorker.GetAll(cm);
            while (e.MoveNext())
            {
                CacheController.CacheWrite(e.Current.Key);
                CacheController.CacheWrite(" : ");
                CacheController.CacheWriteLn(e.Current.Value);
            }

            return ok;
        }
    }
}
