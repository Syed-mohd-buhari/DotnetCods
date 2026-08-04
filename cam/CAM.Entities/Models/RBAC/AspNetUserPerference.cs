
namespace CAM.Entities.Models.RBAC
{
    public class AspNetUserPerference
    {
        public int Aspnetuserpreferenceid { get; set; }
        public int? Userid { get; set; }
        public int? Moduleid { get; set; }
        public short? Permission { get; set; }
        public int? Order { get; set; }
        public bool? Isuserpreference { get; set; }
        public virtual AspNetModules Aspnetmodule { get; set; }

    }
}
