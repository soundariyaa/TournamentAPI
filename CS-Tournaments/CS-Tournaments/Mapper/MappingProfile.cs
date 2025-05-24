using AutoMapper;
using CS_Tournaments.Api.Models;

namespace CS_Tournaments.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tournament, ResponseTournament>()
           .ForMember(dest => dest.SubTournaments, opt => opt.MapFrom(src => src.SubTournaments))
           .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players));

            // SubTournaments can still be Tournament entities
            CreateMap<Tournament, ResponseSubTournament>()
                .ForMember(dest => dest.SubTournaments, opt => opt.MapFrom(src => src.SubTournaments));

            CreateMap<Player, ResponsePlayer>();
        }
    }   
}
