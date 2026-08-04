using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.BPT;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.BPT;
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
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.BPT
{
    public class BPTManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        public BPTManager(IEnumerable<IRepositoryWrapper> wrappers,
            IMapper mapper,
            GridCustomColumnManager customColumnManager,
            IRepositoryWrapper repositoryWrapper,
            IProductNameManager productNameManager,
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor,
            CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _customColumnManager = customColumnManager;
            _logger = logger;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;

        }

        #region Grid Load, Filter 
        private IQueryable<BudgetProjectTrackers> GetBPTRecords(ExpressionStarter<Budgetprojecttrackers> predicateResult)
        {
            var query = _repositoryWrapper.BudgetProjectTrackersRepository.FindByCondition(predicateResult).Where(x => x.Archive == false)
                        .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Plannedactivityresource)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent)
                         .Include(x => x.Plannedactivity).ThenInclude(x => x.Driver)
                         .Include(x => x.Plannedactivity).ThenInclude(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Opco)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Networkelementasplanned).ThenInclude(x => x.Networkelementasplannedsubdomainspoc)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                                                             .ThenInclude(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts)
                       .Include(x => x.Plannedactivity).ThenInclude(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                                                             .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware.Majorhwbuildsdesigncontacts)
                       .Include(x => x.Plannedactivity).ThenInclude(x => x.Serviceplan).ThenInclude(x => x.Serviceplandcfmappings).ThenInclude(x => x.Dcf).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                       .Include(x => x.Plannedactivity).ThenInclude(x => x.Serviceplan).ThenInclude(x => x.Serviceplandcfmappings).ThenInclude(x => x.Dcf).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts)
                       .OrderByDescending(x => x.Modificationdate).AsQueryable();

            var mapperQuery = query.ToList();
            var mapper = mapperQuery.AsEnumerable().Select(p => BudgetProjectTrackerMapper.Get(p)).ToList();
            foreach (var item in mapper)
            {
                if (item.NetworkElementAsPlannedId != null)
                {
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.NetworkElementAsPlannedSubdomainSpoc, 0)
                                              ?.Select(t => new FilterValueDtoKeyValueList
                                              {
                                                  Key = Convert.ToInt16(t.Value),
                                                  Value = t.Text
                                              })?.Distinct()?.ToList();
                }
                else if (item.LcmEngineeringId != null)
                {
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.LcmEngineeringSubdomainSpoc, 0)
                                             ?.Select(t => new FilterValueDtoKeyValueList
                                             {
                                                 Key = Convert.ToInt16(t.Value),
                                                 Value = t.Text
                                             })?.Distinct()?.ToList();
                }
                else if (item.DesignAspectId != null)
                {
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.DesignContactDto, 0)
                                            ?.Select(t => new FilterValueDtoKeyValueList
                                            {
                                                Key = Convert.ToInt16(t.Value),
                                                Value = t.Text
                                            })?.Distinct()?.ToList();
                }
                else if (item.Serviceplanid != null)
                {
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.DesignContactDto, 0)
                                            ?.Select(t => new FilterValueDtoKeyValueList
                                            {
                                                Key = Convert.ToInt16(t.Value),
                                                Value = t.Text
                                            })?.Distinct()?.ToList();
                }
            }
            return mapper.AsQueryable();
        }

        public async Task<QueryResultDto<BPTGridDto>> FindWithCondition(BPTQueryDto bptQueryDto)
        {
            var predicateResult = ApplyFilter(bptQueryDto);
            var rtn = new QueryResultDto<BPTGridDto>(new GenerateRenderForGrid<BPTGridDto>(_customColumnManager))
            {
            };
            var query = GetBPTRecords(predicateResult);

            #region vertical implementation for all lcm,asset,da and service info
            //var filterData = query.ToList();
            
            if (bptQueryDto.VerticalName?.Any() == true && !bptQueryDto.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Any(c => x.VerticalFilterDto != null && bptQueryDto.VerticalName.Contains(c.Key.ToString())));
            }
            else if (bptQueryDto.VerticalName?.Any() == true && bptQueryDto.VerticalName.Count() == 1 && bptQueryDto.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0));
            }
            else if (bptQueryDto.VerticalName?.Any() == true && bptQueryDto.VerticalName.Count() > 1 && bptQueryDto.VerticalName.Contains("yes"))
            {
                var nullVerticals = bptQueryDto.VerticalName.Contains("yes") ?
                    query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0)) : null;
                var verticalFilter = query
                                    .Where(x => x.VerticalFilterDto != null &&
                                    x.VerticalFilterDto.Any(c => bptQueryDto.VerticalName.Where(t => t != "yes").Contains(c.Key.ToString())));
                query = nullVerticals?.Any() == true && verticalFilter?.Any() == true ?
                    nullVerticals.Concat(verticalFilter) : nullVerticals?.Any() == true && verticalFilter
                    ?.Any() == false ? nullVerticals
                    : nullVerticals?.Any() == false && verticalFilter?.Any() == true ? verticalFilter : null;
            }
            #endregion

            var paginatedQuery = await Task.Run(() => query.ApplyOrdering(bptQueryDto, GetColumnsMap()).ApplyPaging(bptQueryDto).ToList());
            var data = paginatedQuery.ToList();

            IEnumerable<BPTGridDto> bptGridDto;

            bptGridDto = _mapper.Map<IEnumerable<BPTGridDto>>(data);

            rtn.Items = bptGridDto.ToArray();
            rtn.TotalItems = query.Count();
            return rtn;
        }

        public ExpressionStarter<Budgetprojecttrackers> ApplyFilter(BPTQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Budgetprojecttrackers>(true);
            var predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();

            if (buildFilterDto.BudgetLineCode != null && buildFilterDto.BudgetLineCode.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.BudgetLineCode)
                {
                    predicateInner.Or(x => x.Budgetlinecode == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.UploadStatus != null && buildFilterDto.UploadStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.UploadStatus)
                {
                    predicateInner.Or(x => x.Uploadstatus == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.UploadMode != null && buildFilterDto.UploadMode.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.UploadMode)
                {
                    predicateInner.Or(x => x.Uploadmode == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CurrentTrackingNumber != null && buildFilterDto.CurrentTrackingNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.CurrentTrackingNumber)
                {
                    predicateInner.Or(x => x.Currenttrackingnumber == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NewTrackingNumber != null && buildFilterDto.NewTrackingNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.NewTrackingNumber)
                {
                    predicateInner.Or(x => x.Newtrackingnumber == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BudgetLineCode != null && buildFilterDto.BudgetLineCode.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.BudgetLineCode)
                {
                    predicateInner.Or(x => x.Budgetlinecode == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Wbs != null && buildFilterDto.Wbs.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Wbs)
                {
                    predicateInner.Or(x => x.Wbs == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Opco != null && buildFilterDto.Opco.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Opco)
                {
                    predicateInner.Or(x => x.Opcoid.ToString() == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Domain != null && buildFilterDto.Domain.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Domain)
                {
                    predicateInner.Or(x => x.Domain == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Team != null && buildFilterDto.Team.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Team)
                {
                    predicateInner.Or(x => x.Team == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BudgetOwner != null && buildFilterDto.BudgetOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.BudgetOwner)
                {
                    predicateInner.Or(x => x.Budgetowner == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Program != null && buildFilterDto.Program.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Program)
                {
                    predicateInner.Or(x => x.Program == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BudgetProject != null && buildFilterDto.BudgetProject.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.BudgetProject)
                {
                    predicateInner.Or(x => x.Budgetproject == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Activity != null && buildFilterDto.Activity.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Activity)
                {
                    predicateInner.Or(x => x.Budgetprojecttrackerid == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Priority != null && buildFilterDto.Priority.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Priority)
                {
                    predicateInner.Or(x => x.Priority == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Driver != null && buildFilterDto.Driver.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Driver)
                {
                    predicateInner.Or(x => x.Plannedactivity.Driverid.ToString() == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Benefits != null && buildFilterDto.Benefits.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Benefits)
                {
                    predicateInner.Or(x => x.Benefits == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Risks != null && buildFilterDto.Risks.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Risks)
                {
                    predicateInner.Or(x => x.Risks == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Category != null && buildFilterDto.Category.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Category)
                {
                    predicateInner.Or(x => x.Category == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Nwelement != null && buildFilterDto.Nwelement.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Nwelement)
                {
                    predicateInner.Or(x => x.Nwelement == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VirtualizedNwElement != null && buildFilterDto.VirtualizedNwElement.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.VirtualizedNwElement)
                {
                    predicateInner.Or(x => x.Virtualizednwelement == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Vendor != null && buildFilterDto.Vendor.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Vendor)
                {
                    predicateInner.Or(x => x.Vendor == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LcmCategories != null && buildFilterDto.LcmCategories.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.LcmCategories)
                {
                    predicateInner.Or(x => x.Lcmcategories == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OhpLev1 != null && buildFilterDto.OhpLev1.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.OhpLev1)
                {
                    predicateInner.Or(x => x.Ohplev1 == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OhpLev2 != null && buildFilterDto.OhpLev2.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.OhpLev2)
                {
                    predicateInner.Or(x => x.Ohplev2 == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HfmLev1 != null && buildFilterDto.HfmLev1.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.HfmLev1)
                {
                    predicateInner.Or(x => x.Hfmlev1 == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HfmLev2 != null && buildFilterDto.HfmLev2.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.HfmLev2)
                {
                    predicateInner.Or(x => x.Hfmlev2 == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Fy != null && buildFilterDto.Fy.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Fy)
                {
                    predicateInner.Or(x => x.Fy == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Operational != null && buildFilterDto.Operational.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Operational)
                {
                    predicateInner.Or(x => x.Operational == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Transfers != null && buildFilterDto.Transfers.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Transfers)
                {
                    predicateInner.Or(x => x.Transfers == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Cost1sTest != null && buildFilterDto.Cost1sTest.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Cost1sTest)
                {
                    predicateInner.Or(x => x.Cost1stest == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Validation != null && buildFilterDto.Validation.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Validation)
                {
                    predicateInner.Or(x => x.Validation == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SignOff != null && buildFilterDto.SignOff.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.SignOff)
                {
                    predicateInner.Or(x => x.Signoff == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Sub != null && buildFilterDto.Sub.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Sub)
                {
                    predicateInner.Or(x => x.Sub == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.FinalReSub != null && buildFilterDto.FinalReSub.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.FinalReSub)
                {
                    predicateInner.Or(x => x.Finalresub == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Currency != null && buildFilterDto.Currency.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Currency)
                {
                    predicateInner.Or(x => x.Currency == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Adjustments != null && buildFilterDto.Adjustments.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Adjustments)
                {
                    predicateInner.Or(x => x.Adjustments == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.YtdActuals != null && buildFilterDto.YtdActuals.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.YtdActuals)
                {
                    predicateInner.Or(x => x.Ytdactuals == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedAbsorption != null && buildFilterDto.PlannedAbsorption.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.PlannedAbsorption)
                {
                    predicateInner.Or(x => x.Plannedabsorption == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Deviation != null && buildFilterDto.Deviation.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Deviation)
                {
                    predicateInner.Or(x => x.Deviation == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Apr != null && buildFilterDto.Apr.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Apr)
                {
                    predicateInner.Or(x => x.Apr == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.May != null && buildFilterDto.May.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.May)
                {
                    predicateInner.Or(x => x.May == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Jun != null && buildFilterDto.Jun.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Jun)
                {
                    predicateInner.Or(x => x.Jun == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Jul != null && buildFilterDto.Jul.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Jul)
                {
                    predicateInner.Or(x => x.Jul == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Aug != null && buildFilterDto.Aug.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Aug)
                {
                    predicateInner.Or(x => x.Aug == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Sep != null && buildFilterDto.Sep.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Sep)
                {
                    predicateInner.Or(x => x.Sep == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Oct != null && buildFilterDto.Oct.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Oct)
                {
                    predicateInner.Or(x => x.Oct == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Nov != null && buildFilterDto.Nov.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Nov)
                {
                    predicateInner.Or(x => x.Nov == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Dec != null && buildFilterDto.Dec.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Dec)
                {
                    predicateInner.Or(x => x.Dec == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Jan != null && buildFilterDto.Jan.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Jan)
                {
                    predicateInner.Or(x => x.Jan == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Feb != null && buildFilterDto.Feb.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Feb)
                {
                    predicateInner.Or(x => x.Feb == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Mar != null && buildFilterDto.Mar.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Mar)
                {
                    predicateInner.Or(x => x.Mar == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ApprovedBudget != null && buildFilterDto.ApprovedBudget.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.ApprovedBudget)
                {
                    predicateInner.Or(x => x.Approvedbudget == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Commitment != null && buildFilterDto.Commitment.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Commitment)
                {
                    predicateInner.Or(x => x.Commitment == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BudgetProjectDependency != null && buildFilterDto.BudgetProjectDependency.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.BudgetProjectDependency)
                {
                    predicateInner.Or(x => x.Budgetprojectdependency == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LocalProgram != null && buildFilterDto.LocalProgram.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.LocalProgram)
                {
                    predicateInner.Or(x => x.Localprogram == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LocalBudgetProject != null && buildFilterDto.LocalBudgetProject.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.LocalBudgetProject)
                {
                    predicateInner.Or(x => x.Localbudgetproject == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LocalDriver != null && buildFilterDto.LocalDriver.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.LocalDriver)
                {
                    predicateInner.Or(x => x.Localdriver == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LocalPrioritization != null && buildFilterDto.LocalPrioritization.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.LocalPrioritization)
                {
                    predicateInner.Or(x => x.Localprioritization == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PpmId != null && buildFilterDto.PpmId.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.PpmId)
                {
                    predicateInner.Or(x => x.Ppmid == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PpmBudgetProjectId != null && buildFilterDto.PpmBudgetProjectId.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.PpmBudgetProjectId)
                {
                    predicateInner.Or(x => x.Ppmbudgetprojectid == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CostCentre != null && buildFilterDto.CostCentre.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.CostCentre)
                {
                    predicateInner.Or(x => x.Costcentre == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.WpId != null && buildFilterDto.WpId.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.WpId)
                {
                    predicateInner.Or(x => x.Wpid == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.GroupBudgetOpcoId != null && buildFilterDto.GroupBudgetOpcoId.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.GroupBudgetOpcoId)
                {
                    predicateInner.Or(x => x.Groupbudgetopcoid == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.InternalProgram != null && buildFilterDto.InternalProgram.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.InternalProgram)
                {
                    predicateInner.Or(x => x.Internalprogram == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VerticalProject != null && buildFilterDto.VerticalProject.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.VerticalProject)
                {
                    predicateInner.Or(x => x.Verticalproject == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DomainSpecificLabels != null && buildFilterDto.DomainSpecificLabels.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.DomainSpecificLabels)
                {
                    predicateInner.Or(x => x.Domainspecificlabels == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LabelsMarketVsVertical != null && buildFilterDto.LabelsMarketVsVertical.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.LabelsMarketVsVertical)
                {
                    predicateInner.Or(x => x.Labelsmarketvsvertical == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OtherMinorVendors != null && buildFilterDto.OtherMinorVendors.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.OtherMinorVendors)
                {
                    predicateInner.Or(x => x.Otherminorvendors == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ExternalDemandBudget != null && buildFilterDto.ExternalDemandBudget.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.ExternalDemandBudget)
                {
                    predicateInner.Or(x => x.Externaldemandbudget == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.OpexImpact != null && buildFilterDto.OpexImpact.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.OpexImpact)
                {
                    predicateInner.Or(x => x.Opeximpact == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.LegalEntity != null && buildFilterDto.LegalEntity.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.LegalEntity)
                {
                    predicateInner.Or(x => x.Legalentity == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.YearlyTransfersTrack != null && buildFilterDto.YearlyTransfersTrack.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.YearlyTransfersTrack)
                {
                    predicateInner.Or(x => x.Yearlytransferstrack == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.YearlyAdjustmentsTrack != null && buildFilterDto.YearlyAdjustmentsTrack.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.YearlyAdjustmentsTrack)
                {
                    predicateInner.Or(x => x.Yearlyadjustmentstrack == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Mcustom02 != null && buildFilterDto.Mcustom02.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Mcustom02)
                {
                    predicateInner.Or(x => x.Mcustom02 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Mcustom03 != null && buildFilterDto.Mcustom03.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Mcustom03)
                {
                    predicateInner.Or(x => x.Mcustom03 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Mcustom04 != null && buildFilterDto.Mcustom04.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Mcustom04)
                {
                    predicateInner.Or(x => x.Mcustom04 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Mcustom05 != null && buildFilterDto.Mcustom05.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Mcustom05)
                {
                    predicateInner.Or(x => x.Mcustom05 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Mcustom06 != null && buildFilterDto.Mcustom06.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Mcustom06)
                {
                    predicateInner.Or(x => x.Mcustom06 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Mcustom07 != null && buildFilterDto.Mcustom07.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Mcustom07)
                {
                    predicateInner.Or(x => x.Mcustom07 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Mcustom08 != null && buildFilterDto.Mcustom08.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Mcustom08)
                {
                    predicateInner.Or(x => x.Mcustom08 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Mcustom09 != null && buildFilterDto.Mcustom09.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Mcustom09)
                {
                    predicateInner.Or(x => x.Mcustom09 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Mcustom10 != null && buildFilterDto.Mcustom10.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Mcustom10)
                {
                    predicateInner.Or(x => x.Mcustom10 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Vcustom01 != null && buildFilterDto.Vcustom01.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Vcustom01)
                {
                    predicateInner.Or(x => x.Vcustom01 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Vcustom02 != null && buildFilterDto.Vcustom02.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Vcustom02)
                {
                    predicateInner.Or(x => x.Vcustom02 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Vcustom03 != null && buildFilterDto.Vcustom03.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Vcustom03)
                {
                    predicateInner.Or(x => x.Vcustom03 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Vcustom04 != null && buildFilterDto.Vcustom04.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Vcustom04)
                {
                    predicateInner.Or(x => x.Vcustom04 == item);
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.Vcustom05 != null && buildFilterDto.Vcustom05.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Vcustom05)
                {
                    predicateInner.Or(x => x.Vcustom05 == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Dcustom01 != null && buildFilterDto.Dcustom01.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Dcustom01)
                {
                    predicateInner.Or(x => x.Dcustom01 == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Dcustom02 != null && buildFilterDto.Dcustom02.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Dcustom02)
                {
                    predicateInner.Or(x => x.Dcustom02 == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Dcustom03 != null && buildFilterDto.Dcustom03.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Dcustom03)
                {
                    predicateInner.Or(x => x.Dcustom03 == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Dcustom04 != null && buildFilterDto.Dcustom04.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Dcustom04)
                {
                    predicateInner.Or(x => x.Dcustom04 == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Dcustom05 != null && buildFilterDto.Dcustom05.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Dcustom05)
                {
                    predicateInner.Or(x => x.Dcustom05 == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Lsdb != null && buildFilterDto.Lsdb.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Lsdb)
                {
                    predicateInner.Or(x => x.Lsdb == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Ls012 != null && buildFilterDto.Ls012.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Ls012)
                {
                    predicateInner.Or(x => x.Ls012 == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Ls210 != null && buildFilterDto.Ls210.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Ls210)
                {
                    predicateInner.Or(x => x.Ls210 == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Ls57 != null && buildFilterDto.Ls57.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Ls57)
                {
                    predicateInner.Or(x => x.Ls57 == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Archive != null && buildFilterDto.Archive.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.Archive)
                {
                    predicateInner.Or(x => x.Archive == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Budgetprojecttrackers>();
                if (buildFilterDto.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModifiedValue.StartDate);
                if (buildFilterDto.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private Dictionary<string, Expression<Func<BudgetProjectTrackers, object>>[]> GetColumnsMap()
        {

            return null;
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, BPTQueryDto buildFilterDto,bool isAdmin=false)
        {
            var predicateResult = ApplyFilter(buildFilterDto);
            var query = await Task.Run(() => GetBPTRecords(predicateResult));

            foreach (var filter in query)
            {
                filter.Activity = PlannedActivityMapper.Set(filter.PlannedActivity).GetActvityString(_repositoryWrapper);
            }

            if (buildFilterDto.VerticalName?.Any() == true && !buildFilterDto.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Any(c => x.VerticalFilterDto != null && buildFilterDto.VerticalName.Contains(c.Key.ToString())));
            }
            else if (buildFilterDto.VerticalName?.Any() == true && buildFilterDto.VerticalName.Count() == 1 && buildFilterDto.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0));
            }
            else if (buildFilterDto.VerticalName?.Any() == true && buildFilterDto.VerticalName.Count() > 1 && buildFilterDto.VerticalName.Contains("yes"))
            {
                var nullVerticals = buildFilterDto.VerticalName.Contains("yes") ?
                    query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0)) : null;
                var verticalFilter = query
                                    .Where(x => x.VerticalFilterDto != null &&
                                    x.VerticalFilterDto.Any(c => buildFilterDto.VerticalName.Where(t => t != "yes").Contains(c.Key.ToString())));
                query = nullVerticals?.Any() == true && verticalFilter?.Any() == true ?
                    nullVerticals.Concat(verticalFilter) : nullVerticals?.Any() == true && verticalFilter
                    ?.Any() == false ? nullVerticals
                    : nullVerticals?.Any() == false && verticalFilter?.Any() == true ? verticalFilter : null;
            }

            List<FilterValueDto> result = propertyName switch
            {
                "budgetLineCode" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.BudgetLineCode)).Distinct().ToList()
                                        : query.Where(x => x.BudgetLineCode.Contains(propertyFilter)).Select(p => new FilterValueDto(p.BudgetLineCode)).Distinct().ToList(),

                "uploadStatus" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.UploadStatus)).Distinct().ToList()
                                        : query.Where(x => x.UploadStatus.Contains(propertyFilter)).Select(p => new FilterValueDto(p.UploadStatus)).Distinct().ToList(),

                "uploadMode" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.UploadMode)).Distinct().ToList()
                                        : query.Where(x => x.UploadMode.Contains(propertyFilter)).Select(p => new FilterValueDto(p.UploadMode)).Distinct().ToList(),

                "currentTrackingNumber" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.CurrentTrackingNumber)).Distinct().ToList()
                                        : query.Where(x => x.CurrentTrackingNumber.Contains(propertyFilter)).Select(p => new FilterValueDto(p.CurrentTrackingNumber)).Distinct().ToList(),

                "newTrackingNumber" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.NewTrackingNumber)).Distinct().ToList()
                                        : query.Where(x => x.NewTrackingNumber.Contains(propertyFilter)).Select(p => new FilterValueDto(p.NewTrackingNumber)).Distinct().ToList(),

                "wbs" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Wbs)).Distinct().ToList()
                                        : query.Where(x => x.Wbs.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Wbs)).Distinct().ToList(),

                "opco" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto { Text = p.Opco, Value = p.OpcoId.ToString() }).Distinct().ToList()
                                        : query.Where(x => x.Opco.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.Opco, Value = p.OpcoId.ToString() }).Distinct().ToList(),

                "domain" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Domain)).Distinct().ToList()
                                        : query.Where(x => x.Domain.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Domain)).Distinct().ToList(),

                "team" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Team)).Distinct().ToList()
                                        : query.Where(x => x.Team.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Team)).Distinct().ToList(),

                "budgetOwner" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.BudgetOwner)).Distinct().ToList()
                                        : query.Where(x => x.BudgetOwner.Contains(propertyFilter)).Select(p => new FilterValueDto(p.BudgetOwner)).Distinct().ToList(),

                "program" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Program)).Distinct().ToList()
                                        : query.Where(x => x.Program.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Program)).Distinct().ToList(),

                "budgetProject" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.BudgetProject)).Distinct().ToList()
                                        : query.Where(x => x.BudgetProject.Contains(propertyFilter)).Select(p => new FilterValueDto(p.BudgetProject)).Distinct().ToList(),

                "activity" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.ToList().Select(p => new FilterValueDto
                                        {
                                            Text = p.Activity,
                                            Value = p.BudgetProjectTackerId.ToString()
                                        }).Distinct().ToList()
                                        : query.ToList()
                                            .Where(x =>
                                               x.Activity.Contains(
                                                    propertyFilter.ToUpper())).Select(p => new FilterValueDto
                                                    {
                                                        Text = p.Activity,
                                                        Value = p.BudgetProjectTackerId.ToString()
                                                    }).Distinct().ToList(),

                "priority" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Priority)).Distinct().ToList()
                                        : query.Where(x => x.Priority.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Priority)).Distinct().ToList(),

                "driver" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Where(x => x.PlannedActivity.DriverId != null && x.PlannedActivity.DriverId != null
                                        && x.PlannedActivity.Driver.BptDriverDetails != null).Select(p => new FilterValueDto
                                        {
                                            Value = Convert.ToString(p.PlannedActivity.DriverId),
                                            Text = p.PlannedActivity.Driver.BptDriverDetails
                                        }).Distinct().ToList()
                                        : query.Where(x => x.PlannedActivity.DriverId != null && x.PlannedActivity.Driver != null
                                        && x.PlannedActivity.Driver.BptDriverDetails != null && x.Driver.Contains(propertyFilter))
                                        .Select(p => new FilterValueDto
                                        {
                                            Value = Convert.ToString(p.PlannedActivity.DriverId),
                                            Text = p.PlannedActivity.Driver.BptDriverDetails
                                        }).Distinct().ToList(),

                "benefits" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Benefits)).Distinct().ToList()
                                        : query.Where(x => x.Benefits.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Benefits)).Distinct().ToList(),

                "risks" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Risks)).Distinct().ToList()
                                        : query.Where(x => x.Risks.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Risks)).Distinct().ToList(),

                "category" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Category)).Distinct().ToList()
                                        : query.Where(x => x.Category.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Category)).Distinct().ToList(),

                "nwelement" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Nwelement)).Distinct().ToList()
                                        : query.Where(x => x.Nwelement.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Nwelement)).Distinct().ToList(),

                "virtualizedNwElement" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.VirtualizedNwElement)).Distinct().ToList()
                                        : query.Where(x => x.VirtualizedNwElement.Contains(propertyFilter)).Select(p => new FilterValueDto(p.VirtualizedNwElement)).Distinct().ToList(),

                "vendor" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Vendor)).Distinct().ToList()
                                        : query.Where(x => x.Vendor.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Vendor)).Distinct().ToList(),

                "lcmCategories" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.LcmCategories)).Distinct().ToList()
                                        : query.Where(x => x.LcmCategories.Contains(propertyFilter)).Select(p => new FilterValueDto(p.LcmCategories)).Distinct().ToList(),

                "ohpLev1" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.OhpLev1)).Distinct().ToList()
                                        : query.Where(x => x.OhpLev1.Contains(propertyFilter)).Select(p => new FilterValueDto(p.OhpLev1)).Distinct().ToList(),

                "ohpLev2" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.OhpLev2)).Distinct().ToList()
                                        : query.Where(x => x.OhpLev2.Contains(propertyFilter)).Select(p => new FilterValueDto(p.OhpLev2)).Distinct().ToList(),

                "hfmLev1" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.HfmLev1)).Distinct().ToList()
                                        : query.Where(x => x.HfmLev1.Contains(propertyFilter)).Select(p => new FilterValueDto(p.HfmLev1)).Distinct().ToList(),

                "hfmLev2" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.HfmLev2)).Distinct().ToList()
                                        : query.Where(x => x.HfmLev2.Contains(propertyFilter)).Select(p => new FilterValueDto(p.HfmLev2)).Distinct().ToList(),

                "fy" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Fy)).Distinct().ToList()
                                        : query.Where(x => x.Fy.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Fy)).Distinct().ToList(),

                "operational" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Operational)).Distinct().ToList()
                                        : query.Where(x => x.Operational.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Operational)).Distinct().ToList(),

                "transfers" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Transfers)).Distinct().ToList()
                                        : query.Where(x => x.Transfers.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Transfers)).Distinct().ToList(),

                "cost1sTest" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Cost1sTest)).Distinct().ToList()
                                        : query.Where(x => x.Cost1sTest.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Cost1sTest)).Distinct().ToList(),

                "validation" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Validation)).Distinct().ToList()
                                        : query.Where(x => x.Validation.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Validation)).Distinct().ToList(),

                "signOff" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.SignOff)).Distinct().ToList()
                                        : query.Where(x => x.SignOff.Contains(propertyFilter)).Select(p => new FilterValueDto(p.SignOff)).Distinct().ToList(),

                "sub" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Sub)).Distinct().ToList()
                                        : query.Where(x => x.Sub.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Sub)).Distinct().ToList(),

                "finalReSub" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.FinalReSub)).Distinct().ToList()
                                        : query.Where(x => x.FinalReSub.Contains(propertyFilter)).Select(p => new FilterValueDto(p.FinalReSub)).Distinct().ToList(),

                "latestScenario" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.LatestScenario)).Distinct().ToList()
                                        : query.Where(x => x.LatestScenario.Contains(propertyFilter)).Select(p => new FilterValueDto(p.LatestScenario)).Distinct().ToList(),

                "currency" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Currency)).Distinct().ToList()
                                        : query.Where(x => x.Currency.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Currency)).Distinct().ToList(),

                "adjustments" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Adjustments)).Distinct().ToList()
                                        : query.Where(x => x.Adjustments.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Adjustments)).Distinct().ToList(),

                "ytdActuals" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.YtdActuals)).Distinct().ToList()
                                        : query.Where(x => x.YtdActuals.Contains(propertyFilter)).Select(p => new FilterValueDto(p.YtdActuals)).Distinct().ToList(),

                "plannedAbsorption" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.PlannedAbsorption)).Distinct().ToList()
                                        : query.Where(x => x.PlannedAbsorption.Contains(propertyFilter)).Select(p => new FilterValueDto(p.PlannedAbsorption)).Distinct().ToList(),

                "deviation" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Deviation)).Distinct().ToList()
                                        : query.Where(x => x.Deviation.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Deviation)).Distinct().ToList(),

                "apr" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Apr)).Distinct().ToList()
                                        : query.Where(x => x.Apr.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Apr)).Distinct().ToList(),

                "may" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.May)).Distinct().ToList()
                                        : query.Where(x => x.May.Contains(propertyFilter)).Select(p => new FilterValueDto(p.May)).Distinct().ToList(),

                "jun" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Jun)).Distinct().ToList()
                                        : query.Where(x => x.Jun.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Jun)).Distinct().ToList(),

                "jul" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Jul)).Distinct().ToList()
                                        : query.Where(x => x.Jul.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Jul)).Distinct().ToList(),

                "aug" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Aug)).Distinct().ToList()
                                        : query.Where(x => x.Aug.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Aug)).Distinct().ToList(),

                "sep" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Sep)).Distinct().ToList()
                                        : query.Where(x => x.Sep.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Sep)).Distinct().ToList(),

                "oct" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Oct)).Distinct().ToList()
                                        : query.Where(x => x.Oct.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Oct)).Distinct().ToList(),

                "nov" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Nov)).Distinct().ToList()
                                        : query.Where(x => x.Nov.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Nov)).Distinct().ToList(),

                "dec" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Dec)).Distinct().ToList()
                                        : query.Where(x => x.Dec.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Dec)).Distinct().ToList(),

                "jan" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Jan)).Distinct().ToList()
                                        : query.Where(x => x.Jan.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Jan)).Distinct().ToList(),

                "feb" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Feb)).Distinct().ToList()
                                        : query.Where(x => x.Feb.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Feb)).Distinct().ToList(),

                "mar" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Mar)).Distinct().ToList()
                                        : query.Where(x => x.Mar.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Mar)).Distinct().ToList(),

                "approvedBudget" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.ApprovedBudget)).Distinct().ToList()
                                        : query.Where(x => x.ApprovedBudget.Contains(propertyFilter)).Select(p => new FilterValueDto(p.ApprovedBudget)).Distinct().ToList(),

                "commitment" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Commitment)).Distinct().ToList()
                                        : query.Where(x => x.Commitment.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Commitment)).Distinct().ToList(),

                "budgetProjectDependency" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.BudgetProjectDependency)).Distinct().ToList()
                                        : query.Where(x => x.BudgetProjectDependency.Contains(propertyFilter)).Select(p => new FilterValueDto(p.BudgetProjectDependency)).Distinct().ToList(),

                "localProgram" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.LocalProgram)).Distinct().ToList()
                                        : query.Where(x => x.LocalProgram.Contains(propertyFilter)).Select(p => new FilterValueDto(p.LocalProgram)).Distinct().ToList(),

                "localBudgetProject" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.LocalBudgetProject)).Distinct().ToList()
                                        : query.Where(x => x.LocalBudgetProject.Contains(propertyFilter)).Select(p => new FilterValueDto(p.LocalBudgetProject)).Distinct().ToList(),

                "localDriver" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.LocalDriver)).Distinct().ToList()
                                        : query.Where(x => x.LocalDriver.Contains(propertyFilter)).Select(p => new FilterValueDto(p.LocalDriver)).Distinct().ToList(),

                "localPrioritization" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.LocalPrioritization)).Distinct().ToList()
                                        : query.Where(x => x.LocalPrioritization.Contains(propertyFilter)).Select(p => new FilterValueDto(p.LocalPrioritization)).Distinct().ToList(),

                "ppmId" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.PpmId)).Distinct().ToList()
                                        : query.Where(x => x.PpmId.Contains(propertyFilter)).Select(p => new FilterValueDto(p.PpmId)).Distinct().ToList(),

                "ppmBudgetProjectId" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.PpmBudgetProjectId)).Distinct().ToList()
                                        : query.Where(x => x.PpmBudgetProjectId.Contains(propertyFilter)).Select(p => new FilterValueDto(p.PpmBudgetProjectId)).Distinct().ToList(),

                "costCentre" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.CostCentre)).Distinct().ToList()
                                        : query.Where(x => x.CostCentre.Contains(propertyFilter)).Select(p => new FilterValueDto(p.CostCentre)).Distinct().ToList(),

                "wpId" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.WpId)).Distinct().ToList()
                                        : query.Where(x => x.WpId.Contains(propertyFilter)).Select(p => new FilterValueDto(p.WpId)).Distinct().ToList(),

                "groupBudgetOpcoId" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.GroupBudgetOpcoId)).Distinct().ToList()
                                        : query.Where(x => x.GroupBudgetOpcoId.Contains(propertyFilter)).Select(p => new FilterValueDto(p.GroupBudgetOpcoId)).Distinct().ToList(),

                "internalProgram" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.InternalProgram)).Distinct().ToList()
                                        : query.Where(x => x.InternalProgram.Contains(propertyFilter)).Select(p => new FilterValueDto(p.InternalProgram)).Distinct().ToList(),

                "verticalProject" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.VerticalProject)).Distinct().ToList()
                                        : query.Where(x => x.VerticalProject.Contains(propertyFilter)).Select(p => new FilterValueDto(p.VerticalProject)).Distinct().ToList(),

                "domainSpecificLabels" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.DomainSpecificLabels)).Distinct().ToList()
                                        : query.Where(x => x.DomainSpecificLabels.Contains(propertyFilter)).Select(p => new FilterValueDto(p.DomainSpecificLabels)).Distinct().ToList(),

                "labelsMarketVsVertical" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.LabelsMarketVsVertical)).Distinct().ToList()
                                        : query.Where(x => x.LabelsMarketVsVertical.Contains(propertyFilter)).Select(p => new FilterValueDto(p.LabelsMarketVsVertical)).Distinct().ToList(),

                "otherMinorVendors" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.OtherMinorVendors)).Distinct().ToList()
                                        : query.Where(x => x.OtherMinorVendors.Contains(propertyFilter)).Select(p => new FilterValueDto(p.OtherMinorVendors)).Distinct().ToList(),

                "externalDemandBudget" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.ExternalDemandBudget)).Distinct().ToList()
                                        : query.Where(x => x.ExternalDemandBudget.Contains(propertyFilter)).Select(p => new FilterValueDto(p.ExternalDemandBudget)).Distinct().ToList(),

                "opexImpact" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.OpexImpact)).Distinct().ToList()
                                        : query.Where(x => x.OpexImpact.Contains(propertyFilter)).Select(p => new FilterValueDto(p.OpexImpact)).Distinct().ToList(),

                "legalEntity" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.LegalEntity)).Distinct().ToList()
                                        : query.Where(x => x.LegalEntity.Contains(propertyFilter)).Select(p => new FilterValueDto(p.LegalEntity)).Distinct().ToList(),

                "yearlyTransfersTrack" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.YearlyTransfersTrack)).Distinct().ToList()
                                        : query.Where(x => x.YearlyTransfersTrack.Contains(propertyFilter)).Select(p => new FilterValueDto(p.YearlyTransfersTrack)).Distinct().ToList(),

                "yearlyAdjustmentsTrack" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.YearlyAdjustmentsTrack)).Distinct().ToList()
                                        : query.Where(x => x.YearlyAdjustmentsTrack.Contains(propertyFilter)).Select(p => new FilterValueDto(p.YearlyAdjustmentsTrack)).Distinct().ToList(),

                "mcustom02" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Mcustom02)).Distinct().ToList()
                                        : query.Where(x => x.Mcustom02.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Mcustom02)).Distinct().ToList(),

                "mcustom03" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Mcustom03)).Distinct().ToList()
                                        : query.Where(x => x.Mcustom03.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Mcustom03)).Distinct().ToList(),

                "mcustom04" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Mcustom04)).Distinct().ToList()
                                        : query.Where(x => x.Mcustom04.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Mcustom04)).Distinct().ToList(),

                "mcustom05" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Mcustom05)).Distinct().ToList()
                                        : query.Where(x => x.Mcustom05.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Mcustom05)).Distinct().ToList(),

                "mcustom06" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Mcustom06)).Distinct().ToList()
                                        : query.Where(x => x.Mcustom06.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Mcustom06)).Distinct().ToList(),

                "mcustom07" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Mcustom07)).Distinct().ToList()
                                        : query.Where(x => x.Mcustom07.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Mcustom07)).Distinct().ToList(),

                "mcustom08" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Mcustom08)).Distinct().ToList()
                                        : query.Where(x => x.Mcustom08.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Mcustom08)).Distinct().ToList(),

                "mcustom09" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Mcustom09)).Distinct().ToList()
                                        : query.Where(x => x.Mcustom09.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Mcustom09)).Distinct().ToList(),

                "mcustom10" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Mcustom10)).Distinct().ToList()
                                        : query.Where(x => x.Mcustom10.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Mcustom10)).Distinct().ToList(),

                "vcustom01" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Vcustom01)).Distinct().ToList()
                                        : query.Where(x => x.Vcustom01.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Vcustom01)).Distinct().ToList(),

                "vcustom02" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Vcustom02)).Distinct().ToList()
                                        : query.Where(x => x.Vcustom02.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Vcustom02)).Distinct().ToList(),

                "vcustom03" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Vcustom03)).Distinct().ToList()
                                        : query.Where(x => x.Vcustom03.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Vcustom03)).Distinct().ToList(),

                "vcustom04" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Vcustom04)).Distinct().ToList()
                                        : query.Where(x => x.Vcustom04.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Vcustom04)).Distinct().ToList(),

                "vcustom05" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Vcustom05)).Distinct().ToList()
                                        : query.Where(x => x.Vcustom05.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Vcustom05)).Distinct().ToList(),

                "dcustom01" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Dcustom01)).Distinct().ToList()
                                        : query.Where(x => x.Dcustom01.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Dcustom01)).Distinct().ToList(),

                "dcustom02" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Dcustom02)).Distinct().ToList()
                                        : query.Where(x => x.Dcustom02.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Dcustom02)).Distinct().ToList(),

                "dcustom03" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Dcustom03)).Distinct().ToList()
                                        : query.Where(x => x.Dcustom03.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Dcustom03)).Distinct().ToList(),

                "dcustom04" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Dcustom04)).Distinct().ToList()
                                        : query.Where(x => x.Dcustom04.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Dcustom04)).Distinct().ToList(),

                "dcustom05" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Dcustom05)).Distinct().ToList()
                                        : query.Where(x => x.Dcustom05.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Dcustom05)).Distinct().ToList(),

                "lsdb" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Lsdb)).Distinct().ToList()
                                        : query.Where(x => x.Lsdb.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Lsdb)).Distinct().ToList(),

                "ls012" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Ls012)).Distinct().ToList()
                                        : query.Where(x => x.Ls012.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Ls012)).Distinct().ToList(),

                "ls210" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Ls210)).Distinct().ToList()
                                        : query.Where(x => x.Ls210.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Ls210)).Distinct().ToList(),

                "ls57" => string.IsNullOrEmpty(propertyFilter)
                                        ? query.Select(p => new FilterValueDto(p.Ls57)).Distinct().ToList()
                                        : query.Where(x => x.Ls57.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Ls57)).Distinct().ToList(),
                "verticalName" => query.AsEnumerable().Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0).SelectMany(p => p.VerticalFilterDto.Select(m =>
                                   new FilterValueDto
                                   {
                                       Value = m.Key.ToString(),
                                       Text = m.Value
                                   }))?.Distinct()?.ToList()
                                   .Concat(
                                       (query.AsEnumerable().Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count == 0))
                                       .Select(x =>
                                               _commonManager.AddBlankFilterValue()
                                       )
                                   )
                                   .Distinct().ToList(),




                _ => new List<FilterValueDto>()
            };
            if (!isAdmin && (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count > 0) && propertyName == "verticalName")
            {
                result = result.Where(x => buildFilterDto.VerticalName.Contains(x.Value.ToString())).ToList();
            }
            return result;

        }
        #endregion

        #region Edit and Update BPT  
        public async Task<BuildBagEditPageDto> GetEditedBPTPageDetailsAsync(long id)
        {
            return null;
        }
        public async Task<ResultDto> UpdateProjectTrackerAsync(BuildBagUpdateDto dto, bool? forced)
        {
            return null;
        }

        public async Task<ResultDto> UpdateBaseAsync(BuildBagUpdateDto dto, bool? forced)
        {
            return null;
        }

        #endregion

        #region Dataload

        private IQueryable<PlannedActivity> GetQuery()
        {

            var result = _repositoryWrapper.PlannedActivity.FindAll()
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                    .Include(x => x.Networkelementasplanned)
                    .Include(x => x.Networkelementasplanned).ThenInclude(x => x.Opco)
                    .Include(x => x.Networkelementasplanned).ThenInclude(x => x.Networkelementasplannedsubdomainspoc)
                    .Include(x => x.Networkelementasplanned).ThenInclude(x => x.Networkelementasplannededuspoc)
                    .Include(x => x.Designaspect).ThenInclude(x => x.Opco)
                    .Include(x => x.Lcmengineering)
                    .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                    .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc)
                    .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringeduspoc)
                    .Include(x => x.Opco)
                    .Include(x => x.Plannedactivityresource)
                    .Include(x => x.Plannedactivitycategory)
                    .Include(x => x.ProgramNavigation)
                    .Include(x => x.Driver)
                    .Include(x => x.Benefit)
                    .Include(x => x.Engineeringrisk);

            var data = result.AsEnumerable().Select(p => PlannedActivityMapper.Get(p)).ToList();

            return data.AsQueryable();
        }

        public async Task<ResultDto> BPTDataCreation()
        {
            try
            {
                var bptCreateDataList = new List<Budgetprojecttrackers>();
                var bptUpdateDataList = new List<Budgetprojecttrackers>();

                var query = await Task.Run(() => GetQuery());

                var bPTDatas = query.ToList().Select(x => new Budgetprojecttrackers
                {
                    Plannedactivityid = x.PlannedActivityId,
                    Currenttrackingnumber = x.BudgetTrackingId,
                    Wbs = _commonManager.GetWBSCode(x.DeliveryProjectName),
                    Opco = x.OpCo?.OpCoDescription,
                    Opcoid = x.OpCoId,
                    Domain = _commonManager.GetOrganisationNameForBpt(x, true),//c&s
                    Team = "CES_EDU",
                    Budgetowner = _commonManager.GetBudgetOwnerForBpt(x.ProjectOwner, x),
                    Program = x?.ProgramNavigation?.ProgramDescription,
                    Budgetproject = x.DeliveryProjectName,
                    //Activity = $"{x.PlannedActivityResource?.PlannedActivityResourceDescription} - {x.ActivityDetails}",
                    Priority = x.Priority,
                    Driver = x.Driver?.BptDriverDetails,
                    Benefits = x.Benefit?.BenefitDescription,
                    Risks = x.EngineeringRisk?.RiskDescription,
                    Category = x.PlannedactivitycategoryNavigation?.Categorydescription,
                    Categoryid = x.Plannedactivitycategoryid,
                    Nwelement = x.DesignComponent?.SystemType?.MajorSoftwareBuilds?.ProductName?.Description,
                    Virtualizednwelement = _commonManager.GetVirtualized(x.OriginalDesignComponentIndex, x.DesignComponentId, x.DesignComponent?.SystemType?.SystemTypesMajorHardwareBuilds?.FirstOrDefault().MajorHardware?.BuildConstruction?.IsCluodHostedAsset,
                    x.DesignComponentFamilyId, x.DesignComponent?.SystemType?.SystemTypesMajorHardwareBuilds?.FirstOrDefault().MajorHardware?.BuildConstruction?.Rule),
                    Lcmcategoriesid = x.Lcmcategories,
                    Vendor = x.DesignComponent?.SystemType?.MajorSoftwareBuilds?.OriginalEquipmentManufacturer?.OriginalEquipmentManufacturerDescription,
                    Lcmcategories = _commonManager.LcmCategoryStatust(x.Lcmcategories),
                    Archive = x.Archived
                });

                foreach (var bpt in bPTDatas)
                {
                    var bptDataExist = _repositoryWrapper.BudgetProjectTrackersRepository.FindByCondition(x => x.Plannedactivityid == bpt.Plannedactivityid).FirstOrDefault();
                    if (bptDataExist != null)
                    {
                        var updateList = await Task.Run(() => BptUpdate(bpt, bptDataExist));
                        bptUpdateDataList.Add(updateList);
                    }
                    else
                    {
                        bptCreateDataList.Add(bpt);
                    }


                }

                if (bptUpdateDataList.Count > 0)
                {
                    _repositoryWrapper.BudgetProjectTrackersRepository.BulkUpdate(bptUpdateDataList);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }

                if (bptCreateDataList.Count > 0)
                {
                    _repositoryWrapper.BudgetProjectTrackersRepository.BulkCreate(bptCreateDataList);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }


                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess,

                };
            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Info = ResultMessages.SystemError,
                    Warning = true,
                    Data = ex.Message
                };
            }
        }

        private Budgetprojecttrackers BptUpdate(Budgetprojecttrackers newEnity, Budgetprojecttrackers existingEntity)
        {
            existingEntity.Plannedactivityid = newEnity.Plannedactivityid;
            existingEntity.Currenttrackingnumber = newEnity.Currenttrackingnumber;
            existingEntity.Wbs = newEnity.Wbs;
            existingEntity.Opco = newEnity.Opco;
            existingEntity.Opcoid = newEnity.Opcoid;
            existingEntity.Domain = newEnity.Domain;
            existingEntity.Team = newEnity.Team;
            existingEntity.Budgetowner = newEnity.Budgetowner;
            existingEntity.Program = newEnity.Program;
            existingEntity.Budgetproject = newEnity.Budgetproject;
            existingEntity.Activity = newEnity.Activity;
            existingEntity.Priority = newEnity.Priority;
            existingEntity.Driver = newEnity.Driver;
            existingEntity.Benefits = newEnity.Benefits;
            existingEntity.Risks = newEnity.Risks;
            existingEntity.Category = newEnity.Category;
            existingEntity.Nwelement = newEnity.Nwelement;
            existingEntity.Virtualizednwelement = newEnity.Virtualizednwelement;
            existingEntity.Vendor = newEnity.Vendor;
            existingEntity.Lcmcategories = newEnity.Lcmcategories;
            existingEntity.Archive = newEnity.Archive;



            return existingEntity;
        }
        #endregion

    }

}