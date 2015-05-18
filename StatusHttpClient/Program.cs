using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StatusHttpClient
{
    class Program
    {
        static void Main(string[] argv)
        {
            using (var tester = new EndpointTester(new HttpEndpointCheckFactory().Create("http://localhost:8080", TimeSpan.FromSeconds(3)), TimeSpan.FromSeconds(5)))
            {
                tester.EndpointStatusChanged += (sender, args) => Console.WriteLine(args.Status.Healthy);
                tester.Start();
                Console.ReadLine();
            }
            
        }
    }
}
