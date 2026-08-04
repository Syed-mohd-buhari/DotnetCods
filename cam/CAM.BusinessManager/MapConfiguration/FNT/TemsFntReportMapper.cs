using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.FNT_Report;
using CAM.Entities.Models.FNT;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration.FNT
{
    public class TemsFntReportMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public TemsFntReportMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {
 

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<TemsFntReport, TemsFntReportDtoGrid>()
            .ForMember(x => x.ModificationUser, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.CreationUser, s => s.MapFrom(src => src.CreationUserEntity.Email))
            .ForMember(x => x.ProductImportance, s => s.MapFrom(src => src.Productimportance))
            .ForMember(x => x.VendorFnt, s => s.MapFrom(src => src.Vendor))
            .ForMember(x => x.AssetTypeFnt, s => s.MapFrom(src => src.Assettype))
            .ForMember(x => x.AssetDescriptionFnt, s => s.MapFrom(src => src.Assetdescription))
            .ForMember(x => x.VendorEndOfMaintenanceDateFnt, s => s.MapFrom(src => src.Vendorendofmaintenancedate))
            .ForMember(x => x.IdentifiedActionFnt, s => s.MapFrom(src => src.Identifiedaction))
            .ForMember(x => x.DescriptionOfPlannedActionFnt, s => s.MapFrom(src => src.Descriptionofplannedaction));

        }
    }
}
