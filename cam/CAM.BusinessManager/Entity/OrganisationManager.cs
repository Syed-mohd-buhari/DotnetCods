using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.Organisation;
using CAM.DataTransferObjects.FunctionalityDto;
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
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class OrganisationManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        private readonly CommonManager _commonManager;
        private readonly ILoggerManager _logger;


        public OrganisationManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager, ILoggerManager logger,
             IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonManager commonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _commonManager = commonManager;
            _logger = logger;
        }


        #region UiMemberFunctions
        public QueryResultDto<OrganisatioDtoGrid> FindWithCondition(OrganisationQueryDto OrganisationQueryDto, bool isRemoveOpcoAndVertical = false)
        {
            var predicateResult = ApplyFilter(OrganisationQueryDto);
            var rtn = new QueryResultDto<OrganisatioDtoGrid>(new GenerateRenderForGrid<OrganisatioDtoGrid>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.OrganisationRepository.Count(predicateResult) : _repositoryWrapper.OrganisationRepository.Count(),
            };
            var query = GetQuery(predicateResult, OrganisationQueryDto.Deleted ?? false).ApplyOrdering(OrganisationQueryDto, GetColumnsMap()).ApplyPaging(OrganisationQueryDto);
            var data = query.ToList();

            IEnumerable<OrganisatioDtoGrid> OrganisatioDtoGrid;

            OrganisatioDtoGrid = _mapper.Map<IEnumerable<OrganisatioDtoGrid>>(query);

            rtn.Items = OrganisatioDtoGrid.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Organisation> ApplyFilter(OrganisationQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Organisation>(true);
            var predicateInner = PredicateBuilder.New<Organisation>(true);

            if (buildFilterDto.OrganisationId != null && buildFilterDto.OrganisationId.Any())
            {
                predicateInner = PredicateBuilder.New<Organisation>();
                foreach (var item in buildFilterDto.OrganisationId)
                    predicateInner.Or(x => x.Organisationid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MainOrganisationId != null && buildFilterDto.MainOrganisationId.Any())
            {
                predicateInner = PredicateBuilder.New<Organisation>();
                foreach (var item in buildFilterDto.MainOrganisationId)
                    predicateInner.Or(x => x.Mainorganisationid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MainOrganisation != null && buildFilterDto.MainOrganisation.Any())
            {
                predicateInner = PredicateBuilder.New<Organisation>();
                foreach (var item in buildFilterDto.MainOrganisation)
                    predicateInner.Or(x => x.Mainorganisation.Mainorganisationdescription == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Practice != null && buildFilterDto.Practice.Any())
            {
                predicateInner = PredicateBuilder.New<Organisation>();
                foreach (var item in buildFilterDto.Practice)
                    predicateInner.Or(x => x.Practice.Practicedescription == item);
                predicateResult.And(predicateInner);
            }
                       
            if (buildFilterDto.VerticalResponsible != null && buildFilterDto.VerticalResponsible.Any())
            {
                predicateInner = PredicateBuilder.New<Organisation>();
                foreach (var item in buildFilterDto.VerticalResponsible)
                    predicateInner.Or(x => x.Vertical.Verticalresponsibleid == item);
                predicateResult.And(predicateInner);
            }
                     
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Organisation>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Organisation>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);

                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<OrganisationModel> GetQuery(ExpressionStarter<Organisation> predicateResult, bool includeDeleted)
        {
            var query = _repositoryWrapper.OrganisationRepository.FindByCondition(predicateResult, includeDeleted)
                .Include(x => x.Mainorganisation)
                .Include(x => x.Practice).ThenInclude(x => x.Practiceemail)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Vertical);
                
              
            return query.AsEnumerable().Select(x => OrganisationMapper.GetOrganisationMapper(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<OrganisationModel, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<OrganisationModel, object>>[]>
            {
                ["organisationId"] = new Expression<Func<OrganisationModel, object>>[] { p => p.OrganisationId },
                ["headOfOrganisationId"] = new Expression<Func<OrganisationModel, object>>[] { p => p.MainOrganisationId },
                ["mainOrganisation"] = new Expression<Func<OrganisationModel, object>>[] { p => p.MainOrganisation.MainorganisationDescription },
                ["practice"] = new Expression<Func<OrganisationModel, object>>[] { p => p.Practice.PracticeDescription },
                ["practiceContact"] = new Expression<Func<OrganisationModel, object>>[] { p => p.Practice.PracticeEmailId },
                ["lastModifiedValue"] = new Expression<Func<OrganisationModel, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<OrganisationModel, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationUser"] = new Expression<Func<OrganisationModel, object>>[] { p => p.CreationUser },
                ["creationDate"] = new Expression<Func<OrganisationModel, object>>[] { p => p.CreationDate },
                //["subdomainResponsibleId"] = new Expression<Func<OrganisationModel, object>>[] { p => p.SubdomainResponsibleId },
                //["ContactId"] = new Expression<Func<OrganisationModel, object>>[] { p => p.ContactId },
                ["verticalResponsible"] = new Expression<Func<OrganisationModel, object>>[] { p => p.VerticalResponsible},
                ["verticalResponsibleId"] = new Expression<Func<OrganisationModel, object>>[] { p => p.VerticalResponsibleId },
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, OrganisationQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "organisationId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.OrganisationId.ToString(), Value = p.OrganisationId.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.OrganisationId.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.OrganisationId.ToString(), Value = p.OrganisationId.ToString() }).Distinct()
                   .ToList(),

                "mainOrganisation" => string.IsNullOrEmpty(propertyFilter)
                 ? query.Select(p => new FilterValueDto
                 { Text = p.MainOrganisation.MainorganisationDescription, Value = p.MainOrganisation.MainorganisationDescription }).Distinct().ToList()
                 : query
                     .Where(x => x.MainOrganisation.MainorganisationDescription.Contains(propertyFilter)).Select(p =>
                         new FilterValueDto { Text = p.MainOrganisation.MainorganisationDescription, Value = p.MainOrganisation.MainorganisationDescription }).Distinct()
                     .ToList(),
                "verticalResponsible" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.VerticalResponsibleName != null && x.VerticalResponsibleId != null).Select(p => new FilterValueDto
                    { Text = p.VerticalResponsibleName, Value = p.VerticalResponsibleId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.VerticalResponsibleName.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.VerticalResponsibleName, Value = p.VerticalResponsibleId.ToString() }).Distinct()
                        .ToList(),              
                                        
                "practice" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Practice.PracticeDescription, Value = p.Practice.PracticeDescription }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Practice.PracticeDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Practice.PracticeDescription, Value = p.Practice.PracticeDescription }).Distinct().ToList(),

                "practiceContact" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Practice.PracticeEmail.Email, Value = p.Practice.PracticeEmail.Email }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Practice.PracticeEmail.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Practice.PracticeEmail.Email, Value = p.Practice.PracticeEmail.Email }).Distinct().ToList(),
                            
                "contact" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.ContactNavigation.Email, Value = p.ContactNavigation.Email }).Distinct().ToList()
                   : query
                       .Where(x => x.ContactNavigation.Email.Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.ContactNavigation.Email, Value = p.ContactNavigation.Email }).Distinct()
                       .ToList(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                                   ? query.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                                   : query
                                       .Where(x =>
                                           x.ModificationuserNavigation.Email.Contains(
                                               propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),




                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

        #endregion

        #region CRUD Operations

        public async Task<OrganisationCreateDto> GetCreatepage()
        {
            try
            {
                var mainOrganisation = await _repositoryWrapper.MainOrganisationRepository.FindAll().ToDictionaryAsync(x => x.Mainorganisationid, x => x.Mainorganisationdescription);
                var practices = await _repositoryWrapper.PracticeRepository.FindAll().ToDictionaryAsync(x => x.Practiceid, x => x.Practicedescription);
                //var contacts = await _repositoryWrapper.UserRepository.FindAll().Where(x => x.Email.ToLower() != "admincam").ToDictionaryAsync(x => x.Id, x => x.Email);
                //var subDomainResponsible = await _repositoryWrapper.SubDomainResponsible.FindAll().ToDictionaryAsync(x => x.Subdomainresponsibleid, x => x.Subdomainresponsible);
                var practiceContacts = await _repositoryWrapper.PracticeRepository.FindAll()
                                        .Include(x => x.Practiceemail)
                                        .ToDictionaryAsync(x => x.Practiceemailid, x => x.Practiceemail.Email);
                var practiceMethod = await _repositoryWrapper.PracticeRepository.FindAll().Include(x => x.Practiceemail).ToDictionaryAsync(x => x.Practiceid, y => y.Practiceemailid);
                var verticalResponsible = await _repositoryWrapper.VerticalResponsible.FindAll().ToDictionaryAsync(x => (int?)x.Verticalresponsibleid, x => x.Verticalresponsible);

                var result = new OrganisationCreateDto()
                {
                    
                    MainOrganisations = mainOrganisation,
                    Practices = practices,
                    PracticeContacts = practiceContacts,
                    MainOrganisationId = mainOrganisation.Select(x => x.Key).FirstOrDefault(),
                    PracticeContactId = (int)practiceContacts.Select(x => x.Key).FirstOrDefault(),
                    PracticeId = practices.Select(x => x.Key).FirstOrDefault(),
                    PracticeMethods = practiceMethod,               
                    VerticalResponsible = verticalResponsible
                    
                };
                return result;
            }
            catch (Exception ex)
            {
              _logger.LogError(ex.StackTrace);
                throw;
            }

        }

        public async Task<ResultDto> Add(OrganisationCreateDto dto)
        {
            var entity = _mapper.Map<OrganisationModel>(dto);
            var entityExists = _repositoryWrapper.OrganisationRepository.FindByCondition(x => x.Verticalid== dto.VerticalResponsibleId 
            && x.Mainorganisationid == dto.MainOrganisationId && x.Practiceid == dto.PracticeId).ToList();
            if (entityExists.Count() == 0)
            {
                var organisationEntity = new OrganisationModel
                {
                    MainOrganisationId = dto.MainOrganisationId,
                    PracticeId = dto.PracticeId,
                    VerticalResponsibleId = dto.VerticalResponsibleId,

                };

                _repositoryWrapper.OrganisationRepository.Create(OrganisationMapper.Set(organisationEntity));
                await _repositoryWrapper.SaveAsync();
                return new ResultDto { Info = ResultMessages.EntryAddSuccess };
            }
            else
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryAddExists,
                    Data = entityExists.Select(x => x.Organisationid).FirstOrDefault()
                };
            }

        }

        public OrganisationUpdateDto GetUpdatePage(long id)
        {
            var mainOrganisation = _repositoryWrapper.MainOrganisationRepository.FindAll().ToDictionary(x => x.Mainorganisationid, x => x.Mainorganisationdescription);
            var practices = _repositoryWrapper.PracticeRepository.FindAll().ToDictionary(x => x.Practiceid, x => x.Practicedescription);
            //var contacts = _repositoryWrapper.UserRepository.FindAll().Where(x => x.Email.ToLower() != "admincam").ToDictionary(x => x.Id, x => x.Email);
            //var subDomainResponsible = _repositoryWrapper.SubDomainResponsible.FindAll().ToDictionary(x => x.Subdomainresponsibleid, x => x.Subdomainresponsible);
            var organisationEntity = _repositoryWrapper.OrganisationRepository.FindByCondition(x => x.Organisationid == id)
                                      .Include(x => x.Practice)
                                      //.Include(x => x.Practice).ThenInclude(x => x.Practiceemail)
                                      //.Include(x => x.Mainorganisation)
                                      //.Include(x => x.Contact)
                                      //.Include(x => x.Subdomainresponsible)
                                      .FirstOrDefault();
            var practiceContacts = _repositoryWrapper.PracticeRepository.FindAll()
                        .Include(x => x.Practiceemail)
                        .ToDictionary(x => x.Practiceemailid, x => x.Practiceemail?.Email);
            var practiceMethod = _repositoryWrapper.PracticeRepository.FindAll().Include(x => x.Practiceemail).ToDictionary(x => x.Practiceid, y => y.Practiceemailid);
            var verticalResponsible =  _repositoryWrapper.VerticalResponsible.FindAll().ToDictionary(x => (int?) x.Verticalresponsibleid, x => x.Verticalresponsible);
            var dto = new OrganisationUpdateDto();
            if (organisationEntity != null)
            {
                dto.OrganisationId = organisationEntity.Organisationid;
                //dto.MainOrganisation = organisationEntity.Mainorganisation.Mainorganisationdescription;
                dto.MainOrganisationId = organisationEntity.Mainorganisationid;
                //dto.Practice = organisationEntity.Practice.Practicedescription;
                dto.PracticeId = organisationEntity.Practiceid;              
                dto.PracticeContactId = (int)organisationEntity.Practice.Practiceemailid;                
                dto.MainOrganisations = mainOrganisation;
                dto.Practices = practices;
                dto.PracticeContacts = practiceContacts;               
                dto.PracticeMethods = practiceMethod;
                dto.VerticalResponsibleId = organisationEntity.Verticalid;
                dto.VerticalResponsible = verticalResponsible;



            }
            return dto;
        }

        public async Task<ResultDto> Update(OrganisationUpdateDto dto)
        {

            
            var organisationModel = _repositoryWrapper.OrganisationRepository.FindByCondition(x => x.Verticalid == dto.VerticalResponsibleId 
            && x.Mainorganisationid == dto.MainOrganisationId 
            && x.Practiceid == dto.PracticeId).ToList();

            if (organisationModel != null && organisationModel.Count == 1)

            {
                var organisationEntity = OrganisationMapper.GetOrganisationMapper(organisationModel.FirstOrDefault()); 

                organisationEntity.OrganisationId = dto.OrganisationId;
                organisationEntity.MainOrganisationId = dto.MainOrganisationId;
                organisationEntity.PracticeId = dto.PracticeId;
                organisationEntity.VerticalResponsibleId = dto.VerticalResponsibleId;


                var organisationSetEntity = OrganisationMapper.Set(organisationEntity);
                _repositoryWrapper.OrganisationRepository.Update(organisationSetEntity);
                await _repositoryWrapper.SaveAsync();

                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Warning = false,
                    Data = organisationSetEntity.Organisationid
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateExists,
                    Warning = false,
                    Data = dto.OrganisationId
                };
            }



        }
        public async Task<ResultDto> GetRelatedRecords(long Organisationid, bool IsEduSpoc, bool IsSubDomainSpoc)
        {
           
            var error = new List<ResultMessageDto>();

            var verticalCheck = _repositoryWrapper.AspNetUserVerticalsRepository.FindByCondition(x => x.Organisationid == Organisationid)? .Select(x => x.Userid.ToString()).ToList();
                
            if (verticalCheck != null && verticalCheck.Count() > 0)
            {
                error.Add(new ResultMessageDto { Table = "User", Values = verticalCheck.ToArray()});              
               
                 if (error != null && error.Count > 0)
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.EntryDeleteNotOrphan,
                        Data = new RelatedRecordsResultDto
                        {
                            EntityName = "Organisation",
                            RecordName = String.Join("," ,verticalCheck),
                            DataRelatedList = error
                        }
                    };
                }
                else
                    return new ResultDto();

            }
            else
                return new ResultDto();
        }
        

       

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.OrganisationRepository
                .FindByCondition(x => x.Organisationid == id).Include(x=>x.Mainorganisation).FirstOrDefaultAsync();

            if (entity != null)
            {               
                _repositoryWrapper.OrganisationRepository.Delete(entity);
                await _repositoryWrapper.SaveAsync();

                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Organisationid
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotExists,
                    Data = id
                };
            }
        }
        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.OrganisationRepository
               .FindByConditionWithDelete(x => x.Organisationid == id).SingleAsync();
            if (entity != null)
            {
                _repositoryWrapper.OrganisationRepository.DeleteDeep(entity);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Organisationid
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotExists,
                    Data = id
                };
            }
        }

        //public List<ResultMessageDto> GetReferences(Organisation entity)
        //{
        //    var error = new List<ResultMessageDto>();

            //var hwExist = _repositoryWrapper.MajorHwBuidlsDesignContactsRepository.FindByCondition(x => x.Designcontactid == entity.Contactid).Include(x => x.Majorhardwarebuilds).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
            //      .ThenInclude(x => x.Systemtype).ThenInclude(x => x.Designcomponents);
            //if (hwExist != null && hwExist.Count() > 0)
            //{
            //    error.Add(new ResultMessageDto { Table = "Hardware Build", Values = hwExist.Where(x => x.Majorhardwarebuilds != null).Select(x => x.Majorhardwarebuilds.Majorhardwareid.ToString()).ToArray() });

            //    if (hwExist.ToList().Any(x => x.Majorhardwarebuilds.Systemtypesmajorhardwarebuilds.Any(y => y.Systemtype != null)))
            //    {
            //        error.Add(new ResultMessageDto
            //        {
            //            Table = "System Type",
            //            Values = hwExist.ToList().Where(x => x.Majorhardwarebuilds.Systemtypesmajorhardwarebuilds.Any(x => x.Systemtype != null))
            //        .SelectMany(x => x.Majorhardwarebuilds.Systemtypesmajorhardwarebuilds.Select(x => x.Systemtype.Systemtypeid.ToString())).ToArray()
            //        });
            //    }
            //    if (hwExist.ToList().Any(x => x.Majorhardwarebuilds.Systemtypesmajorhardwarebuilds.Any(x => x.Systemtype.Designcomponents != null && x.Systemtype.Designcomponents.Count() > 0)))
            //    {
            //        error.Add(new ResultMessageDto
            //        {
            //            Table = "Design Component",
            //            Values = hwExist.Where(x => x.Majorhardwarebuilds.Systemtypesmajorhardwarebuilds.Any(x => x.Systemtype.Designcomponents != null && x.Systemtype.Designcomponents.Count() > 0))
            //        .SelectMany(x => x.Majorhardwarebuilds.Systemtypesmajorhardwarebuilds.SelectMany(x => x.Systemtype.Designcomponents.Select(x => x.Designcomponentid.ToString()))).ToArray()
            //        });
            //    }

            //}

            //var swExist = _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.FindByCondition(x => x.Designcontactid == entity.Contactid).Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Systemtypes).ThenInclude(x => x.Designcomponents);
            //if (swExist != null && swExist.Count() > 0)
            //{
            //    error.Add(new ResultMessageDto { Table = "Software Build", Values = swExist.Where(x => x.Majorsoftwarebuilds != null).Select(x => x.Majorsoftwarebuilds.Majorsoftwarebuildsid.ToString()).ToArray() });

            //    if (swExist.ToList().Any(x => x.Majorsoftwarebuilds.Systemtypes != null && x.Majorsoftwarebuilds.Systemtypes.Count() > 0))
            //    {
            //        error.Add(new ResultMessageDto
            //        {
            //            Table = "System Type",
            //            Values = swExist.Where(x => x.Majorsoftwarebuilds.Systemtypes != null && x.Majorsoftwarebuilds.Systemtypes.Count() > 0)
            //        .SelectMany(x => x.Majorsoftwarebuilds.Systemtypes.Select(x => x.Systemtypeid.ToString())).ToArray()
            //        });
            //    }
            //    if (swExist.ToList().Any(x => x.Majorsoftwarebuilds.Systemtypes.Any(y => y.Designcomponents != null && y.Designcomponents.Count() > 0)))
            //    {
            //        error.Add(new ResultMessageDto
            //        {
            //            Table = "Design Component",
            //            Values = swExist.ToList().Where(x => x.Majorsoftwarebuilds.Systemtypes.Any(y => y.Designcomponents != null && y.Designcomponents.Count() > 0)).ToList()
            //            .SelectMany(x => x.Majorsoftwarebuilds.Systemtypes.SelectMany(x => x.Designcomponents.Select(x => x.Designcomponentid.ToString()))).ToArray()
            //        });
            //    }

            //}

            //var lcmSubSpocExist = _repositoryWrapper.LcmEngineeringSubDomainSpoc.FindByCondition(x => x.Subdomainspocid == entity.Contactid);
            //if (lcmSubSpocExist != null && lcmSubSpocExist.Count() > 0)
            //{
            //    error.Add(new ResultMessageDto { Table = "Lcm Engineering", Values = lcmSubSpocExist.Where(x => x.Lcmengineering != null).Select(x => x.Lcmengineeringid.ToString()).ToArray() });
            //}

            //var lcmEduSpocExist = _repositoryWrapper.LcmEngineeringEduSpoc.FindByCondition(x => x.Eduspocid == entity.Contactid);
            //if (lcmEduSpocExist != null && lcmEduSpocExist.Count() > 0)
            //{
            //    error.Add(new ResultMessageDto { Table = "Lcm Engineering", Values = lcmEduSpocExist.Where(x => x.Lcmengineering != null).Select(x => x.Lcmengineeringid.ToString()).ToArray() });
            //}

            //var assetSubSpocExist = _repositoryWrapper.NetworkElementAsPlannedSubDomainSpoc.FindByCondition(x => x.Subdomainspocid == entity.Contactid);
            //if (assetSubSpocExist != null && assetSubSpocExist.Count() > 0)
            //{
            //    error.Add(new ResultMessageDto { Table = "Asset", Values = assetSubSpocExist.Where(x => x.Networkelementasplanned != null).Select(x => x.Networkelementasplannedid.ToString()).ToArray() });
            //}

            //var assetEdoExist = _repositoryWrapper.NetworkElementAsPlannedEduSpoc.FindByCondition(x => x.Eduspocid == entity.Contactid);
            //if (assetEdoExist != null && assetEdoExist.Count() > 0)
            //{
            //    error.Add(new ResultMessageDto { Table = "Asset", Values = assetEdoExist.Where(x => x.Networkelementasplanned != null).Select(x => x.Networkelementasplannedid.ToString()).ToArray() });
            //}

        //    return error;
        //}
        #endregion

    }
}
