using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.InizializeNewProduct;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.Entity
{
    public class InizializeNewProductManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager manager;
        private readonly DesignComponentManager _designComponentManager;
        private readonly MajorSoftwareBuildManager _majorSoftwareBuildManager;
        private readonly MajorHardwareBuildManager _majorHardwareBuildManager;
        private readonly SystemTypeManager _systemTypeManager;
        private readonly ILoggerManager _logger;

        public InizializeNewProductManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, IRepositoryWrapper repositoryWrapper,
            MajorSoftwareBuildManager majorSoftwareBuildManager, GridCustomColumnManager manager, DesignComponentManager designComponentManager,
            SystemTypeManager systemTypeManager, MajorHardwareBuildManager majorHardwareBuildManager, IHttpContextAccessor contextAccessor, ILoggerManager logger_) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            this.manager = manager;
            _designComponentManager = designComponentManager;
            _majorSoftwareBuildManager = majorSoftwareBuildManager;
            _systemTypeManager = systemTypeManager;
            _majorHardwareBuildManager = majorHardwareBuildManager;
            _logger = logger_;
        }


        public async Task<ResultDto> Add(InizializeNewProductCreateDto dto)
        {
            try
            {
                var transaction = await _repositoryWrapper.BeginTransactionAsync();
                var validation = await ValidationDto(dto);
                if (validation == null)
                {

                    var check = await CheckIfEntitiesExist(dto);
                    if (check == null)
                    {
                        //creo ms
                        var ms = await _majorSoftwareBuildManager.AddBase(dto.MajorSoftwareBuildDto);
                        MajorHardwareBuild mh = new MajorHardwareBuild();
                        // creo mh
                        if (dto.MajorHardwareBuildDto.MajorHardwareId == 0)
                        {
                            mh = _mapper.Map<MajorHardwareBuild>(dto.MajorHardwareBuildDto);
                            var model = MajorHardwareBuildMapper.SetMajorHardwareBuildMapper(mh);
                            _repositoryWrapper.MajorHardwareBuild.Create(model);
                            await _repositoryWrapper.SaveAsync();

                            dto.SystemTypeDto.MajorHardwareBuildId.First().MajorHardwareBuildId = model.Majorhardwareid;

                        }
                        else
                        {
                            //var mhBuildExists = await _majorHardwareBuildManager.EntityExists(dto.MajorHardwareBuildDto);
                            var mhBuildExists = await _repositoryWrapper.MajorHardwareBuild
                                .FindByCondition(x => x.Majorhardwareid == dto.MajorHardwareBuildDto.MajorHardwareId)
                                .FirstOrDefaultAsync();
                            if (mhBuildExists != null)
                            {
                                mhBuildExists.Vulnerabilitystatus = dto.MajorHardwareBuildDto.VulnerabilityStatus;
                                _repositoryWrapper.MajorHardwareBuild.Update(mhBuildExists);
                            }
                            else
                            {
                                return new ResultDto
                                {
                                    Warning = true,
                                    Info = ResultMessages.ErrorValidation
                                };
                            }

                        }
                        //creo system type
                        dto.SystemTypeDto.MajorSoftwareBuildsId = (long)ms.Data;

                        var res = await _systemTypeManager.AddBase(dto.SystemTypeDto);
                        var st = (Systemtypes)res.Data;

                        st = _systemTypeManager.SetSystemTypeValue(st);
                        //st.Assettypeid = null;

                        _repositoryWrapper.SystemType.Update(st);
                        _repositoryWrapper.Save();

                        //creo dc

                        dto.DesignComponentDto.SystemTypeId = st.Systemtypeid;
                        dto.DesignComponentDto.VisibleFlag = true;  // Ticket 604 - #503 - Update/Upgrade Ms/w - Software Upgrade Utility 
                        await _designComponentManager.Add(dto.DesignComponentDto);


                    }
                    else
                    {
                        return check;
                    }
                    await transaction.CommitAsync();
                    return new ResultDto
                    {
                        Warning = false,
                        Info = ResultMessages.EntryAddSuccess
                    };
                }
                else
                {
                    await transaction.RollbackAsync();
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.ErrorValidation
                    };

                }
            }
            catch(Exception e)
            {
                _logger.LogError("Issue happen when try to update PA status : " + e);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
         
        }


        private async Task<ResultDto> ValidationDto(InizializeNewProductCreateDto dto)
        {
            //validazione dc
            if (
                //dto.DesignComponentDto.DesignComponentFamilyId == 0 ||
                dto.SystemTypeDto.AssetCategoryId == null || dto.SystemTypeDto.AssetCategoryId == 0 ||
                dto.SystemTypeDto.vodafoneNameId == null || dto.SystemTypeDto.vodafoneNameId == 0 ||
                dto.SystemTypeDto.ProductImportanceId == null || dto.SystemTypeDto.ProductImportanceId == 0 ||
                dto.SystemTypeDto.AssetClassId == null || dto.SystemTypeDto.AssetClassId == 0 ||
                //dto.SystemTypeDto.AssetTypeId == null || dto.SystemTypeDto.AssetTypeId == 0 ||
                //dto.SystemTypeDto.VerticalResponsibleId == null || dto.SystemTypeDto.VerticalResponsibleId == 0 ||
                //dto.SystemTypeDto.SubDomainResponsibleId == null || dto.SystemTypeDto.SubDomainResponsibleId == 0 ||
                //!dto.SystemTypeDto.SubDomainSpocIds.Any() ||
                dto.MajorSoftwareBuildDto.OriginalEquipmentManufacturerId == 0 ||
                 dto.MajorSoftwareBuildDto.ProductNameId == 0 ||
                string.IsNullOrWhiteSpace(dto.MajorSoftwareBuildDto.ProductNameId?.ToString()) ||
                string.IsNullOrWhiteSpace(dto.MajorSoftwareBuildDto.SoftwareVersion)
                //||
                //dto.MajorSoftwareBuildDto.GeneraAvailableDate == null ||
                //dto.MajorSoftwareBuildDto.GeneraAvailableDate == DateTime.MinValue ||
                //dto.MajorSoftwareBuildDto.EndOfMaintenance == null ||
                //dto.MajorSoftwareBuildDto.EndOfMaintenance == DateTime.MinValue
                //Task 404
                //dto.MajorSoftwareBuildDto.EndOfsupport == null ||
                //dto.MajorSoftwareBuildDto.EndOfsupport == DateTime.MinValue
                )
                return new ResultDto() { Warning = true, Info = ResultMessages.ErrorValidation };
            else return null;
        }

        private async Task<ResultDto> CheckIfEntitiesExist(InizializeNewProductCreateDto dto)
        {
            var msBuildExists = await _majorSoftwareBuildManager.EntityExists(dto.MajorSoftwareBuildDto);
            if (msBuildExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = msBuildExists.Deleted ? ResultMessages.MajorSoftwareBuildAddExistsDeleted : ResultMessages.MajorSoftwareBuildAddExists,
                    Data = msBuildExists.MajorSoftwareBuildsId
                };
            }

            if (dto.MajorHardwareBuildDto.MajorHardwareId == 0)
            {
                var mhBuildExists = await _majorHardwareBuildManager.EntityExists(dto.MajorHardwareBuildDto);

                if (mhBuildExists != null)
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = mhBuildExists.Deleted
                            ? ResultMessages.MajorHardwareBuildAddExistsDeleted
                            : ResultMessages.MajorHardwareBuildAddExists,
                        Data = mhBuildExists.MajorHardwareId
                    };
                }
            }

            var stTypeExists = await _systemTypeManager.EntityExists(dto.SystemTypeDto);



            if (stTypeExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = stTypeExists.Deleted ? ResultMessages.SystemTypeAddExistsDeleted : ResultMessages.SystemTypeAddExists,
                    Data = stTypeExists.SystemTypeId
                };
            }

            var supportedAllServices = _repositoryWrapper.SubNetworkBoundaries.FindByCondition(p => p.Default == true).FirstOrDefault();
            //dto.DesignComponentDto.SubNetworkBoundaryIds = dto.DesignComponentDto.SupportedAllServices ? new List<long> { supportedAllServices.Id } : dto.DesignComponentDto.SubNetworkBoundaryIds;

            foreach (var item in dto.DesignComponentDto.SubNetworkBoundaryIds)
            {
                var dcExists = await _designComponentManager.EntityExists(dto.DesignComponentDto.SystemTypeId, dto.DesignComponentDto.DesignComponentFamilyId, item);

                if (dcExists != null)
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = dcExists.Deleted ? ResultMessages.DesignComponentAddExistsDeleted : ResultMessages.DesignComponentAddExists,
                        Data = dcExists.SystemTypeId
                    };
                }
            }


            return null;
        }
    }
}
