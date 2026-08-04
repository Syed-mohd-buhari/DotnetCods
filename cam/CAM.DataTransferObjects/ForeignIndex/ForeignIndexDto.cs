using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.Enum;

namespace CAM.DataTransferObjects.ForeignIndex
{

    public class ForeignIndexDto
    {
        public FI_SessionDto Session { get; set; }
        public List<FI_DesignComponentDto> DesignComponents { get; set; }
        public List<FI_SystemTypesDto> SystemTypes { get; set; }
        public List<FIGroupKey> Groups { get; set; }
        public List<FIGroupRecord> GroupToUpdate { get; set; }
        public List<ImpactCheckDto> ImpactChecks { get; set; }
    }

    public class FI_SessionDto
    {
        public long SessionId { get; set; }
        public ForeignIndexStatus Status { get; set; }
        public ForeignIndexSource Source { get; set; }
        [DateRangeGrid]
        [DisplayName("Last Modified")]
        public DateTime? LastModified { get; set; }

        [MailTo]
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; set; }
    }
    public class FI_DesignComponentDto
    {
        public long FI_DesignComponentId { get; set; }
        public long SessionId { get; set; }
        public long DesignComponentId { get; set; }
        public string Description { get; set; }
        public bool ToDelete { get; set; }
        [DateRangeGrid]
        [DisplayName("Last Modified")]
        public DateTime? LastModified { get; set; }

        [MailTo]
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; set; }
    }

    public class FI_SystemTypesDto
    {
        public long FI_SystemTypesId { get; set; }
        public long SessionId { get; set; }
        public long? DesignComponentId { get; set; }
        public long SystemTypesId { get; set; }
        public string Description { get; set; }
        public bool ToDelete { get; set; }
        [DateRangeGrid]
        [DisplayName("Last Modified")]
        public DateTime? LastModified { get; set; }

        [MailTo]
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; set; }
    }

    public class FIGroupKey
    {
        public short OEM { get; set; }
        public DateTime EOM { get; set; }
        public Dictionary<long, FIGroupRecord> Records { get; set; }
        public List<Dictionary<long, FIGroupRecord>> SimilarityGroups { get; set; }
        public List<Dictionary<long, FIGroupRecord>> CharDiffGroups { get; set; }
        public Dictionary<long, string> DropDownResource { get; set; }


        //public int[,] Similarity { get; set; }
    }
    public class FIGroupRecord
    {
        public long Key { get; set; }
        public string Description { get; set; }
        public string ToAnalyze { get; set; }
        public bool Selected { get; set; }
        public bool Correct { get; set; }
        public DateTime ModificationDate { get; set; }
        public int Count { get; set; }
        public List<ImpactCheckDto> ImpactChecks { get; set; }


        //public Dictionary<long, int> RecordsSimilarity { get; set; }
        //public Dictionary<long, int> RecordsCharDiff { get; set; }
    }

    public class ImpactCheckDto
    {
        public string key { get; set; }

        public List<string> LcmName { get; set; }
        public List<string> LcmNameNew { get; set; }
        public string Opco { get; set; }
        public bool Correct { get; set; }
        public bool NotToShow { get; set; }

        public bool ToDelete { get; set; }
        public List<PlannedActivityImpactResult> PlannedActivity { get; set; }
        public string Type { get; set; }
        public SystemTypeChecker SystemTypeChecker { get; set; }
        public long DesignComponentId { get; set; }
        public long LcmEngeeneringId { get; set; }
        public long NetworkElementId { get; set; }
    }
    public class PlannedActivityImpactResult
    {
        public string Opco { get; set; }
        public string DesignComponent { get; set; }
        public int NumberOfNodes { get; set; }
        public int NumberOfNodesInLab { get; set; }
        public string PproductImportance { get; set; }
        public string PlannedAction { get; set; }
        public string ActivityIndex { get; set; }
        public string Eduspoc { get; set; }
        public string SubDomainSpoc { get; set; }
    }

    public class DesignComponentChecker : IEquatable<DesignComponentChecker>
    {
        public long SistemTypeId { get; set; }
        public long DesignComponentFamilyId { get; set; }

        public bool Equals(DesignComponentChecker other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return SistemTypeId == other.SistemTypeId && DesignComponentFamilyId == other.DesignComponentFamilyId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DesignComponentChecker)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SistemTypeId, DesignComponentFamilyId);
        }
    }

    public class SystemTypeChecker : IEquatable<SystemTypeChecker>
    {
        public long MajorHardwareId { get; set; }
        public long MajorSoftwareId { get; set; }
        public string SystemTypeNameOem { get; set; }

        public bool Equals(SystemTypeChecker other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return MajorHardwareId == other.MajorHardwareId && MajorSoftwareId == other.MajorSoftwareId && SystemTypeNameOem == other.SystemTypeNameOem;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SystemTypeChecker)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MajorHardwareId, MajorSoftwareId, SystemTypeNameOem);
        }
    }


}