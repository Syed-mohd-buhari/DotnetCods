using AutoMapper;
using CAM.DataTransferObjects.Entita;
using CAM.Entities.Models.Lookup;

namespace CAM.BusinessManager.MapConfiguration
{
    public class GlossaryItemsMapper : Profile
    {
        public GlossaryItemsMapper()
        {
            CreateMap<GlossaryItemsGridDto, GlossaryItems>().ReverseMap();
            CreateMap<GlossaryItemsDtoCreate, GlossaryItems>().ReverseMap();
            CreateMap<GlossaryItemsDtoUpdate, GlossaryItems>().ReverseMap();
        }
    }
}