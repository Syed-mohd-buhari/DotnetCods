namespace TEMS.DataTransferObject
{
    public class ResourceKeyMasterDto
    {
        public long? Resourcetypesid { get; set; }
        public string Resourcekey { get; set; }
        public string Opcoid { get; set; }
        public string Dcfid { get; set; }
        public int Keystatus { get; set; }
        public int Creationuser { get; set; }
        public int Modificationuser { get; set; }
        public string Elementname { get; set; }
        public string Lifecycleid { get; set; }
        public string BuildBagId { get; set; }
    }
}
