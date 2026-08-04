using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.TeamManagement
{
    public class TeamGridDto
    {
        [DisplayName("Team Index")]
        [Default]
        public int TeamId {  get; set; }
        [DisplayName("Team Name")]
        [Default]
        public string TeamName { get; set; }
        [DisplayName("Team Description")]
        [Default]
        public string TeamDescription {  get; set; }
        [DisplayName("Active")]
        [Default]
        public bool Active { get; set; }

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
        public string UserName { get; set; }

        [IgnoreGrid]
        public Dictionary<int,string> UserResources {  get; set; }
        [IgnoreGrid]
        public List<int> UserIds {  get; set; }
    }
}
