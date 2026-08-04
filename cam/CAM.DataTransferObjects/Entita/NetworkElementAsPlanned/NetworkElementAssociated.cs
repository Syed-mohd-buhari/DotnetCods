using System;

namespace CAM.DataTransferObjects.Entita.NetworkElementAsPlanned
{
   public class NetworkElementAssociated 
    {
        public long Id { get; set; }
        public string ElementName { get; set; }
        public string Enviroment { get; set; }
        public string Location { get; set; }
        public short EnviromentId { get; set; }
        public short LocationId { get; set; }
        public string AssetsStatus { get; set; }
        public short AssetsStatusId { get; set; }
        public bool? IsFinalAsset { get; set; }
        public DateTime? AssetLiveStatusDate { get; set; }

        public DateTime? AssetRfoDate { get; set; }
        public DateTime? AssetRfsDate { get; set; }

        public DateTime? RfaDate { get; set; }
        public DateTime? HwPoArrivedDate { get; set; }
        public DateTime? HwPoRaisedDate { get; set; }
        public DateTime? BomSubmittedDate { get; set; }
    }
}
