using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.ViewModel
{
    public class AnexoDeInboxViewModel
    {
        public string Nome { get; set; }
        public string Extensao { get; set; }
        public string Caminho { get; set; }
        public int Inbox_Id { get; set; }
        public byte[] Arquivo { get; set; }
    }
}
