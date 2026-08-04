using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.ResourceKeyMaster
{
    public class ResourceKeyMasterDto
    {
      

        [OrderGrid(Order = 3)]
        [Default]
        public string ResourceKey { get; set; }

        [Default]
        [OrderGrid(Order = 4)]
        [DisplayName("Bag Name")]
        public string BagName { get; set; }

        [IgnoreGrid]
        public long BuildBagId { get; set; }
        [IgnoreGrid]
        public long ComponentId { get; set; }

        [Default]
        [OrderGrid(Order = 5)]
        public string ElementName { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        public int LifeCycleId { get; set; }

        [OrderGrid(Order = 8)]
        public string KeyStatus { get; set; }


        [OrderGrid(Order = 9)]
        public long ResourceKeyMasterId { get; set; }

        [OrderGrid(Order = 10)]
        public long ResourceTypesId { get; set; }

        [OrderGrid(Order = 11)]
        public long? DcfId { get; set; }

        [OrderGrid(Order = 12)]
        public short OpCoId { get; set; }

      
        [OrderGrid(Order = 13)]
        [MailTo]       
        public string CreationUser { get; set; }

        [OrderGrid(Order = 14)]
        [DateRangeGrid]
        public DateTime CreationDate { get; set; }

        [OrderGrid(Order = 15)]
        [MailTo]
        public string ModificationUser { get; set; }

        [OrderGrid(Order = 16)]
        [DateRangeGrid]
        public DateTime ModificationDate { get; set; }
    }
    public class ResourceKeyMasterDtoGrid:ResourceKeyMasterDto
    {
              
       

        [Default]
        [OrderGrid(Order = 1)]
        public string OpCo { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        public string DesignComponentFamilyName { get; set; }

        [OrderGrid(Order = 5)]
        [Default]
        public string ResourceType { get; set; }

       
    }
}
