using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILoggerFactory _loggerFactory;

        public RequestLogger(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var logger = _loggerFactory.CreateLogger<TRequest>();
            logger.LogInformation(DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss.fff"));
            return Task.CompletedTask;
        }
    }
}
