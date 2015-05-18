using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StatusHttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server
                = new TinyHttpServer(
                    "http://localhost:8080/",
                    context =>
                    {
                        HttpListenerRequest request = context.Request;
                        // Obtain a response object.
                        HttpListenerResponse response = context.Response;
                        // Construct a response.
                        string responseString = "<HTML><BODY> Hello world!" + DateTime.Now + "</BODY></HTML>";
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        // Get a response stream and write the response to it.
                        response.ContentLength64 = buffer.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        // You must close the output stream.
                        output.Close();
                    }
                );
            
            server.Start();
            Console.ReadLine();
        }

    }
}