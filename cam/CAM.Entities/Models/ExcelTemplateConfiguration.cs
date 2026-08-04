using CAM.Entities.Models.Base;
using CAM.Identity;

namespace CAM.Entities.Models
{
    public class ExcelTemplateConfiguration : AuditableEntity
    {
        public int ExcelTemplateConfigurationId { get; set; }
        public string ProcessName { get; set; }
        public string PropertyName { get; set; }
        public string ColumnHeaderName { get; set; }
        public int? ColumnOrder { get; set; }
        public string FontVolor { get; set; }
        public string CellColor { get; set; }
        public bool? IsImportField { get; set; }
        public bool? IsMandatory { get; set; }
        public string DataType { get; set; }
        public string DefaultValue { get; set; }
        public string MappingReference { get; set; }
        public bool? IsMerged { get; set; }
        public int? Mergestartcolumn { get; set; }
        public int? MergeEndColumn { get; set; }
        public int? RowStarting { get; set; }

        public bool? IsExportField { get; set; }
        public int? HeaderRowStarting { get; set; }
        public string TemplateFileName { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
 
    }
}
