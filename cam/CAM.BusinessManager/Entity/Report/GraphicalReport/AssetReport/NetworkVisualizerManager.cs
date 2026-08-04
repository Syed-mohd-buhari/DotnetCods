using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.NetworkVisualizer;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers;
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

namespace CAM.BusinessManager.Entity.Report.GraphicalReport.AssetReport
{
    public class NetworkVisualizerManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private ICurrentUserService _currentUserService;
        private GridCustomColumnManager _manager;

        public NetworkVisualizerManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _currentUserService = currentUserService;
        }

        #region // UI Member
        public QueryResultDto<NetworkVisulaizerDtoGrid> FindWithCondition(IdentityAsIsDtoQuery dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);

            var rtn = new QueryResultDto<NetworkVisulaizerDtoGrid>(new GenerateRenderForGrid<NetworkVisulaizerDtoGrid>(_manager))
            {

            };
            var query = GetQuery(predicateResult, dto.Deleted ?? false).ApplyOrdering(dto, GetColumnsMap());

            query = query.ApplyPaging(dto);
            rtn.TotalItems = query.Count();

            var networkVirtual = CastObjectToDto(query, _repositoryWrapper);

            rtn.Items = networkVirtual.ToArray();
            return rtn;
        }

        public static ExpressionStarter<Identitiesasis> ApplyFilterForOracleModel(IdentityAsIsDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Identitiesasis>(true);
            var predicateInner = PredicateBuilder.New<Identitiesasis>(true);

            if (request.OpCo != null && request.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.OpCo)
                    predicateInner.Or(x => x.Asset.Opcoid.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (request.SupportedService != null && request.SupportedService.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.SupportedService)
                    predicateInner.Or(x => x.Asset.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr.Any(x => x.Service.Id == item));
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        public static Dictionary<string, Expression<Func<IdentityAsIs, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<IdentityAsIs, object>>[]>
            {
                ["id"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Id },
                ["value"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Value },
                ["categoryDescription"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Category != null ? p.Category.Description : default },
                ["classDescription"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Class != null ? p.Class.Description : default },
                ["typeDescription"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Type != null ? p.Type.Description : default },
                ["assetId"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.AssetId },
                ["resourceKey"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.ResourceKey },
                ["previousResourceKey"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.PreviousResourceKey },
                ["assetName"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Asset.ElementName },
                ["lastModifiedBy"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.ModificationUserEntity.Email },
                ["lastModifiedValue"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.ModificationDate },
                ["verticalId"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Asset.DesignComponent.SystemType.VerticalResponsibleId },
                ["verticalName"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Asset.DesignComponent.SystemType.VerticalResponsible.VerticalResponsibleDescription },
                ["opCoId"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Asset.OpCoId },
                ["opCo"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Asset.OpCo.OpCoDescription },
            };
        }
        private IQueryable<IdentityAsIs> GetQuery(ExpressionStarter<Identitiesasis> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.IdentityAsIsRepository.FindByCondition(predicateResult, includeDeleted)
                       .Include(x => x.Category)
                       .Include(x => x.Class)
                       .Include(x => x.Type)
                       .Include(x => x.Asset).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                       .Include(x => x.Asset).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetworksupportedsvr).ThenInclude(x => x.Service)
                       .Include(x => x.Asset).ThenInclude(x => x.Opco)
                       .Include(x => x.Asset).ThenInclude(x => x.Location)
               : _repositoryWrapper.IdentityAsIsRepository.FindAll()
                                      .Include(x => x.Category)
                       .Include(x => x.Class)
                       .Include(x => x.Type)
                       .Include(x => x.Asset).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                       .Include(x => x.Asset).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetworksupportedsvr).ThenInclude(x => x.Service)
                       .Include(x => x.Asset).ThenInclude(x => x.Opco)
                       .Include(x => x.Asset).ThenInclude(x => x.Location);


            return query.AsEnumerable().Select(x => IdentityAsIsMapper.Get(x)).AsQueryable();
        }

        public static List<NetworkVisulaizerDtoGrid> CastObjectToDto(IQueryable<IdentityAsIs> request, IRepositoryWrapper repositoryWrapper)
        {
            var oem = repositoryWrapper.OriginalEquipmentManufacturer.FindAll();
            var result = request.ToList().Select(dto =>
            {
                var grid = new NetworkVisulaizerDtoGrid();

                grid.OpCo = dto.Asset?.OpCo?.OpCoDescription;
                grid.ElementName = dto.Asset?.ElementName;
                grid.Product = dto.Asset?.DesignComponent?.SystemType?.MajorSoftwareBuilds?.ProductName?.Description;
                grid.Vendor = oem.Where(x => x.Orgeqpmanufacturerid == dto.Asset.OriginalEquipmentManufacturerId).Select(x => x.Originalequipmentmanufacturer).FirstOrDefault();
                grid.Location = dto.Asset?.Location?.LocationDescription;
                grid.VRFName = dto.Class?.Description;
                grid.Service = dto.Asset?.DesignComponent?.SubNetworkBoundary?.SupportedServices?.Select(x => x.SupportedService?.Description).FirstOrDefault();
                grid.IpAddress = dto.Value;

                return grid;

            });
            return result.ToList();
        }


        public NetworkVisulaizerDto GetResource(List<string> opcoList=null)
        {
            var resources = new NetworkVisulaizerDto();
            resources.OpCoResource = opcoList!=null && opcoList.Count > 0 ?
                                     _repositoryWrapper.OpCo.FindByCondition(x=>opcoList.Contains(x.Opcoid.ToString())).ToDictionary(x=>x.Opcoid,x=>x.Opco):
                                     _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, y => y.Opco);
            resources.SupportServiceResource = _repositoryWrapper.SupportedServiceRepository.FindAll().ToDictionary(x => x.Id, y => y.Description);

            return resources;
        }

        #endregion
    }
}
