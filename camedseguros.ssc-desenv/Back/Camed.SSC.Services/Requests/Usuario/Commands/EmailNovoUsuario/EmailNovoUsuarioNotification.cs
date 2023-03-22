using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class EmailNovoUsuarioNotification : INotification
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
