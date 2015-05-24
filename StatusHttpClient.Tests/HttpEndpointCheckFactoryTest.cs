using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using NUnit.Framework;

namespace StatusHttpClient.Tests
{
    [TestFixture]
    public class HttpEndpointCheckFactoryTest
    {
        private MockHttpServer _server;

        [Test]
        public void Should_provide_an_healthy_status_when_checking_a_running_server()
        {
            // given
            _server = MockHttpServer.Run(context =>
            {
                HttpListenerResponse response = context.Response;
                System.IO.Stream output = response.OutputStream;
                output.Close();
            });

            // when
            var check = new HttpEndpointCheckFactory().Create(_server.UrlPrefix, TimeSpan.FromSeconds(3));
            var status = check.Invoke();
            
            // then
            Check.That(status).IsNotNull();
            Check.That(status.Healthy).IsTrue();

        }

        [Test]
        public void Should_provide_an_unhealthy_status_when_server_returns_an_error()
        {
            // given
            _server = MockHttpServer.Run(context =>
            {
                HttpListenerResponse response = context.Response;
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
            });

            // when
            var check = new HttpEndpointCheckFactory().Create(_server.UrlPrefix, TimeSpan.FromSeconds(3));
            var status = check.Invoke();

            // then
            Check.That(status).IsNotNull();
            Check.That(status.Healthy).IsFalse();

        }

        [Test]
        public void Should_provide_an_unhealthy_status_when_there_is_no_server()
        {
            // given
            _server = MockHttpServer.Run(context =>
            {
                HttpListenerResponse response = context.Response;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            });
            _server.Stop();

            // when
            var check = new HttpEndpointCheckFactory().Create(_server.UrlPrefix, TimeSpan.FromSeconds(3));
            var status = check.Invoke();

            // then
            Check.That(status).IsNotNull();
            Check.That(status.Healthy).IsFalse();

        }

        [TearDown]
        public void ShutdownServer()
        {
            if (_server != null)
            {
                _server.Stop();
            }
        }
    }
}
