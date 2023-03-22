using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Interfaces
{
    public interface IUsuarioAppService
    {
        Usuario RetornarAtendente(Solicitacao solicitacao);
        List<Grupo> ObterGrupos(string login);
    }
}
