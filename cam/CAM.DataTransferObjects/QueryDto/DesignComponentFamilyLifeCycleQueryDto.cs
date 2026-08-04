using System.Collections.Generic;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.QueryDto
{
    public class DesignComponentFamilyLifeCycleQueryDto : QueryObject
    {

        public List<long> DcfLifeCycleId { get; set; }
        public List<long> ResourceTypesId { get; set; }
        public List<string> ResourceKey { get; set; }
        public List<string> PreviousResourceKey { get; set; }
        public List<short> OpcoId { get; set; }
        public List<long> DcfId { get; set; }
        public List<long> DcId { get; set; }
        public List<string> EventName { get; set; }
        public List<long> EventId { get; set; }
        public List<string> Notes { get; set; }
        public List<string> CreationUser { get; set; }
        public DateFilter CreationDate { get; set; }
        public List<string> ModificationUser { get; set; }
        public DateFilter ModificationDate { get; set; }
        public List<short> OpCo { get; set; }
        public List<long> DesignComponent { get; set; }
        public List<long> DesignComponentFamilyName { get; set; }
        public List<long> NodeIndex { get; set; }
        public List<string> CurrentDetail { get; set; }

       // public List<string> OpcoDescription { get; set; }

        //public List<string> DcfDescription { get; set; }
        public List<string> CategoryType { get; set; }
        public List<string> PlannedDetails { get; set; }
        //public List<long> DcDescription { get; set; }


    }
}