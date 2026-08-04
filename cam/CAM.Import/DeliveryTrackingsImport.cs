using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Contracts;
using Microsoft.AspNetCore.Http;
using OracleModels.DBContext;
using CAM.BusinessManager;
using CAM.DataTransferObjects;
using System.Globalization;
using CAM.Entities.Mappers.Lookup;
using CAM.Enum;
using CAM.Entities.Models;
using CAM.BusinessManager.CommonUtilities;

namespace CAM.Imports
{
    public class DeliveryTrackingsImport : BaseManager
    {
        private readonly ModelContext _modelContext;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly CommonManager _commonManager;

        public string SheetName = "Export";
        public string IdColumn = "Delivery Tracking Index";
        public string excelName = "Delivery Tracking";

        public List<string> EditableColumnList = new List<string> { "MS1: Event Type", /*"MS1: BaseLine Date",*/ "MS1: Latest Planning Date", "MS1: Status",
            "MS2: Event Type", /*"MS2: BaseLine Date",*/ "MS2: Latest Planning Date", "MS2: Status", "MS3: Event Type", /*"MS3: BaseLine Date", */"MS3: Latest Planning Date",
            "MS3: Status", "MS4: Event Type", /*"MS4: BaseLine Date",*/ "MS4: Latest Planning Date", "MS4: Status", "PPM Import Date", "Notes 1", "Notes 2", "PPM ID","Start Date","End Date","Progress","Plan Description"};

        public DeliveryTrackingsImport(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, IMapper mapper, CommonManager commonManager
            , IHttpContextAccessor contextAccessor, ILoggerManager logger) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _commonManager = commonManager;

        }

