using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.ExodusAssetLevelReport
{
    public class ExodusAssetLevelReportQueryDto : QueryObject
    {
        public List<long> OpCo { get; set; }
        public List<long> DcfName { get; set; }
        public List<long> DcfId { get; set; }
        public List<long> SiteName { get; set; }
        public List<long> Vendor { get; set; }
        public List<long> HwVendor { get; set; }
        public List<short> HwPlatform { get; set; }
        public List<string> VnfCnf { get; set; }
        public List<string> VendorNf { get; set; }
        public List<string> XnfInstance { get; set; }
        public List<string> UsageOptional { get; set; }
        public List<int> XnfSizeCore { get; set; }
        public List<int> Environment { get; set; }
        public List<int> Status { get; set; }
        public List<string> LaasCaasInfraStackInitialGoLive { get; set; }
        public List<string> StackNameInitialGoLive { get; set; }
        public DateFilter RfoReqdBy { get; set; }
        public DateFilter RfsReqdBy { get; set; }
        public List<string> PoRaised { get; set; }
        public List<string> PoReqdByIfNotNa { get; set; }
        public List<string> ClusterName { get; set; }
        public List<string> HardwareTypeOld { get; set; }
        public List<string> HardwareTypeLive { get; set; }
        public List<int> XnfSizeVcpu { get; set; }
        public List<string> TargetXnfInstance { get; set; }

        public List<string> LaasCaasInfraStackTarget { get; set; }
        public List<string> StackNameTarget { get; set; }
        public List<string> PoReqdBy { get; set; }
        public DateFilter BomSubmittedDate { get; set; }
        public DateFilter HwPoRaisedDate { get; set; }
        public DateFilter HwPoArrivedDate { get; set; }
        public DateFilter RfaDate { get; set; }
        public DateFilter StartRfo { get; set; }
        public DateFilter Rfs { get; set; }
        public DateFilter MigrationCompletionDate { get; set; }
        public DateFilter VecDate { get; set; }
        public DateFilter StartOfAppIntegration { get; set; }
        public DateFilter MigrationStart { get; set; }

        public List<string> BroadcomRelease { get; set; }
        public List<string> HardwareTypeTarget { get; set; }
        public List<int> NfSizeExpansionDcekpiValue { get; set; }
        public List<int> NfSizeDcekpiSauGbps { get; set; }
        public List<string> AciAvailable { get; set; }
        public List<string> Power { get; set; }
        public List<string> Traffic { get; set; }
        public List<string> VerticalName {  get; set; }
        


    }
}