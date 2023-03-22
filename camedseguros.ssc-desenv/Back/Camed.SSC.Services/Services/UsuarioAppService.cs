using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IUnitOfWork uow;
        private readonly IAreaDeNegocioAppService areaDeNegocioAppService;

        public UsuarioAppService(IAreaDeNegocioAppService areaDeNegocioAppService, IUnitOfWork uow)
        {
            this.areaDeNegocioAppService = areaDeNegocioAppService;
            this.uow = uow;
        }

        public Usuario RetornarAtendente(Solicitacao solicitacao)
        {
            var grupoAgencia_Id = uow.GetRepository<TipoDeSeguro>().QueryFirstOrDefaultAsync(x => x.Id == solicitacao.TipoDeSeguro.Id,
                includes: new[] { "GrupoAgencia" }).Result.GrupoAgencia.Id;

            var areaDeNegocio = areaDeNegocioAppService.RetornarAreaNegocio(solicitacao);

            var mapeamentoAtendente = uow.GetRepository<MapeamentoAtendente>().QueryFirstOrDefaultAsync(x => x.GrupoAgencia.Id == grupoAgencia_Id
                    && x.TipoDeSeguro.Id == solicitacao.TipoDeSeguro.Id
                    && x.AreaDeNegocio.Id == areaDeNegocio.Id
                    && x.Agencia.Id == solicitacao.Agencia.Id, includes: new[] { "Atendente" }).Result;

            if (mapeamentoAtendente == null || mapeamentoAtendente.Atendente == null)
                return null;
            else
                return mapeamentoAtendente.Atendente;
        }

        public List<Grupo> ObterGrupos(string login)
        {
            if (string.IsNullOrEmpty(login))
                throw new ArgumentException("O login do usuário deve ser informado.", "login");

            //var usuario = uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(x => x.Login == login, 
            //    includes: new[] { "Grupo", "Grupo.SubGrupos" }).Result;

            var usuario = uow.GetRepository<Usuario>()
                                .QueryFirstOrDefaultAsync(x => x.Login == login,
                                        includes: new[] { "Agencia", "Empresa", "AreasDeNegocio",
                                            "AreasDeNegocio.AreaDeNegocio", "Grupos", "Grupos.Grupo",
                                            "GruposAgencias", "GruposAgencias.GrupoAgencia" }).Result;

            if (usuario == null)
                throw new ApplicationException(string.Format("Usuário com o login '{0}' não foi encontrado.", login));

            var gruposDoUsuario = new List<Grupo>();

            foreach (var g in usuario.Grupos)
            {
                gruposDoUsuario.Add(g.Grupo);

                foreach (var s in g.Grupo.SubGrupos)
                {
                    if (!gruposDoUsuario.Exists(x => x.Id == s.Subgrupo.Id))
                    {
                        gruposDoUsuario.Add(s.Subgrupo);
                    }

                    if (s.Subgrupo.SubGrupos.Count > 0)
                    {
                        foreach (var s1 in s.Subgrupo.SubGrupos)
                        {
                            if (!gruposDoUsuario.Exists(x => x.Id == s1.Subgrupo.Id))
                            {
                                gruposDoUsuario.Add(s1.Subgrupo);
                            }

                            if (s1.Subgrupo.SubGrupos.Count > 0)
                            {
                                foreach (var s2 in s1.Subgrupo.SubGrupos)
                                {
                                    if (!gruposDoUsuario.Exists(x => x.Id == s2.Subgrupo.Id))
                                    {
                                        gruposDoUsuario.Add(s2.Subgrupo);
                                    }

                                    if (s2.Subgrupo.SubGrupos.Count > 0)
                                    {
                                        foreach (var s3 in s2.Subgrupo.SubGrupos)
                                        {
                                            if (!gruposDoUsuario.Exists(x => x.Id == s3.Subgrupo.Id))
                                            {
                                                gruposDoUsuario.Add(s3.Subgrupo);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return gruposDoUsuario;
        }
    }
}
