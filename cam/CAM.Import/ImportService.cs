using CAM.BusinessManager.CommonUtilities;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElementAsIs;
using CAM.Infrastucture;
using ClosedXML.Excel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using OracleModels.DBModels;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Reflection;
using System.Text;
using static CAM.Imports.VBomImportService;

namespace CAM.Imports
{
    public class ImportService : IImportService
    {
        private readonly LcmImport _lcm;
        private readonly NetworkElementAsIsImport _networkelementasis;
        DeliveryTrackingsImport _deliveryTrackings;
        private readonly IdentityAsIsImport _identityAsIs;
        private readonly TsrPassThroughImport _tsrPassThrough;
        private readonly NfviSoftwareCompatibilityImport _nfviSoftwareCompatibilityImport;
        private readonly ProjectPlanImport _projectPlanImport;
        private readonly VBomImportService _vBomImportService;
        private readonly CommonManager _commonManager;

        private readonly CBomImportService _cBomImportService;
        private readonly BPTImport _bPTImport;
        private readonly PassThroughImport _passThroughImport;
        private readonly TemsFntImport _temsFntImport;


        public ImportService(LcmImport lcm, NetworkElementAsIsImport networkelementasis, DeliveryTrackingsImport deliveryTrackings, ProjectPlanImport project, IdentityAsIsImport identityAsIs,
            TsrPassThroughImport tsrPassThrough,CommonManager commonManager,NfviSoftwareCompatibilityImport nfviSoftwareCompatibilityImport, VBomImportService vBomImportService, 
            CBomImportService cBomImportService, BPTImport bPTImport, PassThroughImport passThroughImport, TemsFntImport temsFntImport)
        {
            _lcm = lcm;
            _networkelementasis = networkelementasis;
            _deliveryTrackings = deliveryTrackings;
            _identityAsIs = identityAsIs;
            _tsrPassThrough = tsrPassThrough;
            _nfviSoftwareCompatibilityImport = nfviSoftwareCompatibilityImport;
            _vBomImportService = vBomImportService;
            _cBomImportService = cBomImportService;
            _bPTImport = bPTImport;
            _projectPlanImport = project;
            _commonManager = commonManager;
             _passThroughImport = passThroughImport;
            _temsFntImport = temsFntImport;
        } 

