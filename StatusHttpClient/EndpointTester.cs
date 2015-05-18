using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StatusHttpClient
{
    public class EndpointTester : IDisposable
    {

        public event EventHandler<StatusEventArgs> EndpointStatusChanged;

        private readonly Func<Status> _endpointCheck;
        private readonly TimeSpan _delay; 

        private volatile Status _currentStatus;

        private Timer _timer;

        public EndpointTester(Func<Status> endpointCheck, TimeSpan delay)
        {
            this._endpointCheck = endpointCheck;
            this._delay = delay;
        }


        public void Start()
        {
            _timer = new Timer(_ =>
            {
                try
                {
                    Status status = _endpointCheck();
                    HandleNewEndpointStatus(status);
                }
                catch (Exception ex)
                {
                    HandleNewEndpointStatus(
                        new Status(
                            healthy: false, 
                            message: ex.Message
                        )
                    );
                }
            }, null, _delay, _delay);
        }

        private void HandleNewEndpointStatus(Status status)
        {
            if (_currentStatus == null || _currentStatus.Healthy != status.Healthy)
            {
                _currentStatus = status;
                EndpointStatusChanged(this, new StatusEventArgs(status));
            }
            else
            {
                _currentStatus = status;
            }
        }

        public Status CurrentStatus
        {
            get { return _currentStatus; }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

    }

    public class StatusEventArgs : EventArgs
    {
        private Status _status;


        public StatusEventArgs(Status status)
        {
            _status = status;
        }

        public Status Status
        {
            get { return _status; }
        }
    }

    public class Status
    {
        private bool _healthy;
        private string _message;

        public Status(bool healthy, string message)
        {
            _healthy = healthy;
            _message = message;
        }

        public string Message
        {
            get { return _message; }
        }


        public bool Healthy
        {
            get { return _healthy; }
        }
    }
}
