using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Infrastucture.Enums;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.VolteKPI
{
    public class VolteKPIDto : GridDtoBase
    {
        [OrderGrid(Order = 2)]
        public short Month { get; set; }
        [OrderGrid(Order = 3)]
        public short Year { get; set; }

        [OrderGrid(Order = 4)]
        public decimal? KPIOneEoYTarget { get; set; }
        [OrderGrid(Order = 5)]
        public decimal? KPIOneMonthlyTarget { get; set; }
        [OrderGrid(Order = 6)]
        public decimal? KPIOneActualValue { get; set; }
        [OrderGrid(Order = 7)]
        public decimal? KPIOneTargetValueChangeProposal { get; set; }
        [OrderGrid(Order = 8)]
        public string KPIOneComment { get; set; }

        [OrderGrid(Order = 9)]
        public decimal? KPITwoActualNumberOfRegisteredSubscribers { get; set; }
        [OrderGrid(Order = 10)]
        public decimal? KPITwoActualNumberOfProvisionedSubscriber { get; set; }
        //[OrderGrid(Order = 11)]
        //public decimal? KPITwoActualValue { get; set; }
        //[OrderGrid(Order = 12)]
        //public decimal? KPITwoTargetValueChangeProposal { get; set; }
        [OrderGrid(Order = 13)]
        public string KPITwoComment { get; set; }

        [OrderGrid(Order = 14)]
        public decimal? KPIThreeEoYTarget { get; set; }
        [OrderGrid(Order = 15)]
        public decimal? KPIThreeMonthlyTarget { get; set; }
        [OrderGrid(Order = 16)]
        public decimal? KPIThreeActualValue { get; set; }
        [OrderGrid(Order = 17)]
        public decimal? KPIThreeTargetValueChangeProposal { get; set; }
        [OrderGrid(Order = 18)]
        public string KPIThreeComment { get; set; }

        [OrderGrid(Order = 19)]
        public decimal? KPIFourFinalTarget { get; set; }
        [OrderGrid(Order = 20)]
        public decimal? KPIFourTargetMonthly { get; set; }
        [OrderGrid(Order = 21)]
        public decimal? KPIFourActualMonthly { get; set; }
        [OrderGrid(Order = 22)]
        public decimal? KPIFourTargetValueChangeProposal { get; set; }
        [OrderGrid(Order = 23)]
        public short? KPIFourTargetDateMonth { get; set; }
        [OrderGrid(Order = 24)]
        public short? KPIFourTargetDateYear { get; set; }
        [OrderGrid(Order = 25)]
        public string KPIFourComment { get; set; }
    }
}
