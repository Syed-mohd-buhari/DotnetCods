using CAM.DataAttributes.Grid;
using System;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.TeamManagement
{
    public class TeamMemberGridDto
    {
        [DisplayName("Team Member Index")]
        [Default]
        public int TeamMemberId {  get; set; }
        [DisplayName("User")]
        [Default]
        public string User {  get; set; }

        [DateRangeGrid]
        [DisplayName("Last Modified Date")]
        [Default]
        public virtual DateTime? LastModified { get; set; }
        [Default]
        [DisplayName("Last Modified By")]
        public virtual string LastModifiedBy { get; set; }
        [IgnoreGrid]
        public int LastModifiedId { get; set; }
        [IgnoreGrid]
        public int UserId { get; set; }
    }
}
