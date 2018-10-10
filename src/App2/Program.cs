using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Hello from {typeof(Program).FullName}, in domain {AppDomain.CurrentDomain.FriendlyName}: {string.Join(" ", args)}. Sleeping...");
            Thread.Sleep(1000 * 2);
        }
    }
}
