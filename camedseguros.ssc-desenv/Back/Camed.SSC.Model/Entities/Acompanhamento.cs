using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class Acompanhamento : EntityBase
    {
        public int Ordem { get; set; }
        public DateTime DataEHora { get; set; }
        public string Observacao { get; set; }
        public bool PermiteVisualizarAnexo { get; set; }
        public bool PermiteVisualizarObservacao { get; set; }
        public int Situacao_Id { get; set; }
        public int Solicitacao_Id { get; set; }
        public int Atendente_Id { get; set; }
        public Situacao Situacao { get; set; }
        public Usuario Atendente { get; set; }
        public Solicitacao Solicitacao { get; set; }
        public Grupo Grupo { get; set; }
        public int? TempoSLADef { get; set; }
        public string TempoSLAEfet { get; set; }
        public bool EVendaCompartilhada { get; set; }
        public bool? PermiteEmailAoSegurado { get; set; }
        public virtual ICollection<AnexoDeAcompanhamento> Anexos { get; set; }
        public virtual ICollection<VendaCompartilhada> VendasCompartilhadas { get; set; }
        public Acompanhamento()
        {
            Anexos = new HashSet<AnexoDeAcompanhamento>();
            VendasCompartilhadas = new HashSet<VendaCompartilhada>();
            PermiteVisualizarAnexo = true;
        }
    }
}
