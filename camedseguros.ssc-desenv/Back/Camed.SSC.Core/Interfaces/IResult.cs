using Camed.SSC.Core.Commands;
using System.Collections.Generic;

namespace Camed.SSC.Core.Interfaces
{
    public interface IResult
    {
        bool Successfully { get; set; }
        string Message { get; set; }
        IList<Notification> Notifications { get; }
        bool HasNotification { get; }
        void SetValidationErros(CommandBase command);
        dynamic Payload { get; set; }

    }
}
