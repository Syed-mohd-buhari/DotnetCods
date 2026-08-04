using AutoMapper;
using CAM.BusinessManager;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.Infrastucture;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Imports
{
    public class TsrPassThroughImport : BaseManager
    {
        private readonly ModelContext _modelContext;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        public readonly Dictionary<string, string> nonTsrColumns = new Dictionary<string, string>
                                                    {
                                                            {"SUPPORT_OWNER" ,  "Supportowner" } ,
                                                            {"SUPPORT_TEAM" ,  "Supportteam" } ,
                                                            {"SUPPORT_DOMAIN" ,  "Supportteamsplaceintheorganisation" } ,
                                                            {"RISK_ID" ,  "Relatedriskidsfromriskregisters" } ,
                                                            {"UPSTREAM_DEPENDENCIES" ,  "Upstreamdependencies" } ,
                                                            {"CHANGE_DESCRIPTION" ,  "Changestotheassetsincedeployment" } ,
                                                            {"DOWNSTREAM_DEPENDENCIES" ,  "Downstreamdependencies" } ,
                                                            {"VIRTUAL_PLATFORM_LOCATION" ,  "Hostlocationwithinphysicallocation" } ,
                                                            {"FIRMWARE_PATCH_LEVEL" ,  "Firmwareversionpatchlevel" } ,
                                                            {"MAINTENANCE_SUPPORT_SUPPLIER(HW)" ,  "Maintenancesupportsupplier" } ,
                                                            {"HW_END_OF_LIFE" ,  "Maintenancehardwareendofsupportdate" } ,
                                                            {"OS_PATCH_LEVEL" ,  "Operatingsystemswversionpatchlevel" } ,
                                                            {"MAINTENANCE_SUPPORT_SUPPLIER(SW)" ,  "Maintenancesupportsuppliersecond" } ,
                                                            {"SW_EEOSL_CONTRACT_DATE" ,  "Maintenancesoftwareendofsupportdate" } ,
                                                            {"DEPENDANT_PRODUCT_NAME" ,  "Nameofproductsdependantonasset" } ,
                                                            {"CUSTOMER" ,  "Customer" } ,
                                                            {"ME_PRIVILEGED_ACCESS_LOGGING" ,  "Privilegedaccesslogging" } ,
                                                            {"HW_PART_NUMBER" ,  "Partnumber" } ,
                                                            {"LAST_UPGRADE_DATE" ,  "Lastupgradedate" } ,
                                                            {"SERVICE_LEVEL" ,  "Servicelevel" } ,
                                                            {"NETWORK_OVERSIGHT" ,  "Networkoversightfunction" } ,
                                                            {"DEPENDENT_HARDWARE","Dependanthardware"},
                                                            {"ME_SERIAL_NUMBER" ,  "Serialnumber" } ,

                                                    };
        private readonly Dictionary<int, string> errorList = new Dictionary<int, string>();
        private readonly ILoggerManager _logger;
        public TsrPassThroughImport(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, IMapper mapper,
            IHttpContextAccessor contextAccessor, ILoggerManager logger) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ResultDto> BulkImportOptimizedCode(IFormFile formFile, long recordClassifier, long? nonTemsVertical)
        {
            var insertList = new List<Tsrpassthrough>();
            var updateTemsAssetList = new List<Tsrpassthrough>();
            var updateNonTemsAssetList = new List<Tsrpassthrough>();
            var assetPassthroughEntity = new List<Nontemsnweasplannedpassthrough>();

            try
            {
                using (var stream = new MemoryStream())
                {

                    await formFile.CopyToAsync(stream);
                    using (var package = new XLWorkbook(stream))
                    {

                        var worksheet = package.Worksheet(1);
                        var rows = worksheet.RowsUsed();
                        var columns = worksheet.ColumnsUsed();

                        if (!rows.Any()) return new ResultDto { Info = ResultMessages.NoDataFoundInFile };

                        var headers = rows.First().CellsUsed()
                                        .Select((cell, index) => new { Index = index + 1, Name = cell.GetString().Trim() })
                                        .ToDictionary(h => h.Index, h => h.Name);

                        var propertyMap = new Dictionary<string, string>
                    {
                            {"ME_SOURCE_ASSET_ID" ,  "Vodafoneuniqueidentifier" } ,
                            {"ME_NAME" ,  "Assetname" } ,
                            {"ME_DESCRIPTION" ,  "Assetdescriptionorpurpose" } ,
                            {"ME_TYPE" ,  "Assettype" } ,
                            {"BUSINESS_OWNER" ,  "Businessowner" } ,
                            {"SUPPORT_OWNER" ,  "Supportowner" } ,
                            {"SUPPORT_TEAM" ,  "Supportteam" } ,
                            {"SUPPORT_DOMAIN" ,  "Supportteamsplaceintheorganisation" } ,
                            {"ME_SERVICE_TYPE" ,  "Assetfunction" } ,
                            {"ME_DEPLOYMENT_STATUS" ,  "Deploymentorlifecyclestatus" } ,
                            {"RISK_ID" ,  "Relatedriskidsfromriskregisters" } ,
                            {"REGULATORY_SCOPE" ,  "Regulatoryscope" } ,
                            {"ME_COUNTRY_LOCATED" ,  "Countrywhereassetislocated" } ,
                            {"GEO_LOCATION" ,  "Geolocation" } ,
                            {"INFRASTRUCTURE LOCATION" ,  "Infrastructure" } ,
                            {"UPSTREAM_DEPENDENCIES" ,  "Upstreamdependencies" } ,
                            {"DOWNSTREAM_DEPENDENCIES" ,  "Downstreamdependencies" } ,
                            {"CHANGE_DESCRIPTION" ,  "Changestotheassetsincedeployment" } ,
                            {"IS_CLOUD_HOSTED" ,  "Cloudhostedasset" } ,
                            {"CLOUD_TYPE" ,  "Cloudtype" } ,
                            {"CLOUD_VENDOR" ,  "Cloudvendor" } ,
                            {"EQUIPMENT_NAME" ,  "Equipmentname" } ,
                            {"VIRTUAL_PLATFORM_LOCATION" ,  "Hostlocationwithinphysicallocation" } ,
                            {"ME_SW_VENDOR" ,  "Softwarevendorname" } ,
                            {"MODEL" ,  "Model" } ,
                            {"FIRMWARE_VERSION" ,  "Firmwareversion" } ,
                            {"FIRMWARE_PATCH_LEVEL" ,  "Firmwareversionpatchlevel" } ,
                            {"MAINTENANCE_SUPPORT_SUPPLIER(HW)" ,  "Maintenancesupportsupplier" } ,
                            {"HW_END_OF_SUPPORT" ,  "Vendorhardwareendofsupportdate" } ,
                            {"HW_END_OF_LIFE" ,  "Maintenancehardwareendofsupportdate" } ,
                            {"DEPENDENT_HARDWARE","Dependanthardware"},
                            {"INSTANCE_TYPE" ,  "Instancetype" } ,
                            {"OS_NAME" ,  "Operatingsystemname" } ,
                            {"OS_SW_VERSION" ,  "Operatingsystemswvversion" } ,
                            {"OS_PATCH_LEVEL" ,  "Operatingsystemswversionpatchlevel" } ,
                            {"SYSTEMNAME" ,  "Systemnamedns" } ,
                            {"MANAGEMENT_IP_ADDRESS" ,  "Systemnamemanagementipaddress" } ,
                            {"SYSTEMNAME_NETBIOS" ,  "Systemnamenetbios" } ,
                            {"HOSTNAME" ,  "Systemnamehostname" } ,
                            {"ME_LIVE_STATUS_DATE" ,  "Dateassetmovedtolivestatus" } ,
                            {"ME_DECOMMISSIONED_DATE" ,  "Dateassetdecommissioned" } ,
                            {"HW_MANUFACTURER" ,  "Hardwarevendorname" } ,
                            {"MAINTENANCE_SUPPORT_SUPPLIER(SW)" ,  "Maintenancesupportsuppliersecond" } ,
                            {"SW_EOSL_CONTRACT_DATE" ,  "Vendorsoftwareendofsupportdate" } ,
                            {"SW_EEOSL_CONTRACT_DATE" ,  "Maintenancesoftwareendofsupportdate" } ,
                            {"DEPENDANT_SYSTEM_SW" ,  "Dependantsystemsoftware" } ,
                            {"RESILIENCE_MODEL" ,  "Resiliencemodel" } ,
                            {"RESILIENCE_GEOGRAPHIC_SITE" ,  "Geographicsiteresilience" } ,
                            {"RESILIENCE_LOCAL_SITE" ,  "Localsiteresilience" } ,
                            {"DEPENDANT_PRODUCT_NAME" ,  "Nameofproductsdependantonasset" } ,
                            {"BUSINESS_SERVICE_NAME" ,  "Technicalservicenames" } ,
                            {"CUSTOMER" ,  "Customer" } ,
                            {"ME_PRIVILEGED_ACCESS_LOGGING" ,  "Privilegedaccesslogging" } ,
                            {"HW_COMPONENT_NAME" ,  "Boardormodulenamecomponentname" } ,
                            {"HW_COMPONENT_SUBTYPE" ,  "Boardormoduletypecomponentsubtype" } ,
                            {"HW_COMPONENT_VERSION" ,  "Boardormoduletypecomponentversionnumber" } ,
                            {"EXPOSED EDGE FLAG" ,  "Exposededge" } ,
                            {"EXTERNAL FACING FLAG" ,  "Externallyfacingsystem" } ,
                            {"MANAGEMENT_PLANE" ,  "Managementplane" } ,
                            {"NETWORK_OVERSIGHT" ,  "Networkoversightfunction" } ,
                            {"PECN FLAG" ,  "Pecn" } ,
                            {"PECS FLAG" ,  "Pecs" } ,
                            {"SECURITY_CRITICAL_FUNCTION" ,  "Securitycriticalfunction" } ,
                            {"PRODUCT_IMPORTANCE" ,  "Productimportance" } ,
                            {"Business Critical" ,  "Critical" } ,
                            {"CRITICALITY_TYPE" ,  "Criticalitytype" } ,
                            {"ME_SERIAL_NUMBER" ,  "Serialnumber" } ,
                            {"HW_PART_NUMBER" ,  "Partnumber" } ,
                            {"PLANNED_ACTION_DESCRIPTION" ,  "Descriptionofplannedaction" } ,
                            {"IDENTIFIED_ACTION" ,  "Identifiedaction" } ,
                            {"LAST_UPGRADE_DATE" ,  "Lastupgradedate" } ,
                            {"ME_ENVIRONMENT" ,  "Prodorlab" } ,
                            {"ORGANISATION_NAME" ,  "Localmarketownership" } ,
                            {"BUDGET_ESTIMATED" ,  "Budgetestimated" } ,
                            {"BUNDLE_BUDGET" ,  "Bundlebudget" } ,
                            {"ASSURANCE_CALL" ,  "Assurancecall" } ,
                            {"COMMENTS_ON_PROJECT_STATUS" ,  "Commentonprojectstatus" } ,
                            {"PROJECT_END_DATE" ,  "Projectenddate" } ,
                            {"PROJECT_STATUS" ,  "Projectstatus" } ,
                            {"SERVICE_LEVEL" ,  "Servicelevel" } ,
                            {"LAST_PENTEST_DATE" ,  "Lastpentestdate" } ,
                            {"LAST_PENTEST_REFNO" ,  "Lastpentestrefno" } ,
                            {"PI_DATA" ,  "Pidata" } ,
                            {"ENCRYPTED_PI_DATA" ,  "Encryptedpidata" } ,
                            {"ME_PRODUCT_NAME" ,  "Productname" } ,
                            {"ME_SW_VERSION" ,  "Softwareversion" } ,
                            {"ME_SUB_DOMAIN_RESPONSIBLE" ,  "Subdomainresponsible" } ,

                    };

                        int vuIdColumnIndex = headers.FirstOrDefault(x => x.Value == "ME_SOURCE_ASSET_ID").Key;
                        int assetNameColumnIndex = headers.FirstOrDefault(x => x.Value == "ME_NAME").Key;

                        var tsrPassthroughList = _repositoryWrapper.TsrPassThroughRepository.FindAll().ToList();
                        var assetExistsInAssetPassthroughList = _repositoryWrapper.NonTemsNweAsPlannedPassThroughRepository.FindAll().ToList();


                        int readIndex = 1;
                        #region  1657 -TSR - Export the Glossary / Description on separate sheet
                        //var tsrDescription = _repositoryWrapper.GlossaryItemsRepository.FindByCondition(x => x.Istsrfield == true).FirstOrDefault();
                        //if (tsrDescription != null)
                        //    readIndex = 2;
                        #endregion

                        foreach (var row in rows.Skip(readIndex))
                        {
                            var tsr = new Tsrpassthrough();
                            tsr.Recordclassifier = (int)recordClassifier;
                            tsr.Nontemsvertical = recordClassifier == 2 ? (int?)nonTemsVertical : null;
                            string vuid = row.Cell(vuIdColumnIndex).GetString().Trim();
                            string assetName = row.Cell(assetNameColumnIndex).GetString().Trim();


                            var existingTsrPassthrough = tsrPassthroughList.Where(x => x.Assetname.Trim() == assetName.Trim()).ToList();
                            var matchedTsrPassthroughByVuid = existingTsrPassthrough.Where(x => x.Vodafoneuniqueidentifier == vuid).FirstOrDefault();

                            var existsInNonTemsEntity = new Nontemsnweasplannedpassthrough();

                            if (int.TryParse(vuid, out int parsedVuid))
                            {
                                existsInNonTemsEntity = assetExistsInAssetPassthroughList
                                    .FirstOrDefault(x =>
                                        x.Elementname.Trim() == assetName.Trim() &&
                                        x.Nontemsnweasplannedpassthroughid == parsedVuid
                                    );
                            }
                            else
                            {
                                existsInNonTemsEntity = null;
                            }
                            


                            var isRecordChanged = false;
                            if (string.IsNullOrEmpty(assetName))
                                errorList.Add(row.RowNumber(), ConstantValueFilter.AssetNameIsEmpty);
                            else
                            {
                                bool isTempsRecord = (matchedTsrPassthroughByVuid != null && existsInNonTemsEntity == null) ? true : false;
                                foreach (var colIndex in headers.Keys)
                                {
                                    var headerName = headers[colIndex];

                                    if (!propertyMap.ContainsKey(headerName)) continue;

                                    var propertyInfo = typeof(Tsrpassthrough).GetProperty(propertyMap[headerName]);
                                    if (propertyInfo == null) continue;

                                    var cell = row.Cell(colIndex);
                                    try
                                    {
                                        object? value = cell.GetString().Trim();
                                        var columnName = propertyMap[headerName];
                                        if (matchedTsrPassthroughByVuid != null && isRecordChanged == false)
                                        {
                                            var dbPropertyInfo = matchedTsrPassthroughByVuid.GetType().GetProperty(columnName);
                                            var dbvalue = dbPropertyInfo?.GetValue(matchedTsrPassthroughByVuid);

                                            if ((Convert.ToString( value) ?? "") != (Convert.ToString(dbvalue) ?? ""))
                                            {
                                                isRecordChanged = true;
                                            }
                                        }
                                        if (isTempsRecord)
                                        {
                                            nonTsrColumns.TryGetValue(headerName, out var tempsPassthroughColumn);
                                            if (tempsPassthroughColumn != null)
                                            {
                                                propertyInfo.SetValue(matchedTsrPassthroughByVuid, value);
                                            }

                                        }
                                        else
                                            propertyInfo.SetValue(tsr, value);
                                    }
                                    catch (Exception ex)
                                    {
                                        errorList.Add(row.RowNumber(), $"{assetName}, ");
                                    }
                                }

                                if (isTempsRecord)
                                {
                                    if (recordClassifier == 1 && isRecordChanged == true)
                                        updateTemsAssetList.Add(matchedTsrPassthroughByVuid);
                                    else if (recordClassifier == 2 && isRecordChanged == true)
                                        errorList.Add(row.RowNumber(), matchedTsrPassthroughByVuid.Assetname);
                                }
                                else if (existsInNonTemsEntity != null && (existingTsrPassthrough != null && existingTsrPassthrough.Count > 0))
                                {
                                    if (recordClassifier == 2 && matchedTsrPassthroughByVuid != null && isRecordChanged == true)
                                    {
                                        tsr.Tsrpassthroughid = matchedTsrPassthroughByVuid.Tsrpassthroughid;
                                        tsr.Vodafoneuniqueidentifier = matchedTsrPassthroughByVuid.Vodafoneuniqueidentifier;
                                        tsr.Assetid = matchedTsrPassthroughByVuid.Assetid;
                                        tsr.Deleted = false;
                                        updateNonTemsAssetList.Add(tsr);
                                    }
                                    else if (recordClassifier == 2 && matchedTsrPassthroughByVuid == null)
                                    {
                                        assetPassthroughEntity.Add(new Nontemsnweasplannedpassthrough { Elementname = assetName });
                                        insertList.Add(tsr);
                                    }
                                    else if ((recordClassifier == 1 && isRecordChanged == true) || matchedTsrPassthroughByVuid == null)
                                        errorList.Add(row.RowNumber(), assetName);

                                }
                                else
                                {

                                    if (recordClassifier == 2)
                                    {
                                        assetPassthroughEntity.Add(new Nontemsnweasplannedpassthrough { Elementname = assetName });
                                        insertList.Add(tsr);
                                    }
                                    else if (recordClassifier == 1)
                                    {
                                        errorList.Add(row.RowNumber(), tsr.Assetname);
                                    }

                                }


                            }
                        }
                    }

                }

                var batchIdentifier = recordClassifier == 1 ? "TEMS" : "NON-TEMS";
                var records = insertList.Count + updateNonTemsAssetList.Count + updateTemsAssetList.Count;
                _repositoryWrapper.TsrLogRepository.Create(new Tsrlogs
                {
                    Typeofoperation = ConstantValueFilter.ImportTypeofoperation,
                    Filename = formFile.FileName,
                    Totalrecord = records,
                    Processedrecord = 0,
                    Domain = batchIdentifier,
                    Status = ConstantValueFilter.TsrLogInProgressStatus,
                    Batchidentifier = batchIdentifier,

                });
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();

                try
                {
                    if (assetPassthroughEntity.Any())
                    {
                        _repositoryWrapper.NonTemsNweAsPlannedPassThroughRepository.BulkCreate(assetPassthroughEntity);
                    }

                    await _repositoryWrapper.SaveAsync();

                }
                catch (Exception ex)
                {
                    _logger.LogError(ResultMessages.errorLogTracke + "BulkImportOptimizedCode()  : " + ex);
                    errorList.Add(assetPassthroughEntity.Count, string.Join(',', assetPassthroughEntity.Select(x => x.Elementname).Distinct().ToList()));
                }

                try
                {
                    if (insertList.Any())
                    {//duplicate element id fetch
                        var nameToAQueue = assetPassthroughEntity
                                        .GroupBy(a => a.Elementname)
                                        .ToDictionary(g => g.Key, g => new Queue<Nontemsnweasplannedpassthrough>(g));

                        foreach (var b in insertList)
                        {
                            if (nameToAQueue.TryGetValue(b.Assetname, out var nontemsPkeyId) && nontemsPkeyId.Count > 0)
                            {
                                var fetchEntity = nontemsPkeyId.Dequeue();
                                b.Vodafoneuniqueidentifier = Convert.ToString(fetchEntity.Nontemsnweasplannedpassthroughid);
                                b.Assetid = Convert.ToString(fetchEntity.Nontemsnweasplannedpassthroughid);
                            }

                        }

                        _repositoryWrapper.TsrPassThroughRepository.BulkCreate(insertList);
                    }

                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ResultMessages.errorLogTracke + "BulkImportOptimizedCode()  : " + ex);
                    errorList.Add(insertList.Count, string.Join(',', insertList.Select(x => x.Assetname).Distinct().ToList()));
                }

                try
                {

                    if (updateTemsAssetList.Any())
                    {
                        _repositoryWrapper.TsrPassThroughRepository.BulkUpdate(updateTemsAssetList);
                    }

                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ResultMessages.errorLogTracke + "BulkImportOptimizedCode()  : " + ex);
                    errorList.Add(updateTemsAssetList.Count, string.Join(',', updateTemsAssetList.Select(x => x.Assetname).Distinct().ToList()));
                }

                try
                {
                    _repositoryWrapper.TsrPassThroughRepository.Detach();
                    if (updateNonTemsAssetList.Any())
                    {
                        _repositoryWrapper.TsrPassThroughRepository.BulkUpdate(updateNonTemsAssetList);
                    }

                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ResultMessages.errorLogTracke + "BulkImportOptimizedCode()  : " + ex);
                    errorList.Add(updateNonTemsAssetList.Count, string.Join(',', updateNonTemsAssetList.Select(x => x.Assetname).Distinct().ToList()));
                }

                _repositoryWrapper.TsrLogRepository.Create(new Tsrlogs
                {
                    Typeofoperation = ConstantValueFilter.ImportTypeofoperation,
                    Filename = formFile.FileName,
                    Totalrecord = records,
                    Processedrecord = errorList.Count > 0 ? errorList.Count : records,
                    Domain = batchIdentifier,
                    Status = errorList.Count > 0 ? ConstantValueFilter.TsrLogFailedStatus : ConstantValueFilter.TsrLogCompletedStatus,
                    Batchidentifier = batchIdentifier,

                });
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();

            }
            catch (Exception ex)
            {
                _logger.LogError(ResultMessages.errorLogTracke + "BulkImportOptimizedCode()  : " + ex);
            }



            return new ResultDto
            {
                Data = errorList,
                Info = errorList.Count > 0 ? ResultMessages.ImportFailed + string.Join(",", errorList.Values) : ResultMessages.ImportSuccess,
                Warning = errorList.Count > 0 ? false : true,
            };
        }
    }
}
