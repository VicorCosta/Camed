using AutoMapper;
using Camed.SSC.Application.ViewModel;

namespace Camed.SSC.Application.Requests
{
    public class ExpedienteAutoMapper : Profile
    {
        public ExpedienteAutoMapper()
        {
            CreateMap<Domain.Entities.Expediente, ExpedienteViewModel>()
            .ForMember(dest => dest.HoraInicialManha, config => config.MapFrom(m => m.HoraInicialManha.ToString("HH:mm")))
            .ForMember(dest => dest.HoraFinalManha, config => config.MapFrom(m => m.HoraFinalManha.ToString("HH:mm")))
            .ForMember(dest => dest.HoraInicialTarde, config => config.MapFrom(m => m.HoraInicialTarde.ToString("HH:mm")))
            .ForMember(dest => dest.HoraFinalTarde, config => config.MapFrom(m => m.HoraFinalTarde.ToString("HH:mm")));

        }
    }
}

