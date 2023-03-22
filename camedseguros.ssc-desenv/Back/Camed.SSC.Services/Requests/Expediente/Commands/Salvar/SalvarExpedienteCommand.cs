using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;

namespace Camed.SSC.Application.Requests
{
    public class SalvarExpedienteCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }
        public int Dia { get; set; }
        public string HoraInicialManha { get; set; }
        public string HoraFinalManha { get; set; }
        public string HoraInicialTarde { get; set; }
        public string HoraFinalTarde { get; set; }


        public DateTime HoraInicialManhaDate
        {
            get
            {
                try
                {
                    var hoje = DateTime.Parse($"{DateTime.Now.ToString("yyyy-MM-dd")} {HoraInicialManha}");
                    return hoje;
                }
                catch
                {
                    throw new ApplicationException($"Ocorreu um erro na Hora Inicial Manha");
                }
            }
        }
        public DateTime HoraFinalManhaDate
        {
            get
            {
                try
                {
                    var hoje = DateTime.Parse($"{DateTime.Now.ToString("yyyy-MM-dd")} {HoraFinalManha}");
                    return hoje;
                }
                catch
                {
                    throw new ApplicationException($"Ocorreu um erro na Hora Final Manha");
                }
              
               
            }
        }
        public DateTime HoraInicialTardeDate
        {
            get
            {
                try
                {
                    var hoje = DateTime.Parse($"{DateTime.Now.ToString("yyyy-MM-dd")} {HoraInicialTarde}");
                    return hoje;
                }
                catch
                {
                    throw new ApplicationException($"Ocorreu um erro na Hora Inicial Tarde");
                }
                
            }
        }
        public DateTime HoraFinalTardeDate
        {
            get
            {
                try
                {
                    var hoje = DateTime.Parse($"{DateTime.Now.ToString("yyyy-MM-dd")} {HoraFinalTarde}");
                    return hoje;
                }
                catch
                {
                    throw new ApplicationException($"Ocorreu um erro na Hora Final Tarde");
                }
            }

        }

        public override bool IsValid()
        {
            ValidationResult = new SalvarExpedienteCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
