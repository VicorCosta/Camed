using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.ViewModel
{
    public class AnexoBNBViewModel
    {
        public string NomeDoArquivo { get; set; }
        public string ConteudoBase64 { get; set; }
        public int Solicitacao_Id { get; set; }
        public string Caminho { get; set; }
        public string Extensao { get; set; }
    }
}