        /// <summary>
        /// This function is to process the files imported from respective entities like LCM, Delivery tracking etc.,
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="importfiletype"></param>
        /// <returns></returns>
        public async Task<ResultDto> Processimportexcel(IFormFile file, string importfiletype)
        {
            string errorDescription = string.Empty;
            string noRowsUpdated = string.Empty;
            bool hasError = false;
            string SheetName = string.Empty;
            string IdColumn = string.Empty;
            string excelName = string.Empty;
            List<string> EditableColumnList = new List<string>();
            NetworkElementAsIsDtoGrid networkasisInsertionRecords = new NetworkElementAsIsDtoGrid();

            try
            {


                var ValidateExcel = IsValideExcel(file);
                if (ValidateExcel)
                {

                    if (importfiletype == "lcm")
                    {
                        SheetName = _lcm.SheetName;
                        IdColumn = _lcm.IdColumn;
                        EditableColumnList = _lcm.EditableColumnList;
                        excelName = _lcm.excelName;
                    }
                    else if (importfiletype == "networkelementasis")
                    {
                        SheetName = _networkelementasis.SheetName;
                        IdColumn = _networkelementasis.IdColumn;
                        EditableColumnList = _networkelementasis.EditableColumnList;
                        excelName = _networkelementasis.excelName;
                    }
                    else if (importfiletype == "deliverytrackings")
                    {
                        SheetName = _deliveryTrackings.SheetName;
                        IdColumn = _deliveryTrackings.IdColumn;
                        EditableColumnList = _deliveryTrackings.EditableColumnList;
                        excelName = _deliveryTrackings.excelName;
                    }
                    else if (importfiletype == "identityAsIs")
                    {
                        SheetName = _identityAsIs.SheetName;
                        IdColumn = _identityAsIs.IdColumn;
                        EditableColumnList = _identityAsIs.EditableColumnList;
                        excelName = _identityAsIs.excelName;
                    }
                    else if (importfiletype == "nfvisoftwarecompatibility")
                    {
                        SheetName = _nfviSoftwareCompatibilityImport.SheetName;
                        IdColumn = _nfviSoftwareCompatibilityImport.IdColumn;
                        EditableColumnList = _nfviSoftwareCompatibilityImport.EditableColumnList;
                        excelName = _nfviSoftwareCompatibilityImport.excelName;
                    }
                    else
                    {
                        //not handled
                    }



                    List<Dictionary<string, string>> processData = new List<Dictionary<string, string>>();
                    Dictionary<string, string> rowData = new Dictionary<string, string>();

                    Dictionary<string, string> InsertingColumns = new Dictionary<string, string>();
                    Dictionary<int, string> InsertingColumnNo = new Dictionary<int, string>();
                    List<Dictionary<string, string>> processDataforInsertion = new List<Dictionary<string, string>>();


                    string? key = "";
                    string? value = "";
                    //get lcm details
                    if (ValidateSheetName(file, SheetName))
                    {
                        if (ValidateColumnCheck(file, IdColumn, EditableColumnList, SheetName))
                        {
                            var stream = file.OpenReadStream();
                            var bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, (int)stream.Length);
                            var fileContent = Encoding.UTF8.GetString(bytes);

                            var package = new ExcelPackage(stream);
                            var sheetNames = new List<string>();
                            if (package.Workbook.Worksheets.Count >= 1)
                            {
                                sheetNames.Add(_deliveryTrackings.SheetName);
                                //sheetNames.Add(_projectPlanImport.SheetName);
                            }
                            else
                            {
                                sheetNames.Add(SheetName);
                            }
                            foreach (var sheetName in sheetNames)
                            {
                                if (sheetName == _deliveryTrackings.SheetName && importfiletype == "nfvisoftwarecompatibility")
                                {
                                    IdColumn = _nfviSoftwareCompatibilityImport.IdColumn;
                                    EditableColumnList = _nfviSoftwareCompatibilityImport.EditableColumnList;
                                    excelName = _nfviSoftwareCompatibilityImport.excelName;
                                }
                                else if (sheetName == _deliveryTrackings.SheetName)
                                {
                                    IdColumn = _deliveryTrackings.IdColumn;
                                    EditableColumnList = _deliveryTrackings.EditableColumnList;
                                    excelName = _deliveryTrackings.excelName;
                                }
                                //else if (sheetName == _projectPlanImport.SheetName)
                                //{
                                //    IdColumn = _projectPlanImport.IdColumn;
                                //    EditableColumnList = _projectPlanImport.EditableColumnList;
                                //    excelName = _projectPlanImport.excelName;
                                //    importfiletype = "projectplan";
                                //    processData = new List<Dictionary<string, string>>();
                                //}
                                var sheet = package.Workbook.Worksheets[sheetName];
                                int IdColumnNo = 0;
                                Dictionary<int, string> editableColumnNo = new Dictionary<int, string>();
                                for (int row = 1; row <= sheet.Dimension.End.Row; row++)
                                {
                                    //var data = _networkelementasis.MappeExcelDataToGrid(sheet, row);

                                    if (row == 1)
                                    {
                                        for (int col = 1; col <= sheet.Dimension.End.Column; col++)
                                        {
                                            string? val = sheet.Cells[1, col].Value?.ToString();
                                            if (val.Equals(IdColumn))
                                            {
                                                IdColumnNo = col;
                                            }
                                            if (EditableColumnList.Contains(val))
                                            {
                                                editableColumnNo[col] = val;
                                            }

                                            InsertingColumnNo[col] = val;
                                        }
                                        continue;
                                    }
                                    else
                                    {
                                        rowData = new Dictionary<string, string>();
                                        InsertingColumns = new Dictionary<string, string>();

                                        for (int col = 1; col <= sheet.Dimension.End.Column; col++)
                                        {

                                            if (editableColumnNo.ContainsKey(col))
                                            {
                                                string? val = sheet.Cells[row, col].Value?.ToString();

                                                var columnName = editableColumnNo[col];
                                                rowData[columnName] = val;
                                            }
                                            else if (col == IdColumnNo)
                                            {
                                                string? val = sheet.Cells[row, col].Value?.ToString();
                                                rowData[IdColumn] = val;
                                            }
                                            if (InsertingColumnNo.ContainsKey(col))
                                            {
                                                string? val = sheet.Cells[row, col].Value?.ToString();

                                                var columnName = InsertingColumnNo[col];
                                                InsertingColumns[columnName] = val;
                                            }
                                        }

                                        processData.Add(rowData);
                                        if (importfiletype == "networkelementasis")
                                        {
                                            processDataforInsertion.Add(InsertingColumns);
                                        }
                                    }
                                }
                                if (importfiletype == "lcm")
                                {
                                    var update = await _lcm.UpdateExcelColumn(processData, processDataforInsertion);
                                    if (!update.Warning)
                                    {
                                        hasError = true;
                                        errorDescription += update.Info;
                                    }
                                    else
                                    {
                                        noRowsUpdated = update.Info;
                                    }
                                }
                                else if (importfiletype == "networkelementasis")
                                {
                                    var update = await _networkelementasis.UpdateExcelColumn(processData, processDataforInsertion);
                                    if (!update.Warning)
                                    {
                                        hasError = true;
                                        errorDescription += update.Info;
                                    }
                                    else
                                    {
                                        noRowsUpdated = update.Info;
                                    }
                                }
                                else if (importfiletype == "deliverytrackings")
                                {
                                    var update = await _deliveryTrackings.UpdateExcelColumn(processData, processDataforInsertion);
                                    if (!update.Warning)
                                    {
                                        hasError = true;
                                        errorDescription += update.Info;
                                    }
                                    else
                                    {
                                        noRowsUpdated = update.Info;
                                    }
                                }
                                else if (importfiletype == "identityAsIs")
                                {
                                    var update = await _identityAsIs.UpdateExcelColumn(processData, processDataforInsertion);
                                    if (!update.Warning)
                                    {
                                        hasError = true;
                                        errorDescription += update.Info;
                                    }
                                    else
                                    {
                                        noRowsUpdated = update.Info;
                                    }
                                }
                                else if (importfiletype == "nfvisoftwarecompatibility")
                                {
                                    var update = await _nfviSoftwareCompatibilityImport.UpdateExcelColumn(processData, processDataforInsertion);
                                    if (!update.Warning)
                                    {
                                        hasError = true;
                                        errorDescription += update.Info;
                                    }
                                    else
                                    {
                                        noRowsUpdated = update.Info;
                                    }
                                }
                                else if (importfiletype == "projectplan")
                                {
                                    var update = await _projectPlanImport.UpdateExcelColumn(processData, processDataforInsertion);
                                    if (!update.Warning)
                                    {
                                        hasError = true;
                                        errorDescription += " and " + update.Info + " in Project Plan";
                                    }
                                    else
                                    {
                                        if (errorDescription.Length > 0)
                                        {
                                            errorDescription += "and " + update.Info + " in Project Plan";
                                        }
                                        noRowsUpdated += update.Info;
                                    }
                                }

                                else
                                {
                                    //not handled
                                }
                            }

                        }
                        else
                        {

                            return new ResultDto
                            {
                                Info = "Primary column (" + IdColumn + ") or Editable Column (" + string.Join(", ", EditableColumnList.ToArray()) + ") are missing in the file " + excelName + " to import data please download the excel from \" + excelName + \" update the values and try to upload again",
                                Warning = false
                            };

                        }

                    }
                    else
                    {
                        return new ResultDto
                        {
                            // excel file did not contain 
                            Info = "Excel file did not contain correct sheetname, please download the excel from " + excelName + " update the values and try to upload again",
                            Warning = false
                        };

                    }
                }
                else
                {
                    return new ResultDto
                    {
                        Info = "Invalid File - Please upload xlsx file",
                        Warning = false
                    };

                }
            }
            catch (Exception ex)
            {
                return new ResultDto { Info = ex.Message };
            }

