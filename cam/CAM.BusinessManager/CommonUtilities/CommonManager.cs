using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.Common;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.ReportScheduler;
using CAM.DataTransferObjects.Entita.ServicePlan;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.GenericReportDto;
using CAM.DataTransferObjects.LookUp.DeliveryTracking;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Enum;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.CommonUtilities
{
    public class CommonManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerManager _logger;


        public readonly string popupTabName = "Other Linked Reference";
        List<KeyValuePair<string, string>> referenceTableAliasName = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("Designaspects", "Design Aspects"),
            new KeyValuePair<string, string>("Dcflifecycle", "DCF Life Cycle"),
            new KeyValuePair<string, string>("Designcomponents", "Design Components"),
            new KeyValuePair<string, string>("Designcomponentfamilies", "Design Component Families"),

            new KeyValuePair<string, string>("Lcmengineering", "LCM Engineering"),
            new KeyValuePair<string, string>("Plannedactivitytypes", "Planned Activity"),
            new KeyValuePair<string, string>("Networkelementsasplanned", "Network Element As Planned"),

            new KeyValuePair<string, string>("Systemtypesmajorhardwarebuilds", "System Types Major Hardware Build"),
            new KeyValuePair<string, string>("Systemtypes", "System Type"),
            new KeyValuePair<string, string>("Softwarebuildcompatibility", "Software Build Compatibility"),
            new KeyValuePair<string, string>("Systemtypessubdomainspoc","System Types Sub Domain Spoc"),
            new KeyValuePair<string, string>("Plannedactivities", "Planned Activities"),
            new KeyValuePair<string, string>("Componentsoftwarebuildbags", "Component Software Build Bag"),
              new KeyValuePair<string, string>("Buildbags", "Build Bag"),
               new KeyValuePair<string, string>("Nfvisoftwarecompatibility", "NFVI Softwware Compatibility")

        };
        public CommonManager(IEnumerable<IRepositoryWrapper> wrappers, IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, ILoggerManager logger
             ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;

        }
        public async Task<List<string>> GetForeignKeyRefernceTable(string entityTableName, long entityPKeyId, List<string> excludedForeignKeyTables)
        {

            List<string> returnLinkedFkeysReference = new List<string>();
            var linkedReferenceDetails = await _repositoryWrapper.GetLinkedReferenceDetails(entityTableName, entityPKeyId);

            var referencedTables = ((Infrastucture.QueryResult.QueryResultDto<List<Repository.ReferenceTable>>)linkedReferenceDetails)?.Items;
            if (referencedTables != null && referencedTables.Count() > 0)
            {

                foreach (var item in referencedTables)
                {
                    string linkedPrimaryKeysString = string.Join(',', item.Select(x => x.ReferencePrimaryKey));

                    var getTableName = item.GroupBy(x => x.ReferenceTableName).Select(x => x.Key);//.FirstOrDefault();
                    foreach (var rfItem in getTableName)
                    {
                        string getTableNamefromEntity = rfItem.Replace("OracleModels.DBModels.", "");
                        if (excludedForeignKeyTables.Where(x => x == getTableNamefromEntity).FirstOrDefault() == null)
                        {
                            string aliasName = referenceTableAliasName.Find(x => x.Key == getTableNamefromEntity).Value;
                            aliasName = string.IsNullOrEmpty(aliasName) ? getTableNamefromEntity : aliasName;
                            returnLinkedFkeysReference.Add(aliasName + " - " + linkedPrimaryKeysString.TrimEnd(','));
                        }


                    }
                }
            }

            return returnLinkedFkeysReference;
        }

        public async Task<List<string>> GetReferencedForeignKeyTablesAsync(string entityTableName, long primaryKeyId, List<string> excludedForeignKeyTables, Dictionary<string, List<long>> referencedTableIds)

        {

            var referencedForeignKeyTables = new List<string>();

            var linkedReferenceDetails = await _repositoryWrapper.GetLinkedReferenceDetails(entityTableName, primaryKeyId);

            var referencedTables = (linkedReferenceDetails as Infrastucture.QueryResult.QueryResultDto<List<Repository.ReferenceTable>>)?.Items;

            if (referencedTables != null && referencedTables.Any())

            {

                foreach (var referencedTable in referencedTables)

                {

                    var foreignKeyTableNames = referencedTable.GroupBy(x => x.ReferenceTableName).Select(g => g.Key).ToList();

                    foreach (var foreignKeyTableName in foreignKeyTableNames)

                    {

                        string cleanTableName = foreignKeyTableName.Replace("OracleModels.DBModels.", "");

                        var existingReferenceID = referencedTableIds

                            .Where(entry => entry.Key == cleanTableName)

                            .SelectMany(entry => entry.Value)

                            .ToList();

                        var missingPrimaryKeys = referencedTable

                            .Select(x => x.ReferencePrimaryKey)

                            .Where(pk => existingReferenceID.All(existingPk => !existingPk.ToString().Contains(pk.ToString())))

                            .ToList();

                        string missingPrimaryKeysString = string.Join(',', missingPrimaryKeys).TrimEnd(',');

                        if (!string.IsNullOrEmpty(missingPrimaryKeysString) && !excludedForeignKeyTables.Contains(cleanTableName))

                        {

                            string aliasName = referenceTableAliasName.Find(x => x.Key == cleanTableName).Value;

                            aliasName = string.IsNullOrEmpty(aliasName) ? cleanTableName : aliasName;

                            referencedForeignKeyTables.Add($"{aliasName} - {missingPrimaryKeysString}");

                        }

                    }

                }

            }

            return referencedForeignKeyTables;

        }
        public List<int?> GetDesignContactFromMajorSoftwareAndHardWare(List<DesignComponent> dcEntity)
        {

            if (dcEntity?.Any() == false) return new List<int?>();

            var designContactList = dcEntity.SelectMany(t => t?.SystemType?.MajorSoftwareBuilds?.MajorSwBuidlsDesignContacts.Where(x => x.Deleted == false).Select(m => (int?)m.DesignContactId))?.
                  Distinct().ToList();

            var majorHardwareDesignContact = dcEntity.SelectMany(t => t?.SystemType?.SystemTypesMajorHardwareBuilds?.SelectMany(m =>
            m.MajorHardware?.MajorHwBuidlsDesignContacts.Where(x => x.Deleted == false).Select(n => (int?)n.DesignContactId))?.
               Distinct().ToList())?.ToList();

            if (designContactList?.Any() == true && majorHardwareDesignContact?.Any() == true)
                designContactList = designContactList.Union(majorHardwareDesignContact).ToList();
            else if (designContactList?.Any() == false && majorHardwareDesignContact?.Any() == true)
                designContactList = majorHardwareDesignContact;

            return designContactList;
        }

        public List<int?> GetDesignContactFromMajorSoftwareAndHardWare(List<Designcomponents> dcEntity)
        {

            if (dcEntity?.Any() == false) return new List<int?>();

            var designContactList = dcEntity.SelectMany(t => t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts.Where(x => x.Deleted == false).Select(m => (int?)m.Designcontactid))?.
                  Distinct().ToList();

            var majorHardwareDesignContact = dcEntity.SelectMany(t => t?.Systemtype?.Systemtypesmajorhardwarebuilds?.SelectMany(m =>
            m.Majorhardware?.Majorhwbuildsdesigncontacts.Where(x => x.Deleted == false).Select(n => (int?)n.Designcontactid))?.
               Distinct().ToList())?.ToList();

            if (designContactList?.Any() == true && majorHardwareDesignContact?.Any() == true)
                designContactList = designContactList.Union(majorHardwareDesignContact).ToList();
            else if (designContactList?.Any() == false && majorHardwareDesignContact?.Any() == true)
                designContactList = majorHardwareDesignContact;

            return designContactList;
        }



        public async Task<ResultDto> GenerateAuditLogEntryForPAHardDeleteEntity(Plannedactivities deletedPaItem)
        {
            if (deletedPaItem != null)
            {
                deletedPaItem.Deleted = true;
                _repositoryWrapper.PlannedActivity.Update(deletedPaItem);
                await _repositoryWrapper.SaveAsync();
            }

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = deletedPaItem
            };
        }

        #region //DateRangeConvert
        public DateTime? ConvertDateValue(string dateValue)
        {
            DateTime? date = null;
            if (dateValue != null)
            {
                dateValue = dateValue.Split(' ')[0];
                dateValue = dateValue.Replace("-", "/");
                string[] formats = { "M/d/yyyy h:mm:ss tt", "M/d/yyyy H:mm:ss", "MM/dd/yyyy h:mm:ss tt", "MM/dd/yyyy H:mm:ss" };
                string formattedDate = "";

                if (DateTime.TryParseExact(dateValue, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {

                    formattedDate = parsedDate.ToString("d/M/yyyy");
                }
                else
                {
                    formattedDate = dateValue;
                }

                if (formattedDate != null)
                {
                    formattedDate = formattedDate.Split(' ')[0];
                    formattedDate = formattedDate.Replace("-", "/");

                    if (DateTime.TryParseExact(formattedDate, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                        || DateTime.TryParseExact(formattedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                    {
                        date = parsedDate;
                    }
                }
            }

            return date;

        }
        #endregion

        #region 
        public string GetDesignContacts<T>(IEnumerable<T> designContactEntities) where T : class
        {
            if (designContactEntities == null || !designContactEntities.Any())
                return string.Empty;

            try
            {
                var designContactEmail = designContactEntities
                    .Select(x => new FilterValueDto
                    {
                        Text = (x as dynamic).DesignContact.Email,
                        Value = (x as dynamic).DesignContactId.ToString()
                    })
                    .Distinct()
                    .ToList();

                var result = isDesignContactInOrganisation(designContactEmail);
                return result != null && result.Any() ? string.Join(", ", result.Select(x => x.Text)) : string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string GetMajorHardwareDesignContacts(IEnumerable<MajorHwBuidlsDesignContact> hwBuidlsDesignContactEntity)
        {
            string subdomain = string.Empty;

            try
            {
                if (hwBuidlsDesignContactEntity != null && hwBuidlsDesignContactEntity.Count() > 0)
                {
                    var designContactEmail = hwBuidlsDesignContactEntity.Where(x => x.Deleted == false).Select(x => x?.DesignContact?.Email).ToList().Distinct();

                    if (designContactEmail != null && designContactEmail.Count() > 0)
                    {

                        subdomain = string.Join(", ", designContactEmail);
                    }


                }

                return subdomain;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return string.Empty;// e.Messages;
            }

        }

        #endregion
        #region  Generic Report 
        public long GetLcmDeploymentStatusId(string deploymentstatus)
        {
            try
            {
                return (long)_repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.ToLower().Replace(" ", "") == deploymentstatus.ToLower().Replace(" ", "")).FirstOrDefault()?.Deploymentstatusid;

            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public long GetEnvironmentId(string environmentDescripion)
        {
            try
            {

                return (long)_repositoryWrapper.Environment.FindByCondition(x => x.Environment.ToLower().Replace(" ", "") == environmentDescripion).FirstOrDefault()?.Environmentid;

            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public IEnumerable<UserOrganisation> GetSoftwareDesignContactEmailForLCM(Majorsoftwarebuilds swEntity)
        {

            string[] VirtualisedHWTypeArray = null; // ConstantValueFilter.virtualisedHWTypeArray;  - No Need - In MHW We provide Not Mandatory for Virtualised HW Only

            try
            {
                if (swEntity != null)
                {
                    var majorSofware = swEntity?.Majorswbuildsdesigncontacts.Select(x => new UserOrganisation
                    {
                        UserId = x.Designcontactid,
                        Email = x?.Designcontact?.Email
                    }).ToList().Distinct();


                    return majorSofware.ToList();

                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IEnumerable<UserOrganisation> GetHardwareDesignContactEmailForLCM(Majorhardwarebuilds hwEntity)
        {

            string[] VirtualisedHWTypeArray = null; // ConstantValueFilter.virtualisedHWTypeArray;  - No Need - In MHW We provide Not Mandatory for Virtualised HW Only

            try
            {
                if (hwEntity != null)
                {
                    var majorHardwareEntity = hwEntity?.Majorhwbuildsdesigncontacts;


                    var majorHWBuildContanct = (VirtualisedHWTypeArray != null ? majorHardwareEntity.
                          Where(x => !VirtualisedHWTypeArray.Contains(x.Majorhardwarebuilds.Buildconstruction.Buildconstruction))
                          : majorHardwareEntity)
                             .Select(x => new UserOrganisation
                             {
                                 UserId = x.Designcontactid,
                                 Email = x.Designcontact?.Email,
                             }).ToList().Distinct();


                    return majorHWBuildContanct.ToList();

                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        #endregion

        #region  -- Get Major SW and HW Records and LCM and Generice Rpeorts
        public List<NetWorkElementAsPlannedEduSpoc> GetCalculatedAssetEduSpocEntityforReport(Dictionary<long, long> NetWorkElementId)
        {


            try
            {
                var AssetidEntity = NetWorkElementId?.Select(x => x.Key)?.Distinct().ToList();

                var AssetSubEduEntity = (NetWorkElementId?.Any() == true ? _repositoryWrapper.NetworkElementAsPlannedEduSpoc
                .FindByCondition(x => AssetidEntity.Contains(x.Networkelementasplannedid) && x.Eduspoc != null)

                : _repositoryWrapper.NetworkElementAsPlannedEduSpoc.FindByCondition(x => x.Eduspoc != null))
                  .Include(x => x.Eduspoc)
                 .ThenInclude(x => x.Subdomainresponsible)
                 .Include(x => x.Eduspoc).ThenInclude(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x =>x.Vertical);

                var returnEduEntity = AssetSubEduEntity.Where(x => x.Deleted == false)
                   .ToList().Select(y => new NetWorkElementAsPlannedEduSpoc
                   {

                       NetWorkElementAsPlannedEduSpocId = (long)y?.Ntkelementasplneduspocid,
                       NetWorkElementAsPlannedId = (long)y?.Networkelementasplannedid,
                       ContactEmail = y?.Eduspoc.Email,

                       SubdomainresponsiblesDic = new Dictionary<int, string>
                                    {
                                        { y?.Eduspoc?.Subdomainresponsibleid?? 0,
                                          y?.Eduspoc?.Subdomainresponsible?.Subdomainresponsible }
                                    },

                       VerticalDic = (NetWorkElementId?.Any(m => m.Value != 0 && m.Value != null && m.Key == y.Networkelementasplannedid) == true ?

                        (y?.Eduspoc?.AspnetuserverticalsUser.Where(x => x.Deleted == false && x?.Organisation?.Verticalid != null && 
                        y?.Eduspoc?.AspnetuseropcosUser.Any(f => f.Opcoid == (NetWorkElementId.Where(m => m.Value != 0 & m.Value != null && m.Key == y.Networkelementasplannedid)?.FirstOrDefault().Value)) == true )) :

                      // y?.Eduspoc?.Aspnetuserroles.Where(x => x.Deleted == false && x?.Verticalresponsibleid != null &&
                      //x.Opcoid == (NetWorkElementId.Where(m => m.Value != 0 & m.Value != null && m.Key == y.Networkelementasplannedid)?.FirstOrDefault().Value)) :

                       y?.Eduspoc?.AspnetuserverticalsUser.Where(x => x?.Organisation?.Verticalid != null && x.Deleted == false))

                       ?.DistinctBy(x => x?.Organisation?.Verticalid)
                       ?.ToDictionary(m => (int)m?.Organisation?.Verticalid, m => Convert.ToString(m?.Organisation?.Vertical?.Verticalresponsible)),


                       isAdminRole = y?.Eduspoc?.Aspnetuserroles.Where(x => x?.Roleid == 1).FirstOrDefault() != null ? true : false,


                   }).ToList();

                return returnEduEntity;

            }
            catch
            {
                return null;
            }

        }

        public List<LcmOperationalContracts> GetCalculateLCMOperationalContractsForReport(List<long> lcmengineeringsEntityId)
        {


            var LCMOperationalContracts = (lcmengineeringsEntityId?.Any() == true ?
                _repositoryWrapper.LCMOperationalContracts.FindByCondition(x => lcmengineeringsEntityId.Contains(x.Lcmid))
                : _repositoryWrapper.LCMOperationalContracts.FindAll()).Include(x => x.Operationalcontract)?.ToList().Where(x => x.Deleted == false)
                .Select(y => new LcmOperationalContracts
                {
                    LcmengineeringId = y.Lcmid,
                    OperationDescription = y.Operationalcontract.Description,
                }).ToList();
            return LCMOperationalContracts;

        }

        public List<HardwareConfigurations> GetHardwareConfigurations(List<string> elementName)
        {
            var hardWareConfigurations = _repositoryWrapper.HardwareConfiguration.FindByCondition(x => elementName.Contains(x.Elementname)).ToList()
                .Select(y => new HardwareConfigurations
                {
                    ElementName = y.Elementname,
                    HardwareType = y.Hardwaretype,
                    ProductName = y.Productname,
                    ProductNumber = y.Productnumber,
                    Revision = y.Revision,
                    SerialNumber = y.Serialnumber,
                    UnitLocation = y.Unitlocation,
                    Vendor = y.Vendor,
                    Modificationdate = y.Modificationdate,



                }).ToList();

            return hardWareConfigurations;
        }
        #endregion

        public string GetEngContactPointFromEduAndSubDomainSpoc(List<string> eduSpoc, List<string> subDomainSpoc)
        {
            if (eduSpoc != null && subDomainSpoc != null)
            {
                var combinedSpocs = eduSpoc.Concat(subDomainSpoc).Distinct().ToList();
                return string.Join(",", combinedSpocs ?? new List<string>());
            }
            else
            {
                return $"{string.Join(",", eduSpoc ?? new List<string>())} " +
                      $"{string.Join(",", subDomainSpoc ?? new List<string>())} "
                        ?.TrimStart();
            }
        }

        #region Filter 


        public List<FilterValueDto> isDesignContactInOrganisations(List<FilterValueDto> aspnetUserContact)
        {
            var filterValueEntity = new List<FilterValueDto>();
            var result = new FilterValueDto();

            if (aspnetUserContact?.Any() == false)
            {
                filterValueEntity = new List<FilterValueDto>
                {
                  AddBlankFilterValue()
                };

            }
            else
            {
                var orgEntity = _repositoryWrapper.OrganisationRepository.FindAll().Select(x => x.Organisationid)?.Distinct()?.ToList();
                if (orgEntity != null)
                {
                    var filter = aspnetUserContact?.Where(x => !orgEntity.Any(y => y.ToString() == x.Value)).ToList();
                    if (filter.Count() > 0)
                    {
                        result =
                           AddBlankFilterValue();
                    }
                    filterValueEntity = aspnetUserContact?.Where(x => orgEntity.Any(y => y.ToString() == x.Value)).ToList();
                    filterValueEntity.Add(result);

                }
            }
            return filterValueEntity;

        }
        public List<FilterValueDto> isDesignContactInOrganisation(List<FilterValueDto> aspnetUserContact)
        {
            var filterValueEntity = new List<FilterValueDto>();
            var result = new FilterValueDto();

            if (aspnetUserContact?.Any() == false)
            {
                filterValueEntity = new List<FilterValueDto>
                {
                  AddBlankFilterValue()
                };

            }
            else
            {
                var aspNetUserEntity = _repositoryWrapper.UserRepository.FindByCondition(x => aspnetUserContact.Select(s => Convert.ToInt32(s.Value)).Contains(x.Id)).Distinct()?.ToList();
                if (aspNetUserEntity != null && aspNetUserEntity?.Any() == true)
                {
                    var filter = aspNetUserEntity.Where(f => f.Isdesigncontact == false).ToList(); //aspnetUserContact?.Where(x => !orgEntity.Any(y => y.ToString() == x.Value)).ToList();
                    if (filter.Count() > 0)
                    {
                        result =
                           AddBlankFilterValue();
                    }
                    filterValueEntity = aspNetUserEntity?.Where(f => f.Isdesigncontact == true).Select(r => new FilterValueDto
                    {
                        Text = r.Email,
                        Value = r.Id.ToString()
                    }).ToList();
                    filterValueEntity.Add(result);

                }
            }
            return filterValueEntity;

        }
        public List<FilterValueDto> GetOrganisationReleatedFilters(List<FilterValueDto> aspnetUserContact)
        {
            var filterValueEntity = new List<FilterValueDto>();
            if (aspnetUserContact?.Any() == true)
            {
                var orgEntity = _repositoryWrapper.OrganisationRepository.FindAll().ToList();
                if (orgEntity != null)
                {

                    filterValueEntity = orgEntity.Where(x => aspnetUserContact.Any(y => y.Value == x.Organisationid.ToString())).ToList().Select(p => new FilterValueDto
                    {
                        //Text = p.Subdomainresponsible.Subdomainresponsible,
                        //Value = p.Subdomainresponsibleid.ToString()
                    }).ToList();
                }
            }
            return filterValueEntity;

        }

        public List<FilterValueDto> GetOrganisationReleatedFilter(List<FilterValueDto> aspnetUserContact)
        {
            var filterValueEntity = new List<FilterValueDto>();
            if (aspnetUserContact?.Any() == true)
            {
                var aspNetUserEntity = _repositoryWrapper.UserRepository.FindByCondition(x => aspnetUserContact.Select(s => Convert.ToInt32(s.Value)).Contains(x.Id)).Distinct()?.ToList();
                if (aspNetUserEntity != null && aspNetUserEntity?.Any() == true)
                {

                    filterValueEntity = aspNetUserEntity.Where(f => f.Isdesigncontact == true).Select(p => new FilterValueDto
                    {
                        Text = p.Subdomainresponsible.Subdomainresponsible,
                        Value = p.Subdomainresponsibleid.ToString()
                    }).ToList();
                }
            }
            return filterValueEntity;

        }


        public List<FilterValueDto> OrgOpcoFilters(List<FilterValueDto> contactId)
        {
            var filterValue = new List<FilterValueDto>();
            var result = new FilterValueDto();
            if (contactId?.Any() == false)
            {
                filterValue = new List<FilterValueDto>
                {
                    new FilterValueDto
                    {
                        Text = ConstantValueFilter.blankTextValue,
                        Value = ConstantValueFilter.blankZeroValue.ToString()
                    }
                };

            }
            else
            {
                var userEntity = _repositoryWrapper.UserRepository.FindAll().Include(x => x.AspnetuseropcosUser).ThenInclude(x => x.Opco).ToList();
                if (userEntity != null)
                {
                    var Filter = contactId.Where(x => !userEntity.Any(r => r.Id.ToString() == x.Value));
                    if (Filter != null)
                    {
                        result =
                           new FilterValueDto
                           {
                               Text = ConstantValueFilter.blankTextValue,
                               Value = ConstantValueFilter.blankZeroValue.ToString()
                           };
                    }
                    filterValue = userEntity.Where(x => contactId.Any(y => y.Value == x.Id.ToString())).SelectMany(x => x.AspnetuseropcosUser.Select(x => x.Opco).Select(p => new FilterValueDto
                    {
                        Text = p.Opco,
                        Value = p.Opcoid.ToString()
                    })).Distinct().ToList();
                    filterValue.Add(result);

                }
            }
            return filterValue;
        }
        public List<FilterValueDto> OrgVerticalFilters(List<FilterValueDto> contactId)
        {
            var filterValue = new List<FilterValueDto>();
            var result = new FilterValueDto();
            if (contactId?.Any() == false)
            {
                filterValue = new List<FilterValueDto>
                {
                    new FilterValueDto
                    {
                        Text = ConstantValueFilter.blankTextValue,
                        Value = ConstantValueFilter.blankZeroValue.ToString()
                    }
                };

            }
            else
            {
                var userEntity = _repositoryWrapper.UserRepository.FindAll().Include(x => x.Aspnetuserroles)/*.ThenInclude(x => x.Verticalresponsible)*/.ToList();
                if (userEntity != null)
                {
                    var Filter = contactId.Where(x => !userEntity.Any(r => r.Id.ToString() == x.Value));
                    if (Filter != null)
                    {
                        result =
                           new FilterValueDto
                           {
                               Text = ConstantValueFilter.blankTextValue,
                               Value = ConstantValueFilter.blankZeroValue.ToString()
                           };
                    }
                    filterValue = userEntity.Where(x => contactId.Any(y => y.Value == x.Id.ToString())).SelectMany(x => x.Aspnetuserroles.Select(x => x.Role).Select(p => new FilterValueDto
                    {
                        //Text = p.Verticalresponsible,
                        //Value = p.Verticalresponsibleid.ToString()
                    })).Distinct().ToList();
                    filterValue.Add(result);

                }
            }
            return filterValue;
        }

        public List<FilterValueDto> OrgVerticalFilterss(List<FilterValueDto> contactId)
        {
            var filterValue = new List<FilterValueDto>();
            var result = new FilterValueDto();
            if (contactId?.Any() == false)
            {
                filterValue = new List<FilterValueDto>
                {
                    new FilterValueDto
                    {
                        Text = ConstantValueFilter.blankTextValue,
                        Value = ConstantValueFilter.blankZeroValue.ToString()
                    }
                };

            }
            else
            {
                var userEntity = _repositoryWrapper.UserRepository.FindAll().AsNoTracking().Select(x => new Aspnetusers
                {

                    AspnetuserverticalsUser = x.AspnetuserverticalsUser != null && x.AspnetuserverticalsUser.Count < 0 ? new List<Aspnetuserverticals>() :
                    x.AspnetuserverticalsUser.Select(a => new Aspnetuserverticals
                    {
                        Organisation = a.Organisation != null ? new Organisation
                        {
                            Vertical = a.Organisation.Vertical != null ? new Verticalresponsibles
                            {
                                Verticalresponsibleid = a.Organisation.Vertical.Verticalresponsibleid,
                                Verticalresponsible = a.Organisation.Vertical.Verticalresponsible,
                            } : null,                            
                        }: null,
                    }).ToList()
                }).ToList();
                   
                if (userEntity != null)
                {
                    var Filter = contactId.Where(x => !userEntity.Any(r => r.Id.ToString() == x.Value));
                    if (Filter != null)
                    {
                        result =
                           new FilterValueDto
                           {
                               Text = ConstantValueFilter.blankTextValue,
                               Value = ConstantValueFilter.blankZeroValue.ToString()
                           };
                    }
                    filterValue = userEntity.Where(x => contactId.Any(y => y.Value == x.Id.ToString())).SelectMany(x => x.Aspnetuserroles.Select(x => x.Role).Select(p => new FilterValueDto
                    {
                        //Text = p.Verticalresponsible,
                        //Value = p.Verticalresponsibleid.ToString()
                    })).Distinct().ToList();
                    filterValue.Add(result);

                }
            }
            return filterValue;
        }

        public List<FilterValueDto> GenerateVerticalFilterResponse(List<int?> aspnetId, long? lcmOpcoid, bool isEduSpoc = false, bool isSubSpoc = false)
        {
            bool isEdu = isEduSpoc, isSub = isSubSpoc, isUserDetail = false, isOnlyVertical = true, isOnlyOpco = false, isSubDomainResponse = false, isAllDetia = false;

            string veticaleNameRes = string.Empty;
            List<FilterValueDto> VerticalResponseFilterValue = new List<FilterValueDto>();

            try
            {
                if (aspnetId != null && aspnetId.Count > 0)
                {
                    var getVerticaleFilterDto = FetchUserVerticalAndSubdomainInfo(aspnetId, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco, isSubDomainResponse, isAllDetia, lcmOpcoid);

                    if (getVerticaleFilterDto != null & getVerticaleFilterDto.Count > 0 && getVerticaleFilterDto?.Any(x => x.VerticalResponseFilterValue?.Any() == true) == true)
                    {
                        VerticalResponseFilterValue.AddRange(
                            getVerticaleFilterDto.SelectMany(x => x.VerticalResponseFilterValue)?.Distinct().ToList());
                    }

                }
                return VerticalResponseFilterValue.ToList() ?? new List<FilterValueDto>();
            }
            catch (Exception e)
            {
                return null;
            }
        }


        #endregion

        #region  New implementation   
        public List<int?> GetDesignContactIdFromLibaray(long majorSwId, long majorHwId, bool isMhw = false)
        {

            string[] VirtualisedHWTypeArray = null; // ConstantValueFilter.virtualisedHWTypeArray; - No Need - In MHW We provide Not Mandatory for Virtualised HW Only
                                                    // if (isMhw == true)  VirtualisedHWTypeArray = null;

            try
            {

                var majorSWBuildContanct = _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.FindByCondition(x => x.Majorsoftwarebuildsid == majorSwId && x.Deleted == false)
                   .Select(t => (int?)t.Designcontactid)?.Distinct()?.ToList();

                var majorHWBuildContanct = (VirtualisedHWTypeArray != null ? _repositoryWrapper.MajorHwBuidlsDesignContactsRepository.
                               FindByCondition(x => x.Majorhardwarebuildsid == majorHwId
                                            && !VirtualisedHWTypeArray.Contains(x.Majorhardwarebuilds.Buildconstruction.Buildconstruction) && x.Deleted == false)
                               : _repositoryWrapper.MajorHwBuidlsDesignContactsRepository.
                               FindByCondition(x => x.Majorhardwarebuildsid == majorHwId && x.Deleted == false))
                               .Select(t => (int?)t.Designcontactid)?.Distinct()?.ToList();

                if (majorHWBuildContanct?.Any() == true && majorSWBuildContanct?.Any() == true)
                    majorSWBuildContanct = majorSWBuildContanct.Union(majorHWBuildContanct).ToList();
                else if (majorHWBuildContanct?.Any() == true && majorSWBuildContanct?.Any() == false)
                    majorSWBuildContanct = majorHWBuildContanct;

                return majorSWBuildContanct;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public string GetUserVerticalForOrganisationLibary(long? id)
        {
            bool isEdu = false, isSub = false, isUserDetail = false, isOnlyVertical = true, isOnlyOpco = false, isSubDomainResponse = false, isAllDetia = false;
            List<int?> contactId = new List<int?> { (int)id };
            string verticalResponse = "";

            var usersVerticalResponsibles = FetchUserVerticalAndSubdomainInfo(contactId, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco, isSubDomainResponse, isAllDetia, 0);

            if (usersVerticalResponsibles?.Any() == true)
                verticalResponse = string.Join(",", usersVerticalResponsibles.SelectMany(t => t.VerticleResponseDic.Select(x => x.Value)).ToList()
                ?? new List<string>());

            return verticalResponse;

        }

        public string GetUserOpcosForOrganisationLibary(long? id)
        {
            bool isEdu = false, isSub = false, isUserDetail = false, isOnlyVertical = false, isOnlyOpco = true, isSubDomainResponse = false, isAllDetia = false;
            List<int?> contactId = new List<int?> { (int)id };
            string opcoDescription = "";

            var usersVerticalResponsibles = FetchUserVerticalAndSubdomainInfo(contactId, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco, isSubDomainResponse, isAllDetia, 0);

            if (usersVerticalResponsibles?.Any() == true)
                opcoDescription = string.Join(",", usersVerticalResponsibles.SelectMany(t => t.OpCosDic.Select(x => x.Value)).ToList()
                ?? new List<string>());

            return opcoDescription;

        }

        public string GetOpcoForOrganisation(long id)
        {

            var usersOpcos = _repositoryWrapper.AspNetUserOpcosRepository.FindByCondition(x => x.Userid == id && x.Opco.Opco != null && x.Deleted == false)
                            .Include(x => x.Opco)
                            /*.Include(x => x.Role)*/.ToList();

            string opCos = string.Join(",", usersOpcos.DistinctBy(x => x?.Opcoid).Select(x => x?.Opco?.Opco) ?? new List<string>());
            return opCos;
        }

        public string GetVerticalResponsiblesForOrganisation(long? id)
        {

            var usersVerticalResponsibless = _repositoryWrapper.AspNetUserVerticalsRepository.FindByCondition(x => x.Userid == id && x.Deleted == false && x.Organisation.Vertical != null)
                            .Include(x => x.Organisation).ThenInclude(x => x.Vertical)
                            /*.Include(x => x.Role)*/.ToList();
            string verticalResponse = string.Join(",", usersVerticalResponsibless.DistinctBy(x => x?.Organisation.Vertical.Verticalresponsibleid).Select(x => x?.Organisation.Vertical
               ?.Verticalresponsible) ?? new List<string>());
            return verticalResponse;

        }

        #region // old code
        //public List<UserOrganisation> FetchUserVerticalAndSubdomainInfos(List<int?> contactId, bool isEdu, bool isSub, bool isUserDetail,
        //    bool isOnlyVertical, bool isOnlyOpcos, bool isSubDomainResponse, bool isAllDetial, long? lcmOpcoid = 0, bool isPratice = false)
        //{

        //    List<UserOrganisation> designContactDetails = new List<UserOrganisation>();
        //    try
        //    {
        //        var predicateResult = PredicateBuilder.New<Organisation>(true);
        //        var predicateInner = PredicateBuilder.New<Organisation>(true);

        //        if (isEdu)
        //        {
        //            predicateInner.Or(x => x.Iseduspoc == true);
        //            predicateResult.And(predicateInner);
        //        }

        //        if (isSub)
        //        {
        //            predicateInner.Or(x => x.Issubdomainspoc == true);
        //            predicateResult.And(predicateInner);
        //        }

        //        var orgRepoEntity = _repositoryWrapper.OrganisationRepository.FindByCondition(predicateResult)
        //            .Where(x => contactId.Any(t => t == x.Contactid)).Include(x => x.Contact);

        //        if (isUserDetail)
        //        {
        //            return designContactDetails = orgRepoEntity?.Select(x => new UserOrganisation
        //            {
        //                Email = x.Contact.Email,
        //                UserId = x.Contactid,

        //            }).ToList();
        //        }
        //        else if (isOnlyVertical)
        //        {
        //            return orgRepoEntity.Include(x => x.Contact).ThenInclude(x => x.Aspnetuserroles)
        //      .ThenInclude(x => x.Verticalresponsible).ToList().Select(x => new UserOrganisation
        //      {
        //          VerticleResponseDic = GetDistinctVerticalNames(x.Contact.Aspnetuserroles, lcmOpcoid),
        //          VerticalResponseFilterValue = GetVerticalResponseFilterValues(x.Contact.Aspnetuserroles, lcmOpcoid)
        //      }).ToList();

        //        }
        //        else if (isSubDomainResponse)
        //        {
        //            return designContactDetails = orgRepoEntity
        //                .Include(x => x.Subdomainresponsible).AsEnumerable().Where(x => x.Subdomainresponsible.Deleted == false).Select(x =>
        //                new UserOrganisation
        //                {
        //                    SubDomainResponsible = x.Subdomainresponsible?.Subdomainresponsible,
        //                    subDomainResponseDic = GetDistinctSubDomainSpoc(x.Subdomainresponsible),
        //                }).ToList();
        //        }
        //        else if (isAllDetial)
        //        {
        //            return designContactDetails = orgRepoEntity
        //                .Include(x => x.Contact).ThenInclude(x => x.Aspnetuserroles).ThenInclude(x => x.Verticalresponsible)
        //                .Include(x => x.Subdomainresponsible)
        //                .Include(x => x.Mainorganisation)
        //                .Include(x => x.Contact).ThenInclude(x => x.Aspnetuserroles).ThenInclude(x => x.Opco)
        //                .Where(x => x.Contact.Aspnetuserroles.Any(z => z.Deleted == false) && x.Subdomainresponsible.Deleted == false)
        //                .ToList().Select(x =>
        //                new UserOrganisation
        //                {
        //                    Email = x.Contact?.Email,
        //                    OpCosDic = GetDistinctOpcosNames(x.Contact.Aspnetuserroles),
        //                    VerticleResponseDic = GetDistinctVerticalNames(x.Contact.Aspnetuserroles, lcmOpcoid),
        //                    SubDomainResponsible = x.Subdomainresponsible?.Subdomainresponsible,
        //                    OrganizationName = x.Mainorganisation.Mainorganisationdescription

        //                }).ToList();
        //        }
        //        else if (isOnlyOpcos)
        //        {
        //            return designContactDetails = orgRepoEntity
        //                .Include(x => x.Contact).ThenInclude(x => x.Aspnetuserroles).ThenInclude(x => x.Opco).ToList().Select(x =>
        //                new UserOrganisation
        //                {
        //                    OpCosDic = GetDistinctOpcosNames(x.Contact.Aspnetuserroles),

        //                }).ToList();
        //        }
        //        else if (isPratice)
        //        {
        //            return designContactDetails = orgRepoEntity
        //                .Include(x => x.Practice).ToList().Select(x =>
        //                new UserOrganisation
        //                {
        //                    Practicedescription = x.Practice.Practicedescription,

        //                }).ToList();
        //        }


        //        return designContactDetails;
        //    }
        //    catch (Exception e)
        //    {
        //        return designContactDetails;
        //    }
        //}

        #endregion
        public List<UserOrganisation> FetchUserVerticalAndSubdomainInfo(List<int?> contactId, bool isEdu, bool isSub, bool isUserDetail,
           bool isOnlyVertical, bool isOnlyOpcos, bool isSubDomainResponse, bool isAllDetial, long? lcmOpcoid = 0, bool isPratice = false)
        {

            List<UserOrganisation> designContactDetails = new List<UserOrganisation>();
            try
            {
                var predicateResult = PredicateBuilder.New<Aspnetusers>(true);
                var predicateInner = PredicateBuilder.New<Aspnetusers>(true);

                if (isEdu)
                {
                    predicateInner.Or(x => x.Iseduspoc == true);
                    predicateResult.And(predicateInner);
                }

                if (isSub)
                {
                    predicateInner.Or(x => x.Issubdomainspoc == true);
                    predicateResult.And(predicateInner);
                }

                var userRepoEntity = _repositoryWrapper.UserRepository.FindByCondition(predicateResult)
                    .Where(x => contactId.Any(t => t == x.Id));

                if (isUserDetail)
                {
                    return designContactDetails = userRepoEntity?.Select(x => new UserOrganisation
                    {
                        Email = x.Email,
                        UserId = x.Id,

                    }).ToList();
                }
                else if (isOnlyVertical)
                {
                    return userRepoEntity.Include(x => x.AspnetuserverticalsUser)
              .ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical).ToList().Select(x => new UserOrganisation
              {
                  VerticleResponseDic = GetDistinctVerticalNames(x.AspnetuserverticalsUser, lcmOpcoid),
                  VerticalResponseFilterValue = GetVerticalResponseFilterValues(x.AspnetuserverticalsUser, lcmOpcoid)
              }).ToList();

                }
                else if (isSubDomainResponse)
                {
                    return designContactDetails = userRepoEntity
                        .Include(x => x.Subdomainresponsible).AsEnumerable().Where(x => x.Subdomainresponsible != null && x.Subdomainresponsible.Deleted == false).Select(x =>
                        new UserOrganisation
                        {
                            SubDomainResponsible = x.Subdomainresponsible?.Subdomainresponsible,
                            subDomainResponseDic = GetDistinctSubDomainSpoc(x.Subdomainresponsible),
                        }).ToList();
                }
                else if (isAllDetial)
                {
                    return designContactDetails = userRepoEntity
                        .Include(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical)
                        .Include(x => x.Subdomainresponsible)
                        .Include(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Mainorganisation)
                        .Include(x => x.AspnetuseropcosUser).ThenInclude(x => x.Opco)
                        .Where(x => x.AspnetuserverticalsUser.Any(z => z.Organisation.Vertical.Deleted == false) && x.Subdomainresponsible.Deleted == false)
                        .ToList().Select(x =>
                        new UserOrganisation
                        {
                            Email = x?.Email,
                            OpCosDic = GetDistinctOpcosNames(x.AspnetuseropcosUser),
                            VerticleResponseDic = GetDistinctVerticalNames(x.AspnetuserverticalsUser, lcmOpcoid),
                            SubDomainResponsible = x.Subdomainresponsible?.Subdomainresponsible,
                            OrganizationName = x.AspnetuserverticalsUser?.Select(r => r.Organisation?.Mainorganisation?.Mainorganisationdescription).FirstOrDefault()

                        }).ToList();
                }
                else if (isOnlyOpcos)
                {
                    return designContactDetails = userRepoEntity
                        .Include(x => x.AspnetuseropcosUser).ThenInclude(x => x.Opco).ToList().Select(x =>
                        new UserOrganisation
                        {
                            OpCosDic = GetDistinctOpcosNames(x.AspnetuseropcosUser),

                        }).ToList();
                }
                else if (isPratice)
                {
                    return designContactDetails = userRepoEntity
                        .Include(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Practice).ToList().Select(x =>
                        new UserOrganisation
                        {
                            Practicedescription = x.AspnetuserverticalsUser.Select(x => x.Organisation?.Practice?.Practicedescription).FirstOrDefault(),

                        }).ToList();
                }


                return designContactDetails;
            }
            catch (Exception e)
            {
                return designContactDetails;
            }
        }




        // Get Distinct Opcos Names
        Dictionary<short, string> GetDistinctOpcosNames(IEnumerable<Aspnetuseropcos> userOpco)
        {
            return FilterUserOpcos(userOpco, 0)
                 .DistinctBy(role => role.Opcoid)
                 .ToDictionary(
                     role => (short)role?.Opcoid,
                     role => role?.Opco.Opco);
        }
        //Dictionary<short, string> GetDistinctOpcosNames(IEnumerable<Aspnetuserroles> userRoles)
        //{
        //    return FilterUserOpcos(userRoles, 0)
        //         .DistinctBy(role => role.Opcoid)
        //         .ToDictionary(
        //             role => (short)role?.Opcoid,
        //             role => role?.Opco.Opco);
        //}
        // Get Distinct SubDomainSpoc
        Dictionary<int, string> GetDistinctSubDomainSpoc(Subdomainresponsibles subDomainEntity)
        {
            return new Dictionary<int, string>
            {
                { subDomainEntity.Subdomainresponsibleid, subDomainEntity.Subdomainresponsible }
            };
        }
        // Get Distinct Vertical Names
        //Dictionary<int, string> GetDistinctVerticalNames(IEnumerable<Aspnetuserroles> userRoles, long? lcmOpcoid)
        //{
        //    return FilterUserRoles(userRoles, lcmOpcoid ?? 0)
        //        .DistinctBy(role => role.Userid)
        //        .ToDictionary(
        //            role => role.Userid,
        //            role => role.User.Email);
        //}
        Dictionary<int, string> GetDistinctVerticalNames(IEnumerable<Aspnetuserverticals> userVerticals, long? lcmOpcoid)
        {
            return FilterUserVerticals(userVerticals, lcmOpcoid ?? 0)
                .DistinctBy(role => role.Organisation.Vertical.Verticalresponsibleid)
                .ToDictionary(
                    role => role.Organisation.Vertical.Verticalresponsibleid,
                    role => role.Organisation.Vertical.Verticalresponsible);
        }

        //Get Filter Values
        //List<FilterValueDto> GetVerticalResponseFilterValues(IEnumerable<Aspnetuserroles> userRoles, long? lcmOpcoid)
        //{
        //    return FilterUserRoles(userRoles, lcmOpcoid ?? 0)
        //        .DistinctBy(role => role.Verticalresponsibleid)
        //        .Select(role => new FilterValueDto
        //        {
        //            Text = role.Verticalresponsible.Verticalresponsible,
        //            Value = role.Verticalresponsible.Verticalresponsibleid.ToString()
        //        })
        //        .Distinct()
        //        .ToList();
        //}
        List<FilterValueDto> GetVerticalResponseFilterValues(IEnumerable<Aspnetuserverticals> userVerticals, long? lcmOpcoid)
        {
            return FilterUserVerticals(userVerticals, lcmOpcoid ?? 0)
                .DistinctBy(role => role.Organisation.Vertical.Verticalresponsibleid)
                .Select(role => new FilterValueDto
                {
                    Text = role.Organisation.Vertical.Verticalresponsible,
                    Value = role.Organisation.Vertical.Verticalresponsibleid.ToString()
                })
                .Distinct()
                .ToList();
        }

        //   Filter User Roles
        //IEnumerable<Aspnetuserroles> FilterUserRoles(IEnumerable<Aspnetuserroles> userRoles, long lcmOpcoid = 0)
        //{
        //    return lcmOpcoid != 0
        //        ? userRoles.Where(role => /*role.Verticalresponsibleid != null && role.Opcoid != null && role.Opcoid == lcmOpcoid */ role.Deleted == false)
        //        : userRoles.Where(role => /*role.Verticalresponsibleid != null && role.Opcoid != null &&*/ role.Deleted == false);
        //}
        IEnumerable<Aspnetuserverticals> FilterUserVerticals(IEnumerable<Aspnetuserverticals> userVerticals, long lcmOpcoid = 0)
        {
            return /*lcmOpcoid != 0*/
                /*?*/ userVerticals.Where(role => role.Organisation != null && role.Organisation.Vertical != null /*&& role.Opcoid != null && role.Opcoid == lcmOpcoid*/ && role.Deleted == false)
                /*: userVerticals.Where(role => role.Verticalresponsibleid != null && role.Opcoid != null && role.Deleted == false)*/;
        }
        IEnumerable<Aspnetuseropcos> FilterUserOpcos(IEnumerable<Aspnetuseropcos> userOpcos, long lcmOpcoid = 0)
        {
            return /*lcmOpcoid != 0*/
                /*?*/ userOpcos.Where(role => role.Opco != null /*&& role.Opcoid != null && role.Opcoid == lcmOpcoid*/ && role.Deleted == false)
                /*: userVerticals.Where(role => role.Verticalresponsibleid != null && role.Opcoid != null && role.Deleted == false)*/;
        }

        public FilterValueDto AddBlankFilterValue()
        {
            return new FilterValueDto
            {
                Text = ConstantValueFilter.blankTextValue,
                Value = ConstantValueFilter.yes.ToLower()
            };
        }
        public string GetVerticalResponseForLibary(long majorSwId, long majorHwId)
        {
            string verticleResponse = string.Empty;
            bool isEdu = false, isSub = false, isUserDetail = false, isOnlyVertical = true, isOnlyOpco = false, isSubDomainResponse = false, isAllDetia = false;

            try
            {
                var contactIds = GetDesignContactIdFromLibaray(majorSwId, majorHwId);
                var usersVerticalResponsibles = FetchUserVerticalAndSubdomainInfo(contactIds, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco, isSubDomainResponse, isAllDetia, 0);

                if (usersVerticalResponsibles?.Any() == true)
                    verticleResponse = string.Join(",", usersVerticalResponsibles.SelectMany(t => t.VerticleResponseDic.Select(x => x.Value))?.Distinct()?.ToList()
                    ?? new List<string>());

                return verticleResponse;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        public string GetOpcoForLibary(long majorSwId, long majorHwId)
        {
            string OpcoList = string.Empty;
            bool isEdu = false, isSub = false, isUserDetail = false, isOnlyVertical = false, isOnlyOpco = true, isSubDomainResponse = false, isAllDetia = false;

            try
            {
                var contactIds = GetDesignContactIdFromLibaray(majorSwId, majorHwId);
                var usersOpcoResponsibles = FetchUserVerticalAndSubdomainInfo(contactIds, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco, isSubDomainResponse, isAllDetia, 0);

                if (usersOpcoResponsibles?.Any() == true)
                    OpcoList = string.Join(",", usersOpcoResponsibles.SelectMany(t => t.OpCosDic.Select(x => x.Value))?.Distinct()?.ToList()
                    ?? new List<string>());

                return OpcoList;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        public string GetOrgNamesForLibary(long majorSwId, long majorHwId, bool isDomainOnly)
        {
            string verticleResponse = string.Empty;
            bool isEdu = false, isSub = false, isUserDetail = false, isOnlyVertical = false, isOnlyOpco = false,
                isSubDomainResponse = false, isAllDetia = false;

            try
            {
                var contactIds = GetDesignContactIdFromLibaray(majorSwId, majorHwId);
                var usersOrganizationNames = FetchUserVerticalAndSubdomainInfo(contactIds, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco, isSubDomainResponse, isAllDetia, 0, isDomainOnly);

                if (usersOrganizationNames?.Any() == true && !isDomainOnly)
                    verticleResponse = string.Join(",", usersOrganizationNames.Select(t => t.OrganizationName).ToList().Distinct()
                    ?? new List<string>());
                else if (usersOrganizationNames?.Any() == true && isDomainOnly)
                    verticleResponse = string.Join(",", usersOrganizationNames.Select(t => t.Practicedescription).ToList().Distinct()
                    ?? new List<string>());

                return verticleResponse;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public List<int> GetDesignContactIdsFromOpcoAndVerticalIds(List<short> opcoIds, List<int> verticalIds)
        {

            try
            {

                var opcoIdsShort = opcoIds?.Select(x => (short?)x).ToList();
                var vertical = verticalIds?.Select(x => (int?)x).ToList();

                var designContactIds = _repositoryWrapper.UserRepository
                    .FindByCondition(x =>
                        x.AspnetuseropcosUser.Any(o => opcoIdsShort.Contains(o.Opcoid))
                        &&
                          x.AspnetuserverticalsUser.Any(v => vertical.Contains(v.Organisation.Verticalid)) && x.Isdesigncontact == true
                         )
                    .Select(x => x.Id)
                    .Distinct().ToList();

                return designContactIds;
            }
            catch (Exception e)
            {
                return new List<int>();
            }
        }

        public List<FilterValueDto> GetDesignContactBasedOnOpcoAndVerticalIds(List<short> opcoIds, List<int> verticalIds)
        {

            try
            {

                var opcoIdsShort = opcoIds?.Select(x => (short?)x).ToList();
                var vertical = verticalIds?.Select(x => (int?)x).ToList();

                var designContacts = _repositoryWrapper.UserRepository
                    .FindByCondition(x =>
                        x.AspnetuseropcosUser.Any(o => opcoIdsShort.Contains(o.Opcoid))
                        &&
                          x.AspnetuserverticalsUser.Any(v => vertical.Contains(v.Organisation.Verticalid)) && x.Isdesigncontact == true
                         )
                    .Select(x => new FilterValueDto { Value = x.Id.ToString(), Text = x.Email})
                    .Distinct().ToList();

                return designContacts;
            }
            catch (Exception e)
            {
                return new List<FilterValueDto>();
            }
        }


        public string GetBudgetOwnerForBpt(string projectOwner, Plannedactivities plannedactivities)
        {
            string budgetOwner = projectOwner;
            if (string.IsNullOrEmpty(budgetOwner))
            {
                if ((plannedactivities?.Networkelementasplanned != null && plannedactivities?.Networkelementasplanned?.Networkelementasplannededuspoc != null) &&
                    ((plannedactivities.Plannedactivityresource.Foreditasset == true) || (plannedactivities.Plannedactivityresource.Foraddasset == true)))
                {
                    var aspNetIds = plannedactivities.Networkelementasplanned.Networkelementasplannededuspoc.Select(x => x.Eduspocid).ToList();
                    budgetOwner = GetEduAndSubDomainSpocUserEmail(aspNetIds, true, false);
                }
                else if ((plannedactivities?.Lcmengineering != null && plannedactivities?.Lcmengineering?.Lcmengineeringeduspoc != null) &&
                    (plannedactivities.Plannedactivityresource.Forlcm == true))
                {
                    var aspNetIds = plannedactivities.Lcmengineering.Lcmengineeringeduspoc.Select(x => x.Eduspocid).ToList();
                    budgetOwner = GetEduAndSubDomainSpocUserEmail(aspNetIds, true, false);
                }
                else if (plannedactivities.Designaspectid != null && plannedactivities.Plannedactivityresource.Fordesignaspect == true)
                {
                    var dcEntities = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentfamilyid == plannedactivities.Designcomponentfamilyid)
                                    .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts)
                                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                                    .ToList();
                    var designContactId = GetDesignContactFromMajorSoftwareAndHardWare(dcEntities);
                    budgetOwner = string.Join(",", FetchUserVerticalAndSubdomainInfo(designContactId, false, false, true, false, false, false, false, 0).Select(x => x.Email).ToList().Distinct());
                }
                else
                {
                    return budgetOwner;
                }
            }
            return budgetOwner;
        }
        public string GetBudgetOwnerForBpt(string projectOwner, PlannedActivity plannedactivities)
        {
            string budgetOwner = projectOwner;
            try
            {
                if (string.IsNullOrEmpty(budgetOwner))
                {

                    if ((plannedactivities?.NetworkElementAsPlanned != null && plannedactivities?.NetworkElementAsPlanned?.NetworkElementAsPlannedSubDomainSpoc != null) && ((plannedactivities.PlannedActivityResource.ForEditAsset == true) || (plannedactivities.PlannedActivityResource.ForEditAsset == true)))
                    {
                        var aspNetIds = plannedactivities.NetworkElementAsPlanned.NetworkElementAsPlannedEduSpoc.Select(x => x.Eduspocid).ToList();
                        budgetOwner = GetEduAndSubDomainSpocUserEmail(aspNetIds, true, false);
                    }
                    else if ((plannedactivities?.LcmEngineering != null && plannedactivities?.LcmEngineering?.LcmEngineeringEduSpoc != null) && (plannedactivities.PlannedActivityResource.ForLcm == true))
                    {
                        var aspNetIds = plannedactivities.LcmEngineering.LcmEngineeringEduSpoc.Select(x => x.Eduspocid).ToList();
                        budgetOwner = GetEduAndSubDomainSpocUserEmail(aspNetIds, true, false);
                    }
                    else if (plannedactivities.DesignAspectId != null && plannedactivities.PlannedActivityResource.ForDesignAspect == true)
                    {
                        var dcEntities = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentfamilyid == plannedactivities.DesignComponentFamilyId)
                                        .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts)
                                        .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                                        .ToList();
                        var designContactId = GetDesignContactFromMajorSoftwareAndHardWare(dcEntities);
                        budgetOwner = string.Join(",", FetchUserVerticalAndSubdomainInfo(designContactId, false, false, true, false, false, false, false, 0).Select(x => x.Email).ToList().Distinct());
                    }
                    else
                    {
                        return budgetOwner;
                    }
                }
                return budgetOwner;
            }
            catch (Exception ex)
            {
                return budgetOwner;
            }
        }



        public string GetOrganisationNameForBpt(PlannedActivity plannedactivities, bool isDomainOnly)
        {
            string domainName = string.Empty;
            try
            {

                if ((plannedactivities?.NetworkElementAsPlanned != null && plannedactivities?.NetworkElementAsPlanned?.NetworkElementAsPlannedSubDomainSpoc != null) && ((plannedactivities.PlannedActivityResource.ForEditAsset == true) || (plannedactivities.PlannedActivityResource.ForEditAsset == true)))
                {
                    var aspNetIds = plannedactivities.NetworkElementAsPlanned.NetworkElementAsPlannedSubDomainSpoc.Select(x => x.Subdomainspocid).ToList();
                    domainName = FetchUserVerticalAndSubdomainInfo(aspNetIds, false, true, false, false, false, false, true, plannedactivities.NetworkElementAsPlanned.OpCoId).Select(x => x.OrganizationName).Distinct().FirstOrDefault();
                }
                else if (plannedactivities?.LcmEngineering != null && plannedactivities.PlannedActivityResource.ForLcm == true)
                {
                    var aspNetIds = plannedactivities.LcmEngineering.LcmEngineeringSubDomainSpoc.Select(x => x.Subdomainspocid).ToList();
                    domainName = FetchUserVerticalAndSubdomainInfo(aspNetIds, false, true, false, false, false, false, true, plannedactivities.LcmEngineering.OpCoId).Select(x => x.OrganizationName).Distinct().FirstOrDefault();
                }
                else if (plannedactivities.DesignAspectId != null && plannedactivities.PlannedActivityResource.ForDesignAspect == true)
                {
                    var systemTypeId = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == plannedactivities.DesignComponentId || x.Designcomponentfamilyid == plannedactivities.DesignComponentFamilyId).Select(x => x.Systemtypeid).FirstOrDefault();
                    var sytemTypeEntity = _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == systemTypeId).Include(x => x.Systemtypesmajorhardwarebuilds).FirstOrDefault();
                    if (sytemTypeEntity != null)
                        domainName = GetOrgNamesForLibary((long)sytemTypeEntity.Majorsoftwarebuildsid, sytemTypeEntity.Systemtypesmajorhardwarebuilds.FirstOrDefault().Majorhardwareid, isDomainOnly);

                }
                else
                {
                    return domainName;
                }
                return domainName;
            }
            catch (Exception ex)
            {
                return domainName;
            }
        }
        public string GetOrganisationNameForBpt(Plannedactivities plannedactivities, bool isDomainOnly)
        {
            string domainName = string.Empty;
            try
            {


                if ((plannedactivities.Networkelementasplanned != null && plannedactivities.Networkelementasplanned.Networkelementasplannedsubdomainspoc != null)
                    && (plannedactivities.Plannedactivityresource.Foraddasset == true) || (plannedactivities.Plannedactivityresource.Foreditasset == true))
                {
                    var aspNetIds = plannedactivities?.Networkelementasplanned?.Networkelementasplannedsubdomainspoc.Select(x => x.Subdomainspocid).ToList();
                    if (aspNetIds != null && aspNetIds.Count > 0) domainName = FetchUserVerticalAndSubdomainInfo(aspNetIds, false, true, false, false, false, false, false, plannedactivities.Networkelementasplanned.Opcoid, isDomainOnly).Select(x => x.Practicedescription).Distinct().FirstOrDefault();
                }
                else if ((plannedactivities.Lcmengineering != null && plannedactivities.Lcmengineering.Lcmengineeringsubdomainspoc != null)
                    && (plannedactivities.Plannedactivityresource.Forlcm == true))
                {
                    var aspNetIds = plannedactivities.Lcmengineering.Lcmengineeringsubdomainspoc.Select(x => x.Subdomainspocid).ToList();
                    if (aspNetIds != null && aspNetIds.Count > 0) domainName = FetchUserVerticalAndSubdomainInfo(aspNetIds, false, true, false, false, false, false, false, plannedactivities.Lcmengineering.Opcoid, isDomainOnly).Select(x => x.Practicedescription).Distinct().FirstOrDefault();
                }
                else if (plannedactivities.Plannedactivityresource.Fordesignaspect == true)
                {
                    var systemTypeId = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == plannedactivities.Designcomponentid || x.Designcomponentfamilyid == plannedactivities.Designcomponentfamilyid).Select(x => x.Systemtypeid).FirstOrDefault();
                    var sytemTypeEntity = _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == systemTypeId).Include(x => x.Systemtypesmajorhardwarebuilds).FirstOrDefault();
                    if (sytemTypeEntity != null) domainName = GetOrgNamesForLibary((long)sytemTypeEntity.Majorsoftwarebuildsid, sytemTypeEntity.Systemtypesmajorhardwarebuilds.FirstOrDefault().Majorhardwareid, isDomainOnly);

                }
                else
                {
                    return domainName;
                }
                return domainName;
            }
            catch (Exception ex)
            {
                return domainName;
            }
        }
        //public string GetPractice()
        //{

        //}
        public string GetWBSCode(string deliveryProjectName)
        {
            string wbsCode = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(deliveryProjectName))
                {
                    string[] wbsCodes = deliveryProjectName.Split("-");
                    if (wbsCodes.Length > 0)
                    {
                        wbsCode = wbsCodes[1];
                    }
                }
                return wbsCode;
            }
            catch (Exception ex)
            {
                return wbsCode;
            }
        }

        public string GetSubdomainSpocEmailForLibary(long majorSwId, long majorHwId)
        {
            string subdomainSpocEmail = string.Empty;
            bool isEdu = false, isSub = false, isUserDetail = true, isOnlyVertical = false, isOnlyOpco = false, isSubDomainResponse = false, isAllDetia = false;

            try
            {
                var contactIds = GetDesignContactIdFromLibaray(majorSwId, majorHwId);
                var usersSubdomainSpocEmail = FetchUserVerticalAndSubdomainInfo(contactIds, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco, isSubDomainResponse, isAllDetia, 0);

                if (usersSubdomainSpocEmail?.Any() == true)
                    subdomainSpocEmail = string.Join(",", usersSubdomainSpocEmail.Select(t => t.Email)?.Distinct()?.ToList()
                        ?? new List<string>());

                return subdomainSpocEmail;
            }
            catch (Exception e)
            {
                return string.Empty;
            }

        }

        public string GetSubdomainResponseForLibary(long majorSwId, long majorHwId)
        {
            string subdomainResponse = string.Empty;
            bool isEdu = false, isSub = false, isUserDetail = false, isOnlyVertical = false, isOnlyOpco = false, isSubDomainResponse = true, isAllDetia = false;

            try
            {
                var contactIds = GetDesignContactIdFromLibaray(majorSwId, majorHwId);
                var usersSubdomainSpocEmail = FetchUserVerticalAndSubdomainInfo(contactIds, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco, isSubDomainResponse, isAllDetia, 0);

                if (usersSubdomainSpocEmail?.Any() == true)
                    subdomainResponse = string.Join(",", usersSubdomainSpocEmail.SelectMany(t => t.subDomainResponseDic.Select(x => x.Value))?.Distinct()?.ToList()
                        ?? new List<string>());

                return subdomainResponse;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        public string GetEduAndSubDomainSpocUserEmail(List<int?> contactId, bool isEduSpoc = false, bool isSubSpoc = false)
        {
            // Sub and Edu spoc filter not implemented - Need below condition
            isEduSpoc = false; isSubSpoc = false;

            string edoSpocUserEmail = string.Empty;
            bool isEdu = isEduSpoc, isSub = isSubSpoc, isUserDetail = true, isOnlyVertical = false, isOnlyOpco = false, isSubDomainResponse = false, isAllDetia = false;

            try
            {
                var getEduSpocUserEmail = FetchUserVerticalAndSubdomainInfo(contactId, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco, isSubDomainResponse, isAllDetia, 0);

                if (getEduSpocUserEmail?.Any() == true)
                    edoSpocUserEmail = string.Join(",", getEduSpocUserEmail.Select(x => x.Email).ToList().Distinct() ?? new List<string>());

                return edoSpocUserEmail;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<FilterValueDto> GetDesignContactUsersFilterDto(List<int?> contactId, bool isEduSpoc = false, bool isSubSpoc = false)
        {
            // Sub and Edu spoc filter not implemented - Need below condition
            isEduSpoc = false; isSubSpoc = false;
            bool isEdu = isEduSpoc, isSub = isSubSpoc, isUserDetail = true, isOnlyVertical = false, isOnlyOpco = false, isSubDomainResponse = false, isAllDetia = false;
            List<FilterValueDto> designContactfilter = new List<FilterValueDto>();

            try
            {
                var getEduAndSubSpocUsers = FetchUserVerticalAndSubdomainInfo(contactId, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco,
                    isSubDomainResponse, isAllDetia, 0);
                if (getEduAndSubSpocUsers?.Any() == true)
                {
                    designContactfilter = getEduAndSubSpocUsers.Select(x => new FilterValueDto
                    {
                        Text = x.Email,
                        Value = x.UserId.ToString()
                    })?.Distinct()?.ToList();
                }

                return designContactfilter;

            }
            catch (Exception ex)
            {
                return new List<FilterValueDto>();
            }
        }
        public string GetSubDomainResponsibleDescription(List<int?> contactId, bool isEduSpoc = false, bool isSubSpoc = false)
        {  // Sub and Edu spoc filter not implemented - Need below condition
            isEduSpoc = false; isSubSpoc = false;
            string subDomainResponsibleDescription = string.Empty;
            try
            {
                var getSubDomainResponsible = GetSubDomainResponsibleDic(contactId, isEduSpoc, isSubSpoc);

                if (getSubDomainResponsible?.Any() == true)
                    subDomainResponsibleDescription = string.Join(",", getSubDomainResponsible.Select(t => t.Value)?.Distinct()?.ToList() ?? new List<string>());

                return subDomainResponsibleDescription;


            }
            catch (Exception ex)
            {
                return subDomainResponsibleDescription;
            }
        }
        public Dictionary<int, string> GetSubDomainResponsibleDic(List<int?> contactId, bool isEduSpoc = false, bool isSubSpoc = false)
        {  // Sub and Edu spoc filter not implemented - Need below condition
            isEduSpoc = false; isSubSpoc = false;
            Dictionary<int, string> subDomainResponsibleDic = new Dictionary<int, string>();
            bool isEdu = isEduSpoc, isSub = isSubSpoc, isUserDetail = false, isOnlyVertical = false, isOnlyOpco = false, isSubDomainResponse = true, isAllDetia = false;

            try
            {
                var getSubDomainResponsible = FetchUserVerticalAndSubdomainInfo(contactId, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco,
                     isSubDomainResponse, isAllDetia, 0);
                if (getSubDomainResponsible?.Any() == true)
                    subDomainResponsibleDic = getSubDomainResponsible.SelectMany(t => t.subDomainResponseDic).ToDictionary(y => y.Key, y => y.Value);

                return subDomainResponsibleDic;


            }
            catch (Exception ex)
            {
                return new Dictionary<int, string>();
            }
        }
        public string GetVerticaleNameRes(List<int?> aspnetId, long? lcmOpcoid, bool isEduSpoc = false, bool isSubSpoc = false)
        {
            // Sub and Edu spoc filter not implemented - Need below condition
            isEduSpoc = false; isSubSpoc = false;
            string verticalName = string.Empty;
            try
            {
                var getVerticalName = GetVerticaleNameDynamicFormat(aspnetId, lcmOpcoid, isEduSpoc, isSubSpoc);

                if (getVerticalName?.Any() == true)
                    verticalName = string.Join(",", getVerticalName.Select(x => x.Text).ToList().Distinct() ?? new List<string>());

                return verticalName;
            }
            catch (Exception ex)
            {
                return verticalName;
            }
        }

        public IEnumerable<FilterValueDto> GetVerticaleFilterDto(List<int?> aspnetId, long? lcmOpcoid, bool isEduSpoc = false, bool isSubSpoc = false)
        {
            // Sub and Edu spoc filter not implemented - Need below condition
            isEduSpoc = false; isSubSpoc = false;
            var getVerticalName = GetVerticaleNameDynamicFormat(aspnetId, lcmOpcoid, isEduSpoc, isSubSpoc);

            if (getVerticalName?.Any() == true)
            {
                return getVerticalName;
            }

            return new List<FilterValueDto>();
        }

        public IEnumerable<FilterValueDto> GetVerticaleNameDynamicFormat(List<int?> aspnetId, long? lcmOpcoid, bool isEduSpoc = false, bool isSubSpoc = false)
        {
            // Sub and Edu spoc filter not implemented - Need below condition
            isEduSpoc = false; isSubSpoc = false;
            bool isEdu = isEduSpoc, isSub = isSubSpoc, isUserDetail = false, isOnlyVertical = true, isOnlyOpco = false, isSubDomainResponse = false, isAllDetia = false;

            IEnumerable<FilterValueDto> VerticalFilterValueDto = new List<FilterValueDto>();
            try
            {
                if (aspnetId?.Any() == true)
                {
                    var getSpocVertical = FetchUserVerticalAndSubdomainInfo(aspnetId, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco,
                     isSubDomainResponse, isAllDetia, lcmOpcoid);
                    if (getSpocVertical?.Any() == true)
                    {
                        VerticalFilterValueDto = getSpocVertical.SelectMany(x => x.VerticalResponseFilterValue).Select(m => new FilterValueDto
                        {
                            Text = m.Text,
                            Value = m.Value,
                        }).Distinct().ToList();
                    }
                }
                return VerticalFilterValueDto;


            }
            catch (Exception e)
            {
                return null;
            }
        }


        public string GetDesignContactForSoftware(long mswId)
        {
            //List<int?> contactId = new List<int?> { (int)aspnetId };
            bool isEdu = false, isSub = false, isUserDetail = true, isOnlyVertical = false, isOnlyOpco = false, isSubDomainResponse = false, isAllDetia = false;

            var contactId = GetDesignContactIdFromLibaray(mswId, 0, false);
            var designContacts = FetchUserVerticalAndSubdomainInfo(contactId, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco, isSubDomainResponse,
                   isAllDetia, 0);

            if (designContacts != null)
                return string.Join(", ", designContacts?.Select(x => x.Email) ?? new List<string>());

            else
                return string.Empty;

        }
        public string GetEduAndSubDomainSpocUsersOpcos(List<int?> aspnetId, bool isEduSpoc = false, bool isSubSpoc = false)
        {
            // Sub and Edu spoc filter not implemented - Need below condition
            isEduSpoc = false; isSubSpoc = false;
            string Opcos = string.Empty;
            bool isEdu = isEduSpoc, isSub = isSubSpoc, isUserDetail = false, isOnlyVertical = false, isOnlyOpco = true, isSubDomainResponse = false, isAllDetia = false;
            try
            {
                if (aspnetId != null && aspnetId.Count > 0)
                {
                    var eduAndSubDomainSpocUsersOpcos = FetchUserVerticalAndSubdomainInfo(aspnetId, isEdu, isSub, isUserDetail, isOnlyVertical, isOnlyOpco, isSubDomainResponse,
                   isAllDetia, 0);
                    if (eduAndSubDomainSpocUsersOpcos != null & eduAndSubDomainSpocUsersOpcos.Count > 0)
                        Opcos = string.Join(",", eduAndSubDomainSpocUsersOpcos.SelectMany(x => x.OpCosDic.Values).ToList().Distinct() ?? new List<string>());

                }
                return Opcos;
            }
            catch (Exception e)
            {
                return Opcos;
            }
        }

        #region // this is code not refered in anywhere
        //public IEnumerable<UserOrganisation> GetLibaryBasedDesignContactInformation1111(Majorsoftwarebuilds swEntity, bool isOpco, bool isVertical, bool isSubDomain, bool isSoftware, bool isHardware, Majorhardwarebuilds hwEntity)
        //{

        //    string[] VirtualisedHWTypeArray = null;//  ConstantValueFilter.virtualisedHWTypeArray;  - No Need - In MHW We provide Not Mandatory for Virtualised HW Only
        //    string verticleRes = string.Empty;

        //    try
        //    {

        //        var majorSofware = isSoftware == true && swEntity != null ? swEntity.Majorswbuildsdesigncontacts.Select(x => new UserOrganisation
        //        {
        //            UserId = x.Designcontactid,
        //            Email = x?.Designcontact?.Email,
        //            OpCosFilterValue = isOpco ?
        //                           (x.Designcontact?.Aspnetuserroles.Where(x => x.Opcoid != null && x.Deleted == false).DistinctBy(x => x.Opcoid)
        //                          .Select(x => new FilterValueDto
        //                          {
        //                              Value = x.Opcoid.ToString(),
        //                              Text = x?.Opco?.Opco
        //                          })) : null,
        //            VerticalResponseFilterValue =
        //                          (x.Designcontact?.Aspnetuserroles.Where(x => x.Verticalresponsibleid != null && x.Deleted == false).DistinctBy(x => x.Verticalresponsibleid)
        //                          .Select(x => new FilterValueDto
        //                          {
        //                              Value = x.Verticalresponsibleid.ToString(),
        //                              Text = x?.Verticalresponsible?.Verticalresponsible
        //                          })),
        //            SubDomainResponseFilterValue = isSubDomain ? x?.Designcontact?.OrganisationContact.Select(x => new FilterValueDto
        //            {
        //                Value = x?.Subdomainresponsibleid.ToString(),
        //                Text = x?.Subdomainresponsible?.Subdomainresponsible
        //            }).ToList() : null
        //        }).ToList().Distinct() : null;

        //        var majorHWBuildContanct = isHardware == true && hwEntity != null ? (VirtualisedHWTypeArray != null ? hwEntity.Majorhwbuildsdesigncontacts.
        //              Where(x => !VirtualisedHWTypeArray.Contains(x.Majorhardwarebuilds.Buildconstruction.Buildconstruction))
        //              : hwEntity.Majorhwbuildsdesigncontacts)
        //                 .Select(x => new UserOrganisation
        //                 {
        //                     UserId = x.Designcontactid,
        //                     Email = x.Designcontact?.Email,
        //                     OpCosFilterValue = isOpco ? (x.Designcontact?.Aspnetuserroles.Where(x => x.Opcoid != null && x.Deleted == false).DistinctBy(x => x.Opcoid)
        //                          .Select(x => new FilterValueDto
        //                          {
        //                              Value = x.Opcoid.ToString(),
        //                              Text = x?.Opco?.Opco
        //                          })) : null,
        //                     VerticalResponseFilterValue = isVertical ?
        //                          (x.Designcontact?.Aspnetuserroles.Where(x => x.Verticalresponsibleid != null && x.Deleted == false).DistinctBy(x => x.Verticalresponsibleid)
        //                          .Select(x => new FilterValueDto
        //                          {
        //                              Value = x.Verticalresponsibleid.ToString(),
        //                              Text = x?.Verticalresponsible?.Verticalresponsible
        //                          })) : null,
        //                     SubDomainResponseFilterValue = isSubDomain ? x?.Designcontact?.OrganisationContact.Select(x => new FilterValueDto
        //                     {
        //                         Value = x?.Subdomainresponsibleid.ToString(),
        //                         Text = x?.Subdomainresponsible?.Subdomainresponsible
        //                     }).ToList() : null
        //                 }).ToList().Distinct() : null;

        //        if (majorSofware != null && majorHWBuildContanct != null)
        //            return majorSofware.Union(majorHWBuildContanct).ToList();
        //        else if (majorSofware != null && majorHWBuildContanct == null)
        //            return majorSofware.ToList();
        //        else if (majorSofware == null && majorHWBuildContanct != null)
        //            return majorHWBuildContanct.ToList();
        //        else return null;

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}
        #endregion

        public List<UserOrganisation> GetUserOrganisationsForLibaries(bool isSoftware, bool isHardware, bool isOpco, bool isVertical, bool isSubDomain,
        Majorsoftwarebuilds swEntity, Majorhardwarebuilds hwEntity)
        {
            string[] virtualisedHWTypeArray = null;   //ConstantValueFilter.virtualisedHWTypeArray;  - No Need - In MHW We provide Not Mandatory for Virtualised HW Only

            // Extract reusable logic into helper methods
            static IEnumerable<FilterValueDto> GetOpCosFilterValue(Aspnetusers designContact, bool isOpco) =>
                isOpco
                    ? designContact?.AspnetuseropcosUser
                        .Where(role => role.Opcoid != null && role.Deleted == false)
                        .DistinctBy(role => role.Opcoid)
                        .Select(role => new FilterValueDto
                        {
                            Value = role.Opcoid.ToString(),
                            Text = role?.Opco?.Opco
                        })
                    : null;

            static IEnumerable<FilterValueDto> GetVerticalResponseFilterValue(Aspnetusers designContact, bool isVertical) =>
                isVertical
                    ? designContact?.AspnetuserverticalsUser
                        .Where(role => role.Organisation != null && role.Organisation.Vertical != null && role.Deleted == false)
                        .DistinctBy(role => role.Organisation.Vertical.Verticalresponsibleid)
                        .Select(role => new FilterValueDto
                        {
                            Value = role.Organisation.Vertical.Verticalresponsibleid.ToString(),
                            Text = role?.Organisation.Vertical?.Verticalresponsible
                        })
                    : null;

            //static List<FilterValueDto> GetSubDomainResponseFilterValues(Aspnetusers designContact, bool isSubDomain) =>
            //    isSubDomain
            //        ? designContact?.OrganisationContact.Select(contact => new FilterValueDto
            //        {
            //            Value = contact?.Subdomainresponsibleid.ToString(),
            //            Text = contact?.Subdomainresponsible?.Subdomainresponsible
            //        }).ToList()
            //        : null;

            static FilterValueDto GetSubDomainResponseFilterValue(Aspnetusers designContact, bool isSubDomain) =>
               isSubDomain
                   ? new FilterValueDto
                   {
                       Value = designContact?.Subdomainresponsibleid.ToString(),
                       Text = designContact?.Subdomainresponsible?.Subdomainresponsible
                   }
                   : null;

            // Process Software Data
            var majorSoftware = isSoftware && swEntity != null
                ? swEntity.Majorswbuildsdesigncontacts.Where(x => x.Deleted == false).Select(contact => new UserOrganisation
                {
                    UserId = contact.Designcontactid,
                    Email = contact.Designcontact?.Email,
                    OpCosFilterValue = GetOpCosFilterValue(contact.Designcontact, isOpco),
                    VerticalResponseFilterValue = GetVerticalResponseFilterValue(contact.Designcontact, isVertical),
                    SubDomainResponseFilterValues = GetSubDomainResponseFilterValue(contact.Designcontact, isSubDomain)
                }).ToList().Distinct()
                : null;

            // Process Hardware Data
            var majorHWBuildContact = isHardware && hwEntity != null
                ? (virtualisedHWTypeArray != null
                    ? hwEntity.Majorhwbuildsdesigncontacts.Where(contact => !virtualisedHWTypeArray.Contains(contact.Majorhardwarebuilds.Buildconstruction.Buildconstruction))
                    : hwEntity.Majorhwbuildsdesigncontacts)
                  .Select(contact => new UserOrganisation
                  {
                      UserId = contact.Designcontactid,
                      Email = contact.Designcontact?.Email,
                      OpCosFilterValue = GetOpCosFilterValue(contact.Designcontact, isOpco),
                      VerticalResponseFilterValue = GetVerticalResponseFilterValue(contact.Designcontact, isVertical),
                      SubDomainResponseFilterValues = GetSubDomainResponseFilterValue(contact.Designcontact, isSubDomain)
                  }).ToList().Distinct()
                : null;

            // Merge and return results
            return majorSoftware != null && majorHWBuildContact != null
                ? majorSoftware.Union(majorHWBuildContact).ToList()
                : majorSoftware?.ToList() ?? majorHWBuildContact?.ToList();
        }

        public IEnumerable<FilterValueDto> GetVerticalResponsibleFromLibaryDesignContacts(Majorsoftwarebuilds swEntity, bool isSoftware, bool isHardware, Majorhardwarebuilds hwEntity)
        {

            FilterValueDto concatedVerticalFilterDto = new FilterValueDto();
            try
            {
                if (swEntity == null && isSoftware == true || swEntity?.Majorswbuildsdesigncontacts.Count() == 0) return null;
                else if (hwEntity == null && isHardware == true || hwEntity?.Majorhwbuildsdesigncontacts.Count() == 0) return null;

                var verticalEntity = GetUserOrganisationsForLibaries(isSoftware, isHardware, false, true, false, swEntity, hwEntity)
                    .Where(x => x.VerticalResponseFilterValue != null).SelectMany(y => y.VerticalResponseFilterValue).DistinctBy(x => x?.Value);

                return verticalEntity;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public IEnumerable<FilterValueDto> GetSubDomainResponsibleFromLibaryDesignContacts(Majorsoftwarebuilds swEntity, bool isSoftware, bool isHardware, Majorhardwarebuilds hwEntity)
        {

            FilterValueDto concatedVerticalFilterDto = new FilterValueDto();
            try
            {
                if (swEntity == null && isSoftware == true) return null;
                else if (hwEntity == null && isHardware == true) return null;

                var designContactId = GetUserOrganisationsForLibaries(isSoftware, isHardware, false, false, true, swEntity, hwEntity)
                       .Where(x => x.SubDomainResponseFilterValue != null).SelectMany(y => y.SubDomainResponseFilterValue).DistinctBy(x => x?.Value);

                return designContactId;


            }
            catch (Exception e)
            {
                return null;
            }
        }
        public IEnumerable<FilterValueDto> GetSystemTypeDesignContactFromLibaryDesignContacts(Majorsoftwarebuilds swEntity, bool isSoftware, bool isHardware, Majorhardwarebuilds hwEntity)
        {

            FilterValueDto concatedVerticalFilterDto = new FilterValueDto();
            try
            {
                if (swEntity == null && isSoftware == true) return null;
                else if (hwEntity == null && isHardware == true) return null;
                var contactEntity = GetUserOrganisationsForLibaries(isSoftware, isHardware, false, false, false, swEntity, hwEntity)
                    .Select(y => new FilterValueDto
                    {
                        Text = y.Email,
                        Value = y.UserId.ToString()
                    }).DistinctBy(x => x?.Value);

                return contactEntity;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<LcmEngineeringEduSpoc> GetCalculatedLcmEduSpocEntityForReport(Dictionary<long, long> lcmEngineeringsEntityId)
        {
            var lcmIds = lcmEngineeringsEntityId?.Keys?.Distinct().ToList();

            var lcmEduEntities = (lcmEngineeringsEntityId?.Any() == true
                ? _repositoryWrapper.LcmEngineeringEduSpoc.FindByCondition(x => lcmIds.Contains(x.Lcmengineeringid))
                : _repositoryWrapper.LcmEngineeringEduSpoc.FindAll())
                .Include(x => x.Eduspoc)
                    .ThenInclude(x => x.Subdomainresponsible);


            return lcmEduEntities.Where(x => x.Deleted == false).ToList().Select(y => CreateEduSpocEntity(y, lcmEngineeringsEntityId)).ToList();
        }

        private LcmEngineeringEduSpoc CreateEduSpocEntity(Lcmengineeringeduspoc entity, Dictionary<long, long> lcmEngineeringsEntityId)
        {
            var eduSpoc = entity.Eduspoc;
            var lcmId = entity.Lcmengineeringid;

            return new LcmEngineeringEduSpoc
            {
                Lcmengineeringeduspocid = entity.Lcmengineeringeduspocid,
                Lcmengineeringid = lcmId,
                Eduspocid = eduSpoc?.Id,
                ContactEmail = eduSpoc?.Email,
                SubdomainresponsiblesDic = new[] { eduSpoc}.Where(x => x.Subdomainresponsible != null).ToDictionary(x => x.Subdomainresponsible.Subdomainresponsibleid, y => y.Subdomainresponsible.Subdomainresponsible),
               
                

                VerticalDic = GetDistinctVerticalNames(eduSpoc?.AspnetuserverticalsUser, lcmEngineeringsEntityId.Where(m => m.Value != 0
                && m.Key == lcmId)?.FirstOrDefault().Value),

                isAdminRole = eduSpoc?.Aspnetuserroles?.Any(x => x.Roleid == 1) == true
            };
        }

        public List<LcmEngineeringSubDpomainSpoc> GetCalculatedLcmSubDomainSpocEntityForReport(Dictionary<long, long> lcmEngineeringsEntityId)
        {
            try
            {
                var lcmIds = lcmEngineeringsEntityId?.Keys?.Distinct().ToList();

                var lcmSubDomainEntities = (lcmEngineeringsEntityId?.Any() == true
                    ? _repositoryWrapper.LcmEngineeringSubDomainSpoc.FindByCondition(x => lcmIds.Contains(x.Lcmengineeringid))
                    : _repositoryWrapper.LcmEngineeringSubDomainSpoc.FindAll())
                    .Include(x => x.Subdomainspoc)
                        .ThenInclude(x => x.Subdomainresponsible)
                    .Include(x => x.Subdomainspoc)
                        .ThenInclude(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation)   
                        .ThenInclude(x => x.Vertical);

                return lcmSubDomainEntities.Where(x => x.Deleted == false).ToList().Select(y => CreateSubDomainSpocEntity(y, lcmEngineeringsEntityId)).ToList();
            }
            catch
            {
                return null;
            }
        }

        private LcmEngineeringSubDpomainSpoc CreateSubDomainSpocEntity(Lcmengineeringsubdomainspoc entity, Dictionary<long, long> lcmEngineeringsEntityId)
        {
            var subDomainSpoc = entity.Subdomainspoc;
            var lcmId = entity.Lcmengineeringid;

            return new LcmEngineeringSubDpomainSpoc
            {
                Lcmengineeringsubdomainspocid = entity.Lcmengineeringsubdomainspocid,
                Lcmengineeringid = lcmId,
                ContactEmail = subDomainSpoc?.Email,
                SubdomainresponsiblesDic = new[] { subDomainSpoc }
                    ?.Where(x => x.Subdomainresponsible != null)?.DistinctBy(m => m?.Subdomainresponsibleid)
                    ?.ToDictionary(m => m.Subdomainresponsible.Subdomainresponsibleid, m => m?.Subdomainresponsible?.Subdomainresponsible),

                VerticalDic = GetDistinctVerticalNames(subDomainSpoc?.AspnetuserverticalsUser, lcmEngineeringsEntityId.Where(m => m.Value != 0
                && m.Key == lcmId)?.FirstOrDefault().Value),

                isAdminRole = subDomainSpoc?.Aspnetuserroles?.Any(x => x.Roleid == 1) == true
            };
        }

        public List<LcmEngineeringEduSpoc> GetGenericReportCalculatedLcmEduSpocEntityforReport(List<long> lcmengineeringsEntityId)
        {

            var lcmidEntity = lcmengineeringsEntityId?.Distinct().ToList();

            var lcmEduEntity = (lcmengineeringsEntityId?.Any() == true ? _repositoryWrapper.LcmEngineeringEduSpoc
                .FindByCondition(x => lcmidEntity.Contains(x.Lcmengineeringid))
                : _repositoryWrapper.LcmEngineeringEduSpoc.FindAll())
                .Include(x => x.Eduspoc)
                .ThenInclude(x => x.Subdomainresponsible)
                 .Include(x => x.Eduspoc).ThenInclude(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical);
            var returnEduEntity = lcmEduEntity
               .ToList().Select(y => new LcmEngineeringEduSpoc
               {

                   Lcmengineeringeduspocid = (long)y?.Lcmengineeringeduspocid,
                   Lcmengineeringid = (long)y?.Lcmengineeringid,
                   Eduspocid = y?.Eduspocid,
                   ContactEmail = y?.Eduspoc.Email,
                   SubdomainresponsiblesDic = y.Eduspoc?.Subdomainresponsible!=null ?new[] { y.Eduspoc }.DistinctBy(m => m?.Subdomainresponsibleid)
                   .ToDictionary(m => m.Subdomainresponsible.Subdomainresponsibleid, m => Convert.ToString(m?.Subdomainresponsible.Subdomainresponsible)):null,

                   VerticalDic = y?.Eduspoc?.AspnetuserverticalsUser.Where(x => x?.Organisation != null && x?.Organisation?.Vertical != null)
                   ?.DistinctBy(x => x?.Organisation.Vertical.Verticalresponsibleid)
                   ?.ToDictionary(m => (int)m.Organisation.Vertical.Verticalresponsibleid, m => Convert.ToString(m?.Organisation.Vertical.Verticalresponsible)),


                   isAdminRole = y?.Eduspoc?.Aspnetuserroles.Where(x => x?.Roleid == 1).FirstOrDefault() != null ? true : false,


               }).ToList();

            return returnEduEntity;
        }
        public List<NetWorkElementAsPlannedSubDpomainSpoc> GetCalculatedAssetSubDomainSpocEntityforReport(Dictionary<long, long> NetWorkElementId)
        {


            try
            {
                var AssetidEntity = NetWorkElementId?.Select(x => x.Key)?.Distinct().ToList();

                var AssetSubEduEntity = (NetWorkElementId?.Any() == true ? _repositoryWrapper.NetworkElementAsPlannedSubDomainSpoc
                .FindByCondition(x => AssetidEntity.Contains(x.Networkelementasplannedid) && x.Subdomainspoc != null)
                : _repositoryWrapper.NetworkElementAsPlannedSubDomainSpoc.FindByCondition(x => x.Subdomainspoc != null))
                .Include(x => x.Subdomainspoc)
                .ThenInclude(x => x.Subdomainresponsible)
                 .Include(x => x.Subdomainspoc).ThenInclude(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical);
                var returnEduEntity = AssetSubEduEntity.Where(x => x.Deleted == false)
                   .ToList().Select(y => new NetWorkElementAsPlannedSubDpomainSpoc
                   {

                       NetWorkElementAsPlannedsubdomainspocid = (long)y?.Ntkelementasplnsubdomainspocid,
                       NetWorkElementAsPlannedId = (long)y?.Networkelementasplannedid,
                       ContactEmail = y?.Subdomainspoc.Email,

                       SubdomainresponsiblesDic = new[] { y?.Subdomainspoc }?.DistinctBy(m => m?.Subdomainresponsibleid).Where(f => f.Subdomainresponsible != null)
                       .ToDictionary(m => m.Subdomainresponsible.Subdomainresponsibleid, m => m?.Subdomainresponsible.Subdomainresponsible),

                       VerticalDic = (NetWorkElementId?.Any(m => m.Value != 0 && m.Value != null && m.Key == y.Networkelementasplannedid) == true ?

                      // y?.Subdomainspoc?.AspnetuserverticalsUser.Where(x => x?.Organisation != null && x?.Organisation.Vertical != null &&
                      //y?.Subdomainspoc?.AspnetuseropcosUser.Any(s => s.Opcoid == (NetWorkElementId.Where(m => m.Value != 0 & m.Value != null && m.Key == y.Networkelementasplannedid)?.FirstOrDefault().Value))) == true :
                      (y?.Subdomainspoc?.AspnetuserverticalsUser.Where(x => x.Deleted == false && x?.Organisation?.Verticalid != null)) :

                       y?.Subdomainspoc?.AspnetuserverticalsUser.Where(x => x?.Organisation?.Vertical?.Verticalresponsibleid != null && x.Deleted == false))


                       ?.DistinctBy(x => x?.Organisation.Vertical.Verticalresponsibleid)
                       ?.ToDictionary(m => (int)m.Organisation.Vertical.Verticalresponsibleid, m => m?.Organisation.Vertical.Verticalresponsible),


                       isAdminRole = y?.Subdomainspoc?.Aspnetuserroles.Where(x => x?.Roleid == 1).FirstOrDefault() != null ? true : false,


                   }).ToList();

                return returnEduEntity;

            }
            catch
            {
                return null;
            }

        }
        #endregion

        public string GetRiskValue(string risk)
        {

            var riskEvaluationArr = risk?.ToLower().Replace(" ", "").Split("-");

            if (riskEvaluationArr != null && riskEvaluationArr.Count() > 0)
            {
                if (riskEvaluationArr[0] == ConstantValueFilter.High)
                {
                    risk = ConstantValueFilter.High;
                }
                else if (riskEvaluationArr[0] == ConstantValueFilter.Moderate)
                {
                    risk = ConstantValueFilter.Moderate;
                }
                else if (riskEvaluationArr[0] == ConstantValueFilter.Low)
                {
                    risk = ConstantValueFilter.Low;
                }
                else if (riskEvaluationArr[0] == ConstantValueFilter.Extreme)
                {
                    risk = ConstantValueFilter.Extreme;
                }
            }

            return risk;
        }

        public string GetAssetType(string build)
        {
            if (build.ToLower().Replace(" ", "") == ConstantValueFilter.BluprintNfvi.ToLower() || build.ToLower().Replace(" ", "") == ConstantValueFilter.OtherNfvi.ToLower())
            {
                return ConstantValueFilter.Virtual;
            }
            else if (build.ToLower().Replace(" ", "") == ConstantValueFilter.BluprintNfci.ToLower() || build.ToLower().Replace(" ", "") == ConstantValueFilter.OtherNfci.ToLower())
            {
                return ConstantValueFilter.Container;
            }
            else
            {
                return build;
            }
        }

        public string GetHwProfile(string elementName)
        {
            try
            {
                var hwConfiguration = _repositoryWrapper.HardwareConfiguration.FindByCondition(x => x.Elementname == elementName).ToList();

                if (hwConfiguration != null && hwConfiguration.Count > 0)
                {
                    var hwGroupbyPn = hwConfiguration.GroupBy(x => string.IsNullOrEmpty(x.Productname) ? "Unknown" : x.Productname).ToDictionary(x => x.Key, x => x.Count());
                    var hwProfileJson = JsonConvert.SerializeObject(hwGroupbyPn, Formatting.Indented);
                    return hwProfileJson;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public bool CheckImplementationFlag(long designComponentFamilyId)
        {
            // Get production environment ID
            var assetEnvironmentId = _repositoryWrapper.Environment
                .FindByCondition(p => p.Environment.ToLower() == ConstantValueFilter.production)
                .Select(p => p.Environmentid)
                .FirstOrDefault();

            // Fetch Design Component Family
            var designComponentIds = _repositoryWrapper.DesignComponent
                .FindByCondition(p => p.Designcomponentfamilyid == designComponentFamilyId)
                .Select(p => p.Designcomponentid)
                .ToList();

            if (!designComponentIds.Any())
                return false;

            // Fetch related LCM entities
            var lcmEntities = _repositoryWrapper.Lcmengineering
                .FindByCondition(p => designComponentIds.Contains(p.Designcomponentid))
                .ToList();

            if (!lcmEntities.Any())
                return false;

            // Check conditions for implementation flag
            bool isImplemented = lcmEntities.Any(x => x.Numberofnodes > 0) ||
                                  //lcmEntities.Any(x => !x.Elementcount) &&
                                  _repositoryWrapper.NetworkElementAsPlanned
                                    .FindByCondition(p => designComponentIds.Contains(p.Designcomponentid) && p.Environmentid == assetEnvironmentId)
                                    .Any();

            return isImplemented;
        }
        #region  //Ticket 1292 - Insert/Update/Delete actions in the Asset screen are not updating the LCM Number of Nodes Count and DCF Implementation Value.

        public async Task<ResultDto> SetImplementationFlagInDCF(long designComponentId)
        {
            if (designComponentId <= 0)
                return new ResultDto() { Info = ResultMessages.EntryUpdateNotExists };

            try
            {
                var dcfId = _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentid == designComponentId)
                    .Select(x => x.Designcomponentfamilyid)
                    .FirstOrDefault();

                if (dcfId == 0)
                    return new ResultDto() { Info = ResultMessages.EntryUpdateNotExists };

                var implementation = CheckImplementationFlag((long)dcfId);

                var dcfEntity = _repositoryWrapper.DesignComponentFamily
                    .FindByCondition(x => x.Designcomponentfamilyid == dcfId)
                    .FirstOrDefault();

                if (dcfEntity != null)
                {
                    dcfEntity.Implementation = implementation;
                    _repositoryWrapper.DesignComponentFamily.Update(dcfEntity);
                    await _repositoryWrapper.SaveAsync();
                }

                return new ResultDto() { Info = ResultMessages.EntryUpdateSuccess };
            }
            catch (Exception)
            {
                return new ResultDto() { Info = ResultMessages.EntryNotUpdate };
            }
        }
        public async Task<ResultDto> UpdateLcmNodeCountsField(long lcmId)
        {
            if (lcmId == 0)
                return new ResultDto() { Info = ResultMessages.EntryUpdateNotExists };

            try
            {
                var existingLcmRecord = _repositoryWrapper.Lcmengineering
                    .FindByCondition(x => x.Lcmengineeringid == lcmId)
                    .FirstOrDefault();

                if (existingLcmRecord != null)
                {
                    //var objLcmEngineer = new Lcmengineering();
                    existingLcmRecord.Numberofnodes = existingLcmRecord.CountNetworkElementReleated(false, _repositoryWrapper);
                    existingLcmRecord.Numberofnodesinlab = existingLcmRecord.CountNetworkElementReleated(true, _repositoryWrapper);

                    _repositoryWrapper.Lcmengineering.Update(existingLcmRecord);
                    await _repositoryWrapper.SaveAsync();
                }

                return new ResultDto() { Info = ResultMessages.EntryUpdateSuccess };
            }
            catch (Exception)
            {
                return new ResultDto() { Info = ResultMessages.EntryNotUpdate };
            }
        }
        #endregion

        #region // Bag details
        public string GetBuildBagDescription(Buildbags bagEntity) =>
    $"{bagEntity?.Bagdescription}-{bagEntity?.Bagversion}" ?? string.Empty;

        public string GetBuildBagDescriptionFromEnity(BuildBag bagEntity) =>
            $"{bagEntity?.BagDescription}-{bagEntity?.BagVersion}" ?? string.Empty;

        public string GetBuildBagDescription(long buildBagId)
        {
            var buildBagName = _repositoryWrapper.BuildBagRepository.FindByCondition(x => x.Buildbagid == buildBagId).FirstOrDefault()?.Bagdescription ?? string.Empty;
            return buildBagName;
        }
        #endregion
        #region // SOS Changes
        public async Task<List<Componentsoftwarebuildbags>> ComponetBuildBagEntity(long buildBagId)
        {
            List<Componentsoftwarebuildbags> componentbuildBag = await _repositoryWrapper.ComponentSoftwareBuildBagRepository.FindByCondition(
                    x => x.Buildbagid == buildBagId).Include(y => y.Buildbag)
                    .Include(x => x.Componentsoftwarebuild).ThenInclude(x => x.Componentmanufacturer)
                .OrderByDescending(x => x.Creationdate).ToListAsync();

            return componentbuildBag;
        }
        public async Task<ViewBagandComponenetDto> ViewBagAndComponentDetailsAsync(long buildBagId)
        {
            var componentbuildBag = await GetBagAndComponentDetailsForDropdownAsync(buildBagId, false);

            return componentbuildBag.FirstOrDefault(); ;
        }

        public async Task<List<Buildbags>> GetBagAndComponentEntity(long buildBagId, bool isEditDropDown = false, bool isOpcoAndDcfBased = false, long dcId = 0, short opCoId = 0)
        {
            List<Buildbags> componentbuildBag = new List<Buildbags>();
            if (isOpcoAndDcfBased)
            {
                var dcEntity = await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == dcId).FirstOrDefaultAsync();

                if (dcEntity != null)
                {
                    componentbuildBag = await _repositoryWrapper.BuildBagRepository.FindByCondition(x => x.Opcoid == opCoId &&
                    x.Designcomponentfamilyid == dcEntity.Designcomponentfamilyid)
                    .Include(x => x.Componentsoftwarebuildbags).ThenInclude(x => x.Componentsoftwarebuild).ThenInclude(x => x.Componentmanufacturer)
                    .ToListAsync();
                }
                else
                {
                    componentbuildBag = new List<Buildbags>();
                }
            }
            else
            {
                if (!isEditDropDown)
                {
                    componentbuildBag = await _repositoryWrapper.BuildBagRepository.FindByCondition(
                        x => x.Buildbagid == buildBagId)
                        .Include(x => x.Componentsoftwarebuildbags).ThenInclude(x => x.Componentsoftwarebuild).ThenInclude(x => x.Componentmanufacturer)
                        .ToListAsync();

                }
                else
                {
                    componentbuildBag = await _repositoryWrapper.BuildBagRepository.FindAll(true)
                        .Include(x => x.Componentsoftwarebuildbags).ThenInclude(x => x.Componentsoftwarebuild).ThenInclude(x => x.Componentmanufacturer)
                        .ToListAsync();
                }
            }

            return componentbuildBag;
        }

        public Buildbags GetDummyBag()
        {
            var getDummyBagEntity = _repositoryWrapper.BuildBagRepository.FindByCondition(x => x.Bagdescription.ToLower() == ConstantValueFilter.dummyBagName.ToLower()
                        && x.Bagversion == ConstantValueFilter.dummyVersion).FirstOrDefault();

            return getDummyBagEntity;

        }
        public async Task<List<ViewBagandComponenetDto>> GetBagAndComponentDetailsForDropdownAsync(long buildBagId, bool isEditDropDown = false, bool isOpcoAndDcfBased = false, long dcId = 0, short opCoId = 0, long lcmbagId = 0)
        {
            var existingComponenetBuild = await GetBagAndComponentEntity(buildBagId, isEditDropDown, isOpcoAndDcfBased, dcId, opCoId);
            List<ViewBagandComponenetDto> viewBagComponenet = new List<ViewBagandComponenetDto>();

            var getDummyBag = GetDummyBag();

            if (getDummyBag != null)
            {
                if (existingComponenetBuild.Any(x => x.Buildbagid == getDummyBag.Buildbagid) == false)
                {
                    existingComponenetBuild.Add(getDummyBag);
                }

            }

            var checkLcmBagExist = existingComponenetBuild.Where(x => x.Buildbagid == lcmbagId).FirstOrDefault();

            if (existingComponenetBuild != null)
            {

                foreach (var item in existingComponenetBuild)
                {
                    ViewBagandComponenetDto fetchComponentSoftwareBuild = new ViewBagandComponenetDto();

                    if (checkLcmBagExist != null && checkLcmBagExist.Bagdescription == item.Bagdescription && checkLcmBagExist.Bagversion != item.Bagversion)
                    {
                        fetchComponentSoftwareBuild.isColour = true;
                    }

                    var componenetDetails = item.Componentsoftwarebuildbags.Select(x => new FilterValueDto
                    {
                        Text = $"{x.Componentsoftwarebuild.Componentmanufacturer.Componentmanufacturer}-" +
                                             $"{x.Componentsoftwarebuild.Componentmanufacturer.Componentname}-" +
                                             $"{x.Componentsoftwarebuild.Softwareversion}",
                        Value = x.Componentsoftwarebuild.Componentsoftwarebuildid.ToString()
                    }).DistinctBy(x => x.Value).ToList();
                    fetchComponentSoftwareBuild.BuildBagId = item.Buildbagid;
                    fetchComponentSoftwareBuild.ComponentBagDescription = GetBuildBagDescription(item);

                    fetchComponentSoftwareBuild.Key = item.Buildbagid;
                    fetchComponentSoftwareBuild.Text = GetBuildBagDescription(item);

                    fetchComponentSoftwareBuild.MappedComponentDetails = componenetDetails;
                    viewBagComponenet.Add(fetchComponentSoftwareBuild);
                }
                viewBagComponenet.Where(x => x.BuildBagId != lcmbagId).OrderBy(x => x.BuildBagId).ToList();
            }
            return viewBagComponenet;

        }
        #endregion
        #region //Get Active users for EDU and SUB domain spoc
        //public async Task<List<Organisation>> GetActiveUserDetails(List<int> userId)
        //{
        //    var userDetailsEntity = await _repositoryWrapper.UserRepository.FindByCondition(x => userId.Any(y => y == x.Id) && x.Active == true).Select(x => x.Id).Distinct().ToListAsync();
        //    var ActiveDetailsRecord = new List<Organisation>();
        //    if (userDetailsEntity != null)
        //    {
        //        ActiveDetailsRecord = _repositoryWrapper.OrganisationRepository
        //            .FindByCondition(x => userDetailsEntity.Any(y => y == x.Contactid)).Include(x => x.Contact).ToList();
        //        return ActiveDetailsRecord?.DistinctBy(x => x.Contactid).ToList();
        //    }
        //    return ActiveDetailsRecord;
        //}
        public async Task<List<Aspnetusers>> GetActiveUserDetails(List<int> userId)
        {
            var userDetailsEntity = await _repositoryWrapper.UserRepository.FindByCondition(x => userId.Any(y => y == x.Id) && x.Active == true).ToListAsync();
            var ActiveDetailsRecord = new List<Aspnetusers>();
            if (userDetailsEntity != null)
            {
                ActiveDetailsRecord = userDetailsEntity.Where(x => x.Isdesigncontact == true).ToList(); 
                return ActiveDetailsRecord?.DistinctBy(x => x.Id).ToList();
            }
            return ActiveDetailsRecord;
        }
        #endregion

        #region
        public async Task<long> CheckDuplicatesAssetExistsForMigration(short opCoId, long dcId, string elementName)
        {

            try
            {
                var assetEntity = await _repositoryWrapper.NetworkElementAsPlanned
                    .FindByCondition(x => x.Designcomponentid == dcId && x.Opcoid == opCoId && x.Elementname.ToLower() == elementName.ToLower()
                    && x.Deleted == false)
                    .FirstOrDefaultAsync();

                if (assetEntity != null)
                {
                    return (long)assetEntity?.Lcmengineeringid;
                }

                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion

        public async Task<List<Exceltemplateconfiguration>> GetExcelConfiguration(string processName)
        {
            var excelConfigEntity = await _repositoryWrapper.ExcelTemplateConfigurationRepository.FindByCondition(x => x.Processname.Trim().ToLower() == processName.Trim().ToLower()).OrderBy(x => x.Columnorder).ToListAsync();
            return excelConfigEntity;
        }
        public string GetProjectStatus(DateTime? paCompletionDate, DateTime? EOM, DateTime? EOS)
        {
            if (paCompletionDate != null && EOM != null && EOS != null)
            {
                if (paCompletionDate < EOM)
                {
                    return ConstantValueFilter.GREEN;
                }
                else if ((EOM < paCompletionDate) && (paCompletionDate < EOS))
                {
                    return ConstantValueFilter.AMBER;
                }
                else
                {
                    if (paCompletionDate > EOS)
                    {
                        return ConstantValueFilter.RED;
                    }

                }
            }
            else
            {
                return ConstantValueFilter.NA;
            }
            return ConstantValueFilter.NA;
        }
        public string GetDependentSoftware(string build, string os)
        {
            if (build.ToLower().Replace(" ", "") == ConstantValueFilter.BluprintNfvi.ToLower() || build.ToLower().Replace(" ", "") == ConstantValueFilter.BluprintNfci.ToLower())
            {
                return "VMWare";
            }
            else
            {
                return os;
            }
        }
        public string LcmCategoryStatust(short? status)
        {
            switch (status)
            {
                case 0:
                    return "N/A";
                case 1:
                    return "LCM High";
                case 2:
                    return "LCM Medium";
                case 3:
                    return "LCM Low";
                default:
                    return "N/A";
            }
        }
        public string LcmCategoryStatus(string status)
        {
            switch (status)
            {
                case "N/A":
                    return "0";
                case "LCM High":
                    return "1";
                case "LCM Medium":
                    return "2";
                case "LCM Low":
                    return "3";
                default:
                    return string.Empty;
            }
        }
        public IDictionary<short, string> LcmCategoryStatusResource()
        {

            var result = new Dictionary<short, string>();
            result.Add(0, "N/A");
            result.Add(1, "LCM High");
            result.Add(2, "LCM Medium");
            result.Add(3, "LCM Low");

            return result;
        }
        public string PriorityStatus(string status)
        {
            switch (status)
            {
                case "0":
                    return "External Demand";
                case "1":
                    return "Mandatory";
                case "2":
                    return "Must Have";
                case "3":
                    return "Important";
                case "4":
                    return "Nice to Have";
                default:
                    return string.Empty;
            }
        }
        public IDictionary<string, string> PriorityStatusResource()
        {

            var result = new Dictionary<string, string>();
            result.Add("0", "External Demand");
            result.Add("1", "Mandatory");
            result.Add("2", "Must Have");
            result.Add("3", "Important");
            result.Add("4", "Nice to Have");


            return result;
        }

        public string GetVirtualized(long? originalDcIndex, long? plannedDc, bool? cloudHostedAsset, long? dcfId, int? rule)
        {
            try
            {
                originalDcIndex = Convert.ToInt64(originalDcIndex);
                plannedDc = Convert.ToInt64(plannedDc);
                dcfId = Convert.ToInt64(dcfId);
                cloudHostedAsset = Convert.ToBoolean(cloudHostedAsset);
                rule = Convert.ToInt32(rule);

                if (originalDcIndex == 0 && plannedDc == 0) return ConstantValueFilter.NA;

                var isOriginalDcVirtualized = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == originalDcIndex || x.Designcomponentfamilyid == dcfId)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                    .FirstOrDefault()?.Systemtype?.Systemtypesmajorhardwarebuilds?.FirstOrDefault()?.Majorhardware?.Buildconstruction?.Rule;

                var isplannedDcVirtualized = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == originalDcIndex || x.Designcomponentfamilyid == dcfId)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                    .FirstOrDefault()?.Systemtype?.Systemtypesmajorhardwarebuilds?.FirstOrDefault()?.Majorhardware?.Buildconstruction?.Rule;

                if (plannedDc == (int)BuildconstructionRuleEnum.ProprietaryHW)
                {
                    return ConstantValueFilter.NO;
                }
                else if ((isOriginalDcVirtualized == (int)BuildconstructionRuleEnum.VirtualHW) && (isplannedDcVirtualized == (int)BuildconstructionRuleEnum.VirtualHW) && cloudHostedAsset == true)
                {
                    return ConstantValueFilter.YesVirtualized;
                }
                else if ((isOriginalDcVirtualized == (int)BuildconstructionRuleEnum.ProprietaryHW) && cloudHostedAsset == false)
                {
                    return ConstantValueFilter.NO;
                }
                else if ((isplannedDcVirtualized == (int)BuildconstructionRuleEnum.VirtualHW) && cloudHostedAsset == true)
                {
                    return ConstantValueFilter.YesNewVirtualization;
                }
                else
                {
                    return ConstantValueFilter.NA;
                }
            }
            catch (Exception ex)
            {
                return ConstantValueFilter.NA;
            }
        }
        public async Task<ResultDto> CreateOrUpdateBptreport(long paId, long originalDc = 0, long plannedDc = 0, long dcfId = 0)
        {

            var bpttrackerEntity = _repositoryWrapper.BudgetProjectTrackersRepository.FindByCondition(x => x.Plannedactivityid == paId).FirstOrDefault();
            var paEntity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == paId)
                           .Include(x => x.Opco)
                           .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                           .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                           .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                           .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                           .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                           .Include(x => x.Plannedactivityresource)
                           .Include(x => x.Plannedactivitycategory)
                           .Include(x => x.ProgramNavigation)
                           .Include(x => x.Engineeringrisk)
                           .Include(x => x.Benefit)
                           .Include(x => x.Driver)
                           .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc)
                           .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringeduspoc)
                           .Include(x => x.Networkelementasplanned).ThenInclude(x => x.Networkelementasplannedsubdomainspoc)
                           .Include(x => x.Networkelementasplanned).ThenInclude(x => x.Networkelementasplannededuspoc)
                           .FirstOrDefault();

            if (paEntity == null)
            {
                return new ResultDto()
                {
                    Info = ResultMessages.EntryAddUpdateFailed
                };
            }


            var bptEntity = new Budgetprojecttrackers()
            {

                Budgetowner = GetBudgetOwnerForBpt(paEntity.Projectowner, paEntity),
                Plannedactivityid = paEntity.Plannedactivityid,
                Currenttrackingnumber = paEntity.Budgettrackingid,
                Wbs = GetWBSCode(paEntity.Deliveryprojectname),
                Opco = paEntity.Opco.Opco,
                Opcoid = paEntity.Opcoid,
                Domain = GetOrganisationNameForBpt(paEntity, true),
                Team = "CES_EDU",
                Program = paEntity?.ProgramNavigation?.Programdescription,
                Budgetproject = paEntity.Deliveryprojectname,
                // Activity = $"{paEntity.Plannedactivityresource?.Plannedactivityresource} - {paEntity.Activitydetails}",
                Priority = paEntity.Priority,
                Driver = paEntity?.Driver?.Bptdriverdetails,
                Benefits = paEntity?.Benefit?.Benefit,
                Risks = paEntity?.Engineeringrisk?.Description,
                Category = paEntity?.Plannedactivitycategory?.Categorydescription,
                Categoryid = (paEntity?.Plannedactivitycategoryid == null) ? null : paEntity?.Plannedactivitycategoryid,
                Lcmcategories = LcmCategoryStatust(paEntity.Lcmcategories),
                Lcmcategoriesid = paEntity.Lcmcategories,
              Nwelement = paEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Project_Plan
            ? string.Join(",", paEntity?.Designcomponentfamily?.Designcomponents?.
            Select(x => x.Systemtype?.Majorsoftwarebuilds?.Productname?.Description).ToList()) :
            paEntity?.Designcomponentfamily!=null ?
             paEntity?.Designcomponentfamily?.Designcomponents?.
             Select(x => x.Systemtype?.Majorsoftwarebuilds?.Productname?.Description).FirstOrDefault()
            : paEntity?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Productname?.Description,

                Virtualizednwelement = GetVirtualized(originalDc, plannedDc, paEntity.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.FirstOrDefault().Majorhardware?.Buildconstruction?.Iscloudasset, dcfId
                , paEntity.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.FirstOrDefault().Majorhardware?.Buildconstruction?.Rule),
                Vendor = paEntity?.Plannedactivityresource?.Rulelinkeddc == (int)PlannedActivityResourceEnum.Project_Plan
                            ? "Multiple Vendors" : paEntity?.Designcomponentfamily != null ?
                            paEntity?.Designcomponentfamily?.Designcomponents?.Select(x => x.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer).FirstOrDefault()
                            : paEntity?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer,
                Archive = paEntity.Archived == null ? false : paEntity.Archived,

            };

            if (bpttrackerEntity != null)
            {
                bpttrackerEntity = BptUpdate(bptEntity, bpttrackerEntity);
                _repositoryWrapper.BudgetProjectTrackersRepository.Update(bpttrackerEntity);
            }
            else
            {
                _repositoryWrapper.BudgetProjectTrackersRepository.Create(bptEntity);
            }

            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto()
            {
                Data = bptEntity.Plannedactivityid
            };

        }

        public async Task<ResultDto> setArchiveStatusForBpt(Plannedactivities item)
        {
            var existingExtity = await _repositoryWrapper.BudgetProjectTrackersRepository.FindByCondition(x => x.Plannedactivityid == item.Plannedactivityid).FirstOrDefaultAsync();

            if (existingExtity != null)
            {

                existingExtity.Plannedactivityid = item.Plannedactivityid;
                existingExtity.Archive = item.Archived;

                _repositoryWrapper.BudgetProjectTrackersRepository.Update(existingExtity);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto()
                {
                    Data = existingExtity.Plannedactivityid
                };
            }
            return new ResultDto()
            {
                Data = item
            };
        }

        private Budgetprojecttrackers BptUpdate(Budgetprojecttrackers newEnity, Budgetprojecttrackers existingEntity)
        {
            existingEntity.Plannedactivityid = newEnity.Plannedactivityid;
            existingEntity.Currenttrackingnumber = newEnity.Currenttrackingnumber;
            existingEntity.Wbs = newEnity.Wbs;
            existingEntity.Opco = newEnity.Opco;
            existingEntity.Opcoid = newEnity.Opcoid;
            existingEntity.Domain = newEnity.Domain;
            existingEntity.Team = newEnity.Team;
            existingEntity.Budgetowner = newEnity.Budgetowner;
            existingEntity.Program = newEnity.Program;
            existingEntity.Budgetproject = newEnity.Budgetproject;
            existingEntity.Activity = newEnity.Activity;
            existingEntity.Priority = newEnity.Priority;
            existingEntity.Driver = newEnity.Driver;
            existingEntity.Benefits = newEnity.Benefits;
            existingEntity.Risks = newEnity.Risks;
            existingEntity.Category = newEnity.Category;
            existingEntity.Nwelement = newEnity.Nwelement;
            existingEntity.Virtualizednwelement = newEnity.Virtualizednwelement;
            existingEntity.Vendor = newEnity.Vendor;
            existingEntity.Lcmcategories = newEnity.Lcmcategories;
            existingEntity.Archive = newEnity.Archive;



            return existingEntity;
        }
        #region Common Validation Function
        public bool TrySetValueFromImportCell(object target, string propertyName, object? value)
        {
            var prop = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop == null) return false;

            try
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                string? stringValue = value?.ToString();

                if (type == typeof(int))
                {
                    if (string.IsNullOrEmpty(stringValue))
                        prop.SetValue(target, null);
                    else if (int.TryParse(stringValue, out int intResult))
                        prop.SetValue(target, intResult);
                    else
                        return false;
                }
                else if (type == typeof(long))
                {
                    if (string.IsNullOrEmpty(stringValue))
                        prop.SetValue(target, null);
                    else if (long.TryParse(stringValue, out long longResult))
                        prop.SetValue(target, longResult);
                    else
                        return false;
                }
                else if (type == typeof(short))
                {
                    if (string.IsNullOrEmpty(stringValue))
                        prop.SetValue(target, null);
                    else if (short.TryParse(stringValue, out short shortResult))
                        prop.SetValue(target, shortResult);
                    else
                        return false;
                }
                else if (type == typeof(decimal))
                {
                    if (string.IsNullOrEmpty(stringValue))
                        prop.SetValue(target, null);
                    else if (decimal.TryParse(stringValue, out decimal decimalResult))
                        prop.SetValue(target, decimalResult);
                    else
                        return false;
                }
                else if (type == typeof(bool))
                {
                    if (string.IsNullOrEmpty(stringValue))
                        prop.SetValue(target, null);
                    else if (bool.TryParse(stringValue, out bool boolResult))
                        prop.SetValue(target, boolResult);
                    else
                        return false;
                }
                else
                {
                    var converted = Convert.ChangeType(value, type);
                    prop.SetValue(target, converted);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public object? ConvertValueToTypeForImport(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            return value.Trim().ToLower() switch
            {
                "yes" => true,
                "true" => true,
                "no" => false,
                "false" => false,
                _ => value
            };
        }

        #endregion

        public DeliveryTrackingDtoCreate MappingDeliveryTracing(MIlestoneStatus milestoneStatus, Plannedactivities plannedactivities, int nodeCount = 0)
        {
            try
            {
                var result = new DeliveryTrackingDtoCreate();
                result.PlannedActivityId = plannedactivities.Plannedactivityid;

                result.Ms4BaseLineDate = plannedactivities.Plannedcompletion;
                result.Ms4LatestPlanningDate = plannedactivities.Plannedcompletion;

                #region m3 Node calculation
                int mileStoneDurationForM3 = milestoneStatus.MS4Status;
                if (milestoneStatus.isNodeCountApplicable && nodeCount != 0) mileStoneDurationForM3 = nodeCount * milestoneStatus.MS4Status;

                result.Ms3BaseLineDate = (milestoneStatus.MS4Status == 0) ? null : result.Ms4BaseLineDate.Value.AddDays(-7 * (mileStoneDurationForM3));
                result.Ms3LatestPlanningDate = (milestoneStatus.MS4Status == 0) ? null : result.Ms3BaseLineDate;

                #endregion
                if (result.Ms3BaseLineDate != null)
                {
                    result.Ms2BaseLineDate = (milestoneStatus.MS3Status == 0) ? null : result.Ms3BaseLineDate.Value.AddDays(-7 * (milestoneStatus.MS3Status));
                    result.Ms2LatestPlanningDate = (milestoneStatus.MS3Status == 0) ? null : result.Ms2BaseLineDate;
                }
                if (result.Ms2BaseLineDate != null)
                {
                    result.Ms1BaseLineDate = (milestoneStatus.MS2Status == 0) ? null : result.Ms2BaseLineDate.Value.AddDays(-7 * (milestoneStatus.MS2Status));
                    result.Ms1LatestPlanningDate = (milestoneStatus.MS2Status == 0) ? null : result.Ms1BaseLineDate;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue is happed while Mapping DeliveryTracing for this PA : {plannedactivities.Plannedactivityid} {ex.Message} ");
                return null;
            }
        }

        public Deliverytrackings MappingDeliveryTracingForUpdate(MIlestoneStatus milestoneStatus, Deliverytrackings result, DateTime? PaPlannedcompletion, int nodeCount = 0)
        {
            try
            {

                result.Ms4baselinedate = PaPlannedcompletion;
                result.Ms4latestplanningdate = PaPlannedcompletion;

                #region m3 Node calculation
                int mileStoneDurationForM3 = milestoneStatus.MS4Status;
                if (milestoneStatus.isNodeCountApplicable && nodeCount != 0) mileStoneDurationForM3 = nodeCount * milestoneStatus.MS4Status;

                result.Ms3baselinedate = (milestoneStatus.MS4Status == 0) ? null : result.Ms4baselinedate.Value.AddDays(-7 * (mileStoneDurationForM3));
                result.Ms3latestplanningdate = (milestoneStatus.MS4Status == 0) ? null : result.Ms3baselinedate;

                #endregion

                result.Ms2baselinedate = (milestoneStatus.MS3Status == 0) ? null : result.Ms3baselinedate.Value.AddDays(-7 * (milestoneStatus.MS3Status));
                result.Ms2latestplanningdate = (milestoneStatus.MS3Status == 0) ? null : result.Ms2baselinedate;

                result.Ms1baselinedate = (milestoneStatus.MS2Status == 0) ? null : result.Ms2baselinedate.Value.AddDays(-7 * (milestoneStatus.MS2Status));
                result.Ms1latestplanningdate = (milestoneStatus.MS2Status == 0) ? null : result.Ms1baselinedate;


                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue is happed while Mapping for DeliveryTracing Update  : {result.Id} {ex.Message} ");
                return null;
            }
        }
        public async Task<MIlestoneStatus> CalculateMSDuration(Plannedactivities plannedactivities, IRepositoryWrapper _repositoryWrapper)
        {
            var result = new MIlestoneStatus();
            try
            {

                var settingUpdateDetails = await _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivityresourceid ==
                plannedactivities.Plannedactivityresourceid && x.Milestonestatus != null).ToListAsync();

                if (settingUpdateDetails != null && settingUpdateDetails.Count > 0)
                {
                    if (settingUpdateDetails.Any(x => x.Settingsupdateplnactdes.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.RolloutComplete))
                    {
                        result.isNodeCountApplicable = true;
                    }

                    result.MS1Status = (int)settingUpdateDetails.Where(x => x.Milestonestatus.Value == (int)MilestoneStatusEnum.MS1).ToList().Sum(x => x.Milestonestatusduration);
                    result.MS2Status = (int)settingUpdateDetails.Where(x => x.Milestonestatus.Value == (int)MilestoneStatusEnum.MS2).ToList().Sum(x => x.Milestonestatusduration);
                    result.MS3Status = (int)settingUpdateDetails.Where(x => x.Milestonestatus.Value == (int)MilestoneStatusEnum.MS3).ToList().Sum(x => x.Milestonestatusduration);
                    result.MS4Status = (int)settingUpdateDetails.Where(x => x.Milestonestatus.Value == (int)MilestoneStatusEnum.MS4).ToList().Sum(x => x.Milestonestatusduration);
                    result.isSuccess = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                _logger.LogError($"Issue is happed while calculating MS Duratioin for this PA : {plannedactivities.Plannedactivityid} {ex.Message}");
                return result;

            };
        }

        public async Task<IEnumerable<Projectsplan>> AddProjectPlan(Plannedactivities plannedactivities, IRepositoryWrapper _repositoryWrappere)
        {
            var projectPlane = new List<Projectsplan>();
            try
            {
                var settingUpdatePlanned = await _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x =>
                x.Plannedactivityresourceid == plannedactivities.Plannedactivityresourceid && x.Milestonestatus != null && x.Milestonestatus != 0 && x.Milestonestatusduration != 0).OrderByDescending(x => x.Order).ToListAsync();
                DateTime? existingDateforCalculation = null;
                int existingDuration = 0;
                int count = 0;
                int previousFinalStateCount = 0;
                int assetCount = 0;
                bool isNodeCountApplicable = false;
                if (plannedactivities.Lcmengineeringid != null)
                {
                    var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                    var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;

                    var lcmAssetEntities = _repositoryWrapper.Lcmengineering
                          .FindByCondition(x =>
                              x.Lcmengineeringid == plannedactivities.Lcmengineeringid &&
                              x.Elementcount == true
                          )
                          .Include(x => x.Networkelementsasplanned).FirstOrDefault();

                    if (lcmAssetEntities?.Networkelementsasplanned != null && lcmAssetEntities?.Networkelementsasplanned.Count > 0)
                    {
                        assetCount = lcmAssetEntities.Networkelementsasplanned.Where(t => t.Deploymentstatusid == getAssetInserviceID
                        && t.Environmentid == getProductionEnvrionmentId).Count();

                    }

                }

                if (settingUpdatePlanned != null && settingUpdatePlanned.Count > 0)
                {
                    var existingEntity = await _repositoryWrapper.ProjectPlanRepository.FindByCondition(x => x.Plannedactivityid == plannedactivities.Plannedactivityid).ToListAsync();

                    if (existingEntity != null && existingEntity.Count == 0)
                    {
                        count = settingUpdatePlanned.Count();
                        previousFinalStateCount = (int)(settingUpdatePlanned.Skip(1)?.FirstOrDefault()?.Order);

                        if (settingUpdatePlanned.Any(x => x.Settingsupdateplnactdes.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.RolloutComplete))
                            isNodeCountApplicable = true;


                        var orderedSettingUpdatePlanned = settingUpdatePlanned;

                        foreach (var x in orderedSettingUpdatePlanned)
                        {
                            int currentDuration = x.Milestonestatusduration.Value;
                            var grid = new Projectsplan
                            {
                                Plannedactivityid = plannedactivities.Plannedactivityid,
                                Settingsupdateplannedactivityid = x.Settingsupdateplnactid
                            };

                            if (existingDateforCalculation == null)
                            {
                                grid.Planningenddate = plannedactivities.Plannedcompletion;
                                if (assetCount != 0 && isNodeCountApplicable)
                                    currentDuration = assetCount * currentDuration;

                                grid.Planningstartdate = plannedactivities.Plannedcompletion.Value.AddDays(-7 * (currentDuration));

                            }
                            else
                            {
                                int msDurationWeek = existingDuration;
                                if (previousFinalStateCount == x.Order && assetCount != 0 && isNodeCountApplicable)
                                    msDurationWeek = assetCount * existingDuration;

                                grid.Planningenddate = existingDateforCalculation.Value.AddDays(-7 * msDurationWeek);

                                grid.Planningstartdate = grid.Planningenddate.Value.AddDays(-7 * (currentDuration));
                            }

                            if (count == 1)
                            {
                                grid.Planningenddate = existingDateforCalculation.Value.AddDays(-7 * existingDuration);
                                grid.Planningstartdate = existingDateforCalculation.Value.AddDays(
                                    -7 * (x.Milestonestatusduration.Value + existingDuration));
                            }

                            existingDateforCalculation = grid.Planningenddate;
                            existingDuration = x.Milestonestatusduration.Value;
                            count -= 1;
                            grid.Baselineenddate = grid.Planningenddate;
                            grid.Baselinestartdate = grid.Planningstartdate;

                            projectPlane.Add(grid);
                        }

                    }

                    return projectPlane;
                }
                return projectPlane;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue is happed while Adding ProjectPlane for this PA : {plannedactivities.Plannedactivityid} {ex.Message}");
                return projectPlane;
            }
        }

        public async Task<IEnumerable<Projectsplan>> GAddProjectPlan(Plannedactivities plannedactivities, IRepositoryWrapper _repositoryWrappere)
        {
            var projectPlane = new List<Projectsplan>();
            try
            {
                var settingUpdatePlanned = await _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x =>
                x.Plannedactivityresourceid == plannedactivities.Plannedactivityresourceid && x.Milestonestatus != null).OrderByDescending(x => x.Order).ToListAsync();
                DateTime? existingDateforCalculation = null;
                int existingDuration = 0;
                int count = 0;
                int previousFinalStateCount = 0;
                int assetCount = 0;
                if (plannedactivities.Lcmengineeringid != null)
                {
                    var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                    var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;

                    var lcmAssetEntities = _repositoryWrapper.Lcmengineering
                          .FindByCondition(x =>
                              x.Lcmengineeringid == plannedactivities.Lcmengineeringid &&
                              x.Elementcount == true
                          )
                          .Include(x => x.Networkelementsasplanned).FirstOrDefault();

                    if (lcmAssetEntities?.Networkelementsasplanned != null && lcmAssetEntities?.Networkelementsasplanned.Count > 0)
                    {
                        assetCount = lcmAssetEntities.Networkelementsasplanned.Where(t => t.Deploymentstatusid == getAssetInserviceID
                        && t.Environmentid == getProductionEnvrionmentId).Count();

                    }

                }

                if (settingUpdatePlanned != null && settingUpdatePlanned.Count > 0)
                {
                    var existingEntity = await _repositoryWrapper.ProjectPlanRepository.FindByCondition(x => x.Plannedactivityid == plannedactivities.Plannedactivityid).ToListAsync();

                    if (existingEntity != null && existingEntity.Count == 0)
                    {
                        count = settingUpdatePlanned.Count();
                        previousFinalStateCount = (int)(settingUpdatePlanned.Skip(1)?.FirstOrDefault()?.Order);

                        var orderedSettingUpdatePlanned = settingUpdatePlanned; ;

                        foreach (var x in orderedSettingUpdatePlanned)
                        {
                            var grid = new Projectsplan
                            {
                                Plannedactivityid = plannedactivities.Plannedactivityid,
                                Settingsupdateplannedactivityid = x.Settingsupdateplnactid
                            };

                            if (existingDateforCalculation == null)
                            {
                                grid.Planningstartdate = plannedactivities.Plannedcompletion;
                                grid.Planningenddate = grid.Planningstartdate;

                            }
                            else if (previousFinalStateCount == x.Order && assetCount != 0)
                            {
                                grid.Planningstartdate = existingDateforCalculation.Value.AddDays(-7 * (assetCount * existingDuration));
                                grid.Planningenddate = grid.Planningstartdate;
                            }
                            else
                            {
                                grid.Planningstartdate = existingDateforCalculation.Value.AddDays(-7 * existingDuration);
                                grid.Planningenddate = grid.Planningstartdate;
                            }

                            if (count == 1)
                            {
                                grid.Planningenddate = existingDateforCalculation.Value.AddDays(-7 * existingDuration);
                                grid.Planningstartdate = existingDateforCalculation.Value.AddDays(
                                    -7 * (x.Milestonestatusduration.Value + existingDuration));
                            }

                            existingDateforCalculation = grid.Planningstartdate;
                            existingDuration = x.Milestonestatusduration.Value;
                            count -= 1;

                            projectPlane.Add(grid);
                        }

                    }

                    return projectPlane;
                }
                return projectPlane;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue is happed while Adding ProjectPlane for this PA : {plannedactivities.Plannedactivityid} {ex.Message}");
                return projectPlane;
            }
        }
        public string NormalizeKey(string input)
        {
            return input?
                .Trim()
                .ToLowerInvariant()
                .Replace(" ", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("\"", "");
        }

        public string GetPropertFY(string input)
        {
            // Match FY with optional space, then digits/ digits
            string pattern = @"FY\s?\d{2}/\d{2}";
            var result = string.Empty;

            Match match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                // Trim in case there was an extra space like "FY 19/20"
                result = match.Value.Replace(" ", "");
            }
            return result;
        }

        public async Task<ResultDto> UpdateProjectPlanDateForLastDeliveryStatus(Plannedactivities plannedactivities, IRepositoryWrapper _repositoryWrappere)
        {
            try
            {
                var settingUpdatePlanned = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x =>
                       x.Plannedactivityresourceid == plannedactivities.Plannedactivityresourceid && x.Milestonestatus != null && x.Milestonestatus != 0 && x.Milestonestatusduration != 0).OrderByDescending(x => x.Order).FirstOrDefault();

                if (settingUpdatePlanned != null)
                {
                    var lastDeliverySatusProjectPlan = _repositoryWrapper.ProjectPlanRepository.FindByCondition(x => x.Plannedactivityid == plannedactivities.Plannedactivityid && x.Settingsupdateplannedactivityid == settingUpdatePlanned.Settingsupdateplnactid).FirstOrDefault();

                    var deliverTracking = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Plannedactivityid == plannedactivities.Plannedactivityid).FirstOrDefault();
                    if (lastDeliverySatusProjectPlan != null)
                    {
                        var oldValue = lastDeliverySatusProjectPlan.Planningenddate.Value.ToShortDateString();
                        lastDeliverySatusProjectPlan.Planningenddate = plannedactivities.Plannedcompletion;
                        _repositoryWrapper.ProjectPlanRepository.Update(lastDeliverySatusProjectPlan);
                        await CreateOrUpdateProjectPlanAudit(lastDeliverySatusProjectPlan.Projectsplanid, oldValue, plannedactivities.Plannedcompletion.Value.ToShortDateString(), 1, _repositoryWrappere);
                    }
                    if (deliverTracking != null)
                    {
                        deliverTracking.Ms4latestplanningdate = plannedactivities.Plannedcompletion;
                        _repositoryWrapper.DeliveryTrackingRepository.Update(deliverTracking);
                    }


                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }

                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {ex.StackTrace}");
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddUpdateFailed
                };
            }
        }

        public async Task<ResultDto> CreateOrUpdateProjectPlanAudit(long projectPlanId, string oldValue, string newValue, short processType, IRepositoryWrapper repositoryWrapper)
        {
            if (projectPlanId != 0)
            {
                Projectplanaudit projectplanaudit = new Projectplanaudit
                {
                    Projectsplanid = projectPlanId,
                    Oldvalue = oldValue,
                    Newvalue = newValue,
                    Processtype = processType
                };
                _repositoryWrapper.ProjectPlanAuditRepository.Create(projectplanaudit);
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();

                return new ResultDto { Info = ResultMessages.EntryAddSuccess };
            }
            else
            {
                return new ResultDto { Info = ResultMessages.EntryNotAdd };
            }
        }

        public async Task<ResultDto> UpdateProjectPlanDateForLastDeliveryStatusOfMS(Plannedactivities plannedactivities, DateTime? mileStoneDate, int mileStone, IRepositoryWrapper _repositoryWrapper, bool isEdit = false, long SettingsUpdateplnactId = 0)
        {
            try
            {
                var settingUpdatePlanned = SettingsUpdateplnactId != 0 ? _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x =>
                                               x.Plannedactivityresourceid == plannedactivities.Plannedactivityresourceid
                                               && x.Milestonestatus != null
                                               && x.Milestonestatus != 0
                                               && x.Milestonestatusduration != 0
                                               && x.Settingsupdateplnactid == SettingsUpdateplnactId).FirstOrDefault() 
                                               : _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x =>
                                               x.Plannedactivityresourceid == plannedactivities.Plannedactivityresourceid
                                               && x.Milestonestatus != null
                                               && x.Milestonestatus != 0
                                               && x.Milestonestatusduration != 0
                                               && x.Milestonestatus == mileStone).OrderByDescending(x => x.Order).FirstOrDefault();
                int processType = isEdit == true ? 1 : 2;
                if (settingUpdatePlanned != null)
                {
                    var lastDeliverySatusProjectPlan = _repositoryWrapper.ProjectPlanRepository.FindByCondition(x => x.Plannedactivityid == plannedactivities.Plannedactivityid
                                                                                && x.Settingsupdateplannedactivity.Settingsupdateplnactid == settingUpdatePlanned.Settingsupdateplnactid).FirstOrDefault();
                    var deliverTracking = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Plannedactivityid == plannedactivities.Plannedactivityid).FirstOrDefault();
                    if (lastDeliverySatusProjectPlan != null)
                    {
                        var oldValue = lastDeliverySatusProjectPlan.Planningenddate?.ToShortDateString();

                        lastDeliverySatusProjectPlan.Planningenddate = mileStoneDate;

                        _repositoryWrapper.ProjectPlanRepository.Update(lastDeliverySatusProjectPlan);

                        await CreateOrUpdateProjectPlanAudit(lastDeliverySatusProjectPlan.Projectsplanid, oldValue, mileStoneDate.Value.ToShortDateString(), (short)processType, _repositoryWrapper);
                    }


                    if (deliverTracking != null && isEdit == true)
                    {
                        if(mileStone == (int)MilestoneStatusEnum.MS1)
                        deliverTracking.Ms1latestplanningdate = mileStoneDate;

                        if (mileStone == (int)MilestoneStatusEnum.MS2)
                            deliverTracking.Ms2latestplanningdate = mileStoneDate;

                        if (mileStone == (int)MilestoneStatusEnum.MS3)
                            deliverTracking.Ms3latestplanningdate = mileStoneDate;

                        if (mileStone == (int)MilestoneStatusEnum.MS4)
                        {
                            deliverTracking.Ms4latestplanningdate = mileStoneDate;
                            var plannedActivity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == plannedactivities.Plannedactivityid).FirstOrDefault();
                            if(plannedActivity != null && SettingsUpdateplnactId != 0)
                            {
                                plannedActivity.Plannedcompletion = mileStoneDate;
                                _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                            }
                        }
                        _repositoryWrapper.DeliveryTrackingRepository.Update(deliverTracking);
                    }

                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                }

                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {ex.StackTrace}");
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddUpdateFailed
                };
            }
        }
    
        public async Task<Hwpassthroughlcm> GetHwPassthroughlcmRecords(string localMarket, string assetClass, string hardwareModel)
        {
            var passthrough = await Task.Run(() => _repositoryWrapper.HwPassThroughLcmRepository.FindByCondition(x => x.Localmarket == localMarket && x.Assetclass == assetClass && x.Hardwaremodel == hardwareModel).FirstOrDefault());
            return passthrough;
        }

        public virtual async Task<string> GetDomainName(long nonTemsVerticalId)
        {
            return await _repositoryWrapper.AppConfigurationSettingsRepository.FindByCondition(x => x.Appsettingid == 2 && x.Appconfigurationsettingid == nonTemsVerticalId).Select(x => x.Settingsvalue).FirstOrDefaultAsync();
        }

        #region FNT AND TSR SCHEDULER CONFIG TABLE

        public async Task<GenericReportSchedulerDto> GetReportSchedulerDetails(string reportName)
        {
            string currentDayOfWeek = DateTime.Now.DayOfWeek.ToString();

            var query=await _repositoryWrapper.ReportSchedulerRepository.FindByCondition(x=>x.Reportname== reportName && 
            (x.Scheduleddate==DateTime.Now.Day ||x.Scheduleddate==0 ||x.Scheduleddayinweek.ToLower()==currentDayOfWeek.ToLower()) && x.Isscheduled==true).FirstOrDefaultAsync();

            if (query == null) return null;

            return new GenericReportSchedulerDto
            {
                DynamicReportId = (long)query.Reportschedulerid,
                ReportName = query?.Reportname,
                Isscheduled = query?.Isscheduled,
                ExportFileFormat = query?.Exportfileformat?? ".xlsx",
                ExportFilePath = query?.Exportfilepath,
                ScheduledDate = Convert.ToInt16(query?.Scheduleddate),
                ScheduledDayInWeek = query?.Scheduleddayinweek,
            };
        }
        #endregion

        #region service Plan DCF status 
        public string GetServieStatus(string status)
        {
            switch (status)
            {
                case "1":
                    return "Planned";
                case "2":
                    return "InProgress";
                case "3":
                    return "Completed";
                default:
                    return string.Empty;


            };           
        }

        #endregion

        #region  AssetOutofScope only for LCM export
        public string GetAssetOutofScope(string input)
        {
            if (input == null) return string.Empty;
          var scopeDictionary = new Dictionary<string, string>
                            {
                                { "In scope", "In Scope" },
                                { "Asset in planned dismission, no replacement", "In Scope" },
                                { "Data gathering still ongoing", "Out of Scope" },
                                { "Asset accountability currently on migration to different unit", "Out of Scope" },
                                { "Historical back-up (remediation action completed)", "Historical Backup" },
                                { "Asset planned to be inserted in the network", "Out of Scope" },
                                { "DE asset (managed by DE local tool)", "Out of Scope" },
                                { "Other", "Out of Scope" },
                                { "Asset in planned decommission, no replacement", "In Scope" },
                                { "Asset not under GN accountability", "Out of Scope" }
                            };
            var result = scopeDictionary
                      .FirstOrDefault(kvp =>
                          input.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase))
                      .Value ?? string.Empty;

            return result;
        }
        #endregion

        #region // get Servie details
        public  List<ServicePlanGridDto> GetServicePlanDetails(long serviceplanId)
        {
            var result = new List<ServicePlanGridDto>();
            try
            {
                var Entities =  _repositoryWrapper.ServicePlanDcfMappingRepository.FindByCondition(x => x.Serviceplanid == serviceplanId).AsNoTracking()
                    .Include(x => x.Dcf)
                    .ToList();

                result = Entities.Select(x =>
                {
                    var grid = new ServicePlanGridDto();
                    grid.ServicePlanId = x.Serviceplanid.Value;
                    grid.DCFId = x.Dcfid;
                    grid.Status = x.Status.ToString();
                    grid.DCFName = x.Dcf.DCFName(_repositoryWrapper);

                    return grid;
                }).ToList();

                return result;
            }
            catch
            {
                return result;
            }
        }
        #endregion

    }

}
