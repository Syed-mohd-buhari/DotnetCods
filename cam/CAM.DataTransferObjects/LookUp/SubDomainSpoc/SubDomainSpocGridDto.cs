using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.SubDomainSpoc
{
    public class SubDomainSpocGridDto
    {
        [OrderGrid(Order = 1)]
        [Default]
        public short Id { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        public string Description { get; set; }

        [OrderGrid(Order = 3)]
        [Default]
        public bool? isEdu { get; set; }

        [OrderGrid(Order = 4)]
        [Default]
        public bool? isSubDomain { get; set; }

        [OrderGrid(Order = 5)]
        [DateRangeGrid]
        [Default]

        public DateTime? LastModified { get; set; }

        [OrderGrid(Order = 6)]
        [MailTo]
        [DisplayName("Last Modified By")]
        [Default]
        public string LastModifiedBy { get; set; }
    }
}
