using System;
using System.Net;
using System.Threading.Tasks;

namespace StatusHttpServer
{
    public class TinyHttpServer
    {
        private readonly Action<HttpListenerContext> _httpHandler;
        private HttpListener _listener;

        public TinyHttpServer(string urlPrefix, Action<HttpListenerContext> httpHandler)
        {
            _httpHandler = httpHandler;
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            // Create a listener.
            _listener = new HttpListener();
            _listener.Prefixes.Add(urlPrefix);
        }

        public void Start()
        {
            _listener.Start();
            Task.Run(async () =>
            {
                using (_listener)
                {
                    while (true)
                    {
                        HttpListenerContext context = await _listener.GetContextAsync();
                        _httpHandler.Invoke(context);
                    }
                }
            });
        }
    }
}