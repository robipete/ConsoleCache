using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCache
{
    internal static class CacheController
    {
        internal static void CacheCommandLoop()
        {
            var cs = new CacheService();
            // first input is 'special'
            bool getSizeFirst = true;
            string input;
            Console.WriteLine("Size?");
            while ((input = Console.ReadLine()).ToLower() != "exit")
            {
                try
                {
                    if (getSizeFirst)
                    {
                        Console.WriteLine(cs.Initialize(input));
                        getSizeFirst = false;  // OK got size first, lets move on
                    }
                    else
                        Console.WriteLine(cs.ProcessCommand(input));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
        }

        // Maybe the output might change, good to centralize
        internal static void CacheWriteLn(string s)
        {
            Console.WriteLine(s);
        }
        internal static void CacheWrite(string s)
        {
            Console.Write(s);
        }

    }
}
