using CAM.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using OracleModels.DBModels;

namespace CAM.Imports
{
    public interface IImportService
    {
        bool IsValideExcel(IFormFile importFile);
        bool ValidateSheetName(IFormFile Importedfile, string SheetName);
        //Task<ResultDto> ValidateColumnCheck(LcmImport lcmImport,IFormFile Importedfile);
        bool ValidateColumnCheck(IFormFile Importedfile,string SheetName,List<string> EditableColumnList, string pkcolumn);
        Task<ResultDto> Processimportexcel(IFormFile file, string importfiletype);

        Task<ResultDto> BulkImport(IFormFile file , long recordClassifier, long? nonTemsVertical = null);
        Task<ResultDto> ImportDataBasedOnTemplateConfiguration(IFormFile file, string importfiletype, List<Exceltemplateconfiguration> excelConfiguration, long nonTemsVertical = 0);
        Task<ResultDto> ImportDataBasedOnTemplateConfigurationForXL(IFormFile formFile, string processName, List<Exceltemplateconfiguration> excelConfigData, long nonTemsVertical = 0);

        Task<ResultDto> ImportDataBasedOnTemplateConfigurationForVbom(IFormFile file, string importfiletype, List<Exceltemplateconfiguration> excelConfiguration,string fileName);
        Task<ResultDto> ImportDataBasedOnTemplateConfigurationForCbom(IFormFile file, string importfiletype, List<Exceltemplateconfiguration> excelConfiguration, List<Exceltemplateconfiguration> excelConfigurationForCnf, string fileName);

    }
}
