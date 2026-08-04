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
using CAM.DataTransferObjects.LookUp.Practice;

namespace CAM.BusinessManager.LookUp
{
    public class PracticeManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public PracticeManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        #region //UI Member
        public ExpressionStarter<Practice> ApplyFilterForOracleModel(PracticeQueryDto request)
        {
               var predicateResult = PredicateBuilder.New<Practice>();
            var predicateInner = PredicateBuilder.New<Practice>();

            if (request.PracticeDescription != null && request.PracticeDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Practice>();
                foreach (var item in request.PracticeDescription)
                    predicateInner.Or(x => x.Practicedescription == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Practice>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.PracticeId != null && request.PracticeId.Any())
            {
                predicateInner = PredicateBuilder.New<Practice>();
                foreach (var item in request.PracticeId)
                    predicateInner.Or(x => x.Practiceid == item);
                predicateResult.And(predicateInner);
            }
            if (request.PracticeEmail != null && request.PracticeEmail.Any())
            {
                predicateInner = PredicateBuilder.New<Practice>();
                foreach (var item in request.PracticeEmail)
                    predicateInner.Or(x => x.Practiceemail.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Practice>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        public async Task<QueryResultDto<PracticeGridDto>> FindWithCondition(PracticeQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<PracticeGridDto>(new GenerateRenderForGrid<PracticeGridDto>(_columnManager))
            {

            };
            var query = PrepareQuery(predicateResult);
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = data.Select(x =>
            {
                var grid = new PracticeGridDto();

                grid.PracticeId = x.PracticeId;
                grid.PracticeDescription = x.PracticeDescription;
                grid.PracticeEmail = x.PracticeEmail?.Email;
                grid.LastModified = x.ModificationDate;
                grid.LastModifiedBy = x.ModificationUserEntity.Email;
                return grid;
            }).ToList();

            rtn.Items = result.ToArray();
            return rtn;
        }

        public Dictionary<string, Expression<Func<PracticeModel, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<PracticeModel, object>>[]>
            {
                ["practiceDescription"] = new Expression<Func<PracticeModel, object>>[] { p => p.PracticeDescription },
                ["practiceId"] = new Expression<Func<PracticeModel, object>>[] { p => p.PracticeId },
                ["practiceEmailId"] = new Expression<Func<PracticeModel, object>>[] { p => p.PracticeEmailId},
                ["lastModifiedBy"] = new Expression<Func<PracticeModel, object>>[] { p => p.ModificationUserEntity.Email }
            };
        }

        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, PracticeQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var query = PrepareQuery(predicateResult);

            var data = (await GetFilterValueList(query, propertyName, propertyFilter)).Distinct().ToList();
            return data;
        }

        public async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<PracticeModel> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "practiceDescription" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.PracticeDescription))
                : request.Where(x => x.PracticeDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.PracticeDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "practiceId" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.PracticeId.ToString()))
                : request.Where(x => x.PracticeId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.PracticeId.ToString())),
                "practiceEmail" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.PracticeEmail.Email))
                : request.Where(x => x.PracticeEmail.Email.ToString().Contains(propertyFilter)).Select(x => new FilterValueDto(x.PracticeEmail.Email)),
            };
        }

       
        public IQueryable<PracticeModel> PrepareQuery(ExpressionStarter<Practice> predicateResult)
        {
            var query = predicateResult.IsStarted
             ? _repositoryWrapper.PracticeRepository.FindByCondition(predicateResult)
             .Include(x=>x.Practiceemail)
             : _repositoryWrapper.PracticeRepository.FindAll()
             .Include(x=>x.Practiceemail);
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => PracticeMapper.GetPracticeMapper(p)).AsQueryable();
        }


        #endregion

        #region //CRUD 
        public async Task<ResultDto> Add(PracticeGridDto dto)
        {
            var entityExists = await _repositoryWrapper.PracticeRepository.FindByCondition(
               x => x.Practicedescription.ToLower().Trim() == dto.PracticeDescription.ToLower().Trim()).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Practiceid.ToString(),
                };
            }
            PracticeModel entity = new PracticeModel() 
            {
                Deleted = false, 
                PracticeDescription = dto.PracticeDescription,
                PracticeEmailId = dto.PracticeEmailId
            };
            _repositoryWrapper.PracticeRepository.Create(PracticeMapper.SetPracticeMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(PracticeGridDto dto)
        {
            var entityExists = await _repositoryWrapper.PracticeRepository.FindByCondition(
               x => x.Practiceid != dto.PracticeId 
               && x.Practicedescription.ToLower().Trim() == dto.PracticeDescription.ToLower().Trim()
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Practiceid
                };
            }
            PracticeModel entity = new PracticeModel() 
            {
                PracticeId = dto.PracticeId,
                PracticeDescription = dto.PracticeDescription,
                PracticeEmailId = dto.PracticeEmailId,
                Deleted = false
            };
            _repositoryWrapper.PracticeRepository.Update(PracticeMapper.SetPracticeMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.PracticeRepository.FindByCondition(x => x.Practiceid == id).SingleAsync();
            _repositoryWrapper.PracticeRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Practiceid
            };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.PracticeRepository.FindByCondition(x => x.Practiceid == id).Include(x=>x.Organisation).SingleAsync();
            if (entity != null) 
            {
                var unLikedFromOrganisation = _repositoryWrapper.OrganisationRepository.FindByCondition(x => entity.Organisation.Select(y=>y.Organisationid).Contains(x.Organisationid)).ToList();

                if(unLikedFromOrganisation != null && unLikedFromOrganisation.Count > 0)
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.RelationShip,
                        Data = entity.Practiceid
                    };                   
                }               
                _repositoryWrapper.PracticeRepository.DeleteDeep(entity);
                await _repositoryWrapper.SaveAsync();

            }
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Practiceid
            };
        }
     
        public PracticeGridDto GetCreatePage()
        {
            var dto = new PracticeGridDto();
            var practiceResource = _repositoryWrapper.UserRepository.FindAll().Where(x => x.Email.ToLower() != "admincam").ToDictionary(x => x.Id, x => x.Email);
            dto.PracticeResource = practiceResource;
            return dto;
        }

        public PracticeGridDto GetUpdatePage(long id)
        {
            var entity = _repositoryWrapper.PracticeRepository.FindByCondition(x => x.Practiceid == id).Include(x=>x.ModificationuserNavigation).FirstOrDefault();
            var practiceResource = _repositoryWrapper.UserRepository.FindAll().Where(x => x.Email.ToLower() != "admincam").ToDictionary(x => x.Id, x => x.Email);
            
            var dto = new PracticeGridDto();
            if (entity != null)
            {
                dto.PracticeEmailId = entity.Practiceemailid;
                dto.PracticeDescription = entity.Practicedescription;
                dto.PracticeId = entity.Practiceid;
                dto.LastModifiedBy = entity.ModificationuserNavigation.Email;
                dto.PracticeResource = practiceResource;
            }
            return dto;
        }
        #endregion


    }
}

    
