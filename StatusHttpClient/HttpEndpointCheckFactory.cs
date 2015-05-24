using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StatusHttpClient
{
    public class HttpEndpointCheckFactory
    {
        public Func<Status> Create(string url, TimeSpan timeout)
        {
            return () =>
            {
                var httpClient = new HttpClient();
                var httpTask
                    = httpClient.GetAsync(url);

                bool success = httpTask.Wait(timeout);
                if (success)
                {
                    return new Status(healthy: true, message: url + " up & running");
                }
                return new Status(healthy: false, message: url + " down");
            };
        }
    }
}
