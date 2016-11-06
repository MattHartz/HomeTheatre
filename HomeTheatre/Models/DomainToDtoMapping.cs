using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HomeTheatre.Dto.Models;

namespace HomeTheatre.Models
{
    public class DomainToDtoMapping : Profile
    {
        public override string ProfileName => "DomainToDto";

        protected override void Configure()
        {
            CreateMap<RoomViewModel, Room>()
                .ForMember(d => d.Name, s => s.MapFrom(f => f.Name))
                .ForMember(d => d.IsPrivate, s => s.MapFrom(f => f.IsPrivate))
                .ForMember(d => d.VideoSource, s => s.MapFrom(f => f.Source))
                .ForMember(d => d.DateAdded, s => s.UseValue(DateTime.Now));
        }
    }
}