        public async Task<ResultDto> UpdateExcelColumn(List<Dictionary<string, string>> processData, List<Dictionary<string, string>> insertionData)
        {
            //Ticket 1248 - Delivery Tracking Import : Separate the error message for deleted and updated records
            string errordescription = "", error = "", htmlbreak = " ", errorShortDescription = "",deletedRecordsError ="" ;
            bool recordChanged = false, RecordUpdated = false, RecordDeleted = false, MsstatusError = false;
            int norecordsUpdated = 0 , norecordsDeleted = 0; 
            var msStatusErrorDescription = new List<string>();    
            try
            {
                foreach (Dictionary<string, string> item in processData)
                {
                    if (item[IdColumn] != null)
                    {
                        
                        var model = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Id.ToString() == item[IdColumn],true).FirstOrDefault();

                        var plannedActivity = await Task.Run(() => _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == model.Plannedactivityid).FirstOrDefault());


                        if ((model != null  && model.Deleted == true) || model == null)
                        {
                            RecordDeleted = true;
                            norecordsDeleted++;
                        }
                        else
                        {
                            RecordDeleted = false;
                            norecordsDeleted = 0;
                        }

                        var entityExists = (RecordDeleted == false) ? DeliveryTrackingMapper.GetDeliveryTrackingMapper(model) : null;
                        var entity = (RecordDeleted == false) ? DeliveryTrackingMapper.SetDeliveryTrackingMapper(entityExists) : null;
                        recordChanged = false;
                        try
                        {
                            error = "";
                            if (entityExists != null)
                            {
                                if (item.ContainsKey("MS1: Event Type"))
                                {
                                    string value = item["MS1: Event Type"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        if (!string.IsNullOrEmpty(val.ToString()))
                                        {
                                            if (val.Length >= 255)
                                            {
                                                error += htmlbreak + " MS1: Event Type column maximum 255 char. Received value : " + value.Length;
                                            }                                           

                                            else if (entityExists.MS1EventType != val)
                                            {
                                                entity.Ms1eventtype = val;
                                                recordChanged = true;
                                            }                                                                            
                                            
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.MS1EventType != null)
                                            {
                                                entity.Ms1eventtype = null;                                               
                                                recordChanged = true;
                                            }
                                        }


                                    }
                                    catch
                                    {
                                        error += htmlbreak + " MS1: Event Type column Unable to updated : " + value;
                                    }
                                }
                                #region
                                //if (item.ContainsKey("MS1: BaseLine Date"))
                                //{
                                //    string value = item["MS1: BaseLine Date"];
                                //    var dateValue = ConvertDateValue(value);
                                //    try
                                //    {                                      
                                //        if (!string.IsNullOrEmpty(dateValue.ToString()))
                                //        {
                                //            if (plannedActivity != null && (!string.IsNullOrEmpty(plannedActivity.Startdate.ToString())))
                                //            {
                                //                if ((dateValue.Value.Date >= plannedActivity.Startdate.Value.Date) && (dateValue.Value.Date <= plannedActivity.Plannedcompletion.Value.Date))
                                //                {
                                //                    if (entityExists.MS1BaseLineDate != dateValue)
                                //                    {
                                //                        entity.Ms1baselinedate = dateValue;
                                //                        recordChanged = true;
                                //                    }
                                //                }
                                //                else
                                //                {
                                //                    MsstatusError = true;
                                //                    msStatusErrorDescription.Add(item[IdColumn]);
                                //                    error += htmlbreak + " MS1: BaseLine Date column should be align with planned activity start date. Received value : " + value;
                                //                }
                                //            }
                                //            else if (entityExists.MS1BaseLineDate != dateValue)
                                //            {
                                //                entity.Ms1baselinedate = dateValue;
                                //                recordChanged = true;

                                //            }
                                //        }
                                //        else if (value == null)
                                //        {
                                //            if (entityExists.MS1BaseLineDate != null)
                                //            {
                                //                entity.Ms1baselinedate = null;
                                //                recordChanged = true;
                                //            }
                                //        }                                       
                                //        else
                                //        {
                                //            error += htmlbreak + " MS1: BaseLine Date column should be Date and Time. Received value : " + value;
                                //        }
                                //    }
                                //    catch
                                //    {
                                //        error += htmlbreak + " MS1: BaseLine Date Unable to convert date. Received value : " + value;
                                //    }
                                //}
                                #endregion
                                if (item.ContainsKey("MS1: Latest Planning Date"))
                                {
                                    string value = item["MS1: Latest Planning Date"];
                                    var dateValue = ConvertDateValue(value);
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(dateValue.ToString()))
                                        {
                                            if (entityExists.MS1LatestPlanningDate != dateValue)
                                            {
                                                entity.Ms1latestplanningdate = dateValue;
                                                recordChanged = true;
                                                await _commonManager.UpdateProjectPlanDateForLastDeliveryStatusOfMS(plannedActivity, dateValue, (int)MilestoneStatusEnum.MS1, _repositoryWrapper);
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.MS1LatestPlanningDate != null)
                                            {
                                                entity.Ms1latestplanningdate = null;
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + " MS1: Latest Planning Date column should be Date and Time. Received value : " + value;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " MS1: Latest Planning Date Unable to convert date. Received : " + value;
                                    }
                                }
                                if (item.ContainsKey("MS1: Status"))
                                {
                                    string value = item["MS1: Status"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        int newValue =0;
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                            if (val.ToLower().Replace(" ","") == "ontrack" || val.ToLower() == "delayed" || val.ToLower() == "completed")
                                            {
                                                newValue = MSdeliveryStatus(val,newValue);
                                                if (entityExists.MS1Status != newValue && newValue != 0)
                                                {
                                                    entity.Ms1status = newValue;
                                                    recordChanged = true;
                                                }
                                            }
                                            else
                                            {
                                                error += htmlbreak + " MS1: Status column value should be like this (Ontrack, Delayed and Completed). Received value : " + val;
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.MS1Status != null)
                                            {
                                                entity.Ms1status = null;
                                                recordChanged = true;
                                            }
                                        }


                                    }
                                    catch
                                    {
                                        error += htmlbreak + " MS1: Status column Unable to update : " + value;
                                    }
                                }
                                if (item.ContainsKey("MS2: Event Type"))
                                {
                                    string value = item["MS2: Event Type"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                            if (val.Length >= 255)
                                            {
                                                error += htmlbreak + " MS2: Event Type column allow 255 char. Received value : " + value.Length;
                                            }

                                            else if (entityExists.MS2EventType != val)
                                            {
                                                entity.Ms2eventtype = val;
                                                recordChanged = true;
                                            }
                                          
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.MS2EventType != null)
                                            {
                                                entity.Ms2eventtype = null;
                                                recordChanged = true;
                                            }
                                        }


                                    }
                                    catch
                                    {
                                        error += htmlbreak + " MS2: Event Type column Unable to update : " + value;
                                    }
                                }
                                #region
                                //if (item.ContainsKey("MS2: BaseLine Date"))
                                //{
                                //    string value = item["MS2: BaseLine Date"];
                                //    var dateValue = ConvertDateValue(value);
                                //    try
                                //    {
                                //        if (!string.IsNullOrEmpty(dateValue.ToString()))
                                //        {
                                //            if (plannedActivity != null && (!string.IsNullOrEmpty(plannedActivity.Startdate.ToString())))
                                //            {
                                //                if ((dateValue.Value.Date >= plannedActivity.Startdate.Value.Date) && (dateValue.Value.Date <= plannedActivity.Plannedcompletion.Value.Date))
                                //                {

