using AutoMapper.Configuration.Annotations;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.Entita.AppSettingsConfiguration
{
    public class AppSettingsConfigurationDto : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [Default]
        public long AppSettingsConfigurationId { get; set; }
        [OrderGrid(Order = 2)]
        public long? AppSettingsId { get; set; }
        [OrderGrid(Order = 3)]
        [Default]
        public string SettingsValue { get; set; }
    }

    public class AppSettingsDto : GridDtoBase
    {
        public long AppConfigurationId { get; set; }

        [IgnoreGrid]
        public string Description { get; set; }
    }
}
