using AutoMapper;
using CAM.BusinessManager;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.Infrastucture;
using ClosedXML.Excel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using OracleModels.DBModels;
using System.Data;
using System.Reflection;


namespace CAM.Imports
{
    public class VBomImportService : BaseManager
    {

        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly CommonManager _commonManager;
        public VBomImportService(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, IMapper mapper,
            IHttpContextAccessor contextAccessor, ILoggerManager logger,CommonManager commonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _commonManager = commonManager;
        }

        #region //VBOM

        public async Task<ResultDto> parseExcelRowAsync(ISheet sheet, Dictionary<int, Exceltemplateconfiguration> columnMap, int dataRowIndex, string opCo, string hardwareType, string revision, string fileName, string siteName,
            string clusterName, Dictionary<string, FyPosition> fyPosition)
        {
            var updateVnfInstanceList = new List<Vnfinfo>();
            var InsertVnfInstanceList = new List<Vnfinfo>();
            var updateVnfVmCapacityList = new List<Vnfvmcapacity>();
            var insertVnfVmCapacityList = new List<Vnfvmcapacity>();
            var errorRows = new List<string>();
            var noSRowUpdate = 0;
            Vnfclusterinfo vnfClusterInfoEntity = null;
            var existingVnfClusterInfoEntity = new Vnfclusterinfo();

            try
            {
                

                for (int rowIndex = dataRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    existingVnfClusterInfoEntity = await _repositoryWrapper.VnfClusterInfoRepository.FindByCondition(x => x.Opco.Opco.Trim().ToLower() ==
                   opCo.Trim().ToLower() && x.Location.Shortdescription.Trim().ToLower() == siteName.Trim().ToLower() &&
                   x.Clustername.Clusterdescription.Trim().ToLower() == clusterName.Trim().ToLower() && x.Revision == Convert.ToInt32(revision) && x.Hardwaretype.Description.Trim().ToLower() == hardwareType.Trim().ToLower())
                   .Include(x => x.Vnfinfo).ThenInclude(x => x.Vnfvmcapacity)
                   .Include(x => x.Vnfinfo).ThenInclude(x => x.Vnfname)
                   .Include(x => x.Vnfinfo).ThenInclude(x => x.Vnfvmtypename).FirstOrDefaultAsync();

                    bool hasMandatoryOrDataTypeError = false;

                    var errorRowNumber = string.Empty;

                    var row = sheet.GetRow(rowIndex);

                    if (row == null) { continue; }

                    if (IsRowEffectivelyEmpty(row)) { continue; }

                    if (row == null || row.Cells.All(c => string.IsNullOrWhiteSpace(c?.ToString()?.Replace(" ", ""))
                   || string.IsNullOrEmpty(c?.ToString()?.Replace(",", ""))))
                    {
                        continue;
                    }

                    if (!row.Any()) { continue; }

                    // VFN Info Fields
                    #region// get Mandatory fields
                    var vnfNameColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                     ConstantValueFilter.vnfName);

                    string vnfNameColumnValue = row.GetCell(vnfNameColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var vmTypeNameColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                     ConstantValueFilter.vmTypeName);

                    string vmTypeNameColumnValue = row.GetCell(vmTypeNameColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var existingVnInfo = existingVnfClusterInfoEntity?.Vnfinfo.Where(x => x.Vnfname.Vnfdescription.Trim().ToLower() == vnfNameColumnValue.ToLower() &&
                    x.Vnfvmtypename.Vmtypedescription.Trim().ToLower() == vmTypeNameColumnValue.ToLower())
                          .FirstOrDefault();

                    #endregion

                    #region // Drop down 
                    //vnfname
                    var checkVnfNameExist = _repositoryWrapper.VnfNameRepository.FindByCondition(x => x.Vnfdescription.Trim().ToLower().Replace(" ", "")
                    == vnfNameColumnValue.ToLower().Replace(" ", "")).FirstOrDefault();

                    var checkVmTypeNameExist = new Vmtypename();
                    if (checkVnfNameExist != null)
                    {
                        // vmtype
                        checkVmTypeNameExist = _repositoryWrapper.VmTypeNameRepository.FindByCondition(x => x.Vmtypedescription.Trim().ToLower().Replace(" ", "") ==
                        vmTypeNameColumnValue.ToLower().Replace(" ", "") && x.Vnfnameid == checkVnfNameExist.Vnfnameid).FirstOrDefault();
                    }

                    var intraVMColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                     ConstantValueFilter.intra);

                    string intraVMColumnValue = row.GetCell(intraVMColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var interVmColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                     ConstantValueFilter.inter);

                    string interVmColumnValue = row.GetCell(interVmColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var workloadTypeColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                     ConstantValueFilter.workload);

                    string workloadTypeColumnValue = row.GetCell(workloadTypeColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    //inter
                    var checkInterExist = _repositoryWrapper.InterVmTypeRepository.FindByCondition(x => x.Interdescription.Trim().ToLower().Replace(" ", "") ==
                    interVmColumnValue.ToLower().Replace(" ", "")).FirstOrDefault();
                    // intra
                    var checkIntraExist = _repositoryWrapper.IntraVmTypeRepository.FindByCondition(x => x.Intradescription.Trim().ToLower().Replace(" ", "") ==
                    intraVMColumnValue.ToLower().Replace(" ", "")).FirstOrDefault();
                    // workload
                    var checkWorkloadExist = _repositoryWrapper.VnfWorkLoadTypeRepository.FindByCondition(x => x.Description.Trim().ToLower().Replace(" ", "") ==
                    workloadTypeColumnValue.ToLower().Replace(" ", "")).FirstOrDefault();

                    var checkHardwareType = _repositoryWrapper.VnfHardwareRepository.FindByCondition(x => x.Description.Trim().ToLower().Replace(" ", "") ==
                    hardwareType.Trim().ToLower().Replace(" ", "")).FirstOrDefault();
                    #endregion

                    if (existingVnfClusterInfoEntity != null)
                    {
                        existingVnfClusterInfoEntity.Filename = fileName;

                        // read instance data
                        if (existingVnInfo == null)
                        {
                            existingVnInfo = new Vnfinfo();
                            #region //drop down

                            if (checkVnfNameExist != null)
                            {
                                existingVnInfo.Vnfnameid = checkVnfNameExist.Vnfnameid;
                            }
                            else
                            {
                                hasMandatoryOrDataTypeError = true;
                                errorRows.Add($"Row {rowIndex} - This VNF Name '{vnfNameColumnValue.ToUpper()}' not in TEMS, ");
                            }

                            if (checkVmTypeNameExist != null && checkVnfNameExist != null && checkVmTypeNameExist.Vnfnameid == checkVnfNameExist.Vnfnameid)
                            {
                                existingVnInfo.Vnfvmtypenameid = checkVmTypeNameExist.Vmtypenameid;
                            }
                            //else if (checkVmTypeNameExist == null)
                            //{
                            //    hasMandatoryOrDataTypeError = true;
                            //    errorRows.Add($"Row {rowIndex} - This VNF VMType Name '{vmTypeNameColumnValue.ToUpper()}' not in TEMS, ");
                            //}
                            else
                            {
                                errorRows.Add($"The VNF VMType Name '{vmTypeNameColumnValue.ToUpper()}' is not linked with VNF Name '{vnfNameColumnValue.ToUpper()}', ");
                            }

                            if (checkInterExist != null)
                            {
                                existingVnInfo.Intervmtypeid = checkInterExist.Intervmtypeid;
                            }
                            else
                            {
                                hasMandatoryOrDataTypeError = true;
                                errorRows.Add($"Row {rowIndex} - This Inter VM Type '{interVmColumnValue.ToUpper()}' not in TEMS, ");
                            }

                            if (checkIntraExist != null)
                            {
                                existingVnInfo.Intravmtypeid = checkIntraExist.Intravmtypeid;
                            }
                            else
                            {
                                hasMandatoryOrDataTypeError = true;
                                errorRows.Add($"Row {rowIndex} - This Intra VM Type '{intraVMColumnValue.ToUpper()}' not in TEMS, ");
                            }

                            if (checkWorkloadExist != null)
                            {
                                existingVnInfo.Vmworkloadtypeid = checkWorkloadExist.Vmworkloadtypeid;
                            }
                            else
                            {
                                hasMandatoryOrDataTypeError = true;
                                errorRows.Add($"Row {rowIndex} - This VM Workload Type '{workloadTypeColumnValue.ToUpper()}' not in TEMS, ");
                            }
                            #endregion
                        }

                        foreach (var colIndex in columnMap.Keys)
                        {
                            if (colIndex > 13)
                            {
                                break;
                            }
                            else
                            {
                                var cellValue = row.GetCell(colIndex)?.ToString()?.Trim();
                                var Columnheadername = columnMap[colIndex].Columnheadername;
                                var propertyName = Columnheadername; // or Propertyname

                                if (string.IsNullOrEmpty(propertyName))
                                    propertyName = columnMap[colIndex].Propertyname;

                                if (string.IsNullOrEmpty(propertyName))
                                    continue;


                                PropertyInfo prop = typeof(Vnfinfo).GetProperty(propertyName);

                                if (prop != null && string.IsNullOrEmpty(cellValue)) continue;

                                var excelConfigTemp = columnMap.FirstOrDefault(x => x.Key == colIndex);
                                var columnName = excelConfigTemp.Value.Propertyname.Trim();
                                var excelValue = row.GetCell(colIndex)?.ToString()?.Trim();
                                var masterTable = excelConfigTemp.Value.Mappingreference;
                                var isMandatory = excelConfigTemp.Value?.Ismandatory ?? false;

                                if (columnName.ToLower() == "nsxt" && excelValue.IsNullOrEmpty())
                                {
                                    excelValue = "no";
                                }

                                object? convertedValue = masterTable switch
                                {
                                    _ => _commonManager.ConvertValueToTypeForImport(excelValue)
                                };

                                if (string.IsNullOrEmpty(excelValue) && isMandatory)
                                {
                                    hasMandatoryOrDataTypeError = true;
                                    errorRows.Add($"Row {rowIndex} - Missing mandatory field: {Columnheadername.ToUpper()}");
                                }


                                try
                                {
                                    if (existingVnInfo != null && existingVnInfo.Vnfinfoid != 0)
                                    {
                                        if (_commonManager.TrySetValueFromImportCell(existingVnInfo, columnName, convertedValue)) continue;
                                    }
                                    if (existingVnInfo != null && existingVnInfo.Vnfinfoid == 0)
                                    {
                                        // insert
                                        existingVnInfo.Vnfclusterinfoid = existingVnfClusterInfoEntity.Vnfclusterinfoid;
                                        if (_commonManager.TrySetValueFromImportCell(existingVnInfo, columnName, convertedValue)) continue;
                                    }

                                }

                                catch (Exception ex)
                                {
                                    errorRows.Add($"{ex.Message} - Vnfinfo is null");
                                }
                            }
                        }
                        if (errorRows.Count > 0)
                        {
                            break;
                        }
                        else
                        {
                            if (existingVnInfo != null && existingVnInfo.Vnfinfoid != 0)
                            {
                                _repositoryWrapper.VnfInfoRepository.Update(existingVnInfo);
                            }
                            if (existingVnInfo != null && existingVnInfo.Vnfinfoid == 0)
                            {
                                _repositoryWrapper.VnfInfoRepository.Create(existingVnInfo);
                            }
                            await _repositoryWrapper.SaveAsync();
                            await _repositoryWrapper.ClearTracker();

                            // read Fy data
                            foreach (var fy in fyPosition)
                            {
                                int colIndex = fy.Value.FyFirstIndex;
                                var val = row.GetCell(colIndex)?.ToString()?.Trim();
                                var headername = columnMap[colIndex].Propertyname;
                                if (headername.Trim().ToLower() == "noofvnfinstances" && val.IsNullOrEmpty())
                                {
                                    continue;
                                }
                                else
                                {
                                    Vnfvmcapacity vnfVmCapacityEntity = null;

                                    if (existingVnInfo != null)
                                    {
                                        if (existingVnInfo.Vnfvmcapacity != null && existingVnInfo.Vnfvmcapacity.Count > 0)
                                        {
                                            vnfVmCapacityEntity = new Vnfvmcapacity();

                                            var existingCapacitys = existingVnInfo.Vnfvmcapacity.
                                            Where(f => f.Financialyear == Convert.ToInt16(RechangeFinancialYear(fy.Key))).ToList();

                                            if (existingCapacitys.Count > 1 && existingCapacitys != null)
                                            {
                                                vnfVmCapacityEntity = existingCapacitys.Where(x => x.Financialversion == 2).FirstOrDefault();
                                            }
                                            else
                                            {
                                                vnfVmCapacityEntity = existingCapacitys.FirstOrDefault();
                                            }
                                        }

                                        if (vnfVmCapacityEntity == null)
                                        {
                                            vnfVmCapacityEntity = new Vnfvmcapacity();
                                            vnfVmCapacityEntity.Financialversion = 1;
                                            vnfVmCapacityEntity.Financialyear = Convert.ToInt16(RechangeFinancialYear(fy.Key));
                                        }

                                        for (var col = colIndex; col <= fy.Value.FyLastIndex; col++)
                                        {
                                            var cellValue = row.GetCell(col)?.ToString()?.Trim();
                                            var Columnheadername = columnMap[col].Columnheadername;
                                            var propertyName = Columnheadername; // or Propertyname

                                            if (string.IsNullOrEmpty(propertyName))
                                                propertyName = columnMap[col].Propertyname;

                                            if (string.IsNullOrEmpty(propertyName))
                                                continue;


                                            PropertyInfo prop = typeof(Vnfvmcapacity).GetProperty(propertyName);

                                            if (prop != null && string.IsNullOrEmpty(cellValue)) continue;

                                            var excelConfigTemp = columnMap.FirstOrDefault(x => x.Key == col);
                                            var columnName = excelConfigTemp.Value.Propertyname.Trim();
                                            if (columnName == "vcpuPerVm")
                                            {
                                                columnName = "vnfcpupervm";
                                            }
                                            var excelValue = row.GetCell(col)?.ToString()?.Trim();
                                            var masterTable = excelConfigTemp.Value.Mappingreference;
                                            var isMandatory = excelConfigTemp.Value?.Ismandatory ?? false;

                                            if (columnName.ToLower() == "vnfcpupervm" && excelValue.IsNullOrEmpty())
                                            {
                                                excelValue = "0";
                                            }
                                            if (columnName.ToLower() == "rampervm" && excelValue.IsNullOrEmpty())
                                            {
                                                excelValue = "0";
                                            }
                                            if (columnName.ToLower() == "datadisk" && excelValue.IsNullOrEmpty())
                                            {
                                                excelValue = "0";
                                            }
                                            if (columnName.ToLower() == "noofvnfinstances" && excelValue.IsNullOrEmpty())
                                            {
                                                excelValue = "0";
                                            }
                                            if (columnName.ToLower() == "noofvmspertype" && excelValue.IsNullOrEmpty())
                                            {
                                                excelValue = "0";
                                            }

                                            object? convertedValue = masterTable switch
                                            {
                                                _ => _commonManager.ConvertValueToTypeForImport(excelValue)
                                            };


                                            if (string.IsNullOrEmpty(excelValue) && isMandatory)
                                            {
                                                hasMandatoryOrDataTypeError = true;
                                                errorRows.Add($"Row {rowIndex} - Missing mandatory field: {Columnheadername.ToUpper()}");
                                            }

                                            try
                                            {
                                                if (vnfVmCapacityEntity != null && vnfVmCapacityEntity.Vnfvmcapacityid != 0)
                                                {
                                                    if (_commonManager.TrySetValueFromImportCell(vnfVmCapacityEntity, columnName, convertedValue)) continue;
                                                }
                                                if (vnfVmCapacityEntity != null && vnfVmCapacityEntity.Vnfvmcapacityid == 0)
                                                {
                                                    vnfVmCapacityEntity.Vnfinfoid = existingVnInfo.Vnfinfoid;
                                                    if (_commonManager.TrySetValueFromImportCell(vnfVmCapacityEntity, columnName, convertedValue)) continue;
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                errorRows.Add($"{ex.Message} - Capacity is null");
                                            }
                                        }

                                        if (vnfVmCapacityEntity != null && vnfVmCapacityEntity.Vnfvmcapacityid != 0)
                                        {
                                            _repositoryWrapper.VnfVmCapacityRepository.Update(vnfVmCapacityEntity);
                                        }
                                        if (vnfVmCapacityEntity != null && vnfVmCapacityEntity.Vnfvmcapacityid == 0)
                                        {
                                            _repositoryWrapper.VnfVmCapacityRepository.Create(vnfVmCapacityEntity);
                                        }
                                        await _repositoryWrapper.SaveAsync();

                                    }
                                }
                            }

                        }

                    }
                    else if (existingVnfClusterInfoEntity == null)
                    {
                        vnfClusterInfoEntity = new Vnfclusterinfo();
                        vnfClusterInfoEntity.Filename = fileName;
                        vnfClusterInfoEntity.Revision = Convert.ToInt32(revision);
                        vnfClusterInfoEntity.Noofblades = 1;

                        #region //drop down
                        //hardwaretype
                        if (checkHardwareType != null)
                        {
                            vnfClusterInfoEntity.Hardwaretypeid = checkHardwareType.Vnfhardwareid;
                        }
                        else
                        {
                            errorRows.Add($"This hardwaretype '{hardwareType.ToUpper()}' is not in TEMS, ");
                        }

                        // opco
                        var opcoExistEntity = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.Trim().ToLower().Replace(" ", "") == opCo.Trim().ToLower().Replace(" ", "")).FirstOrDefault();

                        if (opcoExistEntity != null)
                        {
                            vnfClusterInfoEntity.Opcoid = opcoExistEntity.Opcoid;
                        }
                        else
                        {
                            hasMandatoryOrDataTypeError = true;
                            errorRows.Add($"This opco '{opCo.ToUpper()}' is not in TEMS, ");
                        }

                        // cluster
                        var ClusterExistEntity = _repositoryWrapper.ClusterNameRepository.FindByCondition(x => x.Clusterdescription.Trim().ToLower().Replace(" ", "") == clusterName.Trim().ToLower().Replace(" ", "")).FirstOrDefault();

                        if (ClusterExistEntity != null)
                        {
                            vnfClusterInfoEntity.Clusternameid = ClusterExistEntity.Clusternameid;
                        }
                        else
                        {
                            hasMandatoryOrDataTypeError = true;
                            errorRows.Add($"This cluster name '{clusterName.ToUpper()}' is not in TEMS, ");
                        }

                        // sitename
                        if (opcoExistEntity != null)
                        {
                            var LocationExistEntity = _repositoryWrapper.Location.FindByCondition(x => x.Shortdescription.Trim().ToLower().Replace(" ", "") == 
                            siteName.Trim().ToLower().Replace(" ", "") && x.Opcoid == opcoExistEntity.Opcoid).FirstOrDefault();

                            if (LocationExistEntity != null)
                            {
                                vnfClusterInfoEntity.Locationid = LocationExistEntity.Locationid;
                            }
                            else if(LocationExistEntity == null)
                            {
                                errorRows.Add($"This Site '{siteName.ToUpper()}' is not in TEMS, ");
                            }
                            else
                            {
                                hasMandatoryOrDataTypeError = true;
                                errorRows.Add($"The site name '{siteName.ToUpper()}' is not linked with OPCO '{opCo.ToUpper()}', ");
                            }
                        }
                        #endregion


                        if (errorRows.Count > 0)
                        {

                            break;
                        }
                        else
                        {
                            _repositoryWrapper.VnfClusterInfoRepository.Create(vnfClusterInfoEntity);
                            await _repositoryWrapper.SaveAsync();

                            // read instance data
                            existingVnInfo = new Vnfinfo();
                            existingVnInfo.Vnfclusterinfoid = vnfClusterInfoEntity.Vnfclusterinfoid;

                            if (checkVnfNameExist != null)
                            {
                                existingVnInfo.Vnfnameid = checkVnfNameExist.Vnfnameid;
                            }
                            else
                            {
                                hasMandatoryOrDataTypeError = true;
                                errorRows.Add($"Row {rowIndex} - This VNF Name '{vnfNameColumnValue.ToUpper()}' not in TEMS, ");
                            }

                            if (checkVmTypeNameExist != null && checkVnfNameExist != null && checkVmTypeNameExist.Vnfnameid == checkVnfNameExist.Vnfnameid)
                            {
                                existingVnInfo.Vnfvmtypenameid = checkVmTypeNameExist.Vmtypenameid;
                            }
                            else if (checkVmTypeNameExist == null)
                            {
                                hasMandatoryOrDataTypeError = true;
                                errorRows.Add($"Row {rowIndex} - This VNF VMType Name '{vmTypeNameColumnValue.ToUpper()}' not in TEMS, ");
                            }
                            else
                            {
                                errorRows.Add($"The VNF VMType Name '{vmTypeNameColumnValue.ToUpper()}' is not linked with VNF Name '{vnfNameColumnValue.ToUpper()}', ");
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


                                PropertyInfo prop = typeof(Vnfinfo).GetProperty(propertyName);

                                if (prop != null && string.IsNullOrEmpty(cellValue)) continue;

                                var excelConfigTemp = columnMap.FirstOrDefault(x => x.Key == colIndex);
                                var columnName = excelConfigTemp.Value.Propertyname.Trim();
                                var excelValue = row.GetCell(colIndex)?.ToString()?.Trim();
                                var masterTable = excelConfigTemp.Value.Mappingreference;
                                var isMandatory = excelConfigTemp.Value?.Ismandatory ?? false;


                                if ((columnName.ToLower() == "intravmtype" || columnName.ToLower() == "intervmtype" || columnName.ToLower() == "vmworkloadtype") && existingVnInfo != null)
                                {
                                    
                                    if (checkInterExist != null)
                                    {
                                        existingVnInfo.Intervmtypeid = checkInterExist.Intervmtypeid;
                                    }
                                    else
                                    {
                                        hasMandatoryOrDataTypeError = true;
                                        errorRows.Add($"Row {rowIndex} - This Inter VM Type '{interVmColumnValue.ToUpper()}' not in TEMS, ");
                                    }
                                  
                                    if (checkIntraExist != null)
                                    {
                                        existingVnInfo.Intravmtypeid = checkIntraExist.Intravmtypeid;
                                    }
                                    else
                                    {
                                        hasMandatoryOrDataTypeError = true;
                                        errorRows.Add($"Row {rowIndex} - This Intra VM Type '{intraVMColumnValue.ToUpper()}' not in TEMS, ");
                                    }                                   

                                    if (checkWorkloadExist != null)
                                    {
                                        existingVnInfo.Vmworkloadtypeid = checkWorkloadExist.Vmworkloadtypeid;
                                    }
                                    else
                                    {
                                        hasMandatoryOrDataTypeError = true;
                                        errorRows.Add($"Row {rowIndex} - This VM Workload Type '{workloadTypeColumnValue.ToUpper()}' not in TEMS, ");
                                    }
                                }

                                if (columnName.ToLower() == "nsxt" && excelValue.IsNullOrEmpty())
                                {
                                    excelValue = "no";
                                }
                                object? convertedValue = masterTable switch
                                {

                                    _ => _commonManager.ConvertValueToTypeForImport(excelValue)
                                };

                                if (string.IsNullOrEmpty(excelValue) && isMandatory)
                                {
                                    hasMandatoryOrDataTypeError = true;
                                    errorRows.Add($"Row {rowIndex} - Missing mandatory field: {Columnheadername.ToUpper()}");
                                }

                                try
                                {
                                    if (existingVnInfo != null)
                                    {
                                        if (_commonManager.TrySetValueFromImportCell(existingVnInfo, columnName, convertedValue)) continue;
                                    }
                                }

                                catch (Exception ex)
                                {
                                    errorRows.Add($"{ex.Message} - Vnfinfo is null");
                                }
                            }

                            if (errorRows.Count > 0)
                            {
                                break;
                            }
                            else
                            {
                                _repositoryWrapper.VnfInfoRepository.Create(existingVnInfo);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();


                                // read Fy data
                                foreach (var fy in fyPosition)
                                {
                                    int colIndex = fy.Value.FyFirstIndex;
                                    var val = row.GetCell(colIndex)?.ToString()?.Trim();
                                    var headername = columnMap[colIndex].Propertyname;
                                    if (headername.Trim().ToLower() == "noofvnfinstances" && val.IsNullOrEmpty())
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (existingVnInfo != null)
                                        {
                                            var vnfVmCapacityEntity = new Vnfvmcapacity();
                                            vnfVmCapacityEntity.Vnfinfoid = existingVnInfo.Vnfinfoid;
                                            vnfVmCapacityEntity.Financialversion = 1;
                                            vnfVmCapacityEntity.Financialyear = Convert.ToInt16(RechangeFinancialYear(fy.Key));

                                            for (var col = colIndex; col <= fy.Value.FyLastIndex; col++)
                                            {
                                                var cellValue = row.GetCell(col)?.ToString()?.Trim();
                                                var Columnheadername = columnMap[col].Columnheadername;
                                                var propertyName = Columnheadername; // or Propertyname

                                                if (string.IsNullOrEmpty(propertyName))
                                                    propertyName = columnMap[col].Propertyname;

                                                if (string.IsNullOrEmpty(propertyName))
                                                    continue;


                                                PropertyInfo prop = typeof(Vnfvmcapacity).GetProperty(propertyName);

                                                if (prop != null && string.IsNullOrEmpty(cellValue)) continue;

                                                var excelConfigTemp = columnMap.FirstOrDefault(x => x.Key == col);
                                                var columnName = excelConfigTemp.Value.Propertyname.Trim();
                                                if (columnName == "vcpuPerVm")
                                                {
                                                    columnName = "vnfcpupervm";
                                                }
                                                if (columnName.ToLower() == "northdouthboundbandwidth")
                                                {
                                                    columnName = "northsouthboundbandwidth";
                                                }
                                                var excelValue = row.GetCell(col)?.ToString()?.Trim();
                                                var masterTable = excelConfigTemp.Value.Mappingreference;
                                                var isMandatory = excelConfigTemp.Value?.Ismandatory ?? false;

                                                if (columnName.ToLower() == "vnfcpupervm" && excelValue.IsNullOrEmpty())
                                                {
                                                    excelValue = "0";
                                                }
                                                if (columnName.ToLower() == "rampervm" && excelValue.IsNullOrEmpty())
                                                {
                                                    excelValue = "0";
                                                }
                                                if (columnName.ToLower() == "datadisk" && excelValue.IsNullOrEmpty())
                                                {
                                                    excelValue = "0";
                                                }
                                                if (columnName.ToLower() == "noofvnfinstances" && excelValue.IsNullOrEmpty())
                                                {
                                                    excelValue = "0";
                                                }
                                                if (columnName.ToLower() == "noofvmspertype" && excelValue.IsNullOrEmpty())
                                                {
                                                    excelValue = "0";
                                                }

                                                object? convertedValue = masterTable switch
                                                {
                                                    _ => _commonManager.ConvertValueToTypeForImport(excelValue)
                                                };

                                                if (string.IsNullOrEmpty(excelValue) && isMandatory)
                                                {
                                                    hasMandatoryOrDataTypeError = true;
                                                    errorRows.Add($"Row {rowIndex} - Missing mandatory field: {Columnheadername.ToUpper()}");
                                                }

                                                try
                                                {
                                                    if (vnfVmCapacityEntity != null)
                                                    {
                                                        if (_commonManager.TrySetValueFromImportCell(vnfVmCapacityEntity, columnName, convertedValue)) continue;

                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    errorRows.Add($"{ex.Message} - Capacity is null");
                                                }
                                            }
                                            if (errorRows != null && errorRows.Count > 0)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                _repositoryWrapper.VnfVmCapacityRepository.Create(vnfVmCapacityEntity);
                                                await _repositoryWrapper.SaveAsync();
                                                await _repositoryWrapper.ClearTracker();
                                            }

                                        }

                                    }


                                }
                            }
                        }
                    }
                    if (errorRows != null && errorRows.Count <= 0)
                    {
                        noSRowUpdate++;
                    }

                }
                try
                {

                    if (existingVnfClusterInfoEntity != null && errorRows.Count <= 0)
                    {
                        _repositoryWrapper.VnfClusterInfoRepository.Update(existingVnfClusterInfoEntity);
                        await _repositoryWrapper.SaveAsync();

                    }
                    await _repositoryWrapper.ClearTracker();

                }
                catch (Exception ex)
                {
                    errorRows.Add(ex.Message);
                    _logger.LogError(ResultMessages.errorLogTracke + "ParseExcelRowsAsync()  : " + ex);
                }

                return new ResultDto
                {
                    Info = $"No of processed rows: {noSRowUpdate} and No of failed rows: {errorRows.Count} ",
                    Warning = (!errorRows.Any() && errorRows.Count == 0) ? true : false,
                    Data = errorRows.Count > 0 ? $"Faild records {string.Join(", ", errorRows)}" : "Success"
                };
            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Data = $"{ex.Message}",
                };
            }

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
        #endregion

        
 
        public class FyPosition
        {
            public int FyFirstIndex { get; set; }
            public int FyLastIndex { get; set; }
        }

        public string RechangeFinancialYear(string input)
        {
            var result = string.Empty;
            input = input.Trim();
            if (input.StartsWith("FY", StringComparison.OrdinalIgnoreCase))
            {
                input = input.Substring(2).Trim();
            }

            // Case 1: Input is a financial year (e.g. "24/25")
            if (input.Contains("/"))
            {
                var parts = input.Split('/');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int start) &&
                    int.TryParse(parts[1], out int end))
                {
                    // Handle century rollover (e.g., "99/00" → 1999, "20/01" → 2000)
                    if (end < start)
                    {
                        result = $"20{start:D2}";
                    }
                    else
                    {
                        result = $"20{start:D2}";
                    }
                }
            }
            return result;
        }
    }
}
