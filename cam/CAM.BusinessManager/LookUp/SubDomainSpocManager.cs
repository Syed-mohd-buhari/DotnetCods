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
using CAM.DataTransferObjects.LookUp;
using CAM.Entities.Models.Lookup;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsPlanned;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.DataTransferObjects.LookUp.SubDomainSpoc;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using CAM.Infrastucture.QueryResult;
using AutoMapper;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
    public class SubDomainSpocManager : GridBaseAsync<SubDomainSpoc, TipologicaGridDto, TipologicaQueryDto, Subdomainspocs>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly IMapper _mapper;

        public SubDomainSpocManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, IMapper mapper,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
        }
        public QueryResultDto<SubDomainSpocGridDto> FindWithCondition(SubDomainSpocQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);
            if (buildFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            if (buildFilterDto.Orphan == true)
            {
                predicateResult/*.And(x => !x.Lcmengineeringeduspoc.Any())
                    .And(x => !x.Networkelementasplannededuspoc.Any())
                    .And(x => !x.Networkelementasplannedsubdomainspoc.Any())
                    .And(x => x.Lcmengineeringsubdomainspoc.Any())*/
                    .And(x => x.Systemtypessubdomainspoc.Any());
            }

            var rtn = new QueryResultDto<SubDomainSpocGridDto>(new GenerateRenderForGrid<SubDomainSpocGridDto>(_columnManager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.SubDomainSpoc.Count(predicateResult) : _repositoryWrapper.SubDomainSpoc.Count(),
            };

            var query = predicateResult.IsStarted ? _repositoryWrapper.SubDomainSpoc.FindByCondition(predicateResult, buildFilterDto.Deleted ?? false)

                            .Include(x => x.ModificationuserNavigation)
                            .Include(x => x.CreationuserNavigation).AsEnumerable()
                            .Select(p => SubDomainSpocMapper.GetSubDomainSpocMapper(p)).AsQueryable()
                            .ApplyOrdering(buildFilterDto, GetColumnsMap()).ApplyPaging(buildFilterDto)
                            :
                            _repositoryWrapper.SubDomainSpoc.FindAll()

                            .Include(x => x.ModificationuserNavigation)
                            .Include(x => x.CreationuserNavigation).AsEnumerable()
                            .Select(p => SubDomainSpocMapper.GetSubDomainSpocMapper(p)).AsQueryable()
                            .ApplyOrdering(buildFilterDto, GetColumnsMap())
                            .ApplyPaging(buildFilterDto);


            var data = query.ToList();

            if (buildFilterDto.PrincipalId != 0)
            {
                var exist = data.Any(x => x.SubDomainSpocId == buildFilterDto.PrincipalId);
                if (!exist)
                {
                    var addedResource = _repositoryWrapper.SubDomainSpoc.FindAll(true)

                            .Include(x => x.ModificationuserNavigation)
                        .Single(x => x.Subdomainspocid == buildFilterDto.PrincipalId);
                    data.Add(SubDomainSpocMapper.GetSubDomainSpocMapper(addedResource));
                }
            }

            var subDomainSpocResult = _mapper.Map<IEnumerable<SubDomainSpocGridDto>>(data);
            rtn.Items = subDomainSpocResult.ToArray();
            return rtn;
        }
        public static ExpressionStarter<Subdomainspocs> ApplyFilter(SubDomainSpocQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Subdomainspocs>();
            var predicateInner = PredicateBuilder.New<Subdomainspocs>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Subdomainspocs>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Subdomainspoc == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Subdomainspocs>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Subdomainspocid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Subdomainspocs>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Subdomainspocs>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Subdomainspocs>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            if (request.IsEdu != null && request.IsEdu.Any())
            {
                predicateInner = PredicateBuilder.New<Subdomainspocs>();
                foreach (var item in request.IsEdu)
                    predicateInner.Or(x => x.Isedu == item);
                predicateResult.And(predicateInner);
            }
            if (request.IsSubDomain != null && request.IsSubDomain.Any())
            {
                predicateInner = PredicateBuilder.New<Subdomainspocs>();
                foreach (var item in request.IsSubDomain)
                    predicateInner.Or(x => x.Issubdomain == item);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public IQueryable<SubDomainSpoc> PrepareQuery(SubDomainSpocQueryDto request, ExpressionStarter<SubDomainSpoc> predicateResult, ExpressionStarter<Subdomainspocs> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.SubDomainSpoc.FindByCondition(oraclePredicateResult)
               : _repositoryWrapper.SubDomainSpoc.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => SubDomainSpocMapper.GetSubDomainSpocMapper(p)).AsQueryable();
        }
        //(string propertyName, string propertyFilter, TQueryObject request
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, SubDomainSpocQueryDto buildFilterDto)
        {
            buildFilterDto.PageSize = 0;
            buildFilterDto.Page = 1;
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = PrepareQueryForSubDomain(buildFilterDto, null, predicateResult);

            var data = (await GetFilterValueList(query, propertyName, propertyFilter)).Distinct().ToList();
            return data;

        }
        public override ExpressionStarter<Subdomainspocs> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Subdomainspocs>();
            var predicateInner = PredicateBuilder.New<Subdomainspocs>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Subdomainspocs>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Subdomainspoc == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Subdomainspocs>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Subdomainspocid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Subdomainspocs>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Subdomainspocs>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Subdomainspocs>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<SubDomainSpoc> request)
        {
            return request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.SubDomainSpocId,
                Description = dto.SubDomainSpocDescription,
                LastModified = dto.ModificationDate,
                Deleted = dto.Deleted,
                Orphan = null,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<SubDomainSpoc, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<SubDomainSpoc, object>>[]>
            {
                ["description"] = new Expression<Func<SubDomainSpoc, object>>[] { p => p.SubDomainSpocDescription },
                ["lastModifiedBy"] = new Expression<Func<SubDomainSpoc, object>>[] { p => p.ModificationUserEntity.Email },
                ["id"] = new Expression<Func<SubDomainSpoc, object>>[] { p => p.SubDomainSpocId }

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<SubDomainSpoc> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.SubDomainSpocDescription))
                : request.Where(x => x.SubDomainSpocDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.SubDomainSpocDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.SubDomainSpocId.ToString()))
                : request.Where(x => x.SubDomainSpocId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.SubDomainSpocId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "isEdu" => string.IsNullOrEmpty(propertyFilter)
               ? request.Select(x => new FilterValueDto(x.isEdu.ToString()))
               : request.Where(x => x.isEdu.ToString() == propertyFilter).Select(x => new FilterValueDto(x.isEdu.ToString())),
                "isSubDomain" => string.IsNullOrEmpty(propertyFilter)
               ? request.Select(x => new FilterValueDto(x.isSubDomain.ToString()))
               : request.Where(x => x.isSubDomain.ToString() == propertyFilter).Select(x => new FilterValueDto(x.isSubDomain.ToString())),
            };
        }

        public IQueryable<SubDomainSpoc> PrepareQueryForSubDomain(SubDomainSpocQueryDto request, ExpressionStarter<SubDomainSpoc> predicateResult, ExpressionStarter<Subdomainspocs> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.SubDomainSpoc.FindByCondition(oraclePredicateResult)
               : _repositoryWrapper.SubDomainSpoc.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => SubDomainSpocMapper.GetSubDomainSpocMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(SubDomainSpocGridDto dto)
        {
            var entityExists = await _repositoryWrapper.SubDomainSpoc.FindByCondition(
               x => x.Subdomainspoc.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Subdomainspocid
                };
            }
            SubDomainSpoc entity = new SubDomainSpoc() { SubDomainSpocId = dto.Id, SubDomainSpocDescription = dto.Description, isSubDomain = dto.isSubDomain, isEdu = dto.isEdu };
            _repositoryWrapper.SubDomainSpoc.Create(SubDomainSpocMapper.SetSubDomainSpocMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(SubDomainSpocGridDto dto)
        {
            var entityExists = await _repositoryWrapper.SubDomainSpoc.FindByCondition(
               x => x.Subdomainspocid != dto.Id
               && x.Subdomainspoc.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Subdomainspocid
                };
            }
            SubDomainSpoc entity = new SubDomainSpoc() { SubDomainSpocId = dto.Id, SubDomainSpocDescription = dto.Description, isSubDomain = dto.isSubDomain, isEdu = dto.isEdu };
            _repositoryWrapper.SubDomainSpoc.Update(SubDomainSpocMapper.SetSubDomainSpocMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.SubDomainSpoc.FindByCondition(x => x.Subdomainspocid == id).SingleAsync();
            _repositoryWrapper.SubDomainSpoc.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Subdomainspocid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.SubDomainSpoc.FindByCondition(x => x.Subdomainspocid == id).SingleAsync();
            _repositoryWrapper.SubDomainSpoc.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Subdomainspocid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO: verificare descrizione da visualizzare
            var LcmEngineeringEduSpoc = _repositoryWrapper.LcmEngineeringEduSpoc.FindByCondition(x => x.Eduspocid == id)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                .Select(x => x.Lcmengineering.toDescription(_repositoryWrapper)
            ).ToArray();
            var LcmEngineeringSubDomainSpoc = _repositoryWrapper.LcmEngineeringSubDomainSpoc.FindByCondition(x => x.Subdomainspocid == id)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                .Select(x => x.Lcmengineering.toDescription(_repositoryWrapper)
            ).ToArray();
            var SystemTypes = _repositoryWrapper.SystemTypesSubDomainSpoc.FindByCondition(x => x.Subdomainspocid == id)
                    .Include(x => x.Systemtype)
                    .Select(x => x.Systemtype.toSystemTypeName(_repositoryWrapper)
                    ).ToArray();
            var NetworkElementAsPlannedEduSpoc = _repositoryWrapper.NetworkElementAsPlannedEduSpoc.FindByCondition(x => x.Eduspocid == id)
                    .Include(x => x.Networkelementasplanned)
                    .Select(x => NetworkElementAsPlannedMapper.Get(x.Networkelementasplanned, true).toDescription()
                    ).ToArray();
            var NetworkElementAsPlannedSubDomainSpoc = _repositoryWrapper.NetworkElementAsPlannedSubDomainSpoc.FindByCondition(x => x.Subdomainspocid == id)
                    .Include(x => x.Networkelementasplanned)
                    .Select(x => NetworkElementAsPlannedMapper.Get(x.Networkelementasplanned, true).toDescription()
                    ).ToArray();


            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (LcmEngineeringEduSpoc.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Lcm engineering Edu Spoc", Values = LcmEngineeringEduSpoc });
            if (LcmEngineeringSubDomainSpoc.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Lcm Engineering Sub Domain Spoc", Values = LcmEngineeringSubDomainSpoc });
            if (SystemTypes.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "System Types Sub Domain Spoc", Values = SystemTypes });
            if (NetworkElementAsPlannedEduSpoc.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Network Element As Planned Edu Spoc", Values = NetworkElementAsPlannedEduSpoc });
            if (NetworkElementAsPlannedSubDomainSpoc.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Network Element As Planned Sub Domain Spoc", Values = NetworkElementAsPlannedSubDomainSpoc });


            var entity = await _repositoryWrapper.SubDomainSpoc.FindByCondition(x => x.Subdomainspocid == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Sub-Domain SPOC",
                        RecordName = entity.Subdomainspoc,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public SubDomainSpocGridDto GetCreatePage()
        {
            var dto = new SubDomainSpocGridDto();
            dto.isEdu = false;
            dto.isSubDomain = false;
            return dto;
        }

        public SubDomainSpocGridDto GetUpdatePage(short id)
        {
            var entity = SubDomainSpocMapper.GetSubDomainSpocMapper(_repositoryWrapper.SubDomainSpoc
                .FindByCondition(x => x.Subdomainspocid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new SubDomainSpocGridDto()
            {
                Id = entity.SubDomainSpocId,
                Description = entity.SubDomainSpocDescription,
                LastModified = entity.ModificationDate,
                LastModifiedBy = entity.ModificationUserEntity.Email,
                isEdu = entity.isEdu,
                isSubDomain = entity.isSubDomain,
            };
            return dto;
        }
    }
}



