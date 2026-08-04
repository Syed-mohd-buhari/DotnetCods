namespace CAM.Identity
{
    public class ApplicationOrgandVertical
    {
        public int Aspnetuserverticalid { get; set; }
        public int? Userid { get; set; }
        public long? Organisationid { get; set; }
        public bool? Isvertical { get; set; }
        public bool? Isverticalresponcible { get; set; }
        public virtual ApplicationOrganisation ApplicationOrganisation { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
