using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElement;
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
    public class NetworkElementManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;

        public NetworkElementManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor, 
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }


        public QueryResultDto<NetworkelementDtoGrid> FindWithCondition(NetworkElementQueryDto networkElementFilterDto)
        {
            var predicateResult = ApplyFilter(networkElementFilterDto);
            if (networkElementFilterDto.Deleted == ConstantValueFilter.isTrue)
            {
               predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            var rtn = new QueryResultDto<NetworkelementDtoGrid>(new GenerateRenderForGrid<NetworkelementDtoGrid>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.NetworkElement.Count(predicateResult) : _repositoryWrapper.NetworkElement.Count(),
            };
            var query = GetQuery(predicateResult, networkElementFilterDto.Deleted ?? !ConstantValueFilter.isTrue).ApplyOrdering(networkElementFilterDto, GetColumnsMap()).ApplyPaging(networkElementFilterDto);
            var data = query.ToList();
                       

            IEnumerable<NetworkelementDtoGrid> NetworkElementResult;


            NetworkElementResult = _mapper.Map<IEnumerable<NetworkelementDtoGrid>>(data);
                
            rtn.Items = NetworkElementResult.ToArray();
            return rtn;
        }
        private static ExpressionStarter<Networkelement> ApplyFilter(NetworkElementQueryDto NetworkQueryFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Networkelement>();
            var predicateInner = PredicateBuilder.New<Networkelement>();

            if (NetworkQueryFilterDto.Networkelementid != null && NetworkQueryFilterDto.Networkelementid.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Networkelementid)
                    predicateInner.Or(x => x.Networkelementid == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Opco != null && NetworkQueryFilterDto.Opco.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Opco)
                    predicateInner.Or(x => x.Opco == item);
                predicateResult.And(predicateInner);
            }
            if (NetworkQueryFilterDto.Oem != null && NetworkQueryFilterDto.Oem.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Oem)
                    predicateInner.Or(x => x.Oem == item);
                predicateResult.And(predicateInner);
            }
            if (NetworkQueryFilterDto.Elementname != null && NetworkQueryFilterDto.Elementname.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Elementname)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Nodetype != null && NetworkQueryFilterDto.Nodetype.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Nodetype)
                    predicateInner.Or(x => x.Nodetype == item);
                predicateResult.And(predicateInner);
            }
            if (NetworkQueryFilterDto.NodeTypeName != null && NetworkQueryFilterDto.NodeTypeName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.NodeTypeName)
                    predicateInner.Or(x => x.Nodetypename == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Platformtype != null && NetworkQueryFilterDto.Platformtype.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Platformtype)
                    predicateInner.Or(x => x.Platformtype == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Sitelocation != null && NetworkQueryFilterDto.Sitelocation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Sitelocation)
                    predicateInner.Or(x => x.Sitelocation == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Dataacquisitiondate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                if (NetworkQueryFilterDto.Dataacquisitiondate.StartDate != null)
                    predicateInner.And(x => x.Dataacquisitiondate >= NetworkQueryFilterDto.Dataacquisitiondate.StartDate);
                if (NetworkQueryFilterDto.Dataacquisitiondate.EndDate != null)
                    predicateInner.And(x => x.Dataacquisitiondate <= NetworkQueryFilterDto.Dataacquisitiondate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (NetworkQueryFilterDto.Softwareinstalldate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                if (NetworkQueryFilterDto.Softwareinstalldate.StartDate != null)
                    predicateInner.And(x => x.Softwareinstalldate >= NetworkQueryFilterDto.Softwareinstalldate.StartDate);
                if (NetworkQueryFilterDto.Softwareinstalldate.EndDate != null)
                    predicateInner.And(x => x.Softwareinstalldate <= NetworkQueryFilterDto.Softwareinstalldate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Softwareinstalldateap != null)
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                if (NetworkQueryFilterDto.Softwareinstalldateap.StartDate != null)
                    predicateInner.And(x => x.Softwareinstalldateap >= NetworkQueryFilterDto.Softwareinstalldateap.StartDate);
                if (NetworkQueryFilterDto.Softwareinstalldateap.EndDate != null)
                    predicateInner.And(x => x.Softwareinstalldateap <= NetworkQueryFilterDto.Softwareinstalldateap.EndDate);
                predicateResult.And(predicateInner);
            }
            
            if (NetworkQueryFilterDto.Softwareinstalldatecp != null)
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                if (NetworkQueryFilterDto.Softwareinstalldatecp.StartDate != null)
                    predicateInner.And(x => x.Softwareinstalldatecp >= NetworkQueryFilterDto.Softwareinstalldatecp.StartDate);
                if (NetworkQueryFilterDto.Softwareinstalldatecp.EndDate != null)
                    predicateInner.And(x => x.Softwareinstalldatecp <= NetworkQueryFilterDto.Softwareinstalldatecp.EndDate);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Softwareproductdate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                if (NetworkQueryFilterDto.Softwareproductdate.StartDate != null)
                    predicateInner.And(x => x.Softwareproductdate >= NetworkQueryFilterDto.Softwareproductdate.StartDate);
                if (NetworkQueryFilterDto.Softwareproductdate.EndDate != null)
                    predicateInner.And(x => x.Softwareproductdate <= NetworkQueryFilterDto.Softwareproductdate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Softwareproductdateap != null)
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                if (NetworkQueryFilterDto.Softwareproductdateap.StartDate != null)
                    predicateInner.And(x => x.Softwareproductdateap >= NetworkQueryFilterDto.Softwareproductdateap.StartDate);
                if (NetworkQueryFilterDto.Softwareproductdateap.EndDate != null)
                    predicateInner.And(x => x.Softwareproductdateap <= NetworkQueryFilterDto.Softwareproductdateap.EndDate);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Softwareproductdatecp != null)
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                if (NetworkQueryFilterDto.Softwareproductdatecp.StartDate != null)
                    predicateInner.And(x => x.Softwareproductdatecp >= NetworkQueryFilterDto.Softwareproductdatecp.StartDate);
                if (NetworkQueryFilterDto.Softwareproductdatecp.EndDate != null)
                    predicateInner.And(x => x.Softwareproductdatecp <= NetworkQueryFilterDto.Softwareproductdatecp.EndDate);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Softwareproductnumber != null && NetworkQueryFilterDto.Softwareproductnumber.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Softwareproductnumber)
                    predicateInner.Or(x => x.Softwareproductnumber == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Softwareproductnumberap != null && NetworkQueryFilterDto.Softwareproductnumberap.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Softwareproductnumberap)
                    predicateInner.Or(x => x.Softwareproductnumberap == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Softwareproductnumbercp != null && NetworkQueryFilterDto.Softwareproductnumbercp.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Softwareproductnumbercp)
                    predicateInner.Or(x => x.Softwareproductnumbercp == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Softwarereleaseinformation != null && NetworkQueryFilterDto.Softwarereleaseinformation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Softwarereleaseinformation)
                    predicateInner.Or(x => x.Softwarereleaseinformation == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Softwarereleaseinformationap != null && NetworkQueryFilterDto.Softwarereleaseinformationap.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Softwarereleaseinformationap)
                    predicateInner.Or(x => x.Softwarereleaseinformationap == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Softwarereleaseinformationcp != null && NetworkQueryFilterDto.Softwarereleaseinformationcp.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Softwarereleaseinformationcp)
                    predicateInner.Or(x => x.Softwarereleaseinformationcp == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Spare1ossorenm != null && NetworkQueryFilterDto.Spare1ossorenm.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Spare1ossorenm)
                    predicateInner.Or(x => x.Spare1ossorenm == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Spare2xmlversion != null && NetworkQueryFilterDto.Spare2xmlversion.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Spare2xmlversion)
                    predicateInner.Or(x => x.Spare2xmlversion == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Elementname != null && NetworkQueryFilterDto.Elementname.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.Elementname)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Xmllastparsefiledate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                if (NetworkQueryFilterDto.Xmllastparsefiledate.StartDate != null)
                    predicateInner.And(x => x.Xmllastparsefiledate >= NetworkQueryFilterDto.Xmllastparsefiledate.StartDate);
                if (NetworkQueryFilterDto.Xmllastparsefiledate.EndDate != null)
                    predicateInner.And(x => x.Xmllastparsefiledate <= NetworkQueryFilterDto.Xmllastparsefiledate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (NetworkQueryFilterDto.LastModifiedBy != null && NetworkQueryFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                foreach (var item in NetworkQueryFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.CreationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.Creationdate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                if (NetworkQueryFilterDto.Creationdate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= NetworkQueryFilterDto.Creationdate.StartDate);
                if (NetworkQueryFilterDto.Creationdate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= NetworkQueryFilterDto.Creationdate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (NetworkQueryFilterDto.Modificationdate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelement>();
                if (NetworkQueryFilterDto.Modificationdate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= NetworkQueryFilterDto.Modificationdate.StartDate);
                if (NetworkQueryFilterDto.Modificationdate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= NetworkQueryFilterDto.Modificationdate.EndDate);
                predicateResult.And(predicateInner);
            }
          return predicateResult;
        }

        private IQueryable<NetworkElement> GetQuery(ExpressionStarter<Networkelement> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.NetworkElement.FindByCondition(predicateResult, includeDeleted)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
                : _repositoryWrapper.NetworkElement.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation);
            return query.AsEnumerable().Select(x => NetworkElementMapper.Get(x)).AsQueryable();
        }

        private Dictionary<string, Expression<Func<NetworkElement, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<NetworkElement, object>>[]>
            {
                ["networkElementId"] = new Expression<Func<NetworkElement, object>>[] { p => p.Networkelementid },
                ["lastModified"] = new Expression<Func<NetworkElement, object>>[] { p => p.CreationUser },
                ["lastModifiedBy"] = new Expression<Func<NetworkElement, object>>[] { p => p.CreationUserEntity.Email },
                ["opCo"] = new Expression<Func<NetworkElement, object>>[] { p => p.Opco },
                ["oem"] = new Expression<Func<NetworkElement, object>>[] { p => p.Oem },
                ["nodeType"] = new Expression<Func<NetworkElement, object>>[] { p => p.Nodetype },
                ["nodeTypeName"] = new Expression<Func<NetworkElement, object>>[] { p => p.NodeTypeName },
                ["elementName"] = new Expression<Func<NetworkElement, object>>[] { p => p.Elementname },
                ["dataAcquisitionDate"] = new Expression<Func<NetworkElement, object>>[] { p => p.Dataacquisitiondate },
                ["siteLocation"] = new Expression<Func<NetworkElement, object>>[] { p => p.Sitelocation },
                ["platformType"] = new Expression<Func<NetworkElement, object>>[] { p => p.Platformtype },
                ["softwareReleaseInformation"] = new Expression<Func<NetworkElement, object>>[] { p => p.Softwarereleaseinformation },
                ["softwareReleaseInformationAP"] = new Expression<Func<NetworkElement, object>>[] { p => p.Softwarereleaseinformationap },
                ["softwareReleaseInformationCP"] = new Expression<Func<NetworkElement, object>>[] { p => p.Softwarereleaseinformationcp },
                ["softwareProductNumber"] = new Expression<Func<NetworkElement, object>>[] { p => p.Softwareproductnumber },
                ["softwareProductNumberAP"] = new Expression<Func<NetworkElement, object>>[] { p => p.Softwareproductnumberap },
                ["softwareProductNumberCP"] = new Expression<Func<NetworkElement, object>>[] { p => p.Softwareproductnumbercp },
                ["softwareProductDate"] = new Expression<Func<NetworkElement, object>>[] { p => p.Softwareproductdate },
                ["softwareProductDateAP"] = new Expression<Func<NetworkElement, object>>[] { p => p.Softwareproductdateap },
                ["softwareProductDateCp"] = new Expression<Func<NetworkElement, object>>[] { p => p.Softwareproductdatecp },
                ["softwareInstallDate"] = new Expression<Func<NetworkElement, object>>[] { p => p.Softwareinstalldate },
                ["softwareInstallDateAP"] = new Expression<Func<NetworkElement, object>>[] { p => p.Softwareinstalldateap },
                ["softwareInstallDateCP"] = new Expression<Func<NetworkElement, object>>[] { p => p.Softwareinstalldatecp },
                ["spare1ossorenm"] = new Expression<Func<NetworkElement, object>>[] { p => p.Spare1ossorenm },
                ["spare2XmlVersion"] = new Expression<Func<NetworkElement, object>>[] { p => p.Spare2xmlversion },
                ["xmlLastParseFileDate"] = new Expression<Func<NetworkElement, object>>[] { p => p.Xmllastparsefiledate },
                ["modificationDate"] = new Expression<Func<NetworkElement, object>>[] { p => p.Modificationdate },
                ["modificationUser"] = new Expression<Func<NetworkElement, object>>[] { p => p.ModificationUser },
                ["creationDate"] = new Expression<Func<NetworkElement, object>>[] { p => p.Creationdate },
                ["creationUser"] = new Expression<Func<NetworkElement, object>>[] { p => p.CreationUser },
            };
        }
        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, NetworkElementQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, !ConstantValueFilter.isTrue);

            var rtn = propertyName switch
            {
                "networkElementId" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.Networkelementid.ToString(), Value = p.Networkelementid.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.Networkelementid.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.Networkelementid.ToString(), Value = p.Networkelementid.ToString() }).Distinct()
                    .ToList(),

                "opCo" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Opco, Value = p.Opco }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Opco.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Opco, Value = p.Opco }).Distinct().ToList(),

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

                "dataAcquisitionDate" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Dataacquisitiondate.ToString(), Value = p.Dataacquisitiondate.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Dataacquisitiondate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Dataacquisitiondate.ToString(), Value = p.Dataacquisitiondate.ToString() }).Distinct().ToList(),

                "nodeType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Nodetype, Value = p.Nodetype }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Nodetype.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Nodetype, Value = p.Nodetype }).Distinct().ToList(),

                "nodeTypeName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.NodeTypeName, Value = p.NodeTypeName }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.NodeTypeName.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.NodeTypeName, Value = p.NodeTypeName }).Distinct().ToList(),


                "platformType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Platformtype, Value = p.Platformtype }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Platformtype.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Platformtype, Value = p.Platformtype }).Distinct().ToList(),


                "siteLocation" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Sitelocation, Value = p.Sitelocation }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Sitelocation.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Sitelocation, Value = p.Sitelocation }).Distinct().ToList(),

                "softwareInstallDate" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Softwareinstalldate.ToString(), Value = p.Softwareinstalldate.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Softwareinstalldate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Softwareinstalldate.ToString(), Value = p.Softwareinstalldate.ToString() }).Distinct().ToList(),


                "softwareInstallDateAP" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.Softwareinstalldateap.ToString(), Value = p.Softwareinstalldateap.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.Softwareinstalldateap.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.Softwareinstalldateap.ToString(), Value = p.Softwareinstalldateap.ToString() }).Distinct()
                        .ToList(),

                "softwareInstallDateCP" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.Softwareinstalldatecp.ToString(), Value = p.Softwareinstalldatecp.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.Softwareinstalldatecp.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.Softwareinstalldatecp.ToString(), Value = p.Softwareinstalldatecp.ToString() }).Distinct()
                        .ToList(),

                "softwareProductDate" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Softwareproductdate.ToString(), Value = p.Softwareproductdate.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Softwareproductdate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Softwareproductdate.ToString(), Value = p.Softwareproductdate.ToString() }).Distinct().ToList(),

                "softwareProductDateAP" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Softwareproductdateap.ToString(), Value = p.Softwareproductdateap.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Softwareproductdateap.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Softwareproductdateap.ToString(), Value = p.Softwareproductdateap.ToString() }).Distinct().ToList(),

                "softwareProductDateCP" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Softwareproductdatecp.ToString(), Value = p.Softwareproductdatecp.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Softwareproductdatecp.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Softwareproductdatecp.ToString(), Value = p.Softwareproductdatecp.ToString() }).Distinct().ToList(),

                "softwareProductNumber" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Softwareproductnumber, Value = p.Softwareproductnumber }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Softwareproductnumber.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Softwareproductnumber, Value = p.Softwareproductnumber }).Distinct().ToList(),

                "softwareProductNumberAP" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Softwareproductnumberap, Value = p.Softwareproductnumberap }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Softwareproductnumberap.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Softwareproductnumberap, Value = p.Softwareproductnumberap }).Distinct().ToList(),

                "softwareProductNumberCP" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Softwareproductnumbercp, Value = p.Softwareproductnumbercp }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Softwareproductnumbercp.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Softwareproductnumbercp, Value = p.Softwareproductnumbercp }).Distinct().ToList(),

                "softwareReleaseInformation" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.Softwarereleaseinformation, Value = p.Softwarereleaseinformation }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.Softwarereleaseinformation.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.Softwarereleaseinformation, Value = p.Softwarereleaseinformation }).Distinct().ToList(),

                "softwareReleaseInformationAP" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Softwarereleaseinformationap, Value = p.Softwarereleaseinformationap }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Softwarereleaseinformationap.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Softwarereleaseinformationap, Value = p.Softwarereleaseinformationap }).Distinct().ToList(),


                "softwareReleaseInformationCP" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.Softwarereleaseinformationcp, Value = p.Softwarereleaseinformationcp }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.Softwarereleaseinformationcp.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.Softwarereleaseinformationcp, Value = p.Softwarereleaseinformationcp }).Distinct().ToList(),

                "spare1ossorenm" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Spare1ossorenm, Value = p.Spare1ossorenm }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Spare1ossorenm.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Spare1ossorenm, Value = p.Spare1ossorenm }).Distinct().ToList(),


                "spare2XmlVersion" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.Spare2xmlversion, Value = p.Spare2xmlversion }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.Spare2xmlversion.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.Spare2xmlversion, Value = p.Spare2xmlversion }).Distinct().ToList(),

                "xmlLastParseFileDate" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.Xmllastparsefiledate.ToString(), Value = p.Xmllastparsefiledate.ToString() }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.Xmllastparsefiledate.ToString().Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.Xmllastparsefiledate.ToString(), Value = p.Xmllastparsefiledate.ToString() }).Distinct().ToList(),


                //"creationUser" => string.IsNullOrEmpty(propertyFilter)
                //    ? query.Select(p => new FilterValueDto { Text = p.CreationUser.ToString(), Value = p.CreationUser.ToString() }).Distinct().ToList()
                //    : query
                //        .Where(x =>
                //            x.CreationUser.ToString().Contains(
                //                propertyFilter)).Select(p => new FilterValueDto { Text = p.CreationUser.ToString(), Value = p.CreationUser.ToString() }).Distinct().ToList(),
                "creationUser" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.CreationUserEntity.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.CreationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.CreationUserEntity.Email)).Distinct().ToList(),

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
