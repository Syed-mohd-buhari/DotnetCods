using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.SoftwareConfiguration;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CAM.BusinessManager.Entity
{
    public class SWConfigManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;

        public SWConfigManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor, ReconciliationManager reconciliation,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }


        #region //UiMemberFunctions
        public QueryResultDto<SWConfigGridDto> FindWithCondition(SWConfigQueryDto dto, bool isExport)
        {
            var predicateResult = ApplyFilter(dto);        
            var rtn = new QueryResultDto<SWConfigGridDto>(new GenerateRenderForGrid<SWConfigGridDto>(_manager))
            {
               
            };
            var query = GetQuery(predicateResult);
            query = query.ApplyOrdering(dto, GetColumnsMap());           
          
            var data = query.ToList();
            var result = new List<SWConfigGridDto>();
            foreach (var item in data)
            {
                if (dto.FunctionId != null && dto.FunctionId.Count() > 0)
                {
                    item.Function = item.Function.Where(fun => dto.FunctionId.Contains(fun.Functionid)).ToList();
                }
                if (dto.FunctionName != null && dto.FunctionName.Count() > 0)
                {
                    item.Function = item.Function.Where(fun => dto.FunctionName.Contains(fun.Functionname)).ToList();
                }
                if (item.Function != null && item.Function.Any())
                {
                    foreach (var function in item.Function)
                    {
                        if (dto.FunctionAreaName != null && dto.FunctionAreaName.Count() > 0)
                        {
                            function.Swconfigfunctionareas = function.Swconfigfunctionareas
                                .Where(funArea => dto.FunctionAreaName.Contains(funArea.Functionareaname)).ToList();                            
                        }
                        if (dto.FunctionAreaDescription != null && dto.FunctionAreaDescription.Count() > 0)
                        {
                            function.Swconfigfunctionareas = function.Swconfigfunctionareas
                                .Where(funArea => dto.FunctionAreaDescription.Contains(funArea.Functionareadescription)).ToList();
                        }
                        if (dto.SWConfigFunctionAreaId != null && dto.SWConfigFunctionAreaId.Count() > 0)
                        {
                            function.Swconfigfunctionareas = function.Swconfigfunctionareas
                                .Where(funArea => dto.SWConfigFunctionAreaId.Contains(funArea.Swconfigfunctionareaid)).ToList();
                        }
                        if (function.Swconfigfunctionareas != null && function.Swconfigfunctionareas.Any())
                        {
                            foreach (var functionArea in function.Swconfigfunctionareas)
                            {                               
                                result.Add(GetSWConfigFunctionArea(item,function, functionArea));
                            }
                        }
                        else if((dto.FunctionAreaName == null || dto.FunctionAreaName.Count() == 0 ) &&
                            (dto.FunctionAreaDescription == null|| dto.FunctionAreaDescription.Count() ==0  )&&
                            (dto.SWConfigFunctionAreaId == null || dto.SWConfigFunctionAreaId.Count() == 0)
                            )
                        {                           
                            result.Add(GetSWConfigFunction(item,function));
                        }
                    }
                }
                else if((dto.FunctionAreaName == null || dto.FunctionAreaName.Count() == 0 ) &&
                            (dto.FunctionAreaDescription == null || dto.FunctionAreaDescription.Count() == 0) &&
                            (dto.SWConfigFunctionAreaId == null || dto.SWConfigFunctionAreaId.Count() == 0) && 
                            (dto.FunctionId == null || dto.FunctionId.Count()==0)&& (dto.FunctionName == null || dto.FunctionName.Count() ==0))
                {         
                    result.Add(GetSWConfig(item));
                }
            }


            rtn.TotalItems = result.Count();
            if (isExport)
            {
                result = result.ToList();
            }
            else 
            {
                result = result.ApplyPaginationList(dto);
            }
            rtn.Items =result.ToArray();
            return rtn;
        }
       
        private static ExpressionStarter<Softwareconfiguration> ApplyFilter(SWConfigQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Softwareconfiguration>();
            var predicateInner = PredicateBuilder.New<Softwareconfiguration>();

            if (buildFilterDto.SoftwareConfigurationId != null && buildFilterDto.SoftwareConfigurationId.Any())
            {
                predicateInner = PredicateBuilder.New<Softwareconfiguration>();
                foreach (var item in buildFilterDto.SoftwareConfigurationId)
                    predicateInner.Or(x => x.Softwareconfigurationid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Softwareconfiguration>();
                foreach (var item in buildFilterDto.OpCo)
                    predicateInner.Or(x => x.Opco == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Oem != null && buildFilterDto.Oem.Any())
            {
                predicateInner = PredicateBuilder.New<Softwareconfiguration>();
                foreach (var item in buildFilterDto.Oem)
                    predicateInner.Or(x => x.Oem == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ElementName != null && buildFilterDto.ElementName.Any())
            {
                predicateInner = PredicateBuilder.New<Softwareconfiguration>();
                foreach (var item in buildFilterDto.ElementName)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.FunctionId != null && buildFilterDto.FunctionId.Any())
            {
                predicateInner = PredicateBuilder.New<Softwareconfiguration>();
                predicateResult = predicateResult.And(x=>x.Function.Any(f=> buildFilterDto.FunctionId.Contains(f.Functionid)));
            }
            if (buildFilterDto.FunctionName != null && buildFilterDto.FunctionName.Any())
            {
                predicateInner = PredicateBuilder.New<Softwareconfiguration>();
                foreach (var item in buildFilterDto.FunctionName)
                    predicateInner.Or(x => x.Function.Any(x=>x.Functionname == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SoftwareConfigurationId != null && buildFilterDto.SoftwareConfigurationId.Any())
            {
                predicateInner = PredicateBuilder.New<Softwareconfiguration>();
                foreach (var item in buildFilterDto.SoftwareConfigurationId)
                    predicateInner.Or(x => x.Function.Any(x=>x.Swconfigfunctionareas.Any(x=>x.Swconfigfunctionareaid == item)) );
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.FunctionAreaName != null && buildFilterDto.FunctionAreaName.Any())
            {
                predicateInner = PredicateBuilder.New<Softwareconfiguration>();
                foreach (var item in buildFilterDto.FunctionAreaName)
                    predicateInner.Or(x => x.Function.Any(x=>x.Swconfigfunctionareas.Any(x=>x.Functionareaname == item)));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationuser != null && buildFilterDto.Creationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Softwareconfiguration>();
                foreach (var item in buildFilterDto.Creationuser)
                    predicateInner.Or(x => x.CreationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Modificationuser != null && buildFilterDto.Modificationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Softwareconfiguration>();
                foreach (var item in buildFilterDto.Modificationuser)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationdate != null)
            {
                predicateInner = PredicateBuilder.New<Softwareconfiguration>();
                if (buildFilterDto.Creationdate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= buildFilterDto.Creationdate.StartDate);
                if (buildFilterDto.Creationdate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= buildFilterDto.Creationdate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Modificationdate != null)
            {
                predicateInner = PredicateBuilder.New<Softwareconfiguration>();
                if (buildFilterDto.Modificationdate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= buildFilterDto.Modificationdate.StartDate);
                if (buildFilterDto.Modificationdate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= buildFilterDto.Modificationdate.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }

        private IQueryable<Softwareconfiguration> GetQuery(ExpressionStarter<Softwareconfiguration> predicateResult)
        {

            var query = predicateResult.IsStarted
            ? _repositoryWrapper.SoftwareConfiguration.FindByCondition(predicateResult)
            .Include(x => x.Function).ThenInclude(x => x.Swconfigfunctionareas)
            .ThenInclude(x=>x.Swconfigsubfunction).ThenInclude(x=>x.Swconfigsubfunctionareas)
            .Include(x=>x.CreationuserNavigation)
            .Include(x=>x.ModificationuserNavigation)
           : _repositoryWrapper.SoftwareConfiguration.FindAll()
           .Include(x => x.Function).ThenInclude(x => x.Swconfigfunctionareas)
           .ThenInclude(x => x.Swconfigsubfunction).ThenInclude(x => x.Swconfigsubfunctionareas)
            .Include(x => x.CreationuserNavigation)
            .Include(x => x.ModificationuserNavigation);
            return query;           
        }


        private Dictionary<string, Expression<Func<Softwareconfiguration, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Softwareconfiguration, object>>[]>
            {
                ["sWConfigFunctionAreaId"] = new Expression<Func<Softwareconfiguration, object>>[] { p => p.Softwareconfigurationid },             
                ["opCo"] = new Expression<Func<Softwareconfiguration, object>>[] { p => p.Opco },
                ["oem"] = new Expression<Func<Softwareconfiguration, object>>[] { p => p.Oem },
                ["modificationDate"] = new Expression<Func<Softwareconfiguration, object>>[] { p => p.Modificationdate },
                ["modificationUser"] = new Expression<Func<Softwareconfiguration, object>>[] { p => p.Modificationuser },
                ["creationUser"] = new Expression<Func<Softwareconfiguration, object>>[] { p => p.Creationuser },
                ["creationDate"] = new Expression<Func<Softwareconfiguration, object>>[] { p => p.Creationdate }              
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, SWConfigQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult);

            var rtn = propertyName switch
            {
                "softwareConfigurationId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.Softwareconfigurationid.ToString(), Value = p.Softwareconfigurationid.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.Softwareconfigurationid.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.Softwareconfigurationid.ToString(), Value = p.Softwareconfigurationid.ToString() }).Distinct()
                   .ToList(),             

                "opCo" => string.IsNullOrEmpty(propertyFilter)
           ? query.Select(p => new FilterValueDto
           { Text = p.Opco, Value = p.Opco }).Distinct().ToList()
           : query
               .Where(x => x.Opco.Contains(propertyFilter)).Select(p =>
                   new FilterValueDto { Text = p.Opco, Value = p.Opco }).Distinct()
               .ToList(),

                "oem" => string.IsNullOrEmpty(propertyFilter)
           ? query.Select(p => new FilterValueDto
           { Text = p.Oem, Value = p.Oem }).Distinct().ToList()
           : query
               .Where(x => x.Oem.Contains(propertyFilter)).Select(p =>
                   new FilterValueDto { Text = p.Oem, Value = p.Oem }).Distinct()
               .ToList(),

                "elementName" => string.IsNullOrEmpty(propertyFilter)
           ? query.Select(p => new FilterValueDto
           { Text = p.Elementname, Value = p.Elementname }).Distinct().ToList()
           : query
               .Where(x => x.Elementname.Contains(propertyFilter)).Select(p =>
                   new FilterValueDto { Text = p.Elementname, Value = p.Elementname }).Distinct()
               .ToList(),

                "functionId" => string.IsNullOrEmpty(propertyFilter)
           ? query.Select(p => new FilterValueDto
           { Text = p.Function.Select(x=>x.Functionid).FirstOrDefault().ToString(), Value = p.Function.Select(x => x.Functionid).FirstOrDefault().ToString() }).Distinct().ToList()
           : query
               .Where(x => x.Function.Select(x=>x.Functionid).ToString().Contains(propertyFilter)).Select(p =>
                   new FilterValueDto { Text = p.Function.Select(x => x.Functionid).FirstOrDefault().ToString(), Value = p.Function.Select(x => x.Functionid).FirstOrDefault().ToString() }).Distinct()
               .ToList(),

                "functionName" => string.IsNullOrEmpty(propertyFilter)
          ? query.Select(p => new FilterValueDto
          { Text = p.Function.Select(x => x.Functionname).FirstOrDefault().ToString(), Value = p.Function.Select(x => x.Functionname).FirstOrDefault().ToString() }).Distinct().ToList()
          : query
              .Where(x => x.Function.Select(x=>x.Functionname).Contains(propertyFilter)).Select(p =>
                  new FilterValueDto { Text = p.Function.Select(x => x.Functionname).FirstOrDefault(), Value = p.Function.Select(x => x.Functionname).FirstOrDefault() }).Distinct()
              .ToList(),

                "swConfigFunctionAreaId" => 
           query.Select(p => new FilterValueDto
           { Text = p.Function.Select(x => x.Swconfigfunctionareas.Select(x=>x.Swconfigfunctionareaid).FirstOrDefault()).FirstOrDefault().ToString(), Value = p.Function.Select(x => x.Swconfigfunctionareas.Select(x => x.Swconfigfunctionareaid).FirstOrDefault()).FirstOrDefault().ToString() }).Distinct().ToList(),

             "functionAreaName" => 
           query.Select(p => new FilterValueDto
           { Text = p.Function.Select(x => x.Swconfigfunctionareas.Select(x => x.Functionareaname).FirstOrDefault()).FirstOrDefault().ToString(), Value = p.Function.Select(x => x.Swconfigfunctionareas.Select(x => x.Functionareaname).FirstOrDefault()).FirstOrDefault().ToString() }).Distinct().ToList(),


             "functionAreaDescription" =>
             query.Select(p => new FilterValueDto
             { Text = p.Function.Select(x => x.Swconfigfunctionareas.Select(x => x.Functionareadescription).FirstOrDefault()).FirstOrDefault().ToString(),
                Value = p.Function.Select(x => x.Swconfigfunctionareas.Select(x => x.Functionareadescription).FirstOrDefault()).FirstOrDefault().ToString()
            }).Distinct().ToList(),



                "creationUser" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto(p.CreationuserNavigation.Email)).Distinct().ToList()
                : query
                    .Where(x =>
                        x.CreationuserNavigation.Email.Contains(
                            propertyFilter)).Select(p => new FilterValueDto(p.CreationuserNavigation.Email)).Distinct().ToList(),


            "creationDate" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto { Text = p.Creationdate.ToString(), Value = p.Creationdate.ToString() }).Distinct().ToList()
                : query
                    .Where(x =>
                        x.Creationdate.ToString().Contains(
                            propertyFilter)).Select(p => new FilterValueDto { Text = p.Creationdate.ToString(), Value = p.Creationdate.ToString() }).Distinct().ToList(),


            "modificationUser" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto { Text = p.ModificationuserNavigation.Email.ToString(), Value = p.ModificationuserNavigation.Email.ToString() }).Distinct().ToList()
                : query
                    .Where(x =>
                        x.ModificationuserNavigation.Email.ToString().Contains(
                            propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationuserNavigation.Email.ToString(), Value = p.ModificationuserNavigation.Email.ToString() }).Distinct().ToList(),


            "modificationDate" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto { Text = p.Modificationdate.ToString(), Value = p.Modificationdate.ToString() }).Distinct().ToList()
                : query
                    .Where(x =>
                        x.Modificationdate.ToString().Contains(
                            propertyFilter)).Select(p => new FilterValueDto { Text = p.Modificationdate.ToString(), Value = p.Modificationdate.ToString() }).Distinct().ToList(),


                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

        #endregion

        #region // UIMemberFunctionForSubFunctioin
        public QueryResultDto<SWConfigSubFunctionGridDto> GetSWConfigSubFunctionDetails(SWConfigQueryDto dto, bool isExport)
        {
            var predicateResult = ApplyFilterForSubFunction(dto);
            var rtn = new QueryResultDto<SWConfigSubFunctionGridDto>(new GenerateRenderForGrid<SWConfigSubFunctionGridDto>(_manager))
            {

            };
            // var query = _repositoryWrapper.SWConfigSubFunctionRepository.FindByCondition(x=>x.Swconfigfunctionareaid == id);
            var query = GetQueryForSubFunction(predicateResult);
            query = query.ApplyOrdering(dto, GetColumnsMapForSubFunction());
           // query = query.Include(x => x.Swconfigsubfunctionareas).Include(x=>x.ModificationuserNavigation).Include(x=>x.CreationuserNavigation);

            var data = query.ToList();
            var result = new List<SWConfigSubFunctionGridDto>();

            foreach (var item in data)
            {
                if (item.Swconfigsubfunctionareas != null && item.Swconfigsubfunctionareas.Any())
                {
                    foreach (var subFunctionArea in item.Swconfigsubfunctionareas)
                    {
                        result.Add(GetSWConfigSubFunctionAreas(item,subFunctionArea));                                             
                    }
                }
                else
                {                   
                    result.Add(GetSWConfigSubFunction(item));
                }
            }


            rtn.TotalItems = result.Count();
            if (isExport)
            {
                result = result.ToList();
            }
            else
            {
                result = result.ApplyPaginationList(dto);
            }
            rtn.Items = result.ToArray();
            return rtn;
        }
        private static ExpressionStarter<Swconfigsubfunction> ApplyFilterForSubFunction(SWConfigQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Swconfigsubfunction>();
            var predicateInner = PredicateBuilder.New<Swconfigsubfunction>();
            if (buildFilterDto.SWConfigFunctionAreaId != null && buildFilterDto.SWConfigFunctionAreaId.Any())
            {
                predicateInner = PredicateBuilder.New<Swconfigsubfunction>();
                foreach (var item in buildFilterDto.SWConfigFunctionAreaId)
                    predicateInner.Or(x => x.Swconfigfunctionareaid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubFunctionId != null && buildFilterDto.SubFunctionId.Any())
            {
                predicateInner = PredicateBuilder.New<Swconfigsubfunction>();
                foreach (var item in buildFilterDto.SubFunctionId)
                    predicateInner.Or(x => x.Swconfigsubfunctionid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubFunctionName != null && buildFilterDto.SubFunctionName.Any())
            {
                predicateInner = PredicateBuilder.New<Swconfigsubfunction>();
                foreach (var item in buildFilterDto.SubFunctionName)
                    predicateInner.Or(x => x.Subfunctionname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubFunctioinAreaId != null && buildFilterDto.SubFunctioinAreaId.Any())
            {
                predicateInner = PredicateBuilder.New<Swconfigsubfunction>();
                foreach (var item in buildFilterDto.SubFunctioinAreaId)
                    predicateInner.Or(x => x.Swconfigsubfunctionareas.Any(x=>x.Swconfigsubfunctionareaid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubFunctionAreaName != null && buildFilterDto.SubFunctionAreaName.Any())
            {
                predicateInner = PredicateBuilder.New<Swconfigsubfunction>();
                foreach (var item in buildFilterDto.SubFunctionAreaName)
                    predicateInner.Or(x => x.Swconfigsubfunctionareas.Any(x=>x.Subfunctionareaname == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubFunctionAreaDescription != null && buildFilterDto.SubFunctionAreaDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Swconfigsubfunction>();
                foreach (var item in buildFilterDto.SubFunctionAreaDescription)
                    predicateInner.Or(x => x.Swconfigsubfunctionareas.Any(x => x.Subfunctionareadescription == item));
                predicateResult.And(predicateInner);
            }           
            if (buildFilterDto.Creationuser != null && buildFilterDto.Creationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Swconfigsubfunction>();
                foreach (var item in buildFilterDto.Creationuser)
                    predicateInner.Or(x => x.CreationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Modificationuser != null && buildFilterDto.Modificationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Swconfigsubfunction>();
                foreach (var item in buildFilterDto.Modificationuser)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationdate != null)
            {
                predicateInner = PredicateBuilder.New<Swconfigsubfunction>();
                if (buildFilterDto.Creationdate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= buildFilterDto.Creationdate.StartDate);
                if (buildFilterDto.Creationdate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= buildFilterDto.Creationdate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Modificationdate != null)
            {
                predicateInner = PredicateBuilder.New<Swconfigsubfunction>();
                if (buildFilterDto.Modificationdate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= buildFilterDto.Modificationdate.StartDate);
                if (buildFilterDto.Modificationdate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= buildFilterDto.Modificationdate.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }

        private IQueryable<Swconfigsubfunction> GetQueryForSubFunction(ExpressionStarter<Swconfigsubfunction> predicateResult)
        {

            var query = predicateResult.IsStarted
            ? _repositoryWrapper.SWConfigSubFunctionRepository.FindByCondition(predicateResult)
            .Include(x => x.Swconfigsubfunctionareas)
            .Include(x=>x.CreationuserNavigation)
            .Include(x=>x.ModificationuserNavigation)
           : _repositoryWrapper.SWConfigSubFunctionRepository.FindAll()          
           .Include(x => x.Swconfigsubfunctionareas)
           .Include(x => x.CreationuserNavigation)
            .Include(x => x.ModificationuserNavigation);
            return query;
        }


        private Dictionary<string, Expression<Func<Swconfigsubfunction, object>>[]> GetColumnsMapForSubFunction()
        {
            return new Dictionary<string, Expression<Func<Swconfigsubfunction, object>>[]>
            {
                
                ["subFunctionId"] = new Expression<Func<Swconfigsubfunction, object>>[] { p => p.Swconfigsubfunctionid },
                ["subFunctionName"] = new Expression<Func<Swconfigsubfunction, object>>[] { p => p.Subfunctionname },
                ["modificationDate"] = new Expression<Func<Swconfigsubfunction, object>>[] { p => p.Modificationdate },
                ["modificationUser"] = new Expression<Func<Swconfigsubfunction, object>>[] { p => p.Modificationuser },
                ["creationUser"] = new Expression<Func<Swconfigsubfunction, object>>[] { p => p.Creationuser },
                ["creationDate"] = new Expression<Func<Swconfigsubfunction, object>>[] { p => p.Creationdate }
            };
        }


        public List<FilterValueDto> GetFilterForSubFunction(string propertyName, string propertyFilter, SWConfigQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilterForSubFunction(buildFilterDto);

            var query = GetQueryForSubFunction(predicateResult);

            var rtn = propertyName switch
            {
                "subFunctionId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.Swconfigsubfunctionid.ToString(), Value = p.Swconfigsubfunctionid.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.Swconfigsubfunctionid.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.Swconfigsubfunctionid.ToString(), Value = p.Swconfigsubfunctionid.ToString() }).Distinct()
                   .ToList(),

                "subFunctionName" => string.IsNullOrEmpty(propertyFilter)
           ? query.Select(p => new FilterValueDto
           { Text = p.Subfunctionname, Value = p.Subfunctionname }).Distinct().ToList()
           : query
               .Where(x => x.Subfunctionname.Contains(propertyFilter)).Select(p =>
                   new FilterValueDto { Text = p.Subfunctionname, Value = p.Subfunctionname }).Distinct()
               .ToList(),

                "subFunctionAreaId" =>
           query.Select(p => new FilterValueDto
           { Text = p.Swconfigsubfunctionareas.Select(x => x.Swconfigsubfunctionareaid).FirstOrDefault().ToString(), Value = p.Swconfigsubfunctionareas.Select(x => x.Swconfigsubfunctionareaid).FirstOrDefault().ToString() }).Distinct().ToList(),

                "subFunctionAreaName" =>
              query.Select(p => new FilterValueDto
              { Text = p.Swconfigsubfunctionareas.Select(x => x.Subfunctionareaname).FirstOrDefault().ToString(), Value = p.Swconfigsubfunctionareas.Select(x => x.Swconfigsubfunctionareaid).FirstOrDefault().ToString() }).Distinct().ToList(),


                "subFunctionAreaDescription" =>
                query.Select(p => new FilterValueDto
                {
                    Text = p.Swconfigsubfunctionareas.Select(x => x.Subfunctionareadescription).FirstOrDefault().ToString(),
                    Value = p.Swconfigsubfunctionareas.Select(x => x.Swconfigsubfunctionareaid).FirstOrDefault().ToString()
                }).Distinct().ToList(),



                "creationUser" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto(p.CreationuserNavigation.Email)).Distinct().ToList()
                : query
                    .Where(x =>
                        x.CreationuserNavigation.Email.Contains(
                            propertyFilter)).Select(p => new FilterValueDto(p.CreationuserNavigation.Email)).Distinct().ToList(),


                "creationDate" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Creationdate.ToString(), Value = p.Creationdate.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Creationdate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Creationdate.ToString(), Value = p.Creationdate.ToString() }).Distinct().ToList(),


                "modificationUser" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationuserNavigation.Email.ToString(), Value = p.ModificationuserNavigation.Email.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationuserNavigation.Email.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationuserNavigation.Email.ToString(), Value = p.ModificationuserNavigation.Email.ToString() }).Distinct().ToList(),


                "modificationDate" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Modificationdate.ToString(), Value = p.Modificationdate.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Modificationdate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Modificationdate.ToString(), Value = p.Modificationdate.ToString() }).Distinct().ToList(),


                _ => new List<FilterValueDto>()
            };

            return rtn;
        }
        #endregion

        #region // Get Child SW Config details
        public SWConfigSubFunctionGridDto GetSWConfigSubFunctionAreas(Swconfigsubfunction subFunction, Swconfigsubfunctionareas subFunctionArea)
        {
            var grid = new SWConfigSubFunctionGridDto()
            {
                SubFunctionId = subFunction.Swconfigsubfunctionid,
                SubFunctionName = subFunction.Subfunctionname,
                SubFunctionAreaId = subFunctionArea.Swconfigsubfunctionareaid,
                SubFuncAreaName = subFunctionArea.Subfunctionareaname,
                SubFuncAreaDescription = subFunctionArea.Subfunctionareadescription,
                CreationDate = subFunction.Creationdate,
                CreationUser = subFunction.CreationuserNavigation.Email,
                ModificationDate = subFunction.Modificationdate,
                ModificationUser = subFunction.ModificationuserNavigation.Email,
            };
            return grid;
        }
        public SWConfigSubFunctionGridDto GetSWConfigSubFunction(Swconfigsubfunction subFunction)
        {
            var grid = new SWConfigSubFunctionGridDto()
            {
                SubFunctionId = subFunction.Swconfigsubfunctionid,
                SubFunctionName = subFunction.Subfunctionname,
                SubFunctionAreaId = 0,
                CreationDate = subFunction.Creationdate,
                CreationUser = subFunction.CreationuserNavigation.Email,
                ModificationDate = subFunction.Modificationdate,
                ModificationUser = subFunction.ModificationuserNavigation.Email,
                
            };
            return grid;
        }

        public SWConfigGridDto GetSWConfigFunctionArea(Softwareconfiguration softwareconfiguration,Function function,Swconfigfunctionareas functionArea)
        {
            var grid = new SWConfigGridDto
            {
                SoftwareConfigurationId = softwareconfiguration.Softwareconfigurationid,
                OpCo = softwareconfiguration.Opco,
                Oem = softwareconfiguration.Oem,
                ElementName = softwareconfiguration.Elementname,
                FunctionId = function.Functionid,
                FunctionName = function.Functionname,
                FunctionAreaDescription = functionArea.Functionareadescription,
                swConfigFunctionAreaId = functionArea.Swconfigfunctionareaid,
                FunctionAreaName = functionArea.Functionareaname,
                creationDate = softwareconfiguration.Creationdate,
                creationUser = softwareconfiguration.CreationuserNavigation.Email,
                modificationDate = softwareconfiguration.Modificationdate,
                modificationUser = softwareconfiguration.ModificationuserNavigation.Email,

            };
            return grid;
        }

        public SWConfigGridDto GetSWConfigFunction(Softwareconfiguration softwareconfiguration, Function function)
        {
            var grid = new SWConfigGridDto
            {
                SoftwareConfigurationId = softwareconfiguration.Softwareconfigurationid,
                OpCo = softwareconfiguration.Opco,
                Oem = softwareconfiguration.Oem,
                ElementName = softwareconfiguration.Elementname,
                FunctionId = function.Functionid,
                FunctionName = function.Functionname,
                swConfigFunctionAreaId = 0,
                creationDate = softwareconfiguration.Creationdate,
                creationUser = softwareconfiguration.CreationuserNavigation.Email,
                modificationDate = softwareconfiguration.Modificationdate,
                modificationUser = softwareconfiguration.ModificationuserNavigation.Email,
            };
            return grid;
        }

        public SWConfigGridDto GetSWConfig(Softwareconfiguration softwareconfiguration)
        {
            var grid = new SWConfigGridDto
            {
                SoftwareConfigurationId = softwareconfiguration.Softwareconfigurationid,
                OpCo = softwareconfiguration.Opco,
                Oem = softwareconfiguration.Oem,
                ElementName = softwareconfiguration.Elementname,
                FunctionId = 0,
                swConfigFunctionAreaId = 0,
                creationDate = softwareconfiguration.Creationdate,
                creationUser = softwareconfiguration.CreationuserNavigation.Email,
                modificationDate = softwareconfiguration.Modificationdate,
                modificationUser = softwareconfiguration.ModificationuserNavigation.Email,
            };
            return grid;
        }
        #endregion

        #region // download Excel
        public QueryResultDto<SWConfigDownloadExcel> GetDownloadExcel(SWConfigQueryDto dto, bool isExport)
        {
            var predicateResult = ApplyFilter(dto);
            var rtn = new QueryResultDto<SWConfigDownloadExcel>(new GenerateRenderForGrid<SWConfigDownloadExcel>(_manager))
            {

            };
            var query = GetQuery(predicateResult);
            query = query.ApplyOrdering(dto, GetColumnsMap());

            var data = query.ToList();
            var result = new List<SWConfigDownloadExcel>();
            foreach (var item in data)
            {
                if (item.Function != null && item.Function.Any())
                {
                    foreach (var function in item.Function)
                    {
                        if (function.Swconfigfunctionareas != null && function.Swconfigfunctionareas.Any())
                        {
                            foreach (var functionArea in function.Swconfigfunctionareas)
                            {
                                if (functionArea.Swconfigsubfunction != null && functionArea.Swconfigsubfunction.Any())
                                {
                                    foreach (var subFunction in functionArea.Swconfigsubfunction)
                                    {
                                        if (subFunction.Swconfigsubfunctionareas != null && subFunction.Swconfigsubfunctionareas.Any())
                                        {
                                            foreach (var subFunctionArea in subFunction.Swconfigsubfunctionareas)
                                            {
                                                result.Add(GetSWConfigSubFunctionAreasForDownload(item, function, functionArea, subFunction, subFunctionArea));
                                            }
                                        }
                                        else
                                        {
                                            result.Add(GetSWConfigSubFunctionForDownload(item, function, functionArea, subFunction));
                                        }
                                    }
                                }
                                else
                                {
                                    result.Add(GetSWConfigFunctionAreaForDownload(item, function, functionArea));
                                }
                            }
                        }
                        else
                        {
                            result.Add(GetSWConfigFunctionForDownload(item, function));
                        }
                    }
                }
                else
                {
                    result.Add(GetSWConfigForDownload(item));
                }
            }


            rtn.TotalItems = result.Count();
            if (isExport)
            {
                result = result.ToList();
            }
            else
            {
                result = result.ApplyPaginationList(dto);
            }
            rtn.Items = result.ToArray();
            return rtn;
        }
        #endregion

        #region // download excel function
        public SWConfigDownloadExcel GetSWConfigSubFunctionAreasForDownload(Softwareconfiguration softwareconfiguration, Function function, 
            Swconfigfunctionareas functionArea, Swconfigsubfunction subFunction, Swconfigsubfunctionareas subFunctionArea)
        {
            var grid = new SWConfigDownloadExcel()
            {
                SoftwareConfigurationId = softwareconfiguration.Softwareconfigurationid,
                OpCo = softwareconfiguration.Opco,
                Oem = softwareconfiguration.Oem,
                ElementName = softwareconfiguration.Elementname,
                FunctionId = function.Functionid,
                FunctionName = function.Functionname,
                swConfigFunctionAreaId = functionArea.Swconfigfunctionareaid,
                FunctionAreaName = functionArea.Functionareaname,
                FunctionAreaDescription = functionArea.Functionareadescription,
                SubFunctionId = subFunction.Swconfigsubfunctionid,
                SubFunctionName = subFunction.Subfunctionname,
                SubFunctionAreaId = subFunctionArea.Swconfigsubfunctionareaid,
                SubFuncAreaName = subFunctionArea.Subfunctionareaname,
                SubFuncAreaDescription = subFunctionArea.Subfunctionareadescription,
                CreationDate = softwareconfiguration.Creationdate,
                CreationUser = softwareconfiguration.CreationuserNavigation.Email,
                ModificationDate = softwareconfiguration.Modificationdate,
                ModificationUser = softwareconfiguration.ModificationuserNavigation.Email,
            };
            return grid;
        }
        public SWConfigDownloadExcel GetSWConfigSubFunctionForDownload(Softwareconfiguration softwareconfiguration, Function function, 
            Swconfigfunctionareas functionArea, Swconfigsubfunction subFunction)
        {
            var grid = new SWConfigDownloadExcel()
            {
                SoftwareConfigurationId = softwareconfiguration.Softwareconfigurationid,
                OpCo = softwareconfiguration.Opco,
                Oem = softwareconfiguration.Oem,
                ElementName = softwareconfiguration.Elementname,
                FunctionId = function.Functionid,
                FunctionName = function.Functionname,
                swConfigFunctionAreaId = functionArea.Swconfigfunctionareaid,
                FunctionAreaName = functionArea.Functionareaname,
                FunctionAreaDescription = functionArea.Functionareadescription,
                SubFunctionId = subFunction.Swconfigsubfunctionid,
                SubFunctionName = subFunction.Subfunctionname,
                SubFunctionAreaId = 0,
                CreationDate = softwareconfiguration.Creationdate,
                CreationUser = softwareconfiguration.CreationuserNavigation.Email,
                ModificationDate = softwareconfiguration.Modificationdate,
                ModificationUser = softwareconfiguration.ModificationuserNavigation.Email,

            };
            return grid;
        }

        public SWConfigDownloadExcel GetSWConfigFunctionAreaForDownload(Softwareconfiguration softwareconfiguration, Function function, 
            Swconfigfunctionareas functionArea)
        {
            var grid = new SWConfigDownloadExcel
            {
                SoftwareConfigurationId = softwareconfiguration.Softwareconfigurationid,
                OpCo = softwareconfiguration.Opco,
                Oem = softwareconfiguration.Oem,
                ElementName = softwareconfiguration.Elementname,
                FunctionId = function.Functionid,
                FunctionName = function.Functionname,
                FunctionAreaDescription = functionArea.Functionareadescription,
                swConfigFunctionAreaId = functionArea.Swconfigfunctionareaid,
                FunctionAreaName = functionArea.Functionareaname,
                SubFunctionId = 0,
                SubFunctionAreaId =0,
                CreationDate = softwareconfiguration.Creationdate,
                CreationUser = softwareconfiguration.CreationuserNavigation.Email,
                ModificationDate = softwareconfiguration.Modificationdate,
                ModificationUser = softwareconfiguration.ModificationuserNavigation.Email,

            };
            return grid;
        }

        public SWConfigDownloadExcel GetSWConfigFunctionForDownload(Softwareconfiguration softwareconfiguration, Function function)
        {
            var grid = new SWConfigDownloadExcel
            {
                SoftwareConfigurationId = softwareconfiguration.Softwareconfigurationid,
                OpCo = softwareconfiguration.Opco,
                Oem = softwareconfiguration.Oem,
                ElementName = softwareconfiguration.Elementname,
                FunctionId = function.Functionid,
                FunctionName = function.Functionname,
                swConfigFunctionAreaId = 0,
                SubFunctionId = 0,
                SubFunctionAreaId = 0,
                CreationDate = softwareconfiguration.Creationdate,
                CreationUser = softwareconfiguration.CreationuserNavigation.Email,
                ModificationDate = softwareconfiguration.Modificationdate,
                ModificationUser = softwareconfiguration.ModificationuserNavigation.Email,
            };
            return grid;
        }

        public SWConfigDownloadExcel GetSWConfigForDownload(Softwareconfiguration softwareconfiguration)
        {
            var grid = new SWConfigDownloadExcel
            {
                SoftwareConfigurationId = softwareconfiguration.Softwareconfigurationid,
                OpCo = softwareconfiguration.Opco,
                Oem = softwareconfiguration.Oem,
                ElementName = softwareconfiguration.Elementname,
                FunctionId = 0,
                swConfigFunctionAreaId = 0,
                SubFunctionId = 0,
                SubFunctionAreaId = 0,
                CreationDate = softwareconfiguration.Creationdate,
                CreationUser = softwareconfiguration.CreationuserNavigation.Email,
                ModificationDate = softwareconfiguration.Modificationdate,
                ModificationUser = softwareconfiguration.ModificationuserNavigation.Email,
            };
            return grid;
        }
        #endregion


        public SWConfigurationFilterDto GetSWConfigFilter(List<string> opcoDescriptionList)
        {
            try
            {
                var getSWConfig = _repositoryWrapper.SoftwareConfiguration.FindAll();
                var result = new SWConfigurationFilterDto()
                {
                    OpCoResource = opcoDescriptionList!=null && opcoDescriptionList.Count > 0?
                                   getSWConfig.Where(x=>x.Opco!=null && opcoDescriptionList.Contains(x.Opco)).Select(x=>x.Opco).Distinct().ToList(): 
                                   getSWConfig.Where(x => x.Opco != null).Select(x => x.Opco).Distinct().ToList(),
                    OemResource = getSWConfig.Where(x => x.Oem != null).Select(x => x.Oem).Distinct().ToList(),
                    ElementResource = getSWConfig.Where(x => x.Elementname != null).Select(x => x.Elementname).Distinct().ToList(),
                };
                return result;
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
