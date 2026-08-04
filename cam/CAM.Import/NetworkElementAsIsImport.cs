using AutoMapper;
using CAM.BusinessManager;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElementAsIs;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System.Globalization;


namespace CAM.Imports
{
    public class NetworkElementAsIsImport : BaseManager
    {
       public readonly CommonManager _common;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public string SheetName = "Export";
        public string IdColumn = "NetworkElementAsIsId";
        public string excelName = "NetworkElementAsIs";
        public string opCo = "OpCo";
        public string elementName = "ElementDeploymentName";

        public List<string> EditableColumnList = new List<string> { "HardwareInstallDate", "DataAcquisitionDateValue" };

        public NetworkElementAsIsImport(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper,
             IHttpContextAccessor contextAccessor, CommonManager common) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;            
            _common = common;         
        }

        public async Task<ResultDto> UpdateExcelColumn(List<Dictionary<string, string>> processData, List<Dictionary<string, string>> insertionDatas)
        {

            string errordescription = "", error = "", htmlbreak = " ", errorShortDescription = "";
            bool recordChanged = false;
            bool RecordUpdated = false;
            int norecordsUpdated = 0;

            try
            {
                foreach (Dictionary<string, string> insertionData in insertionDatas)
                {
                    if (insertionData[IdColumn] != null)
                    {
                        foreach (Dictionary<string, string> item in processData)
                        {
                            if (item[IdColumn] != null)
                            {
                                var model = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Networkelementasisid.ToString() == item[IdColumn]).FirstOrDefault();
                                var entityExists = CAM.Entities.Mappers.Entity.NetworkElementAsIsMapper.Get(model);
                                var entity = CAM.Entities.Mappers.Entity.NetworkElementAsIsMapper.Set(entityExists);
                                recordChanged = false;
                                try
                                {
                                    error = "";
                                    if (entityExists != null)
                                    {
                                        if (item.ContainsKey("HardwareInstallDate"))
                                        {
                                            string value = item["HardwareInstallDate"];
                                            var dateValue = ConvertDateValue(value);
                                            try
                                            {
                                                if (!string.IsNullOrEmpty(dateValue.ToString()))
                                                {
                                                    if (entityExists.HardwareInstallDate != dateValue)
                                                    {
                                                        entity.Hardwareinstalldate = dateValue;
                                                        recordChanged = true;
                                                    }
                                                }
                                                else if (value == null)
                                                {
                                                    if (entityExists.HardwareInstallDate != null)
                                                    {
                                                        entity.Hardwareinstalldate = null;
                                                        recordChanged = true;
                                                    }
                                                }
                                                else
                                                {
                                                    error += htmlbreak + " Hardware Install Date column should be Date and Time. Received value : " + value;
                                                }
                                            }
                                            catch
                                            {
                                                error += htmlbreak + " Hardware Install Date Unable to convert date. Received value : " + value;
                                            }
                                        }
                                        if (item.ContainsKey("DataAcquisitionDateValue"))
                                        {
                                            string value = item["DataAcquisitionDateValue"];
                                            var dateValue = ConvertDateValue(value);
                                            var dbValue = ConvertDateValue(entity.Dataacquisitiondate.Value.ToString("dd/MM/yyyy"));
                                            
                                            
                                            try
                                            {
                                                if (!string.IsNullOrEmpty(dateValue.ToString()))
                                                {
                                                    if (dbValue != dateValue)
                                                    {
                                                        entity.Dataacquisitiondate = dateValue;
                                                        recordChanged = true;
                                                    }
                                                }
                                                else if (value == null)
                                                {
                                                    if (entityExists.DataAcquisitionDate != null)
                                                    {
                                                        entity.Dataacquisitiondate = null;
                                                        recordChanged = true;
                                                    }
                                                }
                                                else
                                                {
                                                    error += htmlbreak + " DataAcquisition Date column should be Date and Time. Received value : " + value;
                                                }
                                            }
                                            catch
                                            {
                                                error += htmlbreak + " DataAcquisition Date Unable to convert date. Received value : " + value;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        error += htmlbreak + " DB Doesn't Contain : " + IdColumn + "column " + "value : " + item[IdColumn];
                                    }


                                }
                                catch (Exception ex)
                                {
                                    errordescription += htmlbreak + " Unhandled Error " + ex.Message + " in updating record : " + IdColumn + " column : " + item[IdColumn];
                                }
                                try
                                {


                                    if (error == "" && recordChanged)
                                    {
                                        _repositoryWrapper.NetworkElementAsIs.Update(entity);
                                        _repositoryWrapper.Save();
                                        RecordUpdated = true;
                                        norecordsUpdated++;
                                    }
                                    else if (error == "" && !recordChanged)
                                    {
                                        //Nothing to Update
                                    }
                                    else
                                    {
                                        errordescription += htmlbreak + "Issue in excel record with : " + IdColumn + " value : " + item[IdColumn] + htmlbreak + "Error message : " + error;
                                        if (errorShortDescription != "")
                                        {
                                            errorShortDescription += ", ";
                                        }
                                        errorShortDescription += item[IdColumn];
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errordescription += ex.Message;
                                }
                            }                           
                            else
                            {
                                // The data doesn't deleted properly in excel 
                            }
                        }
                    }
                    else if (insertionData[IdColumn] == null && insertionData[opCo] != null && insertionData[elementName] != null)
                    {

                        var Entity = MappeExcelDataToGrid(insertionData);
                        if (Entity.Networkelementasis == null)
                        {
                            var model = NetworkElementAsIsMapper.Set(Entity.NetworkElementAsIs);
                            if (model != null)
                            {
                                if (!Entity.IsUpdateRecords && Entity.Error.IsNullOrEmpty())
                                {
                                    _repositoryWrapper.NetworkElementAsIs.Create(model);
                                    RecordUpdated = true;
                                    norecordsUpdated++;
                                }
                                else
                                {
                                    if (Entity.IsUpdateRecords)
                                    {
                                        if (Entity.Error.IsNullOrEmpty())
                                        {
                                            _repositoryWrapper.NetworkElementAsIs.Update(model);
                                            RecordUpdated = true;
                                            norecordsUpdated++;
                                        }
                                        else
                                        {
                                            errorShortDescription = Entity.Error.ToString();
                                        }
                                    }
                                    else
                                    {
                                        // no records updated
                                    }
                                }
                                _repositoryWrapper.Save();
                                await _repositoryWrapper.ClearTracker();


                            }
                        }
                        else if(Entity.Networkelementasis != null)
                        {
                            if (Entity.IsUpdateRecords)
                            {
                                if (Entity.Error.IsNullOrEmpty())
                                {
                                    _repositoryWrapper.NetworkElementAsIs.Update(Entity.Networkelementasis);
                                    RecordUpdated = true;
                                    norecordsUpdated++;
                                }
                                else
                                {
                                    errorShortDescription = Entity.Error.ToString();
                                }
                            }
                            else
                            {
                                // no records updated
                            }
                        }
                        else
                        {
                            if (!Entity.Error.IsNullOrEmpty())
                            {
                                errorShortDescription = Entity.Error.ToString();
                            }
                            else
                            {
                                errorShortDescription = $"OpCo = '{insertionData[opCo]}' and Element Name = '{insertionData[elementName]}'";

                            }
                        }
                    }
                }
            }



            catch (Exception ex)
            {
                return new ResultDto
                {
                    Info = errordescription + "\n" + ex.Message,
                    Warning = false
                };
            }
            if (errorShortDescription.Length > 0)
            {
                errorShortDescription = IdColumn + "  : " + errorShortDescription + " records not updated, please update the correct value in the excel and try to upload again...";
            }
            else if (errorShortDescription.Length == 0 && !RecordUpdated)
            {
                errorShortDescription = "No Records Updated in " + excelName + ", Please update the value and try to upload again...";
            }
            return new ResultDto
            {

                Info = errorShortDescription.Length > 0 ? errorShortDescription : norecordsUpdated.ToString(),
                Warning = errorShortDescription.Length > 0 ? false : true,
            };
        }

        public NetworkElementAsisImport MappeExcelDataToGrid(Dictionary<string, string> insertionData)
        {
            NetworkElementAsisImport result = new NetworkElementAsisImport();

            var opCoId = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.ToLower().Trim() == insertionData["OpCo"].ToLower().Trim()).Select(x=>x.Opcoid).FirstOrDefault();

            var assetDetails = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == opCoId && x.Elementname.ToLower().Trim() == insertionData["ElementDeploymentName"].ToLower().Trim())
                .Include(x=>x.Designcomponent).ThenInclude(x=>x.Systemtype).FirstOrDefault();

            var LocationId = _repositoryWrapper.Location.FindByCondition(x =>x.Location.ToLower().Trim() ==
            (insertionData.ContainsKey(ConstantValueFilter.Location)?insertionData[ConstantValueFilter.Location] == null? insertionData[ConstantValueFilter.Location] : insertionData[ConstantValueFilter.Location].ToLower() :null))
                .Select(x=>x.Locationid).FirstOrDefault();
          


            var existingAsis = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Opcoid == opCoId && x.Elementdeploymentname == insertionData[ConstantValueFilter.ElementDeploymentName]).FirstOrDefault();
            if (existingAsis == null && assetDetails != null)
            {
               result.NetworkElementAsIs = InsertRecords(insertionData, assetDetails, opCoId, LocationId);
            }
            else
            {
                if (assetDetails != null)
                {
                    var records = new NetworkElementAsisUpdateDto();
                    records = UpdatedRecords(existingAsis, insertionData, assetDetails, opCoId, LocationId);

                    result.Networkelementasis = records.NetworkElementAsIs;
                    result.IsUpdateRecords = records.IsUpdateRecords;
                }
                else
                {
                    result.Error = "There is no Relationship on Asset page";
                }
            }