                                //                    if (entityExists.MS2BaseLineDate != dateValue)
                                //                    {
                                //                        entity.Ms2baselinedate = dateValue;
                                //                        recordChanged = true;
                                //                    }
                                //                }

                                //                else
                                //                {
                                //                    MsstatusError = true;
                                //                    msStatusErrorDescription.Add(item[IdColumn]);
                                //                    error += htmlbreak + " MS2: BaseLine Date column should be align with planned activity start date . Received value : " + value;
                                //                }
                                //            }
                                //            else if (entityExists.MS2BaseLineDate != dateValue)
                                //            {
                                //                entity.Ms2baselinedate = dateValue;
                                //                recordChanged = true;
                                //            }
                                //        }
                                //        else if (value == null)
                                //        {
                                //            if (entityExists.MS2BaseLineDate != null)
                                //            {
                                //                entity.Ms2baselinedate = null;
                                //                recordChanged = true;
                                //            }
                                //        }
                                //        else
                                //        {
                                //            error += htmlbreak + " MS2: BaseLine Date column should be Date and Time. Received " + value;
                                //        }
                                //    }
                                //    catch
                                //    {
                                //        error += htmlbreak + " MS2: BaseLine Date Unable to convert date.Received : " + value;
                                //    }
                                //}
                                #endregion
                                if (item.ContainsKey("MS2: Latest Planning Date"))
                                {
                                    string value = item["MS2: Latest Planning Date"];
                                    var dateValue = ConvertDateValue(value);
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(dateValue.ToString()))
                                        {
                                            if (entityExists.MS2LatestPlanningDate != dateValue)
                                            {
                                                entity.Ms2latestplanningdate = dateValue;
                                                recordChanged = true;
                                                await _commonManager.UpdateProjectPlanDateForLastDeliveryStatusOfMS(plannedActivity, dateValue, (int)MilestoneStatusEnum.MS2, _repositoryWrapper);
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.MS2LatestPlanningDate != null)
                                            {
                                                entity.Ms2latestplanningdate = null;
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + " MS2: Latest Planning Date column should be Date and Time. Received " + value;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " MS2: Latest Planning Date Unable to convert date.Received : " + value;
                                    }
                                }
                                if (item.ContainsKey("MS2: Status"))
                                {
                                    string value = item["MS2: Status"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        int newValue = 0;
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                           
                                            if (val.ToLower().Replace(" ", "") == "ontrack" || val.ToLower() == "delayed" || val.ToLower() == "completed")
                                            {
                                                newValue = MSdeliveryStatus(val, newValue);

                                                if (entityExists.MS2Status != newValue && newValue != 0)
                                                {
                                                    entity.Ms2status = newValue;
                                                    recordChanged = true;
                                                }
                                            }
                                            else
                                            {
                                                error += htmlbreak + " MS2: Status column value should be like this (Ontrack, Delayed and Completed). Received value : " + val;
                                            }
                                           
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.MS2Status != null)
                                            {
                                                entity.Ms2status = null;
                                                recordChanged = true;
                                            }
                                        }


                                    }
                                    catch
                                    {
                                        error += htmlbreak + " MS2: Status column Unable to update : " + value;
                                    }
                                }
                                if (item.ContainsKey("MS3: Event Type"))
                                {
                                    string value = item["MS3: Event Type"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                            if (val.Length >= 255)
                                            {
                                                error += htmlbreak + " MS3: Event Type column maximum 255 char. Received value  : " + value.Length;
                                            }
                                            if (entityExists.MS3EventType != val)
                                                {
                                                    entity.Ms3eventtype = val;
                                                    recordChanged = true;
                                                }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.MS3EventType != null)
                                            {
                                                entity.Ms3eventtype = null;
                                                recordChanged = true;
                                            }
                                        }


                                    }
                                    catch
                                    {
                                        error += htmlbreak + " MS3: Event Type column Unable to updated : " + value;
                                    }
                                }
                                #region
                                //if (item.ContainsKey("MS3: BaseLine Date"))
                                //{
                                //    string value = item["MS3: BaseLine Date"];
                                //    var dateValue = ConvertDateValue(value);
                                //    try
                                //    {
                                //        if (!string.IsNullOrEmpty(dateValue.ToString()))
                                //        {
                                //            if (plannedActivity != null && (!string.IsNullOrEmpty(plannedActivity.Startdate.ToString())))
                                //            {
                                //                if ((dateValue.Value.Date >= plannedActivity.Startdate.Value.Date) && (dateValue.Value.Date <= plannedActivity.Plannedcompletion.Value.Date))
                                //                {
                                //                    if (entityExists.MS3BaseLineDate != dateValue)
                                //                    {
                                //                        entity.Ms3baselinedate = dateValue;
                                //                        recordChanged = true;

                                //                    }
                                //                }
                                //                else
                                //                {
                                //                    MsstatusError = true;
                                //                    msStatusErrorDescription.Add(item[IdColumn]);
                                //                    error += htmlbreak + " MS3: BaseLine Date column should be align with planned activity start date . Received value : " + value;
                                //                }
                                //            }
                                //            else if (entityExists.MS3BaseLineDate != dateValue)
                                //            {

                                //                entity.Ms3baselinedate = dateValue;
                                //                recordChanged = true;

                                //            }
                                //        }
                                //        else if (value == null)
                                //        {
                                //            if (entityExists.MS3BaseLineDate != null)
                                //            {
                                //                entity.Ms3baselinedate = null;
                                //                recordChanged = true;
                                //            }
                                //        }
                                //        else
                                //        {
                                //            error += htmlbreak + " MS3: BaseLine Date column should be Date and Time. Received value : " + value;
                                //        }
                                //    }
                                //    catch
                                //    {
                                //        error += htmlbreak + " MS3: BaseLine Date Unable to convert date. Received value : " + value;
                                //    }
                                //}
                                #endregion
                                if (item.ContainsKey("MS3: Latest Planning Date"))
                                {
                                    string value = item["MS3: Latest Planning Date"];
                                    var dateValue = ConvertDateValue(value);
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(dateValue.ToString()))
                                        {
                                            if (entityExists.MS3LatestPlanningDate != dateValue)
                                            {
                                                entity.Ms3latestplanningdate = dateValue;
                                                recordChanged = true;
                                                await _commonManager.UpdateProjectPlanDateForLastDeliveryStatusOfMS(plannedActivity, dateValue, (int)MilestoneStatusEnum.MS3, _repositoryWrapper);
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.MS3LatestPlanningDate != null)
                                            {
                                                entity.Ms3latestplanningdate = null;
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + " MS3: Latest Planning Date column should be Date and Time. Received value : " + value;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " MS3: Latest Planning Date Unable to convert date. Received value : " + value;
                                    }
                                }
                                if (item.ContainsKey("MS3: Status"))
                                {
                                    string value = item["MS3: Status"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        int newValue = 0;
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                            if (val.ToLower().Replace(" ", "") == "ontrack" || val.ToLower() == "delayed" || val.ToLower() == "completed")
                                            {
                                                newValue = MSdeliveryStatus(val, newValue);

                                                if (entityExists.MS3Status != newValue && newValue != 0)
                                                {
                                                    entity.Ms3status = newValue;
                                                    recordChanged = true;
                                                }
                                            }
                                            else
                                            {
                                                error += htmlbreak + " MS3: Status column value should be like this (Ontrack, Delayed and Completed). Received value : " + val;
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.MS3Status != null)
                                            {
                                                entity.Ms3status = null;
                                                recordChanged = true;
                                            }
                                        }


                                    }
                                    catch
                                    {
                                        error += htmlbreak + " MS3: Status column Unable to update : " + value;
                                    }
                                }
                                if (item.ContainsKey("MS4: Event Type"))
                                {
                                    string value = item["MS4: Event Type"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                            if (int.TryParse(val, out int intValue))
                                            {
                                                if (val.Length >= 255)
                                                {
                                                    error += htmlbreak + " MS4: Event Type column maximum 255 char. Received value : " + value.Length;
                                                }

                                                else if (entityExists.MS4EventType != intValue)
                                                {
                                                    entity.Ms4eventtype = intValue;
                                                    recordChanged = true;
                                                }
                                            }
                                            else
                                            {
                                                error += htmlbreak + " MS4: Event Type column should be Integer. Received value : " + val;

                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.MS4EventType != null)
                                            {
                                                entity.Ms4eventtype = null;
                                                recordChanged = true;
                                            }
                                        }


                                    }
                                    catch
                                    {
                                        error += htmlbreak + " MS4: Event Type column Unable to updated : " + value;
                                    }
                                }
                                #region
                                //if (item.ContainsKey("MS4: BaseLine Date"))
                                //{
                                //    string value = item["MS4: BaseLine Date"];
                                //    var dateValue = ConvertDateValue(value);
                                //    try
                                //    {
                                //        if (!string.IsNullOrEmpty(dateValue.ToString()))
                                //        {
                                //            if (plannedActivity != null && (!string.IsNullOrEmpty(plannedActivity.Startdate.ToString())))
                                //            {
                                //                if ((dateValue.Value.Date >= plannedActivity.Startdate.Value.Date) && (dateValue.Value.Date <= plannedActivity.Plannedcompletion.Value.Date))
                                //                {
                                //                    if (entityExists.MS4BaseLineDate != dateValue)
                                //                    {
                                //                        entity.Ms4baselinedate = dateValue;
                                //                        recordChanged = true;
                                //                    }
                                //                }
                                //                else
                                //                {
                                //                    MsstatusError = true;
                                //                    msStatusErrorDescription.Add(item[IdColumn]);
                                //                    error += htmlbreak + " MS4: BaseLine Date column should be align with planned activity start date . Received value : " + value;
                                //                }
                                //            }
                                //            else if(entityExists.MS4BaseLineDate != dateValue)
                                //            {                                              
                                //                entity.Ms4baselinedate = dateValue;
                                //                recordChanged = true;
                                //            }

                                //        }
                                //        else if (value == null)
                                //        {
                                //            if (entityExists.MS4BaseLineDate != null)
                                //            {
                                //                entity.Ms4baselinedate = null;
                                //                recordChanged = true;
                                //            }
                                //        }
                                //        else
                                //        {
                                //            error += htmlbreak + " MS4: BaseLine Date column should be Date and Time. Received value : " + value;
                                //        }
                                //    }
                                //    catch
                                //    {
                                //        error += htmlbreak + " MS4: BaseLine Date Unable to convert date. Received value : " + value;
                                //    }
                                //}
                                #endregion
                                if (item.ContainsKey("MS4: Latest Planning Date"))
                                {
                                    string value = item["MS4: Latest Planning Date"];
                                    var dateValue = ConvertDateValue(value);
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(dateValue.ToString()))
                                        {
                                            if (entityExists.MS4LatestPlanningDate != dateValue)
                                            {
                                                entity.Ms4latestplanningdate = dateValue;
                                                recordChanged = true;
                                                await _commonManager.UpdateProjectPlanDateForLastDeliveryStatusOfMS(plannedActivity, dateValue, (int)MilestoneStatusEnum.MS4, _repositoryWrapper);
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.MS4LatestPlanningDate != null)
                                            {
                                                entity.Ms4latestplanningdate = null;
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + " MS4: Latest Planning Date column should be Date and Time. Received value : " + value;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " MS4: Latest Planning Date Unable to convert date. Received value : " + value;
                                    }
                                }
                                if (item.ContainsKey("MS4: Status"))
                                {
                                    string value = item["MS4: Status"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        int newValue = 0;
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                            if (val.ToLower().Replace(" ", "") == "ontrack" || val.ToLower() == "delayed" || val.ToLower() == "completed")
                                            {
                                                newValue = MSdeliveryStatus(val, newValue);

                                                if (entityExists.MS4Status != newValue && newValue != 0)
                                                {
                                                    entity.Ms4status = newValue;
                                                    recordChanged = true;
                                                }
                                            }
                                            else
                                            {
                                                error += htmlbreak + " MS4: Status column value should be like this (Ontrack, Delayed and Completed). Received value : " + val;
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.MS4Status != null)
                                            {
                                                entity.Ms4status = null;
                                                recordChanged = true;
                                            }
                                        }


                                    }
                                    catch
                                    {
                                        error += htmlbreak + " MS4: Status column Unable to update : " + value;
                                    }
                                }
                                if (item.ContainsKey("PPM Import Date"))
                                {
                                    string value = item["PPM Import Date"];
                                    var dateValue = ConvertDateValue(value);
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(dateValue.ToString()))
                                        {
                                            if (entityExists.PPMImportDate != dateValue)
                                            {
                                                entity.Ppmimportdate = dateValue;
                                                recordChanged = true;
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.PPMImportDate != null)
                                            {
                                                entity.Ppmimportdate = null;
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + " PPM Import Date column should be Date and Time. Received value : " + value;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " PPM Import Date Unable to convert date. Received value : " + value;
                                    }
                                }
                                if (item.ContainsKey("Notes 1"))
                                {
                                    try
                                    {
                                        string value = item["Notes 1"];
                                        string val = value != null ? value : "";
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                            if (entityExists.Notes1 != value)
                                            {
                                                entity.Notes1 = value;
                                                recordChanged = true;
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.Notes1 != null)
                                            {
                                                entity.Notes1 = value;
                                                recordChanged = true;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " Notes 1 Value unable to updated ";

                                    }

                                }
                                if (item.ContainsKey("Notes 2"))
                                {
                                    try
                                    {
                                        string value = item["Notes 2"];
                                        string val = value != null ? value : "";
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                            if (entityExists.Notes2 != value)
                                            {
                                                entity.Notes2 = value;
                                                recordChanged = true;
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.Notes2 != null)
                                            {
                                                entity.Notes2 = value;
                                                recordChanged = true;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        error += " Notes 2 Value unable to updated ";

                                    }

                                }
                                if (item.ContainsKey("PPM ID"))
                                {
                                    try
                                    {
                                        string value = item["PPM ID"];
                                        string val = value != null ? value : "";
                                        if (!string.IsNullOrEmpty(val))
                                        {                                           
                                             var result = UpdatePPMIDInPAPage(entityExists,value).Result;
                                            if (result.Warning)
                                            {
                                                recordChanged = result.Warning;
                                                entity.Ppmimportdate = DateTime.Now.Date; 
                                            }
                                        }
                                        else if (value == null)
                                        {   var result = UpdatePPMIDInPAPage(entityExists, value).Result;
                                            if (result.Info.ToLower().Trim() != "valueisnull" && result.Warning)
                                            {
                                                recordChanged = true;
                                            }
                                            
                                            if (recordChanged) { entity.Ppmimportdate = DateTime.Now.Date; }
                                        }
                                    }
                                    catch
                                    {
                                        error += " PPM ID Value unable to updated ";

                                    }

                                }
                            } 
                            else
                            {
                               // error += " DB Doesn't Contain : " + IdColumn + "column " + "value : " + item[IdColumn];
                                deletedRecordsError +=  " "+item[IdColumn] + ",";
                                norecordsDeleted = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            errordescription += " Unhandled Error " + ex.Message + " in updating record : " + IdColumn + " column : " + item[IdColumn];
                        }
                        try
                        {
                            if (error == "" && recordChanged)
                            {
                                _repositoryWrapper.DeliveryTrackingRepository.Update(entity);
                                _repositoryWrapper.Save();
                                await _repositoryWrapper.ClearTracker();
                                RecordUpdated = true;
                                norecordsUpdated++;
                            }                                                 
                            else if (norecordsDeleted != 0)
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
            
            if (norecordsUpdated != 0)
            {
                errorShortDescription += norecordsUpdated + " record updated successfully in Delivery Tracking";
            }
            if (deletedRecordsError.Length > 2)
            {
                string suffixValue = deletedRecordsError.TrimEnd(',').Split(',').Count() > 1 ? " don't exist " : " doesn't exist ";
                if (norecordsUpdated != 0) errorShortDescription += " and " + IdColumn + " column values " + deletedRecordsError.TrimEnd(',') + suffixValue;
                else errorShortDescription += " " + IdColumn + " column values " + deletedRecordsError.TrimEnd(',') + suffixValue;
            }

            if (errorShortDescription.Length > 0 &&  norecordsUpdated == 0 )
            {
                   if (deletedRecordsError.Length > 2) errorShortDescription = errorShortDescription + " and some records are not updated. please update the correct value in the excel and try to upload again .";
                   else errorShortDescription = IdColumn + "  : " + errorShortDescription + " and some records are not updated. please update the correct value in the excel and try to upload again .";
            }
            if(errorShortDescription.Length == 0 && msStatusErrorDescription.Count >0)
            {
                var msStatusName =string.Empty;
                var msStatusErrorDescriptionId =string.Empty;
                foreach(var status in msStatusErrorDescription.Distinct())
                {
                    
                    msStatusErrorDescriptionId += status +",";
                }
                errorShortDescription += $"MS1,MS2,MS3 and MS4 BaseLine Date column should be align with planned activity start date and Planned completion date for this records {msStatusErrorDescriptionId}";


            }
            if (errorShortDescription.Length == 0 && !RecordUpdated)
            {
                errorShortDescription += "No Records Updated in "+excelName+". please update the value and try to upload again .";
            }
            
            return new ResultDto
            {
                Data = errorShortDescription,
                Info = errorShortDescription.Length > 0 ? errorShortDescription : norecordsUpdated.ToString(),
                Warning = errorShortDescription.Length > 0 && norecordsUpdated == 0 ? false : true,
            };
        }

        #region future use code
        public DateTime? ConvertDateValue(string dateValue)
        {
            DateTime? date = null;
            if (!string.IsNullOrEmpty(dateValue))
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

        public DateTime? ConvertDateValues(string dateValue)
        {
            DateTime? date = null;

            if (!string.IsNullOrEmpty(dateValue))
            {
                // Attempt to parse the full date-time string first
                string[] formats = { "M/d/yyyy h:mm:ss tt", "M/d/yyyy H:mm:ss", "MM/dd/yyyy h:mm:ss tt", "MM/dd/yyyy H:mm:ss" };

                if (DateTime.TryParseExact(dateValue, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    date = parsedDate;
                }
                else
                {
                    // If the full date-time string fails, try parsing just the date part
                    dateValue = dateValue.Split(' ')[0].Replace("-", "/");

                    if (DateTime.TryParseExact(dateValue, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                        || DateTime.TryParseExact(dateValue, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                        || DateTime.TryParseExact(dateValue, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                        || DateTime.TryParseExact(dateValue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                    {
                        date = parsedDate;
                    }
                }
            }

            return date;
        }

        public int MSdeliveryStatus ( string val , int newValue)
        {
           
                if (val == "1")
                {
                    val = "On track";
                }
                else if (val == "2")
                {
                    val = "Delayed";
                }
                else if (val == "3")
                {
                    val = "Completed";
                }

                if (val.ToLower().Replace(" ", "") == "ontrack")
                {
                    newValue = 1;
                }
                else if (val.ToLower() == DeliveryStatusEnum.Delayed.ToString().ToLower())
                {
                    newValue = (int)DeliveryStatusEnum.Delayed;
                }
                else if (val.ToLower() == DeliveryStatusEnum.Completed.ToString().ToLower())
                {
                    newValue = (int)DeliveryStatusEnum.Completed;
                }
                return newValue;
          
        }
        public async Task<ResultDto> UpdatePPMIDInPAPage(DeliveryTracking deliveryTracking,string? value)
        {
            var getPlannedActivity = _repositoryWrapper.PlannedActivity.FindByCondition(x=>x.Plannedactivityid == deliveryTracking.PlannedActivityId).FirstOrDefault();
            var isResult = false;
            var info = value!=null? "value is not null" : "value is null";
            if(getPlannedActivity != null)
            {
                if (getPlannedActivity.Deliveryprojectid != value)
                {
                    getPlannedActivity.Deliveryprojectid = value;
                    _repositoryWrapper.PlannedActivity.Update(getPlannedActivity);
                    _repositoryWrapper.Save();
                    await _repositoryWrapper.ClearTracker();
                    isResult = true;
                    info = value != null ? "value is not null" : "value is null";
                }
            }
            return new ResultDto
            {
                Warning = isResult,
                Info = info,
            };
        }
    }
}
