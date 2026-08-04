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
    public class TeamMembersManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private GridCustomColumnManager _manager;
        private ILoggerManager _logger;

        public TeamMembersManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor, ILoggerManager loggerManager, ICurrentUserService currentUserService,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _manager = manager;
            _logger = loggerManager;
        }

        #region //UI Member
        public async Task<QueryResultDto<TeamMemberGridDto>> FindWithCondition(TeamMemberQueryDto dto)
        {
            try
            {
                var predicateResult = ApplyFilter(dto);

                var rtn = new QueryResultDto<TeamMemberGridDto>(new GenerateRenderForGrid<TeamMemberGridDto>(_manager))
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

        private static ExpressionStarter<Teammembers> ApplyFilter(TeamMemberQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Teammembers>(true);
            var predicateInner = PredicateBuilder.New<Teammembers>(true);

            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Teammembers>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.Modificationuser.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Teammembers>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TeamMemberId != null && buildFilterDto.TeamMemberId.Any())
            {
                predicateInner = PredicateBuilder.New<Teammembers>();
                foreach (var item in buildFilterDto.TeamMemberId)
                    predicateInner.Or(x => x.Teammemberid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.UserId != null && buildFilterDto.UserId.Any())
            {
                predicateInner = PredicateBuilder.New<Teammembers>();
                foreach (var item in buildFilterDto.UserId)
                    predicateInner.Or(x => x.Userid == item);
                predicateResult.And(predicateInner);
            }          
            if (buildFilterDto.Creationuser != null && buildFilterDto.Creationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Teammembers>();
                foreach (var item in buildFilterDto.Creationuser)
                    predicateInner.Or(x => x.Creationuser == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Modificationuser != null && buildFilterDto.Modificationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Teammembers>();
                foreach (var item in buildFilterDto.Modificationuser)
                    predicateInner.Or(x => x.Modificationuser == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationdate != null)
            {
                predicateInner = PredicateBuilder.New<Teammembers>();
                if (buildFilterDto.Creationdate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= buildFilterDto.Creationdate.StartDate);
                if (buildFilterDto.Creationdate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= buildFilterDto.Creationdate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Modificationdate != null)
            {
                predicateInner = PredicateBuilder.New<Teammembers>();
                if (buildFilterDto.Modificationdate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= buildFilterDto.Modificationdate.StartDate);
                if (buildFilterDto.Modificationdate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= buildFilterDto.Modificationdate.EndDate);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        private Task<IQueryable<TeamMemberGridDto>> GetQuery(ExpressionStarter<Teammembers> predicateResult)
        {
            var query = _repositoryWrapper.TeamMemberRepository.FindByConditionWithDelete(predicateResult).AsNoTracking()
               .Select(x => new TeamMemberGridDto
               {
                   TeamMemberId = x.Teammemberid,
                   User = x.User.Username,
                   LastModified = x.Modificationdate,
                   LastModifiedBy = x.ModificationuserNavigation.Username,
                   LastModifiedId = x.Modificationuser,
                   UserId = x.Userid.Value,
               }).AsQueryable();


            return Task.FromResult(query);
        }

        private Dictionary<string, Expression<Func<TeamMemberGridDto, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<TeamMemberGridDto, object>>[]>
            {
                ["teamDescription"] = new Expression<Func<TeamMemberGridDto, object>>[] { p => p.TeamMemberId },
                ["modificationDate"] = new Expression<Func<TeamMemberGridDto, object>>[] { p => p.LastModified },

            };
        }

        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, TeamMemberQueryDto buildFilterDto)
        {
            try
            {
                var predicateResult = ApplyFilter(buildFilterDto);

                var query = await GetQuery(predicateResult);


                var rtn = propertyName switch
                {


                    "teamMemberId" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.TeamMemberId.ToString(), Value = p.TeamMemberId.ToString() }).Distinct().ToList()
                   : query
                       .Where(x => x.TeamMemberId.ToString().Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.TeamMemberId.ToString(), Value = p.TeamMemberId.ToString() }).Distinct()
                       .ToList(),

                    "user" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.User, Value = p.UserId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.UserId.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.User, Value = p.UserId.ToString() }).Distinct()
                        .ToList(),
                   
                    "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                       ? query
                       .Select(p => new FilterValueDto
                       { Text = p.LastModifiedBy, Value = p.LastModifiedId.ToString() }).Distinct().ToList()
                       : query
                           .Where(x => x.LastModifiedId.ToString().Contains(propertyFilter))
                           .Select(p =>
                               new FilterValueDto { Text = p.LastModifiedBy, Value = p.LastModifiedId.ToString() }).Distinct()
                           .ToList(),

                    _ => new List<FilterValueDto>()
                };

                return rtn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region //CRUD

        public async Task<TeamMemberCreateAndUpdateDto> GetCreatePage()
        {
            var dto = new TeamMemberCreateAndUpdateDto();
            dto.UserResource = await _repositoryWrapper.UserRepository.FindAll().ToDictionaryAsync(x => x.Id, y => y.Username);
            return dto;
        }

        public async Task<TeamMemberCreateAndUpdateDto> GetUpdatePage(int id)
        {
            try
            {
                var model = await _repositoryWrapper.TeamMemberRepository.FindByCondition(x => x.Teammemberid == id).Include(x => x.User).Include(x => x.ModificationuserNavigation).SingleOrDefaultAsync();

                return new TeamMemberCreateAndUpdateDto
                {
                    TeamMemberId = model.Teammemberid,
                    User = model.User.Username,
                    LastModified = model.Modificationdate,
                    LastModifiedBy = model.ModificationuserNavigation.Username,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public async Task<ResultDto> CreateTeamMembers(int TeamId, List<int> UserIds)
        {
            
            try
            {
                var membersEntities = await _repositoryWrapper.TeamMemberRepository.FindByCondition(f => f.Teamid == TeamId).ToListAsync();

                var insertMember = UserIds.Where(f => !membersEntities.Select(x => x.Userid).ToList().Contains(f));

                if(membersEntities != null && membersEntities.Count > 0)
                {
                    var deleteMemberEntitys = membersEntities.Where(f => !UserIds.Contains(f.Userid.Value)).ToList();
                    if(deleteMemberEntitys != null && deleteMemberEntitys.Count > 0)
                    {
                        _repositoryWrapper.TeamMemberRepository.BulkDeepDelete(deleteMemberEntitys);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                if (insertMember != null && insertMember.Count() > 0)
                {
                    var insertMemberEntities = insertMember.Select(r => new Teammembers
                    {
                        Teamid = TeamId,
                        Userid = r,
                    }).ToList();

                    _repositoryWrapper.TeamMemberRepository.BulkCreate(insertMemberEntities);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }

                return new ResultDto { Info = ResultMessages.EntryAddSuccess};

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
                var model = await _repositoryWrapper.TeamMemberRepository.FindByCondition(x => x.Teammemberid == id).SingleOrDefaultAsync();
                _repositoryWrapper.TeamMemberRepository.DeleteDeep(model);
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = model.Teammemberid,
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
