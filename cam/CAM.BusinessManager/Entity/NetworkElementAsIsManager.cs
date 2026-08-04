using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.ExtensionMethod.SystemTypesMajorHardwareBuilds;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElementAsIs;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class NetworkElementAsIsManager : BaseManager
    {
        private readonly CommonManager _commonManager;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly ICurrentUserService _currentUserService;
        //private readonly List<short> _opcoList;
        public NetworkElementAsIsManager(CommonManager commonManager,IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
            PlannedActivityManager plannedActivityManager, IHttpContextAccessor contextAccessor, 
            IRepositoryWrapper repositoryWrapper, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _commonManager= commonManager;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRoleOpcoList(this.sessionUserId);
            var _adminRoleCheck = _roleOpcoList.Where(x => x.Role == this.adminRoleId.ToString()).Select(x => x.Role).FirstOrDefault();

           // _opcoList = (_adminRoleCheck != null) ? null : _roleOpcoList.Select(x => Convert.ToInt16(x.OpCo)).Distinct().ToList();
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Networkelementasisid == id)
                .SingleAsync();

            _repositoryWrapper.NetworkElementAsIs.Delete(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Networkelementasplannedid
            };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Networkelementasisid == id)
                .SingleAsync();

            _repositoryWrapper.NetworkElementAsIs.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Networkelementasplannedid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            var entity = await _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Networkelementasisid == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Network Element As is",
                        RecordName = "",
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public async Task<ResultDto> Restore(long id)
        {

            var entity = await _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Networkelementasisid == id, true).SingleAsync();

            entity.Deleted = false;
            entity.Deletiondate = null;

            _repositoryWrapper.NetworkElementAsIs.Update(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.Networkelementasplannedid
            };
        }

        public async Task<ResultDto> Add(NetworkElementAsIsDtoCreate dto, bool? forced = false)
        {
            var entityExists = await _repositoryWrapper.NetworkElementAsIs.FindByCondition(
                       x => x.Opcoid == dto.OpCoId && x.Elementdeploymentname == dto.ElementDeploymentName
                       && x.Systemtypeid == dto.SystemTypeId
                       , true)
                   .OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();


            if (entityExists != null)
            {
                if (entityExists.Deleted == true)
                {
                    if (forced == true)
                    {
                        return await AddBase(dto);
                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.EntryAddExists,
                            Data = new { id = entityExists.Networkelementasisid, orphanDeleted = true }
                        };
                    }
                }
                else
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                        Data = entityExists.Networkelementasisid
                    };
                }
            }
            return await AddBase(dto);
        }

        public async Task<ResultDto> AddBase(NetworkElementAsIsDtoCreate dto)
        {
            var entity = _mapper.Map<NetworkElementAsIs>(dto);
            _repositoryWrapper.NetworkElementAsIs.Create(NetworkElementAsIsMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }

        public async Task<ResultDto> Update(NetworkElementAsIsDtoUpdate dto, bool? forced = false)
        {
            var anotherEntityWithSameNaturalKeyExists = await _repositoryWrapper.NetworkElementAsIs.FindByCondition(
                x => x.Opcoid == dto.OpCoId && x.Elementdeploymentname == dto.ElementDeploymentName
                     && x.Networkelementasisid != dto.NetworkElementAsIsId && x.Systemtypeid == dto.SystemTypeId, true)
                .OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();

            var originalEntityWithSameNaturalKey = await _repositoryWrapper.NetworkElementAsIs.FindByCondition(
                x => x.Networkelementasplannedid == dto.NetworkElementAsPlannedId && x.Elementdeploymentname == dto.ElementDeploymentName
                     && x.Networkelementasisid == dto.NetworkElementAsIsId, true).SingleOrDefaultAsync();


            if (anotherEntityWithSameNaturalKeyExists != null && originalEntityWithSameNaturalKey == null)
            {
                if (anotherEntityWithSameNaturalKeyExists.Deleted == true)
                {
                    if (forced == true)
                    {
                        return await UpdateBase(dto, forced);
                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.EntryUpdateExists,
                            Data = new { id = anotherEntityWithSameNaturalKeyExists.Networkelementasisid, orphanDeleted = true }
                        };
                    }
                }
                else
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = anotherEntityWithSameNaturalKeyExists.Deleted.Value ? ResultMessages.EntryUpdateExistsDeleted : ResultMessages.EntryUpdateExists,
                        Data = anotherEntityWithSameNaturalKeyExists.Networkelementasisid
                    };
                }
            }


            return await UpdateBase(dto, forced);
        }

        private async Task<ResultDto> UpdateBase(NetworkElementAsIsDtoUpdate dto, bool? forced)
        {
            var entity = _mapper.Map<NetworkElementAsIs>(dto);

            var networkElementAsPlanned = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(
                                                                                                x => x.Networkelementasplannedid == entity.NetworkElementAsPlannedId
                                                                                            ).SingleOrDefaultAsync();
            if (networkElementAsPlanned != null)
            {
                networkElementAsPlanned.Locationid = entity.LocationId;
                networkElementAsPlanned.Elementname = entity.ElementDeploymentName;
                _repositoryWrapper.NetworkElementAsPlanned.Update(networkElementAsPlanned);
                var networkElementAsIsRelations = _repositoryWrapper.NetworkElementAsIs.
                        FindByCondition(x => x.Networkelementasplannedid == entity.NetworkElementAsPlannedId && x.Networkelementasisid != entity.NetworkElementAsIsId);
                foreach (var toUpdate in networkElementAsIsRelations)
                {
                    toUpdate.Elementdeploymentname = entity.ElementDeploymentName;
                    toUpdate.Locationid = entity.LocationId;
                    _repositoryWrapper.NetworkElementAsIs.Update(toUpdate);
                }
            }

            if (forced == true)
            {
                entity.Deleted = false;
                entity.DeletionDate = null;
            }
            _repositoryWrapper.NetworkElementAsIs.Update(NetworkElementAsIsMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.NetworkElementAsPlannedId
            };
        }

        public NetworkElementAsIsDtoCreate GetCreatePage(List<short> opcoList=null)
        {
            var locations = _repositoryWrapper.Location.FindAll();
            var oemResource = _repositoryWrapper.OriginalEquipmentManufacturer.FindAll();
            var opcoResource = opcoList !=null && opcoList.Count>0 ?
                              _repositoryWrapper.OpCo.FindByCondition(x=>opcoList.Contains(x.Opcoid)): _repositoryWrapper.OpCo.FindAll();
            var systemTypeResource = _repositoryWrapper.SystemType.FindAll()
                .Include(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Platform)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer);


            var networkElementAsPlannedResource = _repositoryWrapper.NetworkElementAsPlanned.FindAll();
            var model = new NetworkElementAsIsDtoCreate()
            {
                OpCoResource = opcoResource.ToDictionary(x => x.Opcoid, x => x.Opco),
                DataAcquisitionMethod = ConstantValueFilter.Manual,
                OriginalEquipmentManufacturerResource = oemResource.ToDictionary(x => x.Orgeqpmanufacturerid,
                    x => x.Originalequipmentmanufacturer),
                LocationResource = locations.ToDictionary(x => x.Locationid, x => x.Location),
                SystemTypeResource = systemTypeResource
                .ToDictionary(x => x.Systemtypeid, x => x.toSystemTypeName(_repositoryWrapper)),
                NetworkElementAsPlannedResource = networkElementAsPlannedResource.ToDictionary(x => x.Networkelementasplannedid, x => x.Elementname)
            };
            return model;
        }

        public NetworkElementAsIsDtoUpdate GetUpdatePage(long id)
        {
            var entity = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Networkelementasisid == id, ConstantValueFilter.isTrue)
                .Include(x => x.Networkelementasplanned)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Systemtype)
                .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer).Include(x => x.Opco)
                .Single();
            var dto = _mapper.Map<NetworkElementAsIsDtoUpdate>(NetworkElementAsIsMapper.Get(entity));

            var opocResource = _repositoryWrapper.OpCo.FindAll();
            dto.OpCoResource = opocResource.ToDictionary(x => x.Opcoid, x => x.Opco);
            if (!dto.OpCoResource.ContainsKey(dto.OpCoId))
            {
                var data = _repositoryWrapper.OpCo.FindByCondition(
                    x => x.Opcoid == dto.OpCoId, ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.OpCoResource.Add(data.Opcoid, data.Opco);
                }
            }

            var locations = _repositoryWrapper.Location.FindAll();
            dto.LocationResource = locations.ToDictionary(x => x.Locationid, x => x.Location);
            if (!dto.LocationResource.ContainsKey(dto.LocationId))
            {
                var data = _repositoryWrapper.Location.FindByCondition(
                    x => x.Locationid == dto.LocationId, ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.LocationResource.Add(data.Locationid, data.Location);
                }
            }
            var oemResource = _repositoryWrapper.OriginalEquipmentManufacturer.FindAll();
            dto.OriginalEquipmentManufacturerResource = oemResource.ToDictionary(x => x.Orgeqpmanufacturerid,
                    x => x.Originalequipmentmanufacturer);
            if (!dto.OriginalEquipmentManufacturerResource.ContainsKey(dto.OriginalEquipmentManufacturerId))
            {
                var data = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(
                    x => x.Orgeqpmanufacturerid == dto.OriginalEquipmentManufacturerId, ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.LocationResource.Add(data.Orgeqpmanufacturerid, data.Originalequipmentmanufacturer);
                }
            }

            var systemTypeResource = _repositoryWrapper.SystemType.FindAll();
            dto.SystemTypeResource = systemTypeResource
                                        .Include(x => x.Majorsoftwarebuilds)
                                        .ThenInclude(x => x.Orgeqpmanufacturer)
                                        .Include(x => x.Systemtypesmajorhardwarebuilds)
                                        .ThenInclude(x => x.Majorhardware)
                                        .ThenInclude(x => x.Platform)
                                        .ToDictionary(x => x.Systemtypeid, x => x.toSystemTypeName(_repositoryWrapper));
            var networkElementAsPlannedResource = _repositoryWrapper.NetworkElementAsPlanned.FindAll();
            return dto;
        }

        private static ExpressionStarter<Networkelementsasis> ApplyFilter(NetworkElementsAsIsQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Networkelementsasis>(true);
            var predicateInner = PredicateBuilder.New<Networkelementsasis>(true);

            if (buildFilterDto.NetworkElementAsIsId != null && buildFilterDto.NetworkElementAsIsId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.NetworkElementAsIsId)
                    predicateInner.Or(x => x.Networkelementasisid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.OpCo)
                    predicateInner.Or(x => x.Opco.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ElementDeploymentName != null && buildFilterDto.ElementDeploymentName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.ElementDeploymentName)
                    predicateInner.Or(x => x.Elementdeploymentname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Location != null && buildFilterDto.Location.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.Location)
                    predicateInner.Or(x => x.Location.Locationid == item);
                predicateResult.And(predicateInner);
            }
            //if (buildFilterDto.NetworkFunction != null)
            //{
            //    predicateInner = PredicateBuilder.New<NetworkElementAsIs>();
            //    foreach (var item in buildFilterDto.NetworkFunction)
            //        predicateInner.Or(x => x.NetworkFunction == item);
            //    predicateResult.And(predicateInner);
            //}

            if (buildFilterDto.NodeType != null && buildFilterDto.NodeType.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.NodeType)
                    predicateInner.Or(x => x.Nodetype == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.HardwareSolution != null && buildFilterDto.HardwareSolution.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.HardwareSolution)
                    predicateInner.Or(x =>

                        item.Contains(x.Systemtype.Systemtypesmajorhardwarebuilds.Where(x => x.Ismain && !x.Deleted.Value).SingleOrDefault().Majorhardware.Platform.Platform) &&
                        item.Contains(x.Systemtype.Systemtypesmajorhardwarebuilds.Where(x => x.Ismain && !x.Deleted.Value).SingleOrDefault().Majorhardware.Hardwaretype)

                        );
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ManualOverride != null && buildFilterDto.ManualOverride.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.ManualOverride)
                    predicateInner.Or(x => x.Manualoverride == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Platform != null && buildFilterDto.Platform.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.Platform)
                    predicateInner.Or(x=>x.Platformtype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HardwareType != null && buildFilterDto.HardwareType.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.HardwareType)
                    predicateInner.Or(x => x.Hardwaretype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OtherHardwareInfo != null && buildFilterDto.OtherHardwareInfo.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.OtherHardwareInfo)
                    predicateInner.Or(x => x.Systemtype.Systemtypesmajorhardwarebuilds.Where(x => x.Ismain && !x.Deleted.Value).SingleOrDefault().Majorhardware.Otherhardwareinfo == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SoftwareReleaseInformation != null && buildFilterDto.SoftwareReleaseInformation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.SoftwareReleaseInformation)
                    predicateInner.Or(x => x.Softwarereleaseinformation == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto.SoftwareProductNumber != null && buildFilterDto.SoftwareProductNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.SoftwareProductNumber)
                    predicateInner.Or(x => x.Softwareproductnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SoftwareProductionDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                if (buildFilterDto.SoftwareProductionDateValue.StartDate != null)
                    predicateInner.And(x => x.Softwareproductiondate >= buildFilterDto.SoftwareProductionDateValue.StartDate);

                if (buildFilterDto.SoftwareProductionDateValue.EndDate != null)
                    predicateInner.And(x => x.Softwareproductiondate <= buildFilterDto.SoftwareProductionDateValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SoftwareInstallDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                if (buildFilterDto.SoftwareInstallDateValue.StartDate != null)
                    predicateInner.And(x => x.Softwareinstalldate >= buildFilterDto.SoftwareInstallDateValue.StartDate);

                if (buildFilterDto.SoftwareInstallDateValue.EndDate != null)
                    predicateInner.And(x => x.Softwareinstalldate <= buildFilterDto.SoftwareInstallDateValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HardwareAcquisition != null && buildFilterDto.HardwareAcquisition.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.HardwareAcquisition)
                    predicateInner.Or(x => x.Hardwareacquisition == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DataAcquisitionDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                if (buildFilterDto.DataAcquisitionDateValue.StartDate != null)
                    predicateInner.And(x => x.Dataacquisitiondate >= buildFilterDto.DataAcquisitionDateValue.StartDate);

                if (buildFilterDto.DataAcquisitionDateValue.EndDate != null)
                    predicateInner.And(x => x.Dataacquisitiondate <= buildFilterDto.DataAcquisitionDateValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DataAcquisitionMethod != null && buildFilterDto.DataAcquisitionMethod.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.DataAcquisitionMethod)
                    predicateInner.Or(x => x.Dataacquisitionmethod == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ElementManager != null && buildFilterDto.ElementManager.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.ElementManager)
                    predicateInner.Or(x => x.Elementmanager == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ElementManagerExportFileFormat != null && buildFilterDto.ElementManagerExportFileFormat.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.ElementManagerExportFileFormat)
                    predicateInner.Or(x => x.Elementmanagerexportfileformat == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Oem != null && buildFilterDto.Oem.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.Oem)
                    predicateInner.Or(x => x.Orgeqpmanufacturerid == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                if (buildFilterDto.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModifiedValue.StartDate);

                if (buildFilterDto.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto.SoftwareInstallDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                if (buildFilterDto.SoftwareInstallDateValue.StartDate != null)
                    predicateInner.And(x => x.Softwareinstalldate >= buildFilterDto.SoftwareInstallDateValue.StartDate);

                if (buildFilterDto.SoftwareInstallDateValue.EndDate != null)
                    predicateInner.And(x => x.Softwareinstalldate <= buildFilterDto.SoftwareInstallDateValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.HardwareInstallDate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasis>();
                if (buildFilterDto.HardwareInstallDate.StartDate != null)
                    predicateInner.And(x => x.Hardwareinstalldate >= buildFilterDto.HardwareInstallDate.StartDate);

                if (buildFilterDto.HardwareInstallDate.EndDate != null)
                    predicateInner.And(x => x.Hardwareinstalldate <= buildFilterDto.HardwareInstallDate.EndDate);
                predicateResult.And(predicateInner);
            }

            //if (buildFilterDto.ElementName != null && buildFilterDto.ElementName.Any())
            //{
            //    predicateInner = PredicateBuilder.New<NetworkElementAsIs>();
            //    foreach (var item in buildFilterDto.ElementName)
            //        predicateInner.Or(x => x.ElementDeploymentName == item);
            //    predicateResult.And(predicateInner);
            //}

            return predicateResult;
        }


        public QueryResultDto<NetworkElementAsIsDtoGrid> FindWithCondition(NetworkElementsAsIsQueryDto designComponentFilterDto)
        {
            var predicateResult = ApplyFilter(designComponentFilterDto);
            if (designComponentFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
           
             var query = GetQuery(predicateResult, designComponentFilterDto.Deleted ?? false)
                .ApplyOrdering(designComponentFilterDto, GetColumnsMap());

            //if (_opcoList != null && _opcoList.Count > 0)
            //    query = query.Where(
            //    x => _opcoList.Contains((short)x.OpCoId)
            // );
            if (designComponentFilterDto.VerticalName?.Any() == true && !designComponentFilterDto.VerticalName.Contains("yes"))
            {
                query = query.Where(x =>
                x.VerticalFilterDto != null && x.VerticalFilterDto.Any(c => x.VerticalFilterDto != null && designComponentFilterDto.VerticalName.Contains(c.Key.ToString())));
            }
            else if (designComponentFilterDto.VerticalName?.Any() == true && designComponentFilterDto.VerticalName.Count() == 1
                && designComponentFilterDto.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0));
            }
            else if (designComponentFilterDto.VerticalName?.Any() == true && designComponentFilterDto.VerticalName.Count() > 1 && designComponentFilterDto.VerticalName.Contains("yes"))
            {
                var nullVerticals = designComponentFilterDto.VerticalName.Contains("yes") ?
                    query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0)) : null;
                var verticalFilter = query
                                    .Where(x => x.VerticalFilterDto != null &&
                                    x.VerticalFilterDto.Any(c => designComponentFilterDto.VerticalName.Where(t => t != "yes").Contains(c.Key.ToString())));
                query = nullVerticals?.Any() == true && verticalFilter?.Any() == true ?
                    nullVerticals.Concat(verticalFilter) : nullVerticals?.Any() == true && verticalFilter
                    ?.Any() == false ? nullVerticals
                    : nullVerticals?.Any() == false && verticalFilter?.Any() == true ? verticalFilter : null;
            }
            var rtn = new QueryResultDto<NetworkElementAsIsDtoGrid>(new GenerateRenderForGrid<NetworkElementAsIsDtoGrid>(_manager))
            {
                TotalItems = query.Count(),
            };

            query = query.ApplyPaging(designComponentFilterDto);


            var data = query
                       .Include(x => x.SystemType).ThenInclude(x => x.MajorSoftwareBuilds).ThenInclude(x => x.OriginalEquipmentManufacturer)
                       .Include(x => x.SystemType).ThenInclude(x => x.SystemTypesMajorHardwareBuilds).ThenInclude(x => x.MajorHardware).ThenInclude(x => x.Platform)
                       .Include(x => x.OpCo)
                       .ToList();

            IEnumerable<NetworkElementAsIsDtoGrid> networkelementAsIsResult;
            networkelementAsIsResult = _mapper.Map<IEnumerable<NetworkElementAsIsDtoGrid>>(data);

            rtn.Items = networkelementAsIsResult.ToArray();
            return rtn;
        }

        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, NetworkElementsAsIsQueryDto buildFilterDto,bool isAdmin=false)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false).Include(x => x.SystemType);

            var rtn = propertyName switch
            {
                "verticalName" => query.ToList().Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0)
                                 .SelectMany(p => p.VerticalFilterDto.Select(m =>
                                 new FilterValueDto
                                 {
                                     Value = m.Key.ToString(),
                                     Text = m.Value
                                 }))?.DistinctBy(y=>y.Text)?.ToList()
                                 .Concat(
                                     (query.AsEnumerable().Where(x => x.VerticalFilterDto == null ||(x.VerticalFilterDto != null && x.VerticalFilterDto.Count == 0)))
                                     .Select(x =>
                                             _commonManager.AddBlankFilterValue()
                                     )
                                 )
                                 .Distinct().ToList(),
                "networkElementAsPlannedId" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.NetworkElementAsPlannedId.ToString(), Value = p.NetworkElementAsPlannedId.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.NetworkElementAsPlannedId.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.NetworkElementAsPlannedId.ToString(), Value = p.NetworkElementAsPlannedId.ToString() }).Distinct()
                    .ToList(),

                "networkElementAsIsId" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.NetworkElementAsIsId.ToString(), Value = p.NetworkElementAsIsId.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.NetworkElementAsIsId.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.NetworkElementAsIsId.ToString(), Value = p.NetworkElementAsIsId.ToString() }).Distinct()
                    .ToList(),

                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),

                "softwareProductNumber" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.SoftwareProductNumber, Value = p.SoftwareProductNumber }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.SoftwareProductNumber.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.SoftwareProductNumber, Value = p.SoftwareProductNumber }).Distinct().ToList(),

                "hardwareAcquisition" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.HardwareAcquisition, Value = p.HardwareAcquisition }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.HardwareAcquisition.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.HardwareAcquisition, Value = p.HardwareAcquisition }).Distinct().ToList(),

                "elementManagerExportFileFormat" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ElementManagerExportFileFormat, Value = p.ElementManagerExportFileFormat }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ElementManagerExportFileFormat.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ElementManagerExportFileFormat, Value = p.ElementManagerExportFileFormat }).Distinct().ToList(),


                "elementManager" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ElementManager, Value = p.ElementManager }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ElementManager.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ElementManager, Value = p.ElementManager }).Distinct().ToList(),


                "dataAcquisitionMethod" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.DataAcquisitionMethod, Value = p.DataAcquisitionMethod }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.DataAcquisitionMethod.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.DataAcquisitionMethod, Value = p.DataAcquisitionMethod }).Distinct().ToList(),

                "nodeType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.NodeType, Value = p.NodeType }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.NodeType.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.NodeType, Value = p.NodeType }).Distinct().ToList(),


                "elementDeploymentName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.ElementDeploymentName, Value = p.ElementDeploymentName }).Distinct().ToList()
                    : query
                        .Where(x => x.ElementDeploymentName.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.ElementDeploymentName, Value = p.ElementDeploymentName }).Distinct()
                        .ToList(),

                "opCo" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.OpCo.OpCoDescription,
                        Value = p.OpCoId.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.OpCo.OpCoDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.OpCo.OpCoDescription,
                                    Value = p.OpCoId.ToString()
                                }).Distinct().ToList(),

                "location" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.Location.LocationDescription,
                        Value = p.LocationId.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Location.LocationDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.Location.LocationDescription,
                                    Value = p.LocationId.ToString()
                                }).Distinct().ToList(),


                "platform" => string.IsNullOrEmpty(propertyFilter)
                            ? query.Select(p => new FilterValueDto
                            {
                                Text = p.PlatformType,
                                Value = p.PlatformType
                            }).Distinct().ToList()
                            : query
                                .Where(x => x.PlatformType.Contains(propertyFilter))
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.PlatformType,
                                    Value = p.PlatformType
                                }).Distinct().ToList(),

                "hardwareSolution" => string.IsNullOrEmpty(propertyFilter)
                            ? query.ToList().Select(p => new FilterValueDto
                            {
                                Text = p.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().toHardwareSolution(),
                                Value = p.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().toHardwareSolution()
                            }).Distinct().ToList()
                            : query.ToList()
                                .Where(x => x.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().toHardwareSolution().ToUpper().Contains(propertyFilter.ToUpper()))
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().toHardwareSolution(),
                                    Value = p.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().toHardwareSolution()
                                }).Distinct().ToList(),
                "hardwareType" => string.IsNullOrEmpty(propertyFilter)
                            ? query.Select(p => new FilterValueDto
                            {
                                Text = p.HardwareType,
                                Value = p.HardwareType
                            }).Distinct().ToList()
                            : query
                                .Where(x => x.HardwareType.Contains(propertyFilter))
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.HardwareType,
                                    Value = p.HardwareType
                                }).Distinct().ToList(),
                "otherHardwareInfo" => string.IsNullOrEmpty(propertyFilter)
                            ? query.Select(p => new FilterValueDto
                            {
                                Text = p.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().MajorHardware.OtherHardwareInfo,
                                Value = p.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().MajorHardware.OtherHardwareInfo
                            }).Distinct().ToList()
                            : query
                                .Where(x => x.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().MajorHardware.OtherHardwareInfo.Contains(propertyFilter))
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().MajorHardware.OtherHardwareInfo,
                                    Value = p.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().MajorHardware.OtherHardwareInfo
                                }).Distinct().ToList(),
                "softwareReleaseInformation" => string.IsNullOrEmpty(propertyFilter)
                            ? query.ToList().Select(p => new FilterValueDto
                            {
                                Text = p.SoftwareReleaseInformation,
                                Value = p.SoftwareReleaseInformation,
                            }).Distinct().ToList()
                            : query.ToList()
                                .Where(x => x.SoftwareReleaseInformation.Contains(propertyFilter))
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.SoftwareReleaseInformation,
                                    Value = p.SoftwareReleaseInformation,
                                }).Distinct().ToList(),
                "manualOverride" => string.IsNullOrEmpty(propertyFilter)
                            ? query.Select(p => new FilterValueDto
                            {
                                Text = p.ManualOverride ?  ConstantValueFilter.Yes .ToUpper() : ConstantValueFilter.No.ToUpper(),
                                Value = p.ManualOverride ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()
                            }).Distinct().ToList()
                            : query
                                .Where(x => (x.ManualOverride ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter))
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.ManualOverride ? ConstantValueFilter.Yes.ToUpper()  : ConstantValueFilter.No .ToUpper() ,
                                    Value = p.ManualOverride ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()
                                }).Distinct().ToList(),

                "hardwareInstallDate" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.HardwareInstallDate.ToString(), Value = p.HardwareInstallDate.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.HardwareInstallDate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.HardwareInstallDate.ToString(), Value = p.HardwareInstallDate.ToString() }).Distinct().ToList(),


                _ => new List<FilterValueDto>()
            };


            //if ((_opcoList != null) && (!string.IsNullOrEmpty(propertyName)))
            //    rtn = ("opCo" == propertyName.ToString()) ?
            //       (rtn.Where(x => _opcoList.Contains(Convert.ToInt16(x.Value.ToString())))).ToList<FilterValueDto>()
            //       : rtn;
            if (!isAdmin && (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count > 0) && propertyName == "verticalName")
            {
                rtn = rtn.Where(x => buildFilterDto.VerticalName.Contains(x.Value.ToString())).ToList();
            }
            return rtn;
        }

        private IQueryable<NetworkElementAsIs> GetQuery(ExpressionStarter<Networkelementsasis> predicateResult, bool includeDeleted)
        {
            var query = _repositoryWrapper.NetworkElementAsIs.FindByCondition(predicateResult, includeDeleted)
                    .Include(x => x.Location)
                    .Include(x => x.Opco)
                    .Include(x => x.ModificationuserNavigation)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x=>x.Networkelementasplanned).ThenInclude(x=>x.Networkelementasplannedsubdomainspoc);

            var queryToFilter= query.AsEnumerable().Select(x => NetworkElementAsIsMapper.Get(x)).AsQueryable();
            var entityList = queryToFilter.ToList();
            foreach (var item in entityList)
            {
                if (item.NetworkElementAsPlannedSubdomainSpoc?.Any() == true)
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item.NetworkElementAsPlannedSubdomainSpoc,
                        0, false, true)?.Distinct()?.ToDictionary(m => Convert.ToInt16(m.Value), m => m.Text);

            }

            return entityList.AsQueryable();
        }

        private Dictionary<string, Expression<Func<NetworkElementAsIs, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<NetworkElementAsIs, object>>[]>
            {
                ["networkElementAsIsId"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.NetworkElementAsIsId },
                ["opCoId"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.OpCo.OpCoId },
                ["lastModified"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.ModificationUserEntity.Email },
                ["networkElementAsPlannedId"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.NetworkElementAsIsId },
                ["elementDeploymentName"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.ElementDeploymentName },
                ["location"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.Location.LocationDescription },
                ["systemTypeId"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.SystemTypeId },
                ["originalEquipmentManufacturerId"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.OriginalEquipmentManufacturerId },
                ["locationId"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.LocationId },
                ["softwareProductNumber"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.SoftwareProductNumber },
                ["elementManager"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.ElementManager },
                ["patchDetails"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.PatchDetails },
                ["softwareProductionDate"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.SoftwareProductionDate },
                ["softwareInstallDate"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.SoftwareInstallDate },
                ["dataAcquisitionDate"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.DataAcquisitionDate },
                ["dataAcquisitionMethod"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.DataAcquisitionMethod },
                ["hardwareAcquisition"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.HardwareAcquisition },
                ["manualOverride"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.ManualOverride },
                ["nodeType"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.NodeType },
                ["elementManagerExportFileFormat"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.ElementManagerExportFileFormat },
                ["hardwareInstallDate"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.HardwareInstallDate },
                //["softwareReleaseInformation"] = new Expression<Func<NetworkElementAsIs, object>>[] { p => p.toSoftwareReleaseInformation() },




            };
        }

        public async Task<ResultDto> GetSystemTypeList(short? oemId)
        {
            if (oemId != null)
            {

                var sysToReturn = new List<SystemType>();

                var data = await _repositoryWrapper.SystemType
                    .FindByCondition(x => x.Majorsoftwarebuilds.Orgeqpmanufacturerid == oemId && x.Deleted == !ConstantValueFilter.isTrue)
                    .Include(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware)
                    .ThenInclude(x => x.Platform)
                    .Include(x => x.Majorsoftwarebuilds)
                    .ThenInclude(x => x.Orgeqpmanufacturer)
                    .Where(x => x.Systemtypesmajorhardwarebuilds.Any(s => s.Deleted == false && s.Ismain == ConstantValueFilter.isTrue)).ToListAsync();

                var m = data.Distinct();
                var lista = m.ToDictionary(x => x.Systemtypeid, x => x.toSystemTypeName(_repositoryWrapper));


                if (lista.Any())
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.GetInfoSuccess,
                        Warning = false,
                        Data = lista

                    };
                }
                else
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.NoSystemType,
                        Warning = false,
                        Data = lista

                    };
                }
            }
            else
            {
                var stResource = _repositoryWrapper.SystemType.FindAll();
                var lista = stResource
                    .Include(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Majorsoftwarebuilds)
                    .ThenInclude(x => x.Orgeqpmanufacturer)
                    .ToDictionary(x => x.Systemtypeid, x => x.toSystemTypeName(_repositoryWrapper));

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = lista
                };

            }

        }
        public async Task<ResultDto> GetSystemTypeListFromAsPlanned(long networkElementAsPlannedId)
        {

            var sysToReturn = new List<SystemType>();

            var majorSoftwareBuilds = await _repositoryWrapper.NetworkElementAsPlanned
                                                .FindByCondition(x => x.Networkelementasplannedid == networkElementAsPlannedId)
                                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                                                .Select(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds)
                                                .SingleOrDefaultAsync();

            var systemTypePlatformsId = await _repositoryWrapper.NetworkElementAsPlanned
                                                .FindByCondition(x => x.Networkelementasplannedid == networkElementAsPlannedId)
                                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                                                .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                                .Select(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardware.Platform.Platformid)
                                                .SingleOrDefault())
                                                .Distinct()
                                                .ToListAsync();

            if (majorSoftwareBuilds != null && systemTypePlatformsId != null)
            {
                sysToReturn = await _repositoryWrapper.SystemType.FindAll()
                .Include(x => x.Majorsoftwarebuilds)
                .ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Where(x =>
                   x.Majorsoftwarebuilds.Majorsoftwarebuildsid == majorSoftwareBuilds.Majorsoftwarebuildsid &&
                   x.Systemtypesmajorhardwarebuilds
                   .Any(x => systemTypePlatformsId.Contains(x.Majorhardware.Platform.Platformid))

                )
                .Select(p => SystemTypeMapper.GetSystemTypeMapper(p, true))
                .ToListAsync();
            }


            if (sysToReturn.Any())
            {
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = sysToReturn.ToDictionary(x => x.SystemTypeId, x => SystemTypeMapper.SetSystemTypeMapper(x).toSystemTypeName(_repositoryWrapper))

                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.NoSystemType,
                    Warning = true,
                    Data = null

                };
            }

        }

        public async Task<ResultDto> GetSystemTypeInfo(long systemId)
        {
            var systems = await _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == systemId, ConstantValueFilter.isTrue)
                .Include(x => x.Majorsoftwarebuilds)
                .Include(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Platform)
                .SingleOrDefaultAsync();

            var mhb = systems?.Systemtypesmajorhardwarebuilds.Where(x => x.Ismain && x.Deleted == !ConstantValueFilter.isTrue).Select(x =>
                    new
                    {
                        Platform = x.Majorhardware.Platform.Platform,
                        HardwareType = x.Majorhardware.Hardwaretype,
                        HardwareSolution = SystemTypesMajorHardwareBuildMapper.GetSystemTypesMajorHardwareBuildMapper(x).toHardwareSolution(),
                        OtherHardwareInfo = x.Majorhardware.Otherhardwareinfo,
                    })
                .SingleOrDefault();


            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Warning = false,
                Data = new NetworkElementAsIsSystemTypeInfo
                {
                    Platform = mhb?.Platform ?? "",
                    HardwareType = mhb?.HardwareType ?? "",
                    HardwareSolution = mhb?.HardwareSolution ?? "",
                    OtherHardwareInfo = mhb?.OtherHardwareInfo ?? "",
                    SoftwareReleaseInformation = systems?.Majorsoftwarebuilds != null ? systems?.Majorsoftwarebuilds?.Softwareversion : "",
                    NetworkFunction = systems?.Majorsoftwarebuilds?.Productname != null ? systems?.Majorsoftwarebuilds?.Productname.Description : "",

                }

            };
        }


        public async Task<ResultDto> GetNetworkElementAsPlannedResource(long systemId, int opcoId)
        {
            List<NetworkElementAsPlanned> asPlanned = new List<NetworkElementAsPlanned>();
            var majorSoftwareBuilds = await _repositoryWrapper.SystemType
                                                .FindByCondition(x => x.Systemtypeid == systemId)
                                                .Include(x => x.Majorsoftwarebuilds)
                                                .Select(x => x.Majorsoftwarebuilds)
                                                .SingleOrDefaultAsync();

            var systemTypePlatformsId = await _repositoryWrapper.SystemType
                                                .FindByCondition(x => x.Systemtypeid == systemId)
                                                .Include(x => x.Systemtypesmajorhardwarebuilds)
                                                .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                                .Select(x => x.Systemtypesmajorhardwarebuilds
                                                .Select(x => x.Majorhardware.Platform.Platformid).SingleOrDefault())
                                                .Distinct()
                                                .ToListAsync();

            if (majorSoftwareBuilds != null && systemTypePlatformsId != null)
            {
                asPlanned = await _repositoryWrapper.NetworkElementAsPlanned.FindAll()
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Where(x =>
                            x.Opcoid == opcoId &&
                            x.Designcomponent.Systemtype.Majorsoftwarebuilds.Majorsoftwarebuildsid == majorSoftwareBuilds.Majorsoftwarebuildsid &&
                            x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(x => systemTypePlatformsId.Contains(x.Majorhardware.Platform.Platformid))
                    )
                    .Select(p => NetworkElementAsPlannedMapper.Get(p, ConstantValueFilter.isTrue))
                    .ToListAsync();
            }
            if (asPlanned.Any())
            {
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = asPlanned.ToDictionary(x => x.NetworkElementAsPlannedId, x => x.ElementName)

                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.NoNetworkElement,
                    Warning = true,
                    Data = null

                };
            }

        }

        public async Task<ResultDto> GetAsPlannedLocation(long asPlannedId)
        {
            var asPlanned = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == asPlannedId).SingleOrDefaultAsync();
            if (asPlanned.Locationid != null)
            {
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = asPlanned.Locationid
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.NoNetworkElement,
                    Warning = false,
                    Data = null

                };
            }

        }
    }
}
