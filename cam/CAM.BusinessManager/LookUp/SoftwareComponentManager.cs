using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.Component;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
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

namespace CAM.BusinessManager.LookUp
{
    public class SoftwareComponentManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        public SoftwareComponentManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }



        public QueryResultDto<SoftwareComponentsDtoGrid> FindWithCondition(SoftwareComponentQueryDto designComponentFilterDto)
        {
            var predicateResult = ApplyFilter(designComponentFilterDto);
            if (designComponentFilterDto.Deleted == true)
            {
                 predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            var rtn = new QueryResultDto<SoftwareComponentsDtoGrid>(new GenerateRenderForGrid<SoftwareComponentsDtoGrid>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.Component.Count(predicateResult) : _repositoryWrapper.Component.Count(),
            };
            var query = GetQuery(predicateResult, designComponentFilterDto.Deleted ?? false).ApplyOrdering(designComponentFilterDto, GetColumnsMap()).ApplyPaging(designComponentFilterDto);

            var data = query.ToList();

            IEnumerable<SoftwareComponentsDtoGrid> ComponentResult;


            ComponentResult = _mapper.Map<IEnumerable<SoftwareComponentsDtoGrid>>(data);

            rtn.Items = ComponentResult.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Component> ApplyFilter(SoftwareComponentQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Component>();
            var predicateInner = PredicateBuilder.New<Component>();

            if (buildFilterDto.Softwarecomponentid != null && buildFilterDto.Softwarecomponentid.Any())
            {
                predicateInner = PredicateBuilder.New<Component>();
                foreach (var item in buildFilterDto.Softwarecomponentid)
                    predicateInner.Or(x => x.Softwarecomponentid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Networkelementid != null && buildFilterDto.Networkelementid.Any())
            {
                predicateInner = PredicateBuilder.New<Component>();
                foreach (var item in buildFilterDto.Networkelementid)
                    predicateInner.Or(x => x.Softwarecomponent.Networkelementid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ComponentId != null && buildFilterDto.ComponentId.Any())
            {
                predicateInner = PredicateBuilder.New<Component>();
                foreach (var item in buildFilterDto.ComponentId)
                    predicateInner.Or(x => x.Componentid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Opco != null && buildFilterDto.Opco.Any())
            {
                predicateInner = PredicateBuilder.New<Component>();
                foreach (var item in buildFilterDto.Opco)
                    predicateInner.Or(x => x.Softwarecomponent.Opco == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Oem != null && buildFilterDto.Oem.Any())
            {
                predicateInner = PredicateBuilder.New<Component>();
                foreach (var item in buildFilterDto.Oem)
                    predicateInner.Or(x => x.Softwarecomponent.Oem == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Elementname != null && buildFilterDto.Elementname.Any())
            {
                predicateInner = PredicateBuilder.New<Component>();
                foreach (var item in buildFilterDto.Elementname)
                    predicateInner.Or(x => x.Softwarecomponent.Elementname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Componentname != null && buildFilterDto.Componentname.Any())
            {
                predicateInner = PredicateBuilder.New<Component>();
                foreach (var item in buildFilterDto.Componentname)
                    predicateInner.Or(x => x.Componentname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Mainsoftwareversion != null && buildFilterDto.Mainsoftwareversion.Any())
            {
                predicateInner = PredicateBuilder.New<Component>();
                foreach (var item in buildFilterDto.Mainsoftwareversion)
                    predicateInner.Or(x => x.Softwarecomponent.Mainsoftwareversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Productiondate != null)
            {
                predicateInner = PredicateBuilder.New<Component>();
                if (buildFilterDto.Productiondate.StartDate != null)
                    predicateInner.And(x => x.Productiondate >= buildFilterDto.Productiondate.StartDate);
                if (buildFilterDto.Productiondate.EndDate != null)
                    predicateInner.And(x => x.Productiondate <= buildFilterDto.Productiondate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Productionrevision != null && buildFilterDto.Productionrevision.Any())
            {
                predicateInner = PredicateBuilder.New<Component>();
                foreach (var item in buildFilterDto.Productionrevision)
                    predicateInner.Or(x => x.Productionrevision == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Productionnumber != null && buildFilterDto.Productionnumber.Any())
            {
                predicateInner = PredicateBuilder.New<Component>();
                foreach (var item in buildFilterDto.Productionnumber)
                    predicateInner.Or(x => x.Productionnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationuser != null && buildFilterDto.Creationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Component>();
                foreach (var item in buildFilterDto.Creationuser)
                    predicateInner.Or(x => x.Softwarecomponent.Creationuser.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Modificationuser != null && buildFilterDto.Modificationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Component>();
                foreach (var item in buildFilterDto.Modificationuser)
                    predicateInner.Or(x => x.Softwarecomponent.Modificationuser.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationdate != null)
            {
                predicateInner = PredicateBuilder.New<Component>();
                if (buildFilterDto.Creationdate.StartDate != null)
                    predicateInner.And(x => x.Softwarecomponent.Creationdate.Date >= buildFilterDto.Creationdate.StartDate);
                if (buildFilterDto.Creationdate.EndDate != null)
                    predicateInner.And(x => x.Softwarecomponent.Creationdate.Date <= buildFilterDto.Creationdate.EndDate);
                predicateResult.And(predicateInner);
            }            
            if (buildFilterDto.Modificationdate != null)
            {
                predicateInner = PredicateBuilder.New<Component>();
                if (buildFilterDto.Modificationdate.StartDate != null)
                    predicateInner.And(x => x.Softwarecomponent.Modificationdate.Date >= buildFilterDto.Modificationdate.StartDate);
                if (buildFilterDto.Modificationdate.EndDate != null)
                    predicateInner.And(x => x.Softwarecomponent.Modificationdate.Date <= buildFilterDto.Modificationdate.EndDate);
                predicateResult.And(predicateInner);
            }           
            return predicateResult;
        }

        private IQueryable<Components> GetQuery(ExpressionStarter<Component> predicateResult, bool includeDeleted)
        {
            
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.Component.FindByCondition(predicateResult, includeDeleted)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
                .Select(x=>new Component
                {
                    #region //Component
                    Componentid = x.Componentid,
                    Softwarecomponentid = x.Softwarecomponentid,
                    Componentname = x.Componentname,
                    Productiondate = x.Productiondate,
                    Productionnumber = x.Productionnumber,
                    Productionrevision = x.Productionrevision,
                    Creationdate = x.Creationdate,
                    //Creationuser = x.Creationuser,
                    CreationuserNavigation =x.CreationuserNavigation,
                    ModificationuserNavigation =x.ModificationuserNavigation,
                    Modificationdate = x.Modificationdate,
                    //Modificationuser = x.Modificationuser,
                    #endregion
                    #region // SoftwareComponent
                    Softwarecomponent = new Softwarecomponent
                    {
                        Elementname = x.Softwarecomponent.Elementname,
                        Opco = x.Softwarecomponent.Opco,
                        Oem = x.Softwarecomponent.Oem,
                        Networkelementid = x.Softwarecomponent.Networkelementid,
                        Mainsoftwareversion = x.Softwarecomponent.Mainsoftwareversion,
                        Creationdate = x.Softwarecomponent.Creationdate,
                        Creationuser = x.Softwarecomponent.Creationuser,
                        Modificationdate = x.Softwarecomponent.Modificationdate,
                        Modificationuser = x.Softwarecomponent.Modificationuser,
                    }
                    #endregion
                })
               : _repositoryWrapper.Component.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
                 .Select(x => new Component
                 {
                     #region //Component
                     Componentid = x.Componentid,
                     Softwarecomponentid = x.Softwarecomponentid,
                     Componentname = x.Componentname,
                     Productiondate = x.Productiondate,
                     Productionnumber = x.Productionnumber,
                     Productionrevision = x.Productionrevision,
                     Creationdate = x.Creationdate,
                     //Creationuser = x.Creationuser,
                     CreationuserNavigation = x.CreationuserNavigation,
                     ModificationuserNavigation = x.ModificationuserNavigation,
                     Modificationdate = x.Modificationdate,
                     //Modificationuser = x.Modificationuser,
                     #endregion
                     #region // SoftwareComponent
                     Softwarecomponent = new Softwarecomponent
                     {
                         Elementname = x.Softwarecomponent.Elementname,
                         Opco = x.Softwarecomponent.Opco,
                         Oem = x.Softwarecomponent.Oem,
                         Networkelementid = x.Softwarecomponent.Networkelementid,
                         Mainsoftwareversion = x.Softwarecomponent.Mainsoftwareversion,
                         Creationdate = x.Softwarecomponent.Creationdate,
                         Creationuser = x.Softwarecomponent.Creationuser,
                         Modificationdate = x.Softwarecomponent.Modificationdate,
                         Modificationuser = x.Softwarecomponent.Modificationuser,
                     }
                     #endregion
                 });               
            return query.AsEnumerable().Select(x => ComponentMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<Components, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Components, object>>[]>
            {
                ["softwareComponentId"] = new Expression<Func<Components, object>>[] { p => p.softwarecomponentid },
                ["networkElementId"] = new Expression<Func<Components, object>>[] { p => p.NetworkElementId },
                ["opCo"] = new Expression<Func<Components, object>>[] { p => p.OpCO },
                ["oem"] = new Expression<Func<Components, object>>[] { p => p.Oem },
                ["elementName"] = new Expression<Func<Components, object>>[] { p => p.Elementname },
                ["mainSoftwareVersion"] = new Expression<Func<Components, object>>[] { p => p.Mainsoftwareversion },
                ["componentName"] = new Expression<Func<Components, object>>[] { p => p.Componentname },
                ["productionDate"] = new Expression<Func<Components, object>>[] { p => p.Productiondate },
                ["productionNumber"] = new Expression<Func<Components, object>>[] { p => p.Productionnumber },
                ["productionrevision"] = new Expression<Func<Components, object>>[] { p => p.ProductionRevision },
                ["modificationDate"] = new Expression<Func<Components, object>>[] { p => p.Modificationdate },
                ["modificationUser"] = new Expression<Func<Components, object>>[] { p => p.Modificationuser },
                ["creationUser"] = new Expression<Func<Components, object>>[] { p => p.Creationuser },
                ["creationDate"] = new Expression<Func<Components, object>>[] { p => p.Creationdate },
                ["componentModificationDate"] = new Expression<Func<Components, object>>[] { p => p.ComponentModificationdate },
                ["componentModificationUser"] = new Expression<Func<Components, object>>[] { p => p.ComponentModificationuser },
                ["componentCreationUser"] = new Expression<Func<Components, object>>[] { p => p.ComponentCreationuser },
                ["componentCreationUser"] = new Expression<Func<Components, object>>[] { p => p.ComponentCreationdate },
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, SoftwareComponentQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "componentId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.Componentid.ToString(), Value = p.Componentid.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.Componentid.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.Componentid.ToString(), Value = p.Componentid.ToString() }).Distinct()
                   .ToList(),

                "networkElementId" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.NetworkElementId.ToString(), Value = p.NetworkElementId.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.NetworkElementId.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.NetworkElementId.ToString(), Value = p.NetworkElementId.ToString() }).Distinct()
                    .ToList(),
                "softwareComponentId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.softwarecomponentid.ToString(), Value = p.softwarecomponentid.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.softwarecomponentid.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.softwarecomponentid.ToString(), Value = p.softwarecomponentid.ToString() }).Distinct()
                   .ToList(),

                "opCo" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.OpCO.ToString(), Value = p.OpCO.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.OpCO.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.OpCO.ToString(), Value = p.OpCO.ToString() }).Distinct()
                    .ToList(),

                "oem" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Oem, Value = p.Oem }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Oem.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Oem, Value = p.Oem }).Distinct().ToList(),

                "elementName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Elementname, Value = p.Elementname }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Elementname.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Elementname, Value = p.Elementname }).Distinct().ToList(),

                "mainSoftwareVersion" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Mainsoftwareversion, Value = p.Mainsoftwareversion }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Mainsoftwareversion.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Mainsoftwareversion, Value = p.Mainsoftwareversion }).Distinct().ToList(),


                "componentName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Componentname, Value = p.Componentname }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Componentname.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Componentname, Value = p.Componentname }).Distinct().ToList(),


                "productionDate" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.Productiondate.ToString(), Value = p.Productiondate.ToString() }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.Productiondate.ToString().Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.Productiondate.ToString(), Value = p.Productiondate.ToString() }).Distinct().ToList(),

                "productionNumber" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Productionnumber, Value = p.Productionnumber }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Productionnumber.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Productionnumber, Value = p.Productionnumber }).Distinct().ToList(),

                "productionRevision" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.ProductionRevision, Value = p.ProductionRevision }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.ProductionRevision.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.ProductionRevision, Value = p.ProductionRevision }).Distinct().ToList(),

                "creationUser" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.Creationuser.ToString(), Value = p.Creationuser.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.Creationuser.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.Creationuser.ToString(), Value = p.Creationuser.ToString() }).Distinct().ToList(),

                //"componentCreationUser" => string.IsNullOrEmpty(propertyFilter)
                //    ? query.Select(p => new FilterValueDto { Text = p.ComponentCreationuser.ToString(), Value = p.ComponentCreationuser.ToString() }).Distinct().ToList()
                //    : query
                //        .Where(x =>
                //            x.ComponentCreationuser.ToString().Contains(
                //                propertyFilter)).Select(p => new FilterValueDto { Text = p.ComponentCreationuser.ToString(), Value = p.ComponentCreationuser.ToString() }).Distinct().ToList(),

                "creationDate" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.SoftwareComponent.Creationdate.ToString(), Value = p.SoftwareComponent.Creationdate.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.SoftwareComponent.Creationdate.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.SoftwareComponent.Creationdate.ToString(), Value = p.SoftwareComponent.Creationdate.ToString() }).Distinct().ToList(),
                //"componentCreatioDate" => string.IsNullOrEmpty(propertyFilter)
                //   ? query.Select(p => new FilterValueDto { Text = p.ComponentCreationdate.ToString(), Value = p.ComponentCreationdate.ToString() }).Distinct().ToList()
                //   : query
                //       .Where(x =>
                //           x.ComponentCreationdate.ToString().Contains(
                //               propertyFilter)).Select(p => new FilterValueDto { Text = p.ComponentCreationdate.ToString(), Value = p.ComponentCreationdate.ToString() }).Distinct().ToList(),

                "modificationUser" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Modificationuser.ToString(), Value = p.Modificationuser.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Modificationuser.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Modificationuser.ToString(), Value = p.Modificationuser.ToString() }).Distinct().ToList(),
                //"componentModificationUser" => string.IsNullOrEmpty(propertyFilter)
                //    ? query.Select(p => new FilterValueDto { Text = p.ComponentModificationuser.ToString(), Value = p.ComponentModificationuser.ToString() }).Distinct().ToList()
                //    : query
                //        .Where(x =>
                //            x.ComponentModificationuser.ToString().Contains(
                //                propertyFilter)).Select(p => new FilterValueDto { Text = p.ComponentModificationuser.ToString(), Value = p.ComponentModificationuser.ToString() }).Distinct().ToList(),

                "modificationDate" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.SoftwareComponent.Modificationdate.ToString(), Value = p.SoftwareComponent.Modificationdate.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.SoftwareComponent.Modificationdate.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.SoftwareComponent.Modificationdate.ToString(), Value = p.SoftwareComponent.Modificationdate.ToString() }).Distinct().ToList(),

                //"componentModificationDate" => string.IsNullOrEmpty(propertyFilter)
                //   ? query.Select(p => new FilterValueDto { Text = p.ComponentModificationdate.ToString(), Value = p.ComponentModificationdate.ToString() }).Distinct().ToList()
                //   : query
                //       .Where(x =>
                //           x.ComponentModificationdate.ToString().Contains(
                //               propertyFilter)).Select(p => new FilterValueDto { Text = p.ComponentModificationdate.ToString(), Value = p.ComponentModificationdate.ToString() }).Distinct().ToList(),

                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

    }
}