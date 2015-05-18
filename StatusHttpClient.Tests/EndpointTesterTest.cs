using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace StatusHttpClient
{
    
    [TestFixture]
    public class EndpointTesterTest
    {
        
        [Test]
        public async void Should_send_one_unhealthy_event_when_endpoint_check_fails()
        {
            using (var tester = new EndpointTester(() => new Status(healthy: false, message: "whatever"), TimeSpan.FromMilliseconds(1))) { 
                StatusEventArgs eventFired = null;
                tester.EndpointStatusChanged += (sender, args) => eventFired = args;
                tester.Start();
                await Task.Delay(50);
                Assert.IsNotNull(eventFired);
                Assert.IsFalse(eventFired.Status.Healthy);
            }
        }

        [Test]
        public async void Should_send_one_unhealthy_event_when_endpoint_check_throws_an_exception()
        {
            using (var tester = new EndpointTester(() => { throw new Exception(); }, TimeSpan.FromMilliseconds(1)))
            {
                StatusEventArgs eventFired = null;
                tester.EndpointStatusChanged += (sender, args) => eventFired = args;
                tester.Start();
                await Task.Delay(50);
                Assert.IsNotNull(eventFired);
                Assert.IsFalse(eventFired.Status.Healthy);
            }
        }

        [Test]
        public async void Should_send_one_healthy_event_when_endpoint_check_pass()
        {
            using (var tester = new EndpointTester(() => new Status(healthy: true, message: "whatever"), TimeSpan.FromMilliseconds(1)))
            {
                StatusEventArgs eventFired = null;
                tester.EndpointStatusChanged += (sender, args) => eventFired = args;
                tester.Start();
                await Task.Delay(50);
                Assert.IsNotNull(eventFired);
                Assert.IsTrue(eventFired.Status.Healthy);
            }
        }

    }
}
