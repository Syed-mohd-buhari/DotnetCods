using AutoMapper;
using CAM.BusinessManager;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Linq;
using System.Reflection;
using CellType = NPOI.SS.UserModel.CellType;

namespace CAM.Imports
{
    public class BPTImport : BaseManager
    {
        private readonly ModelContext _modelContext;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly ILoggerManager _logger;
        private readonly Dictionary<int, string> errorList = new Dictionary<int, string>();
        public BPTImport(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, IMapper mapper,
            IHttpContextAccessor contextAccessor,
            ILoggerManager logger,CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager, ModelContext modelContext) :
            base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _commonManager =  commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _modelContext = modelContext;
            _logger = logger;

        }
        public bool IsRowEmpty(IRow row)
        {
            if (row == null) return true;

            for (int i = row.FirstCellNum; i < row.LastCellNum; i++)
            {
                var cell = row.GetCell(i);
                // If the cell has any value (including dropdown selection), it's not empty
                if (cell != null && !string.IsNullOrWhiteSpace(cell.ToString()))
                {
                    return false;
                }
            }

            return true; // All cells empty or unselected
        }
        public bool IsRowEffectivelyEmpty(IRow row)
        {
            if (row == null) return true;
            try
            {
                for (int i = row.FirstCellNum; i < row.LastCellNum; i++)
                {
                    var cell = row.GetCell(i);
                    if (cell == null) continue;

                    switch (cell.CellType)
                    {
                        case CellType.String:
                            if (!string.IsNullOrWhiteSpace(cell.StringCellValue))
                                return false;
                            break;

                        case CellType.Numeric:
                            if (cell.NumericCellValue != 0)
                                return false;
                            break;

                        case CellType.Formula:
                            // Get the formula result type
                            var formulaResultType = cell.CachedFormulaResultType;

                            if (formulaResultType == CellType.Numeric && cell.NumericCellValue != 0)
                                return false;

                            if (formulaResultType == CellType.String && !string.IsNullOrWhiteSpace(cell.StringCellValue))
                                return false;

                            break;

                        case CellType.Boolean:
                            if (cell.BooleanCellValue)
                                return false;
                            break;

                        case CellType.Error:
                            return false;

                            // CellType.Blank is treated as empty
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResultMessages.errorLogTracke + "IsRowEmpty()  : " + ex);
            }
          
            return true;
        }

        public async Task<ResultDto> ParseExcelRowsAsync (
           ISheet sheet, Dictionary<int, Exceltemplateconfiguration> columnMap, int dataRowIndex,string sheetName  ) 
        {
           
            var errorRows = new List<string>(); 
            var bptExcelDataList = new List<Budgetprojecttrackers>(); 

            var bptEntity = _repositoryWrapper.BudgetProjectTrackersRepository.FindByCondition(x => x.Archive == false && x.Currenttrackingnumber != null).ToList() ;
 
            var allPAEntity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Archived == false && x.Budgettrackingid !=null)
                .Select(x => new { x.Plannedactivityid,  x.Budgettrackingid }).ToList().GroupBy(a => a.Budgettrackingid)
                                .GroupBy(x => Convert.ToString(x.Key ?? "").ToLower().Replace(" ", ""))
                                          .ToDictionary(
                                                        g => g.Key,
                                                        g => new Queue<long>(g.SelectMany(x => x.Select(i => i.Plannedactivityid)))
                                                    );

            #region
            var opcoReference = _dropdownDataServiceManager.GetAllOpcos().Result;
            var categoryReference = await _dropdownDataServiceManager.GetPACategoryDropDown();
            var priorityReference = _commonManager.PriorityStatusResource();
            var lcmcategoriesReference = _commonManager.LcmCategoryStatusResource();

            #endregion

            try
            {
                for (int rowIndex = dataRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    bool hasMandatoryOrDataTypeError = false;
                    var errorRownumber = string.Empty;

                    var row = sheet.GetRow(rowIndex);

                    #region row - validation
                    if (row == null) continue;

                    /// <summary>
                    ///  The XLS file contains a data list and some cell validations, so basic row null checking doesn't work. That's why we needed to use a different validation method
                    /// </summary>               

                    if (IsRowEffectivelyEmpty(row))                       
                        continue;

                    if (row == null || row.Cells.All(c => string.IsNullOrWhiteSpace(c?.ToString()?.Replace(" ", "")) || string.IsNullOrEmpty(c?.ToString()?.Replace(" ", ""))))
                        continue;

                    if (!row.Any()) continue;
                   
                    #endregion
                    
                    var budgetTrackerIdColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                     ConstantValueFilter.bptImportUniqueId);

                    string budgetTrackerIdColumnValue = row.GetCell(budgetTrackerIdColumn.Key)?.ToString()?.Trim() ?? string.Empty;
                    long existingPaId = 0;

                    if (budgetTrackerIdColumn.Value == null || string.IsNullOrEmpty(budgetTrackerIdColumnValue))
                    {
                       // continue;  // After enable the Tracking ID need to enable below lines
                        hasMandatoryOrDataTypeError = true;
                        errorRows.Add($"Row {rowIndex} - Missing mandatory field: {ConstantValueFilter.bptImportUniqueId.ToUpper()}"); continue;

                    }
                    else
                    {
                        if (allPAEntity.TryGetValue(Convert.ToString(budgetTrackerIdColumnValue??"").ToLower().Replace(" ",""), out var nontemsPkeyIds) && nontemsPkeyIds.Count > 0)
                        {
                            var fetchEntity = nontemsPkeyIds.Dequeue();
                            existingPaId = fetchEntity;
                        }
                        //var paEntity = allPAEntity.Where(x => x.Key == Convert.ToString(budgetTrackerIdColumnValue ?? "").ToLower().Replace(" ", "")).Select(x => x.Value).FirstOrDefault();
                        if (existingPaId == 0)
                        {
                            hasMandatoryOrDataTypeError = true;
                            errorRows.Add($"Row {rowIndex} - Budget TrackingId data not exists");
                            continue;
                        }
                        
                    }


                    var existingBptDto = bptEntity.Where(x => (x.Currenttrackingnumber ?? "").Trim().ToLower()
                    == budgetTrackerIdColumnValue.ToLower().Trim() && x.Plannedactivityid == existingPaId)
                           .FirstOrDefault(); 

                    if (existingBptDto == null)
                    {
                        hasMandatoryOrDataTypeError = true;
                        errorRows.Add($"Row {rowIndex} - Budget TrackingId data not exists");                       
                        continue;
                    }
                    
                    foreach (var colIndex in columnMap.Keys)
                    {
                        var cellValue = row.GetCell(colIndex)?.ToString()?.Trim();
                        var Columnheadername = columnMap[colIndex].Columnheadername;
                        var propertyName = Columnheadername; // or Propertyname

                        if (string.IsNullOrEmpty(propertyName))
                            propertyName = columnMap[colIndex].Propertyname;

                        if (string.IsNullOrEmpty(propertyName))
                            continue;


                        //if (propertyName.ToLower() == "transfers")
                        //    continue;

                        PropertyInfo prop = typeof(Budgetprojecttrackers).GetProperty(propertyName);

                        if (prop != null &&  string.IsNullOrEmpty(cellValue)) continue;

                        var excelConfigTemp = columnMap.FirstOrDefault(x => x.Key == colIndex);
                        var columnName = excelConfigTemp.Value.Propertyname.Trim();
                        var excelValue = row.GetCell(colIndex)?.ToString()?.Trim();
                        var masterTable = excelConfigTemp.Value.Mappingreference;
                        var isMandatory = excelConfigTemp.Value?.Ismandatory ?? false;                    



                        object? convertedValue = masterTable switch
                        {
                            "opco" => opcoReference.FirstOrDefault(x => x.Value.ToLower().Replace(" ", "") == excelValue?.ToLower().Replace(" ", "")).Key,
                            "category" => categoryReference.FirstOrDefault(x => x.Value.ToLower().Replace(" ", "") == excelValue?.ToLower().Replace(" ", "")).Key,
                            "priority" => priorityReference.FirstOrDefault(x => x.Value.ToLower().Replace(" ", "") == excelValue?.ToLower().Replace(" ", "")).Key,
                            "lcmcategories" => lcmcategoriesReference.FirstOrDefault(x => x.Value.ToLower().Replace(" ", "") == excelValue?.ToLower().Replace(" ", "")).Key,
                            _ => _commonManager.ConvertValueToTypeForImport(excelValue)
                        };

                        if (!string.IsNullOrEmpty(masterTable) && string.IsNullOrEmpty(Convert.ToString(convertedValue)))
                        {
                            
                            hasMandatoryOrDataTypeError = true;
                            errorRows.Add($"Row {rowIndex} - Incorrect data in : {Columnheadername.ToUpper()}");
                         
                        }

                        else if (string.IsNullOrEmpty(excelValue) && isMandatory)
                        {                           
                            hasMandatoryOrDataTypeError = true;
                            errorRows.Add($"Row {rowIndex} - Missing mandatory field: {Columnheadername.ToUpper()}");                         
                        }

                        try
                        {

                            if (_commonManager.TrySetValueFromImportCell(existingBptDto, columnName, convertedValue)) continue;
                            else
                            {                               
                                hasMandatoryOrDataTypeError = true;
                                errorRows.Add($"Row {rowIndex} - Data format issue in : {Columnheadername.ToUpper()}");                             
                            } 

                        }
                        catch
                        {
                            // Optional: log or handle conversion error
                        }

                    }
                    if (!hasMandatoryOrDataTypeError  && existingBptDto?.Budgetprojecttrackerid != 0)
                    {
                        bptExcelDataList.Add(existingBptDto);
                    }

                }
                try
                {

                    if (bptExcelDataList.Any())
                    {
                        _repositoryWrapper.BudgetProjectTrackersRepository.BulkUpdate(bptExcelDataList);
                        await _repositoryWrapper.SaveAsync();
                    }
                    await _repositoryWrapper.ClearTracker();

                }
                catch (Exception ex)
                {
                    errorList.Add(bptExcelDataList.Count, string.Join(',', bptExcelDataList.Select(x => x.Currenttrackingnumber).Distinct().ToList()));
                    _logger.LogError(ResultMessages.errorLogTracke + "ParseExcelRowsAsync()  : " + ex);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResultMessages.errorLogTracke + "ParseExcelRowsAsync()  : " + ex);
            }

          
            return new ResultDto
            {
                Info = $"No of processed rows: {bptExcelDataList.Count} rows and No of failed rows: {errorRows.Count} ", 
                Warning = (!errorRows.Any() && errorRows.Count == 0) ? true : false,
                Data = new { SuccessRecord = bptExcelDataList, ErrorDetails = errorRows }
            };

        }
 
    }
}
