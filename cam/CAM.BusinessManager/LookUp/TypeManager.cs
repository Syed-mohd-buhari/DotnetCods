using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects;
using CAM.Entities.Mappers.Lookup;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using CAM.DataTransferObjects.LookUp.Type;
using Type = CAM.Entities.Models.Lookup.Type;
using Microsoft.EntityFrameworkCore;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.DataTransferObjects.Entita.MajorHardwareBuild;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.ExtendedProperties;
using AutoMapper;

namespace CAM.BusinessManager.LookUp
{
    public class TypeManager : GridBaseAsync<Type, TypeDtoGrid, TypeDtoQuery, Types>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly IMapper _mapper;

        public TypeManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, IMapper mapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
        }


        public override ExpressionStarter<Types> ApplyFilterForOracleModel(TypeDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Types>();
            var predicateInner = PredicateBuilder.New<Types>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Types>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.ClassDescription != null && request.ClassDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Types>();
                foreach (var item in request.ClassDescription)
                    predicateInner.Or(x => x.Class.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.CategoryDescription != null && request.CategoryDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Types>();
                foreach (var item in request.CategoryDescription)
                    predicateInner.Or(x => x.Class.Category.Description == item);
                predicateResult.And(predicateInner);
            }


            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Types>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Types>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Types>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Value.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Value.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Types>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<TypeDtoGrid> CastObjectToDto(IQueryable<Type> request)
        {
            return request.Select(dto => new TypeDtoGrid()
            {
                Id = (short)dto.Id,
                Description = dto.Description,
                ClassId = (short)dto.ClassId,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<Type, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Type, object>>[]>
            {
                ["description"] = new Expression<Func<Type, object>>[] { p => p.Description },
                ["ClassId"] = new Expression<Func<Type, object>>[] { p => p.ClassId },
                ["id"] = new Expression<Func<Type, object>>[] { p => p.Id },
                ["lastModifiedBy"] = new Expression<Func<Type, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<Type> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Description))
                    : request.Where(x => x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)),
                "classDescription" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Class.Description))
                : request.Where(x => x.Class.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Class.Description)),
                "categoryDescription" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Class.Category.Description))
               : request.Where(x => x.Class.Category.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Class.Category.Description)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.Id.ToString()))
                    : request.Where(x => x.Id.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Id.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<Type> PrepareQuery(TypeDtoQuery request, ExpressionStarter<Type> predicateResult, ExpressionStarter<Types> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.TypeRepository.FindByCondition(oraclePredicateResult)
                : _repositoryWrapper.TypeRepository.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .Include(x => x.Class.Category)
                .AsEnumerable().Select(p => TypeMapper.Get(p)).AsQueryable();
        }

        public QueryResultDto<TypeDtoGrid> FindWithCondition(TypeDtoQuery buildFilterDto)
        {
            var predicateResult = ApplyFilterForOracleModel(buildFilterDto);

            if (buildFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
         
            var rtn = new QueryResultDto<TypeDtoGrid>(new GenerateRenderForGrid<TypeDtoGrid>(_columnManager))
            {

                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.TypeRepository.Count(predicateResult) : _repositoryWrapper.TypeRepository.Count(),
            };


            var query = predicateResult.IsStarted ? _repositoryWrapper.TypeRepository.FindByCondition(predicateResult, buildFilterDto.Deleted ?? false)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Class.Category)
                .AsEnumerable()
                .Select(p => TypeMapper.Get(p))
                .AsQueryable()
                .ApplyOrdering(buildFilterDto, GetColumnsMap()).ApplyPaging(buildFilterDto)
                :
                _repositoryWrapper.TypeRepository.FindAll()
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Class.Category)
                .AsEnumerable()
                .Select(p => TypeMapper.Get(p))
                .AsQueryable()
                .ApplyOrdering(buildFilterDto, GetColumnsMap()).ApplyPaging(buildFilterDto);
            var data = query.ToList();
            if (buildFilterDto.PrincipalId != 0)
            {
                var exist = data.Any(x => x.Id == buildFilterDto.PrincipalId);
                if (!exist)
                {
                    var addedResource = _repositoryWrapper.TypeRepository.FindAll(true)
                        .Include(x => x.Class.Category)
                        .Single(x => x.Id == buildFilterDto.PrincipalId);
                    data.Add(TypeMapper.Get(addedResource));
                }
            }

            var result = _mapper.Map<IEnumerable<TypeDtoGrid>>(data);
            rtn.Items = result.ToArray();
            return rtn;
        }

        public async Task<ResultDto> Add(TypeDtoCreate dto)
        {
            var entityExists = await _repositoryWrapper.TypeRepository.FindByCondition(
                x => x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            Type entity = new Type() { Id = dto.Id, Description = dto.Description, ClassId =dto.ClassId };
            _repositoryWrapper.TypeRepository.Create(TypeMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TypeDtoUpdate dto)
        {
            var entityExists = await _repositoryWrapper.TypeRepository.FindByCondition(
                x => x.Id != dto.Id && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            Type entity = new Type() { Id = dto.Id, Description = dto.Description, ClassId = dto.ClassId };

            _repositoryWrapper.TypeRepository.Update(TypeMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.TypeRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.TypeRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.TypeRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.TypeRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var entities = _repositoryWrapper.IdentityAsIsRepository.FindByCondition(x => x.Typeid == id)
                                    .Select(c => c.Value).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Identity ", Values = entities });
            var entity = await _repositoryWrapper.TypeRepository.FindByCondition(x => x.Id == id)
                .SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Type",
                        RecordName = entity.Description,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public TypeDtoCreate GetCreatePage()
        {
            return new TypeDtoCreate()
            {
                CategoryResource = _repositoryWrapper.CategoryRepository.FindAll().ToDictionary(x => x.Id, x => x.Description),
            };
        }

        public TypeDtoUpdate GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.TypeRepository.FindByCondition(x => x.Id == id)
                                .Include(x => x.ModificationuserNavigation)
                                .Include(x => x.Class)
                                .Single();
            var categoryId = model.Class.Categoryid;
            var entity = TypeMapper.Get(model);
            return  new TypeDtoUpdate()
            {
                Id = entity.Id,
                Description = entity.Description,
                ClassId = entity.ClassId,
                CategoryId = categoryId,
                LastModified = entity.ModificationDate,
                LastModifiedBy = entity.ModificationUserEntity.Email,
                CategoryResource = _repositoryWrapper.CategoryRepository.FindAll().ToDictionary(x => x.Id, x => x.Description),
                ClassResource = _repositoryWrapper.ClassRepository.FindByCondition(x => x.Categoryid == categoryId).ToDictionary(x => x.Id, x => x.Description),
            };

        }

        public ResultDto GetTypesByClassId(int classId)
        {
            var data = _repositoryWrapper.TypeRepository.GetTypesByClassId(classId);

            return new ResultDto
            {
                Data = data,
                Info = "",
                Warning = false,
            };
        }

    }
}

