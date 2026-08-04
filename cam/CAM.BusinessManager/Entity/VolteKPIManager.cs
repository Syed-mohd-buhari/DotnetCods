using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.ExtensionMethod.VolteKPI;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.VolteKPI;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using CAM.Infrastucture;
using CAM.Infrastucture.Enums;
using CAM.Mail;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Threading.Tasks;
using CAM.Entities.Models.Mail;
using Microsoft.EntityFrameworkCore.Query;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using OracleModels.DBModels;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.Entity
{
    public partial class VolteKPIManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private GridCustomColumnManager _manager;
        private readonly ICurrentUserService _currentUserService;
        private readonly bool _isKPIAdmin;
        private readonly List<short> _currentUserOpCos;
        private readonly IEnumerable<OpCo> _opcoResource;
        private readonly IMailManager _mailManager;
        private readonly UserManager _userManager;


        private readonly List<short> _opcoList; 
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly bool _adminRoleCheck=false;

        public VolteKPIManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
            ICurrentUserService currentUserService, IRepositoryWrapper repositoryWrapper
            , IConfiguration configuration, IMailManager mailManager, UserManager userManager,
            IHttpContextAccessor contextAccessor, AuthorizedRoleManager authorizedRoleManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;

            _mapper = mapper;
            _manager = manager;
            _configuration = configuration;
            _currentUserService = currentUserService;
            _mailManager = mailManager;
            _userManager = userManager;
            this._isKPIAdmin = _currentUserService.UserInRole(ConstantValueFilter.KPIAdministrator, false);
            this._currentUserOpCos = _userManager.GetUserClaim(_currentUserService.UserId, ConstantValueFilter.OpCo);
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;


            //var opCoResource =  _repositoryWrapper.OpCo.
            //       FindByCondition(x => _isKPIAdmin || _currentUserOpCos.Contains(x.Opcoid))
            //       ;

            var opCoResource = _repositoryWrapper.OpCo.FindAll();

            this._opcoResource = (_opcoList != null && _opcoList.Count>0) ? opCoResource.Where(x => _opcoList.Contains(x.Opcoid))
                .Select(p => OpCoMapper.GetOpCoMapper(p)) :
            opCoResource.Select(p => OpCoMapper.GetOpCoMapper(p));

            //this._opcoResource = opCoResource.Select(p => OpCoMapper.GetOpCoMapper(p));

            //this._opcoResource = _repositoryWrapper.OpCo.FindAll();
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.VolteKPI.FindByCondition(x => x.Voltekpiid == id)
                .SingleAsync();

            _repositoryWrapper.VolteKPI.Delete(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Voltekpiid
            };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.VolteKPI.FindByCondition(x => x.Voltekpiid == id)
                .SingleAsync();

            _repositoryWrapper.VolteKPI.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Voltekpiid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = rm
                };
            }
            else
                return new ResultDto();
        }

        public async Task<ResultDto> Restore(long id)
        {

            var entity = await _repositoryWrapper.VolteKPI.FindByCondition(x => x.Voltekpiid == id, true).SingleAsync();

            entity.Deleted = false;
            entity.Deletiondate = null;

            _repositoryWrapper.VolteKPI.Update(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.Voltekpiid
            };
        }

        public async Task<ResultDto> Add(VolteKPIDtoCreate dto, string hostName, bool? forced = false)
        {
            var entityExists = await _repositoryWrapper.VolteKPI.FindByCondition(
                    x => x.Opcoid == dto.OpCoId &&
                         x.Month == dto.Month &&
                         x.Year == dto.Year
                    , true)
                .OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();

            ResultDto retValue = null;
            if (entityExists != null)
            {
                retValue = await UpdateBase(dto, forced);
            }
            else
            {
                retValue = await AddBase(dto);
            }
            if (!retValue.Warning)
            {
                //Add Worklog
                var newWorklog = new VolteKPIWorklogDto
                {
                    VolteKPIWorklogId = null,
                    VolteKPIId = (long)retValue.Data,
                    OpCoId = dto.OpCoId,
                    Month = dto.Month,
                    Year = dto.Year,
                    VolteKPIType = (int)dto.VolteKPIType,
                    OpCo = dto.OpCoResource[dto.OpCoId],
                    KpiIdName = dto.VolteKPIType.GetDescription(),
                    MonthYear = $"{dto.Month.ToString("00")}/{dto.Year.ToString()}",
                    TargetMonthlyValueOld = null,
                    TargetMonthlyValueProposed = null,
                    TargetMonthlyValueNew = null,
                    EoyTargetOld = null,
                    EoyTargetNew = null,
                    ActualMonthlyValueOld = null,
                    ActualMonthlyValueNew = null,
                    ActualNumberOfRegisteredOld = null,
                    ActualNumberOfRegisteredNew = null,
                    ActualNumberOfProvisionedOld = null,
                    ActualNumberOfProvisionedNew = null,
                    Comments = null,
                    Approved = null,
                    SubmissionDate = DateTime.Now,
                    SubmittedBy = _currentUserService.EMail,
                    IsStored = true
                };
                switch (dto.VolteKPIType)
                {
                    case VolteKPIType.KPI1_Capacity:
                        if (dto.KPIOneTargetValueChangeProposal != null)
                        {
                            newWorklog.TargetMonthlyValueProposed = dto.KPIOneTargetValueChangeProposal;
                            newWorklog.TargetMonthlyValueOld = entityExists?.Kpionemonthlytarget;
                            newWorklog.TargetMonthlyValueNew = dto.KPIOneMonthlyTarget;
                        }
                        else if (dto.KPIOneMonthlyTarget != null && (entityExists?.Kpionemonthlytarget != dto.KPIOneMonthlyTarget))
                        {
                            newWorklog.TargetMonthlyValueOld = entityExists?.Kpionemonthlytarget;
                            newWorklog.TargetMonthlyValueNew = dto.KPIOneMonthlyTarget;
                        }
                        if (dto.KPIOneEoYTarget != null && (entityExists?.Kpioneeoytarget != dto.KPIOneEoYTarget))
                        {
                            newWorklog.EoyTargetOld = entityExists?.Kpioneeoytarget;
                            newWorklog.EoyTargetNew = dto.KPIOneEoYTarget;
                        }
                        if (dto.KPIOneActualValue != null && (entityExists?.Kpioneactualvalue != dto.KPIOneActualValue))
                        {
                            newWorklog.ActualMonthlyValueOld = entityExists?.Kpioneactualvalue;
                            newWorklog.ActualMonthlyValueNew = dto.KPIOneActualValue;
                        }
                        newWorklog.Comments = dto.KPIOneComment;
                        newWorklog.IsStored = dto.KPIOneTargetValueChangeProposal == null;
                        break;
                    case VolteKPIType.KPI2_Current_Utilization:
                        if (dto.KPITwoActualNumberOfRegisteredSubscribers != null && (entityExists?.Kpi2actualnoofregsubsc != dto.KPITwoActualNumberOfRegisteredSubscribers))
                        {
                            newWorklog.ActualNumberOfRegisteredOld = entityExists?.Kpi2actualnoofregsubsc;
                            newWorklog.ActualNumberOfRegisteredNew = dto.KPITwoActualNumberOfRegisteredSubscribers;
                        }
                        if (dto.KPITwoActualNumberOfProvisionedSubscriber != null && (entityExists?.Kpi2actualnoofprovisionedsubsc != dto.KPITwoActualNumberOfProvisionedSubscriber))
                        {
                            newWorklog.ActualNumberOfProvisionedOld = entityExists?.Kpi2actualnoofprovisionedsubsc;
                            newWorklog.ActualNumberOfProvisionedNew = dto.KPITwoActualNumberOfProvisionedSubscriber;
                        }
                        newWorklog.Comments = dto.KPITwoComment;
                        newWorklog.IsStored = true;
                        break;
                    case VolteKPIType.KPI3_Penetration:
                        if (dto.KPIThreeTargetValueChangeProposal != null)
                        {
                            newWorklog.TargetMonthlyValueProposed = dto.KPIThreeTargetValueChangeProposal;
                            newWorklog.TargetMonthlyValueOld = entityExists?.Kpithreemonthlytarget;
                            newWorklog.TargetMonthlyValueNew = dto.KPIThreeMonthlyTarget;
                        }
                        else if (dto.KPIThreeMonthlyTarget != null && (entityExists?.Kpithreemonthlytarget != dto.KPIThreeMonthlyTarget))
                        {
                            newWorklog.TargetMonthlyValueOld = entityExists?.Kpithreemonthlytarget;
                            newWorklog.TargetMonthlyValueNew = dto.KPIThreeMonthlyTarget;
                        }
                        if (dto.KPIThreeEoYTarget != null && (entityExists?.Kpithreeeoytarget != dto.KPIThreeEoYTarget))
                        {
                            newWorklog.EoyTargetOld = entityExists?.Kpithreeeoytarget;
                            newWorklog.EoyTargetNew = dto.KPIThreeEoYTarget;
                        }
                        if (dto.KPIThreeActualValue != null && (entityExists?.Kpithreeactualvalue != dto.KPIThreeActualValue))
                        {
                            newWorklog.ActualMonthlyValueOld = entityExists?.Kpithreeactualvalue;
                            newWorklog.ActualMonthlyValueNew = dto.KPIThreeActualValue;
                        }
                        newWorklog.Comments = dto.KPIThreeComment;
                        newWorklog.IsStored = !(dto.KPIThreeTargetValueChangeProposal != null && (entityExists == null || entityExists.Kpi3targetvluchngproposal == null));
                        break;
                    case VolteKPIType.KPI4_3G_ShutDown:
                        break;
                }

                retValue = await Add(newWorklog, hostName, true);




            }
            return retValue;

        }

        public async Task<ResultDto> AddBase(VolteKPIDtoCreate dto)
        {
            var entity = _mapper.Map<VolteKPI>(dto);
            var model = VolteKPIMapper.Set(entity);
            _repositoryWrapper.VolteKPI.Create(model);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess, Data = model.Voltekpiid };

        }

        //public async Task<ResultDto> Update(VolteKPIDtoUpdate dto, bool? forced = false)
        //{
        //    //var anotherEntityWithSameNaturalKeyExists = await _repositoryWrapper.VolteKPI.FindByCondition(
        //    //        x => x.OpCoId == dto.OpCoId &&
        //    //             x.Month == dto.Month &&
        //    //             x.Year == dto.Year &&
        //    //             x.VolteKPIId != dto.VolteKPIId
        //    //        , true)
        //    //    .OrderByDescending(x => x.CreationDate).FirstOrDefaultAsync();

        //    var originalEntityWithSameNaturalKey = await _repositoryWrapper.VolteKPI.FindByCondition(
        //            x => x.OpCoId == dto.OpCoId &&
        //                 x.Month == dto.Month &&
        //                 x.Year == dto.Year &&
        //                 x.VolteKPIId == dto.VolteKPIId
        //            , true).SingleOrDefaultAsync();

        //    if (originalEntityWithSameNaturalKey == null)
        //    {
        //        return new ResultDto
        //        {
        //            Warning = true,
        //            Info = ResultMessages.EntryUpdateNotExists,
        //            Data = dto.VolteKPIId
        //        };
        //    }

        //    //if (anotherEntityWithSameNaturalKeyExists != null && originalEntityWithSameNaturalKey == null)
        //    //{
        //    //    if (anotherEntityWithSameNaturalKeyExists.Deleted == true)
        //    //    {
        //    //        if (forced == true)
        //    //        {
        //    //            return await UpdateBase(dto, forced);
        //    //        }
        //    //        else
        //    //        {
        //    //            return new ResultDto
        //    //            {
        //    //                Warning = true,
        //    //                Info = ResultMessages.EntryUpdateExists,
        //    //                Data = new { id = anotherEntityWithSameNaturalKeyExists.VolteKPIId, orphanDeleted = true }
        //    //            };
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        return new ResultDto
        //    //        {
        //    //            Warning = true,
        //    //            Info = anotherEntityWithSameNaturalKeyExists.Deleted ? ResultMessages.EntryUpdateExistsDeleted : ResultMessages.EntryUpdateExists,
        //    //            Data = anotherEntityWithSameNaturalKeyExists.VolteKPIId
        //    //        };
        //    //    }
        //    //}


        //    return await UpdateBase(dto, forced);
        //}

        private async Task<ResultDto> UpdateBase(VolteKPIDtoCreate dto, bool? forced)
        {
            var entity = _mapper.Map<VolteKPI>(dto);

            if (forced == true)
            {
                entity.Deleted = false;
                entity.DeletionDate = null;
            }
            _repositoryWrapper.VolteKPI.Update(VolteKPIMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.VolteKPIId
            };
        }


        public VolteKPIDtoCreate GetCreatePage(VolteKPIQueryDto volteKPIQueryDto = null)
        {

            VolteKPIDtoCreate model = new VolteKPIDtoCreate()
            {
                KPIOneActualValue = null,
                KPIOneComment = "",
                KPIOneEoYTarget = null,
                KPIOneMonthlyTarget = null,
                KPIOneTargetValueChangeProposal = null,
                //KPITwoActualValue = null,
                KPITwoComment = "",
                KPITwoActualNumberOfRegisteredSubscribers = null,
                KPITwoActualNumberOfProvisionedSubscriber = null,
                //KPITwoTargetValueChangeProposal = null,
                KPIThreeActualValue = null,
                KPIThreeComment = "",
                KPIThreeEoYTarget = null,
                KPIThreeMonthlyTarget = null,
                KPIThreeTargetValueChangeProposal = null,
                KPIFourActualMonthly = null,
                KPIFourComment = "",
                KPIFourFinalTarget = null,
                KPIFourTargetMonthly = null,
                KPIFourTargetDateMonth = null,
                KPIFourTargetDateYear = null,
                KPIFourTargetValueChangeProposal = null
            };
            if (volteKPIQueryDto != null && volteKPIQueryDto.OpCo != null && volteKPIQueryDto.OpCo.Count == 1)
            {
                var entity = _repositoryWrapper.VolteKPI.FindByCondition(x =>
                    x.Month == volteKPIQueryDto.Month &&
                    x.Year == volteKPIQueryDto.Year &&
                    x.Opcoid == volteKPIQueryDto.OpCo[0]
                    )
                    .Include(x => x.ModificationuserNavigation)
                    .SingleOrDefault();
                if (entity != null)
                {
                    var mappedObj = VolteKPIMapper.Get(entity);
                    model = _mapper.Map<VolteKPIDtoCreate>(mappedObj);
                }
                else
                {
                    model.OpCoId = volteKPIQueryDto.OpCo[0];
                    model.Year = volteKPIQueryDto.Year.Value;
                    model.Month = volteKPIQueryDto.Month.Value;
                }
            }
            else if (volteKPIQueryDto.VolteKPIId != null && volteKPIQueryDto.VolteKPIId.Count == 1)
            {
                var entity = _repositoryWrapper.VolteKPI.FindByCondition(x => x.Voltekpiid == volteKPIQueryDto.VolteKPIId[0])
                    .Include(x => x.ModificationuserNavigation)
                    .Single();
                var mappedObj = VolteKPIMapper.Get(entity);
                model = _mapper.Map<VolteKPIDtoCreate>(mappedObj);
            }
            model.KPIOneComment = "";
            model.KPITwoComment = "";
            model.KPIThreeComment = "";
            model.KPIFourComment = "";

            model.KPIOneTargetValueChangeProposal = null;
            model.KPIThreeTargetValueChangeProposal = null;
            model.KPIFourTargetValueChangeProposal = null;

            var data = _repositoryWrapper.VolteKPI.FindByCondition(x =>
                    (!(model.Month >= 4 && model.Month <= 12) || (x.Year == model.Year && x.Month >= 4 && x.Month < model.Month)) &&
                    (!(model.Month >= 1 && model.Month <= 3) || (x.Year == model.Year - 1 && x.Month >= 4) || (x.Year == model.Year && x.Month < model.Month)) &&
                    (model.OpCoId == x.Opcoid)
                )
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).ToList();
            if (data != null && data.Count > 0)
            {
                if (model.KPIOneEoYTarget == null || model.KPIOneEoYTarget <= 0)
                {
                    model.KPIOneEoYTarget = data.FirstOrDefault(x => x.Kpioneeoytarget.HasValue && x.Kpioneeoytarget > 0)?.Kpioneeoytarget;
                }
                if (model.KPIThreeEoYTarget == null || model.KPIThreeEoYTarget <= 0)
                {
                    model.KPIThreeEoYTarget = data.FirstOrDefault(x => x.Kpithreeeoytarget.HasValue && x.Kpithreeeoytarget > 0)?.Kpithreeeoytarget;
                }
                if (model.KPIFourFinalTarget == null || model.KPIFourFinalTarget <= 0)
                {
                    model.KPIFourFinalTarget = data.FirstOrDefault(x => x.Kpifourfinaltarget.HasValue && x.Kpifourfinaltarget > 0)?.Kpifourfinaltarget;
                }

                if (model.KPIOneMonthlyTarget == null || model.KPIOneMonthlyTarget <= 0)
                {
                    model.KPIOneMonthlyTarget = data.FirstOrDefault(x => x.Kpionemonthlytarget.HasValue && x.Kpionemonthlytarget > 0)?.Kpionemonthlytarget;
                }
                if (model.KPIThreeMonthlyTarget == null || model.KPIThreeMonthlyTarget <= 0)
                {
                    model.KPIThreeMonthlyTarget = data.FirstOrDefault(x => x.Kpithreemonthlytarget.HasValue && x.Kpithreemonthlytarget > 0)?.Kpithreemonthlytarget;
                }
                if (model.KPIFourTargetMonthly == null || model.KPIFourTargetMonthly <= 0)
                {
                    model.KPIFourTargetMonthly = data.FirstOrDefault(x => x.Kpifourtargetmonthly.HasValue && x.Kpifourtargetmonthly > 0)?.Kpifourtargetmonthly;
                }
            }
            model.OpCoResource = _opcoResource.ToDictionary(x => x.OpCoId, x => x.OpCoDescription);
            model.VolteKPITypeResource = GetVolteKPITypesResource();
            return model;

        }

        public VolteKPIDtoUpdate GetUpdatePage(long id)
        {
            var entity = _repositoryWrapper.VolteKPI.FindByCondition(x => x.Voltekpiid == id, true)
                .Include(x => x.ModificationuserNavigation)
                .Single();
            var dto = _mapper.Map<VolteKPIDtoUpdate>(entity);

            var opocResource = _repositoryWrapper.OpCo.FindAll();
            dto.OpCoResource = opocResource.ToDictionary(x => x.Opcoid, x => x.Opco);
            if (!dto.OpCoResource.ContainsKey(dto.OpCoId))
            {
                var data = _repositoryWrapper.OpCo.FindByCondition(
                    x => x.Opcoid == dto.OpCoId, true).SingleOrDefault();
                if (data != null)
                {
                    dto.OpCoResource.Add(data.Opcoid, data.Opco);
                }
            }

            dto.KPIOneComment = "";
            dto.KPITwoComment = "";
            dto.KPIThreeComment = "";
            dto.KPIFourComment = "";
            dto.KPIOneTargetValueChangeProposal = null;
            dto.KPIThreeTargetValueChangeProposal = null;
            dto.KPIFourTargetValueChangeProposal = null;
            return dto;
        }

        private static ExpressionStarter<Voltekpi> ApplyFilter(VolteKPIQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Voltekpi>();
            var predicateInner = PredicateBuilder.New<Voltekpi>();

            if (buildFilterDto.VolteKPIId != null && buildFilterDto.VolteKPIId.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpi>();
                foreach (var item in buildFilterDto.VolteKPIId)
                    predicateInner.Or(x => x.Voltekpiid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpi>();
                foreach (var item in buildFilterDto.OpCo)
                    predicateInner.Or(x => x.Opco.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Month != null)
            {
                predicateInner = PredicateBuilder.New<Voltekpi>();
                predicateInner.And(x => x.Month == buildFilterDto.Month);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Year != null)
            {
                predicateInner = PredicateBuilder.New<Voltekpi>();
                predicateInner.And(x => x.Year == buildFilterDto.Year);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpi>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Voltekpi>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);

                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        public QueryResultDto<VolteKPIDtoGrid> FindWithCondition(VolteKPIQueryDto volteKPIFilterDto)
        {
            var predicateResult = ApplyFilter(volteKPIFilterDto);
            if (volteKPIFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            var rtn = new QueryResultDto<VolteKPIDtoGrid>(new GenerateRenderForGrid<VolteKPIDtoGrid>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.VolteKPI.Count(predicateResult) : _repositoryWrapper.VolteKPI.Count(),
            };
            var query = GetQuery(predicateResult, volteKPIFilterDto.Deleted ?? false).ApplyOrdering(volteKPIFilterDto, GetColumnsMap()).ApplyPaging(volteKPIFilterDto);
            var data = query
                        .Include(x => x.OpCo)
                        .ToList();

            IEnumerable<VolteKPIDtoGrid> volteKPIResult;


            volteKPIResult = _mapper.Map<IEnumerable<VolteKPIDtoGrid>>(data);

            rtn.Items = volteKPIResult.ToArray();
            return rtn;
        }
        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, VolteKPIQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "volteKPIId" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.VolteKPIId.ToString(), Value = p.VolteKPIId.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.VolteKPIId.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.VolteKPIId.ToString(), Value = p.VolteKPIId.ToString() }).Distinct()
                    .ToList(),

                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),

                "opCo" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.OpCo.OpCoDescription,
                        Value = p.OpCoId.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.OpCo.OpCoDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.OpCo.OpCoDescription,
                                    Value = p.OpCoId.ToString()
                                }).Distinct().ToList(),

                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

        private IQueryable<VolteKPI> GetQuery(ExpressionStarter<Voltekpi> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.VolteKPI.FindByCondition(predicateResult, includeDeleted)

                    .Include(x => x.Opco)
                    .Include(x => x.ModificationuserNavigation)

                : _repositoryWrapper.VolteKPI.FindAll()

                    .Include(x => x.Opco)
                    .Include(x => x.ModificationuserNavigation)
                    ;
            var result = query.AsEnumerable().Select(p => VolteKPIMapper.Get(p)).AsEnumerable().AsQueryable();
            return result;
        }

        private Dictionary<string, Expression<Func<VolteKPI, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<VolteKPI, object>>[]>
            {
                ["networkElementAsIsId"] = new Expression<Func<VolteKPI, object>>[] { p => p.VolteKPIId },
                ["opCoId"] = new Expression<Func<VolteKPI, object>>[] { p => p.OpCo.OpCoId },
                ["lastModified"] = new Expression<Func<VolteKPI, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<VolteKPI, object>>[] { p => p.ModificationUserEntity.Email },

                ["month"] = new Expression<Func<VolteKPI, object>>[] { p => p.Month },
                ["year "] = new Expression<Func<VolteKPI, object>>[] { p => p.Year },

                ["kpiOneActualValue"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIOneActualValue },
                ["kpiOneEoYTarget"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIOneEoYTarget },
                ["kpiOneMonthlyTarget"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIOneMonthlyTarget },
                ["kpiOneTargetValueChangeProposal"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIOneTargetValueChangeProposal },
                ["kpiOneComment"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIOneComment },
                ["kpiTwoActualNumberOfRegisteredSubscribers"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPITwoActualNumberOfRegisteredSubscribers },
                ["kpiTwoActualNumberOfProvisionedSubscriber"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPITwoActualNumberOfProvisionedSubscriber },
                ["kpiTwoComment"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPITwoComment },
                ["kpiThreeActualValue"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIThreeActualValue },
                ["kpiThreeEoYTarget"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIThreeEoYTarget },
                ["kpiThreeMonthlyTarget"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIThreeMonthlyTarget },
                ["kpiThreeTargetValueChangeProposal"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIThreeTargetValueChangeProposal },
                ["kpiThreeComment"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIThreeComment },
                ["kpiFourActualMonthly"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIFourActualMonthly },
                ["kpiFourFinalTarget"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIFourFinalTarget },
                ["kpiFourTargetMonthly"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIFourTargetMonthly },
                ["kpiFourTargetValueChangeProposal"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIFourTargetValueChangeProposal },
                ["kpiFourTargetDateMonth"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIFourTargetDateMonth },
                ["kpiFourTargetDateYear"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIFourTargetDateYear },
                ["kpiFourComment"] = new Expression<Func<VolteKPI, object>>[] { p => p.KPIFourComment },
            };
        }

        public VolteKPIDashboardDto GetDashboard(VolteKPIQueryDto volteKPIFilterDto)
        {
            List<VolteKPIDtoGrid> volteKPIResult = new List<VolteKPIDtoGrid>();

            //var opCos = _repositoryWrapper.OpCo.FindByCondition(x => volteKPIFilterDto.OpCo.Contains(x.OpCoId)).OrderBy(x => x.OpCoDescription).ToList();
            List<OpCo> opCos = new List<OpCo>();
            if (volteKPIFilterDto.OpCo == null || volteKPIFilterDto.OpCo.Count == 0)
            {
                opCos = _opcoResource != null && _opcoResource.Count()>0? _opcoResource.OrderBy(x => x.OpCoDescription).ToList() : null;
                volteKPIFilterDto.OpCo = opCos != null ? opCos.Select(x => x.OpCoId).ToList() : null;
            }
            else
            {               

               opCos = _opcoResource != null ? _opcoResource.Where(x => volteKPIFilterDto.OpCo.Contains(x.OpCoId))
                    .OrderBy(x => x.OpCoDescription).ToList() : null;
                volteKPIFilterDto.OpCo = opCos != null ? opCos.Select(x => x.OpCoId).ToList() : null;
            }


            var predicateResult = ApplyFilter(volteKPIFilterDto);
            if (volteKPIFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            var rtn = new VolteKPIDashboardDto();
            var query = GetQuery(predicateResult, volteKPIFilterDto.Deleted ?? false);
            var data = query
                        .Include(x => x.OpCo)
                        .OrderBy(x => x.OpCo.OpCoDescription)
                        .ToList();



            volteKPIResult.Add(new VolteKPIDtoGrid
            {
                OpCoColumns = new List<string>(),
                OpCoList = new List<VolteKPIColumn>(),
                Program = ConstantValueFilter.KPI1,
                Type = VolteKPIType.KPI1_Capacity
            });
            volteKPIResult.Add(new VolteKPIDtoGrid
            {
                OpCoColumns = new List<string>(),
                OpCoList = new List<VolteKPIColumn>(),
                Program = ConstantValueFilter.KPI2,
                Type = VolteKPIType.KPI2_Current_Utilization
            });
            volteKPIResult.Add(new VolteKPIDtoGrid
            {
                OpCoColumns = new List<string>(),
                OpCoList = new List<VolteKPIColumn>(),
                Program = ConstantValueFilter.KPI3,
                Type = VolteKPIType.KPI3_Penetration
            });
            volteKPIResult.Add(new VolteKPIDtoGrid
            {
                OpCoColumns = new List<string>(),
                OpCoList = new List<VolteKPIColumn>(),
                Program = ConstantValueFilter.KPI43G,
                Type = VolteKPIType.KPI4_3G_ShutDown
            });

            foreach (var r in data)
            {
                volteKPIResult[0].OpCoColumns.Add(r.OpCo.OpCoDescription);
                volteKPIResult[0].OpCoList.Add(new VolteKPIColumn
                {
                    BackgroundColor = GetColor(VolteKPIType.KPI1_Capacity, r),
                    OpCoId = r.OpCoId,
                    Value = $"{GetThousands(r.KPIOneActualValue)} / {GetThousands(r.KPIOneMonthlyTarget)}",
                    VolteKPIId = r.VolteKPIId,
                    VolteKPIType = VolteKPIType.KPI1_Capacity
                });
                volteKPIResult[1].OpCoColumns.Add(r.OpCo.OpCoDescription);
                volteKPIResult[1].OpCoList.Add(new VolteKPIColumn
                {
                    BackgroundColor = GetColor(VolteKPIType.KPI2_Current_Utilization, r),
                    OpCoId = r.OpCoId,
                    Value = $"{GetThousands(r.KPITwoActualNumberOfProvisionedSubscriber)} / {GetThousands(r.KPITwoActualNumberOfRegisteredSubscribers)}",
                    VolteKPIId = r.VolteKPIId,
                    VolteKPIType = VolteKPIType.KPI2_Current_Utilization
                });
                volteKPIResult[2].OpCoColumns.Add(r.OpCo.OpCoDescription);
                volteKPIResult[2].OpCoList.Add(new VolteKPIColumn
                {
                    BackgroundColor = GetColor(VolteKPIType.KPI3_Penetration, r),
                    OpCoId = r.OpCoId,

                    Value = $"{GetPercentual(r.KPIThreeActualValue)}% / {GetPercentual(r.KPIThreeEoYTarget)}%",
                    //Value = $"{(r.KPIThreeActualValue.HasValue ? (r.KPIThreeActualValue.Value * 100).ToString("###,#0") : "--")} %/{(r.KPIThreeEoYTarget.HasValue ? (r.KPIThreeEoYTarget.Value * 100).ToString("###,#0") : "--")} %",
                    VolteKPIId = r.VolteKPIId,
                    VolteKPIType = VolteKPIType.KPI3_Penetration
                });
                volteKPIResult[3].OpCoColumns.Add(r.OpCo.OpCoDescription);
                volteKPIResult[3].OpCoList.Add(new VolteKPIColumn
                {
                    BackgroundColor = GetColor(VolteKPIType.KPI4_3G_ShutDown, r),
                    OpCoId = r.OpCoId,
                    Value = "0 % / 0 %", //$"{r.KPIFourActualValue}/{r.KPIFourEoYTarget}",
                    VolteKPIId = r.VolteKPIId,
                    VolteKPIType = VolteKPIType.KPI4_3G_ShutDown
                });
            }

            //volteKPIResult = _mapper.Map<IEnumerable<VolteKPIDtoGrid>>(data.AsEnumerable()).ToList();
            rtn.VolteKPITypeResource = new Dictionary<int, string>();
            foreach (VolteKPIType volteKPIType in System.Enum.GetValues(typeof(VolteKPIType)))
            {
                rtn.VolteKPITypeResource.Add((int)volteKPIType, volteKPIType.ToString().Replace("_", " "));
            }
            //rtn.VolteKPITypeResource = GetVolteKPITypesResource();
            if (volteKPIFilterDto.VolteKPITypes != null && volteKPIFilterDto.VolteKPITypes.Count > 0)
            {
                rtn.Items = volteKPIResult.Where(x => volteKPIFilterDto.VolteKPITypes.Contains(x.Type)).ToList();
            }
            else
            {
                rtn.Items = volteKPIResult.Where(x => rtn.VolteKPITypeResource.Select(t => t.Key).Contains((int)x.Type)).ToList();
            }


            rtn.OpCoResource = _opcoResource.ToDictionary(x => x.OpCoId, x => x.OpCoDescription);
            //rtn.OpCoResource = _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);
            return rtn;
        }
        public VolteKPIReportDto GetDashboardReports(VolteKPIQueryDto volteKPIFilterDto)
        {
            var currentYear = volteKPIFilterDto.Year ?? (short)DateTime.Now.Year;

            VolteKPIReportDto volteKPIResult = new VolteKPIReportDto
            {
                Provisioned = new List<VolteKPIReportRow>(),
                Registered = new List<VolteKPIReportRow>(),
                OpCoResource = _opcoResource.ToDictionary(x => x.OpCoId, x => x.OpCoDescription),
                //OpCoResource = _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid , x => x.Opco),
                Year = currentYear
            };

            var opCos = _opcoResource.Where(x => (volteKPIFilterDto.OpCo == null || volteKPIFilterDto.OpCo.Count <= 0 || volteKPIFilterDto.OpCo.Contains(x.OpCoId))).OrderBy(x => x.OpCoDescription).ToList();

            var data = _repositoryWrapper.VolteKPI.FindByCondition(x =>
                    (x.Year == volteKPIFilterDto.Year && x.Month >= 4) || (x.Year == volteKPIFilterDto.Year + 1 && x.Month <= 3) &&
                    (volteKPIFilterDto.OpCo == null || volteKPIFilterDto.OpCo.Count <= 0 || volteKPIFilterDto.OpCo.Contains(x.Opcoid))
                )
                .Include(x => x.Opco)
                .OrderBy(x => x.Opco.Opco);

            foreach (var r in opCos)
            {
                VolteKPIReportColumn[] monthValuesOne = new VolteKPIReportColumn[12];
                VolteKPIReportColumn[] monthValuesTwo = new VolteKPIReportColumn[12];
                for (short i = 0; i < 12; i++)
                {
                    short month = (short)(((i + 3) % 12) + 1);
                    string sMonth = CultureInfo.GetCultureInfo("us-EN").DateTimeFormat.GetAbbreviatedMonthName(month);
                    short year = (short)((i <= 8) ? currentYear : currentYear + 1);
                    string sYear = year.ToString().Substring(2, 2);

                    monthValuesOne[i] = new VolteKPIReportColumn
                    {
                        BackgroundColor = ConstantValueFilter.White,
                        MonthYearLabel = $"{sMonth}-{sYear}",
                        Month = month,
                        Quarter = $"{GetQuarter(month)}",
                        Value = "0"
                    };
                    monthValuesTwo[i] = new VolteKPIReportColumn(monthValuesOne[i]);
                }
                foreach (var x in data.Where(x => x.Opcoid == r.OpCoId).OrderBy(x => x.Year).ThenBy(x => x.Month))
                {
                    var index = (x.Month >= 4 && x.Month <= 12) ? x.Month - 4 : x.Month + 8;
                    //monthValuesOne[index].BackgroundColor = GetColor(VolteKPIType.KPI1_Capacity, x);
                    monthValuesOne[index].Value = $"{x.Kpionemonthlytarget.GetValueOrDefault().ToString("###,#0")}";

                    //monthValuesTwo[index].BackgroundColor = GetColor(VolteKPIType.KPI2_Current_Utilization, x);
                    monthValuesTwo[index].Value = $"{x.Kpi2actualnoofregsubsc.GetValueOrDefault().ToString("###,#0")}";
                    //monthValuesTwo[index].Value = $"{x.KPITwoActualValue.GetValueOrDefault().ToString("###,#0.000")}";
                }
                volteKPIResult.Provisioned.Add(new VolteKPIReportRow
                {
                    OpCo = r.OpCoDescription,
                    MonthValues = monthValuesOne.ToList(),
                    Type = VolteKPIType.KPI1_Capacity
                });
                volteKPIResult.Registered.Add(new VolteKPIReportRow
                {
                    OpCo = r.OpCoDescription,
                    MonthValues = monthValuesTwo.ToList(),
                    Type = VolteKPIType.KPI2_Current_Utilization
                });
            }

            return volteKPIResult;
        }

        //public QueryResultDto<VolteKPITargetApprovalDto> GetApprovals(VolteKPIQueryDto volteKPIFilterDto)
        //{
        //    var opCodIds = _opcoResource.Select(o => o.OpCoId);
        //    var query = _repositoryWrapper.VolteKPI.FindByCondition(x =>
        //                        x.Deleted == false &&
        //                        opCodIds.Contains(x.OpCoId) &&
        //                        (
        //                            x.KPIOneTargetValueChangeProposal.HasValue ||
        //                            //x.KPITwoTargetValueChangeProposal.HasValue ||
        //                            x.KPIThreeTargetValueChangeProposal.HasValue ||
        //                            x.KPIFourTargetValueChangeProposal.HasValue
        //                        )
        //                )
        //                .OrderBy(x => x.ModificationDate);

        //    var rtn = new QueryResultDto<VolteKPITargetApprovalDto>()
        //    {
        //        TotalItems = query.Count(),
        //    };
        //    var data = query
        //                .ApplyPaging(volteKPIFilterDto)
        //                .Include(x => x.OpCo)
        //                .Include(x => x.ModificationUserEntity)
        //                .Include(x => x.CreationUserEntity)
        //                .ToList();

        //    var volteKPITypesResource = GetVolteKPITypesResource();

        //    List<VolteKPITargetApprovalDto> approvalsResult = new List<VolteKPITargetApprovalDto>();

        //    foreach (VolteKPI rec in data)
        //    {
        //        if (rec.KPIOneTargetValueChangeProposal.HasValue && volteKPITypesResource.ContainsKey((int)VolteKPIType.KPI1_Capacity))
        //        {
        //            approvalsResult.Add(rec.ToApprovalRecord(VolteKPIType.KPI1_Capacity));
        //        }
        //        //if (rec.KPITwoTargetValueChangeProposal.HasValue && volteKPITypesResource.ContainsKey((int)VolteKPIType.KPI2_Current_Utilization))
        //        //{
        //        //    approvalsResult.Add(rec.ToApprovalRecord(VolteKPIType.KPI2_Current_Utilization));
        //        //}
        //        if (rec.KPIThreeTargetValueChangeProposal.HasValue && volteKPITypesResource.ContainsKey((int)VolteKPIType.KPI3_Penetration))
        //        {
        //            approvalsResult.Add(rec.ToApprovalRecord(VolteKPIType.KPI3_Penetration));
        //        }
        //        //if (rec.KPIFourTargetValueChangeProposal.HasValue && volteKPITypesResource.ContainsKey((int)VolteKPIType.KPI4_3G_ShutDown))
        //        //{
        //        //    approvalsResult.Add(rec.ToApprovalRecord(VolteKPIType.KPI4_3G_ShutDown));
        //        //}
        //    }

        //    rtn.Items = approvalsResult.ToArray();
        //    rtn.TotalItems = approvalsResult.Count();
        //    return rtn;
        //}

        //public async Task<ResultDto> UpdateApproval(VolteKPITargetApprovalDto dto)
        //{
        //    var originalEntityWithSameNaturalKey = await _repositoryWrapper.VolteKPI.FindByCondition(
        //            x => x.OpCoId == dto.OpCoId &&
        //                 x.Month == dto.Month &&
        //                 x.Year == dto.Year &&
        //                 x.VolteKPIId == dto.VolteKPIId
        //            , true).Include(x => x.ModificationUserEntity).SingleOrDefaultAsync();

        //    if (originalEntityWithSameNaturalKey == null)
        //    {
        //        return new ResultDto
        //        {
        //            Warning = true,
        //            Info = ResultMessages.NoVolteKPI,
        //            Data = dto.VolteKPIId
        //        };
        //    }

        //    VolteKPIDtoCreate entity = _mapper.Map<VolteKPIDtoCreate>(originalEntityWithSameNaturalKey);
        //    switch (dto.Type)
        //    {
        //        case VolteKPIType.KPI1_Capacity:
        //            if (dto.ApproveValueChangeProposal)
        //            {
        //                entity.KPIOneMonthlyTarget = dto.MonthlyTargetNew;
        //            }
        //            entity.KPIOneTargetValueChangeProposal = null;
        //            entity.KPIOneComment = "";
        //            break;
        //        //case VolteKPIType.KPI2_Current_Utilization:
        //        //    if (dto.ApproveValueChangeProposal)
        //        //    {
        //        //        entity.KPITwoActualNumberOfProvisionedSubscriber = dto.MonthlyTargetNew;
        //        //    }
        //        //    //entity.KPITwoTargetValueChangeProposal = null;
        //        //    entity.KPITwoComment = "";
        //        //    break;
        //        case VolteKPIType.KPI3_Penetration:
        //            if (dto.ApproveValueChangeProposal)
        //            {
        //                entity.KPIThreeMonthlyTarget = dto.MonthlyTargetNew;
        //            }
        //            entity.KPIThreeTargetValueChangeProposal = null;
        //            entity.KPIThreeComment = "";
        //            break;
        //        case VolteKPIType.KPI4_3G_ShutDown:
        //            if (dto.ApproveValueChangeProposal)
        //            {
        //                entity.KPIFourTargetMonthly = dto.MonthlyTargetNew;
        //            }
        //            entity.KPIFourTargetValueChangeProposal = null;
        //            entity.KPIFourComment = "";
        //            break;
        //    }
        //    ApplicationUser lastModificationUser = originalEntityWithSameNaturalKey.ModificationUserEntity;
        //    var retValue = await UpdateBase(entity, true);
        //    if (!retValue.Warning)
        //    {
        //        retValue = await SendMailOnApproval(dto.ApproveValueChangeProposal, entity, lastModificationUser, dto);
        //    }
        //    return retValue;
        //}

        //private async Task<ResultDto> SendMailOnApproval(bool approve, VolteKPIDtoCreate entity, ApplicationUser lastModificationUser, VolteKPITargetApprovalDto originalRecord)
        //{
        //    var from = _configuration["ApplicationSettings:MailSender"];
        //    var mailScheduled = bool.Parse(_configuration["ApplicationSettings:SendScheduled"]);

        //    var body = $"Hello {lastModificationUser.UserName},<br />" +
        //        $"<br />Modification for <b>{originalRecord.KPIIDName}</b><br /><br />" +
        //        $"<table>" +
        //        $"  <tr>" +
        //        $"      <td>Selected Month</td>" +
        //        $"      <td>:</td>" +
        //        $"      <td><b>{originalRecord.Month + "/" + originalRecord.Year}</b></td>" +
        //        $"  </tr>" +
        //        $"  <tr>" +
        //        $"      <td>Selected OpCo</td>" +
        //        $"      <td>:</td>" +
        //        $"      <td><b>{originalRecord.OpCo}</b></td>" +
        //        $"  </tr>" +
        //        $"  <tr>" +
        //        $"      <td>Previous Target Value</td>" +
        //        $"      <td>:</td>" +
        //        $"      <td><b>{originalRecord.MonthlyTargetPrevious?.ToString("###,#0.000")}</b></td>" +
        //        $"  </tr>" +
        //        $"  <tr>" +
        //        $"      <td>Comment</td>" +
        //        $"      <td>:</td>" +
        //        $"      <td><b>{originalRecord.Comment}</b></td>" +
        //        $"  </tr>" +
        //        $"</table>" +
        //        $"<br />" +
        //        $"<h1 style=\"color: {(approve ? "green" : "red")};\">{(approve ? "Was Approved" : "Was Not Approved")}</h1>" +
        //        $"<br />" +
        //        $"The new value for Monthly Target Is : <b>{(approve ? originalRecord.MonthlyTargetNew?.ToString("###,#0.000") : "INVARIATED")}</b>";


        //    MailQueue mailQueue = new MailQueue
        //    {
        //        From = from,
        //        Subject = "VoLTE KPI Monthly Target Variation",
        //        Body = body,
        //        To = lastModificationUser.Email,
        //        State = mailScheduled ? MailStateEnum.Insert : MailStateEnum.Sending,
        //        ApprovalUserId = _currentUserService.UserId
        //    };

        //    _repositoryWrapper.MailQueue.Create(mailQueue);
        //    await _repositoryWrapper.SaveAsync();
        //    ResultDto resultDto = new ResultDto
        //    {
        //        Warning = false,
        //        Info = ResultMessages.MailQueueInsert,
        //        Data = entity.VolteKPIId
        //    };

        //    if (!mailScheduled)
        //    {
        //        MailMessage mailMessage = new MailMessage
        //        {
        //            From = new MailAddress(mailQueue.From),
        //            Subject = mailQueue.Subject,
        //            Body = mailQueue.Body,
        //            IsBodyHtml = true,
        //        };
        //        mailMessage.To.Add(mailQueue.To);
        //        try
        //        {
        //            _mailManager.SendMail(mailMessage);
        //            mailQueue.State = MailStateEnum.Sended;
        //            mailQueue.ErrorMessage = "";
        //            resultDto.Warning = false;
        //            resultDto.Info = ResultMessages.MailQueueSended;
        //        }
        //        catch (Exception ex)
        //        {
        //            mailQueue.State = MailStateEnum.ErrorOnSend;
        //            mailQueue.ErrorMessage = ex.ToString();
        //            resultDto.Warning = true;
        //            resultDto.Info = ResultMessages.MailQueueErrorOnSend;
        //        }
        //        _repositoryWrapper.MailQueue.Update(mailQueue);
        //        await _repositoryWrapper.SaveAsync();
        //    }
        //    return resultDto;
        //}
        //private async Task<ResultDto> SendMailOnSaveOrEdit(List<ApplicationUser> kpiAdministrators, VolteKPIType volteKPIType, string kpiName, decimal kpiValue, string kpiComment, VolteKPIDtoCreate entity, string lastModificationUserEmail, string hostName)
        //{
        //    var from = _configuration["ApplicationSettings:MailSender"];
        //    var mailScheduled = bool.Parse(_configuration["ApplicationSettings:SendScheduled"]);

        //    ResultDto resultDto = new ResultDto
        //    {
        //        Warning = false,
        //        Info = "",
        //        Data = entity.VolteKPIId
        //    };
        //    string skpiValue = (volteKPIType == VolteKPIType.KPI3_Penetration) ? $"{(kpiValue * 100).ToString("###,#0")} %" : kpiValue.ToString("###,#0.000");
        //    foreach (var kpiAdmin in kpiAdministrators)
        //    {

        //        var body = $"Hello {kpiAdmin.UserName},<br />" +
        //            $"<br />Request approval for <b>{kpiName}</b><br /><br />" +
        //            $"<table>" +
        //            $"  <tr>" +
        //            $"      <td>Selected Month</td>" +
        //            $"      <td>:</td>" +
        //            $"      <td><b>{entity.Month + "/" + entity.Year}</b></td>" +
        //            $"  </tr>" +
        //            $"  <tr>" +
        //            $"      <td>Selected OpCo</td>" +
        //            $"      <td>:</td>" +
        //            $"      <td><b>{(entity.OpCoResource.ContainsKey(entity.OpCoId) ? entity.OpCoResource[entity.OpCoId] : entity.OpCoId.ToString())}</b></td>" +
        //            $"  </tr>" +
        //            $"  <tr>" +
        //            $"      <td>Proposal Monthly Target Value</td>" +
        //            $"      <td>:</td>" +
        //            $"      <td><b>{skpiValue}</b></td>" +
        //            $"  </tr>" +
        //            $"  <tr>" +
        //            $"      <td>Comment</td>" +
        //            $"      <td>:</td>" +
        //            $"      <td><b>{kpiComment}</b></td>" +
        //            $"  </tr>" +
        //            $"  <tr>" +
        //            $"      <td>Request Submitted by</td>" +
        //            $"      <td>:</td>" +
        //            $"      <td><b>{lastModificationUserEmail}</b></td>" +
        //            $"  </tr>" +
        //            $"  <tr>" +
        //            $"      <td>&nbsp;</td>" +
        //            $"      <td>&nbsp;</td>" +
        //            $"      <td>&nbsp;</td>" +
        //            $"  </tr>" +
        //            $"  <tr>" +
        //            $"      <td>To approve</td>" +
        //            $"      <td>:</td>" +
        //            $"      <td><b><a href='{hostName}{"/targetmonthlyapprovals"}'>click here</a></b></td>" +
        //            $"  </tr>" +
        //            $"</table>" +
        //            $"<br />";


        //        MailQueue mailQueue = new MailQueue
        //        {
        //            From = from,
        //            Subject = "VoLTE KPI Monthly Target Proposal",
        //            Body = body,
        //            To = kpiAdmin.Email,
        //            State = mailScheduled ? MailStateEnum.Insert : MailStateEnum.Sending,
        //            ApprovalUserId = kpiAdmin.Id  //_currentUserService.UserId
        //        };

        //        _repositoryWrapper.MailQueue.Create(mailQueue);
        //        await _repositoryWrapper.SaveAsync();


        //        if (!mailScheduled)
        //        {
        //            MailMessage mailMessage = new MailMessage
        //            {
        //                From = new MailAddress(mailQueue.From),
        //                Subject = mailQueue.Subject,
        //                Body = mailQueue.Body,
        //                IsBodyHtml = true,
        //            };
        //            mailMessage.To.Add(mailQueue.To);
        //            try
        //            {
        //                _mailManager.SendMail(mailMessage);
        //                mailQueue.State = MailStateEnum.Sended;
        //                mailQueue.ErrorMessage = "";
        //                resultDto.Warning |= false;
        //                resultDto.Info += $"{kpiAdmin.Email} - {ResultMessages.MailQueueSended}\n";
        //            }
        //            catch (Exception ex)
        //            {
        //                mailQueue.State = MailStateEnum.ErrorOnSend;
        //                mailQueue.ErrorMessage = ex.ToString();
        //                resultDto.Warning |= true;
        //                resultDto.Info += $"{kpiAdmin.Email} - {ResultMessages.MailQueueErrorOnSend}\n";
        //            }
        //            _repositoryWrapper.MailQueue.Update(mailQueue);
        //            await _repositoryWrapper.SaveAsync();
        //        }
        //        else
        //        {
        //            resultDto.Warning |= false;
        //            resultDto.Info += $"{kpiAdmin.Email} - {ResultMessages.MailQueueInsert}\n";
        //        }
        //    }
        //    return resultDto;
        //}

        private static string GetColor(VolteKPIType type, VolteKPI record)
        {
            double rapporto = 0;
            switch (type)
            {
                case VolteKPIType.KPI1_Capacity:
                    if (record.KPIOneActualValue.HasValue)
                    {
                        rapporto = (record.KPIOneMonthlyTarget.HasValue) ? (double)record.KPIOneActualValue.Value  / (double)record.KPIOneMonthlyTarget.Value : 0.0;
                        if (rapporto >= 0.9)
                        {
                            return ConstantValueFilter.Green;
                        }
                        else if (rapporto <= 0.75)
                        {
                            return ConstantValueFilter.Red;
                        }
                        else
                        {
                            return ConstantValueFilter.Orange;
                        }
                    }
                    else
                    {
                        return ConstantValueFilter.Gray;
                    }
                case VolteKPIType.KPI3_Penetration:
                    if (record.KPIThreeActualValue.HasValue)
                    {
                        rapporto = (record.KPIThreeActualValue.HasValue && record.KPIThreeMonthlyTarget.HasValue) ? (double)record.KPIThreeActualValue.Value / (double)record.KPIThreeMonthlyTarget.Value : 0.0;
                        if (rapporto >= 0.9)
                        {
                            return ConstantValueFilter.Green;
                        }
                        else if (rapporto <= 0.75)
                        {
                            return ConstantValueFilter.Red;
                        }
                        else
                        {
                            return ConstantValueFilter.Orange;
                        }
                    }
                    else
                    {
                        return ConstantValueFilter.Gray;
                    }
                default:
                    return ConstantValueFilter.Gray;
            }
        }
        private static string GetQuarter(short month)
        {
            switch (month)
            {
                case 1:
                case 2:
                case 3:
                    return ConstantValueFilter.Q4;
                case 4:
                case 5:
                case 6:
                    return ConstantValueFilter.Q1;
                case 7:
                case 8:
                case 9:
                    return ConstantValueFilter.Q2;
                case 10:
                case 11:
                case 12:
                    return ConstantValueFilter.Q3;
                default:
                    return "";
            }
        }
        private Dictionary<int, string> GetVolteKPITypesResource()
        {
            var volteKPITypeResource = new Dictionary<int, string>();
            foreach (VolteKPIType volteKPIType in System.Enum.GetValues(typeof(VolteKPIType)))
            {
                //Per il momento il KPI 4 non è utilizzato (Rif: Backlog 371)
                if (volteKPIType != VolteKPIType.KPI4_3G_ShutDown)
                {
                    volteKPITypeResource.Add((int)volteKPIType, volteKPIType.ToString().Replace("_", " "));
                }

            }
            return volteKPITypeResource;
        }

        private string GetThousands(decimal? value, int decimalNumber = 1)
        {
            string decimals = (decimalNumber <= 0) ? "" : ("." + new string('0', decimalNumber));
            if (value.HasValue)
            {
                return (value.Value / (decimal)1000.0).ToString($"###,#0{decimals}");
            }
            else
            {
                return "--";
            }
        }
        private string GetPercentual(decimal? value, int decimalNumber = 0)
        {
            string decimals = (decimalNumber <= 0) ? "" : ("." + new string('0', decimalNumber));
            if (value.HasValue)
            {
                return (value.Value * (decimal)100.0).ToString($"###,#0{decimals}");
            }
            else
            {
                return "--";
            }
        }
    }

}
