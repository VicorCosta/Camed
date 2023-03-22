using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Camed.SSC.Application
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Matricula { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public Empresa Empresa { get; set; }
        public ICollection<Grupo> Grupos { get; set; }
        public ICollection<AreaDeNegocio> AreasDeNegocio { get; set; }
        public bool AlterarSenha { get; set; }
        public bool PermitidoGerarCotacao { get; set; }
        public bool PodeVisualizarObservacao { get; set; }
        public string Token { get; set; }
        public bool EhCalculista { get; set; }
        public string Cpf { get; set; }
       
    }
}
