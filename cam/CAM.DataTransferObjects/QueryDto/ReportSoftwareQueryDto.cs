using CAM.DataTransferObjects.FunctionalityDto;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ReportSoftwareQueryDto : ReportQueryDto
    {
        public List<string> AssetVirtualized { get; set; }
        public List<string> SoftwareVersion { get; set; }
        public DateFilter VendorEndOfVulnerabilitySecuritySupportDateValueLcm { get; set; }
       
        
        public List<string> OutputToLcmSoftware { get; set; }
        public List<string> LcmStatusOpsSoftware { get; set; }
        public List<string> LcmStatusEngSoftware { get; set; }

        public List<short> TypeOfNetworkElement { get; set; }

        public List<string> OriginalLCMSpreadsheetID { get; set; }
        public List<string> IsExtendedSupportOfferedByVendor { get; set; }

        #region LCM R9 Part-1
        //public List<string> CloudVersion { get; set; }
        //public List<string> CertifiedSWReleaseforNFVIbundle { get; set; }
        public List<string> LabSWRelease { get; set; }
        public List<string> OriginalSwLcmId { get; set; }


        #endregion

        #region LCM R11
        public List<string> ApplicationOrOperationgSys { get; set; }
        #endregion
        public List<string> Components { get; set; }


    }
}
