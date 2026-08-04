using System;

namespace CAM.DataTransferObjects.LookUp.Activity
{
    public class ActivityStatusDtoUpdate : ActivityStatusDto
    {
        public short ActivityStatusId { get; set; }
        public DateTime UpdateDate { get; set; }
        public short UpdateUserId { get; set; }
    }
}