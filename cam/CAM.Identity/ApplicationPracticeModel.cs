namespace CAM.Identity
{
    public class ApplicationPracticeModel
    {
        public int PracticeId { get; set; }
        public string PracticeDescription { get; set; }
        public int? PracticeEmailId { get; set; }
        public virtual ApplicationUser PracticeEmail { get; set; }


    }
}
