namespace Camed.SCC.Infrastructure.CrossCutting.Identity
{
    public class SSCIdentity 
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Matricula { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public bool EhCalculista { get; set; }
        public bool EhSolicitante { get; set; }
        public bool EhAtendente { get; set; }
        public bool EhAgrosul { get; set; }
        public bool PodeVisualizarObservacoes { get; set; }
        public bool PermitidoGerarCotacao { get; set; }
    }
}
