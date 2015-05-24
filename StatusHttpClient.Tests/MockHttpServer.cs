using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using StatusHttpServer;

namespace StatusHttpClient.Tests
{
    public class MockHttpServer : TinyHttpServer
    {
        private readonly string _urlPrefix;

        private MockHttpServer(string urlPrefix, Action<HttpListenerContext> httpHandler) : base(urlPrefix, httpHandler)
        {
            _urlPrefix = urlPrefix;
        }

        public string UrlPrefix
        {
            get { return _urlPrefix; }
        }

        public static MockHttpServer Run(Action<HttpListenerContext> httpHandler)
        {
            string url = "http://localhost:" + FreeTcpPort() + "/";
            var server = new MockHttpServer(url, httpHandler);
            server.Start();
            return server;
        }

        // see http://stackoverflow.com/questions/138043/find-the-next-tcp-port-in-net
        static int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}
