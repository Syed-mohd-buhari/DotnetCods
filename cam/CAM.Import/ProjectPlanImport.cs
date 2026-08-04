using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Contracts;
using Microsoft.AspNetCore.Http;
using CAM.BusinessManager;
using CAM.DataTransferObjects;
using System.Globalization;
using CAM.Entities.Mappers.Entity;

namespace CAM.Imports
{
    public class ProjectPlanImport : BaseManager
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public string SheetName = "ProjectPlan";
        public string IdColumn = "Projects Plan Id";
        public string excelName = "ProjectPlan";

        public List<string> EditableColumnList = new List<string> { "Start Date","End Date", "Progress", "Plan Description" };

        public ProjectPlanImport(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, IMapper mapper
            , IHttpContextAccessor contextAccessor, ILoggerManager logger) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;

        }

        public async Task<ResultDto> UpdateExcelColumn(List<Dictionary<string, string>> processData, List<Dictionary<string, string>> insertionData)
        {
            //Ticket 1248 - Delivery Tracking Import : Separate the error message for deleted and updated records
            string errordescription = "", error = "", htmlbreak = " ", errorShortDescription = "",deletedRecordsError ="" ;
            bool recordChanged = false, RecordUpdated = false, RecordDeleted = false;
            int norecordsUpdated = 0 , norecordsDeleted = 0; 
            var msStatusErrorDescription = new List<string>();    
            try
            {
                foreach (Dictionary<string, string> item in processData)
                {
                    if (item[IdColumn] != null)
                    {
                        
                        var model = _repositoryWrapper.ProjectPlanRepository.FindByCondition(x => x.Projectsplanid.ToString() == item[IdColumn],true).FirstOrDefault();

                        if (model != null)
                        {

                            var plannedActivity = await Task.Run(() => _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == model.Plannedactivityid).FirstOrDefault());

                        }
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

                        var entityExists = (RecordDeleted == false) ? ProjectPlanMapper.Get(model) : null;
                        var entity = (RecordDeleted == false) ? ProjectPlanMapper.Set(entityExists) : null;
                        recordChanged = false;
                        try
                        {
                            error = "";
                            if (entityExists != null && entity != null)
                            {                                
                                if (item.ContainsKey("End Date"))
                                {
                                    string value = item["End Date"];
                                    DateTime? dateValue = null;
                                    if (DateTime.TryParseExact(value, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                                    {
                                        dateValue = result;
                                    }
                                    else
                                    {
                                        dateValue = ConvertDateValue(value);

                                    }
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(Convert.ToString( dateValue )))
                                        {
                                            if (entityExists.PlanningEndDate.ToString() != dateValue.ToString())
                                            {
                                                entity.Planningenddate = dateValue;
                                                recordChanged = true;
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.PlanningEndDate != null)
                                            {
                                                entity.Planningenddate = null;
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + " EndDate column should be Date and Time. Received value : " + value;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " EndDate Unable to convert date. Received value : " + value;
                                    }
                                }
                                if (item.ContainsKey("Start Date"))
                                {
                                    string value = item["Start Date"];
                                    DateTime? dateValue = null;
                                    if (DateTime.TryParseExact(value, "M/d/yyyy h:mm:ss tt",CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime result))
                                    {
                                        dateValue = result;
                                    }
                                    else
                                    {
                                        dateValue = ConvertDateValue(value);

                                    }
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(Convert.ToString(dateValue)))
                                        {
                                            if (entityExists.PlanningStartDate.ToString() != dateValue.ToString())
                                            {
                                                entity.Planningstartdate = dateValue;
                                                recordChanged = true;
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.PlanningStartDate != null)
                                            {
                                                entity.Planningstartdate = null;
                                                recordChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            error += htmlbreak + " StartDatee column should be Date and Time. Received value : " + value;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " StartDate Unable to convert date. Received value : " + value;
                                    }
                                }

                                if (item.ContainsKey("Progress"))
                                {
                                    string value = item["Progress"];                                     
                                    try
                                    { 
                                        if(entity.Progress != value)
                                        {
                                            entity.Progress = value;
                                            recordChanged = true;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " Progress Data Fromat Issue. Received value : " + value;
                                    }
                                }
                                if (item.ContainsKey("Plan Description"))
                                {
                                    string value = item["Plan Description"];
                                    try
                                    {
                                        if (entity.Description != value)
                                        {
                                            entity.Description = value;
                                            recordChanged = true;
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " Description Data Fromat Issue. Received value : " + value;
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
                               
                                _repositoryWrapper.ProjectPlanRepository.Update(entity);
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
                errorShortDescription += norecordsUpdated + " record updated successfully ";
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
            
            if (errorShortDescription.Length == 0 && !RecordUpdated)
            {
                errorShortDescription += "No Records Updated in "+excelName+". please update the value and try to upload again .";
            }
            
            return new ResultDto
            {

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


    }
}
