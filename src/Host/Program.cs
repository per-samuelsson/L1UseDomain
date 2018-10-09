using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class Program
    {
        static int Main(string[] args)
        {
            var hostAssembly = typeof(Program).Assembly;
            var runtimePath = Path.GetDirectoryName(hostAssembly.Location);
            var cacheDir = Path.Combine(runtimePath, ".cache");

            Directory.CreateDirectory(cacheDir);

            var domainSetup = new AppDomainSetup()
            {
                ApplicationName = "Host",
                ShadowCopyFiles = "true",
                CachePath = cacheDir 
            };

            var app1 = @"..\..\..\App1\bin\Debug\App1.exe";
            var app1Path = Path.GetFullPath(Path.Combine(runtimePath, app1));

            if (!File.Exists(app1Path))
            {
                return 1;
            }

            var app2 = @"..\..\..\App2\bin\Debug\App2.exe";
            var app2Path = Path.GetFullPath(Path.Combine(runtimePath, app2));

            if (!File.Exists(app2Path))
            {
                return 1;
            }

            var domain = AppDomain.CreateDomain("HostDomain", hostAssembly.Evidence, domainSetup);

            var appArgs = new [] { "These", "are", "args" };

            foreach (var app in new[] { app1Path, app2Path })
            {
                ExecuteDomain(app, appArgs, domain);
                // LoadFile(app, appArgs);
            }

            Console.Write("ENTER to exit...");
            Console.ReadLine();
            
            return 0;
        }

        static void ExecuteDomain(string appPath, string[] args, AppDomain domain)
        {
            domain.ExecuteAssembly(appPath, args);
        }

        private delegate void main1();
        private delegate void main2(string[] p);
        private delegate int main3();
        private delegate int main4(string[] p);

        static void LoadFile(string appPath, string[] args)
        {
            var assembly = Assembly.LoadFile(appPath);

            var entrypoint = assembly.EntryPoint;
            if (entrypoint.GetParameters().Length == 0)
            {
                if (entrypoint.ReturnType == typeof(int))
                {
                    main3 entry = (main3)entrypoint.CreateDelegate(typeof(main3));
                    entry();
                }
                else
                {
                    main1 entry = (main1)entrypoint.CreateDelegate(typeof(main1));
                    entry();
                }
            }
            else
            {
                var arguments = args;

                if (entrypoint.ReturnType == typeof(int))
                {
                    main4 entry = (main4)entrypoint.CreateDelegate(typeof(main4));
                    entry(arguments);
                }
                else
                {
                    main2 entry = (main2)entrypoint.CreateDelegate(typeof(main2));
                    entry(arguments);
                }
            }
        }
    }
}
