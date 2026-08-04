using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.SystemType;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Lookup;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
  public partial class SystemTypeManager
    {
        public async Task<ResultDto> Restore(long id)
        {

            var entity = await _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == id, true).SingleAsync();
            var anotherEntityWithSameNaturalKeyExists = await _repositoryWrapper.SystemType.FindByCondition(
                x => x.Systemtypenameoem == entity.Systemtypenameoem
                     && x.Majorsoftwarebuildsid == entity.Majorsoftwarebuildsid
                     && x.Systemtypeid != entity.Systemtypeid).FirstOrDefaultAsync();

            if (anotherEntityWithSameNaturalKeyExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateExists,
                    Data = anotherEntityWithSameNaturalKeyExists.Systemtypeid
                };
            }
            entity.Deleted = false;
            entity.Deletiondate = null;

            _repositoryWrapper.SystemType.Update(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.Systemtypeid
            };
        }

        public string GetMinorDateFromMajorEntity(int? majorSoftwareId, List<long> majorHardwareIds)
        {
            List<DateTime?> date = new List<DateTime?>();

            var majorSoftwareBuild = new Majorsoftwarebuilds();
            var majorHardwareBuild = new Majorhardwarebuilds();
            if (majorSoftwareId.HasValue)
            {
                majorSoftwareBuild = _repositoryWrapper.MajorSoftwareBuild
                     .FindByCondition(x => x.Majorsoftwarebuildsid == majorSoftwareId, true)
                     .SingleOrDefault();
            }

            if (majorHardwareIds != null && majorHardwareIds.Any() && majorSoftwareBuild != null)
            {
                majorHardwareBuild = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => majorHardwareIds.Contains(x.Majorhardwareid)).SingleOrDefault();
                return GetMinorDateEOM(majorSoftwareBuild, majorHardwareBuild);
            }


            return string.Empty;

        }
        public static string GetMinorDateEOM(Majorsoftwarebuilds majorsoftwarebuilds, Majorhardwarebuilds majorhardwarebuilds)
        {
            var mSoftwareEOM = new EOMType
            {
                EOMDate = majorsoftwarebuilds?.Endofmaintenance,
                Status = (EOMEnum)majorsoftwarebuilds?.Eomstatus
            };

            var mHarwareEOM = new EOMType
            {
                EOMDate = majorhardwarebuilds?.Endofmaintenance,
                Status = (EOMEnum)majorhardwarebuilds?.Eomstatus
            };

            if (mSoftwareEOM.Status == EOMEnum.Default && mHarwareEOM.Status == EOMEnum.Default)
            {
                return (mSoftwareEOM.EOMDate <= mHarwareEOM.EOMDate) ? mSoftwareEOM.EOMDate.Value.ToLongDateString() : mHarwareEOM.EOMDate.Value.ToLongDateString();
            }
            else if (mSoftwareEOM.Status == EOMEnum.Default && mHarwareEOM.Status != EOMEnum.Default)
            {
                return mSoftwareEOM.EOMDate.Value.ToLongDateString();
            }
            else if (mSoftwareEOM.Status != EOMEnum.Default && mHarwareEOM.Status == EOMEnum.Default)
            {
                return mHarwareEOM.EOMDate.Value.ToLongDateString();
            }
            else if (mSoftwareEOM.Status == EOMEnum.NotAnnounced || mHarwareEOM.Status == EOMEnum.NotAnnounced)
            {
                return "Not Announced";
            }
            else if (mSoftwareEOM.Status == EOMEnum.NotSpecified && mHarwareEOM.Status == EOMEnum.NotSpecified)
            {
                return "Not Specified";
            }
            return null;
        }

        public QueryResultDto<SystemTypeDtoGrouped> GetGridGrouped(SystemTypeQueryDto systemTypeFilterDto)
        {
            var predicateResult = ApplyFilter(systemTypeFilterDto);
            var data = GetSystemTypeDtoGrids(systemTypeFilterDto, ref predicateResult);
            var systemTypeResult = _mapper.Map<IEnumerable<SystemTypeDtoGrouped>>(data);
            var d = systemTypeResult.GroupBy(x => new { x.MajorSoftwareBuildId, x.MajorHardwareBuildId });
            foreach (var item in d.ToList())
            {
                foreach (var i in item)
                {
                    i.KeyGrouped = $"{item.Key.MajorSoftwareBuildId}_{item.Key.MajorHardwareBuildId}";

                }
            }
            var rtn = new QueryResultDto<SystemTypeDtoGrouped>(new GenerateRenderForGrid<SystemTypeDtoGrouped>(_customColumnManager))
            {
                TotalItems = systemTypeResult.Count()
            };

            rtn.Items = systemTypeResult.ToList();
            return rtn;
        }

        public ResultDto<SystemTypeReleatedMajorEntity> GetAssetCategoryReleated(int assetCategoryId)
        {
            var systemType = _repositoryWrapper.SystemType.FindByCondition(x => x.Assetcategoryid.HasValue);
            var releated = new SystemTypeReleatedMajorEntity();
            releated.IdMajorHardwareSameAssetCategory = systemType.Where(x => x.Assetcategoryid == assetCategoryId)
                .SelectMany(x => x.Systemtypesmajorhardwarebuilds).Select(x => x.Majorhardwareid).Distinct().ToList();
            releated.IdMajorHardwareOtherAssetCategory = systemType.Where(x => x.Assetcategoryid != assetCategoryId)
                .SelectMany(x => x.Systemtypesmajorhardwarebuilds).Select(x => x.Majorhardwareid).Distinct().ToList();
            releated.IdMajorSoftwareSameAssetCategory = systemType.Where(x => x.Assetcategoryid == assetCategoryId)
                .Select(x => x.Majorsoftwarebuilds.Majorsoftwarebuildsid).Distinct().ToList();
            releated.IdMajorSoftwareOtherAssetCategory = systemType.Where(x => x.Assetcategoryid != assetCategoryId)
                .Select(x => x.Majorsoftwarebuilds.Majorsoftwarebuildsid).Distinct().ToList();


            return new ResultDto<SystemTypeReleatedMajorEntity>()
            {
                Warning = false,
                Data = releated,
                Info = ""
            };

        }

        public async Task<ResultDto<ResultDataRemediationDto>> ApplyDataRemediation(DataRemediationDto data)
        {
            List<long> designComponentIDs = new List<long>();
            var existsRelationWithDCCorrect = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Systemtypeid == data.CorrectId, true, false).ToList();
            //systemtype
            foreach (var item in data.DuplicatesId)
            {
                //Get delle relazioni con design component
                var designComponentWithDuplicates = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Systemtypeid == item, true, false).ToList();
                designComponentIDs.AddRange(designComponentWithDuplicates.Select(x => x.Designcomponentid));
                foreach (var st in designComponentWithDuplicates)
                {
                    st.Systemtypeid = data.CorrectId;
                    var entityExists = existsRelationWithDCCorrect.Where(str => str.Designcomponentid == st.Designcomponentid)?.SingleOrDefault();
                    if (entityExists == null)
                    {
                        _repositoryWrapper.DesignComponent.Update(st);
                        _repositoryWrapper.Save();
                    }
                }
                var systemTypesWithDuplicates = _repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(x => x.Systemtypeid == item, true, false).ToList();
                var systemTypesWithDuplicatesAttivi = new List<SystemTypesMajorHardwareBuild>(systemTypesWithDuplicates.Where(x => x.Deleted == false).Select(p=>SystemTypesMajorHardwareBuildMapper.GetSystemTypesMajorHardwareBuildMapper(p)));
                var systemTypeToDeleted = systemTypesWithDuplicates;
                ////Rimuovi relazioni
                foreach (var toDelete in systemTypeToDeleted)
                {
                    var stSubDomaniSpocs = _repositoryWrapper.SystemTypesSubDomainSpoc.FindByCondition(x => x.Systemtypeid == toDelete.Systemtypeid, true).ToList();
                    foreach (var stSubDomaniSpoc in stSubDomaniSpocs)
                    {
                        _repositoryWrapper.SystemTypesSubDomainSpoc.DeleteDeep(stSubDomaniSpoc);
                        _repositoryWrapper.Save();
                    }
                    _repositoryWrapper.SystemTypesMajorHardwareBuild.DeleteDeep(toDelete);
                    _repositoryWrapper.Save();
                }

                var systemType = _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == item, true).Single();
                _repositoryWrapper.SystemType.DeleteDeep(systemType);
                _repositoryWrapper.Save();
                var existsRelationWithSt = _repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(x => x.Systemtypeid == data.CorrectId, true).ToList();

                foreach (var st in systemTypesWithDuplicatesAttivi)
                {
                    st.SystemTypeId = data.CorrectId;
                    var entityExists = existsRelationWithSt
                        .SingleOrDefault(str => str.Systemtypeid == st.SystemTypeId && str.Majorhardwareid == st.MajorHardwareId);
                    if (entityExists == null)
                    {
                        if (_repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(x => x.Systemtypeid == st.SystemTypeId && x.Ismain).Count() > 0) st.IsMain = false;
                        _repositoryWrapper.SystemTypesMajorHardwareBuild.Create(SystemTypesMajorHardwareBuildMapper.SetSystemTypesMajorHardwareBuildMapper(st));
                        _repositoryWrapper.Save();
                    }
                }

            }
            foreach (var id in designComponentIDs)
            {
                var lcmList = _repositoryWrapper.Lcmengineering
                    .FindByCondition(x => x.Designcomponentid == id, true, false)
                    .Include(x => x.PlannedactivitiesLcmengineering)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Activitystatus)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Designcomponent)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Responsibilityphase)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                    //.Include(x => x.Lcmengineeringsubdomainspoc).ThenInclude(x => x.Subdomainspoc)
                    .Include(x => x.Lcmoperationalcontracts).ThenInclude(x => x.Operationalcontract)
                    //.Include(x => x.Lcmengineeringeduspoc).ThenInclude(x => x.Subdomainspoc)
                    .Include(x => x.Opco)
                    .Include(x => x.Productimportance)
                    .Include(x => x.ModificationuserNavigation)
                    .Include(x => x.Reasoncheckboxresourcelcmengineeringhardware).ThenInclude(x => x.Reasoncheckboxresource)
                    .Include(x => x.Reasoncheckboxresourcelcmengineeringsoftware).ThenInclude(x => x.Reasoncheckboxresource)
                    .ToList();
                foreach (var lcm in lcmList)
                {
                    var updated = await _lcmEngineeringManager.SetLcmValue(lcm);
                    _repositoryWrapper.Lcmengineering.Update(updated);
                    _repositoryWrapper.Save();
                }

            }

            //aggiorno lcm collegati ai dc modificati

            return new ResultDto<ResultDataRemediationDto>()
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = false,
                Data = (designComponentIDs != null && designComponentIDs.Count > 0) ? new ResultDataRemediationDto() { Id = designComponentIDs.Distinct().ToList() } : null
            };
        }


      
        public string GetSystemSolutionName(string nameOem, int? majorHardwareBuilds, int? majorSoftwareBuild, List<int>? majorHardwareList)
        {
            var name = "";
            if (majorSoftwareBuild.HasValue && majorSoftwareBuild != 0)
            {

                var majorSoftware = _repositoryWrapper.MajorSoftwareBuild
                    .FindByCondition(x => x.Majorsoftwarebuildsid == majorSoftwareBuild)
                    .Include(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Productname)
                    .SingleOrDefault();

                name = $"{majorSoftware?.Orgeqpmanufacturer?.Originalequipmentmanufacturer} - ";

                name += $"{(majorSoftware?.Productname != null ?majorSoftware?.Productname.Description:"")} - {majorSoftware?.Softwareversion}";

            }
            else 
            {
                name = nameOem;
            }

            if (majorHardwareBuilds.HasValue && name != "")
            {
                name += "<b class=\"text-lowercase\"> on </b>";
            }

            if (majorHardwareBuilds.HasValue && majorHardwareBuilds != 0)
            {
                var majorHardware = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x =>
                    majorHardwareBuilds == x.Majorhardwareid).Include(x => x.Platform).SingleOrDefault();

                name += $" {majorHardware?.Hardwaresolution} - {majorHardware?.Platform.Platform} - {majorHardware?.Hardwaretype}";

                if (majorHardwareList != null && majorHardwareList.Any())
                {
                    foreach (var item in majorHardwareList)
                    {
                        var maj = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x =>
                            item == x.Majorhardwareid).Include(x => x.Platform).SingleOrDefault();

                        name += $"<b class=\"text-lowercase\"> with </b> {maj?.Hardwaresolution} - {maj?.Platform.Platform} - {maj?.Hardwaretype}";
                    }
                }
            }

            return name;


        }

        public ConstraintInfoDto GetCostraintInfo(ConstrainInfoQueryDto data)
        {
            var date = new List<DateTime?>();
            var constrainInfo = new ConstraintInfoDto();

            if (data.MajorSoftwareBuild.HasValue && data.MajorSoftwareBuild != 0)
            {
                var majorSofrware = _repositoryWrapper.MajorSoftwareBuild
                                 .FindByCondition(x => x.Majorsoftwarebuildsid == data.MajorSoftwareBuild, true).Single();
                date.Add(majorSofrware.Lasttimebuynew);
                date.Add(majorSofrware.Lasttimebuyupgrades);
                date.Add(majorSofrware.Lasttimebuyexpansions);
                date.Add(majorSofrware.Endofmaintenance);
                date.Add(majorSofrware.Endofsupport);
            }


            if (data.MajorHardwareBuilds != null)
            {
                var majorHardware = new List<MajorHardwareBuild>();
                foreach (var id in data.MajorHardwareBuilds)
                {
                    var maj = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == id, true).Single();
                    majorHardware.Add(MajorHardwareBuildMapper.GetMajorHardwareBuildMapper(maj));
                }
                date.AddRange(majorHardware.Select(x => x?.LastTimeBuyNew) ?? Array.Empty<DateTime?>());
                date.AddRange(majorHardware.Select(x => x?.LastTimeBuyUpgrades) ?? Array.Empty<DateTime?>());
                date.AddRange(majorHardware.Select(x => x?.LastTimeBuyExpansions) ?? Array.Empty<DateTime?>());
                date.AddRange(majorHardware.Select(x => x?.EndOfMaintenance) ?? Array.Empty<DateTime?>());
                date.AddRange(majorHardware.Select(x => x?.EndOfsupport) ?? Array.Empty<DateTime?>());
            }

            var minorDate = date.Where(x => x.HasValue).OrderBy(x => x.Value).FirstOrDefault();
            var today = DateTime.Now;
            var nextMonth = today.AddDays(30);

            constrainInfo.ConstraintScaling = minorDate != null ? minorDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "";

            string status = "";
            string constraintLcm = "";

            if (minorDate >= today && minorDate <= nextMonth)
            {
                status = "amber";
                constraintLcm = "On expiration";
            }
            else if (minorDate < today)
            {
                status = "red";
                constraintLcm = "Expired";
            }
            else if (minorDate >= nextMonth)
            {
                status = "green";
                constraintLcm = "On support";
            }
            constrainInfo.LcmStatus = status;
            constrainInfo.ConstraintLcm = constraintLcm;

            return constrainInfo;
        }

        public async Task RefreshSystemTypeNameOem()
        {
            var st = await _repositoryWrapper.SystemType.FindAll()
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .OrderBy(x => x.Modificationdate).AsNoTracking().ToListAsync();
            foreach (var systemType in st)
            {
                systemType.Systemtypenameoem = systemType.Majorsoftwarebuilds.Productname != null ? 
                    systemType.Majorsoftwarebuilds.Productname.Description:"";

                _repositoryWrapper.SystemType.Update(systemType);
            }

            await _repositoryWrapper.SaveAsync();
        }


    }
}
