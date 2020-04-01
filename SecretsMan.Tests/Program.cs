using System;
using System.Threading.Tasks;
using TrivialTestRunner;

namespace SecretsMan.Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TRunner.AddTests<UnitTests>();
            TRunner.CrashHard = true;
            await TRunner.RunTestsAsync();
        }
    }
}
