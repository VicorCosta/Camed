using Camed.SSC.Core.Entities;

namespace Camed.SSC.Domain.Entities
{
    public class RelatorioFiltros : EntityBase
    {
        public string TipoRelatorio { get; set; }
        public string Solicitacao { get; set; }
        public string Segurado { get; set; }
        public string Superintendencia { get; set; }
        public string Operador { get; set; }
        public string SuperConta { get; set; }
        public string Atendente { get; set; }
        public string Cnpj { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public string DataInicialAC { get; set; }
        public string DataFinalAC { get; set; }
        public string DataFechamentoI { get; set; }
        public string DataFechamentoF { get; set; }
        public string AreaNegocio { get; set; }
        public string TipoSeguro { get; set; }
        public string Segmento { get; set; }
        public string RamoSeguro { get; set; }
        public string Status { get; set; }
        public string EmProcesso { get; set; }
        public string Vistoria { get; set; }
        public string Situacao { get; set; }
        public string Agencia { get; set; }
        public string AgenciaConta { get; set; }
        public string UsuarioId { get; set; }
    }
}
