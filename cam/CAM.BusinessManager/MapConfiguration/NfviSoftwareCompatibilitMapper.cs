using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.NfviSoftwareCompatibility;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class NfviSoftwareCompatibilitMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;

        public NfviSoftwareCompatibilitMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<NfviSoftwareCompatibility, NfviSoftwareCompatibilityDtoGrid>()
                 .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                  .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
                 .ForMember(x => x.PlaftFormVersion, s => s.MapFrom(src => src.MajorSoftwareVmwarePlaftForm.SoftwareVersion))
                  .ForMember(x => x.ProductName, s => s.MapFrom(src => src.ProductName.Description))
                   .ForMember(x => x.PlaftFormVersionId, s => s.MapFrom(src => src.PlaftFormId))
                  .ForMember(x => x.ProductNameId, s => s.MapFrom(src => src.ProductId))
                  .ForMember(x => x.Vendor, s => s.MapFrom(src => src.Vendor.OriginalEquipmentManufacturerDescription   ))
                  .ForMember(x => x.VendorId, s => s.MapFrom(src => src.VendorId))
                  .ForMember(x=>x.VodafoneName, s=>s.MapFrom(src =>src.ProductName.VodafoneName.Description))
                  .ForMember(x=>x.VodafoneNameId, s=>s.MapFrom(src => src.ProductName.VodafoneName.Id))
                  ;

        }
    }
}
