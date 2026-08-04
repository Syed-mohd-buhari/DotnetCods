using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.MajorSwBuildsDesignContact;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Lookup;
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

namespace CAM.BusinessManager.LookUp
{
    public class MajorSwBuidlsDesignContactManager : BaseManager 
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;

        public MajorSwBuidlsDesignContactManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }


        #region //UiMemberFunctions
        public QueryResultDto<MajorSwBuildsDesignContactDto> FindWithCondition(MajorSwBuildsDesignContactQueryDto MajorSwBuildsDesignContactQueryDto)
        {
            
            var predicateResult = ApplyFilter(MajorSwBuildsDesignContactQueryDto);
            var rtn = new QueryResultDto<MajorSwBuildsDesignContactDto>(new GenerateRenderForGrid<MajorSwBuildsDesignContactDto>(_manager))
            {

            };
            var query = GetQuery(predicateResult, MajorSwBuildsDesignContactQueryDto.Deleted ?? false).ApplyOrdering(MajorSwBuildsDesignContactQueryDto, GetColumnsMap());
            rtn.TotalItems = query.Count();
            query = query.ApplyPaging(MajorSwBuildsDesignContactQueryDto);
            var data = query.ToList();

            IEnumerable<MajorSwBuildsDesignContactDto> MajorSwBuildsDesignContactDto;

            MajorSwBuildsDesignContactDto = _mapper.Map<IEnumerable<MajorSwBuildsDesignContactDto>>(data);

            rtn.Items = MajorSwBuildsDesignContactDto.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Majorswbuildsdesigncontacts> ApplyFilter(MajorSwBuildsDesignContactQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Majorswbuildsdesigncontacts>();
            var predicateInner = PredicateBuilder.New<Majorswbuildsdesigncontacts>();

            if (buildFilterDto.MajorSwBuidlsDesignContactId != null && buildFilterDto.MajorSwBuidlsDesignContactId.Any())
            {
                predicateInner = PredicateBuilder.New<Majorswbuildsdesigncontacts>();
                foreach (var item in buildFilterDto.MajorSwBuidlsDesignContactId)
                    predicateInner.Or(x => x.Majorswbuidlsdesigncontactid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MajorSoftwareBuildId != null && buildFilterDto.MajorSoftwareBuildId.Any())
            {
                predicateInner = PredicateBuilder.New<Majorswbuildsdesigncontacts>();
                foreach (var item in buildFilterDto.MajorSoftwareBuildId)
                    predicateInner.Or(x => x.Majorsoftwarebuildsid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DesignContactId != null && buildFilterDto.DesignContactId.Any())
            {
                predicateInner = PredicateBuilder.New<Majorswbuildsdesigncontacts>();
                foreach (var item in buildFilterDto.DesignContactId)
                    predicateInner.Or(x => x.Designcontactid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Majorswbuildsdesigncontacts>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Majorswbuildsdesigncontacts>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }

        private IQueryable<MajorSwBuidlsDesignContact> GetQuery(ExpressionStarter<Majorswbuildsdesigncontacts> predicateResult, bool includeDeleted)
        {                     
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.FindByCondition(predicateResult, includeDeleted)
                       .Include(x => x.CreationuserNavigation)
                       .Include(x => x.ModificationuserNavigation)
                       .Include(x => x.Designcontact)
               : _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.FindAll()
                      .Include(x => x.CreationuserNavigation)
                      .Include(x => x.ModificationuserNavigation)
                      .Include(x => x.Designcontact);
            return query.AsEnumerable().Select(x => MajorSwBuidlsDesignContactsMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<MajorSwBuidlsDesignContact, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<MajorSwBuidlsDesignContact, object>>[]>
            {
                ["majorSwBuidlsDesignContactId"] = new Expression<Func<MajorSwBuidlsDesignContact, object>>[] { p => p.MajorSwBuidlsDesignContactId },
                ["majorsoftwarebuildsId"] = new Expression<Func<MajorSwBuidlsDesignContact, object>>[] { p => p.MajorsoftwarebuildsId },
                ["designContactId"] = new Expression<Func<MajorSwBuidlsDesignContact, object>>[] { p => p.DesignContactId },
                ["lastModifiedBy"] = new Expression<Func<MajorSwBuidlsDesignContact, object>>[] { p => p.ModificationUserEntity.Email },
                ["lastModifiedValue"] = new Expression<Func<MajorSwBuidlsDesignContact, object>>[] { p => p.ModificationDate },

            };
        }

        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, MajorSwBuildsDesignContactQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "majorSwBuidlsDesignContactId" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.MajorSwBuidlsDesignContactId.ToString(), Value = p.MajorSwBuidlsDesignContactId.ToString() }).Distinct().ToList()
                      : query
                           .Where(x => x.MajorSwBuidlsDesignContactId.ToString().Contains(propertyFilter)).Select(p =>
                                new FilterValueDto { Text = p.MajorSwBuidlsDesignContactId.ToString(), Value = p.MajorSwBuidlsDesignContactId.ToString() }).Distinct()
                               .ToList(),
                "majorSoftwareBuildsId" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.MajorsoftwarebuildsId.ToString(), Value = p.MajorsoftwarebuildsId.ToString() }).Distinct().ToList()
                     : query
                          .Where(x => x.MajorsoftwarebuildsId.ToString().Contains(propertyFilter)).Select(p =>
                             new FilterValueDto { Text = p.MajorsoftwarebuildsId.ToString(), Value = p.MajorsoftwarebuildsId.ToString() }).Distinct()
                            .ToList(),
                "designContactId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.DesignContactId.ToString(), Value = p.DesignContactId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.DesignContactId.ToString().Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.DesignContactId.ToString(), Value = p.DesignContactId.ToString() }).Distinct()
                          .ToList(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email, Value = p.ModificationUserEntity.Email }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.ModificationUserEntity.Email.Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email, Value = p.ModificationUserEntity.Email }).Distinct().ToList(),


                _ => new List<FilterValueDto>()
            };

            return rtn;



        }
        #endregion

        #region CRUD Operations
        public async Task<ResultDto> Add(MajorSwBuildsDesignContactCreateDto dto)
        {
            var entity = _mapper.Map<MajorSwBuidlsDesignContact>(dto);
            _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.Create(MajorSwBuidlsDesignContactsMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }

        public async Task<ResultDto> Update(MajorSwBuildsDesignContactUpdatedto dto)
        {


            var MajorSwBuildsDesignContactModel = _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.FindByCondition(x => x.Majorswbuidlsdesigncontactid == dto.MajorSwBuidlsDesignContactId).FirstOrDefault();
            var MajorSwBuildsDesignContactEntity = MajorSwBuidlsDesignContactsMapper.Get(MajorSwBuildsDesignContactModel);

            if (MajorSwBuildsDesignContactEntity != null)
            {
                MajorSwBuildsDesignContactEntity.MajorSwBuidlsDesignContactId = dto.MajorSwBuidlsDesignContactId;
                MajorSwBuildsDesignContactEntity.DesignContactId = dto.DesignContactId;
                MajorSwBuildsDesignContactEntity.MajorsoftwarebuildsId = dto.MajorSoftwareBuildId;
                var MajorSwBuildsDesignContactSetEntity = MajorSwBuidlsDesignContactsMapper.Set(MajorSwBuildsDesignContactEntity);
                _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.Update(MajorSwBuildsDesignContactSetEntity);
                await _repositoryWrapper.SaveAsync();
            }


            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = false,
                Data = MajorSwBuildsDesignContactEntity.MajorSwBuidlsDesignContactId
            };
        }
 // No reference for below methods
        //public MajorSwBuildsDesignContactCreateDto GetCraetePage()
        //{
        //    var designContacts = _repositoryWrapper.OrganisationRepository.FindAll()
        //                          .Include(x => x.Contact)
        //                          .ToDictionary(x => x.Contactid, x => x.Contact.Email);
        //    return new MajorSwBuildsDesignContactCreateDto()
        //    {
        //         DesignContacts = designContacts,
        //    };
        //}
        //public MajorSwBuildsDesignContactUpdatedto GetUpdatePage(long id)
        //{
        //    var MajorSwBuildsDesignContactyModel = _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.FindByCondition(x => x.Majorswbuidlsdesigncontactid == id).Include(x => x.ModificationuserNavigation).FirstOrDefault();
        //    var dto = new MajorSwBuildsDesignContactUpdatedto();
        //    var designContacts = _repositoryWrapper.OrganisationRepository.FindAll()
        //              .Include(x => x.Contact)
        //              .ToDictionary(x => x.Contactid, x => x.Contact.Email);
        //    if (MajorSwBuildsDesignContactyModel != null)
        //    {
        //        dto.MajorSwBuidlsDesignContactId = MajorSwBuildsDesignContactyModel.Majorswbuidlsdesigncontactid;
        //        dto.MajorSoftwareBuildId = MajorSwBuildsDesignContactyModel.Majorsoftwarebuildsid;
        //        dto.DesignContactId = MajorSwBuildsDesignContactyModel.Designcontactid;
        //        dto.LastModified = MajorSwBuildsDesignContactyModel.Modificationdate;
        //        dto.LastModifiedBy = MajorSwBuildsDesignContactyModel.ModificationuserNavigation.Email;
        //        dto.DesignContacts = designContacts;
        //    }
        //    return dto;
        //}

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.MajorSwBuidlsDesignContactsRepository
                .FindByCondition(x => x.Majorswbuidlsdesigncontactid == id).SingleAsync();

            if (entity != null)
            {
                _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.Delete(entity);
                await _repositoryWrapper.SaveAsync();

                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Majorswbuidlsdesigncontactid,
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = id
                };
            }
        }
        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.MajorSwBuidlsDesignContactsRepository
               .FindByConditionWithDelete(x => x.Majorswbuidlsdesigncontactid == id).SingleAsync();
            if (entity != null)
            {
                _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.DeleteDeep(entity);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Majorswbuidlsdesigncontactid
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = id
                };
            }
        }
        #endregion
    }
}
