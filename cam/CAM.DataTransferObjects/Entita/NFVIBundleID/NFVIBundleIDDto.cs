using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.Entita.NFVIBundleID
{
   public class NFVIBundleIDDto
    {
        public short NFVIBundleIDId { get; set; }         
       
        public string NFVIBundleIDDescription { get; set; }
        public int Order { get; set; }

        [DateRangeGrid]
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }


    }
    public class NFVIBundleIDDtoGrid : GridDtoBase
    {
        [Default]
        public short Id { get; set; }
        [Default]
        public string Description { get; set; }
        [Default]
        public int Order { get; set; }
        [DateRangeGrid]
        [Default]
        public DateTime? LastModified { get; set; }
        [OrderGrid(Order = 14)]
        [MailTo]
        [DisplayName("Last Modified By")]
        [Default]
        public string LastModifiedBy { get; set; }

    }
    public class NFVIBundleIDDtoQuery : QueryObject
    {
        public List<short> Id { get; set; }
        public List<string> Description { get; set; }
        public List<int> Order { get; set; }
       
    }
}
