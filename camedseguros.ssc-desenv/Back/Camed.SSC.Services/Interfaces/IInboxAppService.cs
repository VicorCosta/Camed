using Camed.SSC.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Interfaces
{
    public interface IInboxAppService
    {
        void SalvarArquivos(List<AnexoDeInboxViewModel> request, int ids_Inboxs);
    }
}
