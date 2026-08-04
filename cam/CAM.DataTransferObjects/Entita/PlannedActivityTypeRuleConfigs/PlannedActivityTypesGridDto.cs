using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.PlannedActivityTypes
{
    public class PlannedActivityTypesGridDto
    {
        [IgnoreGrid]
        [OrderGrid(Order = 0)]
        public long PlannedActivityTypesId { get; set; }

        [Default]
        [OrderGrid(Order = 1)]
        public string PlannedActivityTypeDescription { get; set; }

        [Default]
        [OrderGrid(Order = 2)]
        public bool HwOem { get; set; }

        [Default]
        [OrderGrid(Order = 3)]
        public bool SwOem { get; set; }

        [Default]
        [OrderGrid(Order = 4)]
        public bool HwSolution { get; set; }

        [Default]
        [OrderGrid(Order = 5)]
        public bool HwPlatform { get; set; }



        [Default]
        [OrderGrid(Order =6)]
        public bool SwVersion { get; set; }

        [Default]
        [OrderGrid(Order = 7)]
        public bool SwProductname { get; set; }
        
        [Default]
        [OrderGrid(Order = 8)]
        public bool SubNetworkService { get; set; }       

        [Default]
        [OrderGrid(Order = 9)]
        public bool LinkedDcRule { get; set; }

        [IgnoreGrid]
        [OrderGrid(Order =10)]
        public bool Deleted { get; set; }
        [MailTo]
        public string CreationUser { get; set; }
        [OrderGrid(Order = 11)]
        [DateRangeGrid]
        public DateTime CreationDate { get; set; }
        [OrderGrid(Order = 12)]
        [MailTo]
        public string ModificationUser { get; set; }
        [OrderGrid(Order = 13)]
        [DateRangeGrid]
        public DateTime ModificationDate { get; set; }

        [OrderGrid(Order = 14)]
        public bool ForLcm { get; set; }

        [OrderGrid(Order = 15)]
        public bool ForAsset { get; set; }

        [OrderGrid(Order = 16)]
        public bool ForDesignAspect { get; set; }

        [OrderGrid(Order = 17)]
        public bool ForService { get; set; }

    } 
}
