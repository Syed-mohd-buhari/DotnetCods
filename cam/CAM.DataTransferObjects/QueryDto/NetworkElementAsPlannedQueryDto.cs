using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.QueryDto
{
    public class NetworkElementAsPlannedQueryDto : QueryObject
    {
        public List<long> NodeIndex{ get; set; }
        public List<int> VodafoneName { get; set; }
        public List<short> OpCo { get; set; }
        public List<long> DesignComponent { get; set; }
        public List<long> PlannedActivity { get; set; }
        public List<string> DesignComponentIndex { get; set; }
        public List<string> DesignComponentFamilyIndex { get; set; }
        public List<string> ElementName { get; set; }
        public List<string> CapacityPlanReference { get; set; }
        public List<string> AdditionalInformation1 { get; set; }
        public List<string> AdditionalInformation2 { get; set; }
        public List<bool> AutomatedFeedback { get; set; }
        public List<bool> PlannedAction { get; set; }
        public List<string> NetworkConstruct { get; set; }
        public List<short> Environment { get; set; }
        public List<short> DeploymentStatus { get; set; }
        public List<short> DeploymentType { get; set; }
        public List<short> Location { get; set; }
        

        public List<short> NfviBundleID { get; set; }
       
        public List<string> SubDomainSpoc { get; set; }
        public List<string> Eduspoc { get; set; }
        public DateFilter LastModifiedValue { get; set; }

        public List<string> HwResourceKey { get; set; }
        public List<string> PreviousHWResourceKey { get; set; }
        public List<string> SwResourceKey { get; set; }
        public List<string> PreviousSWResourceKey { get; set; }


        public List<long> LcmEngineeringId { get; set; }
        public List<string> LCMDeployementstatus { get; set; }

        public List<int> VerticalId { get; set; }
        public List<string> VerticalName { get; set; }

        #region ticket 1212 - Asset Pivot

        public List<string> PaImplementaionYear { get; set; }
        //public List<string> isPAImplementation { get; set; }
        public List<bool> isDcfImplementation { get; set; }

        public List<string> HardwareType { get;set; }

        #endregion

        public List<long> BuildBagDescription { get; set; }

        public List<bool> Assured { get; set; }

        public List<string> ElementDomianName { get; set; }
        public DateFilter AssetLiveStatusDateValue { get; set; }
        public DateFilter DateAssetDecommissionedAssetValue { get; set; }

        public List<bool> IsVirtualizedOrContanarized { get; set; }
    }
}
