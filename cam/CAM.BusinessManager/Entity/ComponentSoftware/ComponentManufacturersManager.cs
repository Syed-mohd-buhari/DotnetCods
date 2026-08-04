using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto.ComponentSoftware;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.ComponentSoftware;
using CAM.Entities.Models.Lookup;
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

namespace CAM.BusinessManager.Entity.ComponentSoftware
{
    public class ComponentManufacturersManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        private readonly CommonManager _commonManager;


        public ComponentManufacturersManager(IEnumerable<IRepositoryWrapper> wrappers,
            IMapper mapper,
            GridCustomColumnManager customColumnManager,
            IRepositoryWrapper repositoryWrapper,
            IHttpContextAccessor contextAccessor,
        CommonManager commonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _customColumnManager = customColumnManager;
            _commonManager = commonManager;
        }

        #region Grid Load, Filter 
        private IQueryable<Componentmanufacturers> GetComponetManufacturersRecords(ExpressionStarter<Componentmanufacturers> predicateResult)
        {
            var query = _repositoryWrapper.ComponentManufacturersRepository.FindByCondition(predicateResult)                       
                        .Include(x => x.CreationuserNavigation)
                        .Include(x => x.ModificationuserNavigation)
                        .AsQueryable();

            return query;
        }
        public async Task<QueryResultDto<ComponentManufacturersGridDto>> FindWithCondition(ComponentManufacturersQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);          

            var query = await Task.Run(() => GetComponetManufacturersRecords(predicateResult).AsEnumerable()
                         .Select(p => ComponentManufacturersMapper.GetComponentManufacturersMapper(p)).AsQueryable());


            var totalCount = query.Count();

            var paginatedQuery = await Task.Run(() => query.ApplyOrdering(buildFilterDto, GetColumnsMap()).ApplyPaging(buildFilterDto).ToList());
            

            var mappedData = _mapper.Map<IEnumerable<ComponentManufacturersGridDto>>(paginatedQuery);

