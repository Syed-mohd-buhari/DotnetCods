using AutoMapper;
using CAM.BusinessManager;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.MapConfiguration;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models.Cross;
using CAM.Repository;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using DocumentFormat.OpenXml.Spreadsheet;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OracleModels.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Imports
{
    public class LcmImport : BaseManager
    {
        private readonly ModelContext _modelContext;      
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ResourceKeyMasterManager _resourceKeyMasterManager;

        public string SheetName = "Export";
        public string IdColumn = "Lcm Engineering Index";
        public string excelName = "Lcm Engineering";
        public List<string> EditableColumnList = new List<string> { "Is Software Extended Support Offered By Vendor", "Is Hardware Extended Support Offered By Vendor",
                        "Operational Contact","SW Warranty End Date","SoftwareEndOfSupportContract","HardwareEndOfSupportContract","SoftwareSupportProvider",
            "SoftwareSupportType","HardwareSupportProvider","HW Sup. Type","SW In-Warranty","Previous Resource Key"};

        public LcmImport(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, IMapper mapper
            , IHttpContextAccessor contextAccessor, ResourceKeyMasterManager resourceKeyMasterManage, ILoggerManager logger) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _resourceKeyMasterManager = resourceKeyMasterManage;
        }

        public async Task<ResultDto> UpdateExcelColumn(List<Dictionary<string, string>> processData, List<Dictionary<string, string>> insertionData)
        {

            string errordescription = "", error = "", htmlbreak = " ", errorShortDescription = "";
            bool recordChanged = false;
            bool RecordUpdated = false;
            int norecordsUpdated = 0;

            try
            {
                foreach (Dictionary<string, string> item in processData)
                {
                    if (item[IdColumn] != null)
                    {
                        var model = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid.ToString() == item[IdColumn]).FirstOrDefault();
                        var entityExists = LCMEngineeringMapper.GetLcmEngineeringMapper(model);
                        var entity = LCMEngineeringMapper.SetLcmEngineeringMapper(entityExists);
                        recordChanged = false;
                        try
                        {
                            error = "";
                            if (entityExists != null)
                            {
                                if (item.ContainsKey("Is Software Extended Support Offered By Vendor"))
                                {
                                    try
                                    {
                                        string value = item["Is Software Extended Support Offered By Vendor"];
                                        if (value == null || value.ToLower().Trim() == "yes" || value.ToLower().Trim() == "no" || value.ToLower().Trim() == "unspecified")
                                        {

                                            if (entityExists.IsExtendedSupportOfferedByVendor != (value.ToLower().Trim() == "yes" ? true : (value.ToLower().Trim() == "no" ? false : null)))
                                            {
                                                entity.Isextendedsupportofferedbyvendor = value.ToLower().Trim() == "yes" ? true : (value.ToLower().Trim() == "no" ? false : null);
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + " Is Software Extended Support Offered By Vendor column should be Yes or No or Unspecified. Received value : " + value;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " Is Software Extended Support Offered By Vendor Value unable to updated ";
                                    }
                                }
                                if (item.ContainsKey("Is Hardware Extended Support Offered By Vendor"))
                                {
                                    try
                                    {
                                        string value = item["Is Hardware Extended Support Offered By Vendor"];

                                        if (value == null || value.ToLower().Trim() == "yes" || value.ToLower().Trim() == "no" || value.ToLower().Trim() == "unspecified")
                                        {
                                            if (entityExists.HwIsExtendedSupportOfferedByVendor != (value.ToLower().Trim() == "yes" ? true : (value.ToLower().Trim() == "no" ? false : null)))
                                            {
                                                entity.Hwisextendedsupportofferedbyvendor = value.ToLower().Trim() == "yes" ? true : (value.ToLower().Trim() == "no" ? false : null);
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + " Is Hardware Extended Support Offered By Vendor column should be Yes or No or Unspecified. Received value : " + value;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " Is Hardware Extended Support Offered By Vendor Value unable to updated ";
                                    }

                                }
                                if (item.ContainsKey("SoftwareSupportProvider"))
                                {
                                    try
                                    {
                                        string value = item["SoftwareSupportProvider"];
                                        if (entityExists.SoftwareSupportProvider != value)
                                        {
                                            entity.Softwaresupportprovider = value;
                                            recordChanged = true;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " SoftwareSupportProvider Value unable to updated ";

                                    }

                                }
                                if (item.ContainsKey("SoftwareSupportType"))
                                {
                                    try
                                    {
                                        string value = item["SoftwareSupportType"];
                                        if (entityExists.SoftwareSupportType != value)
                                        {
                                            entity.Softwaresupporttype = value;
                                            recordChanged = true;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " SoftwareSupportType Value unable to updated ";
                                    }

                                }
                                if (item.ContainsKey("HardwareSupportProvider"))
                                {
                                    try
                                    {
                                        string value = item["HardwareSupportProvider"];
                                        if (entityExists.HardwareSupportProvider != value)
                                        {
                                            entity.Hardwaresupportprovider = value;
                                            recordChanged = true;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " HardwareSupportProvider Value unable to updated ";
                                    }
                                }
                                if (item.ContainsKey("HW Sup. Type"))
                                {
                                    try
                                    {
                                        string value = item["HW Sup. Type"];
                                        if (entityExists.HardwareSupportType != value)
                                        {
                                            entity.Hardwaresupporttype = value;
                                            recordChanged = true;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " HW Sup. Type Value unable to updated ";
                                    }

                                }
                                if (item.ContainsKey("Operational Contact"))
                                {
                                    try
                                    {
                                        string value = item["Operational Contact"];
                                        if (value != null && value != "")
                                        {
                                            string[] operationalContracts = value.Split('|');
                                            foreach (string oc in operationalContracts)
                                            {
                                                var existOc = _repositoryWrapper.OperationalContract.FindByCondition(x => x.Description.Trim() == oc.Trim()).FirstOrDefault();
                                                if (existOc != null)
                                                {
                                                    var existlcmOc = _repositoryWrapper.LCMOperationalContracts.FindByCondition(x => x.Lcmid.ToString() == item[IdColumn] && x.Operationalcontractid == existOc.Id).FirstOrDefault();
                                                    if (existlcmOc != null)
                                                    {
                                                        //rectord already exist do nothing
                                                    }
                                                    else
                                                    {
                                                        var lcmop = _repositoryWrapper.LCMOperationalContracts.FindByCondition(x => x.Lcmid.ToString() == item[IdColumn]).FirstOrDefault();

                                                        var lcmoc = new OracleModels.DBModels.Lcmoperationalcontracts
                                                        {
                                                            Lcmid = Convert.ToInt32(item[IdColumn]),
                                                            Operationalcontractid = existOc.Id,
                                                        };
                                                        recordChanged = true;
                                                        _repositoryWrapper.LCMOperationalContracts.Create(lcmoc);
                                                    }

                                                }
                                                else
                                                {
                                                    error += htmlbreak + " Operational Contact desnot exist in data base. Received value : " + value;
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " Operational Contact Value unable to updated ";

                                    }


                                }
                                if (item.ContainsKey("SoftwareEndOfSupportContract"))
                                {
                                    string value = item["SoftwareEndOfSupportContract"];
                                    var dateValue = ConvertDateValue(value);
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(dateValue.ToString()))
                                        {
                                            if (entityExists.SoftwareEndOfSupportContract != dateValue)
                                            {
                                                entity.Softwareendofsupportcontract = dateValue;
                                                recordChanged = true;
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.SoftwareEndOfSupportContract != null)
                                            {
                                                entity.Softwareendofsupportcontract = null;
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + " SoftwareEndOfSupportContract column should be Date and Time. Received value :" + value;
                                        }

                                    }
                                    catch
                                    {
                                        error += htmlbreak + " SoftwareEndOfSupportContract Unable to convert date. Received value : " + value;
                                    }


                                }
                                if (item.ContainsKey("HardwareEndOfSupportContract"))
                                {
                                    string value = item["HardwareEndOfSupportContract"];
                                    var dateValue = ConvertDateValue(value);
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(dateValue.ToString()))

                                        {
                                            if (entityExists.HardwareEndOfSupportContract != dateValue)
                                            {
                                                entity.Hardwareendofsupportcontract = dateValue;
                                                recordChanged = true;
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.HardwareEndOfSupportContract != null)
                                            {
                                                entity.Hardwareendofsupportcontract = null;
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + " HardwareEndOfSupportContract column should be Date and Time. Received value : " + value;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " HardwareEndOfSupportContract Unable to convert date. Received value : " + value;
                                    }


                                }
                                if (item.ContainsKey("SW Warranty End Date"))
                                {
                                    string value = item["SW Warranty End Date"];
                                    var dateValue = ConvertDateValue(value);
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(dateValue.ToString()))
                                        {
                                            if (entityExists.SoftwareEndOfWarrantyDate != dateValue)
                                            {
                                                entity.Warranty = true;
                                                entity.Softwareendofwarrantydate = dateValue;

                                                recordChanged = true;
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.SoftwareEndOfWarrantyDate != null)
                                            {
                                                entity.Warranty = false;
                                                entity.Softwareendofwarrantydate = null;
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + " SW Warranty End Date column should be Date and Time. Received value : " + value;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " SW Warranty End Date Unable to convert date. Received value : " + value;
                                    }


                                }
                                if (item.ContainsKey("SW In-Warranty"))
                                {
                                    try
                                    {
                                        string value = item["SW In-Warranty"];
                                        if (value != null && value != "")
                                        {

                                            if (value.ToLower().Trim() == "yes" || value.ToLower().Trim() == "no")
                                            {

                                                if (entityExists.Warranty != (value.ToLower().Trim() == "yes" ? true : false))
                                                {
                                                    entity.Warranty = value.ToLower().Trim() == "yes" ? true : false;
                                                    recordChanged = true;
                                                }
                                            }
                                            else
                                            {
                                                error += htmlbreak + "SW In-Warranty Value should be Yes or No. Received value : " + value;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + "SW In-Warranty Value should not be null it should be Yes or No. Received value : " + value;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + "SW In-Warranty Value unable to updated ";
                                    }
                                }
                                if (item.ContainsKey("Previous Resource Key"))
                                {
                                    try
                                    {
                                        string value = item["Previous Resource Key"];
                                        if (entityExists.ResourceKey != null)
                                        {
                                            if (entityExists.PreviousResourceKey != value)
                                            {
                                                var Dcf = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == entityExists.DesignComponentId).FirstOrDefault();

                                                if (Dcf.Designcomponentfamilyid != null)
                                                {
                                                    var dcids = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentfamilyid == Dcf.Designcomponentfamilyid.Value).Select(x => x.Designcomponentid).ToList();

                                                    if (dcids != null)
                                                    {
                                                        foreach (var dcid in dcids)
                                                        {
                                                            var dcflcExists = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(x => x.Opcoid == entityExists.OpCoId && x.Dcfid == Dcf.Designcomponentfamilyid
                                                                                 && x.Dcid == dcid).ToList();
                                                            if (dcflcExists != null)
                                                            {
                                                                foreach (var dcflcExist in dcflcExists)
                                                                {
                                                                    string[] splitDcflcPreivous = { };
                                                                    string[] splitValue = { };
                                                                    if (dcflcExist.Previousresourcekey != null)
                                                                    {
                                                                        splitDcflcPreivous = dcflcExist.Previousresourcekey.Split("_");
                                                                    }
                                                                    if(value != null)
                                                                    {
                                                                        splitValue = value.Split("_");
                                                                    }
                                                                    if(splitDcflcPreivous.Length >1 && splitValue.Length > 1)
                                                                    {
                                                                        dcflcExist.Previousresourcekey = splitValue[0] + "_" + splitDcflcPreivous[1];
                                                                    }
                                                                    else if(splitValue.Length <= 1)
                                                                    {
                                                                        dcflcExist.Previousresourcekey = value;
                                                                    }

                                                                    _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Update(dcflcExist);
                                                                }
                                                            }


                                                        }
                                                    }
                                                }
                                                entity.Previousresourcekey = value;
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + "Could not update Previous Resource Key without resource key ";
                                        }
                                                                             
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " Previous Resource Key Value unable to updated ";

                                    }

                                }

                            }
                            else
                            {
                                error += htmlbreak + " Database Doesn't Contain : " + IdColumn + "column " + "value : " + item[IdColumn];
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
                                _repositoryWrapper.Lcmengineering.Update(entity);
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
                errorShortDescription = "No Records Updated in "+ excelName +", Please update the value and try to upload again...";
            }
            return new ResultDto
            {

                Info = errorShortDescription.Length > 0 ? errorShortDescription : norecordsUpdated.ToString(),
                Warning = errorShortDescription.Length > 0 ? false : true,
            };
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
