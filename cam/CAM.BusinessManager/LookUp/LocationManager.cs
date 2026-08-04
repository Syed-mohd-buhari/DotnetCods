using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsIs;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsPlanned;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.LookUp.Location;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using CAM.DataTransferObjects.Entita.Location;
using Microsoft.EntityFrameworkCore.Internal;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CAM.Repository;
using CAM.Entities.Mappers.Vbom;

namespace CAM.BusinessManager.LookUp
{
   public class LocationManager : GridBaseAsync<Location, LocationDtoGrid, LocationDtoQuery, Locations>
   {

       private readonly IRepositoryWrapper _repositoryWrapper;
       private readonly IMapper _mapper;
       private readonly GridCustomColumnManager _columnManager;

       public LocationManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnManager , 
           IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
       {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
           _columnManager = columnManager;
       }

        

        public override ExpressionStarter<Locations> ApplyFilterForOracleModel(LocationDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Locations>();
            var predicateInner = PredicateBuilder.New<Locations>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Locations>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Location == item);
                predicateResult.And(predicateInner);
            }

            if (request.DefaultValue != null && request.DefaultValue.Any())
            {
                predicateInner = PredicateBuilder.New<Locations>();
                foreach (var item in request.DefaultValue)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Defaultvalue == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Defaultvalue == false);
                    }
                }
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Locations>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Locationid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LocationType != null && request.LocationType.Any())
            {
                predicateInner = PredicateBuilder.New<Locations>();
                foreach (var item in request.LocationType)
                    predicateInner.Or(x => x.Locationdeploymenttypes.Any(d=>d.Deploymenttypeid == item));
                predicateResult.And(predicateInner);
            }
            
            if (request.Opco != null && request.Opco.Any())
            {
                predicateInner = PredicateBuilder.New<Locations>();
                foreach (var item in request.Opco)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Locations>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Locations>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Locations>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            if (request.ShortDescription != null && request.ShortDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Locations>();
                foreach (var item in request.ShortDescription)
                    predicateInner.Or(x => x.Shortdescription == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        public override List<LocationDtoGrid> CastObjectToDto(IQueryable<Location> request)
        {
            var model=  request.Select(dto => new LocationDtoGrid()
            {
                Id = dto.LocationId,
                Description = dto.LocationDescription,
                LastModified = dto.ModificationDate,
                LocationType = dto.Locationdeploymenttypes == null ? "" : string.Join(" | ", dto.Locationdeploymenttypes
                .Select(cw => cw.DeploymentType.DeploymentTypeDescription).Distinct()),
                Opco = (dto.OpCo != null) ? dto.OpCo.OpCoDescription : null,
                OpcoId = (dto.OpCo != null) ? dto.OpCo.OpCoId : (short)0,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                DefaultValue = dto.DefaultValue,
                ShortDescription = dto.ShortDescription
            }).ToList();

            return model;
        }


        public override Dictionary<string, Expression<Func<Location, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Location, object>>[]>
            {
                ["description"] = new Expression<Func<Location, object>>[] { p => p.LocationDescription },
                ["id"] = new Expression<Func<Location, object>>[] { p => p.LocationId },
                ["lastModifiedBy"] = new Expression<Func<Location, object>>[] { p => p.ModificationUserEntity.Email },
                ["locationType"] = new Expression<Func<Location, object>>[] { p => p.Locationdeploymenttypes.FirstOrDefault().DeploymentType.DeploymentTypeDescription },
                ["opcoId"] = new Expression<Func<Location, object>>[] { p => p.OpCo.OpCoId },
                ["opco"] = new Expression<Func<Location, object>>[] { p => p.OpCo.OpCoDescription },
                ["defaultValue"] = new Expression<Func<Location, object>>[] { p => p.DefaultValue },

            };
        }
        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<Location> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.LocationDescription)) : request.Where(x =>
                    x.LocationDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.LocationDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.LocationId.ToString()))
                    : request.Where(x =>
                        x.LocationId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.LocationId.ToString())),

                "defaultValue" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(p => new FilterValueDto { Text = p.DefaultValue ? "YES" : "NO", Value = p.DefaultValue.ToString() }).Distinct()
                : request.Where(x => x.DefaultValue == false)
                .Select(p => new FilterValueDto { Text = p.DefaultValue ? "YES" : "NO", Value = p.DefaultValue.ToString() }).Distinct(),
              
                "opco" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.OpCo != null).Select(x => new FilterValueDto(x.OpCo.OpCoId.ToString(), x.OpCo.OpCoDescription))
                    : request.Where(x => x.OpCo != null &&
                        x.OpcoId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.OpCo.OpCoId.ToString(), x.OpCo.OpCoDescription)),
              

                "locationType" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(p => p.Locationdeploymenttypes != null).SelectMany(x => x.Locationdeploymenttypes).Select(p => new FilterValueDto(p.DeploymentTypeId.ToString(), p.DeploymentType.DeploymentTypeDescription)).Distinct().ToList()
                    : request
                    .Where(x => x.Locationdeploymenttypes != null && x.Locationdeploymenttypes.Any(s => s.DeploymentType.DeploymentTypeDescription.ToUpper().Contains(propertyFilter.ToUpper())))
                    .SelectMany(x => x.Locationdeploymenttypes)
                    .Select(p => new FilterValueDto(p.DeploymentTypeId.ToString(), p.DeploymentType.DeploymentTypeDescription)).Distinct()
                    .ToList(),

                "shortDescription" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.ShortDescription)) : request.Where(x =>
                    x.ShortDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.ShortDescription)),
            };
        }

        public override IQueryable<Location> PrepareQuery(LocationDtoQuery request, ExpressionStarter<Location> predicateResult , ExpressionStarter<Locations> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.Location.FindByCondition(oraclePredicateResult)
                : _repositoryWrapper.Location.FindAll();

            return query
                .Include(x => x.Opco)
                .Include(x => x.Locationdeploymenttypes).ThenInclude(x=>x.Deploymenttype)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => LocationMapper.GetLocationMapper(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(LocationDto dto)
        {
            var entityExists = await _repositoryWrapper.Location.FindByCondition(
               x => x.Location.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ","")
               && x.Opcoid == dto.OpcoId, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Locationid
                };
            }
            //Ticket 702 LCM- Asset location: when we make a location to default location to current OPCO - Other OpCo's default location is removed 
            var otherDefaults = await _repositoryWrapper.Location.FindByCondition(x => x.Defaultvalue == true && x.Opcoid == dto.OpcoId && x.Locationid != dto.Id).ToListAsync();

            if (otherDefaults.Any())
            {
                foreach (var defaultLocation in otherDefaults)
                {
                    defaultLocation.Defaultvalue = false;
                    _repositoryWrapper.Location.Update(defaultLocation);                   
                }
                await _repositoryWrapper.SaveAsync();
            }
            Location entity = new Location() 
            {
                LocationId = dto.Id,
                LocationDescription = dto.Description,
                OpcoId = dto.OpcoId,
                DefaultValue = dto.DefaultValue
            };

            if (dto.LocationTypeIdsList != null )
            {
                foreach (var item in dto.LocationTypeIdsList)
                {
                    var entitiesExists = await _repositoryWrapper.LocationDeploymentTypeRepository.FindByCondition(
                        x => x.Deploymenttypeid == item
                        && x.Locationid == dto.Id, true).FirstOrDefaultAsync();

                    if (entitiesExists != null)
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = entitiesExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                            Data = entitiesExists.Locationid
                        };
                    }
                    entity.Locationdeploymenttypes.Add(new LocationDeploymentTypes() { LocationId = entity.LocationId, DeploymentTypeId = item });
                }
            }
            _repositoryWrapper.Location.Create(LocationMapper.SetLocationMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public ResultDto<LocationDeploymentTypeRelatedEntity> GetLocationDeploymentTypeRelated(int locationId)
        {
            var location = _repositoryWrapper.Location.FindByCondition(x => x.Locationid == locationId).Include(x => x.Locationdeploymenttypes).ThenInclude(x => x.Deploymenttype);
            var related = new LocationDeploymentTypeRelatedEntity();
            related.DeploymentTypeRelatedDict = location.Where(x => x.Locationid == locationId)
                .SelectMany(x => x.Locationdeploymenttypes).Select(x => new { x.Deploymenttypeid, x.Deploymenttype.Deploymenttype}).Distinct().ToDictionary(x => x.Deploymenttypeid, x => x.Deploymenttype);
            return new ResultDto<LocationDeploymentTypeRelatedEntity>()
            {
                Warning = false,
                Data = related,
                Info = ""
            };

        }
        public async Task<ResultDto> Update(LocationDto dto)
        {
            var entityExists = await _repositoryWrapper.Location.FindByCondition(
               x => x.Locationid != dto.Id 
               && x.Location.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
               && x.Opcoid == dto.OpcoId
               && !x.Deleted.Value).FirstOrDefaultAsync();

            //Ticket 702 LCM- Asset location: when we make a location to default location to current OPCO - Other OpCo's default location is removed 
            var otherDefaults = await _repositoryWrapper.Location.FindByCondition(x => x.Defaultvalue == true && x.Opcoid == dto.OpcoId
            && x.Locationid != dto.Id).ToListAsync();
            
            var addedLocationTypes =  _repositoryWrapper.Location.FindByCondition(p => p.Locationid == dto.Id).SelectMany(x=>x.Locationdeploymenttypes).ToList();
            foreach (var item in addedLocationTypes)
            {
                try
                {
                    _repositoryWrapper.LocationDeploymentTypeRepository.DeleteDeep(item);
                }
                catch (Exception ex)
                {

                    throw;
                }
                
            }
            foreach (var item in dto.LocationTypeIdsList)
            {
                _repositoryWrapper.LocationDeploymentTypeRepository.Create(new Locationdeploymenttypes() { Locationid = dto.Id, Deploymenttypeid = item });
            }
            if (otherDefaults.Any())
            {
                foreach (var defaultLocation in otherDefaults)
                {
                    defaultLocation.Defaultvalue = false;
                    _repositoryWrapper.Location.Update(defaultLocation);                   
                }
                await _repositoryWrapper.SaveAsync();
            }

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Locationid
                };
            }
            Location entity = new Location() { LocationId = dto.Id, LocationDescription = dto.Description, 
                //LocationTypeId = dto.LocationTypeId
                
                OpcoId = dto.OpcoId, DefaultValue = dto.DefaultValue };
            _repositoryWrapper.Location.Update(LocationMapper.SetLocationMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.Location.FindByCondition(x => x.Locationid == id).SingleAsync();
            _repositoryWrapper.Location.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Locationid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.Location.FindByCondition(x => x.Locationid == id)
                .Include(x => x.Locationdeploymenttypes)
                .Include(x => x.Damigrationstatus)
                .SingleAsync();
            if (entity.Locationdeploymenttypes != null && entity.Locationdeploymenttypes.Count > 0)
            {
                var locationDeploymentsList = entity.Locationdeploymenttypes.ToList();
                foreach (var toDelete in locationDeploymentsList)
                {
                    _repositoryWrapper.LocationDeploymentTypeRepository.DeleteDeep(toDelete);
                }
                var daMigrationStatus = entity?.Damigrationstatus?.ToList();
                foreach (var toDelete in daMigrationStatus)
                {
                    _repositoryWrapper.DaMigrationStatusRepository.DeleteDeep(toDelete);
                }
            }

            _repositoryWrapper.Location.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Locationid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var NetworkElementAsIs = _repositoryWrapper.NetworkElementAsIs
                .FindByCondition(x => x.Locationid == id)
                .Select(x => NetworkElementAsIsMapper.Get(x).toDescription())
                .ToArray();
            var NetworkElementAsPlanned = _repositoryWrapper.NetworkElementAsPlanned
                .FindByCondition(x => x.Locationid == id)
                .Select(x => NetworkElementAsPlannedMapper.Get(x,true).toDescription())
                .ToArray();

            var VbomReference = _repositoryWrapper.VnfClusterInfoRepository.FindByCondition(x => x.Locationid == id)
                 .Select(x => x.Vnfclusterinfoid.ToString())
                 .ToArray();

            var CbomReference = _repositoryWrapper.CnfClusterInfoRepository.FindByCondition(x => x.Siteid == id)
                 .Select(x => x.Cnfclusterinfoid.ToString())
                 .ToArray();
 
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (NetworkElementAsIs.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Network Element As Is", Values = NetworkElementAsIs });
            if (NetworkElementAsPlanned.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Network Element As Planned", Values = NetworkElementAsPlanned });
            if (VbomReference.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Virtual Bill Of Maintenance", Values = VbomReference });
            if (CbomReference.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Cluster Bill Of Maintenance", Values = CbomReference });

            var entity = await _repositoryWrapper.Location.FindByCondition(x => x.Locationid == id).SingleAsync();


            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Location",
                        RecordName = entity.Location,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public LocationDto GetCreatePage()
        {
            var LocationDto = new LocationDto()
            {
                LocationTypeResource = _repositoryWrapper.DeploymentType.FindAll().ToDictionary(x => x.Deploymenttypeid, x => x.Deploymenttype),
                OpcoResource = _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco),
            }
;
            return LocationDto;
        }
        public LocationDto GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.Location.FindByCondition(x => x.Locationid == id)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Locationdeploymenttypes).ThenInclude(x => x.Deploymenttype).Single();

            var entity = LocationMapper.GetLocationMapper(model);

            entity.Locationdeploymenttypes = model.Locationdeploymenttypes.Select(p => LocationDeploymentTypeMapper.Get(p)).ToList();
            var dto = new LocationDto()
            {
                Id = entity.LocationId,
                Description = entity.LocationDescription,
                LastModifiedBy = entity.ModificationUserEntity.Email,
                LastModified = entity.ModificationDate,
                OpcoId = entity.OpcoId,
                LocationTypeResource = _repositoryWrapper.DeploymentType.FindAll().ToDictionary(x => x.Deploymenttypeid, x => x.Deploymenttype),
                OpcoResource = _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco),
                DefaultValue = entity.DefaultValue,
                LocationTypeIdsList = entity.Locationdeploymenttypes != null ? entity.Locationdeploymenttypes.Select(p => (short)p.DeploymentTypeId).ToList() : null,
            };
            return dto;
        }
    }
}
