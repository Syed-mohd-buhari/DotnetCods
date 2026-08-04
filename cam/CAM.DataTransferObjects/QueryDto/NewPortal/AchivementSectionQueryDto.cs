using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.QueryDto.NewPortal
{
    public class AchivementSectionQueryDto : QueryObject
    {
        public long UserId { get; set; }
        public long PortalRoleId { get; set; }
    }
}
