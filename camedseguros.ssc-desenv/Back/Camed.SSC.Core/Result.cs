using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Camed.SSC.Core {
    public class Result : IResult {
        public Result() {
            Notifications = new List<Notification>();
        }

        public bool Successfully { get; set; } = true;
        public string Message { get; set; } = "Ok";
        public IList<Notification> Notifications { get; }
        public bool HasNotification {
            get {
                return Notifications.Any();
            }
        }
        public dynamic Payload { get; set; }

        public void SetValidationErros(CommandBase command) {
            if(command.ValidationResult != null) {
                foreach(var error in command.ValidationResult.Errors) {
                    Notifications.Add(new Notification {
                        Key = error.PropertyName.ToLower(),
                        Value = error.ErrorMessage
                    });
                }

                Successfully = false;
                Message = "Invalid request";
            }
        }
    }
}
