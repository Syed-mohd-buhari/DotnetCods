using CAM.DataAttributes.Grid;
using System;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.SoftwareConfiguration
{
    public class SWConfigDownloadExcel
    {
        [Default]
        [DisplayName("Software Configuration Id")]
        [OrderGrid(Order = 1)]
        public decimal SoftwareConfigurationId { get; set; }
        [Default]
        [DisplayName("Element Name")]
        [OrderGrid(Order = 4)]

        public string ElementName { get; set; }
        [Default]
        [DisplayName("OpCo")]
        [OrderGrid(Order = 2)]

        public string OpCo { get; set; }
        [Default]
        [DisplayName("Oem")]
        [OrderGrid(Order = 3)]


        public string Oem { get; set; }
        [Default]
        [DisplayName("Function Id")]
        [OrderGrid(Order = 5)]

        public decimal FunctionId { get; set; }
        [Default]
        [DisplayName("Function Name")]
        [OrderGrid(Order = 6)]


        public string FunctionName { get; set; }
        [Default]
        [OrderGrid(Order = 7)]
        [DisplayName("Function Area Id")]

        public decimal swConfigFunctionAreaId { get; set; }
        [Default]
        [DisplayName("Function Area Name")]
        [OrderGrid(Order = 8)]

        public string FunctionAreaName { get; set; }
        [Default]
        [DisplayName("Function Area Description")]
        [OrderGrid(Order = 9)]
        public string FunctionAreaDescription { get; set; }
        [Default]
        [DisplayName("Sub Function Id")]
        [OrderGrid(Order = 10)]
        public int SubFunctionId { get; set; }
        [Default]
        [DisplayName("Sub Function Name")]
        [OrderGrid(Order = 11)]
        public string SubFunctionName { get; set; }
        [Default]
        [DisplayName("Sub Function Area Id")]
        [OrderGrid(Order = 12)]
        public int SubFunctionAreaId { get; set; }
        [Default]
        [DisplayName("Sub Function Area Name")]
        [OrderGrid(Order = 13)]
        public string SubFuncAreaName { get; set; }
        [Default]
        [DisplayName("Sub Function Area Description")]
        [OrderGrid(Order = 14)]
        public string SubFuncAreaDescription { get; set; }
        [Default]
        [DisplayName("Creation User")]
        [OrderGrid(Order = 15)]
        public string CreationUser { get; set; }
        [Default]
        [DisplayName("Creation Date")]
        [OrderGrid(Order = 16)]
        public DateTime CreationDate { get; set; }
        [DisplayName("Modification User")]
        [OrderGrid(Order = 17)]
        public string ModificationUser { get; set; }
        [DisplayName("Modification Date")]
        [OrderGrid(Order = 18)]
        public DateTime ModificationDate { get; set; }

    }
}
