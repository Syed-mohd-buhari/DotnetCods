using CAM.DataTransferObjects.Entita;
using CAM.Infrastucture.QueryResult;
using OracleModels.DBModels;
using System.Collections.Generic;

namespace CAM.Exports
{
    public interface IExportService
    {

        ExportResult GetExcelFrom(List<ExportSheet> sheets, string fileName);
        ExportResult GetExcelFrom<T>(List<ExportSheet> sheets, string fileName, CustomGridRender<T> data);
        ExportResult GetExcelFrom<T>(List<ExportSheet> sheets, string fileName, CustomGridRender<T> data, string IDColumn, List<string> HighlightColumns, IEnumerable<GlossaryItemsGridDto> tsrDescription = null);
        ExportResult GetExcelFrom(List<ExportSheetCustom> sheets, string fileName);

        ExportResult GetExcelFrom<T>(List<ExportSheet> sheets, string fileName, GenericReportInfrastructureGrid<T> data);

        void GetCSVFrom(string filePath);

        void GetExcelFromCSV(List<ExportSheet> sheets, string fileName);
        ExportResult GetExcelFromCSV<T>(ExportSheet sheet, string fileName, CustomGridRender<T> dataRender);
        ExportResult GetExcelFromCSV<T>(List<ExportSheet> sheets, string fileName, CustomGridRender<T> dataRender);
        ExportResult GetExcelFromCSV<T>(List<ExportSheet> sheet, string fileName, GenericReportInfrastructureGrid<T> dataRender);

        ExportResult GetExcelFromPAT(List<ExportSheetCustom> sheets, string fileName);
        void GetExcelFromFile<T>(string filePath, List<T> data);

        ExportResult GetExcelBasedOnConfiguration<T>(List<ExportSheet> sheets, string fileName, string filePath, List<T> data);
        ExportResult ExportExcelBasedOnConfiguration<T>(List<ExportSheet> sheets, string fileName, string filePath, List<T> data, List<Exceltemplateconfiguration> excelConfigData);

        ExportResult ExportLegacyExcelBasedOnConfiguration<T>(List<ExportSheet> sheets, string fileName, string filePath, List<T> data, List<Exceltemplateconfiguration> excelConfigData);
        ExportResult ExportLegacyExcelBasedOnConfigurationForXBOM<T>(List<ExportSheet> sheets, string fileName, string filePath, string logoPath, List<T> data, List<Exceltemplateconfiguration> excelConfigData);
        ExportResult ExportLegacyExcelBasedOnConfigurationForCBOM<T>(List<ExportSheet> sheets, string fileName, string filePath, string logoPath, List<T> data, List<Exceltemplateconfiguration> excelConfigData, List<Exceltemplateconfiguration> excelConfigDataCNFInfo);
        ExportResult GetExcelForLcmPassthroughHardware<T>(List<ExportSheet> sheets, string fileName, CustomGridRender<T> dataRender);
        ExportResult GetExcelForLcmPassthroughSoftware<T>(List<ExportSheet> sheets, string fileName, CustomGridRender<T> dataRender);
    }
}
