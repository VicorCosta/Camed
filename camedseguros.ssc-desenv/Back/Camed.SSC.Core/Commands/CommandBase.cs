using Camed.SSC.Core.Interfaces;
using FluentValidation.Results;
using MediatR;

namespace Camed.SSC.Core.Commands
{
    public abstract class CommandBase : IRequest<IResult>
    {
        public CommandBase()
        {
            Issuer = GetType().Name;
        }

        public string Issuer { get; }
        public ValidationResult ValidationResult { get; set; }
        public abstract bool IsValid();
    }
}
