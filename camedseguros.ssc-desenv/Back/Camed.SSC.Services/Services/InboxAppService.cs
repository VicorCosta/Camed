using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Camed.SSC.Application.Services
{
    public class InboxAppService : IInboxAppService
    {
        private readonly IUnitOfWork uow;

        public InboxAppService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async void SalvarArquivos(AnexosDeInbox file_request)
        {
            /*var file_return = new AnexoDeInboxViewModel();
            file_return.Nome = file_request.Nome;
            file_return.Inbox_Id = file_request.Inbox.Id;
            file_return.Caminho = file_request.Caminho;
            var caminho_raiz = await uow.GetRepository<ParametrosSistema>().QueryFirstOrDefaultAsync(w => w.Parametro == "CAMINHORAIZANEXOS");
            using (FileStream fs = new FileStream($"{caminho_raiz}{ file_return.Caminho }", FileMode.Open))
            {
                //fs.
            }*/
        }

        public void SalvarArquivos(List<AnexoDeInboxViewModel> request, int ids_Inboxs)
        {
            throw new NotImplementedException();
        }
    }
}
