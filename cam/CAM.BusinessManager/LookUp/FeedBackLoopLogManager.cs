using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.FeedBackLoopLog;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.LookUp
{
    public class FeedBackLoopLogManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;

        public FeedBackLoopLogManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnmanager,
              IHttpContextAccessor contextAccessor,
             IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnmanager;
           
        }


        #region //UiMemberFunctions
        public QueryResultDto<FeedBackLoopAuditGridDto> FindWithCondition(FeedBackLoopAuditQueryDto dto)
        {
            var predicateResult = ApplyFilter(dto);         
            var rtn = new QueryResultDto<FeedBackLoopAuditGridDto>(new GenerateRenderForGrid<FeedBackLoopAuditGridDto>(_columnManager))
            {

            };
            var query = GetQuery(predicateResult).ApplyOrdering(dto, GetColumnsMap());
            query = query.OrderByDescending(x => x.ModificationDate);
            rtn.TotalItems = query.Count();
            query = query.ApplyPaging(dto);
            var data = query.ToList();

            var result = data.Select(x =>
            {
                var grid = new FeedBackLoopAuditGridDto();
                grid.OpCo = x.OpCo;
                grid.Oem = x.Oem;
                grid.NodeType = x.NodeType;
                grid.FileCount = x.FileCount;
                grid.ProcessStartTime = x.ProcessStartTime != null ? x.ProcessStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty ;
                grid.ProcessEndTime = x.ProcessEndTime != null? x.ProcessEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss"):string.Empty;
                grid.CreationDate = x.CreationDate;
                grid.ModificationDate = x.ModificationDate;
                grid.ProcessingTime = x.ProcessEndTime - x.ProcessStartTime;
                grid.ProcessedFile = new List<string>();
                if(x.XmlParseRun != null && x.XmlParseRun.Count() >0)
                {
                    foreach(var row in x.XmlParseRun)
                    {
                        grid.ProcessedFile.Add(row.Filename);
                    }
                }
                return grid;
            }).ToList();
            
            

            rtn.Items = result.ToArray();
            return rtn;
        }

        private  ExpressionStarter<Feedbackloopaudits> ApplyFilter(FeedBackLoopAuditQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Feedbackloopaudits>();
            var predicateInner = PredicateBuilder.New<Feedbackloopaudits>();

            if (buildFilterDto.FeedBackLoopAuditId != null && buildFilterDto.FeedBackLoopAuditId.Any())
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                foreach (var item in buildFilterDto.FeedBackLoopAuditId)
                    predicateInner.Or(x => x.Feedbackloopauditid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                foreach (var item in buildFilterDto.OpCo)
                    predicateInner.Or(x => x.Opco == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                foreach (var item in buildFilterDto.OpCo)
                    predicateInner.Or(x => x.Opco == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Oem != null && buildFilterDto.Oem.Any())
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                foreach (var item in buildFilterDto.Oem)
                    predicateInner.Or(x => x.Oem == item);
                predicateResult.And(predicateInner);
            }          
            if (buildFilterDto.NodeType != null && buildFilterDto.NodeType.Any())
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                foreach (var item in buildFilterDto.NodeType)
                    predicateInner.Or(x => x.Nodetype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.FileCount != null && buildFilterDto.FileCount.Any())
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                foreach (var item in buildFilterDto.FileCount)
                    predicateInner.Or(x => x.Filecount == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProcessStartTime!= null && buildFilterDto.ProcessStartTime.Any())
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                foreach (var item in buildFilterDto.ProcessStartTime)
                    predicateInner.Or(x => x.Processstarttime.Value == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProcessEndTime != null && buildFilterDto.ProcessEndTime.Any())
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                foreach (var item in buildFilterDto.ProcessEndTime)
                    predicateInner.Or(x => x.Processendtime.Value == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CreationUser != null && buildFilterDto.CreationUser.Any())
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                foreach (var item in buildFilterDto.CreationUser)
                    predicateInner.Or(x => x.CreationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProcessingTime != null && buildFilterDto.ProcessingTime.Any())
            {
                var entitieswithdata = _repositoryWrapper.FeedBackLoopAuditRepository.FindAll().ToList();
                var filteredEntities = entitieswithdata.Where(entity =>
                {
                    foreach (var item in buildFilterDto.ProcessingTime)
                    {
                        if ((entity.Processendtime != null && entity.Processstarttime != null)&&(entity.Processendtime.Value - entity.Processstarttime.Value).ToString()== item)
                        {
                            return true;
                        }
                    }
                    return false;
                });
                var filteredEntityIds = filteredEntities.Select(entity => entity.Feedbackloopauditid).ToList();
                predicateResult.And(x => filteredEntityIds.Contains(x.Feedbackloopauditid));              
            }
            if(buildFilterDto.ProcessedFile != null && buildFilterDto.ProcessedFile.Any())
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                foreach (var item in buildFilterDto.ProcessedFile)
                    predicateInner.Or(x => x.Xmlparserun.Any(x=>x.Filename == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ModificationUser != null && buildFilterDto.ModificationUser.Any())
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                foreach (var item in buildFilterDto.ModificationUser)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CreationDate != null)
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                if (buildFilterDto.CreationDate.StartDate != null)
                    predicateInner.And(x => x.Creationdate.Date >= buildFilterDto.CreationDate.StartDate);
                if (buildFilterDto.CreationDate.EndDate != null)
                    predicateInner.And(x => x.Creationdate.Date <= buildFilterDto.CreationDate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ModificationDate != null)
            {
                predicateInner = PredicateBuilder.New<Feedbackloopaudits>();
                if (buildFilterDto.ModificationDate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.ModificationDate.StartDate);
                if (buildFilterDto.ModificationDate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.ModificationDate.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }

        private IQueryable<FeedBackLoopAudits> GetQuery(ExpressionStarter<Feedbackloopaudits> predicateResult)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.FeedBackLoopAuditRepository.FindByCondition(predicateResult)               
                       .Include(x => x.CreationuserNavigation)
                       .Include(x => x.ModificationuserNavigation).Include(x=>x.Xmlparserun)             
               : _repositoryWrapper.FeedBackLoopAuditRepository.FindAll()
                      .Include(x => x.CreationuserNavigation)
                      .Include(x => x.ModificationuserNavigation).Include(x => x.Xmlparserun);
            return query.AsEnumerable().Select(x => FeedBackLoopAuditMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<FeedBackLoopAudits, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<FeedBackLoopAudits, object>>[]>
            {
                ["feedBackLoopAuditId"] = new Expression<Func<FeedBackLoopAudits, object>>[] { p => p.FeedBackLoopAuditId },
                ["opCo"] = new Expression<Func<FeedBackLoopAudits, object>>[] { p => p.OpCo },
                ["oem"] = new Expression<Func<FeedBackLoopAudits, object>>[] { p => p.Oem },
                ["nodeType"] = new Expression<Func<FeedBackLoopAudits, object>>[] { p => p.NodeType },
                ["fileCount"] = new Expression<Func<FeedBackLoopAudits, object>>[] { p => p.FileCount },              
                ["processStartTime"] = new Expression<Func<FeedBackLoopAudits, object>>[] {p =>p.ProcessStartTime},
                ["processEndTime"] = new Expression<Func<FeedBackLoopAudits, object>>[] { p => p.ProcessEndTime },
                ["modificationDate"] = new Expression<Func<FeedBackLoopAudits, object>>[] { p => p.ModificationDate },
                ["modificationUser"] = new Expression<Func<FeedBackLoopAudits, object>>[] { p => p.ModificationUser },
                ["creationUser"] = new Expression<Func<FeedBackLoopAudits, object>>[] { p => p.CreationUser },
                ["creationDate"] = new Expression<Func<FeedBackLoopAudits, object>>[] { p => p.CreationDate },
                ["deleted"] = new Expression<Func<FeedBackLoopAudits, object>>[] { p => p.Deleted },
                ["deletionDate"] = new Expression<Func<FeedBackLoopAudits, object>>[] { p => p.DeletionDate },


            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, FeedBackLoopAuditQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult);

            var rtn = propertyName switch
            {
                "feedBackLoopAuditId" => string.IsNullOrEmpty(propertyFilter)
          ? query.Select(p => new FilterValueDto
          { Text = p.FeedBackLoopAuditId.ToString(), Value = p.FeedBackLoopAuditId.ToString() }).Distinct().ToList()
          : query
              .Where(x => x.FeedBackLoopAuditId.ToString().Contains(propertyFilter)).Select(p =>
                  new FilterValueDto { Text = p.FeedBackLoopAuditId.ToString(), Value = p.FeedBackLoopAuditId.ToString() }).Distinct()
              .ToList(),

                "opCo" => string.IsNullOrEmpty(propertyFilter)
           ? query.Select(p => new FilterValueDto
           { Text = p.OpCo, Value = p.OpCo }).Distinct().ToList()
           : query
               .Where(x => x.OpCo.Contains(propertyFilter)).Select(p =>
                   new FilterValueDto { Text = p.OpCo, Value = p.OpCo }).Distinct()
               .ToList(),

                "oem" => string.IsNullOrEmpty(propertyFilter)
           ? query.Select(p => new FilterValueDto
           { Text = p.Oem, Value = p.Oem }).Distinct().ToList()
           : query
               .Where(x => x.Oem.Contains(propertyFilter)).Select(p =>
                   new FilterValueDto { Text = p.Oem, Value = p.Oem }).Distinct()
               .ToList(),

                "fileCount" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.FileCount.ToString(), Value = p.FileCount.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.FileCount.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.FileCount.ToString(), Value = p.FileCount.ToString() }).Distinct()
                    .ToList(),

                "columnName" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.NodeType, Value = p.NodeType }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.NodeType.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.NodeType, Value = p.NodeType }).Distinct().ToList(),

                "processStartTime" => string.IsNullOrEmpty(propertyFilter)
                      ? query.Select(p => new FilterValueDto { Text = p.ProcessStartTime.ToString(), Value = p.ProcessStartTime.ToString() }).Distinct().ToList()
                      : query
                          .Where(x =>
                              x.ProcessStartTime.ToString().Contains(
                                  propertyFilter)).Select(p => new FilterValueDto { Text = p.ProcessStartTime.ToString(), Value = p.ProcessStartTime.ToString() }).Distinct().ToList(),

                "processEndTime" => string.IsNullOrEmpty(propertyFilter)
                                   ? query.Select(p => new FilterValueDto { Text = p.ProcessEndTime.ToString(), Value = p.ProcessEndTime.ToString() }).Distinct().ToList()
                                   : query
                                       .Where(x =>
                                           x.ProcessEndTime.ToString().Contains(
                                               propertyFilter)).Select(p => new FilterValueDto { Text = p.ProcessEndTime.ToString(), Value = p.ProcessEndTime.ToString() }).Distinct().ToList(),


                "processingTime" => 
                query.Select(p => new FilterValueDto
               { Text = (p.ProcessEndTime - p.ProcessStartTime).ToString()/*String.Join(";",p.ProcessEndTime,p.ProcessStartTime)*/, 
                   Value = (p.ProcessEndTime - p.ProcessStartTime).ToString()
                }).Distinct().ToList(),



                "creationDate" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.CreationDate.ToString(), Value = p.CreationDate.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.CreationDate.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.CreationDate.ToString(), Value = p.CreationDate.ToString() }).Distinct().ToList(),


                "nodeType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.NodeType.ToString(), Value = p.NodeType.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.NodeType.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.NodeType.ToString(), Value = p.NodeType.ToString() }).Distinct().ToList(),


                "modificationDate" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.ModificationDate.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList(),


                "processedFile" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.XmlParseRun.Select(x=>x.Filename).FirstOrDefault(), Value = p.XmlParseRun.Select(x => x.Filename).FirstOrDefault() }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.XmlParseRun.Select(x => x.Filename).FirstOrDefault().Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.XmlParseRun.Select(x => x.Filename).FirstOrDefault(), Value = p.XmlParseRun.Select(x => x.Filename).FirstOrDefault() }).Distinct().ToList(),


                _ => new List<FilterValueDto>()
            };

            return rtn;
        }
        
        #endregion        


    }
}
