namespace CAM.Identity
{
    public class ApplicationOrganisation
    {
        public int MainOranisationId { get; set; }
        public int PracticeId { get; set; }
        public int PracticeContactId { get; set; }
        public virtual ApplicationMainOrganisation MainOrganisation { get; set; }
        public virtual ApplicationPracticeModel Practice { get; set; } 
        public virtual ApplicationVerticalRes VerticalRes { get; set; }

    }
}
