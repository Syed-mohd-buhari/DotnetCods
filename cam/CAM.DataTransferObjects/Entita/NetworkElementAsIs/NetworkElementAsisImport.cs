namespace CAM.DataTransferObjects.Entita.NetworkElementAsIs
{
    public  class NetworkElementAsisImport
    {
        public CAM.Entities.Models.NetworkElementAsIs NetworkElementAsIs { get; set; }
        public OracleModels.DBModels.Networkelementsasis Networkelementasis { get; set; }
        public bool IsUpdateRecords { get; set; } = false;
        public string Error { get; set; } = string.Empty;
    }
    public class NetworkElementAsisUpdateDto
    {
        public OracleModels.DBModels.Networkelementsasis NetworkElementAsIs { get; set; }
        public bool IsUpdateRecords { get; set;}
    }
}
