using CAM.BusinessManager;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using OracleModels.DBModels;

namespace CAM.Imports
{
    public class TemsFntImport : BaseManager
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly CommonManager _commonManager;
        private readonly Dictionary<int, string> errorList = new Dictionary<int, string>();

        public TemsFntImport(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper,
            IHttpContextAccessor contextAccessor,
            ILoggerManager logger, CommonManager commonManager) :
            base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _commonManager = commonManager;
            _logger = logger;

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

        public async Task<ResultDto> ParseExcelRowsAsync(
           ISheet sheet, Dictionary<int, Exceltemplateconfiguration> columnMap, int dataRowIndex, string sheetName, long nonTemsVertical)
        {

            var errorRows = new List<string>();
            var passThroughCreateDataList = new List<Temsfntreport>();
            var passThroughUpdateDataList = new List<Temsfntreport>();

            var passThroughEntity = _repositoryWrapper.TemsFntReportRepository.FindAll().ToList();
            try
            {

                var existingPassThroughEntity = passThroughEntity.ToDictionary(x => $"{ x.Hostname}_{x.Serialnumberofhardwareasset}", StringComparer.OrdinalIgnoreCase);
                //Instead of comparing one to one columns i checked in the to dictionary app
                var propertyMap = columnMap.ToDictionary(x => x.Key,
                     x => new
                     {
                         x.Value.Columnheadername,
                         Property = typeof(Temsfntreport).GetProperty(ToFirstUpperRestLower(x.Value.Propertyname))
                     });


                for (int rowIndex = dataRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
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

                    var temsFntReportHostName = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").Equals(ConstantValueFilter.HostName, StringComparison.OrdinalIgnoreCase));
                    var temsFntReportSerialNumberOfHardwareAsset = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").Equals(ConstantValueFilter.SerialNumberOfHardwareAsset, StringComparison.OrdinalIgnoreCase));

                    if (temsFntReportHostName.Value == null && temsFntReportSerialNumberOfHardwareAsset.Value == null)
                    {
                        errorRows.Add($"Mapping missing for unique columns: {ConstantValueFilter.HostName.ToUpper()}, {ConstantValueFilter.SerialNumberOfHardwareAsset.ToUpper()}");
                        break;
                    }
                    string temsFntReportHostNameValue = row.GetCell(temsFntReportHostName.Key)?.ToString()?.Trim() ?? string.Empty;
                    string temsFntReportSerialNumberOfHardwareAssetValue = row.GetCell(temsFntReportSerialNumberOfHardwareAsset.Key)?.ToString()?.Trim() ?? string.Empty;

                    string uniqueValu = $"{temsFntReportHostNameValue}_{temsFntReportSerialNumberOfHardwareAssetValue}";

                    if (string.IsNullOrEmpty(temsFntReportHostNameValue) && string.IsNullOrEmpty(temsFntReportSerialNumberOfHardwareAssetValue))
                    {
                        errorRows.Add($"Row {rowIndex} - Missing mandatory fields: {ConstantValueFilter.HostName.ToUpper()}"); continue;
                    }

                    if (existingPassThroughEntity == null) continue;

                    existingPassThroughEntity.TryGetValue(uniqueValu, out var ptEntity);

                    var passThrough = ptEntity ?? new Temsfntreport();

                    foreach (var col in propertyMap)
                    {
                        var cellValue = row.GetCell(col.Key)?.ToString()?.Trim();
                        var prop = col.Value.Property;

                        if (prop == null) continue;

                        try
                        {

                            if (_commonManager.TrySetValueFromImportCell(passThrough, prop.Name, cellValue)) continue;
                            else
                            {
                                errorRows.Add($"Row {rowIndex} - Data format issue in : {col.Value.Columnheadername.ToUpper()}");
                            }

                        }
                        catch
                        {

                        }


                    }
                    //passThrough.Nontemsvertical = nonTemsVertical;
                    if (existingPassThroughEntity != null)
                    {
                        passThroughUpdateDataList.Add(passThrough);
                    }
                    else
                    {
                        passThroughCreateDataList.Add(passThrough);
                    }

                }
                try
                {

                    //if (passThroughCreateDataList.Any())
                    //{
                    //    _repositoryWrapper.PassThroughRepository.BulkCreate(passThroughCreateDataList);
                    //    await _repositoryWrapper.SaveAsync();
                    //}
                    //await _repositoryWrapper.ClearTracker();
                    if (passThroughUpdateDataList.Any())
                    {
                        _repositoryWrapper.TemsFntReportRepository.BulkUpdate(passThroughUpdateDataList);
                        await _repositoryWrapper.SaveAsync();
                    }
                    await _repositoryWrapper.ClearTracker();

                }
                catch (Exception ex)
                {
                    errorList.Add(passThroughCreateDataList.Count, string.Join(',', passThroughCreateDataList.Select(x => x.Hostname).Distinct().ToList()));
                    _logger.LogError(ResultMessages.errorLogTracke + "ParseExcelRowsAsync()  : " + ex);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResultMessages.errorLogTracke + "ParseExcelRowsAsync()  : " + ex);
            }


            return new ResultDto
            {
                Info = $"No of processed rows: {passThroughCreateDataList.Count + passThroughUpdateDataList.Count} rows and No of failed rows: {errorRows.Count} ",
                Warning = (!errorRows.Any() && errorRows.Count == 0) ? true : false,
                Data = new { SuccessRecord = passThroughCreateDataList, ErrorDetails = errorRows }
            };

        }

        public static string ToFirstUpperRestLower(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            input = input.Replace("Tsr", "").Trim();

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

    }
}
