using AutoMapper;
using CAM.BusinessManager;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.Infrastucture;
using CAM.ResourcesKey;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Text;
using CellType = NPOI.SS.UserModel.CellType;

namespace CAM.Imports
{
    public class PassThroughImport : BaseManager
    {
        private readonly ModelContext _modelContext;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly ILoggerManager _logger;
        private readonly Dictionary<int, string> errorList = new Dictionary<int, string>();
        public PassThroughImport(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, IMapper mapper,
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
           ISheet sheet, Dictionary<int, Exceltemplateconfiguration> columnMap, int dataRowIndex,string sheetName ,long nonTemsVertical) 
        {
           
            var errorRows = new List<string>(); 
            var passThroughCreateDataList = new List<Assetpassthrough>(); 
            var passThroughUpdateDataList = new List<Assetpassthrough>(); 

            var passThroughEntity = _repositoryWrapper.PassThroughRepository.FindByCondition(x =>  x.Assetname != null).ToList() ;
            //var assetNames = _repositoryWrapper.NonTemsPassThroughConfigRepository.FindAll().Select(x => x.Elementname).ToList();
 
            try
            {

                var existingPassThroughEntity = passThroughEntity.ToDictionary(x => x.Assetname.Trim(), StringComparer.OrdinalIgnoreCase);
                //Instead of comparing one to one columns i checked in the to dictionary a
                var propertyMap = columnMap.ToDictionary(x => x.Key,
                     x => new
                     {
                         x.Value.Columnheadername,
                         Property = typeof(Assetpassthrough).GetProperty(ToFirstUpperRestLower(x.Value.Propertyname))
                     });
                var resourceKeys = _repositoryWrapper.SwPassThroughLcmRepository.FindByCondition(x => x.Swresourcekey != null).Select(x => x.Swresourcekey).ToList();

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
                    
                    var assetNameColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").Equals(ConstantValueFilter.passThroughImportUniqueName, StringComparison.OrdinalIgnoreCase));
                    if (assetNameColumn.Value == null)
                    {
                        errorRows.Add($"Mapping missing for unique column: {ConstantValueFilter.passThroughImportUniqueName}");
                        break;
                    }

                    var resourceKeyColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").Equals(ConstantValueFilter.passThroughImportResourceKey, StringComparison.OrdinalIgnoreCase));
                    if (resourceKeyColumn.Value == null)
                    {
                        errorRows.Add($"Mapping missing for unique column: {ConstantValueFilter.passThroughImportResourceKey}");
                        break;
                    }
                    string assetName = row.GetCell(assetNameColumn.Key)?.ToString()?.Trim() ?? string.Empty;
                    string resourceKey = row.GetCell(resourceKeyColumn.Key)?.ToString()?.Trim() ?? string.Empty;
                    if (string.IsNullOrEmpty(assetName))
                    {
                        errorRows.Add($"Row {rowIndex} - Missing mandatory field: {ConstantValueFilter.passThroughImportUniqueName.ToUpper()}"); continue;
                    }
                    if (string.IsNullOrEmpty(resourceKey))
                    {
                        errorRows.Add($"Row {rowIndex} - Missing mandatory field: {ConstantValueFilter.passThroughImportUniqueKey.ToUpper()}"); continue;
                    }
                    if (!resourceKeys.Contains(resourceKey))
                    {
                        errorRows.Add($"Row {rowIndex} - The provided resourcekey {resourceKey} is not matching with the LCM AssetPassThrough Resourceky please correct it and import again"); continue;
                    }

                    if (existingPassThroughEntity == null) continue;

                    existingPassThroughEntity.TryGetValue(assetName, out var ptEntity);
                    var passThrough = ptEntity ?? new Assetpassthrough();

                    foreach (var col in propertyMap)
                    {                        
                        var cellValue = row.GetCell(col.Key)?.ToString()?.Trim();
                        var prop = col.Value.Property;

                        if (prop == null ) continue;

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
                    passThrough.Nontemsvertical = (int?)nonTemsVertical;
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

                    if (passThroughCreateDataList.Any())
                    {
                        _repositoryWrapper.PassThroughRepository.BulkCreate(passThroughCreateDataList);
                        await _repositoryWrapper.SaveAsync();
                    }
                    await _repositoryWrapper.ClearTracker();
                    if (passThroughUpdateDataList.Any())
                    {
                        _repositoryWrapper.PassThroughRepository.BulkUpdate(passThroughUpdateDataList);
                        await _repositoryWrapper.SaveAsync();
                    }
                    await _repositoryWrapper.ClearTracker();

                }
                catch (Exception ex)
                {
                    errorList.Add(passThroughCreateDataList.Count, string.Join(',', passThroughCreateDataList.Select(x => x.Assetname).Distinct().ToList()));
                    _logger.LogError(ResultMessages.errorLogTracke + "ParseExcelRowsAsync()  : " + ex);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResultMessages.errorLogTracke + "ParseExcelRowsAsync()  : " + ex);
            }

          
            return new ResultDto
            {
                Info = $"No of processed rows: {passThroughCreateDataList.Count+ passThroughUpdateDataList.Count} rows and No of failed rows: {errorRows.Count} ", 
                Warning = (!errorRows.Any() && errorRows.Count == 0) ? true : false,
                Data = new { SuccessRecord = passThroughCreateDataList, ErrorDetails = errorRows }
            };

        }

