using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.Entita.NFVIStatus
{
  public  class NFVIStatusDto
    {
        public short NFVIStatusId { get; set; }
        public string NFVIStatusDescription { get; set; }
        [IgnoreGrid]
        public string Color { get; set; }
        [DateRangeGrid]
        public DateTime? LastModified { get; set; }
        [MailTo]
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; set; }

    }
    
    public class NFVIStatusDtoGrid : GridDtoBase
    {
        [Default]
        public short Id { get; set; }
        [Default]
        public string Description { get; set; }
        [Default]
        public string Color { get; set; }
        [DateRangeGrid]
        [Default]
        public DateTime? LastModified { get; set; }
        [MailTo]
        [DisplayName("Last Modified By")]
        [Default]
        public string LastModifiedBy { get; set; }

    }
    public class NFVIStatusDtoQuery : QueryObject
    {
        public List<short> Id { get; set; }
        public List<string> Description { get; set; }
        public List<string> Color { get; set; }
       


    }
}
