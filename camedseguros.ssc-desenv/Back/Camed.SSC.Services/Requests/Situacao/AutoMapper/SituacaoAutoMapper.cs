using AutoMapper;

namespace Camed.SSC.Application.Requests
{
    public class SituacaoAutoMapper : Profile
    {
        public SituacaoAutoMapper()
        {
            CreateMap<SalvarSituacaoCommand, Domain.Entities.Situacao>();
        }
    }
}
