using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.TeamManagement;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.TeamManagement;
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
    public class TeamsManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private ICurrentUserService _currentUserService;
        private GridCustomColumnManager _manager;
        private TeamMembersManager _membersManager;
        private ILoggerManager _logger;
        public TeamsManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager manager,TeamMembersManager membersManager,
             IHttpContextAccessor contextAccessor,ICurrentUserService currentUserService,ILoggerManager loggerManager,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _manager = manager;
            _currentUserService = currentUserService;
            _logger = loggerManager;
            _membersManager = membersManager;
        }

        #region //UI Member
        public async Task<QueryResultDto<TeamGridDto>> FindWithCondition(TeamQueryDto dto)
        {
            try
            {
                var predicateResult = ApplyFilter(dto);

                var rtn = new QueryResultDto<TeamGridDto>(new GenerateRenderForGrid<TeamGridDto>(_manager))
                {
                };
                var query = await GetQuery(predicateResult);
                query = query.ApplyOrdering(dto, GetColumnsMap());
                rtn.TotalItems = query.Count();
                query = query.ApplyPaging(dto);

                rtn.Items = query.ToList();
                return rtn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        private static ExpressionStarter<Teams> ApplyFilter(TeamQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Teams>(true);
            var predicateInner = PredicateBuilder.New<Teams>(true);

            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Teams>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.Modificationuser.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Teams>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TeamId != null && buildFilterDto.TeamId.Any())
            {
                predicateInner = PredicateBuilder.New<Teams>();
                foreach (var item in buildFilterDto.TeamId)
                    predicateInner.Or(x => x.Teamid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.TeamName != null && buildFilterDto.TeamName.Any())
            {
                predicateInner = PredicateBuilder.New<Teams>();
                foreach (var item in buildFilterDto.TeamName)
                    predicateInner.Or(x => x.Teamname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TeamDescription != null && buildFilterDto.TeamDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Teams>();
                foreach (var item in buildFilterDto.TeamDescription)
                    predicateInner.Or(x => x.Teamdescription == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Active != null && buildFilterDto.Active.Any())
            {
                predicateInner = PredicateBuilder.New<Teams>();
                foreach (var item in buildFilterDto.Active)
                    predicateInner.Or(x => x.Isactive == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.UserId != null && buildFilterDto.UserId.Any())
            {
                predicateInner = PredicateBuilder.New<Teams>();
                foreach (var item in buildFilterDto.UserId)
                    predicateInner.Or(x => x.Teammembers.Any(f => f.Userid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationuser!= null && buildFilterDto.Creationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Teams>();
                foreach (var item in buildFilterDto.Creationuser)
                    predicateInner.Or(x => x.Creationuser == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Modificationuser != null && buildFilterDto.Modificationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Teams>();
                foreach (var item in buildFilterDto.Modificationuser)
                    predicateInner.Or(x => x.Modificationuser == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationdate != null)
            {
                predicateInner = PredicateBuilder.New<Teams>();
                if (buildFilterDto.Creationdate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= buildFilterDto.Creationdate.StartDate);
                if (buildFilterDto.Creationdate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= buildFilterDto.Creationdate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Modificationdate != null)
            {
                predicateInner = PredicateBuilder.New<Teams>();
                if (buildFilterDto.Modificationdate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= buildFilterDto.Modificationdate.StartDate);
                if (buildFilterDto.Modificationdate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= buildFilterDto.Modificationdate.EndDate);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        private Task<IQueryable<TeamGridDto>> GetQuery(ExpressionStarter<Teams> predicateResult)
        {
            var query = _repositoryWrapper.TeamRepository.FindByConditionWithDelete(predicateResult).AsNoTracking()
               .Select(x => new TeamGridDto
               {
                   TeamId = x.Teamid,
                   TeamDescription = x.Teamdescription,
                   TeamName = x.Teamname,
                   Active = x.Isactive.Value,
                   UserName = string.Join(",", x.Teammembers.Select(r => r.User.Username).ToList()),
                   LastModified = x.Modificationdate,
                   LastModifiedBy = x.ModificationuserNavigation.Username,
                   LastModifiedId = x.Modificationuser
               }).AsQueryable();


            return Task.FromResult(query);
        }

        private Dictionary<string, Expression<Func<TeamGridDto, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<TeamGridDto, object>>[]>
            {
                ["teamId"] = new Expression<Func<TeamGridDto, object>>[] { p => p.TeamId },
                ["teamDescription"] = new Expression<Func<TeamGridDto, object>>[] { p => p.TeamDescription },
                ["teamName"] = new Expression<Func<TeamGridDto, object>>[] { p => p.TeamName },                
                ["modificationDate"] = new Expression<Func<TeamGridDto, object>>[] { p => p.LastModified },
                ["modificationUser"] = new Expression<Func<TeamGridDto, object>>[] { p => p.LastModifiedBy },

            };
        }

        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, TeamQueryDto buildFilterDto)
        {
            try
            {
                var predicateResult = ApplyFilter(buildFilterDto);

                var query = await GetQuery(predicateResult);


                var rtn = propertyName switch
                {


                    "teamId" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.TeamId.ToString(), Value = p.TeamId.ToString() }).Distinct().ToList()
                   : query
                       .Where(x => x.TeamId.ToString().Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.TeamId.ToString(), Value = p.TeamId.ToString() }).Distinct()
                       .ToList(),

                    "teamName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.TeamName, Value = p.TeamName }).Distinct().ToList()
                    : query
                        .Where(x =>x.TeamName.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.TeamName, Value = p.TeamName }).Distinct()
                        .ToList(),

                    "teamDescription" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.TeamDescription, Value = p.TeamDescription }).Distinct().ToList()
                       : query
                           .Where(x => x.TeamDescription.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.TeamDescription, Value = p.TeamDescription }).Distinct()
                           .ToList(),
                    "active" => string.IsNullOrEmpty(propertyFilter)
                       ? query
                       .Select(p => new FilterValueDto
                       { Text = p.Active.ToString(), Value = p.Active.ToString() }).Distinct().ToList()
                       : query
                           .Where(x => x.Active.ToString().Contains(propertyFilter))                           
                           .Select(p =>
                               new FilterValueDto {Text = p.Active.ToString(), Value = p.Active.ToString() }).Distinct()
                           .ToList(),
                    "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                       ? query
                       .Select(p => new FilterValueDto
                       { Text = p.LastModifiedBy, Value = p.LastModifiedId.ToString()}).Distinct().ToList()
                       : query
                           .Where(x => x.LastModifiedId.ToString().Contains(propertyFilter))
                           .Select(p =>
                               new FilterValueDto { Text = p.LastModifiedBy, Value = p.LastModifiedId.ToString()}).Distinct()
                           .ToList(),

                    _ => new List<FilterValueDto>()
                };

                return rtn;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region //Export excel
        public async Task<QueryResultDto<TeamGridDto>> GetTeamsDetails(TeamQueryDto aspNetUserRoleDto)
        {
            var predicateResult = ApplyFilter(aspNetUserRoleDto);
            var rtn = new QueryResultDto<TeamGridDto>(new GenerateRenderForGrid<TeamGridDto>(_manager))
            {

            };
            var query = await GetQuery(predicateResult);
            query= query.OrderByDescending(x => x.LastModified);
            rtn.TotalItems = query.Count();            
            rtn.Items = query.ToArray();
            return rtn;
        }
        #endregion

        #region //CRUD

        public TeamGridDto GetCreatePage()
        {
            var dto = new TeamGridDto();
            dto.UserResources = _repositoryWrapper.UserRepository.FindAll().ToDictionary(x => x.Id, y => y.Username);
            return dto;
        }

        public async Task<TeamGridDto> GetUpdatePage(int id)
        {
            try
            {
                var userResource = _repositoryWrapper.UserRepository.FindAll().ToDictionary(x => x.Id, y => y.Username);
                var model = await _repositoryWrapper.TeamRepository.FindByCondition(x => x.Teamid == id)
                    .Select(x => new TeamGridDto
                    {
                        TeamId = x.Teamid,
                        TeamName = x.Teamname,
                        TeamDescription = x.Teamdescription,
                        Active = x.Isactive.Value,
                        LastModified = x.Modificationdate,
                        LastModifiedBy = x.ModificationuserNavigation.Username,
                        UserName = string.Join(",", x.Teammembers.Select(x => x.Userid).ToList()),
                        UserIds = x.Teammembers.Select(x => x.Userid.Value).ToList(),
                        UserResources = userResource,

                    }).FirstOrDefaultAsync();

                //model.UserResources = userResource;
                return model;
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public async Task<ResultDto> GetRelatedRecords(int id)
        {
            try
            {
                List<ResultMessageDto> rm = new List<ResultMessageDto>();

                var entity = await _repositoryWrapper.TeamRepository.FindByCondition(x => x.Teamid == id).SingleAsync();
                var teamMemberEntity = await _repositoryWrapper.TeamMemberRepository
                    .FindByCondition(x => x.Teamid == entity.Teamid)
                    .Select(x => x.User.Username).ToArrayAsync();



                if (teamMemberEntity.Length > 0)
                {
                    rm.Add(new ResultMessageDto() { Table = "Team Members", Values = teamMemberEntity });
                }

                if (rm.Count > 0)
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.EntryDeleteNotOrphan,
                        Data = new RelatedRecordsResultDto()
                        {
                            EntityName = "Teams",
                            RecordName = entity.Teamname,
                            DataRelatedList = rm
                        }
                    };
                }
                else
                    return new ResultDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public async Task<ResultDto> Deactivate(TeamGridDto dto)
        {
            try
            {
                
                var entity = await _repositoryWrapper.TeamRepository.FindByConditionWithDelete(x => x.Teamid == dto.TeamId).FirstOrDefaultAsync();
                if (entity != null)
                {
                    
                    entity.Isactive = dto.Active;
                    entity.Deleted = false;
                    _repositoryWrapper.TeamRepository.Update(entity);

                    await _repositoryWrapper.SaveAsync();
                    
                }
                return new ResultDto
                {
                    Info = ResultMessages.DeactivityRelation,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
       
        public async Task<ResultDto> CreateOrUpdateTeams(TeamGridDto dto)
        {
            using (var transaction = await _repositoryWrapper.BeginTransactionAsync())
            {
                try
                {
                    if (dto.TeamId == 0)
                    {
                        var entity = await _repositoryWrapper.TeamRepository.FindByCondition(x => x.Teamname.Trim().ToLower().Replace(" ", "") == dto.TeamName.Trim().ToLower().Replace(" ", "")).FirstOrDefaultAsync();
                        if(entity != null)
                        {
                            return new ResultDto
                            {
                                Info = ResultMessages.EntryAddExists,
                                Warning = true,
                            };
                        }
                        var InsertEntity = new Teams
                        {
                            Teamname = dto.TeamName,
                            Teamdescription = dto.TeamDescription,
                            Deleted = false,
                        };
                        _repositoryWrapper.TeamRepository.Create(InsertEntity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();

                        await _membersManager.CreateTeamMembers(InsertEntity.Teamid, dto.UserIds);
                    }
                    else
                    {
                        var entity = await _repositoryWrapper.TeamRepository.FindByCondition(x => x.Teamid != dto.TeamId && x.Teamname.Trim().ToLower().Replace(" ", "") == dto.TeamName.Trim().ToLower().Replace(" ", "")).FirstOrDefaultAsync();

                        if (entity != null)
                        {
                            return new ResultDto
                            {
                                Info = ResultMessages.EntryAddExists,
                                Warning = true,
                            };
                        }
                        var updateEntity = new Teams
                        {
                            Teamid = dto.TeamId,
                            Teamname = dto.TeamName,
                            Teamdescription = dto.TeamDescription,
                            Isactive = dto.Active,
                            Deleted = false,
                        };
                        _repositoryWrapper.TeamRepository.Update(updateEntity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                        await _membersManager.CreateTeamMembers(updateEntity.Teamid, dto.UserIds);

                    }

                    await transaction.CommitAsync();
                    return new ResultDto { Info = ResultMessages.EntryAddSuccess, Data = dto.TeamName};

                }
                catch(Exception ex) 
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex.StackTrace);
                    throw;
                }
            }
        }

        public async Task<ResultDto> Delete(int id)
        {
            try
            {
                var model = await _repositoryWrapper.TeamRepository.FindByCondition(x => x.Teamid == id).SingleOrDefaultAsync();
                _repositoryWrapper.TeamRepository.Delete(model);
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = model.Teamname,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public async Task<ResultDto> DeepDelete(int id)
        {
            try
            {
                var model = await _repositoryWrapper.TeamRepository.FindByCondition(x => x.Teamid == id).SingleOrDefaultAsync();
                _repositoryWrapper.TeamRepository.DeleteDeep(model);
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = model.Teamname,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        #endregion

    }
}
