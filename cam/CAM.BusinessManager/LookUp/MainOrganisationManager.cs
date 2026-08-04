using CAM.BusinessManager.Grid;
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
using OracleModels.DBModels;
using CAM.Entities.Mappers.Entity;
using Microsoft.AspNetCore.Http;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Infrastucture.QueryResult;
using CAM.DataTransferObjects.LookUp.MainOrganisation;

namespace CAM.BusinessManager.LookUp
{
    public class MainOrganisationManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public MainOrganisationManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        #region //UI Member
        public ExpressionStarter<Mainorganisation> ApplyFilterForOracleModel(MainOrganisationQueryDto request)
        {
               var predicateResult = PredicateBuilder.New<Mainorganisation>();
            var predicateInner = PredicateBuilder.New<Mainorganisation>();

            if (request.MainorganisationDescription != null && request.MainorganisationDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Mainorganisation>();
                foreach (var item in request.MainorganisationDescription)
                    predicateInner.Or(x => x.Mainorganisationdescription == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Mainorganisation>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.MainorganisationId != null && request.MainorganisationId.Any())
            {
                predicateInner = PredicateBuilder.New<Mainorganisation>();
                foreach (var item in request.MainorganisationId)
                    predicateInner.Or(x => x.Mainorganisationid == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Mainorganisation>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        public async Task<QueryResultDto<MainOrganisatioinGridDto>> FindWithCondition(MainOrganisationQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<MainOrganisatioinGridDto>(new GenerateRenderForGrid<MainOrganisatioinGridDto>(_columnManager))
            {

            };
            var query = PrepareQuery(predicateResult);
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = data.Select(x =>
            {
                var grid = new MainOrganisatioinGridDto();

                grid.MainOrganisationId = x.MainorganisationId;
                grid.MainOrganisationDescription = x.MainorganisationDescription;
                grid.LastModified = x.ModificationDate;
                grid.LastModifiedBy = x.ModificationUserEntity.Email;
                return grid;
            }).ToList();

            rtn.Items = result.ToArray();
            return rtn;
        }

        public Dictionary<string, Expression<Func<MainOrganisation, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<MainOrganisation, object>>[]>
            {
                ["mainOrganisationDescription"] = new Expression<Func<MainOrganisation, object>>[] { p => p.MainorganisationDescription },
                ["mainOrganisationId"] = new Expression<Func<MainOrganisation, object>>[] { p => p.MainorganisationId },
                ["lastModifiedBy"] = new Expression<Func<MainOrganisation, object>>[] { p => p.ModificationUserEntity.Email }
            };
        }

        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, MainOrganisationQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var query = PrepareQuery(predicateResult);

            var data = (await GetFilterValueList(query, propertyName, propertyFilter)).Distinct().ToList();
            return data;
        }

        public async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<MainOrganisation> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "mainOrganisationDescription" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.MainorganisationDescription))
                : request.Where(x => x.MainorganisationDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.MainorganisationDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "mainOrganisationId" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.MainorganisationId.ToString()))
                : request.Where(x => x.MainorganisationId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.MainorganisationId.ToString())),
            };
        }

       
        public IQueryable<MainOrganisation> PrepareQuery(ExpressionStarter<Mainorganisation> predicateResult)
        {
            var query = predicateResult.IsStarted
             ? _repositoryWrapper.MainOrganisationRepository.FindByCondition(predicateResult)
             : _repositoryWrapper.MainOrganisationRepository.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => MainOrganisationMapper.GetMainOrganisationMapper(p)).AsQueryable();
        }


        #endregion

        #region //CRUD 
        public async Task<ResultDto> Add(MainOrganisatioinGridDto dto)
        {
            var entityExists = await _repositoryWrapper.MainOrganisationRepository.FindByCondition(
               x => x.Mainorganisationdescription.ToLower().Trim() == dto.MainOrganisationDescription.ToLower().Trim()).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Mainorganisationid.ToString(),
                };
            }
            MainOrganisation entity = new MainOrganisation() 
            {
                Deleted = false, 
                MainorganisationDescription = dto.MainOrganisationDescription, 
            };
            _repositoryWrapper.MainOrganisationRepository.Create(MainOrganisationMapper.SetMainOrganisationMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(MainOrganisatioinGridDto dto)
        {
            var entityExists = await _repositoryWrapper.MainOrganisationRepository.FindByCondition(
               x => x.Mainorganisationid != dto.MainOrganisationId 
               && x.Mainorganisationdescription.ToLower().Trim() == dto.MainOrganisationDescription.ToLower().Trim()
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Mainorganisationid
                };
            }
            MainOrganisation entity = new MainOrganisation() 
            {
                MainorganisationId = dto.MainOrganisationId,
                MainorganisationDescription = dto.MainOrganisationDescription,
                Deleted = false
            };
            _repositoryWrapper.MainOrganisationRepository.Update(MainOrganisationMapper.SetMainOrganisationMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.MainOrganisationRepository.FindByCondition(x => x.Mainorganisationid == id).SingleAsync();
            _repositoryWrapper.MainOrganisationRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Mainorganisationid
            };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.MainOrganisationRepository.FindByCondition(x => x.Mainorganisationid == id).Include(x=>x.Organisation).SingleAsync();
            if (entity != null) 
            {
                var unLikedFromOrganisation = _repositoryWrapper.OrganisationRepository.FindByCondition(x => entity.Organisation.Select(y=>y.Organisationid).Contains(x.Organisationid)).ToList();

                if(unLikedFromOrganisation != null && unLikedFromOrganisation.Count > 0)
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.RelationShip,
                        Data = entity.Mainorganisationid
                    };
                }
                _repositoryWrapper.MainOrganisationRepository.DeleteDeep(entity);
                await _repositoryWrapper.SaveAsync();
            }
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Mainorganisationid
            };
        }
     
        public MainOrganisatioinGridDto GetCreatePage()
        {
            var dto = new MainOrganisatioinGridDto();
            return dto;
        }

        public MainOrganisatioinGridDto GetUpdatePage(long id)
        {
            var entity = _repositoryWrapper.MainOrganisationRepository.FindByCondition(x => x.Mainorganisationid == id).Include(x=>x.ModificationuserNavigation).FirstOrDefault();
            var dto = new MainOrganisatioinGridDto();
            if (entity != null)
            {
                dto.MainOrganisationId = entity.Mainorganisationid;
                dto.MainOrganisationDescription = entity.Mainorganisationdescription;
                dto.LastModifiedBy = entity.ModificationuserNavigation.Email;
            }
            return dto;
        }
        #endregion


    }
}

    
