using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirAgenciaTipoDeAgenciaCommand: IRequest<IResult>
    {
        public int Id { get; set; }

    }
}
