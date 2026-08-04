using System;
using System.ComponentModel;
using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using CAM.Enum;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CAM.DataTransferObjects.Entita.Report
{
    public abstract class ReportDto 
    {
        [DisplayName("ID")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 1)]
        [ColorGrid(Color = "blue")]
        [FormatClosetXml(Type = XLDataType.Text)]
        [Default]
        //[CellColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)] Example for cell background on entire column
        public string ReportId { get; set; }

        [DisplayName("Local Market")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 2)]
        [ColorGrid(Color = "red")]
        [Default]
        public string LocalMarket { get; set; }

        [DisplayName("Main Vertical")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 3)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string MainVertical { get; set; }

        [IgnoreGrid]
        public long DesignComponentIndex { get; set; }

        [DisplayName("Vertical Engineering Team")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 4)]
        [ColorGrid(Color = "red")]
        [Default]
        public string VerticalEngineeringTeam { get; set; }
        [DisplayName("Vertical Sub-Domain")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 5)]
        [ColorGrid(Color = "red")]
        [Default]
        public string VerticalSubDomain { get; set; }

        [DisplayName("Engineering Contact Point")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 6)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EngineeringContactPoint { get; set; }

        [DisplayName("Operations Contact Point")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [OrderGrid(Order = 7)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OperationsContactPoint { get; set; }

        
        [DisplayName("Components")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 8)]  
        [ColorGrid(Color = "red")]
        [Default]
        public string Components { get; set; }

        [DisplayName("Asset Category")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 9)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetCategory { get; set; }

        [DisplayName("Asset Class")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 10)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetClass { get; set; }

        [DisplayName("Asset Type")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 12)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetType { get; set; }

        [DisplayName("Asset Description")]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [OrderGrid(Order = 13)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string AssetDescription { get; set; }

        #region Ignore Grid Columns
        [DisplayName("User Owner")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string UserOwner { get; set; }

        [IgnoreGrid]
        public EOMEnum EOMStatus { get; set; }

        [IgnoreGrid]
        [Ignore(Ignore = true)]
        public long SystemTypeId { get; set; }
        [IgnoreGrid]
        [Ignore(Ignore = true)]
        public long DesignComponentId { get; set; }
        [IgnoreGrid]
        [Ignore(Ignore = true)]
        public long? MajorSoftwareBuildId { get; set; }
        [IgnoreGrid]
        [Ignore(Ignore = true)]
        public long? MajorHardwareBuildId { get; set; }
        [IgnoreGrid]
        [Ignore(Ignore = true)]
        public long LcmEngineeringId { get; set; }

        [IgnoreGrid]
        [Ignore(Ignore = true)]
        public long? PlannedActivityId { get; set; }

        [DisplayName("Support Date")]
        [OrderGrid(Order = 116)]
        [HeaderColor(BackgroundColor = 0x0F0D0D, FontColor = 0xffffff)]
        [ColorGrid(Color = "black")]
        [IgnoreGrid]
        public DateTime? SupportDate { get; set; }

        [DisplayName("Project Scope")]
        [OrderGrid(Order = 117)]
        [HeaderColor(BackgroundColor = 0x0F0D0D, FontColor = 0xffffff)]
        [ColorGrid(Color = "black")]
        [IgnoreGrid]
        public string ProjectScope { get; set; }

        [DisplayName("New Risk Evaluation")]
        [OrderGrid(Order = 118)]
        [HeaderColor(BackgroundColor = 0x0F0D0D, FontColor = 0xffffff)]
        [ColorGrid(Color = "black")]
        [IgnoreGrid]
        public string NewRiskEvaluation { get; set; }

        [DisplayName("Incident Cluster")]
        [OrderGrid(Order = 119)]
        [HeaderColor(BackgroundColor = 0x0F0D0D, FontColor = 0xffffff)]
        [ColorGrid(Color = "black")]
        [IgnoreGrid]
        public string IncidentCluster { get; set; }

        [DisplayName("Frequency Cluster")]
        [OrderGrid(Order = 120)]
        [HeaderColor(BackgroundColor = 0x0F0D0D, FontColor = 0xffffff)]
        [ColorGrid(Color = "black")]
        [IgnoreGrid]
        public string FrequencyCluster { get; set; }

        [DisplayName("Bag Name")]
        [OrderGrid(Order = 121)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string BagName { get; set; }
        [DisplayName("Component Name")]
        [OrderGrid(Order = 122)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ComponentName { get; set; }
        [DisplayName("Component Key")]
        [OrderGrid(Order = 123)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ComponentResourceKey { get; set; }



        [DisplayName("Vertical Engineering Team Id")]
        [IgnoreGrid]
        public int VerticalEngineeringTeamId { get; set; }
        #endregion
    }
}
