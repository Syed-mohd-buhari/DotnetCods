using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.Entities;
using CAM.Entities.Models;
using System.Linq;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.SystemType;
using CAM.Entities.Models.Cross;
using CAM.Entities.Mappers.Entity;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System;
using System.Data.SqlTypes;
using System.Collections.Generic;
using CAM.Repository.Helpers;
using System.Security.Claims;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using CAM.BusinessManager.CommonUtilities;

namespace CAM.BusinessManager.MapConfiguration
{
    public class SystemTypeMapper : Profile
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly CommonManager _commonManager;

        public SystemTypeMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, CommonManager commonManager)
        {
            _commonManager = commonManager;
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            #region SystemTypeDtoUpdate

            CreateMap<SystemTypeDtoUpdate, SystemType>()
                .ForMember(dest => dest.AssetClassId, opt => opt.MapFrom(src => src.AssetClassId))
               
                .ForMember(dest => dest.SystemTypesMajorHardwareBuilds, opt => opt.MapFrom(
                    src => src.MajorHardwareBuildId.Select(x => new SystemTypesMajorHardwareBuild()
                    {
                        IsMain = x.IsMain,
                        MajorHardwareId = x.MajorHardwareBuildId,
                    }).ToList()
                ))
                .ForMember(dest=>dest.ConstraintScaling, opt => opt.MapFrom(src=>src.ConstraintScaling.Length == 0 ? (DateTime?)null : DateTime.ParseExact(src.ConstraintScaling, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
              
                .ForMember(dest=>dest.VodafoneName, opt => opt.Ignore())
                .ReverseMap()
                 .ForMember(dest => dest.AssetType, opt => opt.MapFrom(src => src.VodafoneName.Description))
                .ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.ModificationDate))
                .ForMember(dest => dest.MajorHardwareBuildId, opt => opt.MapFrom(
                    src =>
                        src.SystemTypesMajorHardwareBuilds
                            .Where(x => x.Deleted == false)
                            .OrderByDescending(x => x.IsMain)
                            .Select(x => new MajorHardwareBuildMainSystemTypeDto { MajorHardwareBuildId = x.MajorHardwareId, IsMain = x.IsMain })
                            .ToList()
                ))
                .ForMember(dest => dest.LCMStatus, opt => opt.MapFrom(src =>CAM.Entities.Mappers.Entity.SystemTypeMapper.SetSystemTypeMapper(src).GetStatusColor(_repositoryWrapper)))
                .ForMember(dest => dest.ConstraintLcm, otp => otp.MapFrom(src => CAM.Entities.Mappers.Entity.SystemTypeMapper.SetSystemTypeMapper(src).GetStatusName(_repositoryWrapper)))
                .ForMember(dest => dest.EndOfMaintenance, otp => otp.MapFrom(src => CAM.Entities.Mappers.Entity.SystemTypeMapper.SetSystemTypeMapper(src).GetMinorDateEOM(_repositoryWrapper)))
                .ForMember(dest => dest.SystemSolution, opt => opt.MapFrom(src => CAM.Entities.Mappers.Entity.SystemTypeMapper.SetSystemTypeMapper(src).toSystemTypeName(_repositoryWrapper)))
                .ForMember(dest => dest.ConstraintScaling, otp => otp.MapFrom(src => src.ConstraintScaling.HasValue? src.ConstraintScaling.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture):""))
                .ForMember(dest => dest.vodafoneNameId , otp => otp.MapFrom(src =>src.VodafoneNameId))
            //.ForMember(dest => dest.SystemTypesSubDomainSpocs, opt => opt.MapFrom(
            //    src => src.SubDomainSpocIds.Select(x => new SystemTypesSubDomainSpoc()
            //    {
            //        SubDomainSpocId = (short)x,
            //        SystemTypeId = src.SystemTypeId
            //    }).ToList()
            //))
            //.ForMember(dest => dest.AssetTypeId, opt => opt.MapFrom(src => src.AssetTypeId))

            // .ForMember(dest => dest.SubDomainSpocIds, opt => opt.MapFrom(src => src.SystemTypesSubDomainSpocs.Where(x => !x.Deleted).Select(x => x.SubDomainSpocId).ToList()))

            ;
            #endregion
            #region SystemTypeDtoGrid
            CreateMap<SystemType, SystemTypeDtoGrid>()
                .ForMember(x => x.Orphan, s => s.MapFrom(src => !src.DesignComponents.Any()))
                .ForMember(x => x.Deleted, s => s.MapFrom(src => src.Deleted))
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(dest => dest.MajorHardwareBuild, opt => opt.MapFrom(src =>
                    src.SystemTypesMajorHardwareBuilds
                        .Where(x => x.Deleted == false)
                        .OrderBy(x => x.IsMain)
                        .ToDictionary(
                            x => x.MajorHardwareId,
                            x =>
                                $"{x.MajorHardware.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription} - {x.MajorHardware.Platform.PlatformDescription} - {x.MajorHardware.HardwareType}"
                        ))
                )

                .ForMember(dest => dest.MajorHardwareBuildId, opt => opt.MapFrom(src => src.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && !x.Deleted).Select(x => x.MajorHardwareId).FirstOrDefault()))
                .ForMember(dest => dest.MajorSoftwareBuildId, opt => opt.MapFrom(src => src.MajorSoftwareBuildsId))
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.ModificationDate))

            .ForMember(dest => dest.AssetClass, opt => opt.MapFrom(src => src.toAssetClassDescription(_repositoryWrapper)))
            .ForMember(dest => dest.AssetCategory, opt => opt.MapFrom(src => src.AssetCategory.AssetCategoryDescription))
            .ForMember(dest => dest.AssetType, opt => opt.MapFrom(src => src.AssetCategory.TakeFromAssetTypeTable ? src.AssetTypeIdNavigation != null? src.AssetTypeIdNavigation.AssetTypeDescription: "" : (src.VodafoneName.Description != null ? src.VodafoneName.Description : "")))
            .ForMember(dest => dest.LCMStatus, opt => opt.MapFrom(src => CAM.Entities.Mappers.Entity.SystemTypeMapper.SetSystemTypeMapper(src).GetStatusColor(_repositoryWrapper)))
            .ForMember(dest => dest.ConstraintScaling, otp => otp.MapFrom(src => src.ConstraintScaling != null && src.ConstraintScaling.HasValue ? src.ConstraintScaling.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
            .ForMember(dest => dest.ConstraintLcm, otp => otp.MapFrom(src => CAM.Entities.Mappers.Entity.SystemTypeMapper.SetSystemTypeMapper(src).GetStatusName(_repositoryWrapper)))
            .ForMember(dest => dest.EndOfMaintenance, otp => otp.MapFrom(src => CAM.Entities.Mappers.Entity.SystemTypeMapper.SetSystemTypeMapper(src).GetMinorDateEOM(_repositoryWrapper)))
            .ForMember(dest => dest.EndOfMaintenanceValue, otp => otp.MapFrom(src => CAM.Entities.Mappers.Entity.SystemTypeMapper.SetSystemTypeMapper(src).GetDateFormatEOM(_repositoryWrapper)))

            .ForMember(dest => dest.MajorSoftwareBuild, opt => opt.
             MapFrom(x => $"{x.MajorSoftwareBuilds.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription} - {(x.MajorSoftwareBuilds.ProductName != null ? x.MajorSoftwareBuilds.ProductName.Description : "")} - {x.MajorSoftwareBuilds.SoftwareVersion}"))
            .ForMember(dest => dest.SystemSolution, opt => opt.MapFrom(src => CAM.Entities.Mappers.Entity.SystemTypeMapper.SetSystemTypeMapper(src).SystemTypeName(_repositoryWrapper)))
            .ForMember(dest => dest.ProductImportance, opt => opt.MapFrom(src => src.ProductImportanceRel.ProductImportanceDescription))
            .ForMember(dest => dest.HardwareOem, opt => opt.MapFrom(src =>
                src.SystemTypesMajorHardwareBuilds
                    .Where(x => x.IsMain && !x.Deleted)
                    .Select(x => $"{x.MajorHardware.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription}")
                    .FirstOrDefault()
            ))
            .ForMember(dest => dest.MajorHardwareBuildWithoutOem, opt => opt.MapFrom(src =>
                src.SystemTypesMajorHardwareBuilds
                    .Where(x => x.IsMain && !x.Deleted)
                    .Select(x => $"{x.MajorHardware.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription} - {x.MajorHardware.HardwareSolution} - {x.MajorHardware.Platform.PlatformDescription} - {x.MajorHardware.HardwareType}")
                    .FirstOrDefault()
            ))
            .ForMember(dest => dest.SoftwareOem, opt => opt.MapFrom(x => x.MajorSoftwareBuilds.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription))
            .ForMember(dest => dest.ProductImportance, opt => opt.MapFrom(x => x.ProductImportanceRel.ProductImportanceDescription))
            .ForMember(dest => dest.VodafoneName, opt => opt.MapFrom(x => x.VodafoneName.Description))
            .ForMember(dest => dest.VodafoneNameId, opt => opt.MapFrom(x => x.VodafoneName.Id))

            .ForMember(x => x.LastModifiedValue, s => s.MapFrom(src =>
            src.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))

              .ForMember(dest => dest.SoftWareDesignContactEmail, opt => opt.MapFrom(x => _commonManager.GetDesignContactForSoftware(x.MajorSoftwareBuilds.MajorSoftwareBuildsId)))

              .ForMember(dest => dest.SoftWareVertical, opt => opt.MapFrom(x => _commonManager.GetVerticalResponseForLibary(x.MajorSoftwareBuilds.MajorSoftwareBuildsId, 0)))

              .ForMember(dest => dest.SoftWareSubdomain, opt => opt.MapFrom(x => _commonManager.GetSubdomainResponseForLibary(x.MajorSoftwareBuilds.MajorSoftwareBuildsId, 0)))

            .ForMember(dest => dest.HardWareDesignContactEmail, opt => opt.MapFrom(x => _commonManager.GetSubdomainSpocEmailForLibary(0, x.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain == true).Select(x => x.MajorHardwareId).FirstOrDefault())))

              .ForMember(dest => dest.HardWareVertical, opt => opt.MapFrom(x => _commonManager.GetVerticalResponseForLibary(0, x.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain == true).Select(x => x.MajorHardwareId).FirstOrDefault())))

              .ForMember(dest => dest.HardWareSubdomain, opt => opt.MapFrom(x => _commonManager.GetSubdomainResponseForLibary(0, x.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain == true).Select(x => x.MajorHardwareId).FirstOrDefault())))

            ;

            #endregion
            #region SystemTypeDtoCreate
            CreateMap<SystemTypeDtoCreate, SystemType>()
                //.ForMember(dest => dest.SystemTypesSubDomainSpocs, opt => opt.MapFrom(
                //    src => src.SubDomainSpocIds.Select(x => new SystemTypesSubDomainSpoc()
                //    {
                //        SubDomainSpocId = (short)x,
                //    }).ToList()
                //))
                .ForMember(dest => dest.AssetClassId, opt => opt.MapFrom(src => src.AssetClassId))
                .ForMember(dest => dest.VodafoneNameId, opt => opt.MapFrom(src => src.vodafoneNameId))
                .ForMember(dest => dest.VodafoneName, opt => opt.Ignore())
                .ForMember(dest => dest.ConstraintScaling, opt => opt.MapFrom(src => src.ConstraintScalings))
                .ForMember(dest => dest.SystemTypesMajorHardwareBuilds, opt => opt.MapFrom(
                    src => src.MajorHardwareBuildId.Select(x => new SystemTypesMajorHardwareBuild()
                    {
                        MajorHardwareId = x.MajorHardwareBuildId,
                        IsMain = x.IsMain
                    }).ToList()))

                .ReverseMap()

                 .ForMember(dest => dest.VodafoneName, opt => opt.MapFrom(src => src.VodafoneName.Description))
                 .ForMember(dest => dest.VodafoneNameId, opt => opt.MapFrom(src => src.VodafoneName.Id));


            #endregion
            #region SystemTypeDtoGrid, SystemTypeDtoGrouped>().ReverseMap()

            CreateMap<SystemTypeDtoGrid, SystemTypeDtoGrouped>().ReverseMap();
            #endregion
        }
    }
}