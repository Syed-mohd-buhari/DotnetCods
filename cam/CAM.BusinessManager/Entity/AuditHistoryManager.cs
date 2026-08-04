using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AuditHistory;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Wordprocessing;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CAM.Enum.AuditHistortStatusEnum;

namespace CAM.BusinessManager.Entity
{
    public class AuditHistoryManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        private ReconciliationManager _reconciliation;
        ResultDto networkElement = new ResultDto();
        ResultDto hardConfig = new ResultDto();
        ResultDto identityAsis = new ResultDto();

        public AuditHistoryManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,            
            IHttpContextAccessor contextAccessor, ReconciliationManager reconciliation,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _reconciliation = reconciliation;
        }


        #region //UiMemberFunctions
        public QueryResultDto<AuditHistoryDtoGrid> FindWithCondition(AuditHistoryQueryDto designComponentFilterDto)
        {
            var predicateResult = ApplyFilter(designComponentFilterDto);
            #region //put a entry for reconcilliation
            var dto = new ReconciliationQueryDto();
            var result = _reconciliation.AddEntryForAudit(dto);
            #endregion
            var rtn = new QueryResultDto<AuditHistoryDtoGrid>(new GenerateRenderForGrid<AuditHistoryDtoGrid>(_manager))
            {
               
            };
            var query = GetQuery(predicateResult, designComponentFilterDto.Deleted ?? false).ApplyOrdering(designComponentFilterDto, GetColumnsMap());
            query = query.GroupBy(x => new { x.Columnname, x.Primarykey, x.Status})
               .Select(group => group.OrderByDescending(x => x.Modificationdate).FirstOrDefault());
            rtn.TotalItems = query.Count();
            query =query.ApplyPaging(designComponentFilterDto);
            var data = query.ToList();
                     
            IEnumerable <AuditHistoryDtoGrid> AuditHistoryDtoGrid;

            AuditHistoryDtoGrid = _mapper.Map<IEnumerable<AuditHistoryDtoGrid>>(data);

            rtn.Items = AuditHistoryDtoGrid.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Audithistory> ApplyFilter(AuditHistoryQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Audithistory>();
            var predicateInner = PredicateBuilder.New<Audithistory>();

            if (buildFilterDto.Audithistoryid != null && buildFilterDto.Audithistoryid.Any())
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                foreach (var item in buildFilterDto.Audithistoryid)
                    predicateInner.Or(x => x.Audithistoryid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Opco != null && buildFilterDto.Opco.Any())
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                foreach (var item in buildFilterDto.Opco)
                    predicateInner.Or(x => x.Opco == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Oem != null && buildFilterDto.Oem.Any())
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                foreach (var item in buildFilterDto.Oem)
                    predicateInner.Or(x => x.Oem == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ElementName != null && buildFilterDto.ElementName.Any())
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                foreach (var item in buildFilterDto.ElementName)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Primarykey != null && buildFilterDto.Primarykey.Any())
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                foreach (var item in buildFilterDto.Primarykey)
                    predicateInner.Or(x => x.Primarykey == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Tablename != null && buildFilterDto.Tablename.Any())
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                foreach (var item in buildFilterDto.Tablename)
                    predicateInner.Or(x => x.Tablename == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Columnname != null && buildFilterDto.Columnname.Any())
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                foreach (var item in buildFilterDto.Columnname)
                    predicateInner.Or(x => x.Columnname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Oldvalue != null && buildFilterDto.Oldvalue.Any())
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                foreach (var item in buildFilterDto.Oldvalue)
                    predicateInner.Or(x => x.Oldvalue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Newvalue != null && buildFilterDto.Newvalue.Any())
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                foreach (var item in buildFilterDto.Newvalue)
                    predicateInner.Or(x => x.Newvalue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Status != null && buildFilterDto.Status.Any())
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                foreach (var item in buildFilterDto.Status)
                    predicateInner.Or(x => x.Status == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationuser != null && buildFilterDto.Creationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                foreach (var item in buildFilterDto.Creationuser)
                    predicateInner.Or(x => x.CreationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Modificationuser != null && buildFilterDto.Modificationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                foreach (var item in buildFilterDto.Modificationuser)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationdate != null)
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                if (buildFilterDto.Creationdate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= buildFilterDto.Creationdate.StartDate);
                if (buildFilterDto.Creationdate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= buildFilterDto.Creationdate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Modificationdate != null)
            {
                predicateInner = PredicateBuilder.New<Audithistory>();
                if (buildFilterDto.Modificationdate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= buildFilterDto.Modificationdate.StartDate);
                if (buildFilterDto.Modificationdate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= buildFilterDto.Modificationdate.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }

        private IQueryable<AuditHistory> GetQuery(ExpressionStarter<Audithistory> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.AuditHistory.FindByCondition(predicateResult, includeDeleted)
                       .Include(x => x.CreationuserNavigation)
                       .Include(x => x.ModificationuserNavigation)
               //.GroupBy(x => new { x.Columnname, x.Primarykey, x.Status, x.Opco, x.Elementname })
               //.Select(group => group.OrderByDescending(x => x.Modificationdate).FirstOrDefault())
               : _repositoryWrapper.AuditHistory.FindAll()
                      .Include(x => x.CreationuserNavigation)
                      .Include(x => x.ModificationuserNavigation);
                      //.GroupBy(x => new { x.Columnname, x.Primarykey, x.Status, x.Opco, x.Elementname })
                      //.Select(group => group.OrderByDescending(x => x.Modificationdate).FirstOrDefault());
            return query.AsEnumerable().Select(x => AuditHistroyMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<AuditHistory, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AuditHistory, object>>[]>
            {
                ["auditHistoryId"] = new Expression<Func<AuditHistory, object>>[] { p => p.Audithistoryid },
                ["primaryKey"] = new Expression<Func<AuditHistory, object>>[] { p => p.Primarykey },
                ["opCo"] = new Expression<Func<AuditHistory, object>>[] { p => p.Opco },
                ["oem"] = new Expression<Func<AuditHistory, object>>[] { p => p.Oem },
                ["elementName"] = new Expression<Func<AuditHistory, object>>[] { p => p.Elementname },
                ["tableName"] = new Expression<Func<AuditHistory, object>>[] { p => p.Tablename },
                ["columnName"] = new Expression<Func<AuditHistory, object>>[] { p => p.Columnname },
                ["oldValue"] = new Expression<Func<AuditHistory, object>>[] { p => p.Oldvalue },
                ["newValue"] = new Expression<Func<AuditHistory, object>>[] { p => p.Newvalue },
                ["status"] = new Expression<Func<AuditHistory, object>>[] { p => p.Status },
                ["modificationDate"] = new Expression<Func<AuditHistory, object>>[] { p => p.Modificationdate },
                ["modificationUser"] = new Expression<Func<AuditHistory, object>>[] { p => p.Modificationuser },
                ["creationUser"] = new Expression<Func<AuditHistory, object>>[] { p => p.Creationuser },
                ["creationDate"] = new Expression<Func<AuditHistory, object>>[] { p => p.Creationdate },
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, AuditHistoryQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "auditHistoryId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.Audithistoryid.ToString(), Value = p.Audithistoryid.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.Audithistoryid.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.Audithistoryid.ToString(), Value = p.Audithistoryid.ToString() }).Distinct()
                   .ToList(),

                "primaryKey" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.Primarykey.ToString(), Value = p.Primarykey.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.Primarykey.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.Primarykey.ToString(), Value = p.Primarykey.ToString() }).Distinct()
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

                "tableName" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.Tablename.ToString(), Value = p.Tablename.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.Tablename.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.Tablename.ToString(), Value = p.Tablename.ToString() }).Distinct()
                    .ToList(),

                "columnName" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.Columnname, Value = p.Columnname }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.Columnname.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.Columnname, Value = p.Columnname }).Distinct().ToList(),

                "oldValue" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Oldvalue, Value = p.Oldvalue }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Oldvalue.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Oldvalue, Value = p.Oldvalue }).Distinct().ToList(),

                "newValue" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Newvalue, Value = p.Newvalue }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Newvalue.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Newvalue, Value = p.Newvalue }).Distinct().ToList(),


                "status" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Status, Value = p.Status }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Status.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Status, Value = p.Status }).Distinct().ToList(),


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

        #endregion        

        #region //CURD

        public async Task<ResultDto> Approved(AuditHistoryQueryDto dto)
        {
            ResultDto error = new ResultDto();
            List<string> notApprovrdIds = new List<string>();
            NetworkElementAsIsAttributes attributes = new NetworkElementAsIsAttributes();
            foreach (var pkId in dto.Audithistoryid)
            {

                var audit = _repositoryWrapper.AuditHistory.FindByCondition(x => x.Audithistoryid.ToString() == pkId.ToString()).FirstOrDefault();
                if (audit != null)
                {

                    if (audit.Status.Trim() == AuditHistortStatus.AUTO_APPROVED.ToString())
                    {
                        if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.NetworkElementsAsIs)
                        {
                            var networkasis = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Networkelementasisid == audit.Primarykey).FirstOrDefault();
                            if (networkasis != null)
                            {

                                Type type = networkasis.GetType();
                                var columnName = GetColumnName(type, audit);
                                var property = type.GetProperty(columnName);
                                var columnValue = property.GetValue(networkasis);

                                var val1 = columnValue != null ? columnValue.ToString() : "";
                                var val2 = audit.Newvalue != null ? audit.Newvalue.ToString() : "";
                                if (DateTime.TryParseExact(val2, "dd-MMM-yy hh.mm.ss.000000000 tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                                {
                                    val2 = parsedDate.ToString("M/d/yyyy hh:mm:ss tt");
                                }
                                else
                                {
                                    val2 = audit.Newvalue != null ? audit.Newvalue.ToString() : "";
                                }

                                if (val1.Equals(val2))
                                {
                                    audit.Status = AuditHistortStatus.APPROVED.ToString();
                                    _repositoryWrapper.AuditHistory.Update(audit);
                                    _repositoryWrapper.Save();
                                }
                                else
                                {
                                    notApprovrdIds.Add(pkId.ToString());
                                }
                            }
                            else
                            {
                                notApprovrdIds.Add(pkId.ToString());
                            }

                        }
                        else if(audit.Tablename.ToLower().Trim() == ConstantValueFilter.NetworkElementsAsPlannned)
                        {
                            var asset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == audit.Primarykey).Where(x=>x.Lcmengineeringid != null && x.Lcmengineeringid >0)
                                               .Include(x => x.Opco)
                                                .Include(x => x.Plannedactivities)
                                                .Include(x => x.Deploymentstatus)
                                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                                                .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource);
                            if (asset != null)
                            {                                
                                
                                var data = _reconciliation.AddReconciliation(new ReconciliationCreateDto()
                                {
                                    OpCo =audit.Opco,
                                    ElementName = audit.Elementname,
                                    DeploymentStatus = asset.Select(x=>x.Deploymentstatus.Deploymentstatus).FirstOrDefault(),
                                    CurrentSwVersion = audit.Oldvalue,
                                    NewSwVersion = audit.Newvalue,
                                    Status = asset.FirstOrDefault()?.Lcmengineering.PlannedactivitiesLcmengineering != null && asset.FirstOrDefault()?.Lcmengineering.PlannedactivitiesLcmengineering.Count() == 0
                                    ?ConstantValueFilter.CreatePlannedActivity: (asset.FirstOrDefault()?.Lcmengineering.PlannedactivitiesLcmengineering != null &&
                                    asset.FirstOrDefault()?.Lcmengineering.PlannedactivitiesLcmengineering.FirstOrDefault().Plannedactivityid > 0 ?ConstantValueFilter.EditPlannedActivity : ConstantValueFilter.NoActionRequired),
                                    AssetId = asset.Select(x=>x.Networkelementasplannedid).FirstOrDefault(),
                                }
                                );
                                if (!data.Result.Warning)
                                {
                                    audit.Status = AuditHistortStatus.APPROVED.ToString();
                                    _repositoryWrapper.AuditHistory.Update(audit);
                                    _repositoryWrapper.Save();
                                }
                                else
                                {
                                    notApprovrdIds.Add(pkId.ToString());
                                }
                                
                            }
                            
                        }
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.NetworkElement && attributes.AsIsAttributes.Contains(audit.Columnname.ToLower().Trim()))
                        {
                            var opCoid = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.ToLower().Trim() == audit.Opco.Replace("VODAFONE_UK", "UK").ToLower().Trim()).FirstOrDefault()?.Opcoid;
                            var oemid = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Originalequipmentmanufacturer.ToLower().Trim() == audit.Oem.ToLower().Trim()).FirstOrDefault()?.Orgeqpmanufacturerid;

                            var checkNetworkAsIsExit = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Opcoid == opCoid && x.Orgeqpmanufacturerid == oemid &&
                            x.Elementdeploymentname.ToLower().Trim() == audit.Elementname.ToLower().Trim()).FirstOrDefault();

                            if (checkNetworkAsIsExit != null)
                            {
                                audit.Status = AuditHistortStatus.APPROVED.ToString();
                                _repositoryWrapper.AuditHistory.Update(audit);
                                _repositoryWrapper.Save();
                            }
                            else
                            {
                                
                            }
                        }
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.HardwareConfiguration && attributes.AsIsAttributes.Contains(audit.Columnname.ToLower().Trim()))
                        {
                            var opCoid = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.ToLower().Trim() == audit.Opco.Replace("VODAFONE_UK", "UK").ToLower().Trim()).FirstOrDefault()?.Opcoid;
                            var oemid = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Originalequipmentmanufacturer.ToLower().Trim() == audit.Oem.ToLower().Trim()).FirstOrDefault()?.Orgeqpmanufacturerid;

                            var checkNetworkAsIsExit = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Opcoid == opCoid && x.Orgeqpmanufacturerid == oemid &&
                            x.Elementdeploymentname.ToLower().Trim() == audit.Elementname.ToLower().Trim()).FirstOrDefault();

                            if (checkNetworkAsIsExit != null)
                            {
                                audit.Status = AuditHistortStatus.APPROVED.ToString();
                                _repositoryWrapper.AuditHistory.Update(audit);
                                _repositoryWrapper.Save();
                            }
                            else
                            {

                            }
                        }
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.Identities && attributes.AsIsAttributes.Contains(audit.Columnname.ToLower().Trim()))
                        {
                            var opCoid = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.ToLower().Trim() == audit.Opco.Replace("VODAFONE_UK", "UK").ToLower().Trim()).FirstOrDefault()?.Opcoid;
                            var oemid = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Originalequipmentmanufacturer.ToLower().Trim() == audit.Oem.ToLower().Trim()).FirstOrDefault()?.Orgeqpmanufacturerid;

                            var assetEntity = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == opCoid && x.Orgeqpmanufacturerid == oemid &&
                            x.Elementname.ToLower().Trim() == audit.Elementname.ToLower().Trim()).Include(x=>x.Identitiesasis).FirstOrDefault();

                            if (assetEntity != null && (assetEntity.Identitiesasis != null && assetEntity.Identitiesasis.Count() > 0))
                            {
                                audit.Status = AuditHistortStatus.APPROVED.ToString();
                                _repositoryWrapper.AuditHistory.Update(audit);
                                _repositoryWrapper.Save();
                            }
                            else
                            {

                            }
                        }
                        else if(audit.Tablename.ToLower().Trim() == ConstantValueFilter.NetworkElement)
                        {
                            audit.Status = AuditHistortStatus.APPROVED.ToString();
                            _repositoryWrapper.AuditHistory.Update(audit);
                            _repositoryWrapper.Save();
                        }
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.HardwareConfiguration)
                        {
                            audit.Status = AuditHistortStatus.APPROVED.ToString();
                            _repositoryWrapper.AuditHistory.Update(audit);
                            _repositoryWrapper.Save();
                        }                       
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.Identities)
                        {
                            audit.Status = AuditHistortStatus.APPROVED.ToString();
                            _repositoryWrapper.AuditHistory.Update(audit);
                            _repositoryWrapper.Save();
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        notApprovrdIds.Add(pkId.ToString());
                    }
                }
                else
                {
                    notApprovrdIds.Add(pkId.ToString());
                }

            }

            return new ResultDto
            {
                Info = notApprovrdIds.Count == 0 ? ConstantValueFilter.SuccessfullyRejected : $"{ConstantValueFilter.FollowingIdsarenotApproved} : " + string.Join(",", notApprovrdIds),
                Warning = notApprovrdIds.Count == 0 ? true : false,
            };
        }

        public async Task<ResultDto> Reject(AuditHistoryQueryDto dto)
        {
            ResultDto error = new ResultDto();
            List<string> notRejectedIds = new List<string>();
            bool isOverRide = false;
            string overrideValue = "";
            NetworkElementAsIsAttributes attributes = new NetworkElementAsIsAttributes();

            foreach (var pkId in dto.Audithistoryid)
            {

                var audit = _repositoryWrapper.AuditHistory.FindByCondition(x => x.Audithistoryid.ToString() == pkId.ToString()).FirstOrDefault();
                if (audit != null)
                {

                    if (audit.Status.Trim() == AuditHistortStatus.AUTO_APPROVED.ToString() || audit.Status.Trim() == AuditHistortStatus.APPROVED.ToString())
                    {
                        if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.NetworkElementsAsIs)
                        {
                            var networkasis = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Networkelementasisid == audit.Primarykey).FirstOrDefault();
                            if (networkasis != null)
                            {

                                Type type = networkasis.GetType();
                                var columnName = GetColumnName(type, audit);
                                var property = type.GetProperty(columnName);
                                var columnValue = property.GetValue(networkasis);

                                var val1 = columnValue != null ? columnValue.ToString() : "";
                                var val2 = audit.Newvalue != null ? audit.Newvalue.ToString() : "";
                                if (DateTime.TryParseExact(val2, "dd-MMM-yy hh.mm.ss.000000000 tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                                {
                                    val2 = parsedDate.ToString("M/d/yyyy hh:mm:ss tt");
                                }
                                else
                                {
                                    val2 = audit.Newvalue != null ? audit.Newvalue.ToString() : "";
                                }
                                if (val1.Equals(val2))
                                {


                                    if (DateTime.TryParseExact(audit.Oldvalue, "dd-MMM-yy hh.mm.ss.000000000 tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                                    {

                                        DateTime oldValue = parsedDate;
                                        property.SetValue(networkasis, oldValue);
                                        _repositoryWrapper.NetworkElementAsIs.Update(networkasis);

                                    }
                                    else
                                    {
                                        property.SetValue(networkasis, audit.Oldvalue);
                                        _repositoryWrapper.NetworkElementAsIs.Update(networkasis);
                                    }


                                    audit.Status = AuditHistortStatus.REJECTED.ToString();
                                    _repositoryWrapper.AuditHistory.Update(audit);
                                    _repositoryWrapper.Save();
                                }
                                else
                                {
                                    notRejectedIds.Add(pkId.ToString());
                                }
                            }
                            else
                            {
                                notRejectedIds.Add(pkId.ToString());
                            }

                        }
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.NetworkElementsAsPlannned)
                        {
                            var asset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == audit.Primarykey)
                                               .Include(x => x.Opco)
                                                .Include(x => x.Plannedactivities)
                                                .Include(x => x.Deploymentstatus)
                                                .Include(x=>x.Networkelementsasis)
                                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                                                .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource);
                            if (asset != null)
                            {
                                // need to update the asis assured status
                                var getAsisEntities = asset.SelectMany(x => x.Networkelementsasis).ToList();
                                if(getAsisEntities != null && getAsisEntities.Count > 0)
                                {
                                    foreach(var asis in getAsisEntities)
                                    {
                                        if (asis.Softwarereleaseinformation != audit.Oldvalue)
                                        {
                                            asis.Softwarereleaseinformation = audit.Oldvalue;
                                            _repositoryWrapper.NetworkElementAsIs.Update(asis);
                                        }
                                    }
                                    asset.FirstOrDefault().Isassured = true;
                                    _repositoryWrapper.NetworkElementAsPlanned.Update(asset.FirstOrDefault());
                                    _repositoryWrapper.Save();
                                    await _repositoryWrapper.ClearTracker();
                                }

                                var data = _reconciliation.AddReconciliation(new ReconciliationCreateDto()
                                {
                                    OpCo = audit.Opco,
                                    ElementName = audit.Elementname,
                                    DeploymentStatus = asset.Select(x => x.Deploymentstatus.Deploymentstatus).FirstOrDefault(),
                                    CurrentSwVersion = audit.Oldvalue,
                                    NewSwVersion = audit.Newvalue,
                                    Status = ConstantValueFilter.NoActionRequired,
                                    AssetId = asset.Select(x => x.Networkelementasplannedid).FirstOrDefault()
                                }
                                );
                                if (!data.Result.Warning)
                                {
                                    audit.Status = AuditHistortStatus.REJECTED.ToString();
                                    _repositoryWrapper.AuditHistory.Update(audit);
                                    _repositoryWrapper.Save();
                                }
                                else
                                {
                                    notRejectedIds.Add(pkId.ToString());
                                }
                                
                            }

                        }
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.NetworkElement && attributes.AsIsAttributes.Contains(audit.Columnname.ToLower()))
                        {
                            networkElement.Warning = UpdateNetworkElement(audit, (long)audit.Primarykey, isOverRide, overrideValue).Result.Warning;
                            #region // need to remove after testing
                            //var opCoid = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.ToLower().Trim() == audit.Opco.Replace("VODAFONE_UK","UK").ToLower().Trim()).FirstOrDefault()?.Opcoid;
                            //var oemid = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Originalequipmentmanufacturer.ToLower().Trim() == audit.Oem.ToLower().Trim()).FirstOrDefault()?.Orgeqpmanufacturerid;

                            //var checkNetworkAsIsExit = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x=>x.Opcoid == opCoid && x.Orgeqpmanufacturerid == oemid && 
                            //x.Elementdeploymentname.ToLower().Trim() == audit.Elementname.ToLower().Trim()).FirstOrDefault();

                            //if (checkNetworkAsIsExit != null)
                            //{

                            //if(audit.Columnname == ConstantValueFilter.Softwarereleaseinformation)
                            //{
                            //    isApproved = await CheckingExistRecorsInAudit(audit.Elementname.Trim().ToLower());
                            //}
                            //if (isApproved)
                            //{
                            //    networkElement.Warning = UpdateNetworkElementASIS(audit, isOverRide, overrideValue);
                            //    if (networkElement.Warning)
                            //    {
                            //        audit.Status = AuditHistortStatus.REJECTED.ToString();
                            //        _repositoryWrapper.AuditHistory.Update(audit);
                            //        _repositoryWrapper.Save();
                            //        await _repositoryWrapper.ClearTracker();
                            //    }
                            //}
                            //else
                            //{
                            //}
                            //}
                            //else
                            //{

                            //}
                            #endregion
                            if (!networkElement.Warning)
                            {
                                notRejectedIds.Add(pkId.ToString());
                            }
                        }
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.HardwareConfiguration && attributes.AsIsAttributes.Contains(audit.Columnname.ToLower()))
                        {
                            var opCoid = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.ToLower().Trim() == audit.Opco.Replace("VODAFONE_UK","UK").ToLower().Trim()).FirstOrDefault()?.Opcoid;
                            var oemid = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Originalequipmentmanufacturer.ToLower().Trim() == audit.Oem.ToLower().Trim()).FirstOrDefault()?.Orgeqpmanufacturerid;

                            var checkNetworkAsIsExit = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Opcoid == opCoid && x.Orgeqpmanufacturerid == oemid &&
                            x.Elementdeploymentname.ToLower().Trim() == audit.Elementname.ToLower().Trim()).FirstOrDefault();
                            //var network = _repositoryWrapper.NetworkElement.FindByCondition(x => x.Opco.ToLower().Trim() == audit.Opco.ToLower().Trim()
                            //&& x.Oem.ToLower().Trim() == audit.Oem.ToLower().Trim()
                            //&& x.Elementname.ToLower().Trim() == audit.Elementname.ToLower().Trim()).FirstOrDefault();
                            if (checkNetworkAsIsExit != null)
                            {
                                hardConfig.Warning = UpdateNetworkElementASIS(audit,isOverRide,overrideValue);
                                if (hardConfig.Warning)
                                {
                                    audit.Status = AuditHistortStatus.REJECTED.ToString();
                                    _repositoryWrapper.AuditHistory.Update(audit);
                                    _repositoryWrapper.Save();
                                    await _repositoryWrapper.ClearTracker();
                                }
                            }
                            if (!hardConfig.Warning)
                            {
                                notRejectedIds.Add(pkId.ToString());
                            }
                        }
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.Identities && attributes.AsIsAttributes.Contains(audit.Columnname.ToLower()))
                        {
                            var opCoid = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.ToLower().Trim() == audit.Opco.Replace("VODAFONE_UK", "UK").ToLower().Trim()).FirstOrDefault()?.Opcoid;
                            var oemid = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Originalequipmentmanufacturer.ToLower().Trim() == audit.Oem.ToLower().Trim()).FirstOrDefault()?.Orgeqpmanufacturerid;

                            var asset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == opCoid && x.Orgeqpmanufacturerid == oemid &&
                            x.Elementname.ToLower().Trim() == audit.Elementname.ToLower().Trim()).Include(x=>x.Identitiesasis).FirstOrDefault();

                            if (asset != null)
                            {

                                identityAsis.Warning = UpdateIdentityASIS(audit, isOverRide, overrideValue,opCoid,oemid, asset.Networkelementasplannedid);
                                if (identityAsis.Warning)
                                {
                                    audit.Status = AuditHistortStatus.REJECTED.ToString();
                                    _repositoryWrapper.AuditHistory.Update(audit);
                                    _repositoryWrapper.Save();
                                    await _repositoryWrapper.ClearTracker();
                                }
                            }
                            else
                            {

                            }
                            if (!identityAsis.Warning)
                            {
                                notRejectedIds.Add(pkId.ToString());
                            }
                        }
                        else
                        {
                            // console.writeln(audit.tablename + "not handled");
                        }
                    }
                    else
                    {
                        notRejectedIds.Add(pkId.ToString());
                    }
                }
                else
                {
                    notRejectedIds.Add(pkId.ToString());
                }

            }

            return new ResultDto
            {
                Info = notRejectedIds.Count == 0 ? ConstantValueFilter.SuccessfullyRejected : $"{ConstantValueFilter.FollowingIdsarenotrejected} : " + string.Join(",", notRejectedIds),
                Warning = notRejectedIds.Count == 0 ? true : false,
            };
        }

        public async Task<ResultDto> Override(AuditHistoryQueryDto dto)
        {
            ResultDto error = new ResultDto();
            List<string> notOverideIds = new List<string>();
            bool isOverride = true;
            NetworkElementAsIsAttributes attributes = new NetworkElementAsIsAttributes();

            for (int count = 0; count < dto.Audithistoryid.Count; count++)
            {
                var pkId = dto.Audithistoryid[count];
                var overrideValue = dto.Newvalue[count];
                var audit = _repositoryWrapper.AuditHistory.FindByCondition(x => x.Audithistoryid.ToString() == pkId.ToString()).FirstOrDefault();
                if (audit != null)
                {
                    if (audit.Status.Trim() == AuditHistortStatus.AUTO_APPROVED.ToString())
                    {
                        if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.NetworkElementsAsIs)
                        {
                            var networkasis = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Networkelementasisid == audit.Primarykey).FirstOrDefault();
                            if (networkasis != null)
                            {

                                Type type = networkasis.GetType();
                                var columnName = GetColumnName(type, audit);
                                var property = type.GetProperty(columnName);
                                string ActualDBValue = property.GetValue(networkasis).ToString();                  
                                var val2 = audit.Newvalue != null ? audit.Newvalue.ToString() : "";
                                if (OverrideAudit(audit, ActualDBValue, overrideValue))
                                {
                                    if (DateTime.TryParseExact(overrideValue, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate)
                        || DateTime.TryParseExact(overrideValue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                        || DateTime.TryParseExact(overrideValue, "dd-MMM-yy hh.mm.ss.ffffff tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                                    {

                                        DateTime oldValue = parsedDate;
                                        property.SetValue(networkasis, oldValue);
                                        _repositoryWrapper.NetworkElementAsIs.Update(networkasis);
                                       
                                    }
                                    else
                                    {
                                        property.SetValue(networkasis, overrideValue);
                                        _repositoryWrapper.NetworkElementAsIs.Update(networkasis);
                                    }
                                    _repositoryWrapper.Save();
                                }                            
                                else
                                {
                                    notOverideIds.Add(pkId.ToString());
                                }
                            }
                            else
                            {
                                notOverideIds.Add(pkId.ToString());
                            }

                        }
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.NetworkElementsAsPlannned)
                        {
                            var asset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == audit.Primarykey)
                                               .Include(x => x.Opco)
                                                .Include(x => x.Plannedactivities)
                                                .Include(x => x.Deploymentstatus)
                                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                                                .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource);
                           
                            var rawNetworkEntity = _repositoryWrapper.NetworkElement.FindByCondition(x => x.Opco.Replace("VODAFONE_UK", "UK").ToLower() == audit.Opco.ToLower()
                            && x.Elementname.ToLower() == audit.Elementname.ToLower() && x.Oem.ToLower() == audit.Oem.ToLower()).FirstOrDefault();

                            var ActualDBValue = rawNetworkEntity.Softwarereleaseinformation;
                            if (asset != null && rawNetworkEntity!=null)
                            {
                                if (OverrideAudit(audit, ActualDBValue, overrideValue))
                                {

                                    var data = _reconciliation.AddReconciliation(new ReconciliationCreateDto()
                                    {
                                        OpCo = audit.Opco,
                                        ElementName = audit.Elementname,
                                        DeploymentStatus = asset.Select(x => x.Deploymentstatus.Deploymentstatus).FirstOrDefault(),
                                        CurrentSwVersion = audit.Oldvalue,
                                        NewSwVersion = audit.Newvalue,
                                        Status = "No Action Required",
                                        AssetId = asset.Select(x => x.Networkelementasplannedid).FirstOrDefault()
                                    }
                                    );
                                    rawNetworkEntity.Softwarereleaseinformation = overrideValue;
                                    _repositoryWrapper.NetworkElement.Update(rawNetworkEntity);
                                    _repositoryWrapper.Save();
                                    await _repositoryWrapper.ClearTracker();
                                    
                                }
                                else
                                {
                                    notOverideIds.Add(pkId.ToString());
                                }

                            }

                        }
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.NetworkElement && attributes.AsIsAttributes.Contains(audit.Columnname.ToLower()))
                        {
                            var opCoid = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.ToLower().Trim() == audit.Opco.Replace("VODAFONE_UK", "UK").ToLower().Trim()).FirstOrDefault()?.Opcoid;
                            var oemid = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Originalequipmentmanufacturer.ToLower().Trim() == audit.Oem.ToLower().Trim()).FirstOrDefault()?.Orgeqpmanufacturerid;

                            var checkNetworkAsIsExit = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Opcoid == opCoid && x.Orgeqpmanufacturerid == oemid &&
                            x.Elementdeploymentname.ToLower().Trim() == audit.Elementname.ToLower().Trim()).FirstOrDefault();

                            if (checkNetworkAsIsExit != null)
                            {
                                networkElement.Warning = UpdateNetworkElementASIS(audit,isOverride, overrideValue);                             
                            }
                            else
                            {

                            }
                            if (!networkElement.Warning)
                            {
                                notOverideIds.Add(pkId.ToString());
                            }
                        }
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.HardwareConfiguration && attributes.AsIsAttributes.Contains(audit.Columnname.ToLower()))
                        {
                            var opCoid = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.ToLower().Trim() == audit.Opco.Replace("VODAFONE_UK", "UK").ToLower().Trim()).FirstOrDefault()?.Opcoid;
                            var oemid = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Originalequipmentmanufacturer.ToLower().Trim() == audit.Oem.ToLower().Trim()).FirstOrDefault()?.Orgeqpmanufacturerid;

                            var checkNetworkAsIsExit = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Opcoid == opCoid && x.Orgeqpmanufacturerid == oemid &&
                            x.Elementdeploymentname.ToLower().Trim() == audit.Elementname.ToLower().Trim()).FirstOrDefault();

                            if (checkNetworkAsIsExit != null)
                            {
                                hardConfig.Warning = UpdateNetworkElementASIS(audit, isOverride, overrideValue);
                            }
                            else
                            {

                            }
                            if (!hardConfig.Warning)
                            {
                                notOverideIds.Add(pkId.ToString());
                            }
                        }
                        else if (audit.Tablename.ToLower().Trim() == ConstantValueFilter.Identities && attributes.AsIsAttributes.Contains(audit.Columnname.ToLower()))
                        {
                            var opCoid = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.ToLower().Trim() == audit.Opco.Replace("VODAFONE_UK", "UK").ToLower().Trim()).FirstOrDefault()?.Opcoid;
                            var oemid = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Originalequipmentmanufacturer.ToLower().Trim() == audit.Oem.ToLower().Trim()).FirstOrDefault()?.Orgeqpmanufacturerid;

                            var asset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == opCoid && x.Orgeqpmanufacturerid == oemid &&
                            x.Elementname.ToLower().Trim() == audit.Elementname.ToLower().Trim()).Include(x=>x.Identitiesasis).FirstOrDefault();

                            if (asset != null)
                            {
                                identityAsis.Warning = UpdateIdentityASIS(audit, isOverride, overrideValue,opCoid,oemid,asset.Networkelementasplannedid);
                            }
                            else
                            {

                            }
                            if (!identityAsis.Warning)
                            {
                                notOverideIds.Add(pkId.ToString());
                            }
                        }
                        else
                        {
                            // console.writeln(audit.tablename + "not handled");
                        }
                    }
                    else
                    {
                        notOverideIds.Add(pkId.ToString());
                    }
                }
                else
                {
                    notOverideIds.Add(pkId.ToString());
                }
            }
            return new ResultDto
            {
                Info = notOverideIds.Count == 0 ? ConstantValueFilter.SuccessfullyRejected : $"{ConstantValueFilter.FollowingIdsarenotOverride} : " + string.Join(",", notOverideIds),
                Warning = notOverideIds.Count == 0 ? true : false,
            };
        }

        public string GetColumnName(Type type, Audithistory _audithistory)
        {
            var parameter = Expression.Parameter(type);
            var propertyAccess = Expression.Property(parameter, _audithistory.Columnname);
            var columnName = propertyAccess.Member.Name;
            return columnName;

        }

        public bool OverrideAudit(Audithistory audit, string ActualDBValue, string overrideValue)
        {
            var val1 = ActualDBValue != null ? ActualDBValue.ToString() : "";
            var val2 = audit.Newvalue != null ? audit.Newvalue.ToString() : "";
            if (DateTime.TryParseExact(val2, "dd-MMM-yy hh.mm.ss.ffffff tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                val2 = parsedDate.ToString("M/d/yyyy h:mm:ss tt");
            }
            else
            {
                val2 = audit.Newvalue != null ? audit.Newvalue.ToString() : "";
            }
            if (val1.Equals(val2))
            {
                //Rejecting the current value
                audit.Status = AuditHistortStatus.REJECTED.ToString();
                _repositoryWrapper.AuditHistory.Update(audit);

                Audithistory newAudit = new Audithistory();
                newAudit.Opco = audit.Opco;
                newAudit.Oem = audit.Oem;
                newAudit.Elementname = audit.Elementname;
                newAudit.Oldvalue = audit.Newvalue;
                newAudit.Newvalue = overrideValue;
                newAudit.Tablename = audit.Tablename;
                newAudit.Columnname = audit.Columnname;
                newAudit.Primarykey = audit.Primarykey;
                newAudit.Status = AuditHistortStatus.OVERRIDE.ToString();
                _repositoryWrapper.AuditHistory.Create(newAudit);

                _repositoryWrapper.Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region // NetworkAsisINsertion
        public async Task<ResultDto> UpdateNetworkElement(Audithistory audit,long pkId,bool isOverRide, string overrideValue)
        {
            var networkElement = _repositoryWrapper.NetworkElement.FindByCondition(x => x.Networkelementid == audit.Primarykey).FirstOrDefault();
            bool isSuccessUpdate = false;
            var isApproved = false;
            ConvertDateTimeDto result = new ConvertDateTimeDto();
            if (networkElement != null)
            {

                Type type = networkElement.GetType();
                var columnName = GetColumnName(type, audit);
                var property = type.GetProperty(columnName);
                var columnValue = property.GetValue(networkElement);
                result = ConvertDateTime(columnValue, audit,isOverRide, overrideValue);
                
                if (result.Warning)
                {

                    if (result.OldVlaueforDateRange != null)
                    {
                        property.SetValue(networkElement, result.OldVlaueforDateRange);                        
                        isSuccessUpdate = UpdateNetworkElementASIS(audit, isOverRide, overrideValue);
                        
                        if (isSuccessUpdate)
                        {
                            _repositoryWrapper.NetworkElement.Update(networkElement);
                        }

                    }
                    else
                    {
                        property.SetValue(networkElement,result.OldValue);

                        if (audit.Columnname == ConstantValueFilter.Softwarereleaseinformation)
                        {
                            isApproved = await CheckingExistRecorsInAudit(audit.Elementname.Trim().ToLower());
                            isSuccessUpdate = true;
                        }
                        else
                        {
                            isApproved = true;
                        }
                        if (isApproved)
                        {
                            isSuccessUpdate = UpdateNetworkElementASIS(audit, isOverRide, overrideValue);
                        }
                        if (isSuccessUpdate)
                        {
                            _repositoryWrapper.NetworkElement.Update(networkElement);
                        }
                    }

                    if (isSuccessUpdate)
                    {
                        audit.Status = AuditHistortStatus.REJECTED.ToString();
                        _repositoryWrapper.AuditHistory.Update(audit);
                        _repositoryWrapper.Save();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                else
                {
                    result.Warning = false;
                }
            }
            return new ResultDto
            {
                Warning = isSuccessUpdate,
                Info = result.Warning?"":pkId.ToString()
            };
        }

        public async Task<ResultDto> UpdateHardwareConfig(Audithistory audit, long pkId, bool isOverRide,string overrideValue)
        {
            var haedConfig = _repositoryWrapper.HardwareConfiguration.FindByCondition(x => x.Hardwareconfigurationid == audit.Primarykey).FirstOrDefault();
            bool isSuccessUpdate = false;
             
            ConvertDateTimeDto result = new ConvertDateTimeDto();
            if (haedConfig != null)
            {
                Type type = haedConfig.GetType();
                var columnName = GetColumnName(type, audit);
                var property = type.GetProperty(columnName);
                var columnValue = property.GetValue(haedConfig);
                result = ConvertDateTime(columnValue, audit,isOverRide, overrideValue);

                if (!(result.Warning))
                {

                    if (result.OldVlaueforDateRange != null)
                    {
                        property.SetValue(haedConfig, result.OldVlaueforDateRange);
                        isSuccessUpdate = UpdateNetworkElementASIS(audit, isOverRide, overrideValue);
                        if (isSuccessUpdate)
                        {
                            _repositoryWrapper.HardwareConfiguration.Update(haedConfig);
                        }

                    }
                    else
                    {
                        property.SetValue(haedConfig, result.OldValue);
                        isSuccessUpdate = UpdateNetworkElementASIS(audit, isOverRide, overrideValue);
                        if (isSuccessUpdate)
                        {
                            _repositoryWrapper.HardwareConfiguration.Update(haedConfig);
                        }
                    }

                    if (isSuccessUpdate)
                    {
                        audit.Status = AuditHistortStatus.REJECTED.ToString();
                        _repositoryWrapper.AuditHistory.Update(audit);
                        _repositoryWrapper.Save();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                else
                {
                    result.Warning = false;
                }
            }
            return new ResultDto
            {
                Warning = isSuccessUpdate,
                Info = result.Warning ? "" : pkId.ToString()
            };
        }

        public bool UpdateNetworkElementASIS(Audithistory audit,bool isOverRide, string overrideValue)
        {
            bool isSuccess = false;
            ConvertDateTimeDto result = new ConvertDateTimeDto();

            var opCoid = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.ToLower().Trim() == audit.Opco.Replace("VODAFONE_UK","UK").ToLower().Trim()).FirstOrDefault()?.Opcoid;
            var oemid = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Originalequipmentmanufacturer.ToLower().Trim() == audit.Oem.ToLower().Trim()).FirstOrDefault()?.Orgeqpmanufacturerid;

            var networkAsIs = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Opcoid == opCoid
            && x.Elementdeploymentname.ToLower().Trim() == audit.Elementname.ToLower().Trim() && x.Orgeqpmanufacturerid == oemid).FirstOrDefault();

            if (networkAsIs != null)
            {
                if (audit.Columnname.ToLower().Trim() == ConstantValueFilter.SoftwareProductDate)
                {
                    audit.Columnname = ConstantValueFilter.SoftwareProductionDate;
                }
                if(audit.Columnname == ConstantValueFilter.Spare1ossorenm)
                {
                    audit.Columnname = ConstantValueFilter.ElementManager.ToLower();
                }

                Type type = networkAsIs.GetType();
                var columnName = GetColumnName(type, audit);
                var property = type.GetProperty(columnName);
                var columnValue = property.GetValue(networkAsIs);
                result = ConvertDateTime(columnValue, audit, isOverRide, overrideValue);
                if (result.Warning)
                {

                    if (result.OldVlaueforDateRange != null)
                    {
                        property.SetValue(networkAsIs, result.OldVlaueforDateRange);
                        _repositoryWrapper.NetworkElementAsIs.Update(networkAsIs);
                        isSuccess = true;

                    }
                    else
                    {
                        property.SetValue(networkAsIs, result.OldValue);
                        _repositoryWrapper.NetworkElementAsIs.Update(networkAsIs);
                        isSuccess = true;
                    }

                    _repositoryWrapper.Save();
                    _repositoryWrapper.ClearTracker();
                }
                //else
                //{
                //    result.Warning = false;
                //}
            }
            else
            {
                isSuccess = true;
            }

            return isSuccess;
        }

        public bool UpdateIdentityASIS(Audithistory audit, bool isOverRide, string overrideValue, short? opCoId, short? oemId, long assetId)
        {
            bool isSuccess = false;
            ConvertDateTimeDto result = new ConvertDateTimeDto();
           
            var IdentityAsIs = _repositoryWrapper.IdentityAsIsRepository.FindByCondition(x =>x.Assetid == assetId && audit.Newvalue == x.Value ).FirstOrDefault();

            if (IdentityAsIs != null)
            {
                if (audit.Columnname.ToLower().Trim() == ConstantValueFilter.IPAddress.ToLower().Trim())
                {
                    audit.Columnname = ConstantValueFilter.Value;
                }   
                Type type = IdentityAsIs.GetType();
                var columnName = GetColumnName(type, audit);
                var property = type.GetProperty(columnName);
                var columnValue = property.GetValue(IdentityAsIs);
                result = ConvertDateTime(columnValue, audit, isOverRide, overrideValue);
                if (result.Warning)
                {

                    if (result.OldVlaueforDateRange != null)
                    {
                        property.SetValue(IdentityAsIs, result.OldVlaueforDateRange);
                        _repositoryWrapper.IdentityAsIsRepository.Update(IdentityAsIs);
                        isSuccess = true;

                    }
                    else
                    {
                        property.SetValue(IdentityAsIs, result.OldValue);
                        _repositoryWrapper.IdentityAsIsRepository.Update(IdentityAsIs);
                        isSuccess = true;
                    }

                    _repositoryWrapper.Save();
                    _repositoryWrapper.ClearTracker();
                }
                //else
                //{
                //    result.Warning = false;
                //}
            }

            return isSuccess;
        }

        public ConvertDateTimeDto ConvertDateTime(object columnValue, Audithistory audit,bool isOverRide,string overrideValue)
        {
            ConvertDateTimeDto result = new ConvertDateTimeDto();
            var val1 = columnValue != null ? columnValue.ToString() : "";
            var val2 = audit.Newvalue != null ? audit.Newvalue.ToString() : "";

           
            if (isOverRide)
            {
                if(OverrideAudit(audit, columnValue.ToString(), overrideValue))
                {
                    if (DateTime.TryParseExact(overrideValue, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate)
                        || DateTime.TryParseExact(overrideValue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                        || DateTime.TryParseExact(overrideValue, "dd-MMM-yy hh.mm.ss.ffffff tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                    {

                        DateTime oldValue = parsedDate;
                        result.OldVlaueforDateRange = oldValue;
                        result.Warning = true;

                    }
                    else
                    {
                        result.OldValue = overrideValue;
                        result.Warning = true;
                    }
                }
                result.Warning = false;
            }
            else
            {

                if (DateTime.TryParseExact(audit.Oldvalue, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate)
                        || DateTime.TryParseExact(audit.Oldvalue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                        || DateTime.TryParseExact(audit.Oldvalue, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                        || DateTime.TryParseExact(audit.Oldvalue, "dd-MMM-yy hh.mm.ss.ffffff tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    DateTime oldValue = parsedDate;
                    result.OldVlaueforDateRange = oldValue;
                    result.Warning = true;
                }
                else
                {
                    result.OldValue = audit.Oldvalue;
                    result.Warning = true;
                }
               
            }
            return result;
        }
        #endregion

        public async Task<bool> CheckingExistRecorsInAudit(string Elementname)
        {
            var isResult = false;
            try
            {
                var auditEntity = await _repositoryWrapper.AuditHistory.FindByCondition(x => x.Elementname.Trim().ToLower() == Elementname
                && x.Opco.Trim().ToLower() == "uk" && x.Oem.Trim().ToLower() == "ericsson"
                && x.Columnname.Trim().ToLower() == "softwareversion").FirstOrDefaultAsync();

                if(auditEntity != null)
                {
                    if(auditEntity.Status != AuditHistortStatus.REJECTED.ToString())
                    {
                        isResult = true;
                    }
                }
                else
                {
                    isResult = true;
                }

                return isResult;
            }
            catch
            {
                throw;
            }
            
        }
    }
}
