using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.IdentityAsIs;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.MapConfiguration
{
    public class IdentityAsIsMapper : Profile
    {
        private const short MAX_CHAR_ACTIVITIES = 80;
        private IRepositoryWrapper _repositoryWrapper;


        public IdentityAsIsMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<IdentityAsIs, IdentityAsIsDtoGrid>()
                .ForMember(des => des.CategoryDescription, opt => opt.MapFrom(src => src.Category.Description))
                .ForMember(des => des.ClassDescription, opt => opt.MapFrom(src => src.Class.Description))
                .ForMember(des => des.TypeDescription, opt => opt.MapFrom(src => src.Type.Description))
                .ForMember(des => des.AssetName, opt => opt.MapFrom(src => src.Asset.ElementName))
                .ForMember(des => des.DesignComponentFamily, opt => opt.MapFrom(src => src.Asset.DesignComponent.DesignComponentFamily.DCFName(_repositoryWrapper)))
                .ForMember(des => des.LastModifiedValue, opt => opt.MapFrom(src => src.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(des => des.LastModified, opt => opt.MapFrom(src => src.ModificationDate))
                .ForMember(des => des.LastModifiedBy, opt => opt.MapFrom(src => src.ModificationUserEntity.Email))
               .ForMember(
                    dest => dest.VerticalId,
                    opt => opt.MapFrom(x => x.Asset.DesignComponent.SystemType.VerticalResponsibleId)
                )
                .ForMember(
                    dest => dest.VerticalName,
                    opt => opt.MapFrom(x =>
                        (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0) ?
                        string.Join(",", x.VerticalFilterDto.Select(m => m.Value).ToList() ??
                        new List<string>()) : string.Empty)
                )
                 
                 .ForMember(des => des.OpCoId, opt => opt.MapFrom(src => src.Asset.OpCoId))
                  .ForMember(des => des.OpCo, opt => opt.MapFrom(src => src.Asset.OpCo.OpCoDescription));

        }
    }
}
