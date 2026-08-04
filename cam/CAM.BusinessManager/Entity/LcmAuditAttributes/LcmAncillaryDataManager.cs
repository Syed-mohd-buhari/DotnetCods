using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.LcmAncillaryData;
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
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class LcmAncillaryDataManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;

        public LcmAncillaryDataManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }


        #region //UiMemberFunctions
        public QueryResultDto<LcmAncillaryDataDtoGrid> FindWithCondition(LcmAncillaryDataQueryDto LcmAuditAttriDto)
        {
            var predicateResult = ApplyFilter(LcmAuditAttriDto);
            var rtn = new QueryResultDto<LcmAncillaryDataDtoGrid>(new GenerateRenderForGrid<LcmAncillaryDataDtoGrid>(_manager))
            {
               
            };
            var query = GetQuery(predicateResult, LcmAuditAttriDto.Deleted ?? false).ApplyOrdering(LcmAuditAttriDto, GetColumnsMap());           
            rtn.TotalItems = query.Count();
            query =query.ApplyPaging(LcmAuditAttriDto);
            var data = query.ToList();
                     
            IEnumerable <LcmAncillaryDataDtoGrid> LcmAncillaryDataDtoGrid;

            LcmAncillaryDataDtoGrid = _mapper.Map<IEnumerable<LcmAncillaryDataDtoGrid>>(data);

            #region columns related to UK TSR are present for other Opco's as well when in 'View Ancillary data' mode
            #region //Ticket 729 Filter Both For Edit And Add Asset Records in PA Grid while click Asset button in Manage Network Menu
            //Ticket 758 - Need to include ZZZ ' Opco also in TSR report along with UK & Group
             
            var checkLcmIsUKOpco = query.Where(x => ConstantValueFilter.tsrOpcos.Contains( x.LcmEngineering.OpCo.OpCoDescription.ToLower())).FirstOrDefault()?.LcmEngineeringId;
            #endregion
            if (checkLcmIsUKOpco == null)
            {
                rtn.GridRender.Render.Find(x => x.PropertyName == ConstantValueFilter.InfrastructureLocation).Show = false;
                rtn.GridRender.Render.Find(x => x.PropertyName == ConstantValueFilter.ExternalFacingFlag).Show = false;
                rtn.GridRender.Render.Find(x => x.PropertyName == ConstantValueFilter.RegulatoryFields).Show = false;
            }
            #endregion
            rtn.Items = LcmAncillaryDataDtoGrid.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Lcmancillarydata> ApplyFilter(LcmAncillaryDataQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Lcmancillarydata>();
            var predicateInner = PredicateBuilder.New<Lcmancillarydata>();

            if (buildFilterDto.LcmEngineeringId != null && buildFilterDto.LcmEngineeringId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmancillarydata>();
                foreach (var item in buildFilterDto.LcmEngineeringId)
                    predicateInner.Or(x => x.Lcmengineeringid == item);
                predicateResult.And(predicateInner);
            }          

            return predicateResult;
        }

        private IQueryable<LcmAncillaryData> GetQuery(ExpressionStarter<Lcmancillarydata> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.LcmAncillaryData.FindByCondition(predicateResult, includeDeleted)
                       .Include(x => x.CreationuserNavigation)
                       .Include(x => x.ModificationuserNavigation)
                       .Include(x=>x.Lcmengineering).ThenInclude(x=>x.Designcomponent)
                       .ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                       .Include(x=>x.Lcmengineering).ThenInclude(x=>x.Opco)
                       .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent)
                       .ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation)
               : _repositoryWrapper.LcmAncillaryData.FindAll()
                      .Include(x => x.CreationuserNavigation)
                      .Include(x => x.ModificationuserNavigation)
                      .Include(x=>x.Lcmengineering)
                     .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent)
                       .ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                      .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                      .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent)
                       .ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation);
            return query.AsEnumerable().Select(x => LcmAncillaryDataMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<LcmAncillaryData, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<LcmAncillaryData, object>>[]>
            {
                ["LcmAncillaryDataId"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.LcmAncillaryDataId },
                ["lcmEngineeringId"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.LcmEngineeringId },
                ["productCode"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.ProductCode },
                ["handedOverToOperation"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.HandedOverToOperation },
                ["contractRenewalPlan"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.ContractRenewalPlan },
                ["reasonForNoPlan"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.ReasonForNoPlan },
                ["commentOnProjectStatus"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.CommentOnProjectStatus },
                ["scopeOfSimplification"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.ScopeOfSimplification },
                ["dataSource"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.DataSource },
                ["incidentClass"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.IncidentClass },
                ["occurenceProbability"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.OccurenceProbability },
                //["securityRiskPotential"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.SecurityRiskPotential },
                ["securityRiskEffective"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.SecurityRiskEffective },
                ["securityMitigation"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.SecurityMitigation },
                ["assetOutOfScope"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.AssetOutOfScope },
                ["includedInSecurityScanning"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.IncludedInSecurityScanning },
                ["raId"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.RaId },
                ["requestId"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.RequestId },
                ["lastScanDate"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.LastScanDate },
                ["lastUpgradeDate"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.LastUpgradeDate },
                ["eomControl"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.EomControl },
                ["engUpdateTracker"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.EngUpdateTracker },
                ["opsUpdateTracker"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.OpsUpdateTracker },
                //["custom2"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.Custom2 },
                //["kpiStatusService"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.KpiStatusService },
                //["custom"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.Custom },
                //["custom1"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.Custom1 },
                //["idNew"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.IdNew },
                ["exNetworks"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.ExNetworks },
                //["productImportanceHistory2"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.ProductImportanceHistory2 },
                //["cloudVersion"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.CloudVersion },
                //["certifiedSWRealeseForNfviBundle"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.CertifiedSWRealeseForNfviBundle },
                //["lcmStatus"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.LcmStatus },
                ["modificationDate"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.ModificationDate },
                ["modificationUser"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.ModificationUser },
                ["creationUser"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.CreationUser },
                ["creationDate"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.CreationDate },
                ["originalSwLcmId"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.OriginalSwLcmId },
                ["originalHwLcmId"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.OriginalHwLcmId },
                #region // 
                ["isPecn"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.Ispecn },
                ["isPecs"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.Ispecs },
                ["isScf"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.Isscf },
                ["isNof"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.Isnof },

                #endregion

                //["regulatoryfields"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.RegulatoryFields },
                ["exposedEdgeFlag"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.ExposedEdgeFlag },
                ["locationInfrastructure"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.InfrastructureLocation },
                ["externalFacingFlag"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.ExternalFacingFlag },

                ["lastPenTestDate"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.LastPenTestDate },
                ["lastPenTestReferenceNumber"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.LastPenTestReferenceNumber },
                ["lastScanRefNumber"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.LastScanRefNumber },
                ["riskComment"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.RiskComment },
                ["qId"] = new Expression<Func<LcmAncillaryData, object>>[] { p => p.Qid },
            };
        }


        #endregion

        #region //CRUD

        public async Task<ResultDto> CreateLcmAncillaryData(LcmAncillaryDataCRUDDto dto)
        {           

            var lcmAuditEntity = _mapper.Map<LcmAncillaryData>(dto);
           
            if(dto.RegulatoryFields!=null && dto.RegulatoryFields.Count() > 0)
            {
                if (dto.RegulatoryFields.Keys.Contains(ConstantValueFilter.PECN))
                {
                    lcmAuditEntity.Ispecn = true;
                }
                if (dto.RegulatoryFields.Keys.Contains(ConstantValueFilter.PECS))
                {
                    lcmAuditEntity.Ispecs = true;
                }
                if (dto.RegulatoryFields.Keys.Contains(ConstantValueFilter.SCF))
                {
                    lcmAuditEntity.Isscf = true;
                }
                if (dto.RegulatoryFields.Keys.Contains(ConstantValueFilter.NOF))
                {
                    lcmAuditEntity.Isnof = true;
                }
            }
            //dto.LastPenTestDate = dto.LastPenTestDate != null ? dto.LastPenTestDate : null;
            var lcmAuditModel = LcmAncillaryDataMapper.Set(lcmAuditEntity);
            if (lcmAuditModel != null)
            {
                _repositoryWrapper.LcmAncillaryData.Create(lcmAuditModel);
                await _repositoryWrapper.SaveAsync();
            }

            return new ResultDto
            {
                Info = ResultMessages.EntryAddSuccess,
                Warning = false
            };
        }

        public async Task<ResultDto> UpdateLcmAncillaryData(LcmAncillaryDataCRUDDto dto)
        {


            var lcmAncillaryDataModel = _repositoryWrapper.LcmAncillaryData.FindByCondition(x => x.Lcmancillarydataid == dto.LcmAncillaryDataId).FirstOrDefault();
            var lcmAncillaryDataEntity = LcmAncillaryDataMapper.Get(lcmAncillaryDataModel);
            if (lcmAncillaryDataEntity != null)
            {
                lcmAncillaryDataEntity.ProductCode = dto.ProductCode;
                lcmAncillaryDataEntity.HandedOverToOperation = dto.HandedOverToOperation;
                lcmAncillaryDataEntity.ContractRenewalPlan = dto.ContractRenewalPlan;
                lcmAncillaryDataEntity.ReasonForNoPlan = dto.ReasonForNoPlan;
                lcmAncillaryDataEntity.CommentOnProjectStatus = dto.CommentOnProjectStatus;
                lcmAncillaryDataEntity.ScopeOfSimplification = dto.ScopeOfSimplification;
                lcmAncillaryDataEntity.DataSource = dto.DataSource;
                lcmAncillaryDataEntity.IncidentClass = dto.IncidentClass;
                lcmAncillaryDataEntity.OccurenceProbability = dto.OccurenceProbability;
                lcmAncillaryDataEntity.SecurityRiskEffective = dto.SecurityRiskEffective;
                lcmAncillaryDataEntity.SecurityMitigation = dto.SecurityMitigation;
                lcmAncillaryDataEntity.AssetOutOfScope = dto.AssetOutOfScope;
                lcmAncillaryDataEntity.IncludedInSecurityScanning = dto.IncludedInSecurityScanning;
                lcmAncillaryDataEntity.RaId = dto.RaId;
                lcmAncillaryDataEntity.RequestId = dto.RequestId;
                lcmAncillaryDataEntity.LastScanDate = dto.LastScanDate;
                lcmAncillaryDataEntity.LastUpgradeDate = dto.LastUpgradeDate;
                lcmAncillaryDataEntity.EomControl = dto.EomControl;
                lcmAncillaryDataEntity.EngUpdateTracker = dto.EngUpdateTracker;
                lcmAncillaryDataEntity.OpsUpdateTracker = dto.OpsUpdateTracker;                
                lcmAncillaryDataEntity.ExNetworks = dto.ExNetworks;                
                lcmAncillaryDataEntity.OriginalHwLcmId = dto.OriginalHwLcmId;
                lcmAncillaryDataEntity.OriginalSwLcmId = dto.OriginalSwLcmId;
                lcmAncillaryDataEntity.InfrastructureLocation = dto.InfrastructureLocation;

                lcmAncillaryDataEntity.ExternalFacingFlag = dto.ExternalFacingFlag;
                lcmAncillaryDataEntity.Vulnerabilityrating = dto.Vulnerabilityrating;
                lcmAncillaryDataEntity.Cyberriskrequestid = dto.Cyberriskrequestid;
                #region // Dev 719 Regulatory fields Changes              
                    if (dto.RegulatoryFields.Keys.Contains(ConstantValueFilter.PECN))
                    {
                        lcmAncillaryDataEntity.Ispecn = true;
                    }
                    if (!dto.RegulatoryFields.Keys.Contains(ConstantValueFilter.PECN))
                    {
                        lcmAncillaryDataEntity.Ispecn = false;
                    }
                    if (dto.RegulatoryFields.Keys.Contains(ConstantValueFilter.PECS))
                    {
                        lcmAncillaryDataEntity.Ispecs = true;
                    }
                    if (!dto.RegulatoryFields.Keys.Contains(ConstantValueFilter.PECS))
                    {
                        lcmAncillaryDataEntity.Ispecs = false;
                    }
                    if (dto.RegulatoryFields.Keys.Contains(ConstantValueFilter.SCF))
                    {
                        lcmAncillaryDataEntity.Isscf = true;
                    }
                    if (!dto.RegulatoryFields.Keys.Contains(ConstantValueFilter.SCF))
                    {
                        lcmAncillaryDataEntity.Isscf = false;
                    }
                    if (dto.RegulatoryFields.Keys.Contains(ConstantValueFilter.NOF))
                    {
                        lcmAncillaryDataEntity.Isnof = true;
                    }
                    if (!dto.RegulatoryFields.Keys.Contains(ConstantValueFilter.NOF))
                    {
                        lcmAncillaryDataEntity.Isnof = false;
                    }               
                #endregion
                
                lcmAncillaryDataEntity.Vulnerabilityrating = dto.Vulnerabilityrating;
                lcmAncillaryDataEntity.Cyberriskrequestid = dto.Cyberriskrequestid;
                lcmAncillaryDataEntity.LastScanRefNumber = dto.LastScanRefNumber;
                lcmAncillaryDataEntity.LastPenTestDate =  dto.LastPenTestDate;
                lcmAncillaryDataEntity.LastPenTestReferenceNumber = dto.LastPenTestReferenceNumber;
                lcmAncillaryDataEntity.ExposedEdgeFlag= dto.ExposedEdgeFlag;
                lcmAncillaryDataEntity.RiskComment= dto.RiskComment;
                lcmAncillaryDataEntity.Qid= dto.QId;
                var lcmAncillaryDataSetEntity = LcmAncillaryDataMapper.Set(lcmAncillaryDataEntity);
                _repositoryWrapper.LcmAncillaryData.Update(lcmAncillaryDataSetEntity);
                await _repositoryWrapper.SaveAsync();


            }                                

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = false,
                Data = lcmAncillaryDataEntity.LcmAncillaryDataId
            };
        }


        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.LcmAncillaryData
                .FindByCondition(x => x.Lcmancillarydataid == id).SingleAsync();

            if (entity != null)
            {
                _repositoryWrapper.LcmAncillaryData.Delete(entity);
                await _repositoryWrapper.SaveAsync();

                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Lcmancillarydataid
                };
            }

            else
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotExists,
                    Data = id
                };
        }
        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.LcmAncillaryData
               .FindByConditionWithDelete(x => x.Lcmancillarydataid == id).SingleAsync();
            if (entity != null)
            {
                _repositoryWrapper.LcmAncillaryData.DeleteDeep(entity);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Lcmancillarydataid
                };
            }

            else
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotExists,
                    Data = id
                };
        }
        #endregion

        #region // Mapping ancillary data
        public Lcmancillarydata MappingAncillaryData(Lcmancillarydata newLcmAncillaryEntity, Lcmancillarydata ancillaryEntity)
        {
            try
            {
                newLcmAncillaryEntity.Productcode = ancillaryEntity.Productcode;
                newLcmAncillaryEntity.Handedovertooperation = ancillaryEntity.Handedovertooperation;
                newLcmAncillaryEntity.Contractrenewalplan = ancillaryEntity.Contractrenewalplan;
                newLcmAncillaryEntity.Reasonfornoplan = ancillaryEntity.Reasonfornoplan;
                newLcmAncillaryEntity.Commentonprojectstatus = ancillaryEntity.Commentonprojectstatus;
                newLcmAncillaryEntity.Scopeofsimplification = ancillaryEntity.Scopeofsimplification;
                newLcmAncillaryEntity.Datasource = ancillaryEntity.Datasource;
                newLcmAncillaryEntity.Incidentclass = ancillaryEntity.Incidentclass;
                newLcmAncillaryEntity.Occurenceprobability = ancillaryEntity.Occurenceprobability;
                newLcmAncillaryEntity.Securityriskeffective = ancillaryEntity.Securityriskeffective;
                newLcmAncillaryEntity.Securitymitigation = ancillaryEntity.Securitymitigation;
                newLcmAncillaryEntity.Includedinsecurityscanning = ancillaryEntity.Includedinsecurityscanning;
                newLcmAncillaryEntity.Raid = ancillaryEntity.Raid;
                newLcmAncillaryEntity.Requestid = ancillaryEntity.Requestid;
                newLcmAncillaryEntity.Lastscandate = ancillaryEntity.Lastscandate;
                newLcmAncillaryEntity.Lastupgradedate = ancillaryEntity.Lastupgradedate;
                newLcmAncillaryEntity.Eomcontrol = ancillaryEntity.Eomcontrol;
                newLcmAncillaryEntity.Exnetworks = ancillaryEntity.Exnetworks;                
                newLcmAncillaryEntity.Originalhwlcmid = ancillaryEntity.Originalhwlcmid;
                newLcmAncillaryEntity.Originalswlcmid = ancillaryEntity.Originalswlcmid;
                newLcmAncillaryEntity.Externalfacingflag = ancillaryEntity.Externalfacingflag;
                newLcmAncillaryEntity.Locationinfrastructure = ancillaryEntity.Locationinfrastructure;
                newLcmAncillaryEntity.Ispecn = ancillaryEntity.Ispecn;
                newLcmAncillaryEntity.Ispecs = ancillaryEntity.Ispecs;
                newLcmAncillaryEntity.Isscf = ancillaryEntity.Isscf;
                newLcmAncillaryEntity.Isnof = ancillaryEntity.Isnof;
                newLcmAncillaryEntity.Vulnerabilityrating = ancillaryEntity.Vulnerabilityrating;
                newLcmAncillaryEntity.Cyberriskrequestid = ancillaryEntity.Cyberriskrequestid;
                newLcmAncillaryEntity.Isexposededge = ancillaryEntity.Isexposededge;
                newLcmAncillaryEntity.Riskcomment = ancillaryEntity.Riskcomment;
                newLcmAncillaryEntity.Qid = ancillaryEntity.Qid;

                //newLcmAncillaryEntity.Lastpentestdate = ancillaryEntity.Lastpentestdate != null?
                return newLcmAncillaryEntity;
            }
            catch (Exception ex)
            {
                return newLcmAncillaryEntity;
            }

        }
       
        public async Task CreateAndUpdateAncillaryViaLcm(Lcmancillarydata newLcmAncillaryEntity = null, Lcmancillarydata existLcmAncillaryEntity = null, bool isNoPa = false, bool isModernizeSolutionFlow = false,
            bool isArchive = false, long lcmId = 0, string assetOutofScope = "")
        {
            try
            {
                if (newLcmAncillaryEntity == null && !isArchive)
                { // this is for adding new ancillary for no PA
                    if (isNoPa && existLcmAncillaryEntity != null)
                    {
                        newLcmAncillaryEntity = new Lcmancillarydata();

                        newLcmAncillaryEntity.Commentonprojectstatus = existLcmAncillaryEntity.Commentonprojectstatus;
                        newLcmAncillaryEntity.Reasonfornoplan = existLcmAncillaryEntity.Reasonfornoplan;
                        newLcmAncillaryEntity.Lcmengineeringid = lcmId;
                    }
                    else if (isArchive && existLcmAncillaryEntity == null && newLcmAncillaryEntity == null)
                    {
                        //need to create a new Ancillary if the archive lcm doesn't have ancillary
                        newLcmAncillaryEntity.Lcmengineeringid = lcmId;
                        newLcmAncillaryEntity.Lcmancillarydataid = 0;
                        newLcmAncillaryEntity.Engupdatetracker = ConstantValueFilter.completed;
                        newLcmAncillaryEntity.Assetoutofscope = ConstantValueFilter.historicalback_up;
                        newLcmAncillaryEntity.Opsupdatetracker = ConstantValueFilter.completed;
                    }
                    // when we creare a new LCM from UI we need to creat new ancillary
                    else if (newLcmAncillaryEntity == null && existLcmAncillaryEntity == null)
                    {
                        newLcmAncillaryEntity = new Lcmancillarydata();

                        newLcmAncillaryEntity.Lcmengineeringid = lcmId;
                        newLcmAncillaryEntity.Lcmancillarydataid = 0;
                        newLcmAncillaryEntity.Engupdatetracker = ConstantValueFilter._engUpdateTracker;
                        newLcmAncillaryEntity.Opsupdatetracker = ConstantValueFilter._engUpdateTracker;
                        newLcmAncillaryEntity.Assetoutofscope = assetOutofScope;
                        newLcmAncillaryEntity.Lastpentestdate = newLcmAncillaryEntity.Lastpentestdate != null ? newLcmAncillaryEntity.Lastpentestdate : null;
                    }
                    // mapping an old ancillary value to new ancillary value when we creat new lcm 
                    else if (newLcmAncillaryEntity == null && existLcmAncillaryEntity != null && !isNoPa)
                    {
                        newLcmAncillaryEntity = new Lcmancillarydata();

                        newLcmAncillaryEntity.Lcmengineeringid = lcmId;
                        newLcmAncillaryEntity.Lcmancillarydataid = 0;
                        newLcmAncillaryEntity.Engupdatetracker = ConstantValueFilter._engUpdateTracker;
                        newLcmAncillaryEntity.Opsupdatetracker = ConstantValueFilter._engUpdateTracker;
                        newLcmAncillaryEntity.Assetoutofscope = assetOutofScope;
                        newLcmAncillaryEntity = MappingAncillaryData(newLcmAncillaryEntity, existLcmAncillaryEntity);
                    }

                    //var lcmAncillaryDataEntity = LcmAncillaryDataMapper.Get(newLcmAncillaryEntity);
                    //var lcmAncillaryDataSetEntity = LcmAncillaryDataMapper.Set(lcmAncillaryDataEntity);
                    _repositoryWrapper.LcmAncillaryData.Create(newLcmAncillaryEntity);
                }
                // we need to update ancillary when we do archive lcm
                else if (isArchive && existLcmAncillaryEntity != null)
                {
                    existLcmAncillaryEntity.Engupdatetracker = ConstantValueFilter.completed;
                    existLcmAncillaryEntity.Assetoutofscope = ConstantValueFilter.historicalback_up;
                    existLcmAncillaryEntity.Opsupdatetracker = ConstantValueFilter.completed;
                    _repositoryWrapper.LcmAncillaryData.Update(existLcmAncillaryEntity);

                }
                //mappping the old ancillary to new ancillary if the lcm already exist for FSI and Rollout case 
                else if (newLcmAncillaryEntity != null && existLcmAncillaryEntity != null && !isNoPa)
                {
                    // lcm already exist 
                    newLcmAncillaryEntity.Engupdatetracker = ConstantValueFilter._engUpdateTracker;
                    newLcmAncillaryEntity.Opsupdatetracker = ConstantValueFilter._engUpdateTracker;
                    newLcmAncillaryEntity.Assetoutofscope = assetOutofScope;
                    newLcmAncillaryEntity = MappingAncillaryData(newLcmAncillaryEntity, existLcmAncillaryEntity);

                    _repositoryWrapper.LcmAncillaryData.Update(newLcmAncillaryEntity);

                }
                // need to update the ancillary for no PA
                else if (newLcmAncillaryEntity != null && existLcmAncillaryEntity != null && isNoPa)
                {
                    existLcmAncillaryEntity.Commentonprojectstatus = newLcmAncillaryEntity.Commentonprojectstatus;
                    existLcmAncillaryEntity.Reasonfornoplan = newLcmAncillaryEntity.Reasonfornoplan;
                    _repositoryWrapper.LcmAncillaryData.Update(existLcmAncillaryEntity);
                }


                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
            
    }
}
