using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp.LCMSoftwareSupportTypeDto
{
   public class LCMSoftwareSupportTypeDto
    {
        public short Id { get; set; }
        public string Description { get; set; }
        public bool OemSupport { get; set; }
        public bool Warranty { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }

    }
    public class LCMSoftwareSupportTypeDtoGrid : GridDtoBase
    {
       public short Id { get; set; }
       public string Description { get; set; }
       public bool OemSupport { get; set; }
       public bool Warranty { get; set; }
       [DateRangeGrid]
       public DateTime? LastModified { get; set; }
       [IgnoreGrid]
       public string ValueToShow { get; set; }
      
       [MailTo]
       [DisplayName("Last Modified By")]
       public string LastModifiedBy { get; set; }
    }
   public class LCMSoftwareSupportTypeDtoQuery : QueryObject
    {
       public List<short> Id { get; set; }
       public List<string> Description { get; set; }
       public List<bool> OemSupport { get; set; }
       public List<bool> Warranty { get; set; }
      
    }


}
