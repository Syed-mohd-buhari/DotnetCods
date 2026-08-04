using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.Idenitty;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
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

namespace CAM.BusinessManager.Entity
{
    public class HardwareConfigurationManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        public HardwareConfigurationManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }



        public QueryResultDto<HardwareConfigurationDtoGrid> FindWithCondition(HardwareConfigurationQueryDto designComponentFilterDto)
        {
            var predicateResult = ApplyFilter(designComponentFilterDto);
            if (designComponentFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            var rtn = new QueryResultDto<HardwareConfigurationDtoGrid>(new GenerateRenderForGrid<HardwareConfigurationDtoGrid>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.HardwareConfiguration.Count(predicateResult) : _repositoryWrapper.HardwareConfiguration.Count(),
            };
            var query = GetQuery(predicateResult, designComponentFilterDto.Deleted ?? false).ApplyOrdering(designComponentFilterDto, GetColumnsMap()).ApplyPaging(designComponentFilterDto);
         
            var data = query.ToList();

            IEnumerable<HardwareConfigurationDtoGrid> HardwareConfigurationResult;


            HardwareConfigurationResult = _mapper.Map<IEnumerable<HardwareConfigurationDtoGrid>>(data);

            rtn.Items = HardwareConfigurationResult.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Hardwareconfiguration> ApplyFilter(HardwareConfigurationQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Hardwareconfiguration>();
            var predicateInner = PredicateBuilder.New<Hardwareconfiguration>();

            if (buildFilterDto.HardwareConfigurationid != null && buildFilterDto.HardwareConfigurationid.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.HardwareConfigurationid)
                    predicateInner.Or(x => x.Hardwareconfigurationid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NetworkElementId != null && buildFilterDto.NetworkElementId.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.NetworkElementId)
                    predicateInner.Or(x => x.Networkelementid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Opco != null && buildFilterDto.Opco.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.Opco)
                    predicateInner.Or(x => x.Opco == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Oem != null && buildFilterDto.Oem.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.Oem)
                    predicateInner.Or(x => x.Oem == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Elementname != null && buildFilterDto.Elementname.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.Elementname)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Hardwaretype!= null && buildFilterDto.Hardwaretype.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.Hardwaretype)
                    predicateInner.Or(x => x.Hardwaretype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Productname != null && buildFilterDto.Productname.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.Productname)
                    predicateInner.Or(x => x.Productname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Serialnumber != null && buildFilterDto.Serialnumber.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.Serialnumber)
                    predicateInner.Or(x => x.Serialnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Unitlocation != null && buildFilterDto.Unitlocation.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.Unitlocation)
                    predicateInner.Or(x => x.Unitlocation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Vendor != null && buildFilterDto.Vendor.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.Vendor)
                    predicateInner.Or(x => x.Vendor == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Revision != null && buildFilterDto.Revision.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.Revision)
                    predicateInner.Or(x => x.Revision == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProductNumber != null && buildFilterDto.ProductNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.ProductNumber)
                    predicateInner.Or(x => x.Productnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationuser != null && buildFilterDto.Creationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.Creationuser)
                    predicateInner.Or(x => x.Creationuser >0);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Modificationuser != null && buildFilterDto.Modificationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                foreach (var item in buildFilterDto.Modificationuser)
                    predicateInner.Or(x => x.Modificationuser >0);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationdate != null)
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                if (buildFilterDto.Creationdate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= buildFilterDto.Creationdate.StartDate);
                if (buildFilterDto.Creationdate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= buildFilterDto.Creationdate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Modificationdate != null)
            {
                predicateInner = PredicateBuilder.New<Hardwareconfiguration>();
                if (buildFilterDto.Modificationdate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= buildFilterDto.Modificationdate.StartDate);
                if (buildFilterDto.Modificationdate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= buildFilterDto.Modificationdate.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }

        private IQueryable<HardwareConfiguration> GetQuery(ExpressionStarter<Hardwareconfiguration> predicateResult, bool includeDeleted)

        {

            var query = predicateResult.IsStarted
                ? _repositoryWrapper.HardwareConfiguration.FindByCondition(predicateResult, includeDeleted)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
               : _repositoryWrapper.HardwareConfiguration.FindAll()
                  .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation);
            return query.AsEnumerable().Select(x => HardwareConfigurationMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<HardwareConfiguration, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<HardwareConfiguration, object>>[]>
            {
                ["hardwareConfigurationId"] =new Expression<Func<HardwareConfiguration, object>>[] {p=> p.Hardwareconfigurationid},
                ["networkElementId"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.Networkelementid },
                ["opCo"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.Opco },
                ["oem"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.Oem },
                ["elementName"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.Elementname },
                ["productName"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.Productname },
                ["serialNumber"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.Serialnumber },
                ["unitLocation"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.Unitlocation },
                ["vendor"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.Vendor },
                ["productNumber"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.Productnumber },
                ["revision"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.Revision },
                ["creationUser"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.CreationUser},
                ["creationDate"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.Creationdate },
                ["modificationDate"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.Modificationdate },
                ["modificationUser"] = new Expression<Func<HardwareConfiguration, object>>[] { p => p.ModificationUser }
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, HardwareConfigurationQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "hardwareConfigurationId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.Hardwareconfigurationid.ToString(), Value = p.Hardwareconfigurationid.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.Hardwareconfigurationid.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.Hardwareconfigurationid.ToString(), Value = p.Hardwareconfigurationid.ToString() }).Distinct()
                   .ToList(),

                "networkElementId" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.Networkelementid.ToString(), Value = p.Networkelementid.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.Networkelementid.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.Networkelementid.ToString(), Value = p.Networkelementid.ToString() }).Distinct()
                    .ToList(),

                "opCo" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.Opco.ToString(), Value = p.Opco.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.Opco.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.Opco.ToString(), Value = p.Opco.ToString() }).Distinct()
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

                "hardwareType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Hardwaretype, Value = p.Hardwaretype }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Hardwaretype.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Hardwaretype, Value = p.Hardwaretype }).Distinct().ToList(),


                "productName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Productname, Value = p.Productname }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Productname.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Productname, Value = p.Productname }).Distinct().ToList(),

                "serialNumber" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Serialnumber, Value = p.Serialnumber }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Serialnumber.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Serialnumber, Value = p.Serialnumber }).Distinct().ToList(),


                "unitLocation" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Unitlocation, Value = p.Unitlocation }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Unitlocation.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Unitlocation, Value = p.Unitlocation }).Distinct().ToList(),


                "vendor" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.Vendor, Value = p.Vendor }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.Vendor.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.Vendor, Value = p.Vendor }).Distinct().ToList(),

                "creationUser" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.CreationUserEntity.Email.ToString(), Value = p.CreationUserEntity.Email.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.CreationUserEntity.Email.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.CreationUserEntity.Email.ToString(), Value = p.CreationUserEntity.Email.ToString() }).Distinct().ToList(),

                "productNumber" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.Productnumber, Value = p.Productnumber }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.Productnumber.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.Productnumber, Value = p.Productnumber }).Distinct().ToList(),

                "revision" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.Revision, Value = p.Revision }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.Revision.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.Revision, Value = p.Revision }).Distinct().ToList(),

                "creationDate" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.Creationdate.ToString(), Value = p.Creationdate.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.Creationdate.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.Creationdate.ToString(), Value = p.Creationdate.ToString() }).Distinct().ToList(),


                "modificationUser" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList(),


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

    }
}
