using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp.ReasonCheckbox
{
    public class ReasonCheckboxDto : GridDtoBase
    {
        [Default]
        public short Id { get; set; }
        [Default]
        public string Description { get; set; }
        [Default]
        public bool IsHardware { get; set; }
        [Default]
        public bool IsSoftware { get; set; }
        
        [MailTo]
        [DisplayName("Last Modified By")]
        [Default]
        public string LastModifiedBy { get; set; }
    }

    public class ReasonCheckboxQueryDto : QueryObject
    {
        public List<short> Id { get; set; }
        public List<string> Description { get; set; }
        public List<bool> IsHardware { get; set; }
        public List<bool> IsSoftware { get; set; }
    }

}
