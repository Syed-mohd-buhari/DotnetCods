using AutoMapper;
using CAM.BusinessManager.AbstractionLayer;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.AbstractionLayer;
using CAM.DataTransferObjects.Entita.Aspnetuserrole;
using CAM.DataTransferObjects.Entita.AspNetUserRole;
using CAM.DataTransferObjects.Entita.Organisation;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using CAM.Identity;
using CAM.Infrastucture;
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

namespace CAM.BusinessManager.Entity
{
    public class AspnetuserroleManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private ICurrentUserService _currentUserService;
        private readonly OrganisationManager _organisationManager;
        private readonly ILoggerManager _loggerManager;
        private readonly AbstractionLayerManager _abstractionLayerManager;
        public AspnetuserroleManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper,
             IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService, OrganisationManager organisationManager,
             ILoggerManager loggerManager, AbstractionLayerManager abstractionLayerManager,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _organisationManager = organisationManager;
            _loggerManager = loggerManager;
            _abstractionLayerManager = abstractionLayerManager;
        }

        #region UIMemberFunction

        private static ExpressionStarter<Aspnetuserroles> ApplyFilter(AspnetuserroleQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Aspnetuserroles>();
            var predicateInner = PredicateBuilder.New<Aspnetuserroles>();

            if (buildFilterDto.AspNetUserRoleId != null && buildFilterDto.AspNetUserRoleId.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetuserroles>();
                foreach (var item in buildFilterDto.AspNetUserRoleId)
                    predicateInner.Or(x => x.Aspnetuserroleid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.UserId != null && buildFilterDto.UserId.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetuserroles>();
                foreach (var item in buildFilterDto.UserId)
                    predicateInner.Or(x => x.Userid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RoleId != null && buildFilterDto.RoleId.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetuserroles>();
                foreach (var item in buildFilterDto.RoleId)
                    predicateInner.Or(x => x.Roleid == item);
                predicateResult.And(predicateInner);
            }
            //syed
            //if (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Aspnetuserroles>();
            //    foreach (var item in buildFilterDto.OpcoId)
            //        predicateInner.Or(x => x.Opcoid == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto.VerticalResponsibleid != null && buildFilterDto.VerticalResponsibleid.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Aspnetuserroles>();
            //    foreach (var item in buildFilterDto.VerticalResponsibleid)
            //        predicateInner.Or(x => x.Verticalresponsibleid == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto.Opco != null && buildFilterDto.Opco.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Aspnetuserroles>();
            //    foreach (var item in buildFilterDto.Opco)
            //        predicateInner.Or(x => x.Opco.Opco == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto.VerticalResponsible != null && buildFilterDto.VerticalResponsible.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Aspnetuserroles>();
            //    foreach (var item in buildFilterDto.VerticalResponsible)
            //        predicateInner.Or(x => x.Verticalresponsible.Verticalresponsible == item);
            //    predicateResult.And(predicateInner);
            //}
            if (buildFilterDto.UserName != null && buildFilterDto.UserName.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetuserroles>();
                foreach (var item in buildFilterDto.UserName)
                    predicateInner.Or(x => x.User.Username == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Email != null && buildFilterDto.Email.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetuserroles>();
                foreach (var item in buildFilterDto.Email)
                    predicateInner.Or(x => x.User.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Active != null && buildFilterDto.Active.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetuserroles>();
                foreach (var item in buildFilterDto.Active)
                    predicateInner.Or(x => x.User.Active == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Role != null && buildFilterDto.Role.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetuserroles>();
                foreach (var item in buildFilterDto.Role)
                    predicateInner.Or(x => x.Role.Name == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Creationuser != null && buildFilterDto.Creationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetuserroles>();
                foreach (var item in buildFilterDto.Creationuser)
                    predicateInner.Or(x => x.Creationuser == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Modificationuser != null && buildFilterDto.Modificationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetuserroles>();
                foreach (var item in buildFilterDto.Modificationuser)
                    predicateInner.Or(x => x.Modificationuser == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationdate != null)
            {
                predicateInner = PredicateBuilder.New<Aspnetuserroles>();
                if (buildFilterDto.Creationdate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= buildFilterDto.Creationdate.StartDate);
                if (buildFilterDto.Creationdate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= buildFilterDto.Creationdate.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }
        private async Task<IQueryable<AspNetUserRoles>> GetRoleQuery(ExpressionStarter<Aspnetuserroles> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
              ? _repositoryWrapper.UserRoleRepository.FindByCondition(predicateResult, includeDeleted)
              .Include(x => x.Role).Include(x => x.User).ThenInclude(x => x.AspnetuseropcosUser).ThenInclude(x => x.Opco)
              .Include(x => x.User).ThenInclude(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical)
             : _repositoryWrapper.UserRoleRepository.FindAll().Include(x => x.Role).Include(x => x.User).ThenInclude(x => x.AspnetuseropcosUser).ThenInclude(x => x.Opco)
              .Include(x => x.User).ThenInclude(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical);

            var data = await query.ToListAsync();

            return data.ToList().Select(x => AspnetuserroleMapper.Get(x)).AsQueryable();



        }

        private Dictionary<string, Expression<Func<AspNetUserRoles, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AspNetUserRoles, object>>[]>
            {
                ["aspNetUserRoleId"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.AspNetUserRoleId },
                ["userId"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.Userid },
                ["roleId"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.Roleid },
                ["userName"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.User.UserName },
                ["email"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.User.Email },
                //["opCoId"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.Opcoid },
                //["verticalResponsibleId"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.Verticalresponsibleid },
                ["role"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.Role.Name },
               // ["opCo"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.Opco.OpCoDescription },
               // ["verticalResponsible"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.VerticalResponsible.VerticalResponsibleDescription },
                ["modificationDate"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.Modificationdate },
                ["modificationUser"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.Modificationuser },
                ["creationUser"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.Creationuser },
                ["creationDate"] = new Expression<Func<AspNetUserRoles, object>>[] { p => p.Creationdate },
            };
        }


        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, AspnetuserroleQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = await GetRoleQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "aspNetUserRoleId" => string.IsNullOrEmpty(propertyFilter)
              ? query.Select(p => new FilterValueDto
              { Text = p.AspNetUserRoleId.ToString(), Value = p.AspNetUserRoleId.ToString() }).Distinct().ToList()
              : query
                  .Where(x => x.Userid.ToString().Contains(propertyFilter)).Select(p =>
                      new FilterValueDto { Text = p.AspNetUserRoleId.ToString(), Value = p.AspNetUserRoleId.ToString() }).Distinct()
                  .ToList(),

                "userId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.Userid.ToString(), Value = p.Userid.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.Userid.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.Userid.ToString(), Value = p.Userid.ToString() }).Distinct()
                   .ToList(),

                "roleId" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.Roleid.ToString(), Value = p.Roleid.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.Roleid.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.Roleid.ToString(), Value = p.Roleid.ToString() }).Distinct()
                    .ToList(),

                "email" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.User.Email, Value = p.User.Email }).Distinct().ToList()
                : query
                    .Where(x => x.User.Email.Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.User.Email, Value = p.User.Email }).Distinct()
                    .ToList(),

                "userName" => string.IsNullOrEmpty(propertyFilter)
           ? query.Select(p => new FilterValueDto
           { Text = p.User.UserName, Value = p.User.UserName }).Distinct().ToList()
           : query
               .Where(x => x.User.UserName.Contains(propertyFilter)).Select(p =>
                   new FilterValueDto { Text = p.User.UserName, Value = p.User.UserName }).Distinct()
               .ToList(),

                "verticalResponsibleId" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.User.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.VerticalRes.VerticalResId.ToString(), 
                         Value = p.User.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.VerticalRes.VerticalResId.ToString() }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.User.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.VerticalRes.VerticalResId.ToString().Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.User.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.VerticalRes.VerticalResId.ToString(), 
                                     Value = p.User.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.VerticalRes.VerticalResId.ToString()
                                 }).Distinct().ToList(),


                "active" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.User.Active == false ? ConstantValueFilter.No.ToUpper() : ConstantValueFilter.Yes.ToUpper(), Value = p.User.Active.ToString() }).Distinct().ToList()
                    : query
                        .Where(p => (p.User.Active.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.User.Active == false ? ConstantValueFilter.No.ToUpper() : ConstantValueFilter.Yes.ToUpper(), Value = p.User.Active.ToString() }).Distinct()
                        .ToList(),

                "opCoId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.User.Opcos.FirstOrDefault().ApplicationOpco.OpCoId.ToString(), 
                        Value = p.User.Opcos.FirstOrDefault().ApplicationOpco.OpCoId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.User.Opcos.FirstOrDefault().ApplicationOpco.OpCoId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.User.Opcos.FirstOrDefault().ApplicationOpco.OpCoId.ToString(), Value = p.User.Opcos.FirstOrDefault().ApplicationOpco.OpCoId.ToString() }).Distinct().ToList(),

                "opCo" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.User.Opcos.FirstOrDefault().ApplicationOpco.OpCoName, Value = p.User.Opcos.FirstOrDefault().ApplicationOpco.OpCoName }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.User.Opcos.FirstOrDefault().ApplicationOpco.OpCoName.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.User.Opcos.FirstOrDefault().ApplicationOpco.OpCoName, Value = p.User.Opcos.FirstOrDefault().ApplicationOpco.OpCoName }).Distinct().ToList(),


                "verticalResponsible" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.User.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.VerticalRes.VerticalRes, 
                        Value = p.User.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.VerticalRes.VerticalRes
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.User.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.VerticalRes.VerticalRes.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.User.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.VerticalRes.VerticalRes, 
                                    Value = p.User.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.VerticalRes.VerticalRes
                                }).Distinct().ToList(),


                "role" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.Role.Name, Value = p.Role.Name }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.Role.Name.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.Role.Name, Value = p.Role.Name }).Distinct().ToList(),


                "creationUser" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.Creationuser.ToString(), Value = p.Creationuser.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.Creationuser.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.Creationuser.ToString(), Value = p.Creationuser.ToString() }).Distinct().ToList(),



                "creationDate" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.Creationdate.ToString(), Value = p.Creationdate.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.Creationdate.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.Creationdate.ToString(), Value = p.Creationdate.ToString() }).Distinct().ToList(),


                "modificationUser" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Modificationuser.ToString(), Value = p.Modificationuser.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Modificationuser.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Modificationuser.ToString(), Value = p.Modificationuser.ToString() }).Distinct().ToList(),


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

        #region //List of ROV
        /// <summary>
        /// This function is used for list out the opco, roles and verticalResponsible for assign the User roles.
        /// </summary>
        /// <returns></returns>
        public async Task<AspNetUserRoleCreateOrUpdateDto> GetCreatePage()
        {
            return new AspNetUserRoleCreateOrUpdateDto()
            {

                OpCoResource = await _repositoryWrapper.OpCo.FindAll().ToDictionaryAsync(x => x.Opcoid, x => x.Opco),
                // The Existing vertica resource we are fetching form the vertical master table so that time we are facing one issue if that vertical doesn't link to org every time if we try to save it won't save. so now change the resource to get from org.
                VerticalResource = await _repositoryWrapper.OrganisationRepository.FindAll().Include(x => x.Vertical).ToDictionaryAsync(x => x.Verticalid.Value, y => y.Vertical.Verticalresponsible),
                RoleResource = await _repositoryWrapper.RoleRepository.FindAll().ToDictionaryAsync(x => x.Id, x => x.Name),
                RoleDescriptionResource = await _repositoryWrapper.RoleRepository.FindAll().ToDictionaryAsync(k => k.Id, v => v.Description),
                AspnetuserrolepermissionsResources = await _repositoryWrapper.AspNetUserRolePermissionsRepository.FindAll().ToListAsync(),
                OrganisatioDtoGrids = await GetOrganisationDetails(),
                SubdomainResbonsibleResource = await _repositoryWrapper.SubDomainResponsible.FindAll().ToDictionaryAsync(x => x.Subdomainresponsibleid, y => y.Subdomainresponsible),
                VerticalResponcibleResource = await _repositoryWrapper.OrganisationRepository.FindAll().Include(x => x.Vertical).ToDictionaryAsync(x => x.Organisationid, y => y.Vertical.Verticalresponsible),

            };
        }
        #endregion

        #region //CRUD
        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.UserRoleRepository.FindByCondition(x => x.Aspnetuserroleid == id).FirstOrDefaultAsync();
            if (entity != null)
            {

                _repositoryWrapper.UserRoleRepository.Delete(entity);
                await _repositoryWrapper.SaveAsync();
            }

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Userid
            };
        }

        public async Task<ResultDto> UpdateOrCreate(AspNetUserRBAMCreateAndUpdateDto dto)
        {
            using (var transaction = await _repositoryWrapper.BeginTransactionAsync())
            {
                try
                {
                    var userId = dto.UserId;

                    #region // User Update
                    var userResult = await UpdateOnUser(dto);
                    #endregion

                    var domainAndRoleIds = GetDomainAndRolesIds(dto.AspNetUserRoleCreateAndUpdateDto);

                    #region // Role Update and Insert
                    var roleResult = await AddOrUpdateOnRole(domainAndRoleIds.RoleIds, userId);
                    #endregion

                    #region // Opco Update and Insert
                    var opcoResult = await AddOrUpdateOnOpco(domainAndRoleIds.OpcoIds,userId);
                    #endregion

                    #region // Restricted Opco Update and insert
                    var restrictedOpcoResult = await AddOrUpdateOnRestrictedOpco(domainAndRoleIds.RestrictedOpcoIds, userId);
                    #endregion

                    #region // Org Vertical Update and Insert
                    var OrgVerticalResult = await AddOrUpdateOnVertical(domainAndRoleIds.OrgVerticalIds, userId);
                    #endregion

                    #region // Org Vertical Update and Insert
                    var VerticalResponcibleResult = await AddOrUpdateOnVerticalResponcible(domainAndRoleIds.VerticalResponcibleIds, userId);
                    #endregion

                    #region // User Prefrence Create and update
                    var userPrefrenceDetails = dto.userPrefrenceDetails;
                    if (userPrefrenceDetails != null && userPrefrenceDetails?.Any() == true)
                    {
                        await AddOrUpdate(userPrefrenceDetails, dto.UserId);
                    }
                    #endregion
                                   
                    await transaction.CommitAsync();

                    return new ResultDto { Info = ResultMessages.EntryAddSuccess };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return new ResultDto
                    {
                        Info = ResultMessages.SystemError,
                        Warning = true
                    };
                }
            }
        }

        public UserDoaminAndRoleDetails GetDomainAndRolesIds(AspNetUserRoleUpdateDto dto)
        {
            var result = new UserDoaminAndRoleDetails();
            try
            {
                result.OpcoIds = dto.OpCoIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(short.Parse)
                    .ToList();

                result.RoleIds = dto.RoleIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();


                result.OrgVerticalIds = dto.OrganisationIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse)
                    .ToList();

                result.RestrictedOpcoIds = !dto.RestrictedOpcoIds.IsNullOrEmpty() ?  dto.RestrictedOpcoIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(short.Parse)
                    .ToList() : new List<short>();

                result.VerticalResponcibleIds = !dto.VerticalResponsibleIds.IsNullOrEmpty() ? dto.VerticalResponsibleIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse)
                    .ToList() : new List<long>();

                return result;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                return result;
            }
        }

        #region //User Update
        public async Task<ResultDto> UpdateOnUser(AspNetUserRBAMCreateAndUpdateDto dto)
        {
            try
            {
                var aspNetUserEntity = await _repositoryWrapper.UserRepository.FindByCondition(x => x.Id == dto.UserId).FirstOrDefaultAsync();

                if (aspNetUserEntity != null)
                {
                    aspNetUserEntity.Isdesigncontact = dto.Isdesigncontact;
                    aspNetUserEntity.Iseduspoc = dto.Iseduspoc;
                    aspNetUserEntity.Issubdomainspoc = dto.Issubdomainspoc;
                    aspNetUserEntity.Subdomainresponsibleid = dto.Subdomainresponsibleid;

                    _repositoryWrapper.UserRepository.Update(aspNetUserEntity);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }

                return new ResultDto
                {
                    Warning = false,
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region //Opco inser and Update
        public async Task<ResultDto> AddOrUpdateOnOpco(List<short> opCoIds, int userId)
        {
            try
            {
                var aspNetUserOpcoEntity = await _repositoryWrapper.AspNetUserOpcosRepository.FindByCondition(x => x.Userid == userId).ToListAsync();

                var insertOpco = opCoIds.Where(f => !aspNetUserOpcoEntity.Select(x => x.Opcoid).Contains(f));

                var updateRestrictOpcoToUserOpco = aspNetUserOpcoEntity.Where(f => opCoIds.Contains(f.Opcoid.Value) && f.Isinusedopcos == false)
                    .ToList();

                if(aspNetUserOpcoEntity != null && aspNetUserOpcoEntity.Count > 0)
                {
                    // need to deleted un selected Opcos
                    var deleteUnselectUserOpco = aspNetUserOpcoEntity.Where(f => !opCoIds.Contains(f.Opcoid.Value)).ToList();
                    if(deleteUnselectUserOpco != null && deleteUnselectUserOpco?.Any() == true)
                    {
                        _repositoryWrapper.AspNetUserOpcosRepository.BulkDeepDelete(deleteUnselectUserOpco);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                
                if(insertOpco != null && insertOpco.Count() > 0)
                {
                    var aspNetUserInsertOpcoEntity = insertOpco.Select(r => new Aspnetuseropcos
                    {
                        Opcoid = r,
                        Userid = userId,
                    }).ToList();


                    if (aspNetUserInsertOpcoEntity != null && aspNetUserInsertOpcoEntity?.Any() == true)
                    {
                        _repositoryWrapper.AspNetUserOpcosRepository.BulkCreate(aspNetUserInsertOpcoEntity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }

                // once user select opco check whether the opco is exist with the value of isrestricted true and update the record for normal opco.
                if (updateRestrictOpcoToUserOpco != null && updateRestrictOpcoToUserOpco.Count > 0)
                {
                    updateRestrictOpcoToUserOpco.ForEach(c => c.Isinusedopcos = true);
                    
                    _repositoryWrapper.AspNetUserOpcosRepository.BulkUpdate(updateRestrictOpcoToUserOpco);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                    
                }

                return new ResultDto
                {
                    Warning = false,
                };
            }
            catch(Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region // Restricted Opco Insert and Update
        public async Task<ResultDto> AddOrUpdateOnRestrictedOpco(List<short> restrictOpCoIds, int userId)
        {
            var updateSelectedRestrictedOpcoRecords = new List<Aspnetuseropcos>();
            var updateNotSelectedRestrictedOpcoRecords = new List<Aspnetuseropcos>();

            try
            {
                var aspNetUserOpcoEntity = await _repositoryWrapper.AspNetUserOpcosRepository.FindByCondition(x => x.Userid == userId).ToListAsync();

                var insertOpco = restrictOpCoIds.Where(f => !aspNetUserOpcoEntity.Select(x => x.Opcoid).Contains(f));

                updateSelectedRestrictedOpcoRecords.AddRange(aspNetUserOpcoEntity.Where(f => restrictOpCoIds.Contains(f.Opcoid.Value)).ToList());

                updateNotSelectedRestrictedOpcoRecords.AddRange(aspNetUserOpcoEntity.Where(f => !restrictOpCoIds.Contains(f.Opcoid.Value)).ToList());

                if(updateSelectedRestrictedOpcoRecords != null && updateSelectedRestrictedOpcoRecords.Count > 0)
                {
                    updateSelectedRestrictedOpcoRecords.ForEach(x => x.Isrestrictedopco = true);
                    _repositoryWrapper.AspNetUserOpcosRepository.BulkUpdate(updateSelectedRestrictedOpcoRecords);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                if (updateNotSelectedRestrictedOpcoRecords != null && updateNotSelectedRestrictedOpcoRecords.Count > 0)
                {
                    updateNotSelectedRestrictedOpcoRecords.ForEach(x => x.Isrestrictedopco = false);
                    _repositoryWrapper.AspNetUserOpcosRepository.BulkUpdate(updateNotSelectedRestrictedOpcoRecords);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }              

                // insert the record for restricted opco
                if (insertOpco != null && insertOpco.Count() > 0)
                {
                    var aspNetUserInsertOpcoEntity = insertOpco.Select(r => new Aspnetuseropcos
                    {
                        Opcoid = r,
                        Userid = userId,
                        Isinusedopcos = false,
                        Isrestrictedopco = true,
                    }).ToList();


                    if (aspNetUserInsertOpcoEntity != null && aspNetUserInsertOpcoEntity?.Any() == true)
                    {
                        _repositoryWrapper.AspNetUserOpcosRepository.BulkCreate(aspNetUserInsertOpcoEntity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                if (aspNetUserOpcoEntity != null && aspNetUserOpcoEntity.Count > 0)
                {
                    // need to deleted un selected Opcos
                    var deleteUnselectUserOpco = aspNetUserOpcoEntity.Where(f => !restrictOpCoIds.Contains(f.Opcoid.Value) 
                    && f.Isrestrictedopco == true && f.Isinusedopcos == false).ToList();
                    if (deleteUnselectUserOpco != null && deleteUnselectUserOpco?.Any() == true)
                    {
                        _repositoryWrapper.AspNetUserOpcosRepository.BulkDeepDelete(deleteUnselectUserOpco);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }


                return new ResultDto
                {
                    Warning = false,
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }

        #endregion

        #region //Vertical inser and Update
        public async Task<ResultDto> AddOrUpdateOnVertical(List<long> orgAndVeicalIds, int userId)
        {
            try
            {
                var aspNetUserOrgVerticalEntity = await _repositoryWrapper.AspNetUserVerticalsRepository.FindByCondition(x => x.Userid == userId).ToListAsync();

                var insertVertical = orgAndVeicalIds.Where(f => !aspNetUserOrgVerticalEntity.Select(r => r.Organisationid.Value).Contains(f));

                var updateVertical = aspNetUserOrgVerticalEntity.Where(f => orgAndVeicalIds.Contains(f.Organisationid.Value) && f.Isvertical == false)
                    .ToList();

                if (aspNetUserOrgVerticalEntity != null && aspNetUserOrgVerticalEntity.Count > 0) 
                {
                    // need to deleted un selected orgVertical
                    var deleteUnselectUserVertical = aspNetUserOrgVerticalEntity.Where(f => !orgAndVeicalIds.Contains(f.Organisationid.Value) && f.Isverticalresponcible == false).ToList();
                    if (deleteUnselectUserVertical != null && deleteUnselectUserVertical?.Any() == true)
                    {
                        _repositoryWrapper.AspNetUserVerticalsRepository.BulkDeepDelete(deleteUnselectUserVertical);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }                

                if (insertVertical != null && insertVertical.Count() > 0)
                {
                    var aspNetUserInsertOrgVerticalEntity = insertVertical.Select(r => new Aspnetuserverticals
                    {
                        Userid = userId,
                        Organisationid = r
                    }).ToList();

                    if (aspNetUserInsertOrgVerticalEntity != null && aspNetUserInsertOrgVerticalEntity?.Any() == true)
                    {
                        _repositoryWrapper.AspNetUserVerticalsRepository.BulkCreate(aspNetUserInsertOrgVerticalEntity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }

                // once user select vertical responcible check whether the vertical is exist with the value of isverticalresponcible true and update the record for normal vertical.
                if (updateVertical != null && updateVertical.Count > 0)
                {
                    updateVertical.ForEach(c => c.Isvertical = true);

                    _repositoryWrapper.AspNetUserVerticalsRepository.BulkUpdate(updateVertical);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                }


                return new ResultDto
                {
                    Warning = false,
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region // Vertical Responcible Insert and Update
        public async Task<ResultDto> AddOrUpdateOnVerticalResponcible(List<long> verticalResIds, int userId)
        {
            var updateSelectedVerticalResponcibleRecords = new List<Aspnetuserverticals>();
            var updateNotSelectedVerticalResponcibleRecords = new List<Aspnetuserverticals>();

            try
            {
                var aspNetUserVerticalEntityEntity = await _repositoryWrapper.AspNetUserVerticalsRepository.FindByCondition(x => x.Userid == userId).ToListAsync();

                var insertVerticalResponcible = verticalResIds.Where(f => !aspNetUserVerticalEntityEntity.Select(x => x.Organisationid.Value).Contains(f));

                updateSelectedVerticalResponcibleRecords.AddRange(aspNetUserVerticalEntityEntity.Where(f => verticalResIds.Contains(f.Organisationid.Value)).ToList());

                updateNotSelectedVerticalResponcibleRecords.AddRange(aspNetUserVerticalEntityEntity.Where(f => !verticalResIds.Contains(f.Organisationid.Value)).ToList());

                if (updateSelectedVerticalResponcibleRecords != null && updateSelectedVerticalResponcibleRecords.Count > 0)
                {
                    updateSelectedVerticalResponcibleRecords.ForEach(x => x.Isverticalresponcible = true);
                    _repositoryWrapper.AspNetUserVerticalsRepository.BulkUpdate(updateSelectedVerticalResponcibleRecords);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                if (updateNotSelectedVerticalResponcibleRecords != null && updateNotSelectedVerticalResponcibleRecords.Count > 0)
                {
                    updateNotSelectedVerticalResponcibleRecords.ForEach(x => x.Isverticalresponcible = false);
                    _repositoryWrapper.AspNetUserVerticalsRepository.BulkUpdate(updateNotSelectedVerticalResponcibleRecords);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }

                // insert the record for verticalResponcible 
                if (insertVerticalResponcible != null && insertVerticalResponcible.Count() > 0)
                {
                    var aspNetUserInsertVerticalResEntity = insertVerticalResponcible.Select(r => new Aspnetuserverticals
                    {
                        Organisationid = r,
                        Userid = userId,
                        Isvertical = false,
                        Isverticalresponcible = true,
                    }).ToList();


                    if (aspNetUserInsertVerticalResEntity != null && aspNetUserInsertVerticalResEntity?.Any() == true)
                    {
                        _repositoryWrapper.AspNetUserVerticalsRepository.BulkCreate(aspNetUserInsertVerticalResEntity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                if (aspNetUserVerticalEntityEntity != null && aspNetUserVerticalEntityEntity.Count > 0)
                {
                    // need to deleted un selected vertical responcible
                    var deleteUnselectUserVerticalRes = aspNetUserVerticalEntityEntity.Where(f => !verticalResIds.Contains(f.Organisationid.Value)
                    && f.Isverticalresponcible == true && f.Isvertical == false).ToList();
                    if (deleteUnselectUserVerticalRes != null && deleteUnselectUserVerticalRes?.Any() == true)
                    {
                        _repositoryWrapper.AspNetUserVerticalsRepository.BulkDeepDelete(deleteUnselectUserVerticalRes);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }


                return new ResultDto
                {
                    Warning = false,
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }

        #endregion



        #region //Role inser and Update
        public async Task<ResultDto> AddOrUpdateOnRole(List<int> roleIds, int userId)
        {
            try
            {
                var aspNetUserRoleEntity = await _repositoryWrapper.UserRoleRepository.FindByCondition(x => x.Userid == userId).ToListAsync();

                var selectedExtryExist = aspNetUserRoleEntity.Where(f => roleIds.Contains(f.Roleid)).Select(x => x.Roleid).ToList();

                var insertOpco = roleIds.Where(f => !selectedExtryExist.Contains(f));

                if (aspNetUserRoleEntity != null && aspNetUserRoleEntity.Count > 0)
                {
                    // need to deleted un selected Opcos
                    var deleteUnselectUserRole = aspNetUserRoleEntity.Where(f => !roleIds.Contains(f.Roleid)).ToList();
                    if (deleteUnselectUserRole != null && deleteUnselectUserRole?.Any() == true)
                    {
                        _repositoryWrapper.UserRoleRepository.BulkDeepDelete(deleteUnselectUserRole);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                if (selectedExtryExist != null && selectedExtryExist.Count <= 0)
                {
                    var aspNetUserInsertRoleEntity = roleIds.Select(r => new Aspnetuserroles
                    {
                        Userid = userId,
                        Roleid = r
                    }).ToList();

                    if (aspNetUserInsertRoleEntity != null && aspNetUserInsertRoleEntity?.Any() == true)
                    {
                        _repositoryWrapper.UserRoleRepository.BulkCreate(aspNetUserInsertRoleEntity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                if (insertOpco != null && insertOpco.Count() > 0 && selectedExtryExist?.Any() != false)
                {
                    var aspNetUserInsertRoleEntity = insertOpco.Select(r => new Aspnetuserroles
                    {
                        Userid = userId,
                        Roleid = r
                    }).ToList();

                    if (aspNetUserInsertRoleEntity != null && aspNetUserInsertRoleEntity?.Any() == true)
                    {
                        _repositoryWrapper.UserRoleRepository.BulkCreate(aspNetUserInsertRoleEntity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }

                return new ResultDto
                {
                    Warning = false,
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region // User Preference
        public async Task<ResultDto> AddOrUpdate(List<UserPrefrenceDetails> dtos, int userid, bool isHomeScreenPreference = false)
        {
            try
            {
                var createAspNetPreferences = new List<Aspnetuserpreferences>();
                var updateAspNetPreferences = new List<Aspnetuserpreferences>();

                var userPreferenceExistEntry = await _repositoryWrapper.aspNetUserPreferenceRepository.FindByCondition(x => x.Userid == userid).Include(x => x.Aspnetmodule).ToListAsync();

                var deletedPreferenceEntry = isHomeScreenPreference == false ? userPreferenceExistEntry.Where(f => !dtos.Select(x => Convert.ToInt32(x.Id)).Contains(f.Aspnetmoduleid.Value)).ToList() : null;

                if (isHomeScreenPreference == false)
                {
                    foreach (var item in dtos)
                    {
                        var entryExist = await _repositoryWrapper.aspNetUserPreferenceRepository.FindByCondition(x => x.Userid == userid && x.Aspnetmoduleid == Convert.ToInt32(item.Id)).FirstOrDefaultAsync();
                        if (entryExist != null)
                        {
                            if (userPreferenceExistEntry.Select(x => x.Aspnetmoduleid).Contains(entryExist.Aspnetmoduleid) && isHomeScreenPreference == false)
                            {
                                entryExist.Permission = (short)(item.ScreenPermission ?? entryExist.Permission.Value);
                                updateAspNetPreferences.Add(entryExist);
                            }
                        }
                        else
                        {
                            var preferenceEntity = new Aspnetuserpreferences();
                            preferenceEntity.Aspnetmoduleid = Convert.ToInt32(item.Id);
                            preferenceEntity.Permission = (short)(item.ScreenPermission ?? 7);
                            preferenceEntity.Userid = userid;
                            preferenceEntity.Order = item.Order;
                            preferenceEntity.Isuserpreference = true;

                            createAspNetPreferences.Add(preferenceEntity);
                        }
                    }
                }
                if (updateAspNetPreferences != null && updateAspNetPreferences?.Any() == true && isHomeScreenPreference == false)
                {
                    _repositoryWrapper.aspNetUserPreferenceRepository.BulkUpdate(updateAspNetPreferences.DistinctBy(x => x.Aspnetuserpreferenceid).ToList());
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                if (createAspNetPreferences != null && createAspNetPreferences?.Any() == true && isHomeScreenPreference == false)
                {
                    _repositoryWrapper.aspNetUserPreferenceRepository.BulkCreate(createAspNetPreferences.Distinct().ToList());
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                if (deletedPreferenceEntry?.Any() == true && isHomeScreenPreference == false)
                {
                    deletedPreferenceEntry.ForEach(f => f.Aspnetmodule = null);
                    _repositoryWrapper.aspNetUserPreferenceRepository.BulkDeepDelete(deletedPreferenceEntry.DistinctBy(x => x.Aspnetuserpreferenceid).ToList());
                }
                if (isHomeScreenPreference)
                {
                    var updatedUnSelectRecordsForHomeScreenPreference = userPreferenceExistEntry.Where(f => !dtos.Select(x => Convert.ToInt32(x.Id)).Contains(f.Aspnetmoduleid.Value)).ToList();
                    if (updatedUnSelectRecordsForHomeScreenPreference != null && updatedUnSelectRecordsForHomeScreenPreference.Count > 0)
                    {
                        updatedUnSelectRecordsForHomeScreenPreference.ForEach(f => f.Isuserpreference = false);
                        _repositoryWrapper.aspNetUserPreferenceRepository.BulkUpdate(updatedUnSelectRecordsForHomeScreenPreference);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                    var updatedSelectRecordsForHomeScreenPreference = userPreferenceExistEntry.Where(f => dtos.Select(x => Convert.ToInt32(x.Id)).Contains(f.Aspnetmoduleid.Value)).ToList();
                    if (updatedSelectRecordsForHomeScreenPreference != null && updatedSelectRecordsForHomeScreenPreference.Count > 0)
                    {
                        updatedSelectRecordsForHomeScreenPreference.ForEach(f => f.Isuserpreference = true);
                        _repositoryWrapper.aspNetUserPreferenceRepository.BulkUpdate(updatedSelectRecordsForHomeScreenPreference);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();


                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess,
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

        #endregion

        #region //ROV page
        /// <summary>
        /// This  function is used for get the list of roles,opcos and verticalResponsible for perticular user.
        /// </summary>
        /// <param name="aspNetUserRoleDto"></param>
        /// <returns></returns>
        public async Task<ResultDto> GetUserRoles(AspnetuserroleQueryDto aspNetUserRoleDto)
        {
            try
            {
                var predicateResult = ApplyFilterForROV(aspNetUserRoleDto);

                var query = await GetROVQuery(predicateResult);
                var data = query.ToList();

                IEnumerable<AspNetUserROVGridDto> AspnetuserROVResult;
                AspnetuserROVResult = _mapper.Map<IEnumerable<AspNetUserROVGridDto>>(data);

                #region // Organisation detils
               // var organisationDetaial = await GetOrganisationDetails();
                #endregion

                #region // User perfrence details
                var className = ConstantValueFilter.RoleWisePreferenceName;
                var userId = aspNetUserRoleDto.UserId.Select(x => x).FirstOrDefault();
                var userPrefrenceDetails = await _abstractionLayerManager.FindWithConditionForUserPrefrenceRecords(userId, className);
                #endregion

                #region // User Prefrence date

                var gridDetails = await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == className).FirstOrDefaultAsync();
                #endregion               

                return new ResultDto
                {
                    Data = new
                    {
                        AspNetUserRovDetails = AspnetuserROVResult.FirstOrDefault(),
                        UserPrefrenceDetails = userPrefrenceDetails.ToList(),
                        PrefrenceDate = gridDetails!= null? gridDetails.Preferencedate : null,
                        MessagingDate = gridDetails != null ? gridDetails.Messagingdate : null,                       
                    }
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }

        private async Task<IQueryable<ApplicationUser>> GetROVQuery(ExpressionStarter<Aspnetusers> predicateResult)
        {
            var query = _repositoryWrapper.UserRepository.FindByConditionWithDelete(predicateResult)
              .Include(x => x.Aspnetuserroles).ThenInclude(x => x.Role).Include(x => x.AspnetuseropcosUser).ThenInclude(x => x.Opco)
              .Include(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical);

            var data = await query.ToListAsync();

            return data.ToList().Select(x => ApplicationUserMapper.GetApplicationUserMapper(x,true)).AsQueryable();



        }
        private static ExpressionStarter<Aspnetusers> ApplyFilterForROV(AspnetuserroleQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Aspnetusers>(true);
            var predicateInner = PredicateBuilder.New<Aspnetusers>(true);

            
            if (buildFilterDto.UserId != null && buildFilterDto.UserId.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.UserId)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }          

            return predicateResult;
        }
        #endregion

        #region // Get Org details which are associated to selected verticals
        public async Task<List<OrganisatioDtoGrid>> GetOrganisationDetails()
        {
            var result = new List<OrganisatioDtoGrid>();
            try
            {
                var organistionEntity = await _repositoryWrapper.OrganisationRepository.FindAll()
                    .Include(i => i.Mainorganisation).Include(i => i.Practice).ThenInclude(i => i.Practiceemail).ToListAsync();
                if (organistionEntity != null)
                {
                    result = organistionEntity.Select(x => new OrganisatioDtoGrid
                    {
                        MainOrganisation = x?.Mainorganisation?.Mainorganisationdescription,
                        Practice = x?.Practice?.Practicedescription,
                        PracticeContact = x?.Practice?.Practiceemail?.Email,
                        OrganisationId = x.Organisationid,
                        VerticalId = x.Verticalid,
                    }).ToList();
                    
                }
                return result;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

    }
}