            return result;
           
        }
        public NetworkElementAsIs InsertRecords(Dictionary<string, string> insertionData, Networkelementsasplanned? assetDetails, short opCoId, short locationId)
        {
            var grid = new NetworkElementAsIs();
            try
            {
                if (opCoId > 0 && assetDetails != null && assetDetails.Designcomponent.Systemtypeid > 0 && assetDetails.Orgeqpmanufacturerid > 0)
                {
                    grid.OpCoId = opCoId;
                    grid.ElementDeploymentName = insertionData.ContainsKey(ConstantValueFilter.ElementDeploymentName) ? insertionData[ConstantValueFilter.ElementDeploymentName] : null;
                    grid.NodeType = insertionData.ContainsKey(ConstantValueFilter.NodeType) ? insertionData[ConstantValueFilter.NodeType] : null;
                    grid.NetworkElementAsPlannedId = assetDetails.Networkelementasplannedid;
                    grid.SystemTypeId = assetDetails.Designcomponent.Systemtypeid;
                    grid.OriginalEquipmentManufacturerId = (short)assetDetails.Orgeqpmanufacturerid;
                    grid.LocationId = locationId > 0 ? locationId : (short)assetDetails.Locationid;
                    grid.HardwareInstallDate = insertionData.ContainsKey(ConstantValueFilter.HardwareInstallDate) ? _common.ConvertDateValue(insertionData[ConstantValueFilter.HardwareInstallDate]) : null;
                    grid.ManualOverride = insertionData.ContainsKey(ConstantValueFilter.ManualOverride) ? insertionData[ConstantValueFilter.ManualOverride] != null ? (insertionData[ConstantValueFilter.ManualOverride] == "1" ? true : false) : false : false;
                    grid.PlatformType = insertionData.ContainsKey(ConstantValueFilter.Platform) ? insertionData[ConstantValueFilter.Platform] : null;
                    grid.HardwareType = insertionData.ContainsKey(ConstantValueFilter.HardwareType) ? insertionData[ConstantValueFilter.HardwareType] : null;
                    grid.PatchDetails = insertionData.ContainsKey(ConstantValueFilter.PatchDetails) ? insertionData[ConstantValueFilter.PatchDetails] : null;
                    grid.ElementManagerExportFileFormat = insertionData.ContainsKey(ConstantValueFilter.ElementManagerExportFileFormat) ? insertionData[ConstantValueFilter.ElementManagerExportFileFormat] : null;
                    grid.SoftwareReleaseInformation = insertionData.ContainsKey(ConstantValueFilter.SoftwareReleaseInformation) ? insertionData[ConstantValueFilter.SoftwareReleaseInformation] : null;
                    grid.SoftwareProductNumber = insertionData.ContainsKey(ConstantValueFilter.SoftwareProductNumber) ? insertionData[ConstantValueFilter.SoftwareProductNumber] : null;
                    grid.SoftwareProductionDate = insertionData.ContainsKey(ConstantValueFilter.SoftwareProductionDateValue) ? _common.ConvertDateValue(insertionData[ConstantValueFilter.SoftwareProductionDateValue]) : null;
                    grid.SoftwareInstallDate = insertionData.ContainsKey(ConstantValueFilter.SoftwareInstallDateValue) ? _common.ConvertDateValue(insertionData[ConstantValueFilter.SoftwareInstallDateValue]) : null;
                    grid.HardwareAcquisition = insertionData.ContainsKey(ConstantValueFilter.HardwareAcquisition) ? insertionData[ConstantValueFilter.HardwareAcquisition] : null;
                    grid.DataAcquisitionDate = insertionData.ContainsKey(ConstantValueFilter.DataAcquisitionDateValue) ? _common.ConvertDateValue(insertionData[ConstantValueFilter.DataAcquisitionDateValue]) : null;
                    grid.DataAcquisitionMethod = ConstantValueFilter.BulkImportData;
                    grid.ElementManager = insertionData.ContainsKey(ConstantValueFilter.ElementManager) ? insertionData[ConstantValueFilter.ElementManager] : null;
                }               
                return grid;
            }
            catch 
            {
                return grid;
            }

        }
        public NetworkElementAsisUpdateDto UpdatedRecords(Networkelementsasis asisEntity, Dictionary<string, string> insertionData,Networkelementsasplanned? assetDetails, short opCoId , short locationId)
        {
            var result = new NetworkElementAsisUpdateDto();
            var grid = new NetworkElementAsIs();
            bool isUpdate = false;
            try
            {
                #region 
                if (opCoId > 0 && assetDetails != null && assetDetails.Designcomponent.Systemtypeid > 0 && assetDetails.Orgeqpmanufacturerid > 0)
                {
                    if(asisEntity.Networkelementasisid != (long)(insertionData.ContainsKey(ConstantValueFilter.NetworkElementAsIsId) ? Convert.ToInt64(insertionData[ConstantValueFilter.NetworkElementAsIsId]) : 0))
                    {
                        asisEntity.Networkelementasisid = asisEntity.Networkelementasisid;
                    }                                    
                    if (asisEntity.Nodetype != (insertionData.ContainsKey(ConstantValueFilter.NodeType) ? insertionData[ConstantValueFilter.NodeType] : null))
                    {
                        asisEntity.Nodetype = insertionData.ContainsKey(ConstantValueFilter.NodeType) ? insertionData[ConstantValueFilter.NodeType] : null;
                        isUpdate = true;
                    }                   
                    if (ConvertDateValue(asisEntity.Hardwareinstalldate != null? asisEntity.Hardwareinstalldate.Value.ToString("MM/dd/yyyy") :asisEntity.Hardwareinstalldate.ToString()) != (insertionData.ContainsKey(ConstantValueFilter.HardwareInstallDate) ? _common.ConvertDateValue(insertionData[ConstantValueFilter.HardwareInstallDate]) : null))
                    {
                        asisEntity.Hardwareinstalldate = insertionData.ContainsKey(ConstantValueFilter.HardwareInstallDate) ? _common.ConvertDateValue(insertionData[ConstantValueFilter.HardwareInstallDate]) : null;
                        isUpdate = true;
                    }
                    if (asisEntity.Manualoverride != (insertionData.ContainsKey(ConstantValueFilter.ManualOverride) ? insertionData[ConstantValueFilter.ManualOverride] != null ? (insertionData[ConstantValueFilter.ManualOverride] == "1" ? true : false) : false : false))
                    {
                        asisEntity.Manualoverride = insertionData.ContainsKey(ConstantValueFilter.ManualOverride) ? insertionData[ConstantValueFilter.ManualOverride] != null ? (insertionData[ConstantValueFilter.ManualOverride] == "1" ? true : false) : false : false;
                        isUpdate = true;
                    }
                    if (asisEntity.Platformtype != (insertionData.ContainsKey(ConstantValueFilter.Platform) ? insertionData[ConstantValueFilter.Platform] : null))
                    {
                        asisEntity.Platformtype = insertionData.ContainsKey(ConstantValueFilter.Platform) ? insertionData[ConstantValueFilter.Platform] : null;
                        isUpdate = true;
                    }
                    if (asisEntity.Hardwaretype != (insertionData.ContainsKey(ConstantValueFilter.HardwareType) ? insertionData[ConstantValueFilter.HardwareType] : null))
                    {
                        asisEntity.Hardwaretype = insertionData.ContainsKey(ConstantValueFilter.HardwareType) ? insertionData[ConstantValueFilter.HardwareType] : null;
                        isUpdate = true;
                    }
                    if (asisEntity.Patchdetails != (insertionData.ContainsKey(ConstantValueFilter.PatchDetails) ? insertionData[ConstantValueFilter.PatchDetails] : null))
                    {
                        asisEntity.Patchdetails = insertionData.ContainsKey(ConstantValueFilter.PatchDetails) ? insertionData[ConstantValueFilter.PatchDetails] : null;
                        isUpdate = true;
                    }
                    if (asisEntity.Elementmanagerexportfileformat != (insertionData.ContainsKey(ConstantValueFilter.ElementManagerExportFileFormat) ? insertionData[ConstantValueFilter.ElementManagerExportFileFormat] : null))
                    {
                        asisEntity.Elementmanagerexportfileformat = insertionData.ContainsKey(ConstantValueFilter.ElementManagerExportFileFormat) ? insertionData[ConstantValueFilter.ElementManagerExportFileFormat] : null;
                        isUpdate = true;
                    }
                    if (asisEntity.Softwarereleaseinformation != (insertionData.ContainsKey(ConstantValueFilter.SoftwareReleaseInformation) ? insertionData[ConstantValueFilter.SoftwareReleaseInformation] : null))
                    {
                        asisEntity.Softwarereleaseinformation = insertionData.ContainsKey(ConstantValueFilter.SoftwareReleaseInformation) ? insertionData[ConstantValueFilter.SoftwareReleaseInformation] : null;
                        isUpdate = true;
                    }
                    if (asisEntity.Softwareproductnumber != (insertionData.ContainsKey(ConstantValueFilter.SoftwareProductNumber) ? insertionData[ConstantValueFilter.SoftwareProductNumber] : null))
                    {
                        asisEntity.Softwareproductnumber = insertionData.ContainsKey(ConstantValueFilter.SoftwareProductNumber) ? insertionData[ConstantValueFilter.SoftwareProductNumber] : null;
                        isUpdate = true;
                    }
                    if (ConvertDateValue(asisEntity.Softwareproductiondate != null? asisEntity.Softwareproductiondate.Value.ToString("MM/dd/yyyy"): asisEntity.Softwareproductiondate.ToString()) != (insertionData.ContainsKey(ConstantValueFilter.SoftwareProductionDateValue) ? _common.ConvertDateValue(insertionData[ConstantValueFilter.SoftwareProductionDateValue]) : null))
                    {
                        asisEntity.Softwareproductiondate = insertionData.ContainsKey(ConstantValueFilter.SoftwareProductionDateValue) ? _common.ConvertDateValue(insertionData[ConstantValueFilter.SoftwareProductionDateValue]) : null;
                        isUpdate = true;
                    }
                    if (ConvertDateValue(asisEntity.Softwareinstalldate != null?asisEntity.Softwareinstalldate.Value.ToString("MM/dd/yyyy") : asisEntity.Softwareinstalldate.ToString()) != (insertionData.ContainsKey(ConstantValueFilter.SoftwareInstallDateValue) ? _common.ConvertDateValue(insertionData[ConstantValueFilter.SoftwareInstallDateValue]) : null))
                    {
                        asisEntity.Softwareinstalldate = insertionData.ContainsKey(ConstantValueFilter.SoftwareInstallDateValue) ? _common.ConvertDateValue(insertionData[ConstantValueFilter.SoftwareInstallDateValue]) : null;
                        isUpdate = true;
                    }
                    if (asisEntity.Hardwareacquisition != (insertionData.ContainsKey(ConstantValueFilter.HardwareAcquisition) ? insertionData[ConstantValueFilter.HardwareAcquisition] : null))
                    {
                        asisEntity.Hardwareacquisition = insertionData.ContainsKey(ConstantValueFilter.HardwareAcquisition) ? insertionData[ConstantValueFilter.HardwareAcquisition] : null;
                        isUpdate = true;
                    }
                    if (ConvertDateValue(asisEntity.Dataacquisitiondate != null? asisEntity.Dataacquisitiondate.Value.ToString("MM/dd/yyyy") : asisEntity.Dataacquisitiondate.ToString()) != (insertionData.ContainsKey(ConstantValueFilter.DataAcquisitionDateValue) ? _common.ConvertDateValue(insertionData[ConstantValueFilter.DataAcquisitionDateValue]) : null))
                    {
                        asisEntity.Dataacquisitiondate = insertionData.ContainsKey(ConstantValueFilter.DataAcquisitionDateValue) ? _common.ConvertDateValue(insertionData[ConstantValueFilter.DataAcquisitionDateValue]) : null;
                        isUpdate = true;
                    }                  
                    if (asisEntity.Elementmanager != (insertionData.ContainsKey(ConstantValueFilter.ElementManager) ? insertionData[ConstantValueFilter.ElementManager] : null))
                    {
                        asisEntity.Elementmanager = insertionData.ContainsKey(ConstantValueFilter.ElementManager) ? insertionData[ConstantValueFilter.ElementManager] : null;
                        isUpdate = true;
                    }
                    if (isUpdate)
                    {
                        asisEntity.Dataacquisitionmethod = ConstantValueFilter.BulkImportData;
                        isUpdate = true;
                    }
                                    
                    result.NetworkElementAsIs = asisEntity;
                    result.IsUpdateRecords = isUpdate;
                    #endregion
                }

                return result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public DateTime? ConvertDateValue(string dateValue)
        {
            DateTime? date = null;
            if (dateValue != null)
            {
                dateValue = dateValue.Split(' ')[0];
                dateValue = dateValue.Replace("-", "/");
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
    }
}
