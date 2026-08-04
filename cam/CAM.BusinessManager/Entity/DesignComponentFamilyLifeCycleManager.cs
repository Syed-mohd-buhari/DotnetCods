using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DesignComponentFamilyLifeCycle;
using CAM.DataTransferObjects.Entita.Identity;
using CAM.DataTransferObjects.Entita.ResourceKeyMaster;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Enum;
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
using static CAM.Enum.ResourceTypeEnum;


namespace CAM.BusinessManager.Entity
{
    public class DesignComponentFamilyLifeCycleManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        private ResourceKeyMasterManager _resourceKeyMasterManager;
        private readonly CommonManager _commonManager;
        #region Constructor
        public DesignComponentFamilyLifeCycleManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
            IHttpContextAccessor contextAccessor, ResourceKeyMasterManager resourceKeyMasterManager,
           IRepositoryWrapper repositoryWrapper, CommonManager commonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _resourceKeyMasterManager = resourceKeyMasterManager;
            _commonManager = commonManager;
        }
        #endregion

        #region UIMemberFunctions

        public QueryResultDto<DesignComponentFamilyLifeCycleDtoGrid> FindWithCondition(DesignComponentFamilyLifeCycleQueryDto DesignComponentFamilyLifeCycleFilterDto)
        {
            var predicateResult = ApplyFilter(DesignComponentFamilyLifeCycleFilterDto);
            if (DesignComponentFamilyLifeCycleFilterDto.Deleted == ConstantValueFilter.isTrue)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            var rtn = new QueryResultDto<DesignComponentFamilyLifeCycleDtoGrid>(new GenerateRenderForGrid<DesignComponentFamilyLifeCycleDtoGrid>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Count(predicateResult) : _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Count(),
            };
            var query = GetQuery(predicateResult, DesignComponentFamilyLifeCycleFilterDto.Deleted ?? !ConstantValueFilter.isTrue).ApplyOrdering(DesignComponentFamilyLifeCycleFilterDto, GetColumnsMap()).ApplyPaging(DesignComponentFamilyLifeCycleFilterDto);
            var data = query.ToList();

            IEnumerable<DesignComponentFamilyLifeCycleDtoGrid> DesignComponentFamilyLifeCycleResult;

            DesignComponentFamilyLifeCycleResult = _mapper.Map<IEnumerable<DesignComponentFamilyLifeCycleDtoGrid>>(data);
            rtn.Items = DesignComponentFamilyLifeCycleResult.ToArray();
            return rtn;
        }
        private static ExpressionStarter<Dcflifecycle> ApplyFilter(DesignComponentFamilyLifeCycleQueryDto NetworkQueryFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Dcflifecycle>();
            var predicateInner = PredicateBuilder.New<Dcflifecycle>();

            if (NetworkQueryFilterDto.DcfLifeCycleId != null && NetworkQueryFilterDto.DcfLifeCycleId.Any())
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.DcfLifeCycleId)
                    predicateInner.Or(x => x.Dcflifecycleid == item);
                predicateResult.And(predicateInner);
            }
             
            if ( 
                (NetworkQueryFilterDto.DesignComponentFamilyName != null && NetworkQueryFilterDto.DesignComponentFamilyName.Any()))
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.DesignComponentFamilyName)
                    predicateInner.Or(x => x.Dcfid == item);
                predicateResult.And(predicateInner);
            }
            if ((NetworkQueryFilterDto.OpcoId != null && NetworkQueryFilterDto.OpcoId.Any())  
                 )
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.OpcoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.CurrentDetail != null && NetworkQueryFilterDto.CurrentDetail.Any())
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.CurrentDetail)
                    predicateInner.Or(x => x.Currentdetails == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.EventName != null && NetworkQueryFilterDto.EventName.Any())
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.EventName)
                    predicateInner.Or(x => x.EventName == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.EventId != null && NetworkQueryFilterDto.EventId.Any())
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.EventId)
                    predicateInner.Or(x => x.EventId == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.ResourceKey != null && NetworkQueryFilterDto.ResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.ResourceKey)
                    predicateInner.Or(x => x.Resourcekey == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.PreviousResourceKey != null && NetworkQueryFilterDto.PreviousResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.PreviousResourceKey)
                    predicateInner.Or(x => x.Previousresourcekey == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.PreviousResourceKey != null && NetworkQueryFilterDto.PreviousResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.PreviousResourceKey)
                    predicateInner.Or(x => x.Previousresourcekey == item);
                predicateResult.And(predicateInner);
            }

            if ( 
                (NetworkQueryFilterDto.DesignComponent != null && NetworkQueryFilterDto.DesignComponent.Any())|| 
                NetworkQueryFilterDto.DcId != null && NetworkQueryFilterDto.DcId.Any())
            {

                var dcId = NetworkQueryFilterDto.DcId?.ToList();
                if (NetworkQueryFilterDto.DesignComponent != null && NetworkQueryFilterDto.DesignComponent.Any())
                {
                    if (dcId != null && dcId.Count > 0) dcId.AddRange(NetworkQueryFilterDto.DesignComponent.ToList());
                    else dcId = NetworkQueryFilterDto.DesignComponent.ToList();
                    dcId = dcId.ToList();
                }

                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in dcId)
                    predicateInner.Or(x => x.Dcid == item);
                predicateResult.And(predicateInner);
            }
            if ( (NetworkQueryFilterDto.DesignComponentFamilyName != null && NetworkQueryFilterDto.DesignComponentFamilyName.Any())
                ||(NetworkQueryFilterDto.DcfId != null && NetworkQueryFilterDto.DcfId.Any()))
            {

                var dcfId = NetworkQueryFilterDto.DcfId?.ToList();
                if (NetworkQueryFilterDto.DesignComponentFamilyName != null && NetworkQueryFilterDto.DesignComponentFamilyName.Any())
                {
                    if (dcfId != null && dcfId.Count > 0) dcfId.AddRange(NetworkQueryFilterDto.DesignComponentFamilyName.ToList());
                    else dcfId = NetworkQueryFilterDto.DesignComponentFamilyName.ToList();
                    dcfId = dcfId.ToList();
                }
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in dcfId)
                    predicateInner.Or(x => x.Dcfid == item);
                predicateResult.And(predicateInner);
            }
            

            if ((NetworkQueryFilterDto.OpCo != null && NetworkQueryFilterDto.OpCo.Any()) 
                || (NetworkQueryFilterDto.OpcoId != null && NetworkQueryFilterDto.OpcoId.Any()))
            {
                var opCoid = NetworkQueryFilterDto.OpcoId?.ToList();
                if (NetworkQueryFilterDto.OpCo != null && NetworkQueryFilterDto.OpCo.Any())
                {
                   if(opCoid != null && opCoid.Count >0) opCoid.AddRange(NetworkQueryFilterDto.OpCo.ToList());
                   else opCoid = NetworkQueryFilterDto.OpCo.ToList() ;
                    opCoid = opCoid.ToList();
                }

                      predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in opCoid)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (NetworkQueryFilterDto.CreationUser != null && NetworkQueryFilterDto.CreationUser.Any())
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.CreationUser)
                    predicateInner.Or(x => x.CreationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.ModificationUser != null && NetworkQueryFilterDto.ModificationUser.Any())
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.ModificationUser)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (NetworkQueryFilterDto.CreationDate != null)
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                if (NetworkQueryFilterDto.CreationDate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= NetworkQueryFilterDto.CreationDate.StartDate);
                if (NetworkQueryFilterDto.CreationDate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= NetworkQueryFilterDto.CreationDate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (NetworkQueryFilterDto.ModificationDate != null)
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                if (NetworkQueryFilterDto.ModificationDate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= NetworkQueryFilterDto.ModificationDate.StartDate);
                if (NetworkQueryFilterDto.ModificationDate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= NetworkQueryFilterDto.ModificationDate.EndDate);
                predicateResult.And(predicateInner);
            }
 
 
            if (NetworkQueryFilterDto.CategoryType != null && NetworkQueryFilterDto.CategoryType.Any())
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.CategoryType)
                    predicateInner.Or(x => x.Categorytype.ToString()  == item);
                predicateResult.And(predicateInner);
            }

            if (NetworkQueryFilterDto.PlannedDetails != null && NetworkQueryFilterDto.PlannedDetails.Any())
            {
                predicateInner = PredicateBuilder.New<Dcflifecycle>();
                foreach (var item in NetworkQueryFilterDto.PlannedDetails)
                    predicateInner.Or(x => x.Planneddetails == item );
                predicateResult.And(predicateInner);
            }
 

            return predicateResult;
        }

        private IQueryable<DesignComponentFamilyLifeCycle> GetQuery(ExpressionStarter<Dcflifecycle> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(predicateResult, includeDeleted)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
                //.Include(x => x.Opco)
                //.Include(x => x.Dc)
                //.Include(x => x.Dcf)
                : _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
                //.Include(x => x.Opco)
                //.Include(x => x.Dc)
                //.Include(x => x.Dcf)
                ;
             
            return query.AsEnumerable().Select(x => DesignComponentFamilyLifeCycleMapper.Get(x)).AsQueryable();
        }

        private Dictionary<string, Expression<Func<DesignComponentFamilyLifeCycle, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<DesignComponentFamilyLifeCycle, object>>[]>
            {
                ["dcfLifeCycleId"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.DcfLifeCycleId },
                ["dcfId"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.DcfId },
                ["resourceKey"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.ResourceKey },
                ["previousResourceKey"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.PreviousResourceKey },
                ["opCoId"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.OpCoId },
                ["dcId"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.DcId },
                ["eventName"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.EventName },
                ["currentdDetails"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.CurrentDetails },
                ["eventId"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.EventId },
                ["notes"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.Notes },
                ["modificationDate"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.ModificationDate },
                ["modificationUser"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationDate"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.CreationDate },
                ["creationUser"] = new Expression<Func<DesignComponentFamilyLifeCycle, object>>[] { p => p.CreationUserEntity.Email },
            };
        }
        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, DesignComponentFamilyLifeCycleQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "dcfLifeCycleId" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.DcfLifeCycleId.ToString(), Value = p.DcfLifeCycleId.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.DcfLifeCycleId.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.DcfLifeCycleId.ToString(), Value = p.DcfLifeCycleId.ToString() }).Distinct()
                    .ToList(),

                "opCoId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.OpCoId.ToString(), Value = p.OpCoId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.OpCoId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.OpCoId.ToString(), Value = p.OpCoId.ToString() }).Distinct().ToList(),

                "opCo" =>  
                      query 
                           .Select(p => new FilterValueDto { Text = p.OpcoDescription, Value = p.OpCoId.ToString() })
                           .Distinct()
                           .ToList() ,
                "designComponentFamilyName" => string.IsNullOrEmpty(propertyFilter)
             ? query.ToList().Select(p => new FilterValueDto
             {
                 Text = p.DcfDescription,
                 Value = p.DcfId.ToString()
             }).Distinct().ToList()
            : query.ToList()
                .Where(x =>
                    x.DcfDescription.ToUpper().Contains(
                        propertyFilter.ToUpper())).Select(p => new FilterValueDto
                        {
                            Text = p.DcfDescription,
                            Value = p.DcfId.ToString()
                        }).Distinct().ToList(),

                "resourceTypesId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ResourceTypesId.ToString(), Value = p.ResourceTypesId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ResourceTypesId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ResourceTypesId.ToString(), Value = p.ResourceTypesId.ToString() }).Distinct().ToList(),

                "resourceKey" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Where(x => x.ResourceKey != null)
                        .Select(p => new FilterValueDto { Text = p.ResourceKey, Value = p.ResourceKey })
                        .Distinct()
                        .ToList()
                    : query
                        .Where(x => x.ResourceKey != null && x.ResourceKey.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.ResourceKey, Value = p.ResourceKey })
                        .Distinct()
                        .ToList(),

                "eventId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.EventId.ToString(), Value = p.EventId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.EventId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.EventId.ToString(), Value = p.EventId.ToString() }).Distinct().ToList(),

                "eventName" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                    .Where(x => x.EventName != null)
                    .Select(p => new FilterValueDto { Text = p.EventName.ToString(), Value = p.EventName.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.EventName.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.EventName.ToString(), Value = p.EventName.ToString() }).Distinct().ToList(),

                "currentDetail" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Where(x => x.CurrentDetails != null)
                       .Select(p => new FilterValueDto (((int)ResourceTypesKey.Component == p.CategoryType)
                        ? p.BagName + " " + p.CurrentDetails : p.CurrentDetails))
                        .Distinct()
                        .ToList()
                    : query
                        .Where(x => x.CurrentDetails != null && x.CurrentDetails.ToString().Contains(propertyFilter))
                        .Select(p => new FilterValueDto(((int)ResourceTypesKey.Component == p.CategoryType)
                        ? p.BagName + " " + p.CurrentDetails : p.CurrentDetails))
                        .Distinct()
                        .ToList(),

                "previousResourceKey" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Where(x => x.PreviousResourceKey != null)
                        .Select(p => new FilterValueDto { Text = p.PreviousResourceKey, Value = p.PreviousResourceKey })
                        .Distinct()
                        .ToList()
                    : query
                        .Where(x => x.PreviousResourceKey != null && x.PreviousResourceKey.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.PreviousResourceKey, Value = p.PreviousResourceKey })
                        .Distinct()
                        .ToList(),

                "dcfId" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Where(x => x.DcfId.ToString() != null)
                        .Select(p => new FilterValueDto { Text = p.DcfId.ToString(), Value = p.DcfId.ToString() })
                        .Distinct()
                        .ToList()
                    : query
                        .Where(x => x.DcfId.ToString() != null && x.DcfId.ToString().Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.DcfId.ToString(), Value = p.DcfId.ToString() })
                        .Distinct()
                        .ToList(),

                "dcId" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Where(x => x.DcId.ToString() != null)
                        .Select(p => new FilterValueDto { Text = p.DcId.ToString(), Value = p.DcId.ToString() })
                        .Distinct()
                        .ToList()
                    : query
                        .Where(x => x.DcId.ToString() != null && x.DcId.ToString().Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.DcId.ToString(), Value = p.DcId.ToString() })
                        .Distinct()
                        .ToList(),

                "creationUser" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.CreationUserEntity.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.CreationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.CreationUserEntity.Email)).Distinct().ToList(),

                "creationDate" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.CreationDate.ToString(), Value = p.CreationDate.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.CreationDate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.CreationDate.ToString(), Value = p.CreationDate.ToString() }).Distinct().ToList(),


                "modificationUser" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList(),


                "modificationDate" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationDate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList(),
 
                "opcoDescription" =>   query    
      .Select(p => new FilterValueDto
      {
          Text = p.OpcoDescription,
          Value = p.OpCoId.ToString()
      })
       .DistinctBy(x => x.Text)
      .ToList() ,                      
                "dcfDescription" =>   query
               .Select(p => new FilterValueDto
               {
                   Text = p.DcfDescription,
                   Value = p.DcfId.ToString()
               })
               .DistinctBy(x => x.Text)
               .ToList() ,
                "plannedDetails" =>   query
             .Where(x => x.PlannedDetails != null)
             .Select(p => new FilterValueDto(p.PlannedDetails))
              .DistinctBy(x => x.Text)
             .ToList() ,
                "bagName" => string.IsNullOrEmpty(propertyFilter)
          ? query
              .Where(x => x.BagName != null)
              .Select(p => new FilterValueDto(p.BagName))
               .DistinctBy(x => x.Text)
              .ToList()
          : query
              .Where(x => x.BagName != null && x.BagName.Contains(propertyFilter))
               .Select(p => new FilterValueDto(p.BagName))
               .DistinctBy(x => x.Text)
              .ToList(),
               "categoryType" => string.IsNullOrEmpty(propertyFilter)
           ? query
               .Where(x => x.CategoryType != null)
              .Select(p => new FilterValueDto { Text = getCategoryType((int)p.CategoryType), Value = p.CategoryType.ToString() }).Distinct().ToList()
               .DistinctBy(y => y.Text)
               .ToList()
           : query
               .Where(x => x.CategoryType != null && x.CategoryType.ToString().Contains(propertyFilter))
              .Select(p => new FilterValueDto { Text = getCategoryType((int)p.CategoryType), Value = p.CategoryType.ToString() }).Distinct().ToList()
                .DistinctBy(y => y.Text)
               .ToList(),
                //"designComponent" => string.IsNullOrEmpty(propertyFilter)
                //            ? query.ToList().Select(p => new FilterValueDto
                //            {
                //                Text = DesignComponentMapper.SetDesignComponentMapper(p.DesignComponents).ToDesignComponentName(_repositoryWrapper),
                //                Value = p.DcId.ToString()
                //            }).Distinct().ToList()
                //            : query.ToList()
                //                .Where(x =>
                //                         DesignComponentMapper.SetDesignComponentMapper(x.DesignComponents).ToDesignComponentName(_repositoryWrapper).ToUpper().Contains(
                //                        propertyFilter.ToUpper())).Select(p => new FilterValueDto
                //                        {
                //                            Text = DesignComponentMapper.SetDesignComponentMapper(p.DesignComponents).ToDesignComponentName(_repositoryWrapper),
                //                            Value = p.DcId.ToString()
                //                        }).Distinct().ToList(),
                "designComponentName" => query
               .Select(p => new FilterValueDto
               {
                   Text = p.DcDescription,
                   Value = p.DcId.ToString()
               })
               .DistinctBy(y => y.Text)
               .ToList(),

                _ => new List<FilterValueDto>()
            };

            return rtn;
        }
        #endregion

        public string getCategoryType(int categoryType)
        {
            if ((int)ResourceTypesKey.Component == categoryType) return "Component";
            else if ((int)ResourceTypesKey.SWAsset == categoryType) return "Asset";
            else if ((int)ResourceTypesKey.HWAsset == categoryType) return "Asset";
            else if ((int)ResourceTypesKey.Identity == categoryType) return "Identity";
            else if ((int)ResourceTypesKey.Lcm == categoryType) return "LCM";

            return "";

        }
        #region LifeCycleIdOperations
        public string IncrementLifeCycle(string resourceKey )
        {
            int? lifecycleId = _repositoryWrapper.ResourceKeyMaster.FindByCondition(x => x.Resourcekey == resourceKey).Select(x => x.Lifecycleid).FirstOrDefault();

            int incrementedLifeCycleID = 0;

            if (lifecycleId != null)
            {
                incrementedLifeCycleID = (int)lifecycleId;//  + 1;
                return setPrefixZeroToLifeCycleId(incrementedLifeCycleID); }
            else
            {
                return setPrefixZeroToLifeCycleId(incrementedLifeCycleID);
            }
        }
        #endregion

        #region
        public string setPrefixZeroToLifeCycleId(int lifeCycleId)
        {
            if (lifeCycleId < 99)
            {
                return lifeCycleId.ToString("D2");
            }
            else if (lifeCycleId < 999)
            {
                return lifeCycleId.ToString("D3");
            }
            else
                return lifeCycleId.ToString("D4");

        }
        #endregion
        #region LCMOperations

        /// <summary>
        /// This function will initialize the DCF Lifecycle with Start of Lifecycle Event whenever a new LCM is created freshly.
        /// Also this will create a new resourcekey and return it to calling method for association with LCM Object.
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <param name="DesignComponentId"></param>
        /// <param name="OpCoId"></param>
        /// <returns></returns>
        public async Task<string> InitialiseDCFLifecycleforLCM(string resourceKey, long DesignComponentId, short? OpCoId, long buildBagId)
        {
            string lcmResourceKey = string.Empty;
            string newlcmResourceKey = string.Empty;
            string plannedActivityResource = string.Empty;

            try
            {
                
                var opCoName = GetOpcoDescription((short) OpCoId).Result;
                var dcEntity = getDesignComponent((short)DesignComponentId);

                var dcName = dcEntity.ToDesignComponentName(_repositoryWrapper);
                var dcfamilyName = dcEntity.toDesignComponentFamily(_repositoryWrapper);
                var plannedDcName = string.Empty;// GetPlannedDcEntitity( (long) OpCoId, DesignComponentId);
                var bagName = getBagName(buildBagId);

                if (dcEntity != null)
                {
                    if (resourceKey != null)
                    {
                        var LcmResourcKey = resourceKey.Split("_");
                        if (LcmResourcKey.Length > 1)
                        {
                            newlcmResourceKey = resourceKey;
                        }
                    }
                    else
                    {
                        ResourceKeyMasterDto dto = new ResourceKeyMasterDto();
                        dto.DcfId = dcEntity.Designcomponentfamilyid;
                        dto.OpCoId = (short)OpCoId;
                        dto.ResourceTypesId = (int)ResourceTypesKey.Lcm;
                        lcmResourceKey = _resourceKeyMasterManager.GenerateResourceKeyForLcm((short)OpCoId, (long)dcEntity.Designcomponentfamilyid, buildBagId, (int)ResourceTypesKey.Lcm);
                        string lifeCycleId = IncrementLifeCycle(lcmResourceKey);
                        dto.ResourceKey = lcmResourceKey;
                        dto.LifeCycleId = int.Parse(lifeCycleId) + 1;
                        dto.BuildBagId  = buildBagId;
                        await _resourceKeyMasterManager.CreateOrUpdateResourceKey(dto);
                        newlcmResourceKey = lcmResourceKey.ToString() + "_" + lifeCycleId;
                        

                        string existsDcfResourceKey = _repositoryWrapper
                            .DesignComponentFamilyLifeCycleRepository.
                            FindByCondition(x => x.Opcoid == OpCoId && x.Resourcekey == (lcmResourceKey + ConstantValueFilter.resourceKeySuffixZero)).
                            Select(x => x.Resourcekey.ToString()).FirstOrDefault();
                        if (string.IsNullOrEmpty(existsDcfResourceKey))
                        {
                            Dcflifecycle newDCFrecord = new Dcflifecycle();
                            newDCFrecord.Dcfid = (long)dcEntity.Designcomponentfamilyid;
                            newDCFrecord.Opcoid = OpCoId;
                            newDCFrecord.Resourcekey = newlcmResourceKey;
                            newDCFrecord.Dcid = DesignComponentId;
                            newDCFrecord.EventId = 0;                            
                            newDCFrecord.EventName = ConstantValueFilter.startOfLifeCycle;

                            newDCFrecord.Currentdetails = dcName;
                            newDCFrecord.Opcodescription = opCoName;
                            newDCFrecord.Dcfdescription = dcfamilyName;
                            newDCFrecord.Planneddetails = plannedDcName;
                            newDCFrecord.Dcdescription = dcName;
                            newDCFrecord.Categorytype = (int)ResourceTypesKey.Lcm;
                            newDCFrecord.Bagname = bagName;
                            _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(newDCFrecord);
                            _repositoryWrapper.Save();
                        }

                        _repositoryWrapper.Save();
                        newlcmResourceKey = lcmResourceKey.ToString() + "_" + setPrefixZeroToLifeCycleId(
                            int.Parse(lifeCycleId) + 1);

                        await InitialiseDCFLifecycleforComponent(  (long)OpCoId, DesignComponentId, buildBagId);
                    }


                }
                //Incrementing the lcm resourcekey after DCF lifecycle is initiated. Once initiated any activity on top of it should have a new life cycle id.
                return newlcmResourceKey;//= IncrementResourceKeyLifeCycleId(newlcmResourceKey);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This function will be invoked specifically for New NFxI or New System when the nodes get moved from planned to In-Service
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="PlannedActivityResourceId"></param>
        /// <returns></returns>
        public async Task<ResultDto> CreateDCFLifecycleforNewSolution(Lcmengineering lcmEntity, Plannedactivities plannedActivity, bool? checkDecommissionFlow = false)
        {
            string newlcmresourceKey = string.Empty;
            string _designComponentName = string.Empty;
            string _plannedActivity = string.Empty;
            string lcmResourceKey = string.Empty;
            string _designComponentFamilyName = string.Empty;
            ResourceKeyMasterDto dto = new ResourceKeyMasterDto();

            try
            {
                if (lcmEntity != null)
                {
 
                    var opCoName = GetOpcoDescription((short)lcmEntity.Opcoid);
                    var _designComponent = getDesignComponent((short)lcmEntity.Designcomponentid);
                    var plannedDcName = GetPlannedDcEntitity((long)lcmEntity.Opcoid, lcmEntity.Designcomponentid);
                    var bagName = getBagName(lcmEntity.Buildbagid);
                    if (_designComponent != null)
                    {
                        _designComponentName = _designComponent.ToDesignComponentName(_repositoryWrapper);
                        _designComponentFamilyName = _designComponent.toDesignComponentFamily(_repositoryWrapper);
                    }

                    if (plannedActivity != null)
                    {

                        if (plannedActivity.Plannedactivityresource != null)
                        {
                            _plannedActivity = plannedActivity.Plannedactivityresource.Plannedactivityresource.ToString();
                        }
                        else
                        {
                            _plannedActivity = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(
                                x => x.Plannedactivityresourceid == plannedActivity.Plannedactivityresourceid).Select(x => x.Plannedactivityresource).FirstOrDefault().ToString();
                        }

                        string compareBagname = bagName?.ToLower() ?? string.Empty;

                        var _DCFEntryExists = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(
                            x => x.Dcfid == _designComponent.Designcomponentfamilyid && x.Opcoid == lcmEntity.Opcoid
                            && x.EventId == plannedActivity.Plannedactivityresourceid
                            && x.Dcid == lcmEntity.Designcomponentid
                            && x.Bagname.ToLower() == compareBagname
                            // && x.Buildbagid == lcmEntity.Buildbagid
                            // && x.Resourcekey == lcmEntity.Resourcekey
                            ).FirstOrDefault();

                        if (_DCFEntryExists == null)
                        {


                            Dcflifecycle newDCFrecord = new Dcflifecycle();
                            newDCFrecord.Dcfid = (long)_designComponent.Designcomponentfamilyid;
                            newDCFrecord.Opcoid = lcmEntity.Opcoid;
                            newDCFrecord.EventId = plannedActivity.Plannedactivityresourceid;
                            newDCFrecord.Resourcekey = lcmEntity.Resourcekey;
                            newDCFrecord.Previousresourcekey = lcmEntity.Previousresourcekey;
                            newDCFrecord.Dcid = lcmEntity.Designcomponentid;
                            newDCFrecord.Currentdetails = _designComponentName;
                            newDCFrecord.EventName = _plannedActivity;
                            newDCFrecord.Opcodescription = opCoName.Result;
                            newDCFrecord.Dcfdescription = _designComponentFamilyName;                           
                            newDCFrecord.Categorytype = (int)ResourceTypesKey.Lcm;
                            newDCFrecord.Planneddetails = plannedDcName;
                            newDCFrecord.Bagname = bagName;

                            _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(newDCFrecord);
                            await _repositoryWrapper.SaveAsync();


                        }
                    }
                    //Introduce a logic to check SettingsPlannedActivity to govern the Resourcekey
                    if (lcmEntity.Resourcekey != null && checkDecommissionFlow == false)
                    {

                        string[] splitUpResourceKey = lcmEntity.Resourcekey.Split("_");

                        int lifeCycleId = Convert.ToInt16(IncrementLifeCycle(splitUpResourceKey[0]));

                        dto.OpCoId = (short)lcmEntity.Opcoid;
                        dto.DcfId = _designComponent.Designcomponentfamilyid;
                        dto.ResourceKey = splitUpResourceKey[0];
                        dto.ResourceTypesId = (int)ResourceTypesKey.Lcm;
                        dto.LifeCycleId = lifeCycleId + 1;
                        dto.BuildBagId = lcmEntity.Buildbagid;
                        dto.BagName = bagName;
                        await _resourceKeyMasterManager.CreateOrUpdateResourceKey(dto);

                        newlcmresourceKey = splitUpResourceKey[0] + "_" +
                            setPrefixZeroToLifeCycleId(lifeCycleId + 1);
                    }

                    await GenerateDCFEntryForComponenet((long)lcmEntity.Opcoid, lcmEntity.Designcomponentid, (long)_designComponent.Designcomponentfamilyid, lcmEntity.Buildbagid, 1,
      ConstantValueFilter.componentDcfEventDeatil.FirstOrDefault(x => x.Key == 2).Text);

                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess,
                    Data = newlcmresourceKey
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This function will be invoked to increment the LCM resource key whenever a transition happens and updates DCF lifecycle table.
        /// </summary>
        /// <param name="oldEntity"></param>
        /// <param name="newEntity"></param>
        /// <param name="PlannedActivityResourceId"></param>
        /// <returns></returns>
        public async Task<ResultDto> CreateDCFLifecycleforLCMTransition(Lcmengineering oldlcmEntity, Lcmengineering newlcmEntity, Plannedactivities plannedActivity)
        {
            string lcmResourceKey = string.Empty;
            string newlcmresourceKey = string.Empty;
            string oldDcName = string.Empty ,oldDCFName = string.Empty;
           // string newDcName = string.Empty, newDCFName = string.Empty;
            Plannedactivityresources _plannedActivityResources = null;

            try
            {

                var oldDc = getDesignComponent((short)oldlcmEntity.Designcomponentid);
                           
                var newDc = getDesignComponent((short)newlcmEntity.Designcomponentid);

                var plannedDcName = GetPlannedDcEntitity((long)oldlcmEntity.Opcoid, oldlcmEntity.Designcomponentid);
                var oldOpCoName = GetOpcoDescription((short)oldlcmEntity.Opcoid).Result;
                var oldbagName = getBagName(oldlcmEntity.Buildbagid);
                if (plannedActivity != null)
                {
                    _plannedActivityResources = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(
                                x => x.Plannedactivityresourceid == plannedActivity.Plannedactivityresourceid).FirstOrDefault();
                }

                if (oldDc != null)
                {
                    oldDcName = oldDc.ToDesignComponentName(_repositoryWrapper);
                    oldDCFName = oldDc.toDesignComponentFamily(_repositoryWrapper);
                }
                string compareBagname = oldbagName?.ToLower() ?? string.Empty;

                if (oldDc != null && oldlcmEntity != null)
                {
                    ResourceKeyMasterDto dto = new ResourceKeyMasterDto();
                    if (_plannedActivityResources != null)
                    {
                       
                        var _dcfEntryExists = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository
                            .FindByCondition(
                            x => x.Dcfid == oldDc.Designcomponentfamilyid && x.Opcoid == oldlcmEntity.Opcoid
                            && x.EventId == plannedActivity.Plannedactivityresourceid
                            && x.Dcid == oldlcmEntity.Designcomponentid
                             && x.Bagname.ToLower() == compareBagname 
                            // && x.Buildbagid == oldlcmEntity.Buildbagid
                            //&& x.Resourcekey == oldlcmEntity.Resourcekey
                            ).FirstOrDefault();

                        if (_dcfEntryExists == null)
                        {
                            await GenerateDCFEntryForLCM(oldlcmEntity, oldDcName, _plannedActivityResources.Plannedactivityresource.ToString(),
                                plannedActivity.Plannedactivityresourceid, (long)oldDc.Designcomponentfamilyid, oldOpCoName,oldDCFName, plannedDcName);
                        }
                    }
                    if (oldDc.Designcomponentfamilyid != null && newDc.Designcomponentfamilyid != null)
                    {
                        if (oldDc.Designcomponentfamilyid == newDc.Designcomponentfamilyid)
                        {
                            if (oldlcmEntity.Resourcekey != null)
                            {

                                string[] splitUpResourceKey = oldlcmEntity.Resourcekey.Split("_");

                                int lifeCycleId = Convert.ToInt16(IncrementLifeCycle(splitUpResourceKey[0] ));


                                newlcmresourceKey = splitUpResourceKey[0] + "_" + setPrefixZeroToLifeCycleId(
                                     lifeCycleId + 1);
                                dto.ResourceKey = splitUpResourceKey[0];
                                dto.OpCoId = (short)oldlcmEntity.Opcoid;
                                dto.DcfId = oldDc.Designcomponentfamilyid;
                                dto.ResourceTypesId = (int)ResourceTypesKey.Lcm;
                                dto.LifeCycleId = lifeCycleId + 1;
                                dto.BuildBagId = oldlcmEntity.Buildbagid;
                                await _resourceKeyMasterManager.CreateOrUpdateResourceKey(dto);


                                await CreateDCFLifecycleforComponentBasedOnLCMTransition(oldlcmEntity);
                            }
                        }
                        else
                        {
                            if (newlcmEntity != null)
                            {
                                //Check for Refactor Planned Activity Resource Types.
                                if (_plannedActivityResources.Rulelinkeddc == (int)PlannedActivityResourceEnum.Refactor)
                                {
                                    newlcmresourceKey = await InitialiseDCFLifecycleforLCM(null, newlcmEntity.Designcomponentid, newlcmEntity.Opcoid, (long)newlcmEntity.Buildbagid);

                                }
                                else
                                {
                                    newlcmresourceKey = await InitialiseDCFLifecycleforLCM(null, newlcmEntity.Designcomponentid, newlcmEntity.Opcoid, (long)newlcmEntity.Buildbagid);
                                }
                            }
                        }
                    }
                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess,
                    Data = newlcmresourceKey
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This function will be invoked when a delete LCM Activity is triggered.
        /// </summary>
        /// <param name="oldlcmEntity"></param>
        /// <returns></returns>
        public async Task<ResultDto> CreateDCFLifecycleforLCMDeletion(Lcmengineering lcmEntity)
        {
            string oldDcName = string.Empty, dcfamilyName = string.Empty;
            string activityDescription = ConstantValueFilter.lcmDeletedManually;

            try
            {              

                var opCoName = GetOpcoDescription((short)lcmEntity.Opcoid).Result;
                var oldDc = getDesignComponent((short)lcmEntity.Designcomponentid);

                var dcName = oldDc.ToDesignComponentName(_repositoryWrapper);
                var plannedDcName = GetPlannedDcEntitity((long)lcmEntity.Opcoid, lcmEntity.Designcomponentid);

                if (oldDc != null)
                {
                    oldDcName = oldDc.ToDesignComponentName(_repositoryWrapper);
                    dcfamilyName = oldDc.toDesignComponentFamily(_repositoryWrapper);
                }

                if (oldDc != null && lcmEntity != null)
                {
                    await GenerateDCFEntryForLCM(lcmEntity, oldDcName, activityDescription, 0, (long)oldDc.Designcomponentfamilyid, opCoName,dcfamilyName, plannedDcName);
                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess,
                    Data = lcmEntity.Resourcekey
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultDto> CreateDCFLifecycleforLCMCreated(Lcmengineering lcmEntity)
        {
            string oldDcName = string.Empty, dcfamilyName = string.Empty;
            string activityDescription = ConstantValueFilter.lcmCreated;

            try
            {

                var opCoName = GetOpcoDescription((short)lcmEntity.Opcoid).Result;
                var oldDc = getDesignComponent((short)lcmEntity.Designcomponentid);

                var plannedDcName = GetPlannedDcEntitity((long)lcmEntity.Opcoid, lcmEntity.Designcomponentid);
                if (oldDc != null)
                {
                    oldDcName = oldDc.ToDesignComponentName(_repositoryWrapper);
                    dcfamilyName = oldDc.toDesignComponentFamily(_repositoryWrapper);
                }

                if (oldDc != null && lcmEntity != null)
                {
                    await GenerateDCFEntryForLCM(lcmEntity, oldDcName, activityDescription, 0, (long)oldDc.Designcomponentfamilyid, opCoName, dcfamilyName, plannedDcName);
                    
                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess,
                    Data = lcmEntity.Resourcekey
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResultDto> GenerateDCFEntryForLCM(Lcmengineering lcmEntity, string DCName, string EventName, short? EventId, long DCFId,string opCoName,string dcfName,string plannedDcName)
        {
            try
            {

                var bagName = getBagName(lcmEntity.Buildbagid);

                Dcflifecycle newDCFRecord = new Dcflifecycle();
                newDCFRecord.Dcfid = DCFId;
                newDCFRecord.Opcoid = lcmEntity.Opcoid;
                newDCFRecord.EventId = EventId;
                newDCFRecord.Resourcekey = lcmEntity.Resourcekey;
                newDCFRecord.Previousresourcekey = lcmEntity.Previousresourcekey;
                newDCFRecord.Dcid = lcmEntity.Designcomponentid;
                newDCFRecord.Currentdetails = DCName;
                newDCFRecord.EventName = EventName;
                newDCFRecord.Bagname = bagName;

                newDCFRecord.Planneddetails = plannedDcName;
                newDCFRecord.Opcodescription = opCoName;
                newDCFRecord.Dcfdescription = dcfName;
                newDCFRecord.Dcdescription = DCName;

                newDCFRecord.Categorytype = (int)ResourceTypesKey.Lcm;

                _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(newDCFRecord); // Add a method to create a new entity in your repository
                await _repositoryWrapper.SaveAsync();

                string componenetActivityDescription = ConstantValueFilter.componentDcfEventDeatil.FirstOrDefault(x => x.Key == 2).Text;
                bool isComponentDeleted = false;
                if (EventName == ConstantValueFilter.lcmDeletedManually)
                {
                    isComponentDeleted = true;
                    componenetActivityDescription = ConstantValueFilter.componentDcfEventDeatil.FirstOrDefault(x => x.Key == 3).Text;
                }
                 

                await GenerateDCFEntryForComponenet((long)lcmEntity.Opcoid, lcmEntity.Designcomponentid, DCFId, (long)lcmEntity.Buildbagid, (long)EventId,
                  componenetActivityDescription, isComponentDeleted);
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess,
                    Data = lcmEntity.Resourcekey
                };
            }
            catch
            {
                throw;
            }
        }
        public async Task<ResultDto> CreateDCFLifecycleForIsReleaseDetailsUnknown(Lcmengineering lcmEntity)
        {
            string oldDcName = string.Empty, oldDcfName = string.Empty;
            string activityDescription = ConstantValueFilter.specifiedDesignComponent;
            string newLCMResourceKey = lcmEntity.Resourcekey;


            try
            {
                var opCoName = GetOpcoDescription((short)lcmEntity.Opcoid).Result;
                
                var oldDc = getDesignComponent((short)lcmEntity.Designcomponentid);
                var plannedDcName = GetPlannedDcEntitity((long)lcmEntity.Opcoid   , lcmEntity.Designcomponentid);
 
                if (oldDc != null)
                {
                    oldDcName = oldDc.ToDesignComponentName(_repositoryWrapper);
                    oldDcfName = oldDc.toDesignComponentFamily(_repositoryWrapper);
                }

                if (oldDc != null && lcmEntity != null)
                {
                    string[] splitUpResourceKey = lcmEntity.Resourcekey.Split("_");

                    int lifeCycleId = Convert.ToInt16(IncrementLifeCycle(splitUpResourceKey[0] ));

                    await GenerateDCFEntryForLCM(lcmEntity, oldDcName, activityDescription, 0, (long)oldDc.Designcomponentfamilyid, opCoName,oldDcfName, plannedDcName);
                    //await GenerateDCFEntryForComponenet((long)lcmEntity.Opcoid, lcmEntity.Designcomponentid, (long)oldDc.Designcomponentfamilyid,lcmEntity.Buildbagid,1 ,
                    //   ConstantValueFilter.componentDcfEventDeatil.FirstOrDefault(x => x.Key == 1).Text);
                    newLCMResourceKey = splitUpResourceKey[0] + "_" + setPrefixZeroToLifeCycleId(
                                    lifeCycleId + 1);
 
                    ResourceKeyMasterDto dto = new ResourceKeyMasterDto();
                    string[] lcmKey = newLCMResourceKey.Split("_");
                    dto.ResourceKey = lcmKey[0];
                    dto.OpCoId = (short)lcmEntity.Opcoid;
                    dto.DcfId = oldDc.Designcomponentfamilyid;
                    dto.ResourceTypesId = (int)ResourceTypesKey.Lcm;
                    dto.LifeCycleId = int.Parse(lcmKey[1]);
                    dto.BuildBagId = lcmEntity.Buildbagid;
                    await _resourceKeyMasterManager.CreateOrUpdateResourceKey(dto);
                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess,
                    Data = newLCMResourceKey
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This function will be invoked whenever a resource key is getting chnaged for existing LCM
        /// </summary>
        /// <param name="lcmEntity"></param>
        /// <param name="NewresourceKey"></param>
        /// <returns></returns>
        public bool CreateDCFLifecycleforResourceKeyUpdates(Lcmengineering lcmEntity, string newLCMResourceKey)
        {
            string oldDcName = string.Empty, oldDcfamilyName = string.Empty;
            string activityDescription = ConstantValueFilter.resourceKeyUpdated;
            try
            {
                var opCoName = GetOpcoDescription((short)lcmEntity.Opcoid).Result;
                var dcEntity = getDesignComponent((short)lcmEntity.Designcomponentid);
 
                var oldDc = getDesignComponent((short)lcmEntity.Designcomponentid);

                var bagname = getBagName(lcmEntity.Buildbagid);
                if (oldDc != null)
                {
                    oldDcName = oldDc.ToDesignComponentName(_repositoryWrapper);
                    oldDcfamilyName = dcEntity.toDesignComponentFamily(_repositoryWrapper);

                }

                if (oldDc != null && lcmEntity != null)
                {

                    Dcflifecycle newDCFrecord = new Dcflifecycle();
                    newDCFrecord.Dcfid = (long)oldDc.Designcomponentfamilyid;
                    newDCFrecord.Opcoid = lcmEntity.Opcoid;
                    newDCFrecord.EventId = 0;
                    newDCFrecord.Resourcekey = newLCMResourceKey;
                    newDCFrecord.Previousresourcekey = lcmEntity.Resourcekey;
                    newDCFrecord.Dcid = lcmEntity.Designcomponentid;
                    newDCFrecord.Currentdetails = oldDcName;
                    newDCFrecord.EventName = activityDescription;
                    newDCFrecord.Opcodescription = opCoName;
                    newDCFrecord.Dcfdescription = oldDcfamilyName;
                    newDCFrecord.Bagname = bagname;
                   // newDCFrecord.Buildbagid = lcmEntity.Buildbagid;
                    newDCFrecord.Categorytype = (int)ResourceTypesKey.Lcm;

                    _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(newDCFrecord);
                    _repositoryWrapper.Save();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Update the Resourcekey in DCFLIfecycle entity to make sure the chain in not broken when the resourcekey changes...
        /// </summary>
        /// <param name="oldlcmResourceKey"></param>
        /// <param name="newlcmResourceKey"></param>
        /// <returns></returns>
        public bool UpdateDCFLifecycleForLCMResourceKey(string oldlcmResourceKey, string newlcmResourceKey)
        {
            string oldResourceKey = string.Empty;
            string newResourceKey = string.Empty;
            string existingLCMResourceKey = string.Empty;
            string existingLCMLifecycleid = string.Empty;
            string[] arrOldResourceKey = null;
            string[] arrNewResourceKey = null;

            if (oldlcmResourceKey != string.Empty)
            {
                arrOldResourceKey = oldlcmResourceKey.Split("_");
                if (arrOldResourceKey.Length > 0)
                {
                    oldResourceKey = arrOldResourceKey[0].ToString();
                }
            }

            if (newlcmResourceKey != string.Empty)
            {
                arrNewResourceKey = newlcmResourceKey.Split("_");
                if (arrNewResourceKey.Length > 0)
                {
                    newResourceKey = arrNewResourceKey[0].ToString();
                }
            }

            var dcfLifecycleExistsforResourceKey = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(
                x => x.Resourcekey.Contains(oldResourceKey)).ToList();

            foreach (var dcflifecycle in dcfLifecycleExistsforResourceKey)
            {
                string[] lcmResourceKey = dcflifecycle.Resourcekey.Split("_");

                if (lcmResourceKey.Length > 1)
                {
                    existingLCMResourceKey = lcmResourceKey[0].ToString();
                    existingLCMLifecycleid = lcmResourceKey[1].ToString();
                    dcflifecycle.Previousresourcekey = dcflifecycle.Resourcekey;
                    dcflifecycle.Resourcekey = string.Concat(newResourceKey, "_", existingLCMLifecycleid);

                    _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Update(dcflifecycle);
                    _repositoryWrapper.Save();
                }
            }
            _repositoryWrapper.ClearTracker();
            return true;
        }

        public string GetResourceKeyfromLCMResourceKeyString(string LCMResourceKeyString)
        {
            string[] parts = LCMResourceKeyString.Split('_');
            if (parts.Length == 2)
            {
                return parts[0].ToString();
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region AssetOperations

        /// <summary>
        /// This function will insert a DCF lifecycle record
        /// </summary>
        /// <param name="DesignComponentId"></param>
        /// <param name="OpCoId"></param>
        /// <param name="ElementName"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> InitialiseDCFLifecycleforAssets(long DesignComponentId, short? OpCoId, string ElementName,long BuildBagId)
        {
            string SWComponentKey = string.Empty;
            string HWComponentKey = string.Empty;
            string newSWComponentKey = string.Empty;
            string newHWComponentKey = string.Empty;
            try
            {
                var opCoName = GetOpcoDescription((short)OpCoId).Result;
                var dcEntity = getDesignComponent((short)DesignComponentId);
 
                var dcName = dcEntity.ToDesignComponentName(_repositoryWrapper);
                var dcfamilyName = dcEntity.toDesignComponentFamily(_repositoryWrapper);
                string bagName = await GetBagDetails(BuildBagId);
                if (dcEntity != null)
                {
                    //Retrieve SWComponent Section of Assets
                    SWComponentKey = _resourceKeyMasterManager.GenerateResourceKeyForAsset((short)OpCoId, (long)dcEntity.Designcomponentfamilyid, ElementName, (int)ResourceTypeEnum.ResourceTypesKey.SWAsset, false);

                    //string HWType = GetHardwareType(DesignComponentId);
                    bool HWTypeVirtualised = IsHardwareVirtualised(DesignComponentId);

                    HWComponentKey = _resourceKeyMasterManager.GenerateResourceKeyForAsset((short)OpCoId, (long)dcEntity.Designcomponentfamilyid, ElementName, (int)ResourceTypeEnum.ResourceTypesKey.HWAsset, HWTypeVirtualised);

                    //var DcfExists =  
                    var DcfExistsForHardware = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(x => x.Opcoid == OpCoId
                                    && x.Currentdetails == ElementName
                                    && x.Dcfid == dcEntity.Designcomponentfamilyid
                                    && x.Resourcekey == HWComponentKey).FirstOrDefault();
                    if (DcfExistsForHardware == null)
                    {
                        var hwComponentKey = new Dcflifecycle()
                        {
                            EventId = 0,
                            EventName = ConstantValueFilter.startOfLifeCycle,
                            Currentdetails = ElementName,
                            Opcoid = OpCoId,
                            Dcfid = (long)dcEntity.Designcomponentfamilyid,
                            Resourcekey = HWComponentKey,
                            Categorytype = (int)ResourceTypesKey.HWAsset,
                            Opcodescription = opCoName,
                            Dcfdescription = dcfamilyName,
                            Bagname = bagName,
                            Dcdescription = dcName,
                            Dcid = DesignComponentId
                        };
                        _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(hwComponentKey);
                        _repositoryWrapper.Save();
                       await _repositoryWrapper.ClearTracker();
                    }

                    var DcfExistsForSoftware = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(x => x.Opcoid == OpCoId
                                                && x.Currentdetails == ElementName
                                                && x.Dcfid == dcEntity.Designcomponentfamilyid
                                                && x.Resourcekey == SWComponentKey).FirstOrDefault();
                    if (DcfExistsForSoftware == null)
                    {
                        var swComponentKey = new Dcflifecycle()
                        {
                            EventId = 0,
                            EventName = ConstantValueFilter.startOfLifeCycle,
                            Currentdetails = ElementName,
                            Opcoid = OpCoId,
                            Dcfid = (long)dcEntity.Designcomponentfamilyid,
                            Resourcekey = SWComponentKey,
                            Categorytype = (int)ResourceTypesKey.SWAsset,
                            Opcodescription = opCoName,
                            Dcfdescription = dcfamilyName,
                            Bagname = bagName,
                             Dcdescription = dcName,
                            Dcid = DesignComponentId
                        };

                        _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(swComponentKey);
                        _repositoryWrapper.Save();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                ResourceKeyMasterDto SWresourcekeyMasterDto = new ResourceKeyMasterDto();
                SWresourcekeyMasterDto.DcfId = (long)dcEntity.Designcomponentfamilyid;
                SWresourcekeyMasterDto.ResourceTypesId = (int)ResourceTypesKey.SWAsset;
                SWresourcekeyMasterDto.ElementName = ElementName;
                SWresourcekeyMasterDto.OpCoId = (short)OpCoId;
                SWresourcekeyMasterDto.ResourceKey = SWComponentKey;

                ResourceKeyMasterDto HWresourcekeyMasterDto = new ResourceKeyMasterDto();
                HWresourcekeyMasterDto.DcfId = (long)dcEntity.Designcomponentfamilyid;
                HWresourcekeyMasterDto.ResourceTypesId = (int)ResourceTypesKey.HWAsset;
                HWresourcekeyMasterDto.ElementName = ElementName;
                HWresourcekeyMasterDto.OpCoId = (short)OpCoId;
                HWresourcekeyMasterDto.ResourceKey = HWComponentKey;

               await _resourceKeyMasterManager.CreateOrUpdateResourceKey(SWresourcekeyMasterDto);
                await _resourceKeyMasterManager.CreateOrUpdateResourceKey(HWresourcekeyMasterDto);


                IDictionary<int, string> AssetComponentKeys = new Dictionary<int, string>
                {
                    { (int)ResourceTypesKey.SWAsset, SWComponentKey },
                    { (int)ResourceTypesKey.HWAsset, HWComponentKey }
                };

                return AssetComponentKeys;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This function will identify the virtualised nodes (hardware) and flag it to calling function as virtualised or not.
        /// </summary>
        /// <param name="designComponentId"></param>
        /// <returns></returns>
        public bool IsHardwareVirtualised(long designComponentId)
        {
            //Virtualised Hardware Types
            string[] VirtualisedHWTypeArray = ConstantValueFilter.virtualisedHWTypeArray;// { "BLUEPRINT NFVI", "OTHER NFVI", "BLUEPRINT NFCI", "OTHER NFCI", "VIRTUALISED" };
            bool VirtualisedHW = false;
            try
            {
                var systemtypes = _repositoryWrapper.DesignComponent
                       .FindByCondition(x => x.Designcomponentid == designComponentId)
                       .Include(x => x.Systemtype)
                       .ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                       .ThenInclude(x => x.Majorhardware)
                       .ThenInclude(x => x.Buildconstruction).AsNoTracking().FirstOrDefault();
                if (systemtypes != null)
                {
                    string buildConstruction = systemtypes.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault()?.Majorhardware?.Buildconstruction?.Buildconstruction?.ToString()?.ToUpper();

                    if (buildConstruction != null)
                        VirtualisedHW = VirtualisedHWTypeArray.Contains(buildConstruction);
                    else
                        VirtualisedHW = false;
                }
                return VirtualisedHW;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateAssetNameOnDcfLifecycle(long DesignComponentid, long OpCoId, long NetworkElementAsisPlannedId, string NewElementName)
        {
            var OldElementName = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == NetworkElementAsisPlannedId).Select(x => x.Elementname).Single();
            var dcf = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == DesignComponentid).FirstOrDefault();
            if (OldElementName != NewElementName)
            {
                var ResourceKeyMasterExists = _repositoryWrapper.ResourceKeyMaster.FindByCondition(x => x.Opcoid == OpCoId
                                           && x.Dcfid == dcf.Designcomponentfamilyid && x.Elementname == OldElementName).ToList();
                var DcfLifeCycleExists = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(x => x.Opcoid == OpCoId
                                           && x.Dcfid == dcf.Designcomponentfamilyid && x.Currentdetails == OldElementName).ToList();
                foreach (var ResourceKeyMasterExist in ResourceKeyMasterExists)
                {
                    if (ResourceKeyMasterExist.Resourcekey.StartsWith("2")
                        || ResourceKeyMasterExist.Resourcekey.StartsWith("1"))
                    {
                        ResourceKeyMasterExist.Elementname = NewElementName;
                        _repositoryWrapper.ResourceKeyMaster.Update(ResourceKeyMasterExist);
                        _repositoryWrapper.Save();
                    }
                }

                foreach (var DcfLifeCycleExist in DcfLifeCycleExists)
                {
                    if (DcfLifeCycleExist.Resourcekey.StartsWith("2")
                        || DcfLifeCycleExist.Resourcekey.StartsWith("1"))
                    {
                        DcfLifeCycleExist.Currentdetails = NewElementName;
                        _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Update(DcfLifeCycleExist);
                        _repositoryWrapper.Save();
                    }
                }

            }
            _repositoryWrapper.ClearTracker();
            return NewElementName;
        }

        /// <summary>
        /// This function will be invoked when a delete Asset Activity is triggered.
        /// </summary>
        /// <param name="assetEntity"></param>
        /// <returns></returns>
        public bool GenerateAssetLifeCycleEntryInDCF(Networkelementsasplanned assetEntity, string? assetActivityDescription = "",  long BuildBagId = 0)
        {
            string oldDcName = string.Empty, oldDCFName = string.Empty;
            string activityDescription = (!string.IsNullOrEmpty(assetActivityDescription)) ? assetActivityDescription :
                "Asset Removed";

            try
            {
                var oldDc = _repositoryWrapper.DesignComponent
                           .FindByCondition(x => x.Designcomponentid == assetEntity.Designcomponentid).FirstOrDefault();

                string bagName =   GetBagDetails(BuildBagId).Result;
                if (oldDc != null)
                {
                    oldDcName = oldDc.ToDesignComponentName(_repositoryWrapper);
                    oldDCFName = oldDc.toDesignComponentFamily(_repositoryWrapper);
                }

                var opCoName =   GetOpcoDescription(assetEntity.Opcoid).Result;
 

                if (oldDc != null && assetEntity != null)
                {

                    Dcflifecycle newDCFrecordSW = new Dcflifecycle();
                    newDCFrecordSW.Dcfid = (long)oldDc.Designcomponentfamilyid;
                    newDCFrecordSW.Opcoid = assetEntity.Opcoid;
                    newDCFrecordSW.EventId = 0;
                    newDCFrecordSW.Resourcekey = assetEntity.Swresourcekey;
                    newDCFrecordSW.Previousresourcekey = assetEntity.Previoushwresourcekey;
                    newDCFrecordSW.Dcid = assetEntity.Designcomponentid;
                    newDCFrecordSW.Currentdetails = assetEntity.Elementname;
                    newDCFrecordSW.EventName = activityDescription;
                    newDCFrecordSW.Categorytype = (int)ResourceTypesKey.SWAsset;
                    newDCFrecordSW.Dcfdescription = oldDCFName;
                    newDCFrecordSW.Bagname = bagName;
                    newDCFrecordSW.Dcdescription = oldDcName;
                    newDCFrecordSW.Opcodescription = opCoName;

                    _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(newDCFrecordSW); // Add a method to create a new entity in your repository
                    _repositoryWrapper.Save();

                    Dcflifecycle newDCFrecordHW = new Dcflifecycle();
                    newDCFrecordHW.Dcfid = (long)oldDc.Designcomponentfamilyid;
                    newDCFrecordHW.Opcoid = assetEntity.Opcoid;
                    newDCFrecordHW.EventId = 0;
                    newDCFrecordHW.Resourcekey = assetEntity.Hwresourcekey;
                    newDCFrecordHW.Previousresourcekey = assetEntity.Previoushwresourcekey;
                    newDCFrecordHW.Dcid = assetEntity.Designcomponentid;
                    newDCFrecordHW.Currentdetails = assetEntity.Elementname;
                    newDCFrecordHW.EventName = activityDescription;
                    newDCFrecordHW.Categorytype = (int)ResourceTypesKey.HWAsset;
                    newDCFrecordHW.Dcfdescription = oldDCFName;
                    newDCFrecordHW.Bagname = bagName;
                    newDCFrecordHW.Dcdescription = oldDcName;
                    newDCFrecordHW.Opcodescription = opCoName;
                    _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(newDCFrecordHW); // Add a method to create a new entity in your repository
                    _repositoryWrapper.Save();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GenerateAssetLifeCycleEntryInDCF(List<Networkelementsasplanned> assetEntitys, string? assetActivityDescription = "", long BuildBagId = 0, Dictionary<string,long?> PlannedDcIds = null)
        {
            string oldDcName = string.Empty, oldDCFName = string.Empty, plannedDcName = string.Empty;
            string activityDescription = (!string.IsNullOrEmpty(assetActivityDescription)) ? assetActivityDescription :
                "Asset Removed";

            try
            {
                foreach (var assetEntity in assetEntitys)
                {
                    var oldDc = _repositoryWrapper.DesignComponent
                               .FindByCondition(x => x.Designcomponentid == assetEntity.Designcomponentid).FirstOrDefault();

                    string bagName = GetBagDetails(BuildBagId).Result;
                    if (oldDc != null)
                    {
                        oldDcName = oldDc.ToDesignComponentName(_repositoryWrapper);
                        oldDCFName = oldDc.toDesignComponentFamily(_repositoryWrapper);
                    }

                    if(PlannedDcIds != null && PlannedDcIds.Count > 0)
                    {
                        var plannedDc = _repositoryWrapper.DesignComponent.FindByCondition(f => f.Designcomponentid == PlannedDcIds.GetValueOrDefault(assetEntity.Elementname).Value).FirstOrDefault();
                        if(plannedDc != null)
                        {
                            plannedDcName = plannedDc.ToDesignComponentName(_repositoryWrapper);
                        }
                    }

                    var opCoName = GetOpcoDescription(assetEntity.Opcoid).Result;


                    if (oldDc != null && assetEntity != null)
                    {

                        Dcflifecycle newDCFrecordSW = new Dcflifecycle();
                        newDCFrecordSW.Dcfid = (long)oldDc.Designcomponentfamilyid;
                        newDCFrecordSW.Opcoid = assetEntity.Opcoid;
                        newDCFrecordSW.EventId = 0;
                        newDCFrecordSW.Resourcekey = assetEntity.Swresourcekey;
                        newDCFrecordSW.Previousresourcekey = assetEntity.Previoushwresourcekey;
                        newDCFrecordSW.Dcid = assetEntity.Designcomponentid;
                        newDCFrecordSW.Currentdetails = assetEntity.Elementname;
                        newDCFrecordSW.EventName = activityDescription;
                        newDCFrecordSW.Categorytype = (int)ResourceTypesKey.SWAsset;
                        newDCFrecordSW.Dcfdescription = oldDCFName;
                        newDCFrecordSW.Bagname = bagName;
                        newDCFrecordSW.Dcdescription = oldDcName;
                        newDCFrecordSW.Opcodescription = opCoName;
                        newDCFrecordSW.Planneddetails = plannedDcName;

                        _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(newDCFrecordSW); // Add a method to create a new entity in your repository
                        _repositoryWrapper.Save();

                        Dcflifecycle newDCFrecordHW = new Dcflifecycle();
                        newDCFrecordHW.Dcfid = (long)oldDc.Designcomponentfamilyid;
                        newDCFrecordHW.Opcoid = assetEntity.Opcoid;
                        newDCFrecordHW.EventId = 0;
                        newDCFrecordHW.Resourcekey = assetEntity.Hwresourcekey;
                        newDCFrecordHW.Previousresourcekey = assetEntity.Previoushwresourcekey;
                        newDCFrecordHW.Dcid = assetEntity.Designcomponentid;
                        newDCFrecordHW.Currentdetails = assetEntity.Elementname;
                        newDCFrecordHW.EventName = activityDescription;
                        newDCFrecordHW.Categorytype = (int)ResourceTypesKey.HWAsset;
                        newDCFrecordHW.Dcfdescription = oldDCFName;
                        newDCFrecordHW.Bagname = bagName;
                        newDCFrecordHW.Dcdescription = oldDcName;
                        newDCFrecordHW.Opcodescription = opCoName;
                        newDCFrecordHW.Planneddetails = plannedDcName;
                        _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(newDCFrecordHW); // Add a method to create a new entity in your repository
                        _repositoryWrapper.Save();
                    }
                }
                    return true;             
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async void UpdateResoureKeyForAssetInDcfLifecycle(NetworkElementAsPlanned dto)
        {
            try
            {
                var assetExistsforCurrentDCId = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(
               x => x.Opcoid == dto.OpCoId && x.Designcomponentid == dto.DesignComponentId && x.Elementname == dto.ElementName).FirstOrDefault();
                var Dcf = _repositoryWrapper.DesignComponent.FindByCondition(
                                x => x.Designcomponentid == dto.DesignComponentId).FirstOrDefault();
                if (assetExistsforCurrentDCId != null)
                {
                    if (assetExistsforCurrentDCId.Hwresourcekey != dto.HwResourceKey)
                    {
                        var reskeyexists = _repositoryWrapper.ResourceKeyMaster.FindByCondition(
                                    x => x.Resourcekey == dto.HwResourceKey).FirstOrDefault();
                        if (reskeyexists == null)
                        {
                            ResourceKeyMasterDto HwresourceKeyMasterDto = new ResourceKeyMasterDto
                            {
                                ElementName = dto.ElementName,
                                OpCoId = dto.OpCoId,
                                DcfId = Dcf.Designcomponentfamilyid,
                                ResourceKey = dto.HwResourceKey,
                                ResourceTypesId = (int)ResourceTypesKey.HWAsset,
                            };
                            await _resourceKeyMasterManager.CreateOrUpdateResourceKey(HwresourceKeyMasterDto);
                        }
                        else
                        {
                            //Set the current resourcekey status to not in use
                            setResourceKeyStatus(assetExistsforCurrentDCId.Hwresourcekey, (int)ResourceTypesKey.HWAsset);
                        }

                        var dcfExists = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(
                                        x => x.Opcoid == dto.OpCoId && x.Currentdetails == dto.ElementName).ToList();
                        foreach (var dcfExist in dcfExists)
                        {
                            if (dcfExist.Resourcekey.StartsWith("2"))
                            {
                                dcfExist.Previousresourcekey = assetExistsforCurrentDCId.Hwresourcekey;
                                dcfExist.Resourcekey = dto.HwResourceKey;
                                _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Update(dcfExist);

                            }
                        }
                        _repositoryWrapper.Save();
                    }

                    if (assetExistsforCurrentDCId.Swresourcekey != dto.SwResourceKey)
                    {
                        ResourceKeyMasterDto SwresourceKeyMasterDto = new ResourceKeyMasterDto
                        {
                            ElementName = dto.ElementName,
                            OpCoId = dto.OpCoId,
                            DcfId = Dcf.Designcomponentfamilyid,
                            ResourceKey = dto.SwResourceKey,
                            ResourceTypesId = (int)ResourceTypesKey.SWAsset,
                        };

                        await _resourceKeyMasterManager.CreateOrUpdateResourceKey(SwresourceKeyMasterDto);


                        var dcfExists = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(
                        x => x.Opcoid == dto.OpCoId && x.Currentdetails == dto.ElementName).ToList();
                        foreach (var dcfExist in dcfExists)
                        {
                            if (dcfExist.Resourcekey.StartsWith("1"))
                            {
                                dcfExist.Previousresourcekey = assetExistsforCurrentDCId.Swresourcekey;

                                dcfExist.Resourcekey = dto.SwResourceKey;
                                _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Update(dcfExist);

                            }
                        }
                        _repositoryWrapper.Save();
                    }
                }
                await _repositoryWrapper.ClearTracker();
            }
            catch (Exception)
            {
                throw;
            }
            //End of LCM R8 - Part 3 requirements to update overridden resource keys

        }

        #endregion

        #region IdentityOperations

        public async Task<string> CreateorupdateIdentityResourceKey(IdentityAsIsDtoCreate dto)
        {
            var elementname = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == dto.AssetId)
                         .Select(x => x.Elementname).Single();

            var opCoEntity = _repositoryWrapper.NetworkElementAsPlanned.
                FindByCondition(x => x.Networkelementasplannedid == dto.AssetId).Include(y => y.Opco).Include(x => x.Designcomponent)
                .ThenInclude(x => x.Designcomponentfamily)
                .SingleOrDefault();

            var dcid = opCoEntity.Designcomponentid;

            var DcfId = opCoEntity?.Designcomponent?.Designcomponentfamilyid;
                
            var opCoName = opCoEntity?.Opco.Opco;
            var opcoId = opCoEntity.Opcoid;
            var dcfamilyName = opCoEntity?.Designcomponent?.toDesignComponentFamily(_repositoryWrapper);
            var dcName = opCoEntity?.Designcomponent?.ToDesignComponentName(_repositoryWrapper);             

            var resourceKeyExists = _repositoryWrapper.ResourceKeyMaster.FindByCondition(
                                   x => x.Opcoid == opcoId
                                   && x.Elementname == elementname
                                   && x.Resourcetypesid == (int)ResourceTypesKey.Identity
                                   && x.Keystatus == ConstantValueFilter.isTrue).FirstOrDefaultAsync();

            string IdentityresourceKey = _resourceKeyMasterManager.GenerateResourceKeyForIdentity(opcoId, (long)DcfId, elementname, (int)ResourceTypesKey.Identity);

            if (resourceKeyExists.Result == null)
            {
                ResourceKeyMasterDto resourcekeyMasterDto = new ResourceKeyMasterDto();
                resourcekeyMasterDto.DcfId = DcfId;
                resourcekeyMasterDto.ResourceTypesId = (int)ResourceTypesKey.Identity;
                resourcekeyMasterDto.ElementName = elementname;
                resourcekeyMasterDto.OpCoId = opcoId;
                resourcekeyMasterDto.ResourceKey = IdentityresourceKey;

               await _resourceKeyMasterManager.CreateOrUpdateResourceKey(resourcekeyMasterDto);

                var IdentityKey = new Dcflifecycle()
                {
                    EventId = 0,
                    EventName = ConstantValueFilter.startOfLifeCycle,
                    Currentdetails = elementname,
                    Opcoid = opcoId,
                    Dcfid = (long)DcfId,
                    Resourcekey = IdentityresourceKey,
                    Categorytype= (int)ResourceTypesKey.Identity,
                    Dcfdescription = dcfamilyName,
                    Opcodescription = opCoName,
                    Dcdescription = dcName

                };

                _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(IdentityKey);
                return resourcekeyMasterDto.ResourceKey;
            }
            else
            {
                ResourceKeyMasterDto resourcekeyMasterDto = new ResourceKeyMasterDto();
                resourcekeyMasterDto.ResourceKey = dto.ResourceKey;

                return resourceKeyExists.Result.Resourcekey.ToString();
            }

        }

        /// <summary>
        /// This function will be invoked when a delete Asset Activity is triggered.
        /// </summary>
        /// <param name="assetEntity"></param>
        /// <returns></returns>
        public bool CreateDCFLifecycleforIdentityDeletion(Identitiesasis identityEntity)
        {
            string _designComponentName = string.Empty, dcfamilyName= string.Empty;
            string activityDescription = ConstantValueFilter.identitydeletedmanually;

            try
            {
                var asset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == identityEntity.Assetid)
                                     .Include(y => y.Opco).Include(x => x.Designcomponentfamily).FirstOrDefault();               

                var dcid = asset.Designcomponentid;

                var DcfId = asset?.Designcomponentfamily?.Designcomponentfamilyid; 
                var opCoName = asset?.Opco.Opco;
                var opcoId = asset.Opcoid;
                

                if (asset != null)
                {
                    var _designComponent = asset?.Designcomponent;// _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == asset.Designcomponentid).FirstOrDefault();

                    if (_designComponent != null)
                    {
                        _designComponentName = _designComponent.ToDesignComponentName(_repositoryWrapper);
                          dcfamilyName = asset?.Designcomponent?.toDesignComponentFamily(_repositoryWrapper);
                    }
                    if (_designComponentName != null && identityEntity != null)
                    {
                        Dcflifecycle newDCFrecordSW = new Dcflifecycle();
                        newDCFrecordSW.Dcfid = (long)_designComponent.Designcomponentfamilyid;
                        newDCFrecordSW.Opcoid = asset.Opcoid;
                        newDCFrecordSW.EventId = 0;
                        newDCFrecordSW.Resourcekey = identityEntity.Resourcekey;
                        newDCFrecordSW.Previousresourcekey = asset.Previoushwresourcekey;
                        newDCFrecordSW.Dcid = asset.Designcomponentid;
                        newDCFrecordSW.Currentdetails = asset.Elementname;
                        newDCFrecordSW.EventName = activityDescription;
                        newDCFrecordSW.Categorytype = (int)ResourceTypesKey.Identity;
                        newDCFrecordSW.Opcodescription = opCoName;
                        newDCFrecordSW.Dcfdescription = dcfamilyName;
                        newDCFrecordSW.Dcdescription = _designComponentName;
                        _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(newDCFrecordSW); // Add a method to create a new entity in your repository
                        _repositoryWrapper.Save();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ResourceKeyStatus
        public async void setResourceKeyStatus(string resourceKey, Int32 resourceTypeId)
        {
            if (resourceTypeId == (int)ResourceTypesKey.HWAsset)
            {
                var HWResourceKeyMapped = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(
                        x => x.Hwresourcekey == resourceKey && x.Deleted != ConstantValueFilter.isTrue)
                        .Select(x => x.Hwresourcekey).FirstOrDefault();

                if (HWResourceKeyMapped == null)
                {
                    var HWResourcekeyStatus = _repositoryWrapper.ResourceKeyMaster.
                                     FindByCondition(x => x.Resourcekey == resourceKey
                                     && x.Keystatus != !ConstantValueFilter.isTrue).FirstOrDefault();
                    if (HWResourcekeyStatus != null)
                    {
                        HWResourcekeyStatus.Keystatus = !ConstantValueFilter.isTrue;
                        _repositoryWrapper.ResourceKeyMaster.Update(HWResourcekeyStatus);
                        await _repositoryWrapper.SaveAsync();
                    }
                }
            }
            else if (resourceTypeId == (int)ResourceTypesKey.SWAsset)
            {
                var SWResourceKeyMapped = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(
                       x => x.Swresourcekey == resourceKey &&
                       x.Deleted != true).Select(x => x.Swresourcekey).FirstOrDefault();

                if (SWResourceKeyMapped == null)
                {
                    var SWResourcekeyStatus = _repositoryWrapper.ResourceKeyMaster.
                                     FindByCondition(x => x.Resourcekey == resourceKey
                                     && x.Keystatus != !ConstantValueFilter.isTrue).FirstOrDefault();
                    if (SWResourcekeyStatus != null)
                    {
                        SWResourcekeyStatus.Keystatus = !ConstantValueFilter.isTrue;
                        _repositoryWrapper.ResourceKeyMaster.Update(SWResourcekeyStatus);
                        await _repositoryWrapper.SaveAsync();
                    }
                }
            }
            else if (resourceTypeId == (int)ResourceTypesKey.Identity)
            {
                var ResourceKeyMapped = _repositoryWrapper.IdentityAsIsRepository.FindByCondition(
                        x => x.Resourcekey == resourceKey &&
                        x.Deleted != ConstantValueFilter.isTrue)
                        .Select(x => x.Resourcekey).FirstOrDefault();

                if (ResourceKeyMapped == null)
                {
                    var IdentityResourcekeyStatus = _repositoryWrapper.ResourceKeyMaster.
                                     FindByCondition(x => x.Resourcekey == resourceKey
                                     && x.Keystatus != !ConstantValueFilter.isTrue).FirstOrDefault();
                    if (IdentityResourcekeyStatus != null)
                    {
                        IdentityResourcekeyStatus.Keystatus = !ConstantValueFilter.isTrue;
                        _repositoryWrapper.ResourceKeyMaster.Update(IdentityResourcekeyStatus);
                        await _repositoryWrapper.SaveAsync();
                    }
                }
            }
            else if (resourceTypeId == (int)ResourceTypesKey.Lcm)
            {
                var lcmResourceKey = resourceKey.Split("_");
                if (lcmResourceKey.Length > 1)
                {
                    var LcmResourceKeyMapped = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Resourcekey.Contains(lcmResourceKey[0])
                                                && x.Deleted != ConstantValueFilter.isTrue && x.Archived != ConstantValueFilter.isTrue).Select(x => x.Resourcekey).FirstOrDefault();


                    if (LcmResourceKeyMapped == null)
                    {
                        var LcmResourceKeyStatus = _repositoryWrapper.ResourceKeyMaster.
                                         FindByCondition(x => x.Resourcekey == lcmResourceKey[0]
                                         && x.Keystatus != false).FirstOrDefault();
                        if (LcmResourceKeyStatus != null)
                        {
                            LcmResourceKeyStatus.Keystatus = false;
                            _repositoryWrapper.ResourceKeyMaster.Update(LcmResourceKeyStatus);
                            await _repositoryWrapper.SaveAsync();
                            await _repositoryWrapper.ClearTracker();
                            setResourceKeyStatusForComponent((long)LcmResourceKeyStatus.Opcoid, (long)LcmResourceKeyStatus.Dcfid, 
                                LcmResourceKeyStatus.Buildbagid) ;
                        }
                    }
                }
            }

        }
        #endregion

        #region SystmeOfSystem ResourceKey Generation for Bag Component

        public async Task<List<Componentsoftwarebuildbags>> GetBagMappedComponenet(long buildBagId)
            {
              var getBagComponenet = await _repositoryWrapper.ComponentSoftwareBuildBagRepository.FindByCondition(x => x.Buildbagid == buildBagId)
                .Include(x => x.Componentsoftwarebuild).ThenInclude(x => x.Componentmanufacturer)
                .Include(x => x.Buildbag)
               .ToListAsync();

            return getBagComponenet;

           }
        public async Task<String> GetBagDetails(long buildBagId)
        {
            var getBagEntity = await _repositoryWrapper.BuildBagRepository.FindByCondition(x => x.Buildbagid  == buildBagId) 
             .FirstOrDefaultAsync();

            string bagName = _commonManager.GetBuildBagDescription(getBagEntity);

            return bagName;

        }
        public async Task<bool> InitialiseDCFLifecycleforComponent( long OpCoId ,long dcId,long buildBagId , bool isComponentUpgrade = false, List<long> addNewOrDeleteComponent=null)
        {
            string componentResourceKey = string.Empty;
            string newComponentResourceKey = string.Empty;
            string plannedActivityResource = string.Empty;

            try
            {
                var bagComponenet = await GetBagMappedComponenet(buildBagId);

                if (bagComponenet != null && bagComponenet.Count > 0)
                {
                    var existsDesignComponent = getDesignComponent((short)dcId);
                       

                    var dcName = existsDesignComponent.ToDesignComponentName(_repositoryWrapper);
                    var dcfamilyName =   existsDesignComponent.toDesignComponentFamily(_repositoryWrapper);

                    var opCoName = GetOpcoDescription((short)OpCoId).Result;
                    var addComponent = addNewOrDeleteComponent != null && addNewOrDeleteComponent.Count()>0 ? 
                        bagComponenet.Where(x => addNewOrDeleteComponent.Any(y => y == x.Componentsoftwarebuildid)).ToList() : bagComponenet;

                    foreach (var item in addComponent)
                    {
                        string ElementName = $"{item.Componentsoftwarebuild.Componentmanufacturer.Componentmanufacturer}-" +
                                      $"{item.Componentsoftwarebuild.Componentmanufacturer.Componentname}-" +
                                      $"{item.Componentsoftwarebuild.Softwareversion}";

                        if (existsDesignComponent != null)
                        {
                            componentResourceKey = _resourceKeyMasterManager.GenerateResourceKeyForComponent((short)OpCoId, 
                                (long)existsDesignComponent.Designcomponentfamilyid, buildBagId, item.Componentsoftwarebuildid, (int)ResourceTypesKey.Component);


                            if (componentResourceKey != null)
                            {
                                ResourceKeyMasterDto dto = new ResourceKeyMasterDto();
                                dto.DcfId = existsDesignComponent.Designcomponentfamilyid;
                                dto.OpCoId = (short)OpCoId;
                                dto.ResourceTypesId = (int)ResourceTypesKey.Component;
                                string lifeCycleId = IncrementLifeCycle(componentResourceKey );
                                dto.ResourceKey = componentResourceKey;
                                dto.LifeCycleId = int.Parse(lifeCycleId) + 1;
                                dto.ElementName = ElementName;
                                dto.BuildBagId = buildBagId;
                                dto.ComponentId = item.Componentsoftwarebuildid;
                                await _resourceKeyMasterManager.CreateOrUpdateResourceKey(dto);
                                newComponentResourceKey = componentResourceKey.ToString() + "_" + setPrefixZeroToLifeCycleId(int.Parse(lifeCycleId) + 1);


                                string existsDcfResourceKey = _repositoryWrapper
                                    .DesignComponentFamilyLifeCycleRepository.
                                    FindByCondition(x => x.Opcoid == OpCoId && 
                                    x.Resourcekey == (componentResourceKey + ConstantValueFilter.resourceKeySuffixZero)).
                                    Select(x => x.Resourcekey.ToString()).FirstOrDefault();
                                if (string.IsNullOrEmpty(existsDcfResourceKey))
                                {
                                    Dcflifecycle newDCFrecord = new Dcflifecycle();
                                    newDCFrecord.Dcfid = (long)existsDesignComponent.Designcomponentfamilyid;
                                    newDCFrecord.Opcoid = (short?)OpCoId;
                                    newDCFrecord.Resourcekey = newComponentResourceKey;
                                    newDCFrecord.Dcid = dcId;
                                    newDCFrecord.EventId = 1;
                                    newDCFrecord.Currentdetails = ElementName;
                                    newDCFrecord.EventName = isComponentUpgrade == true ? ConstantValueFilter.componentDcfEventDeatil.FirstOrDefault(x => x.Key == 2).Text : ConstantValueFilter.componentDcfEventDeatil.FirstOrDefault(x => x.Key == 1).Text;
                                    newDCFrecord.Opcodescription = opCoName;
                                    newDCFrecord.Dcfdescription = dcfamilyName;
                                  
                                    newDCFrecord.Categorytype = (int)ResourceTypesKey.Component;
                                     newDCFrecord.Dcdescription = dcName;
                                    newDCFrecord.Bagname = item.Buildbag.Bagdescription;

                                    _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(newDCFrecord);
                                    _repositoryWrapper.Save();
                                }

                                _repositoryWrapper.Save();

                            }
                            
                        }

                    }

                }
                       
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> GenerateDCFEntryForComponenet(long opCoId, long dcId, long dcfId, long buildBagId, long PaResourceId  
            ,string EventName, bool isComponentDeleted = false, bool isBagDeAssociate = false, List<Componentsoftwarebuildbags> existedBagComponent=null,List<long> deleteComponent=null )
        {
          
            try
            {
                var bagComponents = existedBagComponent!=null && existedBagComponent.Count()>0? existedBagComponent: await GetBagMappedComponenet(buildBagId);

                if (bagComponents != null && bagComponents.Count > 0)
                {
                    var opCoName = (await GetOpcoDescription((short)opCoId));
                    var dcEntity = getDesignComponent((short)dcId);

                    var dcName = dcEntity.ToDesignComponentName(_repositoryWrapper);
                    var dcfamilyName = dcEntity.toDesignComponentFamily(_repositoryWrapper);
                    var addComponent = deleteComponent != null && deleteComponent.Count()>0 ?
                        bagComponents.Where(x => deleteComponent.Any(y => y == x.Componentsoftwarebuildid)).ToList() : bagComponents;

                    foreach (var item in addComponent)
                    {
                        string elementName = $"{item.Componentsoftwarebuild.Componentmanufacturer.Componentmanufacturer}-" +
                                             $"{item.Componentsoftwarebuild.Componentmanufacturer.Componentname}-" +
                                             $"{item.Componentsoftwarebuild.Softwareversion}";

                        var resourceKeyMaster = _repositoryWrapper.ResourceKeyMaster
                            .FindByCondition(x => x.Opcoid == opCoId
                                               && x.Dcfid == dcfId
                                               && x.Resourcetypesid == (int)ResourceTypesKey.Component
                                               && x.Buildbagid == buildBagId
                                               && x.Componentid == item.Componentsoftwarebuildid)
                            .FirstOrDefault();
                        string componentResourceKey = resourceKeyMaster?.Resourcekey + "_" +
                            setPrefixZeroToLifeCycleId(Convert.ToInt16(resourceKeyMaster?.Lifecycleid)+1);

                        if (resourceKeyMaster != null && resourceKeyMaster.Keystatus == false && !isComponentDeleted)
                        {
                            componentResourceKey = resourceKeyMaster?.Resourcekey + "_" +
                            setPrefixZeroToLifeCycleId( resourceKeyMaster.Lifecycleid );

                            resourceKeyMaster.Keystatus = true;
                            resourceKeyMaster.Lifecycleid += 1; // Increment Lifecycle ID
                            _repositoryWrapper.ResourceKeyMaster.Update(resourceKeyMaster);
                            _repositoryWrapper.Save();
                        }else if(isBagDeAssociate  && resourceKeyMaster == null)
                        {
                            componentResourceKey = _resourceKeyMasterManager.GenerateResourceKeyForComponent((short)opCoId,
                              dcfId, buildBagId, item.Componentsoftwarebuildid, (int)ResourceTypesKey.Component);


                            if (componentResourceKey != null)
                            {
                                ResourceKeyMasterDto dto = new ResourceKeyMasterDto();
                                dto.DcfId = dcfId;
                                dto.OpCoId = (short)opCoId;
                                dto.ResourceTypesId = (int)ResourceTypesKey.Component;
                                string lifeCycleId = IncrementLifeCycle(componentResourceKey);
                                dto.ResourceKey = componentResourceKey;
                                dto.LifeCycleId = int.Parse(lifeCycleId) + 1;
                                dto.ElementName = elementName;
                                dto.BuildBagId = buildBagId;
                                dto.ComponentId = item.Componentsoftwarebuildid;
                                 await _resourceKeyMasterManager.CreateOrUpdateResourceKey(dto)  ;
                                componentResourceKey = componentResourceKey.ToString() + "_" + setPrefixZeroToLifeCycleId(int.Parse(lifeCycleId));
                            }
                        }

                        if (resourceKeyMaster != null)
                        {
                            var dcfEntryExists = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository
                                .FindByCondition(x => x.Dcfid == dcfId
                                                  && x.Opcoid == opCoId
                                                  && x.EventId == PaResourceId
                                                  && x.Dcid == dcId
                                                  && x.Currentdetails.ToLower().Replace(" ", "") == elementName.ToLower().Replace(" ", ""))
                                .FirstOrDefault();

                            //  string lifeCycleId = (resourceKeyMaster != null) ? setPrefixZeroToLifeCycleId(resourceKeyMaster.Lifecycleid + 1) : "1";
                            //string newResourceKey = (!isBagDeAssociate)? string.Concat(resourceKeyMaster.Resourcekey, "_", lifeCycleId) :
                            //    string.Concat(componentResourceKey, "_", lifeCycleId);


                            string newResourceKey = componentResourceKey;

                            Dcflifecycle newDCFRecord = new Dcflifecycle
                            {
                                Dcfid = dcfId,
                                Opcoid = (short?)opCoId,
                                EventId = (short?)PaResourceId,
                                Resourcekey = newResourceKey,
                                Previousresourcekey = string.Empty,
                                Dcid = dcId,
                                Currentdetails = elementName,
                                EventName = EventName,
                                Opcodescription = opCoName,
                                Dcfdescription = dcfamilyName,
                                Dcdescription = dcName,
                                Categorytype = (int)ResourceTypesKey.Component,
                                Bagname = item.Buildbag.Bagdescription
                            };

                            _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Create(newDCFRecord);
                            await _repositoryWrapper.SaveAsync();
                        }
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }

            }

        public async Task<bool> CreateDCFLifecycleforComponentBasedOnLCMTransition(Lcmengineering oldlcmEntity)
        {
            var bagComponenet = await GetBagMappedComponenet((long)oldlcmEntity.Buildbagid);
            var componenetResourceKeyEntity = _repositoryWrapper.ResourceKeyMaster
    .FindByCondition(x => x.Buildbagid == oldlcmEntity.Buildbagid && x.Opcoid == oldlcmEntity.Opcoid && x.Dcfid == oldlcmEntity.Designcomponentfamilyid
    && x.Resourcetypesid == (int)ResourceTypesKey.Component && x.Elementname != null) 
    .ToList();
            

            var oldDesignComponent = getDesignComponent((short)oldlcmEntity.Designcomponentid);                 

            if (bagComponenet != null && bagComponenet.Count > 0)
            {
                foreach (var item in bagComponenet)
                {
                    string ElementName = $"{item.Componentsoftwarebuild.Componentmanufacturer.Componentmanufacturer}-" +
                                         $"{item.Componentsoftwarebuild.Componentmanufacturer.Componentname}-" +
                                         $"{item.Componentsoftwarebuild.Softwareversion}";
                    ResourceKeyMasterDto dto = new ResourceKeyMasterDto();
                    string[] splitUpResourceKey = componenetResourceKeyEntity.Find(y => y.Elementname.ToLower().Replace(" ","") == ElementName.ToLower().Replace(" ", ""))?.Resourcekey.Split("_");
                    if (splitUpResourceKey != null)
                    {
                        int lifeCycleId = Convert.ToInt16(IncrementLifeCycle(splitUpResourceKey[0]));
                        dto.ResourceKey = splitUpResourceKey[0];
                        dto.OpCoId = (short)oldlcmEntity.Opcoid;
                        dto.DcfId = oldDesignComponent.Designcomponentfamilyid;
                        dto.ResourceTypesId = (int)ResourceTypesKey.Component;
                        dto.LifeCycleId = lifeCycleId + 1;
                        dto.BuildBagId = oldlcmEntity.Buildbagid;
                        dto.ComponentId = item.Componentsoftwarebuildid;
                        await _resourceKeyMasterManager.CreateOrUpdateResourceKey(dto);

                    }
                    else
                    {
                        await InitialiseDCFLifecycleforComponent((long)oldlcmEntity.Opcoid, (long)oldlcmEntity.Designcomponentid, oldlcmEntity.Buildbagid);
                        return true;
                    }


                }

            }
            return true;
        }

        public string UpdateComponenetNameOnDcfLifecycle(long componenetId,string OldComponentName, string NewComponentElementName)
        {
            
            
                var ResourceKeyMasterExists = _repositoryWrapper.ResourceKeyMaster.FindByCondition(x => 
                                              x.Elementname == OldComponentName).ToList();

                var DcfLifeCycleExists = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(x => 
                x.Currentdetails == OldComponentName).ToList();

                foreach (var ResourceKeyMasterExist in ResourceKeyMasterExists)
                {
                    if (ResourceKeyMasterExist.Resourcekey.StartsWith("5") )
                    {
                        ResourceKeyMasterExist.Elementname = NewComponentElementName;
                        _repositoryWrapper.ResourceKeyMaster.Update(ResourceKeyMasterExist);
                        _repositoryWrapper.Save();
                    }
                }

                foreach (var DcfLifeCycleExist in DcfLifeCycleExists)
                {
                    if (DcfLifeCycleExist.Resourcekey.StartsWith("5") )
                    {
                        DcfLifeCycleExist.Currentdetails = NewComponentElementName;
                        _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Update(DcfLifeCycleExist);
                        _repositoryWrapper.Save();
                    }
                }

            
            _repositoryWrapper.ClearTracker();
            return NewComponentElementName;
        }

        public  void setResourceKeyStatusForComponent(long opCoId , long dcfId, long buildBagId)
        {  

                    var LcmResourceKeyStatus = _repositoryWrapper.ResourceKeyMaster.
                                       FindByCondition(x => x.Opcoid == opCoId && x.Dcfid == dcfId && x.Buildbagid == buildBagId
                                       && x.Keystatus != false).ToList();
                    if (LcmResourceKeyStatus != null)
                    {
                            foreach(var item in LcmResourceKeyStatus)
                            {
                                 item.Keystatus = false;
                                _repositoryWrapper.ResourceKeyMaster.Update(item);
                            }                        
                         _repositoryWrapper.Save();
                        _repositoryWrapper.ClearTracker();
                    }
 
        }

        public Task<string> GetOpcoDescription(short opCoId)
        {
            var opCoDetail = _repositoryWrapper.OpCo.FindByCondition(x => x.Opcoid == opCoId).FirstOrDefaultAsync();

            return Task.FromResult(opCoDetail.Result.Opco);


        }

        public Designcomponents  getDesignComponent(short dcId)
        {
         var dcEntity =    _repositoryWrapper.DesignComponent
                           .FindByCondition(x => x.Designcomponentid == dcId)                          
                           .FirstOrDefault();
            return dcEntity;
        }

        public string GetPlannedDcEntitity(long OpCoId, long DesignComponentId)
        {
            var latestPaEntity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Opcoid == OpCoId && x.Designcomponentid == DesignComponentId && x.Deleted == false
            && x.Archived == false).Include(x => x.Designcomponent).OrderBy(x => x.Plannedcompletion).FirstOrDefault(); 

            string plannedDcName = latestPaEntity?.Designcomponent.ToDesignComponentName(_repositoryWrapper);

            return plannedDcName??string.Empty;
        }

        public string getBagName(long bagId)
        {
            var bagName = _repositoryWrapper.BuildBagRepository
                              .FindByCondition(x => x.Buildbagid == bagId)
                              .FirstOrDefault()?.Bagdescription;
            return bagName ?? string.Empty;
        }

        public async Task< bool> setResourceKeyNotInUseAndAddDcfLifeCycleEntry(long opcoId, long dcId, long dcfId, long  EventId,long bagId)
        {
         
           await   GenerateDCFEntryForComponenet(opcoId, dcId, dcfId, bagId, EventId, 
               ConstantValueFilter.componentDcfEventDeatil.FirstOrDefault(x => x.Key == 4).Text,false,true) ;
            await Task.Run(() => setResourceKeyStatusForComponent(opcoId, dcfId, bagId));

            return true;
        }
        #endregion


    }
}