            return new QueryResultDto<ComponentManufacturersGridDto>(
                new GenerateRenderForGrid<ComponentManufacturersQueryDto>(_customColumnManager))
            {
                TotalItems = totalCount,
                Items = mappedData.ToArray()
            };
        }



        public ExpressionStarter<Componentmanufacturers> ApplyFilter(ComponentManufacturersQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Componentmanufacturers>(true);

            if (filterDto.ComponentManufacturerId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Componentmanufacturers>();
                foreach (var id in filterDto.ComponentManufacturerId)
                    componentIdPredicate.Or(x => x.Componentmanufacturerid == id);

                mainPredicate.And(componentIdPredicate);
            }

            if (filterDto.ComponentManufacturer?.Any() == true)
            {
                var componentManufacturers = PredicateBuilder.New<Componentmanufacturers>();
                foreach (var componentManu in filterDto.ComponentManufacturer)
                    componentManufacturers.Or(x => x.Componentmanufacturer == componentManu);

                mainPredicate.And(componentManufacturers);
            }

            if (filterDto.ComponentName?.Any() == true)
            {
                var ComponentName = PredicateBuilder.New<Componentmanufacturers>();
                foreach (var component in filterDto.ComponentName)
                    ComponentName.Or(x => x.Componentname == component);

                mainPredicate.And(ComponentName);
            }

            if (filterDto.LastModifiedBy?.Any() == true)
            {
                var modifiedByPredicate = PredicateBuilder.New<Componentmanufacturers>();
                foreach (var email in filterDto.LastModifiedBy)
                    modifiedByPredicate.Or(x => x.ModificationuserNavigation.Email == email);

                mainPredicate.And(modifiedByPredicate);
            }
            if (filterDto.LastModified != null)
            {
               var modifiedPredicate = PredicateBuilder.New<Componentmanufacturers>();
                if (filterDto.LastModified.StartDate != null)
                    modifiedPredicate.And(x => x.Creationdate >= filterDto.LastModified.StartDate);
                if (filterDto.LastModified.EndDate != null)
                    modifiedPredicate.And(x => x.Creationdate <= filterDto.LastModified.EndDate);
                mainPredicate.And(modifiedPredicate);
            }

            return mainPredicate;
        }

        private Dictionary<string, Expression<Func<ComponentManufacturers, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ComponentManufacturers, object>>[]>
            {
                ["componentmanufacturerid"] = new Expression<Func<ComponentManufacturers, object>>[] { p => p.Componentmanufacturerid },
                ["componentmanufacturer"] = new Expression<Func<ComponentManufacturers, object>>[] { p => p.Componentmanufacturer },
                ["componentname"] = new Expression<Func<ComponentManufacturers, object>>[] { p => p.Componentname },
                ["lastModified"] = new Expression<Func<ComponentManufacturers, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<ComponentManufacturers, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public async Task<List<FilterValueDto>> GetFilteredValuesAsync(string propertyName, string propertyFilter, ComponentManufacturersQueryDto filterDto)
        {
            var filterCriteria = ApplyFilter(filterDto);

            var filteredQuery = GetComponetManufacturersRecords(filterCriteria);

            var result = propertyName switch
            {
                "componentmanufacturerid" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Componentmanufacturerid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Componentmanufacturerid))
                    .Distinct()
                    .ToListAsync(),

                "componentmanufacturer" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Componentmanufacturer.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Componentmanufacturer, Value = p.Componentmanufacturerid.ToString() })
                    .Distinct()
                    .ToListAsync(),

                "lastModifiedBy" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ModificationuserNavigation.Email.Contains(propertyFilter))
                    .Select(p => new FilterValueDto(p.ModificationuserNavigation.Email))
                    .Distinct()
                    .ToListAsync(),

                "componentname" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Componentname.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Componentname, Value = p.Componentmanufacturerid.ToString() })
                    .Distinct()
                    .ToListAsync(),

                "lastModified" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Modificationdate.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto(p.Modificationdate))
                    .Distinct()
                    .ToListAsync(),


                _ => new List<FilterValueDto>(),
            };

            return result;
        }


        #endregion

        #region //CRUD

        public async Task<ResultDto> Add(ComponentManufacturersGridDto dto)
        {
            var entityExists = await _repositoryWrapper.ComponentManufacturersRepository.FindByCondition(
               x => x.Componentmanufacturer.ToLower().Replace(" ", "") == dto.ComponentManufacturer.ToLower().Replace(" ", "")
               && x.Componentname.ToLower().Replace(" ","") == dto.ComponentName.ToLower().Replace(" ",""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Componentmanufacturerid
                };
            }

            ComponentManufacturers entity = new ComponentManufacturers()
            { Componentmanufacturer = dto.ComponentManufacturer, Componentname = dto.ComponentName };
            _repositoryWrapper.ComponentManufacturersRepository.Create(ComponentManufacturersMapper.SetComponentManufacturersMapper(entity));
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(ComponentManufacturersGridDto dto)
        {
            var entityExists = await _repositoryWrapper.ComponentManufacturersRepository.FindByCondition(
               x => x.Componentmanufacturerid != dto.ComponentManufacturerId && 
               x.Componentmanufacturer.ToLower().Replace(" ", "") == dto.ComponentManufacturer.ToLower().Replace(" ", "") &&
               x.Deleted != false && x.Componentname.ToLower().Replace(" ","") == dto.ComponentName.ToLower().Replace(" ",""))
                .FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Componentmanufacturerid
                };
            }
            ComponentManufacturers entity = new ComponentManufacturers()
            { 
                Componentmanufacturerid = dto.ComponentManufacturerId, 
                Componentmanufacturer = dto.ComponentManufacturer,
                Componentname = dto .ComponentName,
            };
            _repositoryWrapper.ComponentManufacturersRepository.Update
                (ComponentManufacturersMapper.SetComponentManufacturersMapper(entity));

            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.ComponentManufacturersRepository.FindByCondition(x => x.Componentmanufacturerid == id).SingleAsync();
            _repositoryWrapper.ComponentManufacturersRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Componentmanufacturerid
            };
        }

        #endregion

        #region // Page grid

        public ComponentManufacturersGridDto GetCreatePageDetails()
        {
            var dto = new ComponentManufacturersGridDto();
            return  dto;
        }

        public async Task<ComponentManufacturersGridDto> GetUpdatePageDetailsAsync(long id)
        {
            var entity = await _repositoryWrapper.ComponentManufacturersRepository.FindByCondition(x => x.Componentmanufacturerid == id)
                .Include(x=>x.ModificationuserNavigation)
                .FirstOrDefaultAsync();

            var model = ComponentManufacturersMapper.GetComponentManufacturersMapper(entity);
            var result = _mapper.Map<ComponentManufacturersGridDto>(model);

            return result;
        }
        #endregion
    }

}