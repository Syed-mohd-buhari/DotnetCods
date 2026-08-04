using CAM.BusinessManager;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NFVICompatibiltyReport;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.Imports
{
    public class NfviSoftwareCompatibilityImport : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public string SheetName = "Export";
        public string IdColumn = "NfviSoftwareCompatibilityId";
        public string excelName = "NfviSofwareCompatibility";
        public string Vendor = "Vendor";
        public string PlatFormVersion = "PlatformVersion";
        public string ProductName = "ProductName";
        public string MinimumSupportedVersion = "MinimumSupportedVersion";

        public List<string> EditableColumnList = new List<string> { "PlatformVersion", "MinimumSupportedVersion", "Vendor", "ProductName" };

        public NfviSoftwareCompatibilityImport(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper
            , IHttpContextAccessor contextAccessor) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;

        }

        public async Task<ResultDto> UpdateExcelColumn(List<Dictionary<string, string>> processData, List<Dictionary<string, string>> insertionDatas)
        {

            string errorShortDescription = "";
            bool isNotinTems = false, isNullValue = false, recordChanged = false;
            bool RecordUpdated = false;
            int norecordsUpdated = 0;

            try
            {
                foreach (Dictionary<string, string> item in processData)
                {
                    recordChanged = false;
                    if (item[Vendor] != null && item[ProductName] != null && item[PlatFormVersion] != null && item[ProductName] != null)
                    {
                        var getRequiredIds = await GetPrimaryKey(item[Vendor], item[ProductName], item[PlatFormVersion]);

                        if (getRequiredIds.VendorId > 0 && getRequiredIds.PlatformVersionId > 0 && getRequiredIds.ProductNameId > 0)
                        {
                            var checkNfviEntityExist = await _repositoryWrapper.NfviSoftwareCompatibilityRepository.FindByCondition(x=>x.Vendorid == getRequiredIds.VendorId && 
                            x.Productid == getRequiredIds.ProductNameId && x.Plaftformid == getRequiredIds.PlatformVersionId).FirstOrDefaultAsync();

                            if(checkNfviEntityExist != null)
                            {
                                //Compare the excel and DB data for miniversion fields
                                if(item.ContainsKey(MinimumSupportedVersion))
                                {
                                    if(item[MinimumSupportedVersion] != checkNfviEntityExist.Minimumsupportedversion)
                                    {
                                        checkNfviEntityExist.Minimumsupportedversion = item[MinimumSupportedVersion];
                                        recordChanged = true;
                                        _repositoryWrapper.NfviSoftwareCompatibilityRepository.Update(checkNfviEntityExist);
                                    }

                                }
                                  
                            }
                            else if(checkNfviEntityExist == null)
                            {
                                var nfviEntity = new Nfvisoftwarecompatibility();

                                nfviEntity.Vendorid = (short)getRequiredIds.VendorId;
                                nfviEntity.Productid = getRequiredIds.ProductNameId;
                                nfviEntity.Plaftformid = (long)getRequiredIds.PlatformVersionId;
                                nfviEntity.Minimumsupportedversion = item[MinimumSupportedVersion];
                                _repositoryWrapper.NfviSoftwareCompatibilityRepository.Create(nfviEntity);
                                recordChanged = true;

                            }
                            if (recordChanged)
                            {
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();
                                RecordUpdated = true;
                                norecordsUpdated++;
                            }
                        }                                                
                        else
                        {
                            isNotinTems = true;

                            if (errorShortDescription != "")
                            {
                                errorShortDescription += ", ";
                            }
                            errorShortDescription += "( " +item[Vendor] + ", " + item[ProductName] + ", " + item[PlatFormVersion]+" )";
                        }
                        
                    }
                    else
                    {
                        isNullValue = true;

                        if (errorShortDescription != "")
                        {
                            errorShortDescription += ", ";
                        }
                        errorShortDescription += "( " + item[Vendor] + ", " + item[ProductName] + ", " + item[PlatFormVersion] + " )";
                    }
                }

            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Info = errorShortDescription + "\n" + ex.Message,
                    Warning = false
                };
            }
            if(errorShortDescription.Length > 0 && isNotinTems || isNullValue)
            {
                errorShortDescription = $"{Vendor},{ProductName},{PlatFormVersion} : {errorShortDescription} those records not in TEMS, please update the correct value in the excel and try to upload again...";
            }
            else if (errorShortDescription.Length > 0)
            {
                errorShortDescription = $"{Vendor},{ProductName},{PlatFormVersion} : {errorShortDescription}  records not updated, please update the correct value in the excel and try to upload again...";
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

        public async Task<NFVICompatibilityStatusDTO> GetPrimaryKey(string Vendor, string ProductName, string PlatformVersion)
        {
            var result = new NFVICompatibilityStatusDTO();
            var softwareId = 0;
        
            var getVendorId = await _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Originalequipmentmanufacturer.Trim().ToLower() == Vendor.Trim().ToLower())
                .FirstOrDefaultAsync();
            var getProductNameId = await _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Description.Trim().ToLower() == ProductName.Trim().ToLower())
                .FirstOrDefaultAsync();
            var getSoftwareId = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Softwareversion.Trim().ToLower() == PlatformVersion.Trim().ToLower()
            && x.Isvmware == true)
                .ToListAsync();

            result.VendorId = getVendorId != null ? getVendorId.Orgeqpmanufacturerid : 0;
            result.ProductNameId = getProductNameId != null ? (int)getProductNameId.Productnameid : 0;
            //result.PlatformVersionId = getProductNameId != null ? getProductNameId.Productnameid : 0;
            if (getSoftwareId != null && getSoftwareId.Count > 0)
            {
                // if it is less then 1 no need to check the nfvi because it might be a new entry
                if (getSoftwareId.Count > 1)
                {
                    foreach (var id in getSoftwareId)
                    {
                        var chekNfvi = _repositoryWrapper.NfviSoftwareCompatibilityRepository.FindByCondition(x => x.Plaftformid == id.Majorsoftwarebuildsid
                        && x.Productid == result.ProductNameId && x.Vendorid == result.VendorId).FirstOrDefault();
                        if (chekNfvi != null)
                        {
                            softwareId = (int)id.Majorsoftwarebuildsid;
                            break;
                        }
                    }
                }
                else
                {
                    softwareId = (int)getSoftwareId.First().Majorsoftwarebuildsid;
                }
            }

            result.PlatformVersionId = softwareId;
            return result;

        }
    }
}