            return new ResultDto
            {
                Info = errorDescription.Length == 0 && noRowsUpdated.Contains("record") ? noRowsUpdated : errorDescription.Length == 0 ? noRowsUpdated + " Rows Successfully Updated" : errorDescription,
                Warning = errorDescription.Length == 0 ? true : false,

            };
        }

        public bool IsValideExcel(IFormFile importFile)
        {
            bool isValid = false;
            try
            {

                if (importFile == null)
                {
                    return false;

                }

                String filename = importFile.FileName;
                Stream FileStream = importFile.OpenReadStream();

                if (importFile != null && FileStream != null)
                {
                    if (importFile.FileName.EndsWith(".xlsx"))
                    {
                        isValid = true;
                    }
                    else
                    {
                        return false;

                    }
                }

            }
            catch (Exception ex)
            {
                return false;

            }
            if (isValid)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool ValidateColumnCheck(IFormFile file, string pkcolumnname, List<string> EditableColumnList, string SheetName)
        {
            bool haseditcolumn = false;
            bool haspkcolumn = false;
            try
            {
                var stream = file.OpenReadStream();
                var package = new ExcelPackage(stream);
                var sheet = package.Workbook.Worksheets[SheetName];
                var headers = sheet.Cells[1, 1, 1, sheet.Dimension.Columns].Select(cell => cell.Text).ToList();

                foreach (string editcolumn in EditableColumnList)
                {
                    if (headers.Contains(editcolumn))
                    {
                        haseditcolumn = true;
                    }

                }
                if (headers.Contains(pkcolumnname))
                {
                    haspkcolumn = true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            if (haseditcolumn && haspkcolumn)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public bool ValidateSheetName(IFormFile file, string sheetname)
        {
            var stream = file.OpenReadStream();
            var package = new ExcelPackage(stream);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            foreach (var sheet in package.Workbook.Worksheets)
            {
                if (sheet.Name == sheetname)
                {
                    return true;

                }

            }
            return false;
        }


        public async Task<ResultDto> BulkImport(IFormFile file, long recordClassifier, long? nonTemsVertical)
        {
            if (file == null) return new ResultDto { Info = ResultMessages.NoFile, Warning = false };
            return await _tsrPassThrough.BulkImportOptimizedCode(file, recordClassifier, nonTemsVertical);
        }

        public async Task<ResultDto> ImportDataBasedOnTemplateConfiguration(IFormFile formFile, string processName, List<Exceltemplateconfiguration> excelConfigData, long nonTemsVertical = 0)
        {
            string errorDescription = string.Empty;
            string noRowsUpdated = string.Empty;

            var result = new List<Dictionary<string, string>>();
            var headers = new Dictionary<int, string>();

            if (formFile == null) return new ResultDto { Info = ResultMessages.NoFile, Warning = false };

            using var excelStream = formFile.OpenReadStream();
            var workbook = new HSSFWorkbook(excelStream);

            excelConfigData = excelConfigData.Where(x => x.Isimportfield == true).ToList();

            int headerRowIndex = Convert.ToInt16(excelConfigData.FirstOrDefault(x => x.Headerrowstarting != null)?.Headerrowstarting ?? 0);
            int dataRowIndex = Convert.ToInt16(excelConfigData.FirstOrDefault(x => x.Rowstarting != null)?.Rowstarting ?? 1);

            var sheet = workbook.GetSheetAt(0);
            var headerRow = sheet.GetRow(headerRowIndex);
            var columnMap = new Dictionary<int, Exceltemplateconfiguration>();

            #region Read Excel Header

            var headerColumnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                var header = headerRow.GetCell(i)?.ToString()?.Trim();
                var match = excelConfigData.FirstOrDefault(c =>
                     string.Equals(c.Columnheadername?.Trim(), header, StringComparison.OrdinalIgnoreCase));

                if (match != null)
                    columnMap[i] = match;
            }
            #endregion


            if (processName == ConstantValueFilter.bptExcelProcessName)
                return await _bPTImport.ParseExcelRowsAsync(sheet, columnMap, dataRowIndex, sheet.SheetName);

            if (processName == ConstantValueFilter.passThroughExcelProcessName)
                return await _passThroughImport.ParseExcelRowsAsync(sheet, columnMap, dataRowIndex, sheet.SheetName, nonTemsVertical);



            return new ResultDto
            {

                Warning = false,
                Data = "Process Failed"

            };
        }

        public async Task<ResultDto> ImportDataBasedOnTemplateConfigurationForXL(IFormFile formFile, string processName, List<Exceltemplateconfiguration> excelConfigData, long nonTemsVertical = 0)
        {
            string errorDescription = string.Empty;
            string noRowsUpdated = string.Empty;

            var result = new List<Dictionary<string, string>>();
            var headers = new Dictionary<int, string>();

            if (formFile == null) return new ResultDto { Info = ResultMessages.NoFile, Warning = false };

            using var excelStream = formFile.OpenReadStream();
            var workbook = new XSSFWorkbook(excelStream);

            excelConfigData = excelConfigData.Where(x => x.Isimportfield == true).ToList();

            int headerRowIndex = Convert.ToInt16(excelConfigData.FirstOrDefault(x => x.Headerrowstarting != null)?.Headerrowstarting ?? 0);
            int dataRowIndex = Convert.ToInt16(excelConfigData.FirstOrDefault(x => x.Rowstarting != null)?.Rowstarting ?? 1);

            var sheet = workbook.GetSheetAt(0);
            var headerRow = sheet.GetRow(headerRowIndex);
            var columnMap = new Dictionary<int, Exceltemplateconfiguration>();

            #region Read Excel Header

            var headerColumnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                var header = headerRow.GetCell(i)?.ToString()?.Trim();
                var match = excelConfigData.FirstOrDefault(c =>
                     string.Equals(c.Columnheadername?.Trim(), header, StringComparison.OrdinalIgnoreCase));

                if (match != null)
                    columnMap[i] = match;
            }
            #endregion


            if (processName == ConstantValueFilter.TemsFntReport)
                return await _temsFntImport.ParseExcelRowsAsync(sheet, columnMap, dataRowIndex, sheet.SheetName, nonTemsVertical);

            if (processName == ConstantValueFilter.passThroughExcelProcessName)
                return await _passThroughImport.ParseExcelRowsAsync(sheet, columnMap, dataRowIndex, sheet.SheetName, nonTemsVertical);

            if (processName == ConstantValueFilter.passThroughLcmSoftware)
                return await _passThroughImport.ParseExcelRowsAsyncForPassthroughLcmSoftware(sheet, columnMap, dataRowIndex, sheet.SheetName, nonTemsVertical);

            if (processName == ConstantValueFilter.passThroughLcmHardware)
                return await _passThroughImport.ParseExcelRowsAsyncForPassthroughLcmHardware(sheet, columnMap, dataRowIndex, sheet.SheetName, nonTemsVertical);





            return new ResultDto
            {

                Warning = false,
                Data = "Process Failed"

            };
        }


        public static List<T> ParseExcelRows<T>(
            ISheet sheet,
            Dictionary<int, Exceltemplateconfiguration> columnMap,
            int dataRowIndex,
            string processName
        ) where T : new()
        {
            var result = new List<T>();

            for (int rowIndex = dataRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null) continue;

                T dto = new T();

                foreach (var colIndex in columnMap.Keys)
                {
                    var cellValue = row.GetCell(colIndex)?.ToString()?.Trim();
                    var propertyName = columnMap[colIndex].Columnheadername; // or Propertyname

                    if (string.IsNullOrEmpty(propertyName))
                        propertyName = columnMap[colIndex].Propertyname;

                    if (string.IsNullOrEmpty(propertyName))
                        continue;

                    PropertyInfo prop = null;

                    // Handle special process name if needed
                    if (processName.Equals(ConstantValueFilter.bptExcelProcessName, StringComparison.OrdinalIgnoreCase)
                        && typeof(T) == typeof(Budgetprojecttrackers))
                    {
                        prop = typeof(Budgetprojecttrackers).GetProperty(propertyName);
                    }
                    else
                    {
                        prop = typeof(T).GetProperty(propertyName);
                    }

                    if (prop != null && !string.IsNullOrEmpty(cellValue))
                    {
                        try
                        {
                            object safeValue = Convert.ChangeType(cellValue, prop.PropertyType);
                            prop.SetValue(dto, safeValue);
                        }
                        catch
                        {
                            // Optional: log or handle conversion error
                        }
                    }
                }

                result.Add(dto);
            }

            return result;
        }


        #region //VBOM
        public async Task<ResultDto> ImportDataBasedOnTemplateConfigurationForVbom(IFormFile formfile, string ProcessName, List<Exceltemplateconfiguration> excelConfig, string fileName)
        {
            using var excelstreem = formfile.OpenReadStream();
            var workbook = new XSSFWorkbook(excelstreem);
            int dataRowIndex = 0;
            int headerRowIndex = 0;
            List<string> error = new List<string>();
            var columnMap = new Dictionary<int, Exceltemplateconfiguration>();

            var splitFileName = fileName.Split('_');
            var opCo = splitFileName[2];
            var harwareType = splitFileName[1];
            var revision = splitFileName[6];

            for (int SheetIndex = 6; SheetIndex <= workbook.NumberOfSheets - 1; SheetIndex++)
            {
                ISheet sheet = workbook.GetSheetAt(SheetIndex);
                if (sheet.SheetName.Trim().ToLower().Replace(" ", "") == "externalserverforccdmca")
                {
                    break;
                }
                else
                {

                    var fydetails = new Dictionary<string, FyPosition>();
                    foreach (var merge in sheet.MergedRegions)
                    {
                        IRow row = sheet.GetRow(merge.FirstRow);
                        ICell cell = row.GetCell(merge.FirstColumn);
                        var val = cell.ToString();
                        if (val.Trim().ToLower().Replace(" ", "") == "commonrequirements")
                        {
                            headerRowIndex = merge.FirstRow + 1;
                            dataRowIndex = merge.FirstRow + 2;
                        }
                        if (val.Contains("FY"))
                        {
                            var fyposition = new VBomImportService.FyPosition
                            {
                                FyFirstIndex = merge.FirstColumn,
                                FyLastIndex = merge.LastColumn,
                            };
                            fydetails[val] = fyposition;
                        }
                    }
                    var headerRow = sheet.GetRow(headerRowIndex);
                    for (int i = 0; i < headerRow.LastCellNum; i++)
                    {
                        var header = headerRow.GetCell(i)?.ToString()?.Trim();
                        var match = excelConfig.FirstOrDefault(c => string.Equals(c.Columnheadername?.Trim().ToLower().Replace(" ", ""), header?.ToLower().Replace(" ", ""), StringComparison.OrdinalIgnoreCase));
                        if (match != null)
                        {
                            columnMap[i] = match;
                        }
                    }
                    if (ProcessName == ConstantValueFilter.vbomExcelProcessName)
                    {
                        var splitSheetName = sheet.SheetName.Split('_');
                        var clusterName = splitSheetName[0];
                        var siteName = splitSheetName[1];
                        var result = await _vBomImportService.parseExcelRowAsync(sheet, columnMap, dataRowIndex, opCo, harwareType, revision,
                            fileName, siteName, clusterName, fydetails);
                        if (result.Data != "Success")
                        {
                            error.Add($"{sheet.SheetName} {string.Join(", ", result.Data)}");
                        }
                    }
                }
            }
            if (error != null && error.Count <= 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Data = "Successfully Processed",
                    Info = "Successfully Processed"
                };
            }
            else
            {
                return new ResultDto
                {
                    Warning = false,
                    Data = $"Please correct the data in sheet(s): {string.Join(", ", error)}",
                    Info = $"Please correct the data in sheet(s): {string.Join(", ", error)}"
                };
            }


        }

