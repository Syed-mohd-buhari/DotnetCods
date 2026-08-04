namespace CAM.DataTransferObjects.Entita.NetworkElementAsPlanned
{
    public class NetworkElementAsPlannedDtoUpdate : NetworkElementAsPlannedDtoCreate
    {
        public long NetworkElementAsPlannedId { get; set; }
        public bool LinkedToNetworkAsIs { get; set; }

        public long LcmEngineeringId { get; set; }

        public string? OldElementName { get; set; }
    }
}
