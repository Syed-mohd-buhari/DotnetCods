namespace CAM.Identity
{
    public class ApplicationUserOpco
    {
        public int Aspnetuseropcoid { get; set; }
        public int? Userid { get; set; }
        public short? Opcoid { get; set; }
        public bool? Isrestrictedopco { get; set; }
        public bool? Isinusedopcos { get; set; }
        public ApplicationOpco ApplicationOpco { get; set; }
        public virtual ApplicationUser User { get; set; }
       
    }
}
