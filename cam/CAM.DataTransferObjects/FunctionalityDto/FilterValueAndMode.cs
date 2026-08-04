using CAM.Enum;

namespace CAM.DataTransferObjects.FunctionalityDto
{
    public class FilterValueAndMode
    {
        public string PropertyName { get; set; }
        public FilterModeEnum ModeFilter { get; set; }
        public FilterTypeEnum TypeFilter { get; set; }
        public object PropertyValue { get; set; }
    }
}