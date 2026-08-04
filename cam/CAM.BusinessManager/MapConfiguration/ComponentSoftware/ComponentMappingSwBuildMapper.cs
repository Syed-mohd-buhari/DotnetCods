using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration.ComponentSoftware
{
    public class ComponentMappingSwBuildMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        private CommonManager _commonManager { get; set; }
        public ComponentMappingSwBuildMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, CommonManager commonManager)
        {
            _commonManager = commonManager;
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

          
            _ = CreateMap<ComponentSoftwareBuildBag, ComponentMappingSWBuildBagGridDto>();             
             
                //.ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                //.ForMember(x => x.ComponentSwBuildId, s => s.MapFrom(src => src.ComponentSoftwareBuildId))
                //.ForMember(x => x.ComponentSwBuildBagId, s => s.MapFrom(src => src.ComponentSoftwareBuildBagId))
                //.ForMember(
                //    dest => dest.BuildBagDescription,
                //    opt => opt.MapFrom(x => x.BuildBags.ComponentBagDescription  ))
                
                //.ForMember(o => o.ComponentSwBuildBagDescription,
                //    o => o.MapFrom(s => $"{s.ComponentSoftwareBuilds.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription} - " +
                //                         $"{s.ComponentSoftwareBuilds.ProductName.Description} - " +
                //                         $"{s.ComponentSoftwareBuilds.SoftwareVersion}"))

                //.ForMember(x => x.LastModifiedValue, opt => opt.MapFrom(s => s.ModificationDate))
              

                





        }
    }
}