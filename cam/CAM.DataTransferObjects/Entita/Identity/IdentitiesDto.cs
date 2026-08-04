using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.Entita.Idenitty
{
    public class IdentitiesDto
    {
        [OrderGrid(Order =3)]
        [Default]
        public string ElementName { get; set; }
        [OrderGrid(Order = 6)]
        [Default]
        public string IpAddress { get; set; }
        [OrderGrid(Order =7)]
        public string ApNodeAIpaddress { get; set; }
        [OrderGrid(Order =8)]
        public string ApNodeBIpaddress { get; set; }
        [OrderGrid(Order =9)]
        [MailTo]
        public string CreationUser { get; set; }
        [OrderGrid(Order =10)]
        [DateRangeGrid]
        public DateTime CreationDate { get; set; } 
        [OrderGrid(Order =11)]
        [MailTo]
        public string ModificationUser { get; set; }
        [OrderGrid(Order =12)]
        [DateRangeGrid]
        public DateTime ModificationDate { get; set; }
    }
    public class IDentitiesDtoGrid : IdentitiesDto
    {
        [OrderGrid(Order = 4)]
        public long NetworkElementId { get; set; }
        [OrderGrid(Order = 5)]
        public long IdentityId { get; set; }
        [OrderGrid(Order = 1)]
        [Default]
        public string OpCo { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        public string Oem { get; set; }
       

    }
}
