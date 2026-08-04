using CAM.Entities.Models;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.AbstractionLayer
{
    public class GetEosAndEomMileStonesDtoGrid
    {
        public string Product { get; set; }
        public string OEM { get; set; }
        public List<VersionDto> Versions { get; set; }
    }
    public class VersionDto
    {
        public string OpCo { get; set; }
        public string Version { get; set; }
        public string EomDate { get; set; }
        public string EosDate { get; set; }

        public long MajorSwId { get; set; }
    }

    public class DoingSectionForEomAnsEos : GetEosAndEomMileStonesDtoGrid
    {
        public string RagStatus {  get; set; }
    }
    public class EomEosActionsDto 
    {
        public List<DoingSectionForEomAnsEos> OpenActionsResource { get; set; }  
        public List<DoingSectionForEomAnsEos> UpcomingActionsResource { get; set; } 
    }

    public class LcmBasedEomEosGridDto  
    {
        public long LcmEngineeringId { get; set; }

        public string Opco { get; set; }

        public string Product { get; set; }

        public string Oem { get; set; }

        public string Version { get; set; }

        public DateTime? SwEndofmaintenance { get; set; }

        public DateTime? SwEndofsupport { get; set; }

        public long? MajorSwId { get; set; }

        public Majorsoftwarebuilds MajorSoftware { get; set; }

        public ICollection<Majorswbuildsdesigncontacts> MajorDesignContacts { get; set; }

        public bool IsLcmProductImportanceStrategic { get; set; }
    }

    public class HomePageEomEosGridDto
    {
        public long LcmEngineeringId { get; set; }

        public string Opco { get; set; }

        public string ProductAndPlatform { get; set; }

        public string Oem { get; set; }

        public string Version { get; set; }

        public DateTime? Endofmaintenance { get; set; }

        public DateTime? Endofsupport { get; set; }

        public long? MajorId { get; set; }

        public List<int> MajorDesignContacts { get; set; }

        public bool IsLcmProductImportanceStrategic { get; set; }
    }
    public class OpenLinkEomAndEosDto
    {
        public string ProductAndPlatfrom { get; set; }
        public string OEM { get; set; }
        public List<HomePageVersionDto> Versions { get; set; }
    }
    public class HomePageVersionDto
    {
        public string OpCo { get; set; }
        public string Version { get; set; }
        public string EomDate { get; set; }
        public string EosDate { get; set; }

        public long MajorId { get; set; }
    }

    public class LifeCycleManagerGridDto
    {
        public DateTime? Eom { get; set; }
        public DateTime? Eos { get; set; }
        public string Oem {  get; set; }
        public string ProductName { get; set; }
        public decimal ProductId { get; set; }
        public Plannedactivities Pa { get; set; }
        public bool IsStrategic { get; set; }
        public DateTime? Plannedcompletion { get; set; }
        public string ActionDetail { get; set; }
        public string PlannedDcName { get; set; }
        public string CurrentDcName { get; set; }
        public long LcmId { get; set; }
        public long PaId { get; set; }

    }

}

