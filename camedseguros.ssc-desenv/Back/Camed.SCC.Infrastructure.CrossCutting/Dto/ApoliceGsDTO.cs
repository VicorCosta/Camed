using System;

namespace Camed.SCC.Infrastructure.CrossCutting.Dto
{
    public class ApoliceGsDTO
    {

        public string Segurado { get; set; }
        public string Seguradora { get; set; }
        public Int64 Apolice { get; set; }
        public byte ENDOSSO { get; set; }
        public DateTime INICIO_VIGENCIA { get; set; }
        public DateTime FIM_VIGENCIA { get; set; }
        public string Status { get; set; }
        public string Produto { get; set; }
        public string ArquivoApolice { get; set; }
        public string ArquivoBoleto { get; set; }
        public string CODIGOSBEM { get; set; }
        public string OPERACAO { get; set; }
        public string SEGUROOBRIGATORIO { get; set; }
        public Double ImportanciaSegurada { get; set; }
    }
}



