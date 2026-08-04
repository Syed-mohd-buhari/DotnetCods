using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AuditHistory;
using CAM.DataTransferObjects.Entita.Reconsiliation;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Drawing.Charts;
using IdentityServer4.Extensions;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CAM.Enum.AuditHistortStatusEnum;

namespace CAM.BusinessManager.Entity
{
    public class ReconciliationManager : BaseManager 
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private ICurrentUserService _currentUserService;
        private GridCustomColumnManager _manager;        
        public ReconciliationManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _currentUserService = currentUserService;
        }

        #region //UI Member
        public async Task<QueryResultDto<ReconciliationGridDto>> FindWithCondition(ReconciliationQueryDto dto)
        {
            var predicateResult = ApplyFilter(dto);
            var rtn = new QueryResultDto<ReconciliationGridDto>(new GenerateRenderForGrid<ReconciliationGridDto>(_manager))
            {
            };
            var query = GetQuery(predicateResult).ApplyOrdering(dto, GetColumnsMap());
            
            rtn.TotalItems = query.Count(); 
            query = query.ApplyPaging(dto);

            foreach (var item in query.ToList())
            {
                var paId = GetPARecord(item.AssetId, _repositoryWrapper,item.Status);
                var updateEntity = ReconciliationMapper.Set(item);

                if (paId != 0 && item.Status.ToLower().Trim() == ConstantValueFilter.CreatePlannedActivity.ToLower().Trim())
                {
                    updateEntity.Status = ConstantValueFilter.EditPlannedActivity;
                }
                else if (paId == 0 && item.Status.ToLower().Trim() == ConstantValueFilter.CreatePlannedActivity.ToLower().Trim())
                {
                    break;
                } 
                else if (paId != 0 && item.Status.ToLower().Trim() == ConstantValueFilter.EditPlannedActivity.ToLower().Trim())
                {
                    break; // don't do anything
                }
                else
                {
                    updateEntity.Status = ConstantValueFilter.NoActionRequired;
                }
                _repositoryWrapper.ReconciliationRepository.Update(updateEntity);
            }
            _repositoryWrapper.Save();
            await _repositoryWrapper.ClearTracker();

            var data = query.ToList();

            IEnumerable<ReconciliationGridDto> reconciliatioinDto;

            reconciliatioinDto = _mapper.Map<IEnumerable<ReconciliationGridDto>>(data);

            rtn.Items = reconciliatioinDto.ToArray();
            return rtn;
        }     
       
        private static ExpressionStarter<Reconciliations> ApplyFilter(ReconciliationQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Reconciliations>();
            var predicateInner = PredicateBuilder.New<Reconciliations>();


            if (buildFilterDto.AssetsId != null && buildFilterDto.AssetsId.Any())
            {
                predicateInner = PredicateBuilder.New<Reconciliations>();
                foreach (var item in buildFilterDto.AssetsId)
                    predicateInner.Or(x => x.Assetid == item);
                predicateResult.And(predicateInner);
            }
             
            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Reconciliations>();
                foreach (var item in buildFilterDto.OpCo)
                    predicateInner.Or(x => x.Opco == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DeploymentStatus != null && buildFilterDto.DeploymentStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Reconciliations>();
                foreach (var item in buildFilterDto.DeploymentStatus)
                    predicateInner.Or(x => x.Deploymentstatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ElementName != null && buildFilterDto.ElementName.Any())
            {
                predicateInner = PredicateBuilder.New<Reconciliations>();
                foreach (var item in buildFilterDto.ElementName)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CurrentSWVersion != null && buildFilterDto.CurrentSWVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Reconciliations>();
                foreach (var item in buildFilterDto.CurrentSWVersion)
                    predicateInner.Or(x => x.Currentswversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NewSWVersion != null && buildFilterDto.NewSWVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Reconciliations>();
                foreach (var item in buildFilterDto.NewSWVersion)
                    predicateInner.Or(x => x.Newswversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Status != null && buildFilterDto.Status.Any())
            {
                predicateInner = PredicateBuilder.New<Reconciliations>();
                foreach (var item in buildFilterDto.Status)
                    predicateInner.Or(x => x.Status == item);
                predicateResult.And(predicateInner);
            }



            return predicateResult;
        }
        public IQueryable<ReconciliationModel> GetQuery(ExpressionStarter<Reconciliations> predicateResult)
        {

            var query = predicateResult.IsStarted ?
                _repositoryWrapper.ReconciliationRepository.FindByCondition(predicateResult)
                                    .Include(x => x.CreationuserNavigation)
                                    .Include(x => x.ModificationuserNavigation)
                : _repositoryWrapper.ReconciliationRepository.FindAll()
                                    .Include(x => x.CreationuserNavigation)
                                    .Include(x => x.ModificationuserNavigation);
            return query.AsEnumerable().Select(x => ReconciliationMapper.Get(x)).AsQueryable(); ;
        }

        private Dictionary<string, Expression<Func<ReconciliationModel, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ReconciliationModel, object>>[]>
            {
                ["assetsId"] = new Expression<Func<ReconciliationModel, object>>[] { p => p.AssetId },
                ["opCo"] = new Expression<Func<ReconciliationModel, object>>[] { p => p.OpCo },
                ["elementName"] = new Expression<Func<ReconciliationModel, object>>[] { p => p.ElementName },
                ["creationDate"] = new Expression<Func<ReconciliationModel, object>>[] { p => p.Creationdate },
                ["creationUser"] = new Expression<Func<ReconciliationModel, object>>[] { p => p.Creationuser },
                ["modificationDate"] = new Expression<Func<ReconciliationModel, object>>[] { p => p.Modificationdate },
                ["modificationUser"] = new Expression<Func<ReconciliationModel, object>>[] { p => p.Modificationuser },

            };
        }

        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, ReconciliationQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult);                           
            var rtn = propertyName switch
            {
                "opCo" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.OpCo, Value = p.OpCo }).Distinct().ToList()
               : query
                   .Where(x => x.OpCo.Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.OpCo, Value = p.OpCo }).Distinct()
                   .ToList(),

                "deploymentStatus" => string.IsNullOrEmpty(propertyFilter)
             ? query.Select(p => new FilterValueDto
             { Text = p.DeploymentStatus, Value = p.DeploymentStatus }).Distinct().ToList()
             : query
                 .Where(x => x.DeploymentStatus.Contains(propertyFilter)).Select(p =>
                     new FilterValueDto { Text = p.DeploymentStatus, Value = p.DeploymentStatus }).Distinct()
                 .ToList(),


                "elementName" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.ElementName, Value = p.ElementName }).Distinct().ToList()
                : query
                    .Where(x => x.ElementName.Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.ElementName, Value = p.ElementName }).Distinct()
                    .ToList(),

                "currentSwVersion" => string.IsNullOrEmpty(propertyFilter)
           ? query.Select(p => new FilterValueDto
           { Text = p.CurrentSwVersion, Value = p.CurrentSwVersion }).Distinct().ToList()
           : query
               .Where(x => x.CurrentSwVersion.Contains(propertyFilter)).Select(p =>
                   new FilterValueDto { Text = p.CurrentSwVersion, Value = p.CurrentSwVersion }).Distinct()
               .ToList(),


                "newSwVersion" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.NewSwVersion, Value = p.NewSwVersion }).Distinct().ToList()
                    : query
                        .Where(p => (p.NewSwVersion).Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.NewSwVersion, Value = p.NewSwVersion }).Distinct()
                        .ToList(),

                "status" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.Status, Value = p.Status }).Distinct().ToList()
                    : query
                        .Where(p => (p.NewSwVersion).Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Status, Value = p.Status }).Distinct()
                        .ToList(),



                _ => new List<FilterValueDto>()
            };

            return rtn;
        }
        #endregion

        #region //Put a entry for audit page
        public async Task<ResultDto> AddEntryForAudit(ReconciliationQueryDto dto)
        {
            var predicateResult = ApplyFilter(dto);
            var query = GetQuery()
                     .Join(_repositoryWrapper.NetworkElement.FindAll(),
                         x => new { ElementName = x.Elementname.ToLower(), Opco = x.Opco.Opco.ToLower(), Oem = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Trim() },
                      ne => new { ElementName = ne.Elementname.ToLower(), Opco = ne.Opco.Replace("VODAFONE_UK", "UK").ToLower(), Oem = ne.Oem.ToLower().Trim() },
                      (x, ne) => new { x, ne });

            query = query.OrderByDescending(x => x.x.Modificationdate);
            var data = query.ToList();
            var result = data.Select(y =>
            {               
                var grid = new ReconciliationGridDto();
                var deploymentstatus = y.x.Deploymentstatus?.Deploymentstatus.ToLower().Replace(" ", "") == "in-service";
                var currentVersion = y.x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Softwareversion;
                var updateVersion = y.ne.Softwarereleaseinformation;
                var AssetsId = y.x.Networkelementasplannedid;                
                var OpCo = y.x.Opco.Opco;
                var ElementName = y.x.Elementname;
                var status = deploymentstatus && currentVersion != null && updateVersion != null &&
                               CompareVersions(currentVersion, updateVersion, OpCo, AssetsId, ElementName, y.ne.Oem.ToLower().Trim()) > 0;                               
                               
                return grid;
                
            }).ToList().Distinct();
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess,
                    Data = result

                };
        }

        public IQueryable<Networkelementsasplanned> GetQuery()
        {

            var query = _repositoryWrapper.NetworkElementAsPlanned.FindAll().Where(x=>x.Lcmengineeringid != null)
                                    .Include(x => x.Opco)
                                    //.Include(x => x.Plannedactivities) this is not called antywhere
                                    .Include(x => x.Deploymentstatus)
                                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                                    //.Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource)
                                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer);
            return query;
        }

        private int CompareVersions(string currentVersion, string updateVersion, string Opco, long primaryKey, string ElementName, string oem)
        {
            var oemDes = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Originalequipmentmanufacturer.ToLower().Trim() == oem).FirstOrDefault().Originalequipmentmanufacturer;

            string[] _cversion = currentVersion.Split('.', '-');
            string[] _uversion = updateVersion.Split('.', '-');

            for (int i = 0; i < Math.Max(_cversion.Length, _uversion.Length); i++)
            {
                string getCV = i < _cversion.Length ? _cversion[i] : "0";
                string getUV = i < _uversion.Length ? _uversion[i] : "0";

                bool isCvNumberic = ContainsNumber(getCV);
                bool isUvNumberic = ContainsNumber(getUV);
                if (isCvNumberic && isUvNumberic)
                {
                    int CV = Convert.ToInt32(new string(getCV.Where(char.IsDigit).ToArray()));
                    int UV = Convert.ToInt32(new string(getUV.Where(char.IsDigit).ToArray()));

                    if (CV > UV)
                        return -1;
                    else if (CV < UV)
                    {
                        var data = Add(new AuditHistoryCreateDto()
                        {
                            OpCo = Opco,
                            Oem = oemDes,
                            ElementName = ElementName,
                            PrimaryKey = primaryKey,
                            TableName = "Networkelementsasplannned",
                            ColumnName = "Softwareversion",
                            OldValue = currentVersion,
                            NewValue = updateVersion,
                            Status = AuditHistortStatus.AUTO_APPROVED.ToString(),
                            Creationuser = 1,
                            Modificationuser = 1,
                        });
                        return 1;
                    }
                }
                else if (isCvNumberic)
                {
                    return -1;
                }
                else if (isUvNumberic)
                {
                    var data = Add(new AuditHistoryCreateDto()
                    {
                        OpCo = Opco,
                        Oem = oemDes,
                        ElementName = ElementName,
                        PrimaryKey = primaryKey,
                        TableName = "Networkelementsasplannned",
                        ColumnName = "Softwareversion",
                        OldValue = currentVersion,
                        NewValue = updateVersion,
                        Status = AuditHistortStatus.AUTO_APPROVED.ToString(),
                        Creationuser = 1,
                        Modificationuser = 1,
                    });
                    return 1;
                }
            }

            return 0;
        }

        private bool ContainsNumber(string version)
        {
            return version.Any(char.IsDigit);
        }



        #endregion

        #region // Create Audit 
        public async Task<ResultDto> Add(AuditHistoryCreateDto dto)
        {
            try
            {
                var checkAuditHistory = _repositoryWrapper.AuditHistory.FindByCondition(x => x.Opco.Trim().ToLower() == dto.OpCo.Trim().ToLower() && x.Oem.Trim().ToLower() == dto.Oem.Trim().ToLower()
                && x.Elementname.Trim().ToLower() == dto.ElementName.Trim().ToLower() &&
                x.Oldvalue.Trim().ToLower() == dto.OldValue.Trim().ToLower() && x.Newvalue.Trim().ToLower() == dto.NewValue.Trim().ToLower() && x.Primarykey.ToString().Trim().ToLower() == dto.PrimaryKey.ToString().Trim().ToLower()).FirstOrDefault();
                if (checkAuditHistory == null)
                {
                    var entityMap = _mapper.Map<AuditHistory>(dto);
                    var auditEntity = AuditHistroyMapper.Set(entityMap);
                    _repositoryWrapper.AuditHistory.Create(auditEntity);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                    return new ResultDto
                    {
                        Info = ResultMessages.EntryAddSuccess
                    };
                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess
                };
            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Warning = true
                };
            }
        }

        public async Task<ResultDto> AddReconciliation(ReconciliationCreateDto dto)
        {
            try
            {
                var checkEntryExist = _repositoryWrapper.ReconciliationRepository.FindByCondition(x => x.Opco == dto.OpCo && x.Elementname == dto.ElementName
                && x.Status != dto.Status && x.Currentswversion == dto.CurrentSwVersion && x.Newswversion == dto.NewSwVersion).FirstOrDefault();
                if (checkEntryExist == null)
                {
                    var entityMap = _mapper.Map<ReconciliationModel>(dto);
                    var reconciliatioinEntity = ReconciliationMapper.Set(entityMap);
                    _repositoryWrapper.ReconciliationRepository.Create(reconciliatioinEntity);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                    return new ResultDto
                    {
                        Info = ResultMessages.EntryAddSuccess
                    };
                }
                else if (checkEntryExist != null)
                {
                    checkEntryExist.Status = dto.Status;
                    _repositoryWrapper.ReconciliationRepository.Update(checkEntryExist);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess
                };
            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Warning = true
                };
            }
        }
        #endregion

        public static long? GetLcmRecord(long? assetId, IRepositoryWrapper _repositoryWrapper)
        {            
            try
            {
                var lcmid = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == assetId);
                return lcmid.Select(x=>x.Lcmengineeringid).FirstOrDefault();
               
            }   
            catch(Exception e)
            {
                return 0;
            }
        }
        public static long GetPARecord(long? assetId, IRepositoryWrapper _repositoryWrapper, string status = "")
        {
            long plannnedId = 0;
            try
            {
                var assetEntity = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == assetId).Include(x=>x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering);

                if(!status.IsNullOrEmpty() && status.ToLower().Trim() == ConstantValueFilter.NoActionRequired.ToLower().Trim())
                {
                    return plannnedId;
                }
                else
                {
                    plannnedId = assetEntity.ToList().Select(x => x.Lcmengineering.PlannedactivitiesLcmengineering.Select(x => x.Plannedactivityid).FirstOrDefault()).FirstOrDefault();
                }
                return plannnedId;
            }
            catch (Exception e)
            {                
                return 0;
            }
        }

        public async Task<ResultDto> Update(long dto)
        {
            var isSuccess = true;
            try
            {
                if (dto != null )
                {
                    var entityReconciliation = _repositoryWrapper.ReconciliationRepository.FindByCondition(x => x.Reconciliationid == dto).FirstOrDefault();
                    if(entityReconciliation != null)
                    {
                        entityReconciliation.Status = "No Action Required";
                        _repositoryWrapper.ReconciliationRepository.Update(entityReconciliation);
                        _repositoryWrapper.Save();
                        isSuccess = false;
                        await _repositoryWrapper.ClearTracker();

                    }                                           
                }
                return new ResultDto
                {
                    Info = !isSuccess ? ResultMessages.EntryUpdateSuccess:"Not Updated",                   
                    Warning = !isSuccess ? false : true
                };
            }
            catch(Exception e)
            {
                return new ResultDto
                {
                    Info = e.Message,
                    Warning = true
                };
            }
        }

    }
}
