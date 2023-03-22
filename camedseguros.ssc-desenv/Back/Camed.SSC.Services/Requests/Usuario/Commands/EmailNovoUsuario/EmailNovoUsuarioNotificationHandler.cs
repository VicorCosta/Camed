using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class EmailNovoUsuarioNotificationHandler : INotificationHandler<EmailNovoUsuarioNotification>
    {
        private readonly IUnitOfWork uow;

        public EmailNovoUsuarioNotificationHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task Handle(EmailNovoUsuarioNotification request, CancellationToken cancellationToken)
        {
            //return Task.FromResult(Unit.Value);
        }
    }
}
