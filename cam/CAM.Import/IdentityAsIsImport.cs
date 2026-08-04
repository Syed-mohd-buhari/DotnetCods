using AutoMapper;
using CAM.BusinessManager;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using Microsoft.AspNetCore.Http;
using OracleModels.DBContext;

namespace CAM.Imports
{
    public class IdentityAsIsImport : BaseManager
    {
        private readonly ModelContext _modelContext;
        private readonly IMapper _mapper;
        //private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public string SheetName = "Export";
        public string IdColumn = "Identity Index";
        public string excelName = "IdentityAsIs";

        public List<string> EditableColumnList = new List<string> { "Value", "Category", "Class", "Type", "Asset Name" , "Previous Resource Key"};

        public IdentityAsIsImport(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, IMapper mapper
            , IHttpContextAccessor contextAccessor, ILoggerManager logger) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
          // _logger = logger;

        }

        public async Task<ResultDto> UpdateExcelColumn(List<Dictionary<string, string>> processData, List<Dictionary<string, string>> insertionData)
        {

            string errordescription = "", error = "", htmlbreak = " ", errorShortDescription = "";
            bool recordChanged = false;
            bool catrecordChanged = false;
            bool classrecordChanged = false;
            bool typerecordChanged = false;
            bool assetrecordChanged = false;
            bool RecordUpdated = false;
            int norecordsUpdated = 0;

            try
            {
                foreach (Dictionary<string, string> item in processData)
                {
                    if (item[IdColumn] != null)
                    {
                        var model = _repositoryWrapper.IdentityAsIsRepository.FindByCondition(x => x.Id.ToString() == item[IdColumn]).FirstOrDefault();
                        var entityExists = CAM.Entities.Mappers.IdentityAsIsMapper.Get(model);
                        var entity = CAM.Entities.Mappers.IdentityAsIsMapper.Set(entityExists);
                        recordChanged = false;
                        try
                        {
                            error = "";
                            if (entityExists != null)
                            {
                                if (item.ContainsKey("Value"))
                                {
                                    string value = item["Value"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                            if (entityExists.Value != val)
                                            {
                                                entity.Value = val;
                                                recordChanged = true;
                                            }
                                        }
                                        else if (value == null)
                                        {
                                            if (entityExists.Value != null)
                                            {
                                                entity.Value = value;
                                                recordChanged = true;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " Identity Value Unable to updated ";
                                    }
                                }
                                if (item.ContainsKey("Category"))
                                {
                                    string value = item["Category"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        var catModel = _repositoryWrapper.CategoryRepository.FindByCondition(x => x.Id == entityExists.CategoryId).FirstOrDefault();
                                        var catEntityExists = CategoryMapper.Get(catModel);
                                        if (catEntityExists != null)
                                        {
                                            var catEntity = CategoryMapper.Set(catEntityExists);

                                            if (!string.IsNullOrEmpty(val))
                                            {

                                                if (catEntityExists.Description != val)
                                                {
                                                    entity.Categoryid = catEntity.Id;
                                                    catEntity.Description = val;
                                                    catrecordChanged = true;
                                                    recordChanged = true;
                                                }
                                            }
                                            else if (value == null)
                                            {
                                                if (catEntityExists.Description != null)
                                                {
                                                    entity.Categoryid = catEntity.Id;
                                                    catEntity.Description = value;
                                                    catrecordChanged = true;
                                                    recordChanged = true;

                                                }
                                            }
                                            if (catrecordChanged)
                                            {
                                                _repositoryWrapper.CategoryRepository.Update(catEntity);
                                            }
                                        }
                                       
                                    }
                                    catch
                                    {
                                        error += htmlbreak + " Category Unable to updated ";
                                    }
                                }
                                #region 
                                if (item.ContainsKey("Class"))
                                {
                                    string value = item["Class"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        var classModel = _repositoryWrapper.ClassRepository.FindByCondition(x => x.Id == entityExists.ClassId).FirstOrDefault();
                                        var classEntityExists = ClassMapper.Get(classModel);
                                        if (classEntityExists != null)
                                        {
                                            var classEntity = ClassMapper.Set(classEntityExists);

                                            if (!string.IsNullOrEmpty(val))
                                            {
                                                if (classEntityExists.Description != val)
                                                {
                                                    entity.Classid = classEntity.Id;
                                                    classEntity.Description = val;
                                                    classrecordChanged = true;
                                                }
                                            }
                                            else if (value == null)
                                            {
                                                if (classEntityExists.Description != null)
                                                {
                                                    entity.Classid = classEntity.Id;
                                                    classEntity.Description = value;
                                                    classrecordChanged = true;
                                                }
                                            }
                                            if (classrecordChanged)
                                            {
                                                _repositoryWrapper.ClassRepository.Update(classEntity);
                                            }
                                        }
                                    }

                                    catch
                                    {
                                        error += htmlbreak + " Class Unable to updated ";
                                    }
                                }
                                if (item.ContainsKey("Type"))
                                {
                                    string value = item["Type"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        var typeModel = _repositoryWrapper.TypeRepository.FindByCondition(x => x.Id == entityExists.TypeId).FirstOrDefault();
                                        var typeEntityExists = Entities.Mappers.Lookup.TypeMapper.Get(typeModel);
                                        if (typeEntityExists != null)
                                        {
                                            var typesEntity = Entities.Mappers.Lookup.TypeMapper.Set(typeEntityExists);

                                            if (!string.IsNullOrEmpty(val))
                                            {
                                                if (typeEntityExists.Description != val)
                                                {
                                                    entity.Typeid = typesEntity.Id;
                                                    typesEntity.Description = val;
                                                    typerecordChanged = true;
                                                }
                                            }
                                            else if (value == null)
                                            {
                                                if (typeEntityExists.Description != null)
                                                {
                                                    entity.Typeid = typesEntity.Id;
                                                    typesEntity.Description = value;
                                                    typerecordChanged = true;
                                                }
                                            }
                                            if (typerecordChanged)
                                            {
                                                _repositoryWrapper.TypeRepository.Update(typesEntity);
                                            }
                                        }
                                    }

                                    catch
                                    {
                                        error += htmlbreak + " Type Unable to updated ";
                                    }
                                }
                                if (item.ContainsKey("Asset Name"))
                                {
                                    string value = item["Asset Name"];
                                    try
                                    {
                                        string val = value != null ? value : "";
                                        var assetModel = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == entityExists.AssetId).FirstOrDefault();
                                        var assetEntityExists = NetworkElementAsPlannedMapper.Get(assetModel);
                                        if (assetEntityExists != null)
                                        {
                                            var assetEntity = NetworkElementAsPlannedMapper.Set(assetEntityExists);

                                            if (!string.IsNullOrEmpty(val))
                                            {
                                                if (assetEntityExists.ElementName != val)
                                                {
                                                    entity.Assetid = assetEntity.Networkelementasplannedid;
                                                    assetEntity.Elementname = val;
                                                    assetrecordChanged = true;
                                                }
                                            }
                                            else if (value == null)
                                            {
                                                if (assetEntityExists.ElementName != null)
                                                {
                                                    entity.Assetid = assetEntity.Networkelementasplannedid;
                                                    assetEntity.Elementname = value;
                                                    assetrecordChanged = true;
                                                }
                                            }
                                            if (assetrecordChanged)
                                            {
                                                _repositoryWrapper.NetworkElementAsPlanned.Update(assetEntity);

                                            }
                                        }
                                    }

                                    catch
                                    {
                                        error += htmlbreak + " Asset Nam Unable to updated ";
                                    }
                                }
                                if (item.ContainsKey("Previous Resource Key"))
                                {
                                    try
                                    {
                                        string value = item["Previous Resource Key"];
                                        
                                        if (entityExists.PreviousResourceKey != value)
                                        {
                                            if (entityExists.ResourceKey != null)
                                            {
                                                var elementnames = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == entityExists.AssetId).ToList();
                                                if (elementnames != null)
                                                {
                                                    foreach (var elementname in elementnames)
                                                    {
                                                        var opCoId = elementname.Opcoid;
                                                        var dcflcExists = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(x => x.Opcoid == opCoId && x.Currentdetails == elementname.Elementname &&
                                                        x.Resourcekey == entityExists.ResourceKey).ToList();

                                                        if (dcflcExists != null)
                                                        {
                                                            foreach (var dcflc in dcflcExists)
                                                            {
                                                                dcflc.Previousresourcekey = value;
                                                                _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Update(dcflc);
                                                            }
                                                        }
                                                    }

                                                }
                                                entity.Previousresourcekey = value;
                                                recordChanged = true;
                                            }
                                            else
                                            {
                                                error += htmlbreak + "Could not update Previous Resource Key without resource key ";
                                            }
                                        }
                                        
                                       

                                    }
                                    catch
                                    {
                                        error += htmlbreak + " Previous Resource Key Value unable to updated ";

                                    }

                                }
                                #endregion
                            }
                            else
                            {
                                error += htmlbreak + " DB Doesn't Contain : " + IdColumn + "column " + "value : " + item[IdColumn];
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
                                _repositoryWrapper.IdentityAsIsRepository.Update(entity);
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
                            if (recordChanged || catrecordChanged || typerecordChanged || classrecordChanged || assetrecordChanged)
                            {
                                _repositoryWrapper.Save();
                                RecordUpdated = true;

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
            if (errorShortDescription.Length > 0)
            {
                errorShortDescription = IdColumn + "  : " + errorShortDescription + " records not updated, please update the correct value in the excel and try to upload again...";
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
    }
}
