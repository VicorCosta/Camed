using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Diagnostics;
using Camed.SSC.Application.ViewModel;

namespace Camed.SSC.Application.Requests.Inbox.Commands.SalvarArquivo
{
    public class FormatarArquivoInboxCommandHandler : HandlerBase, IRequestHandler<FormatarArquivoInboxCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public FormatarArquivoInboxCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(FormatarArquivoInboxCommand request, CancellationToken cancellationToken)
        {
            var files = new List<AnexoDeInboxViewModel>();
            foreach (var file in request.FormFiles)
            {
                var command = new AnexoDeInboxViewModel();
                command.Extensao = new FileInfo(file.FileName).Extension;
                command.Nome = new FileInfo(file.FileName).Name.Replace(command.Extensao, "");
                BinaryReader b = new BinaryReader(file.OpenReadStream());              
                byte[] binData = b.ReadBytes(file.Length.GetHashCode());
                command.Arquivo = binData;
                files.Add(command);
             
            }
            result.Payload = files;
            return await Task.FromResult(result);
        }
    }
}
