using Camed.SSC.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Camed.SSC.Domain.Entities
{
    public class Usuario : EntityBase
    {
        public string Matricula { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public Empresa Empresa { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
        public string Senha { get; set; }
        public Agencia Agencia { get; set; }
        public bool SenhaETemporaria { get; set; }
        public bool EnviarSLA { get; set; }
        public bool EhCalculista { get; set; }

        public ICollection<UsuarioAreaDeNegocio> AreasDeNegocio { get; set; }
        public ICollection<UsuarioGrupo> Grupos { get; set; }
        public ICollection<UsuarioGrupoAgencia> GruposAgencias { get; set; }
        public ICollection<AlteracaoSenhaUsuario> AlteracaoDeSenhaUsuario { get; set; }

        public Usuario()
        {
            AlteracaoDeSenhaUsuario = new HashSet<AlteracaoSenhaUsuario>();
            AreasDeNegocio = new HashSet<UsuarioAreaDeNegocio>();
            Grupos = new HashSet<UsuarioGrupo>();
            GruposAgencias = new HashSet<UsuarioGrupoAgencia>();
            Ativo = true;
            SenhaETemporaria = true;
        }

        public bool EhSolicitante
        {
            get
            {
                return this.Grupos.Any(x => x.Grupo.Nome.ToLower().Trim() == "solicitante");
            }
        }

        public bool EhAtendente
        {
            get
            {
                return this.Grupos.Any(x => x.Grupo.Nome.ToLower().Trim() == "atendente");
            }
        }

        public bool EhAgrosul
        {
            get
            {
                return this.Login?.ToLower()?.Contains("agrosul") == true;
            }
        }

        public bool PodeVisualizarObservacoes
        {
            get
            {
                return this.Grupos.Any(x => x.Grupo.SempreVisualizarObservacao);
            }
        }

        public bool PermitidoGerarCotacao
        {
            get
            {
                var a1 = this.Id;
                var b1 = this.Grupos;
                return this.Grupos.Any(a => a.Grupo.Acoes.Any(s => s.Acao.Papel.Nome == 
                "SolicitacaoDeCotacao" && s.Acao.Descricao == "Cadastrar"));
            }
        }
    }
}
