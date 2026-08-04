using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Models;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.LookUp.ReasonCheckbox;
using CAM.Entities.Models.Lookup;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
    public class ReasonCheckboxResourcesManager : GridBaseAsync<ReasonCheckboxResource, ReasonCheckboxDto, ReasonCheckboxQueryDto, Reasoncheckboxresources>
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;


        public ReasonCheckboxResourcesManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper; 
            _mapper = mapper;
            _columnManager = columnManager;
        }

  
        public override ExpressionStarter<Reasoncheckboxresources> ApplyFilterForOracleModel(ReasonCheckboxQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Reasoncheckboxresources>();
            var predicateInner = PredicateBuilder.New<Reasoncheckboxresources>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Reasoncheckboxresources>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Reasoncheckboxresources>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Reasoncheckboxresources>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Reasoncheckboxresources>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Reasoncheckboxresources>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<ReasonCheckboxDto> CastObjectToDto(IQueryable<ReasonCheckboxResource> request)
        {
            return  request.Select(dto => new ReasonCheckboxDto()
            {
                Id = dto.Id,
                Description = dto.Description,
                LastModified = dto.ModificationDate,
                IsHardware = dto.IsHardware,
                IsSoftware = dto.IsSoftware,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<ReasonCheckboxResource, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ReasonCheckboxResource, object>>[]>
            {
                ["description"] = new Expression<Func<ReasonCheckboxResource, object>>[] { p => p.Description },
                ["lastModifiedBy"] = new Expression<Func<ReasonCheckboxResource, object>>[] { p => p.ModificationUserEntity.Email },
                ["id"] = new Expression<Func<ReasonCheckboxResource, object>>[] { p => p.Description }

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<ReasonCheckboxResource> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Description)) : request.Where(x =>
                      x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.Id.ToString()))
                : request.Where(x =>
                    x.Id.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Id.ToString())),

            };
        }

        public override IQueryable<ReasonCheckboxResource> PrepareQuery(ReasonCheckboxQueryDto request, ExpressionStarter<ReasonCheckboxResource> predicateResult , ExpressionStarter<Reasoncheckboxresources> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.ReasonCheckboxResource.FindByCondition(oraclePredicateResult)

                : _repositoryWrapper.ReasonCheckboxResource.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => ReasonCheckboxResourceMapper.GetReasonCheckboxResourceMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(ReasonCheckboxDto data)
        {
            var entityExists = await _repositoryWrapper.ReasonCheckboxResource.FindByCondition(
               x => x.Id == data.Id, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            ReasonCheckboxResource entity = new ReasonCheckboxResource() { Id = data.Id, Description = data.Description,IsHardware = data.IsHardware,IsSoftware = data.IsSoftware};
            _repositoryWrapper.ReasonCheckboxResource.Create(ReasonCheckboxResourceMapper.SetReasonCheckboxResourceMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(ReasonCheckboxDto dto)
        {
            var entityExists = await _repositoryWrapper.ReasonCheckboxResource.FindByCondition(
               x => x.Id != dto.Id && x.Description == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            ReasonCheckboxResource entity = new ReasonCheckboxResource() { Id = dto.Id, Description = dto.Description, IsHardware = dto.IsHardware, IsSoftware = dto.IsSoftware };
            _repositoryWrapper.ReasonCheckboxResource.Update(ReasonCheckboxResourceMapper.SetReasonCheckboxResourceMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.ReasonCheckboxResource.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.ReasonCheckboxResource.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.ReasonCheckboxResource.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.ReasonCheckboxResource.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            var hardware = _repositoryWrapper.CheckboxResourceLcmEngineeringHardware.FindByCondition(x => x.Reasoncheckboxresourceid == id)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x=>x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                 .Select(x => x.Lcmengineering.toDescription(_repositoryWrapper))
                .ToArray();
            if (hardware.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "LCM Engineering - Hardware", Values = hardware });

            var software = _repositoryWrapper.CheckboxResourceLcmEngineeringSoftware
                          .FindByCondition(x => x.Reasoncheckboxresourceid == id)
                          .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                          .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                          .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x=>x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                          .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                          .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                          .Select(x => x.Lcmengineering.toDescription(_repositoryWrapper))
                          .ToArray();

            if (software.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "LCM Engineering - Software", Values = software });

            var entity = await _repositoryWrapper.ReasonCheckboxResource.FindByCondition(x => x.Id == id).SingleAsync();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Reason Checkbox",
                        RecordName = entity.Description,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public ReasonCheckboxDto GetCreatePage()
        {
            var opCoDto = new ReasonCheckboxDto();
            return opCoDto;
        }

        public ReasonCheckboxDto GetUpdatePage(short id)
        {
            var entity = ReasonCheckboxResourceMapper.GetReasonCheckboxResourceMapper( _repositoryWrapper.ReasonCheckboxResource
                .FindByCondition(x => x.Id == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new ReasonCheckboxDto() { Id = entity.Id, LastModifiedBy = entity.ModificationUserEntity.Email, Description = entity.Description, LastModified = entity.ModificationDate ,IsHardware = entity.IsHardware,IsSoftware = entity.IsSoftware};
            return dto;
        }

    }
}


 