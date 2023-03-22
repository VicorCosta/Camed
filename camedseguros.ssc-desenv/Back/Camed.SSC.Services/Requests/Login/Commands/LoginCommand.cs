using Camed.SSC.Core.Commands;

namespace Camed.SSC.Application.Requests
{
    public class LoginCommand : CommandBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new LoginCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
