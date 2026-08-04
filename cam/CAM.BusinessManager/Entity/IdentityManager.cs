using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.Idenitty;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.ExtendedProperties;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class IdentityManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        public IdentityManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }



        public QueryResultDto<IDentitiesDtoGrid> FindWithCondition(IdentityQueryDto designComponentFilterDto)
        {
            var predicateResult = ApplyFilter(designComponentFilterDto);
            if (designComponentFilterDto.Deleted == true)
            {
                 predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            var rtn = new QueryResultDto<IDentitiesDtoGrid>(new GenerateRenderForGrid<IDentitiesDtoGrid>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.Identity.Count(predicateResult) : _repositoryWrapper.Identity.Count(),
            };
            var query = GetQuery(predicateResult, designComponentFilterDto.Deleted ?? false).ApplyOrdering(designComponentFilterDto, GetColumnsMap()).ApplyPaging(designComponentFilterDto);
         
            var data = query.ToList();

            IEnumerable<IDentitiesDtoGrid> IdentityResult;


            IdentityResult = _mapper.Map<IEnumerable<IDentitiesDtoGrid>>(data);

            rtn.Items = IdentityResult.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Identities> ApplyFilter(IdentityQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Identities>();
            var predicateInner = PredicateBuilder.New<Identities>();

            if (buildFilterDto.IdentitiesId != null && buildFilterDto.IdentitiesId.Any())
            {
                predicateInner = PredicateBuilder.New<Identities>();
                foreach (var item in buildFilterDto.IdentitiesId)
                    predicateInner.Or(x => x.Identitiesid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NetworkElementId != null && buildFilterDto.NetworkElementId.Any())
            {
                predicateInner = PredicateBuilder.New<Identities>();
                foreach (var item in buildFilterDto.NetworkElementId)
                    predicateInner.Or(x => x.Networkelementid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Opco != null && buildFilterDto.Opco.Any())
            {
                predicateInner = PredicateBuilder.New<Identities>();
                foreach (var item in buildFilterDto.Opco)
                    predicateInner.Or(x => x.Opco == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Oem != null && buildFilterDto.Oem.Any())
            {
                predicateInner = PredicateBuilder.New<Identities>();
                foreach (var item in buildFilterDto.Oem)
                    predicateInner.Or(x => x.Networkelement.Oem == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Apnodeaipaddress != null && buildFilterDto.Apnodeaipaddress.Any())
            {
                predicateInner = PredicateBuilder.New<Identities>();
                foreach (var item in buildFilterDto.Apnodeaipaddress)
                    predicateInner.Or(x => x.Apnodeaipaddress == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Apnodebipaddress != null && buildFilterDto.Apnodebipaddress.Any())
            {
                predicateInner = PredicateBuilder.New<Identities>();
                foreach (var item in buildFilterDto.Apnodebipaddress)
                    predicateInner.Or(x => x.Apnodebipaddress == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Ipaddress != null && buildFilterDto.Ipaddress.Any())
            {
                predicateInner = PredicateBuilder.New<Identities>();
                foreach (var item in buildFilterDto.Ipaddress)
                    predicateInner.Or(x => x.Ipaddress == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Elementname != null && buildFilterDto.Elementname.Any())
            {
                predicateInner = PredicateBuilder.New<Identities>();
                foreach (var item in buildFilterDto.Elementname)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationuser != null && buildFilterDto.Creationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Identities>();
                foreach (var item in buildFilterDto.Creationuser)
                    predicateInner.Or(x => x.Creationuser.ToString() == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Modificationuser != null && buildFilterDto.Modificationuser.Any())
            {
                predicateInner = PredicateBuilder.New<Identities>();
                foreach (var item in buildFilterDto.Modificationuser)
                    predicateInner.Or(x => x.Modificationuser.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Creationdate != null)
            {
                predicateInner = PredicateBuilder.New<Identities>();
                if (buildFilterDto.Creationdate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= buildFilterDto.Creationdate.StartDate);
                if (buildFilterDto.Creationdate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= buildFilterDto.Creationdate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Modificationdate != null)
            {
                predicateInner = PredicateBuilder.New<Identities>();
                if (buildFilterDto.Modificationdate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= buildFilterDto.Modificationdate.StartDate);
                if (buildFilterDto.Modificationdate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= buildFilterDto.Modificationdate.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }

        private IQueryable<NodeIdentity> GetQuery(ExpressionStarter<Identities> predicateResult, bool includeDeleted)

        {

            var query = predicateResult.IsStarted
                ? _repositoryWrapper.Identity.FindByCondition(predicateResult, includeDeleted)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
               : _repositoryWrapper.Identity.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation);
            return query.AsEnumerable().Select(x => IdentityMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<NodeIdentity, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<NodeIdentity, object>>[]>
            {
                ["identitiesId"] =new Expression<Func<NodeIdentity, object>>[] {p=> p.Identityid},
                ["networkElementId"] = new Expression<Func<NodeIdentity, object>>[] { p => p.NetworkelementId },
                ["opCo"] = new Expression<Func<NodeIdentity, object>>[] { p => p.Opco },             
                ["elementName"] = new Expression<Func<NodeIdentity, object>>[] { p => p.Elementname },
                ["apnodeaIpaddress"] = new Expression<Func<NodeIdentity, object>>[] { p => p.Apnodeaipaddress },
                ["apnodebIpaddress"] = new Expression<Func<NodeIdentity, object>>[] { p => p.Apnodebipaddress },
                ["ipaddress"] = new Expression<Func<NodeIdentity, object>>[] { p => p.Ipaddress },
                ["modificationDate"] = new Expression<Func<NodeIdentity, object>>[] { p => p.Modificationdate},
                ["modificationUser"] = new Expression<Func<NodeIdentity, object>>[] { p => p.ModificationUser},
                ["creationUser"] = new Expression<Func<NodeIdentity, object>>[] { p => p.CreationUser},
                ["creationDate"] = new Expression<Func<NodeIdentity, object>>[] { p => p.Creationdate },
                ["oem"] = new Expression<Func<NodeIdentity, object>>[] { p => p.oem },
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, IdentityQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "identitiesId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.Identityid.ToString(), Value = p.Identityid.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.Identityid.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.Identityid.ToString(), Value = p.Identityid.ToString() }).Distinct()
                   .ToList(),

                "networkElementId" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.NetworkelementId.ToString(), Value = p.NetworkelementId.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.NetworkelementId.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.NetworkelementId.ToString(), Value = p.NetworkelementId.ToString() }).Distinct()
                    .ToList(),

                "opCo" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.Opco.ToString(), Value = p.Opco.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.Opco.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.Opco.ToString(), Value = p.Opco.ToString() }).Distinct()
                    .ToList(),

                "oem" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.oem, Value = p.oem }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.oem.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.oem, Value = p.oem }).Distinct().ToList(),

                "elementName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Elementname, Value = p.Elementname }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Elementname.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Elementname, Value = p.Elementname }).Distinct().ToList(),

                "apNodeAIpaddress" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Apnodeaipaddress, Value = p.Apnodeaipaddress }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Apnodeaipaddress.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Apnodeaipaddress, Value = p.Apnodeaipaddress }).Distinct().ToList(),


                "apNodeBIpaddress" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Apnodebipaddress, Value = p.Apnodebipaddress }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Apnodebipaddress.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Apnodebipaddress, Value = p.Apnodebipaddress }).Distinct().ToList(),


                "ipAddress" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.Ipaddress, Value = p.Ipaddress }).Distinct().ToList()
                     : query
                         .Where(x =>
                             x.Ipaddress.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto { Text = p.Ipaddress, Value = p.Ipaddress }).Distinct().ToList(),


                "creationUser" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.CreationUserEntity.Email.ToString(), Value = p.CreationUserEntity.Email.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.CreationUserEntity.Email.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.CreationUserEntity.Email.ToString(), Value = p.CreationUserEntity.Email.ToString() }).Distinct().ToList(),



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

    }
}