        #endregion

        #region //CBOM

        public async Task<ResultDto> ImportDataBasedOnTemplateConfigurationForCbom(IFormFile formfile, string ProcessName, List<Exceltemplateconfiguration> excelConfig, List<Exceltemplateconfiguration> excelConfigForCnf, string fileName)
        {
            using var excelstreem = formfile.OpenReadStream();
            var workbook = new XSSFWorkbook(excelstreem);
            int dataRowIndex = 0;
            int headerRowIndex = 0;
            List<string> error = new List<string>();
            var columnMap = new Dictionary<int, Exceltemplateconfiguration>();
            var cnfColumnMap = new Dictionary<int, Exceltemplateconfiguration>();


            var splitFileName = fileName.Split('_');
            var opCo = splitFileName[0];
            var splitHarwareType = splitFileName[6].Split('-');
            var hardwareType = splitHarwareType[0];
            var splitRevision = splitHarwareType[1].Split(' ');
            var revision = string.Empty;
            if (splitRevision.Count() > 1)
            {
                 revision = splitRevision[1];
            }

            ISheet sheet = workbook.GetSheetAt(9);
            ISheet cnfSheet = workbook.GetSheetAt(8);

            var fydetails = new Dictionary<string, CBomImportService.FyPosition>();
            foreach (var merge in sheet.MergedRegions)
            {
                IRow row = sheet.GetRow(merge.FirstRow);
                ICell cell = row.GetCell(merge.FirstColumn);
                var val = cell.ToString();
                if (!(val.IsNullOrEmpty()) && val.Contains("FY"))
                {
                    headerRowIndex = merge.FirstRow + 1;
                    dataRowIndex = merge.FirstRow + 2;
                    var fyposition = new CBomImportService.FyPosition
                    {
                        FyFirstIndex = merge.FirstColumn,
                        FyLastIndex = merge.LastColumn,
                    };
                    fydetails[val] = fyposition;
                }
            }
            var headerRow = sheet.GetRow(headerRowIndex);
            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                var header = headerRow.GetCell(i)?.ToString()?.Trim();
                var match = excelConfig.FirstOrDefault(c => string.Equals(_commonManager.NormalizeKey(c.Columnheadername), _commonManager.NormalizeKey(header), StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    columnMap[i] = match;
                }
            }
            var cnfHeaderRow = cnfSheet.GetRow(0);
            for (int i = 0; i < cnfHeaderRow.LastCellNum; i++)
            {
                var header = cnfHeaderRow.GetCell(i)?.ToString()?.Trim();
                var match = excelConfigForCnf.FirstOrDefault(c => string.Equals(_commonManager.NormalizeKey(c.Columnheadername), _commonManager.NormalizeKey(header), StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    cnfColumnMap[i] = match;
                }
            }
            if (ProcessName == ConstantValueFilter.cbomExcelProcessName)
            {
                var result = await _cBomImportService.ParseExcelRowAsync(sheet, columnMap, dataRowIndex, opCo, hardwareType, revision,
                    fileName, fydetails);

                var cnfResult = await _cBomImportService.CNFParseExcelRowAsync(cnfSheet, cnfColumnMap, 1, opCo, hardwareType, revision,
                    fileName, fydetails);


                if (result.Data != "Success")
                {
                    error.Add($"{sheet.SheetName} {string.Join(", ", result.Data)}");
                }
                if (cnfResult.Data != "Success")
                {
                    error.Add($"{cnfSheet.SheetName} {string.Join(", ", cnfResult.Data)}");
                }

            }


            if (error != null && error.Count <= 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Data = "Successfully Processed",
                    Info = "Successfully Processed"
                };
            }
            else
            {
                return new ResultDto
                {
                    Warning = false,
                    Data = $"Please correct the data in sheet(s): {string.Join(", ", error)}",
                    Info = $"Please correct the data in sheet(s): {string.Join(", ", error)}"
                };
            }


        }


        #endregion
    }
}