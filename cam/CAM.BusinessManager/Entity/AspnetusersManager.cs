using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AspNetUser;
using CAM.DataTransferObjects.Entita.AspNetUserRole;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
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
    public class AspnetusersManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private ICurrentUserService _currentUserService;
        private GridCustomColumnManager _manager;
        private AspnetuserroleManager _aspRoleManger;
        public AspnetusersManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor, AspnetuserroleManager aspRoleManger, ICurrentUserService currentUserService,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _currentUserService = currentUserService;
            _aspRoleManger = aspRoleManger;
        }

        #region //UI Member
        public QueryResultDto<AspnetuserGridDto> FindWithCondition(AspnetuserroleQueryDto aspNetUserRoleDto)
        {
            var predicateResult = ApplyFilter(aspNetUserRoleDto);
            
           
            var rtn = new QueryResultDto<AspnetuserGridDto>(new GenerateRenderForGrid<AspnetuserGridDto>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.UserRepository.Count(predicateResult) : _repositoryWrapper.UserRepository.Count(),
            };
            var query = GetQuery(predicateResult).ApplyOrdering(aspNetUserRoleDto, GetColumnsMap());
            query = query.OrderByDescending(x => x.Modificationdate);
            if (aspNetUserRoleDto?.OpcoId != null && aspNetUserRoleDto?.OpcoId.Any() == true)
            {
                query = query.Where(x => aspNetUserRoleDto.OpcoId.Any(y => x.Opcos.Any(s => s.Opcoid == y) == true));
            }
            query = query.ApplyPaging(aspNetUserRoleDto); 
            var data = query.ToList();
            var modifiedUserIdList = data?.Select(x => x.Modificationuser)?.Distinct()?.ToList();
            var modifiedUserDetail = modifiedUserIdList != null && modifiedUserIdList.Any() == true ?
                                     _repositoryWrapper.UserRepository.FindByConditionWithDelete(x => modifiedUserIdList.Contains(x.Id))
                                     .Select(x => new { x.Id, x.Email }).ToDictionary(x => x.Id, x => x.Email)
                                     : null;

            foreach (var item in data)
                item.ModificationUserEmail = modifiedUserDetail.TryGetValue(item.Modificationuser ?? 0, out var email) ? email : null;
            IEnumerable<AspnetuserGridDto> AspnetuserroleResult;


            AspnetuserroleResult = _mapper.Map<IEnumerable<AspnetuserGridDto>>(data);

            rtn.Items = AspnetuserroleResult.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Aspnetusers> ApplyFilter(AspnetuserroleQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Aspnetusers>(true);
            var predicateInner = PredicateBuilder.New<Aspnetusers>(true);

            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.Modificationuser.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.UserId != null && buildFilterDto.UserId.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.UserId)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.UserName != null && buildFilterDto.UserName.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.UserName)
                    predicateInner.Or(x => x.Username == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Email != null && buildFilterDto.Email.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.Email)
                    predicateInner.Or(x => x.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Active != null && buildFilterDto.Active.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.Active)
                    predicateInner.Or(x => x.Active == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationuser != null && buildFilterDto.Creationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.Creationuser)
                    predicateInner.Or(x => x.Creationuser == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Modificationuser != null && buildFilterDto.Modificationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.Modificationuser)
                    predicateInner.Or(x => x.Modificationuser == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationdate != null)
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                if (buildFilterDto.Creationdate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= buildFilterDto.Creationdate.StartDate);
                if (buildFilterDto.Creationdate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= buildFilterDto.Creationdate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MainOrganisation != null && buildFilterDto.MainOrganisation.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.MainOrganisation)
                    predicateInner.Or(x => x.AspnetuserverticalsUser.Any(x => x.Organisation.Mainorganisationid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Practice != null && buildFilterDto.Practice.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.Practice)
                    predicateInner.Or(x => x.AspnetuserverticalsUser.Any(x => x.Organisation.Practiceid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubdomainResponsible != null && buildFilterDto.SubdomainResponsible.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.SubdomainResponsible)
                    predicateInner.Or(x => x.Subdomainresponsibleid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PracticeContact != null && buildFilterDto.PracticeContact.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.PracticeContact)
                    predicateInner.Or(x => x.AspnetuserverticalsUser.Any(x => x.Organisation.Practice.Practiceemailid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IsSubDomainSpoc != null && buildFilterDto.IsSubDomainSpoc.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.IsSubDomainSpoc)
                    predicateInner.Or(x => x.Issubdomainspoc == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IsEduSpoc != null && buildFilterDto.IsEduSpoc.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.IsEduSpoc)
                    predicateInner.Or(x => x.Iseduspoc == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IsDesigncontact != null && buildFilterDto.IsDesigncontact.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.IsDesigncontact)
                    predicateInner.Or(x => x.Isdesigncontact == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Opco != null && buildFilterDto.Opco.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.Opco)
                    predicateInner.Or(x => x.AspnetuseropcosUser.Any(x => x.Opcoid.ToString() == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Vertical != null && buildFilterDto.Vertical.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.Vertical)
                    predicateInner.Or(x => x.AspnetuserverticalsUser.Any(x => x.Organisation.Verticalid.ToString() == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VerticalResponsible != null && buildFilterDto.VerticalResponsible.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.VerticalResponsible)
                    predicateInner.Or(x => x.AspnetuserverticalsUser.Any(x => x.Organisation.Verticalid.ToString() == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Role != null && buildFilterDto.Role.Any())
            {
                predicateInner = PredicateBuilder.New<Aspnetusers>();
                foreach (var item in buildFilterDto.Role)
                    predicateInner.Or(x => x.Aspnetuserroles.Any(x => x.Roleid.ToString() == item));
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        private IQueryable<ApplicationUser> GetQuery(ExpressionStarter<Aspnetusers> predicateResult)
        {
            var query = _repositoryWrapper.UserRepository.FindByConditionWithDelete(predicateResult)
                .Include(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Mainorganisation)
                .Include(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Practice).ThenInclude(x => x.Practiceemail)
                .Include(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical)
                .Include(x => x.AspnetuseropcosUser).ThenInclude(x => x.Opco)
                .Include(x => x.Aspnetuserroles).ThenInclude(x => x.Role)
                .Include(x => x.Subdomainresponsible);

            return query.AsNoTracking().AsEnumerable().Select(x => ApplicationUserMapper.GetApplicationUserMapper(x, true)).AsQueryable();
        }

        private Dictionary<string, Expression<Func<ApplicationUser, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ApplicationUser, object>>[]>
            {
                ["userId"] = new Expression<Func<ApplicationUser, object>>[] { p => p.Id },
                ["userName"] = new Expression<Func<ApplicationUser, object>>[] { p => p.UserName },
                ["email"] = new Expression<Func<ApplicationUser, object>>[] { p => p.Email },
                ["creationDate"] =new Expression<Func<ApplicationUser, object>>[] {p=>p.Creationdate},
                ["creationUser"] = new Expression<Func<ApplicationUser, object>>[] { p => p.Creationuser },
                ["modificationDate"] = new Expression<Func<ApplicationUser, object>>[] { p => p.Modificationdate },
                ["modificationUser"] = new Expression<Func<ApplicationUser, object>>[] { p => p.Modificationuser },

            };
        }

        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, AspnetuserroleQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult).ToList();

            if(propertyName== "lastModifiedBy")
            {
                var modifiedUserIdList = query?.Select(x => x.Modificationuser)?.Distinct()?.ToList();
                var modifiedUserDetail = modifiedUserIdList != null && modifiedUserIdList.Any() == true ?
                                         _repositoryWrapper.UserRepository.FindByConditionWithDelete(x => modifiedUserIdList.Contains(x.Id))
                                         .Select(x => new { x.Id, x.Email }).ToDictionary(x => x.Id, x => x.Email)
                                         :null;

                foreach (var item in query)
                    item.ModificationUserEmail = modifiedUserDetail.TryGetValue(item.Modificationuser ?? 0, out var email) ? email : null;

                var rtnUserEmail = string.IsNullOrEmpty(propertyFilter) ?
                                  query.Where(x => !string.IsNullOrEmpty(x.ModificationUserEmail))
                                  .Select(x => new FilterValueDto { Value = x.Modificationuser.ToString(), Text = x.ModificationUserEmail })
                                  .DistinctBy(y=>y.Value).ToList() 
                                  :
                                  query.Where(x => !string.IsNullOrEmpty(x.ModificationUserEmail) && x.ModificationUserEmail.Contains(propertyFilter))
                                  .Select(x => new FilterValueDto { Value = x.Modificationuser.ToString(), Text = x.ModificationUserEmail })
                                  .DistinctBy(y => y.Value).ToList();
                return rtnUserEmail;
            }

            var rtn = propertyName switch
            {


                "userId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.Id.ToString(), Value = p.Id.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.Id.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.Id.ToString(), Value = p.Id.ToString() }).Distinct()
                   .ToList(),



                "email" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.Email, Value = p.Email }).Distinct().ToList()
                : query
                    .Where(x => x.Email != null && x.Email.Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p?.Email, Value = p.Email }).Distinct()
                    .ToList(),

                "userName" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Where(f=>f.UserName != null).Select(p => new FilterValueDto
                   { Text = p.UserName, Value = p.UserName }).Distinct().ToList()
                   : query
                       .Where(x => x.UserName != null && x.UserName.Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.UserName, Value = p.UserName }).Distinct()
                       .ToList(),
                "opCo" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Where(f => f.Opcos != null && f.Opcos.Count > 0).SelectMany(x => x.Opcos)
                   .Select(p => new FilterValueDto
                   { Text = p?.ApplicationOpco?.OpCoName, Value = p?.ApplicationOpco?.OpCoId.ToString() }).Distinct().ToList()
                   : query
                       .Where(x => x.Opcos != null && x.Opcos.Count > 0 && x.Opcos.Select(x => x.ApplicationOpco?.OpCoId?.ToString()).ToList().Contains(propertyFilter))
                       .SelectMany(x => x.Opcos)
                       .Select(p =>
                           new FilterValueDto { Text = p?.ApplicationOpco?.OpCoName, Value = p?.ApplicationOpco?.OpCoId.ToString() }).Distinct()
                       .ToList(),

                "vertical" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Where(f => f.ApplicationOrgAndVerticals != null && f.ApplicationOrgAndVerticals.Count > 0)
                   .SelectMany(x => x?.ApplicationOrgAndVerticals).Where(f => f.Isvertical == true)
                   .Select(p => new FilterValueDto
                   { Text = p?.ApplicationOrganisation?.VerticalRes?.VerticalRes,
                     Value = p?.ApplicationOrganisation?.VerticalRes?.VerticalResId.ToString() }).Distinct().ToList()
                   : query
                       .Where(x => x.ApplicationOrgAndVerticals != null && x.ApplicationOrgAndVerticals.Count > 0 && x.ApplicationOrgAndVerticals
                       .Where(f => f.Isvertical == true)
                       .Select(r => r?.ApplicationOrganisation?.VerticalRes?.VerticalResId.ToString()).ToList().Contains(propertyFilter))
                       .SelectMany(x => x?.ApplicationOrgAndVerticals)
                       .Select(p =>
                           new FilterValueDto { Text = p?.ApplicationOrganisation?.VerticalRes?.VerticalRes, 
                               Value = p?.ApplicationOrganisation?.VerticalRes?.VerticalResId?.ToString() }).Distinct()
                       .ToList(),

                "verticalResponsible" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Where(f => f.ApplicationOrgAndVerticals != null && f.ApplicationOrgAndVerticals.Count > 0)
                   .SelectMany(x => x.ApplicationOrgAndVerticals).Where(f => f.Isverticalresponcible == true)
                   .Select(p => new FilterValueDto
                   { Text = p?.ApplicationOrganisation?.VerticalRes?.VerticalRes,
                       Value = p?.ApplicationOrganisation?.VerticalRes?.VerticalResId?.ToString() }).Distinct().ToList()
                   : query
                       .Where(x => x.ApplicationOrgAndVerticals != null && x.ApplicationOrgAndVerticals.Count > 0 && x.ApplicationOrgAndVerticals.Where(f => f.Isverticalresponcible == true)
                       .Select( r => r?.ApplicationOrganisation?.VerticalRes?.VerticalResId?.ToString()).ToList().Contains(propertyFilter))
                       .SelectMany(x => x?.ApplicationOrgAndVerticals)
                       .Select(p =>
                           new FilterValueDto { Text = p?.ApplicationOrganisation?.VerticalRes?.VerticalRes,
                               Value = p?.ApplicationOrganisation?.VerticalRes?.VerticalResId?.ToString() }).Distinct()
                       .ToList(),

                "role" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Where(f => f.UserRoles != null && f.UserRoles.Count > 0)
                   .SelectMany(X => X.UserRoles).Select(p => new FilterValueDto
                   { Text = p?.Role?.RoleName, Value = Convert.ToString( p?.Role?.RoleId) }).Distinct().ToList()
                   : query
                       .Where(x => x.UserRoles != null && x.UserRoles.Count > 0 && x.UserRoles.Select(r => Convert.ToString(r?.Role?.RoleId)).ToList().Contains(propertyFilter))
                       .SelectMany(X => X.UserRoles)
                       .Select(p =>
                           new FilterValueDto { Text = p?.Role?.RoleName, Value = p?.Role?.RoleId.ToString() }).Distinct()
                       .ToList(),
                "active" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.Active == false ? ConstantValueFilter.No.ToUpper() : ConstantValueFilter.Yes.ToUpper(), Value = p.Active.ToString() }).Distinct().ToList()
                    : query
                        .Where(p => (p.Active.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Active == false ? ConstantValueFilter.No.ToUpper() : ConstantValueFilter.Yes.ToUpper(), Value = p.Active.ToString() }).Distinct()
                        .ToList(),

                "mainOrganisation" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(f => f.ApplicationOrgAndVerticals != null && f.ApplicationOrgAndVerticals.Count > 0)
                        .Select(p => new FilterValueDto { Text = p.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.MainOrganisation.MainorganisationDescription,
                        Value = p.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.MainOranisationId.ToString()}).Distinct().ToList()
                    : query
                        .Where(p => (p.Active.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter) && 
                        p.ApplicationOrgAndVerticals != null && p.ApplicationOrgAndVerticals.Count > 0)
                        .Select(p => new FilterValueDto
                        {
                            Text = p.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.MainOrganisation.MainorganisationDescription,
                            Value = p.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.MainOranisationId.ToString()
                        }).Distinct()
                        .ToList(),

                "practice" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(f => f.ApplicationOrgAndVerticals != null && f.ApplicationOrgAndVerticals.Count > 0)
                        .Select(p => new FilterValueDto
                        {
                            Text = p.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.Practice.PracticeDescription,
                            Value = p.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.PracticeId.ToString()
                        }).Distinct().ToList()
                    : query
                        .Where(p => (p.Active.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter) &&
                        p.ApplicationOrgAndVerticals != null && p.ApplicationOrgAndVerticals.Count > 0)
                        .Select(p => new FilterValueDto
                        {
                            Text = p.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.Practice.PracticeDescription,
                            Value = p.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.PracticeId.ToString()
                        }).Distinct()
                        .ToList(),

                "practiceContact" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(f => f.ApplicationOrgAndVerticals != null && f.ApplicationOrgAndVerticals.Count > 0)
                        .Select(p => new FilterValueDto
                        {
                            Text = p.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.Practice.PracticeEmail.Email,
                            Value = p.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.Practice.PracticeEmailId.ToString()
                        }).Distinct().ToList()
                    : query
                        .Where(p => (p.Active.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter) &&
                        p.ApplicationOrgAndVerticals != null && p.ApplicationOrgAndVerticals.Count > 0)
                        .Select(p => new FilterValueDto
                        {
                            Text = p.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.Practice.PracticeEmail.Email,
                            Value = p.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.Practice.PracticeEmailId.ToString()
                        }).Distinct()
                        .ToList(),
                "subdomainResponsible" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(f => f.ApplicationSubDomainRes != null)
                        .Select(p => new FilterValueDto
                        {
                            Text = p.ApplicationSubDomainRes.SubDomainRes,
                            Value = p.ApplicationSubDomainRes.SubdomainResponsibleId.ToString()
                        }).Distinct().ToList()
                    : query
                        .Where(p => (p.Active.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter) &&
                        p.ApplicationSubDomainRes != null)
                        .Select(p => new FilterValueDto
                        {
                            Text = p.ApplicationSubDomainRes.SubDomainRes,
                            Value = p.ApplicationSubDomainRes.SubdomainResponsibleId.ToString()
                        }).Distinct()
                        .ToList(),
                "isSubDomainSpoc" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto
                        {
                            Text = p.Issubdomainspoc == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.NO.ToUpper(),
                            Value = p.Issubdomainspoc.ToString()
                        }).Distinct().ToList()
                    : query
                        .Where(p => (p.Issubdomainspoc == true ?
                        ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter))
                        .Select(p => new FilterValueDto
                        {
                            Text = p.Issubdomainspoc == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.NO.ToUpper(),
                            Value = p.Issubdomainspoc.ToString()
                        }).Distinct()
                        .ToList(),
                "isEduSpoc" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto
                        {
                            Text = p.Iseduspoc == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.NO.ToUpper(),
                            Value = p.Iseduspoc.ToString()
                        }).Distinct().ToList()
                    : query
                         .Where(p => (p.Iseduspoc == true ?
                        ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter))
                       .Select(p => new FilterValueDto
                       {
                           Text = p.Iseduspoc == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.NO.ToUpper(),
                           Value = p.Iseduspoc.ToString()
                       }).Distinct()
                        .ToList(),

                "isDesigncontact" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto
                        {
                            Text = p.Isdesigncontact == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.NO.ToUpper(),
                            Value = p.Isdesigncontact.ToString()
                        }).Distinct().ToList()
                    : query
                        .Where(p =>(p.Isdesigncontact == true ?
                        ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter))
                       .Select(p => new FilterValueDto
                       {
                           Text = p.Isdesigncontact == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.NO.ToUpper(),
                           Value = p.Isdesigncontact.ToString()
                       }).Distinct()
                        .ToList(),

                _ => new List<FilterValueDto>()
            };

            return rtn;
        }
        #endregion

        #region //Export excel
        public QueryResultDto<UserRoleDownlodeGridDto> GetUserRoles(AspnetuserroleQueryDto aspNetUserRoleDto)
        {
            var predicateResult = ApplyFilter(aspNetUserRoleDto);
            var rtn = new QueryResultDto<UserRoleDownlodeGridDto>(new GenerateRenderForGrid<UserRoleDownlodeGridDto>(_manager))
            {

            };
            var query = GetQuery(predicateResult).ApplyOrdering(aspNetUserRoleDto, GetColumnsMap());
            query= query.OrderByDescending(x => x.Modificationdate);
            rtn.TotalItems = query.Count();
            if (aspNetUserRoleDto?.OpcoId != null && aspNetUserRoleDto?.OpcoId.Any() == true)
            {
                query = query.Where(x => aspNetUserRoleDto.OpcoId.Any(y => x.Opcos.Any(s => s.Opcoid == y) == true));
            }

            query = query.ApplyPaging(aspNetUserRoleDto); // below we need add order
            var data = query.ToList();
            var modifiedUserIdList = data?.Select(x => x.Modificationuser)?.Distinct()?.ToList();
            var modifiedUserDetail = modifiedUserIdList != null && modifiedUserIdList.Any() == true ?
                                     _repositoryWrapper.UserRepository.FindByConditionWithDelete(x => modifiedUserIdList.Contains(x.Id))
                                     .Select(x => new { x.Id, x.Email }).ToDictionary(x => x.Id, x => x.Email)
                                     : null;

            foreach (var item in data)
                item.ModificationUserEmail = modifiedUserDetail.TryGetValue(item.Modificationuser ?? 0, out var email) ? email : null;


            var AspnetuserROVResult = data.Select(g => new UserRoleDownlodeGridDto
            {
                UserId = g.Id,
                UserName = g.UserName,
                Email = g.Email,
                Active = g.Active.Value,
                Role = string.Join(",",g.UserRoles?.Select(r => r.Role.RoleName).Distinct().ToList()),
                OpCo = string.Join (",", g.Opcos?.Select(x => x.ApplicationOpco.OpCoName).Distinct().ToList()),
                Vertical = string.Join(",", g.ApplicationOrgAndVerticals?.Where(f => f.Isvertical == true).Select(x => x.ApplicationOrganisation?.VerticalRes.VerticalRes).ToList()),
                VerticalResponsible = string.Join(",", g.ApplicationOrgAndVerticals?.Where(f => f.Isverticalresponcible == true).Select(x => x.ApplicationOrganisation?.VerticalRes.VerticalRes).ToList()),
                MainOrganisation = g.ApplicationOrgAndVerticals?.FirstOrDefault()?.ApplicationOrganisation.MainOrganisation?.MainorganisationDescription,
                Practice = g.ApplicationOrgAndVerticals?.FirstOrDefault()?.ApplicationOrganisation.Practice?.PracticeDescription,
                PracticeContact = g.ApplicationOrgAndVerticals?.FirstOrDefault()?.ApplicationOrganisation.Practice?.PracticeEmail?.Email,
                SubdomainResponsible = g.ApplicationSubDomainRes?.SubDomainRes,
                IsSubDomainSpoc = g.Issubdomainspoc,
                IsEduSpoc = g.Iseduspoc,
                IsDesigncontact = g.Isdesigncontact,
                LastModified=g.Modificationdate,
                LastModifiedBy = g.ModificationUserEmail,
            })
        .ToList();




            rtn.Items = AspnetuserROVResult.ToArray();
            return rtn;
        }    
        #endregion

        #region //CRUD

        /// <summary>
        /// This function is used for deactive the  user  in User table
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ResultDto> Deactivate(AspnetuserroleGridDto dto)
        {
            if (dto.Active)
            {
                var entity = await _repositoryWrapper.UserRepository.FindByConditionWithDelete(x => x.Id == dto.UserId).FirstOrDefaultAsync();
                if (entity != null)
                {                   
                    if (dto.Active)
                    {
                        entity.Active = dto.Active;
                        entity.Deleted = false;
                        _repositoryWrapper.UserRepository.Update(entity);
                    }
                                     
                }
            }
            else
            {
                var entity = await _repositoryWrapper.UserRepository.FindByCondition(x => x.Id == dto.UserId).FirstOrDefaultAsync();
                if (entity != null)
                {
                    if (entity.Isdesigncontact == false)
                    {
                        entity.Active = dto.Active;
                        entity.Deleted = true;
                        _repositoryWrapper.UserRepository.Update(entity);
                    }
                    else
                    {
                        return new ResultDto
                        {
                            Info = ResultMessages.DeactivityRelation,
                            Warning = true,
                            //Data = entity.Id,
                        };
                    }
                }
                
            }


            await _repositoryWrapper.SaveAsync();


            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                //Data = entity.Id,
            };
        }

        //public bool UserIsLinked(int userId)
        //{
        //    var entityExistInOrg =  _repositoryWrapper.OrganisationRepository.FindByCondition(x => x.Contactid == userId).FirstOrDefault();
        //    if(entityExistInOrg != null)
        //    {
        //        return true;
        //    }
        //    return false;

        //}

        public async Task<ResultDto> CreateUser(List<AspNetUserRoleUpdateDto> dtos)
        {

            int _userId = 0;

            try
            {
                foreach (var dto in dtos)
                {
                    var entity = await _repositoryWrapper.UserRepository.FindByCondition(x => x.Username == dto.UserName).FirstOrDefaultAsync();
                    if (entity == null)
                    {
                        ApplicationUser model = new ApplicationUser()
                        {
                            UserName = dto.UserName,
                            NormalizedUserName = dto.UserName.ToUpper(),
                            Email = dto.Email,
                            NormalizedEmail = dto.Email,
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = false,
                            PasswordHash = ConstantValueFilter.PasswordHash,
                            SecurityStamp = ConstantValueFilter.SecurityStamp,
                            TwoFactorEnabled = false,
                            LockoutEnabled = true,
                            AccessFailedCount = 0
                        };

                        _repositoryWrapper.UserRepository.Create(ApplicationUserMapper.SetApplicationUserMapper(model));
                        await _repositoryWrapper.SaveAsync();
                        _userId = await _repositoryWrapper.UserRepository.FindByCondition(x => x.Username == dto.UserName).Select(x => x.Id).FirstOrDefaultAsync();

                    }                  

                }
                return new ResultDto { Info = ResultMessages.EntryAddSuccess, Data = _userId };
            }
            catch
            {
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        #endregion

    }
}
