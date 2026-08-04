using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.Entita.AspNetUserRole
{
    public class AspnetuserroleGridDto
    {
        [Default]

        public int UserId { get; set; }
        [Default]

        public string UserName { get; set; }
        [Default]

        public string Email { get; set; }
        [Default]

        public bool Active { get; set; }

    }
}
