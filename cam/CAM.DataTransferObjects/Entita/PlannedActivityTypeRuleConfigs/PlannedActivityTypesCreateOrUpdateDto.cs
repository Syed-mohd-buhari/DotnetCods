using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.PlannedActivityTypes
{
    public class PlannedActivityTypesCreateOrUpdateDto
    {
        [Default]
        [OrderGrid(Order = 1)]
        public int PlannedActivityTypesId { get; set; }

        [Default]
        [OrderGrid(Order = 1)]
        public string PlannedActivityTypeDescription { get; set; }

        [Default]
        [OrderGrid(Order = 2)]
        public bool HwOem { get; set; }

        [Default]
        [OrderGrid(Order = 3)]
        public bool HwSolution { get; set; }

        [Default]
        [OrderGrid(Order = 4)]
        public bool HwPlatform { get; set; }

        [Default]
        [OrderGrid(Order = 5)]
        public bool SwOem { get; set; }

        [Default]
        [OrderGrid(Order = 6)]
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

       
        [OrderGrid(Order = 10)]
        public int Id { get; set; }

        [Default]
        [OrderGrid(Order = 11)]
        public bool ForLcm { get; set; }

        [Default]
        [OrderGrid(Order = 12)]
        public bool ForAsset { get; set; }

        [Default]
        [OrderGrid(Order = 13)]
        public bool ForDesignAspect { get; set; }
        [Default]
        [OrderGrid(Order = 14)]
        public bool ForServicePlan { get; set; }
    } 
}
