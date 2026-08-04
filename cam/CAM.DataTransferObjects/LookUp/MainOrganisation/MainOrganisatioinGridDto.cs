using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.MainOrganisation
{
    public class MainOrganisatioinGridDto
    {
        [Default]
        [OrderGrid(Order = 1)]
        public int MainOrganisationId { get; set; }
        [Default]
        [OrderGrid(Order = 2)]

        public string MainOrganisationDescription { get; set; }
        [IgnoreGrid]
        public DateTime LastModified { get; set; }
        [Default]
        [OrderGrid(Order = 3)]

        public string LastModifiedBy { get; set; }
    }
}
