using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.Entita.AspNetUserRole
{
    public class AspNetUserROVGridDto
    {
        [IgnoreGrid]
        public long AspNetUserRoleId { get; set; }
        [Default]
        public string Role { get; set; }
        [Default]

        public string OpCo { get; set; }
        [Default]

        public string Vertical { get; set; }
        [Default]
        public string VerticalResponsible { get; set; }


        [IgnoreGrid ]
        public string RoleId { get; set; }
        [IgnoreGrid]
        public string VerticalId { get; set; }
        [IgnoreGrid]
        public string VerticalResponsibleId { get; set; }

        [IgnoreGrid]
        public string SubdomainResponsibleId { get; set; }
        [IgnoreGrid]
        public bool? Issubdomainspoc { get; set; }
        [IgnoreGrid]
        public bool? Iseduspoc { get; set; }
        [IgnoreGrid]
        public bool? Isdesigncontact { get; set; }

        [IgnoreGrid]
        public string RestrictedOpCoIds { get; set; }
        //[IgnoreGrid]
        //public string RoleDescription { get; set; }

    }
}
