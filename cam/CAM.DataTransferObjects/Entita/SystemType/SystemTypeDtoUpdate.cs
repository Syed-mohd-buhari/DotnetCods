namespace CAM.DataTransferObjects.Entita.SystemType
{
    public class SystemTypeDtoUpdate : SystemTypeDtoCreate
    {
        public long SystemTypeId { get; set; }

        public string AssetType { get; set; }
    }
}