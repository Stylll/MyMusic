using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyMusic.Core.Models;
using MyMusic.API.Resources;

namespace MyMusic.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // model to resource
            CreateMap<Music, MusicResource>();
            CreateMap<Artist, ArtistResource>();

            // resource to model
            CreateMap<MusicResource, Music>();
            CreateMap<ArtistResource, Artist>();
            CreateMap<SaveArtistResource, Artist>();
            CreateMap<SaveMusicResource, Music>();
        }
    }
}
