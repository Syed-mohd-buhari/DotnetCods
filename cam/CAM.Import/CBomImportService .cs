using CAM.BusinessManager;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.Entities.Models.CBom;
using CAM.Infrastucture;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using OracleModels.DBModels;
using System.Data;
using System.Reflection;


namespace CAM.Imports
{
    public class CBomImportService : BaseManager
    {

        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly CommonManager _commonManager;
        public CBomImportService(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper,
            IHttpContextAccessor contextAccessor, ILoggerManager logger,CommonManager commonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _commonManager = commonManager;
        }

        #region //VBOM

        public async Task<ResultDto> ParseExcelRowAsync(ISheet sheet, Dictionary<int, Exceltemplateconfiguration> columnMap, int dataRowIndex, string opCo, string hardwareType,
            string revision, string fileName, Dictionary<string, FyPosition> fyPosition)
        {
            var updateCnfPodInfoList = new List<Cnfpodinfo>();
            var InsertCnfPodInfoList = new List<Cnfpodinfo>();
            var updateCnfCapacityList = new List<Cnfcapacity>();
            var insertCnfCapacityList = new List<Cnfcapacity>();
            var errorRows = new List<string>();
            var noSRowUpdate = 0;
            Cnfclusterinfo cnfClusterInfoEntity = null;
            var existingCnfClusterInfoEntity = new Cnfclusterinfo();
            try
            {
               

                for (int rowIndex = dataRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
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

                    #region// get Mandatory fields // CNF Name //K8 cluster //Node Pool //Pode Type // Pode Des

                    var cnfNameColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                     ConstantValueFilter.cnfNmae);

                    string cnfNameColumnValue = row.GetCell(cnfNameColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var cnfClusterColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                     ConstantValueFilter.cluster);

                    string cnfClusterColumnValue = row.GetCell(cnfClusterColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var cnfPoolColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                    ConstantValueFilter.nodePool);

                    string cnfPoolColumnValue = row.GetCell(cnfPoolColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var cnfPodTypeColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                    ConstantValueFilter.podType);

                    string cnfPodTypeColumnValue = row.GetCell(cnfPodTypeColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var cnfPodDesColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                    ConstantValueFilter.descriptionOfEachPodRole);

                    string cnfPodDesColumnValue = row.GetCell(cnfPodDesColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var siteColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                    ConstantValueFilter.siteName);

                    string siteColumnValue = row.GetCell(siteColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var funColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                    ConstantValueFilter.functionStandardName);

                    string funColumnValue = row.GetCell(funColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var verticalColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                    ConstantValueFilter.verticalDomianOwner);

                    string verticalColumnValue = row.GetCell(verticalColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var priorityColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                    ConstantValueFilter.priority);

                    string priorityColumnValue = row.GetCell(priorityColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    #endregion

                    #region // Drop down 
                    //cnfPodeTypename
                    var checkVnfNameExist = await _repositoryWrapper.PodTypeInfoRepository.FindByCondition(x => x.Podtypeinfoname.Trim().ToLower().Replace(" ", "")
                    == cnfPodTypeColumnValue.ToLower().Replace(" ", "") &&
                    x.Podroledescription.Trim().ToLower().Replace(" ", "") == cnfPodDesColumnValue.ToLower().Replace(" ", "")).FirstOrDefaultAsync();
                    //// podetypeDescription
                    //var checkPodTypeDesExist = _repositoryWrapper.PodTypeInfoRepository.FindByCondition(x => x.Podroledescription.Trim().ToLower().Replace(" ", "") ==
                    //cnfPodDesColumnValue.ToLower().Replace(" ", "")).FirstOrDefault();
                    // function
                    var checkFunctionExist = _repositoryWrapper.FunctionStandardNameRepository.FindByCondition(x => x.Functionname.Trim().ToLower().Replace(" ", "") ==
                    funColumnValue.ToLower().Replace(" ", "")).FirstOrDefault();
                    // priority
                    var checkPriorityExist = _repositoryWrapper.CnfPriorityRepository.FindByCondition(x => x.Description.Trim().ToLower().Replace(" ", "") ==
                    priorityColumnValue.ToLower().Replace(" ", "")).FirstOrDefault();
                    
                    // vertical
                    var verticalExistEntity = _repositoryWrapper.VerticalResponsible.FindByCondition(x => x.Verticalresponsible.Trim().ToLower().Replace(" ", "") ==
                        verticalColumnValue.Trim().ToLower().Replace(" ", "")).FirstOrDefault();

                    // hardware type
                    var hardwareTypeEntityExist = _repositoryWrapper.CnfHardwareRepository.FindByCondition(x => x.Description.Trim().ToLower().Replace(" ", "") ==
                    hardwareType.Trim().ToLower().Replace(" ", "")).FirstOrDefault();

                    #endregion

                    existingCnfClusterInfoEntity = await _repositoryWrapper.CnfClusterInfoRepository.FindByCondition(x => x.Opco.Opco.Trim().ToLower().Replace(" ", "") ==
                        opCo.Trim().ToLower().Replace(" ", "") && x.Site.Shortdescription.Trim().ToLower().Replace(" ","") == siteColumnValue.Trim().ToLower().Replace(" ","") &&
                        x.Cnfcluster.Cnfclustername.Trim().ToLower().Replace(" ", "") == cnfClusterColumnValue.Trim().ToLower().Replace(" ", "") &&
                        x.Cnfcluster.Nodepool.Trim().ToLower().Replace(" ","") == cnfPoolColumnValue.Trim().ToLower().Replace(" ", "") &&
                        x.Cnfhardware.Description.Trim().ToLower().Replace(" ", "") == hardwareType.Trim().ToLower().Replace(" ", "")
                        && x.Cnfname.Cnfdescription.Trim().ToLower().Replace(" ", "") == cnfNameColumnValue.Trim().ToLower().Replace(" ", ""))
                        .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Cnfcapacity)
                        .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Podroledescription)
                        .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Podtypeinfo)
                        .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Functionstandard)
                        .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Priority).FirstOrDefaultAsync();

                    if (existingCnfClusterInfoEntity != null)
                    {
                        existingCnfClusterInfoEntity.Filename = fileName;


                        var existingCnfPod = existingCnfClusterInfoEntity.Cnfpodinfo.Where(x => x.Podtypeinfo.Podtypeinfoname.ToLower().Trim() == cnfPodTypeColumnValue.ToLower()
                        && x.Podtypeinfo.Podroledescription.ToLower().Trim() == cnfPodDesColumnValue.ToLower()).FirstOrDefault();

                        if (existingCnfPod == null)
                        {
                            existingCnfPod = new Cnfpodinfo();
                            existingCnfPod.Cnfclusterinfoid = existingCnfClusterInfoEntity.Cnfclusterinfoid;
                        }
                        if (existingCnfPod != null)
                        {
                            if (checkVnfNameExist != null)
                            {
                                existingCnfPod.Podtypeinfoid = checkVnfNameExist.Podtypeinfoid;
                                existingCnfPod.Podroledescriptionid = checkVnfNameExist.Podtypeinfoid;
                            }
                            else
                            {
                                errorRows.Add($"Row {rowIndex} - This Pod Type '{cnfPodTypeColumnValue.ToUpper()}' and Pod Description '{cnfPodDesColumnValue.ToUpper()}' not is TEMS, ");
                            }

                            if (checkFunctionExist != null)
                            {
                                existingCnfPod.Functionstandardid = checkFunctionExist.Functionstandardnameid;
                            }
                            else
                            {
                                hasMandatoryOrDataTypeError = true;
                                errorRows.Add($"Row {rowIndex} - This Function Standard '{funColumnValue.ToUpper()}' is not in TEMS, ");
                            }
                            if (checkPriorityExist != null)
                            {
                                existingCnfPod.Priorityid = checkPriorityExist.Cnfpriorityid;
                            }
                            else
                            {
                                hasMandatoryOrDataTypeError = true;
                                errorRows.Add($"Row {rowIndex} - This Priority '{priorityColumnValue.ToUpper()}' is not in TEMS, ");
                            }
                        }


                        foreach (var colIndex in columnMap.Keys)
                        {
                            if (colIndex > 17)
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


                                PropertyInfo prop = typeof(Cnfpodinfo).GetProperty(propertyName);

                                if (prop != null && string.IsNullOrEmpty(cellValue)) continue;

                                var excelConfigTemp = columnMap.FirstOrDefault(x => x.Key == colIndex);
                                var columnName = excelConfigTemp.Value.Propertyname.Trim();
                                var excelValue = row.GetCell(colIndex)?.ToString()?.Trim();
                                var masterTable = excelConfigTemp.Value.Mappingreference;
                                var isMandatory = excelConfigTemp.Value?.Ismandatory ?? false;
                               
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
                                    if (existingCnfPod != null && existingCnfPod.Cnfpodinfoid != 0)
                                    {
                                        if (_commonManager.TrySetValueFromImportCell(existingCnfPod, columnName, convertedValue)) continue;
                                    }
                                    if (existingCnfPod != null && existingCnfPod.Cnfpodinfoid == 0)
                                    {
                                        // insert
                                        if (_commonManager.TrySetValueFromImportCell(existingCnfPod, columnName, convertedValue)) continue;
                                    }

                                }

                                catch (Exception ex)
                                {
                                    errorRows.Add($"{ex.Message} - Cnfpodinfo is null");
                                }
                            }
                        }
                        if (errorRows.Count > 0)
                        {
                            break;
                        }
                        else
                        {
                            if (existingCnfPod != null && existingCnfPod.Cnfpodinfoid != 0)
                            {
                                _repositoryWrapper.CnfPodInfoRepository.Update(existingCnfPod);
                            }
                            if (existingCnfPod != null && existingCnfPod.Cnfpodinfoid == 0)
                            {
                                _repositoryWrapper.CnfPodInfoRepository.Create(existingCnfPod);
                            }
                            await _repositoryWrapper.SaveAsync();
                            await _repositoryWrapper.ClearTracker();

                            // read Fy data
                            foreach (var fy in fyPosition)
                            {
                                int colIndex = fy.Value.FyFirstIndex;
                                var val = row.GetCell(colIndex)?.ToString()?.Trim();
                                var headername = columnMap[colIndex].Propertyname;
                                if (headername.Trim().ToLower() == "numberofcnfinstancespersite" && val.IsNullOrEmpty())
                                {
                                    continue;
                                }
                                else
                                {
                                    Cnfcapacity cnfCapacityEntity = null;
                                    var FY = _commonManager.GetPropertFY(fy.Key);

                                    if (existingCnfPod != null)
                                    {
                                        if (existingCnfPod.Cnfcapacity != null && existingCnfPod.Cnfcapacity.Count > 0)
                                        {
                                            cnfCapacityEntity = new Cnfcapacity();

                                            var existingCapacitys = existingCnfPod.Cnfcapacity.
                                            Where(f => f.Financialyear == Convert.ToInt16(RechangeFinancialYear(FY))).ToList();

                                            if (existingCapacitys.Count > 1 && existingCapacitys != null)
                                            {
                                                cnfCapacityEntity = existingCapacitys.Where(x => x.Financialversion == 2).FirstOrDefault();
                                            }
                                            else
                                            {
                                                cnfCapacityEntity = existingCapacitys.FirstOrDefault();
                                            }
                                        }

                                        if (cnfCapacityEntity == null)
                                        {
                                            cnfCapacityEntity = new Cnfcapacity();
                                            cnfCapacityEntity.Financialversion = 1;
                                            cnfCapacityEntity.Financialyear = Convert.ToInt16(RechangeFinancialYear(FY));
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


                                            PropertyInfo prop = typeof(Cnfcapacity).GetProperty(propertyName);

                                            if (prop != null && string.IsNullOrEmpty(cellValue)) continue;

                                            var excelConfigTemp = columnMap.FirstOrDefault(x => x.Key == col);
                                            var columnName = excelConfigTemp.Value.Propertyname.Trim();                                            
                                            var excelValue = row.GetCell(col)?.ToString()?.Trim();
                                            var masterTable = excelConfigTemp.Value.Mappingreference;
                                            var isMandatory = excelConfigTemp.Value?.Ismandatory ?? false;                                            

                                            // add special column name
                                            if(columnName.ToLower() == "specialrequirements")
                                            {
                                                columnName = "capacityspecialrequirement";
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
                                                if (cnfCapacityEntity != null && cnfCapacityEntity.Cnfcapacityid != 0)
                                                {
                                                    if (_commonManager.TrySetValueFromImportCell(cnfCapacityEntity, columnName, convertedValue)) continue;
                                                }
                                                if (cnfCapacityEntity != null && cnfCapacityEntity.Cnfcapacityid == 0)
                                                {
                                                    cnfCapacityEntity.Cnfpodinfoid = existingCnfPod.Cnfpodinfoid;
                                                    if (_commonManager.TrySetValueFromImportCell(cnfCapacityEntity, columnName, convertedValue)) continue;
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                errorRows.Add($"{ex.Message} - Capacity is null");
                                            }
                                        }

                                        if (cnfCapacityEntity != null && cnfCapacityEntity.Cnfcapacityid != 0)
                                        {
                                            _repositoryWrapper.CnfCapacityRepository.Update(cnfCapacityEntity);
                                        }
                                        if (cnfCapacityEntity != null && cnfCapacityEntity.Cnfcapacityid == 0)
                                        {
                                            _repositoryWrapper.CnfCapacityRepository.Create(cnfCapacityEntity);
                                        }
                                        await _repositoryWrapper.SaveAsync();

                                    }
                                }
                            }

                        }

                    }
                    else if (existingCnfClusterInfoEntity == null)
                    {
                        cnfClusterInfoEntity = new Cnfclusterinfo();
                        cnfClusterInfoEntity.Filename = fileName;
                        cnfClusterInfoEntity.Specialrequirements = "0";
                        cnfClusterInfoEntity.Hyperthreading = "0";
                        cnfClusterInfoEntity.Overprovisioning = "0";
                        cnfClusterInfoEntity.Aggregateimageclustersize = "0";
                        cnfClusterInfoEntity.Hardware = "0";
                        cnfClusterInfoEntity.Revision = revision;
                        //cnfClusterInfoEntity.Noofblades = 1;

                        #region //drop down
                        // opco
                        var opcoExistEntity = _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.Trim().ToLower().Replace(" ", "") == opCo.Trim().ToLower().Replace(" ", "")).FirstOrDefault();

                        if (opcoExistEntity != null)
                        {
                            cnfClusterInfoEntity.Opcoid = opcoExistEntity.Opcoid;
                        }
                        else
                        {
                            hasMandatoryOrDataTypeError = true;
                            errorRows.Add($"Row {rowIndex} - This Opco '{opCo.ToUpper()}' is not in TEMS, ");
                        }
                        // cnfName
                        var cnfNameExistEntity = _repositoryWrapper.CnfNameRepository.FindByCondition(x => x.Cnfdescription.Trim().ToLower().Replace(" ", "") == cnfNameColumnValue.Trim().ToLower().Replace(" ", "")).FirstOrDefault();

                        if (cnfNameExistEntity != null)
                        {
                            cnfClusterInfoEntity.Cnfnameid = cnfNameExistEntity.Cnfnameid;
                        }
                        else
                        {
                            hasMandatoryOrDataTypeError = true;
                            errorRows.Add($"Row {rowIndex} - This CNF Name '{cnfClusterColumnValue.ToUpper()}' is not in TEMS, ");
                        }

                        // cluster and pool
                        var ClusterExistEntity = _repositoryWrapper.CnfClusterRepository.FindByCondition(x => x.Cnfclustername.Trim().ToLower().Replace(" ", "") == cnfClusterColumnValue.Trim().ToLower().Replace(" ", "")
                        && x.Nodepool.Trim().ToLower().Replace(" ","") == cnfPoolColumnValue.Trim().ToLower().Replace(" ", "")).FirstOrDefault();

                        if (ClusterExistEntity != null && cnfNameExistEntity != null && ClusterExistEntity.Cnfnameid == cnfNameExistEntity.Cnfnameid)
                        {
                            cnfClusterInfoEntity.Cnfclusterid = ClusterExistEntity.Cnfclusterid;
                            cnfClusterInfoEntity.Cnfclusternodepoolid = ClusterExistEntity.Cnfclusterid;
                        }
                        else
                        {
                            hasMandatoryOrDataTypeError = true;
                            errorRows.Add($"Row {rowIndex} - This Cluster '{cnfClusterColumnValue.ToUpper()}' is not in TEMS, ");
                        }               

                        // vertical
                        if (verticalExistEntity != null)
                        {
                            cnfClusterInfoEntity.Verticalresponsibleid = verticalExistEntity.Verticalresponsibleid;
                        }
                        else
                        {
                            hasMandatoryOrDataTypeError = true;
                            errorRows.Add($"Row {rowIndex} - This Vertical '{verticalColumnValue}' is not in TEMS, ");
                        }

                        // sitename
                        if (opcoExistEntity != null)
                        {
                            var LocationExistEntity = _repositoryWrapper.Location.FindByCondition(x => x.Shortdescription.Trim().ToLower().Replace(" ", "") == siteColumnValue.Trim().ToLower().Replace(" ", "")
                            && x.Opcoid == opcoExistEntity.Opcoid).FirstOrDefault();

                            if (LocationExistEntity != null)
                            {
                                cnfClusterInfoEntity.Siteid = LocationExistEntity.Locationid;
                            }
                            else if (LocationExistEntity == null)
                            {
                                errorRows.Add($"Row {rowIndex} - This Site {siteColumnValue.ToUpper()} is not in TEMS, ");
                            }
                            else
                            {
                                hasMandatoryOrDataTypeError = true;
                                errorRows.Add($"Row {rowIndex} - This Site '{siteColumnValue.ToUpper()}' is not linked with Opco '{opCo.ToUpper()}', ");
                            }
                        }

                        // hardware type
                        if (hardwareTypeEntityExist != null)
                        {
                            cnfClusterInfoEntity.Cnfhardwareid = hardwareTypeEntityExist.Cnfhardwareid;
                        }
                        else
                        {
                            hasMandatoryOrDataTypeError = true;
                            errorRows.Add($"Row {rowIndex} - This Hardware Type '{hardwareType.ToUpper()}' is not in TEMS, ");
                        }
                        #endregion


                        if (errorRows.Count > 0)
                        {

                            break;
                        }
                        else
                        {
                            _repositoryWrapper.CnfClusterInfoRepository.Create(cnfClusterInfoEntity);
                            await _repositoryWrapper.SaveAsync();


                            // read instance data
                            var cnfpodInfo = new Cnfpodinfo();
                            cnfpodInfo.Cnfclusterinfoid = cnfClusterInfoEntity.Cnfclusterinfoid;

                            #region //drop down
                            if (cnfpodInfo != null)
                            {
                                if (checkVnfNameExist != null)
                                {
                                    cnfpodInfo.Podtypeinfoid = checkVnfNameExist.Podtypeinfoid;
                                    cnfpodInfo.Podroledescriptionid = checkVnfNameExist.Podtypeinfoid;
                                }
                                else
                                {
                                    errorRows.Add($"Row {rowIndex} - This Pod Type '{cnfPodTypeColumnValue.ToUpper()}' and Pod Description '{cnfPodDesColumnValue.ToUpper()}' not is TEMS, ");
                                }

                                if (checkFunctionExist != null)
                                {
                                    cnfpodInfo.Functionstandardid = checkFunctionExist.Functionstandardnameid;
                                }
                                else
                                {
                                    hasMandatoryOrDataTypeError = true;
                                    errorRows.Add($"Row {rowIndex} - This Function Standard '{funColumnValue.ToUpper()}' is not in TEMS, ");
                                }
                                if (checkPriorityExist != null)
                                {
                                    cnfpodInfo.Priorityid = checkPriorityExist.Cnfpriorityid;
                                }
                                else
                                {
                                    hasMandatoryOrDataTypeError = true;
                                    errorRows.Add($"Row {rowIndex} - This Priority '{priorityColumnValue.ToUpper()}' is not in TEMS, ");
                                }
                            }
                            #endregion

                            foreach (var colIndex in columnMap.Keys)
                            {
                                var cellValue = row.GetCell(colIndex)?.ToString()?.Trim();
                                var Columnheadername = columnMap[colIndex].Columnheadername;
                                var propertyName = Columnheadername; // or Propertyname

                                if (string.IsNullOrEmpty(propertyName))
                                    propertyName = columnMap[colIndex].Propertyname;

                                if (string.IsNullOrEmpty(propertyName))
                                    continue;


                                PropertyInfo prop = typeof(Cnfpodinfo).GetProperty(propertyName);

                                if (prop != null && string.IsNullOrEmpty(cellValue)) continue;

                                var excelConfigTemp = columnMap.FirstOrDefault(x => x.Key == colIndex);
                                var columnName = excelConfigTemp.Value.Propertyname.Trim();
                                var excelValue = row.GetCell(colIndex)?.ToString()?.Trim();
                                var masterTable = excelConfigTemp.Value.Mappingreference;
                                var isMandatory = excelConfigTemp.Value?.Ismandatory ?? false;

                                

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
                                    if (cnfpodInfo != null)
                                    {
                                        if (_commonManager.TrySetValueFromImportCell(cnfpodInfo, columnName, convertedValue)) continue;
                                    }
                                }

                                catch (Exception ex)
                                {
                                    errorRows.Add($"{ex.Message} - CnfPodinfo is null");
                                }
                            }

                            if (errorRows.Count > 0)
                            {
                                break;
                            }
                            else
                            {
                                _repositoryWrapper.CnfPodInfoRepository.Create(cnfpodInfo);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();


                                // read Fy data
                                foreach (var fy in fyPosition)
                                {
                                    int colIndex = fy.Value.FyFirstIndex;
                                    var val = row.GetCell(colIndex)?.ToString()?.Trim();
                                    var headername = columnMap[colIndex].Propertyname;
                                    if (headername.Trim().ToLower() == "numberofcnfinstancespersite" && val.IsNullOrEmpty())
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (cnfpodInfo != null)
                                        {
                                            var cnfCapacityEntity = new Cnfcapacity();
                                            cnfCapacityEntity.Cnfpodinfoid = cnfpodInfo.Cnfpodinfoid;
                                            cnfCapacityEntity.Financialversion = 1;
                                            var FY = _commonManager.GetPropertFY(fy.Key);
                                            cnfCapacityEntity.Financialyear = Convert.ToInt16(RechangeFinancialYear(FY));

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
                                                var excelValue = row.GetCell(colIndex)?.ToString()?.Trim();
                                                var masterTable = excelConfigTemp.Value.Mappingreference;
                                                var isMandatory = excelConfigTemp.Value?.Ismandatory ?? false;

                                                if (columnName.ToLower() == "specialrequirements")
                                                {
                                                    columnName = "capacityspecialrequirement";
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
                                                    if (cnfCapacityEntity != null)
                                                    {
                                                        if (_commonManager.TrySetValueFromImportCell(cnfCapacityEntity, columnName, convertedValue)) continue;

                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    errorRows.Add($"{ex.Message} - Capacity is null");
                                                }
                                            }

                                            _repositoryWrapper.CnfCapacityRepository.Create(cnfCapacityEntity);
                                            await _repositoryWrapper.SaveAsync();
                                            await _repositoryWrapper.ClearTracker();
                                        }

                                    }


                                }
                            }
                        }
                    }
                    if (errorRows.Count <= 0)
                    {
                        noSRowUpdate++;
                    }

                }
                try
                {

                    if (existingCnfClusterInfoEntity != null && existingCnfClusterInfoEntity.Cnfclusterinfoid != 0 && errorRows.Count <= 0)
                    {
                        _repositoryWrapper.CnfClusterInfoRepository.Update(existingCnfClusterInfoEntity);
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


        public async Task<ResultDto> CNFParseExcelRowAsync(ISheet sheet, Dictionary<int, Exceltemplateconfiguration> columnMap, int dataRowIndex, string opCo, string hardwareType,
           string revision, string fileName, Dictionary<string, FyPosition> fyPosition)
        {           
            var errorRows = new List<string>();
            var noSRowUpdate = 0;
            try
            {


                for (int rowIndex = dataRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
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

                    #region// get Mandatory fields // CNF Name //K8 cluster //Node Pool //Pode Type // Pode Des

                    var cnfClusterColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                     ConstantValueFilter.cnfcluster);

                    string cnfClusterColumnValue = row.GetCell(cnfClusterColumn.Key)?.ToString()?.Trim() ?? string.Empty;

                    var cnfPoolColumn = columnMap.FirstOrDefault(x => x.Value.Columnheadername.Replace(" ", "").ToLower() ==
                    ConstantValueFilter.cnfnodePool);

                    string cnfPoolColumnValue = row.GetCell(cnfPoolColumn.Key)?.ToString()?.Trim() ?? string.Empty;


                    #endregion

                    #region // Drop down 
                    
                    // hardware type
                    var hardwareTypeEntityExist = _repositoryWrapper.CnfHardwareRepository.FindByCondition(x => x.Description.Trim().ToLower().Replace(" ", "") ==
                    hardwareType.Trim().ToLower().Replace(" ", "")).FirstOrDefault();

                    #endregion

                    var existingCnfClusterInfoEntity = await _repositoryWrapper.CnfClusterInfoRepository.FindByCondition(x => x.Opco.Opco.Trim().ToLower().Replace(" ","") ==
                        opCo.Trim().ToLower().Replace(" ", "") &&
                       (x.Cnfcluster.Cnfclustername.Trim().ToLower().Replace(" ", "") == cnfClusterColumnValue.Trim().ToLower().Replace(" ", "") || 
                       x.Cnfcluster.Alaisname.Trim().ToLower().Replace(" ", "") == cnfClusterColumnValue.Trim().ToLower().Replace(" ", "")) &&
                        x.Cnfcluster.Nodepool.Trim().ToLower().Replace(" ", "") == cnfPoolColumnValue.Trim().ToLower().Replace(" ", "")
                        && x.Cnfhardware.Description.Trim().ToLower().Replace(" ", "") == hardwareType.Trim().ToLower().Replace(" ", ""))
                        .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Cnfcapacity)
                        .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Podroledescription)
                        .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Podtypeinfo)
                        .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Functionstandard)
                        .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Priority).FirstOrDefaultAsync();

                    if (existingCnfClusterInfoEntity != null)
                    {                        
                       
                        foreach (var colIndex in columnMap.Keys)
                        {
                            var cellValue = row.GetCell(colIndex)?.ToString()?.Trim();
                            var Columnheadername = columnMap[colIndex].Columnheadername;
                            var propertyName = Columnheadername; // or Propertyname

                            if (string.IsNullOrEmpty(propertyName))
                                propertyName = columnMap[colIndex].Propertyname;

                            if (string.IsNullOrEmpty(propertyName))
                                continue;


                            PropertyInfo prop = typeof(Cnfclusterinfo).GetProperty(propertyName);

                            if (prop != null && string.IsNullOrEmpty(cellValue)) continue;

                            var excelConfigTemp = columnMap.FirstOrDefault(x => x.Key == colIndex);
                            var columnName = excelConfigTemp.Value.Propertyname.Trim();
                            var excelValue = row.GetCell(colIndex)?.ToString()?.Trim();
                            var masterTable = excelConfigTemp.Value.Mappingreference;
                            var isMandatory = excelConfigTemp.Value?.Ismandatory ?? false;

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
                                if (existingCnfClusterInfoEntity != null && existingCnfClusterInfoEntity.Cnfclusterinfoid != 0)
                                {
                                    if (_commonManager.TrySetValueFromImportCell(existingCnfClusterInfoEntity, columnName, convertedValue)) continue;
                                }
                                

                            }

                            catch (Exception ex)
                            {
                                errorRows.Add($"{ex.Message} - Cnfpodinfo is null");
                            }
                        }

                        if (errorRows != null && errorRows.Count <= 0)
                        {
                            _repositoryWrapper.CnfClusterInfoRepository.Update(existingCnfClusterInfoEntity);
                            await _repositoryWrapper.SaveAsync();
                        }


                    }
                    else
                    {
                        errorRows.Add($"This cluster not in TEMS: {cnfClusterColumnValue},");
                    }
                    if (errorRows.Count <= 0)
                    {
                        noSRowUpdate++;
                    }

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
