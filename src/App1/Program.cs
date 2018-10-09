using Lib1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Hello from {typeof(Program).FullName}: {string.Join(" ", args)}");
            Console.WriteLine(new Class1().Foo);
        }
    }
}
