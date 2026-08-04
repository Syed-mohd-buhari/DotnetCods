using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.Entita.SystemType
{
    public abstract class SystemTypeDto : GridDtoBase
    {

        [TabGrid(TabName = "Export")]
        [DisplayName("Vodafone Name")]
        [OrderGrid(Order = 3)]
        [Default]
        public string VodafoneName { get; set; }

        //[TabGrid(TabName = "Export")]
        //[DisplayName("Design Contact")]
        //[OrderGrid(Order = 4)]
        //[Default]
       // public string DesignContact { get; set; }

        [TabGrid(TabName = "Export")]
        [DisplayName("SW Design Contact ")]
        [OrderGrid(Order = 4)]
        public string SoftWareDesignContactEmail { get; set; }

        [TabGrid(TabName = "Export")]
        [DisplayName("SW Vertical")]
        [OrderGrid(Order = 5)]
        public string SoftWareVertical { get; set; }


        [TabGrid(TabName = "Export")]
        [DisplayName("SW Subdomain")]
        [OrderGrid(Order = 6)]
        public string SoftWareSubdomain { get; set; }

        [TabGrid(TabName = "Export")]
        [DisplayName("HW Design Contact")]
        [OrderGrid(Order = 7)]
        [Default]
        public string HardWareDesignContactEmail { get; set; }


        [TabGrid(TabName = "Export")]
        [DisplayName("HW Vertical")]
        [OrderGrid(Order = 8)]
        public string HardWareVertical { get; set; }


        [TabGrid(TabName = "Export")]
        [DisplayName("HW Subdomain")]
        [OrderGrid(Order = 9)]
        public string HardWareSubdomain { get; set; }


        [IgnoreGrid]
        public uint VodafoneNameId { get; set; }

        [TabGrid(TabName = "Export")]
        [DisplayName("System Type Name Given by the Vendor")]
        [OrderGrid(Order = 14)]
        public string SystemTypeNameOem { get; set; }

        [TabGrid(TabName = "Export")]
        [DisplayName("Name 3Gpp")]
        [OrderGrid(Order = 15)]
        public string SystemTypeName3Gpp { get; set; }


        [TabGrid(TabName = "Export")]
        [DisplayName("Asset Type")]
        [OrderGrid(Order = 19)]
        public string AssetType { get; set; }

        [TabGrid(TabName = "Export")]
        [DateRangeGrid]
        [DisplayName("Constraint(Scaling)")]
        [OrderGrid(Order = 21)]
        public string ConstraintScaling { get; set; }

        [TabGrid(TabName = "Export")]
        [DisplayName("Constraint(Lcm)")]
        [OrderGrid(Order = 22)]
        public string ConstraintLcm { get; set; }

        [IgnoreGrid]
        public DateTime? EndOfMaintenanceDate { get; set; }

        [TabGrid(TabName = "Export")]
        [DateRangeGrid]
        [DisplayName("EOM")]
        [OrderGrid(Order = 23)]
        public string EndOfMaintenance { get; set; }

        [TabGrid(TabName = "Export")]
        [DisplayName("Last Modified By")]
        [MailTo]
        [OrderGrid(Order = 24)]
        public string LastModifiedBy { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [IgnoreGrid]
        public short? ProductImportanceId { get; set; }

        //[TabGrid(TabName = "Export")]
        //[DisplayName("Sub-Domain Spoc")]
        //[OrderGrid(Order = 21)]
        //[IgnoreGrid]
        //[Default]
        //public string SubDomainSpoc { get; set; }


        [IgnoreGrid]
        public DateTime? LastModified { get; set; }



        [IgnoreGrid]
        public string SpareFieldsJson { get; set; }

        [IgnoreGrid]
        public string LCMStatus { get; set; }
        [IgnoreGrid]
        public string SystemSolution { get; set; }


    }
}