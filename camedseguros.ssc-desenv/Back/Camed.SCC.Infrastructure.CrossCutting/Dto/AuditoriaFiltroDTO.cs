using System;

namespace Camed.SCC.Infrastructure.CrossCutting.Dto
{
    public class AuditoriaFiltroDTO
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Usuario { get; set; }
        public int? Evento { get; set; }
        public string Tabela { get; set; }
        public int? Chave { get; set; }
        public int? NumeroSolicitacao { get; set; }
        public string Mensagem { get; set; }
    }
}
