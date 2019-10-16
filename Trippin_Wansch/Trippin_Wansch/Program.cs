using System;
using System.Threading.Tasks;

namespace Trippin_Wansch
{
    class Program
    {
        async static Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            User_Request c = new User_Request();
            c.readJson();
            await c.compareUser();
        }
    }
}