        public async Task<ResultDto> ParseExcelRowsAsyncForPassthroughLcmSoftware(
             ISheet sheet, Dictionary<int, Exceltemplateconfiguration> columnMap, int dataRowIndex, string sheetName, long nonTemsVertical)
        {
            var updateList = new List<Swpassthroughlcm>();
            var createList = new List<Swpassthroughlcm>();
            var errorRows = new List<string>();
            var missedColumns = new List<string>();
            var duplicateKeys = new List<string>();
            try
            {
                var propertyMap = columnMap.ToDictionary(x => x.Key,
                                     x => new
                                     {
                                         x.Value.Columnheadername,
                                         Property = typeof(Swpassthroughlcm).GetProperty(ToFirstUpperRestLower(x.Value.Propertyname,true))
                                     });

                var existingPassthroughLcmEntities = _repositoryWrapper.SwPassThroughLcmRepository.FindAll();
                var existingSwKeys = existingPassthroughLcmEntities.Select(x => x.Swresourcekey).ToList();
                if (existingSwKeys != null && existingSwKeys.Count() > 0)
                {
                    var duplicates = existingSwKeys.Where(x => x != null).GroupBy(x => x).Where(u => u.Count() > 1).Select(g => g.Key);
                    if (duplicates != null && duplicates.Count() > 0) errorRows.Add($" A SwResourceKey {duplicates.FirstOrDefault()} cannot be duplicate ");
                }
                var existingResourceKeys = existingPassthroughLcmEntities.Select(x => x.Reportid).ToList();
                if (existingResourceKeys != null && existingResourceKeys.Count() > 0)
                {
                    var duplicates = existingResourceKeys.Where(x => x != null).GroupBy(x => x).Where(u => u.Count() > 1).Select(g => g.Key);
                    if (duplicates != null && duplicates.Count() > 0) errorRows.Add($" A ID {duplicates.FirstOrDefault()} cannot be duplicate ");
                }

                #region Check mandatory column is exists or not in uploaded excel sheet
                var idColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").Equals(ConstantValueFilter.passThroughLcmSWAndHWImportUniqueName, StringComparison.OrdinalIgnoreCase));
                var opcoColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").Equals(ConstantValueFilter.passThroughLcmOpco, StringComparison.OrdinalIgnoreCase));
                var assetClassColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").Equals(ConstantValueFilter.passThroughLcmAssetClass, StringComparison.OrdinalIgnoreCase));
                var swVersionColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").Equals(ConstantValueFilter.passThroughLcmSwVersion, StringComparison.OrdinalIgnoreCase));

                if (opcoColumn.Value == null) missedColumns.Add(ConstantValueFilter.passThroughLcmOpco);
                if (assetClassColumn.Value == null) missedColumns.Add(ConstantValueFilter.passThroughLcmAssetClass);
                if (swVersionColumn.Value == null) missedColumns.Add(ConstantValueFilter.passThroughLcmSwVersion);
                if (idColumn.Value == null) missedColumns.Add(ConstantValueFilter.passThroughLcmSWAndHWImportUniqueName);

                if (missedColumns.Any())
                {
                    errorRows.Add($"The uploaded file is missing the following required columns: {string.Join(", ", missedColumns)}");
                    return new ResultDto
                    {
                        Info = $"No of processed rows: {createList.Count + updateList.Count} rows and No of failed rows: {errorRows.Count} ",
                        Warning = (!errorRows.Any() && errorRows.Count == 0) ? true : false,
                        Data = new { SuccessRecord = createList.Count + updateList.Count, ErrorDetails = errorRows }
                    };
                }
                #endregion

                for (int rowIndex = dataRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    var row = sheet.GetRow(rowIndex);

                    if (row == null) continue;
                    if (IsRowEffectivelyEmpty(row))
                        continue;
                    if (row == null || row.Cells.All(c => string.IsNullOrWhiteSpace(c?.ToString()?.Replace(" ", "")) || string.IsNullOrEmpty(c?.ToString()?.Replace(" ", ""))))
                        continue;
                    if (!row.Any()) continue;

                    string reportId = row.GetCell(idColumn.Key)?.ToString()?.Trim() ?? string.Empty;
                    if (string.IsNullOrEmpty(reportId))
                    {
                        errorRows.Add($"Row {rowIndex} - {ConstantValueFilter.passThroughLcmSWAndHWImportUniqueName.ToUpper()} is required");
                        break;
                    }
                    bool isNewId = !existingResourceKeys.Contains(reportId);

                    #region if report id not exists in table 
                    if (isNewId)
                    {
                        string opco = row.GetCell(opcoColumn.Key)?.ToString()?.Trim() ?? string.Empty;
                        string assetClass = row.GetCell(assetClassColumn.Key)?.ToString()?.Trim() ?? string.Empty;
                        string swVersion = row.GetCell(swVersionColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                        bool allEmpty = string.IsNullOrEmpty(opco) &&
                                        string.IsNullOrEmpty(assetClass) &&
                                        string.IsNullOrEmpty(swVersion);
                        if (allEmpty)
                        {
                            errorRows.Add($"Row {rowIndex} - Please Fill mandatory field : {ConstantValueFilter.passThroughLcmOpco.ToUpper()} , { ConstantValueFilter.passThroughLcmAssetClass.ToUpper()} , {ConstantValueFilter.passThroughLcmSwVendor.ToUpper()}");
                            break;
                        }

                        var uniqueKeyExists = existingPassthroughLcmEntities.Where(x => x.Localmarket == opco
                                                                                      && x.Assetclass == assetClass
                                                                                      && x.Softwareversion == swVersion
                                                                                      && x.Nontemsvertical == nonTemsVertical).FirstOrDefault();

                        if (uniqueKeyExists != null && reportId != uniqueKeyExists.Reportid)
                        {
                            errorRows.Add($"Row {rowIndex} - {ConstantValueFilter.passThroughLcmSWAndHWImportUniqueName.ToUpper()} is mismatched  for the exist entry {ConstantValueFilter.passThroughLcmOpco.ToUpper()} , {ConstantValueFilter.passThroughLcmAssetClass.ToUpper()} , {ConstantValueFilter.passThroughLcmSwVendor.ToUpper()}");
                            break;
                        }
                    }
                    #endregion

                    #region Duplicate Validation for add/update row 
                    if (!string.IsNullOrEmpty(reportId))
                        duplicateKeys.Add(reportId);

                    if (duplicateKeys != null && duplicateKeys.Count() > 0)
                    {
                        var duplicates = duplicateKeys.Where(x => x != null).GroupBy(x => x).Where(u => u.Count() > 1).Select(g => g.Key);
                        if (duplicates != null && duplicates.Count() > 0)
                        {
                            errorRows.Add($"Row {rowIndex} - A ID {duplicates.FirstOrDefault()} cannot be duplicate ");
                            duplicateKeys.Remove(duplicates.FirstOrDefault());
                            continue;
                        }

                    }
                    #endregion

                    var existingEntity = existingPassthroughLcmEntities.Where(x => x.Reportid == reportId).FirstOrDefault();
                    var passThroughLcm = existingEntity != null? existingEntity:new Swpassthroughlcm();

                    foreach (var col in propertyMap)
                    {
                        var cellValue = row.GetCell(col.Key)?.ToString()?.Trim();
                        var prop = col.Value.Property;

                        if (prop == null) continue;

                        try
                        {

                            if (_commonManager.TrySetValueFromImportCell(passThroughLcm, prop.Name, cellValue)) continue;
                            else
                            {
                                errorRows.Add($"Row {rowIndex} - Data format issue in : {col.Value.Columnheadername.ToUpper()}");
                            }

                        }
                        catch
                        {

                        }

                    }
                    passThroughLcm.Nontemsvertical = (int?)nonTemsVertical;
                    if (string.IsNullOrEmpty(passThroughLcm.Swresourcekey))
                    {
                        var resourceKey=GenerateResourceKey(existingSwKeys, "A");
                        passThroughLcm.Swresourcekey = !existingSwKeys.Contains(resourceKey) ? resourceKey : string.Empty;
                        existingSwKeys.Add(resourceKey);
                    } 

                    if (existingEntity != null)
                    {
                        updateList.Add(passThroughLcm);
                    }
                    else
                    {
                        createList.Add(passThroughLcm);
                    }
                }
                try
                {

                    if (createList.Any())
                    {
                        _repositoryWrapper.SwPassThroughLcmRepository.BulkCreate(createList);
                        await _repositoryWrapper.SaveAsync();
                    }
                    await _repositoryWrapper.ClearTracker();
                    if (updateList.Any())
                    {
                        _repositoryWrapper.SwPassThroughLcmRepository.BulkUpdate(updateList);
                        await _repositoryWrapper.SaveAsync();
                    }
                    await _repositoryWrapper.ClearTracker();

                }
                catch (Exception ex)
                {
                    errorList.Add(createList.Count, string.Join(',', updateList.Select(x => x.Swresourcekey).Distinct().ToList()));
                    _logger.LogError(ResultMessages.errorLogTracke + "ParseExcelRowsAsyncForPassthroughLcmSoftware()  : " + ex);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResultMessages.errorLogTracke + "ParseExcelRowsAsyncForPassthroughLcmSoftware()  : " + ex);
            }


            return new ResultDto
            {
                Info = $"No of processed rows: {createList.Count + updateList.Count} rows and No of failed rows: {errorRows.Count} ",
                Warning = (!errorRows.Any() && errorRows.Count == 0) ? true : false,
                Data = new { SuccessRecord = createList.Count + updateList.Count, ErrorDetails = errorRows }
            };


        }
        public async Task<ResultDto> ParseExcelRowsAsyncForPassthroughLcmHardware(
             ISheet sheet, Dictionary<int, Exceltemplateconfiguration> columnMap, int dataRowIndex, string sheetName, long nonTemsVertical)
        {

            var updateList = new List<Hwpassthroughlcm>();
            var createList = new List<Hwpassthroughlcm>();
            var errorRows = new List<string>();
            var missedColumns = new List<string>();
            var duplicateKeys = new List<string>();
            string resourceKey = string.Empty;
            try
            {
                var propertyMap = columnMap.ToDictionary(x => x.Key,
                                     x => new
                                     {
                                         x.Value.Columnheadername,
                                         Property = typeof(Hwpassthroughlcm).GetProperty(ToFirstUpperRestLower(x.Value.Propertyname, true))
                                     });

                var existingPassthroughLcmEnitities = _repositoryWrapper.HwPassThroughLcmRepository.FindAll();
                var existingResourceKeys = existingPassthroughLcmEnitities.Select(x => x.Reportid).ToList();
                if (existingResourceKeys != null && existingResourceKeys.Count() > 0)
                {
                    var duplicates = existingResourceKeys.Where(x => x != null).GroupBy(x => x).Where(u => u.Count() > 1).Select(g => g.Key);
                    if (duplicates != null && duplicates.Count() > 0) errorRows.Add($" A ID {duplicates.FirstOrDefault()} cannot be duplicate ");

                }

                #region Check mandatory column is exists or not in uploaded excel sheet
                var idColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").Equals(ConstantValueFilter.passThroughLcmSWAndHWImportUniqueName, StringComparison.OrdinalIgnoreCase));
                var opcoColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").Equals(ConstantValueFilter.passThroughLcmOpco, StringComparison.OrdinalIgnoreCase));
                var assetClassColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").Equals(ConstantValueFilter.passThroughLcmAssetClass, StringComparison.OrdinalIgnoreCase));
                var hwModelColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").Equals(ConstantValueFilter.passThroughLcmHwModel, StringComparison.OrdinalIgnoreCase));

                if (opcoColumn.Value == null) missedColumns.Add(ConstantValueFilter.passThroughLcmOpco);
                if (assetClassColumn.Value == null) missedColumns.Add(ConstantValueFilter.passThroughLcmAssetClass);
                if (hwModelColumn.Value == null) missedColumns.Add(ConstantValueFilter.passThroughLcmHwModel);
                if (idColumn.Value == null) missedColumns.Add(ConstantValueFilter.passThroughLcmSWAndHWImportUniqueName);

                if (missedColumns.Any())
                {
                    errorRows.Add($"The uploaded file is missing the following required columns: {string.Join(", ", missedColumns)}");
                    return new ResultDto
                    {
                        Info = $"No of processed rows: {createList.Count + updateList.Count} rows and No of failed rows: {errorRows.Count} ",
                        Warning = (!errorRows.Any() && errorRows.Count == 0) ? true : false,
                        Data = new { SuccessRecord = createList.Count + updateList.Count, ErrorDetails = errorRows }
                    };
                }
                #endregion

                for (int rowIndex = dataRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    var row = sheet.GetRow(rowIndex);

                    if (row == null) continue;
                    if (IsRowEffectivelyEmpty(row))
                        continue;
                    if (row == null || row.Cells.All(c => string.IsNullOrWhiteSpace(c?.ToString()?.Replace(" ", "")) || string.IsNullOrEmpty(c?.ToString()?.Replace(" ", ""))))
                        continue;
                    if (!row.Any()) continue;

                    string reportId = row.GetCell(idColumn.Key)?.ToString()?.Trim() ?? string.Empty;
                    if (string.IsNullOrEmpty(reportId))
                    {
                        errorRows.Add($"Row {rowIndex} - {ConstantValueFilter.passThroughLcmSWAndHWImportUniqueName.ToUpper()} Value is required");
                        break;
                    }
                    bool isNewId = !existingResourceKeys.Contains(reportId);

                    #region if report id not exists in table 
                    if (isNewId)
                    {
                        string opco = row.GetCell(opcoColumn.Key)?.ToString()?.Trim() ?? string.Empty;
                        string assetClass = row.GetCell(assetClassColumn.Key)?.ToString()?.Trim() ?? string.Empty;
                        string hwModel = row.GetCell(hwModelColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                        bool allEmpty = string.IsNullOrEmpty(opco) &&
                                        string.IsNullOrEmpty(assetClass) &&
                                        string.IsNullOrEmpty(hwModel);
                        if (allEmpty)
                        {
                            errorRows.Add($"Row {rowIndex} - Please Fill mandatory field: {ConstantValueFilter.passThroughLcmOpco.ToUpper()} , {ConstantValueFilter.passThroughLcmAssetClass.ToUpper()} , {ConstantValueFilter.passThroughLcmHwModel.ToUpper()}");
                            break;
                        }
                        var uniqueKeyExists = existingPassthroughLcmEnitities.Where(x => x.Localmarket == opco
                                                                                    && x.Assetclass == assetClass
                                                                                    && x.Hardwaremodel == hwModel
                                                                                    && x.Nontemsvertical == nonTemsVertical).FirstOrDefault();

                        if (uniqueKeyExists != null && reportId!=uniqueKeyExists.Reportid)
                        {
                            errorRows.Add($"Row {rowIndex} - {ConstantValueFilter.passThroughLcmSWAndHWImportUniqueName.ToUpper()} is mismatched ");
                            break;
                        }
                    }
                    #endregion

                    #region Duplicate Validation for add/update row 
                    if (!string.IsNullOrEmpty(reportId))
                        duplicateKeys.Add(reportId);

                    if (duplicateKeys != null && duplicateKeys.Count() > 0)
                    {
                        var duplicates = duplicateKeys.Where(x => x != null).GroupBy(x => x).Where(u => u.Count() > 1).Select(g => g.Key);
                        if (duplicates != null && duplicates.Count() > 0)
                        {
                            errorRows.Add($"Row {rowIndex} - A ID {duplicates.FirstOrDefault()} cannot be duplicate ");
                            duplicateKeys.Remove(duplicates.FirstOrDefault());
                            continue;
                        }
                    }
                    #endregion 

                    var existingEntity = existingPassthroughLcmEnitities.Where(x => x.Reportid == reportId).FirstOrDefault();
                    var passThroughLcm = existingEntity != null? existingEntity:new Hwpassthroughlcm();

                    foreach (var col in propertyMap)
                    {
                        var cellValue = row.GetCell(col.Key)?.ToString()?.Trim();
                        var prop = col.Value.Property;

                        if (prop == null) continue;

                        try
                        {
                            if (_commonManager.TrySetValueFromImportCell(passThroughLcm, prop.Name, cellValue)) continue;
                            else
                            {
                                errorRows.Add($"Row {rowIndex} - Data format issue in : {col.Value.Columnheadername.ToUpper()}");
                            }
                        }
                        catch
                        {

                        }
                    }
                    passThroughLcm.Nontemsvertical = (int?)nonTemsVertical;
                    if (existingEntity != null)
                    {
                        updateList.Add(passThroughLcm);
                    }
                    else
                    {
                        createList.Add(passThroughLcm);
                    }
                }
                try
                {

                    if (createList.Any())
                    {
                        _repositoryWrapper.HwPassThroughLcmRepository.BulkCreate(createList);
                        await _repositoryWrapper.SaveAsync();
                    }
                    await _repositoryWrapper.ClearTracker();
                    if (updateList.Any())
                    {
                        _repositoryWrapper.HwPassThroughLcmRepository.BulkUpdate(updateList);
                        await _repositoryWrapper.SaveAsync();
                    }
                    await _repositoryWrapper.ClearTracker();

                }
                catch (Exception ex)
                {
                    errorList.Add(createList.Count, string.Join(',', updateList.Select(x => x.Hwresourcekey).Distinct().ToList()));
                    _logger.LogError(ResultMessages.errorLogTracke + "ParseExcelRowsAsyncForPassthroughLcmSoftware()  : " + ex);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResultMessages.errorLogTracke + "ParseExcelRowsAsyncForPassthroughLcmSoftware()  : " + ex);
            }


            return new ResultDto
            {
                Info = $"No of processed rows: {createList.Count + updateList.Count} rows and No of failed rows: {errorRows.Count} ",
                Warning = (!errorRows.Any() && errorRows.Count == 0) ? true : false,
                Data = new { SuccessRecord = createList.Count + updateList.Count, ErrorDetails = errorRows }
            };


        }





        public static string ToFirstUpperRestLower(string input,bool isLcm = false)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            input = isLcm == true ? input.Replace("PassThrough", "").Trim() : input.Replace("Tsr", "").Replace("me","").Trim();

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        private string GenerateResourceKey(List<string> existingKeys,string initialLetter)
        {
            const string chars = "ABCDEF0123456789";
            var random = new Random();
            string newKey;
            do
            {
                var builder = new StringBuilder(initialLetter); 
                for (int i = 0; i < 6; i++) 
                {
                    builder.Append(chars[random.Next(chars.Length)]);
                }
                newKey = builder.ToString();

            } while (existingKeys.Contains(newKey)); 

            return newKey;
        }


    }
}
