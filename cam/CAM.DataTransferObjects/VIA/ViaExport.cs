using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Enum;

namespace CAM.DataTransferObjects.VIA
{
    public class ViaExport : IEquatable<ViaExport>
    {
        [OrderGrid(Order = 1)]
        [DisplayName("ME_SOURCE_ASSET_ID ")]
        [Default]
        public string MeSourceAssetId { get; set; }

        [DisplayName("ME_NAME")]
        [OrderGrid(Order = 2)]
        [Default]
        public string MeName { get; set; }

        [DisplayName("ME_TYPE")]
        [OrderGrid(Order = 3)]
        [Default]
        public string MeType { get; set; }

        [DisplayName("LCM_PROD_NAME")]
        [OrderGrid(Order = 5)]
        [Default]
        public string LcmProdName { get; set; }

        [DisplayName("VENDOR")]
        [OrderGrid(Order = 6)]
        [Default]
        public string Vendor { get; set; }

        [DisplayName("EOSL_CONTRACT_DATE")]
        [OrderGrid(Order = 7)]
        [DateRangeGridString]
        [Default]
        public string EoslContractDate { get; set; }

        [DisplayName("EEOSL_CONTRACT_DATE")]
        [OrderGrid(Order = 8)]
        [DateRangeGridString]
        [Default]
        public string EeoslContractDate { get; set; }

        [DisplayName("LOCATION_NAME")]
        [OrderGrid(Order = 9)]
        [Default]
        public string LocationName { get; set; }

        [DisplayName("ME_DESCRIPTION")]
        [OrderGrid(Order = 10)]
        [Default]
        public string MeDescription { get; set; }

        [DisplayName("ME_STATUS")]
        [OrderGrid(Order = 11)]
        [Default]
        public string MeStatus { get; set; }

        [DisplayName("ME_IP_ADDRESS")]
        [OrderGrid(Order = 12)]
        [Default]
        public string MeIpAddress { get; set; }

        [DisplayName("ME_VIRTUAL_FLG")]
        [OrderGrid(Order = 13)]
        [Default]
        public string MeVirtualFlg { get; set; }

        [DisplayName("ORGANISATION_NAME")]
        [OrderGrid(Order = 14)]
        [Default]
        public string OrganisationName { get; set; }

        [DisplayName("OS_NAME")]
        [OrderGrid(Order = 15)]
        [Default]
        public string OsName { get; set; }

        [DisplayName("OS_EOSL_CONTRACT_DATE")]
        [OrderGrid(Order = 16)]
        [DateRangeGridString]
        [Default]
        public string OsEoslContractDate { get; set; }

        [DisplayName("OS_EEOSL_CONTRACT_DATE")]
        [OrderGrid(Order = 17)]
        [DateRangeGridString]
        [Default]
        public string OsEeoslContractDate { get; set; }

        [DisplayName("LCMPolicy")]
        [OrderGrid(Order = 18)]
        [Default]
        public string MeLCMPolicy { get; set; }

        [DisplayName("Business Critical")]
        [OrderGrid(Order = 19)]
        [Default]
        public string MeCniBcServiceFlg { get; set; }

        [DisplayName("ME_CRITICALITY")]
        [OrderGrid(Order = 20)]
        [Default]
        public string MeCriticality { get; set; }

        [DisplayName("ME_DEPLOYMENT_TYPE")]
        [OrderGrid(Order = 21)]
        [Default]
        public string MeDeploymentType { get; set; }

        [DisplayName("ME_ENVIRONMENT")]
        [OrderGrid(Order = 22)]
        [Default]
        public string MeEnvironment { get; set; }

        [DisplayName("ME_EXTERNAL_CONNECTION_FLG")]
        [OrderGrid(Order = 23)]
        [Default]
        public string MeExternalConnectionFlg { get; set; }

        [DisplayName("ME_FQDN")]
        [OrderGrid(Order = 24)]
        [Default]
        public string MeFqdn { get; set; }

        [DisplayName("ME_GDPR_RELEVANT_FLG")]
        [OrderGrid(Order = 25)]
        [Default]
        public string MeGdprRelevantFlg { get; set; }

        [DisplayName("ME_INSTALLATION_DATE")]
        [OrderGrid(Order = 26)]
        [Default]
        //[DateRangeGridString]
        public string MeInstallationDate { get; set; }

        [DisplayName("ME_LAST_MAJOR_UPGRADE_DATE")]
        [OrderGrid(Order = 27)]
        [DateRangeGridString]
        [Default]
        public string MeLastMajorUpgradeDate { get; set; }

        [DisplayName("ME_LAST_SCAN_DATE")]
        [OrderGrid(Order = 28)]
        [DateRangeGridString]
        [Default]
        public string MeLastScanDate { get; set; }

        [DisplayName("ME_MAC_ADDRESS")]
        [OrderGrid(Order = 29)]
        [Default]
        public string MeMacAddress { get; set; }

        [DisplayName("ME_SERIAL_NUMBER")]
        [OrderGrid(Order = 30)]
        [Default]
        public string MeSerialNumber { get; set; }

        [DisplayName("ME_SERVICE_TYPE")]
        [OrderGrid(Order = 31)]
        [Default]
        public string MeServiceType { get; set; }

        [DisplayName("ME_CYBERARK_INTEGRATION_FLG")]
        [OrderGrid(Order = 32)]
        [Default]
        public string MeCyberarkIntegrationFlg { get; set; }

        [DisplayName("ME_IDM_INTEGRATION_FLG")]
        [OrderGrid(Order = 33)]
        [Default]
        public string MeIdmIntegrationFlg { get; set; }

        [DisplayName("ME_SECURITY_TIER")]
        [OrderGrid(Order = 34)]
        [Default]
        public string MeSecurityTier { get; set; }


        [DisplayName("ME_2FA_INTEGRATION_FLG")]
        [OrderGrid(Order = 35)]
        [Default]
        public string Me2FaIntegrationFlg { get; set; }

        [DisplayName("ME_SIEM_INTEGRATION_FLG")]
        [OrderGrid(Order = 36)]
        [Default]
        public string MeSiemIntegrationFlg { get; set; }

        [DisplayName("SUPPORT_CONTRACT")]
        [OrderGrid(Order = 37)]
        [Default]
        public string SupportContract { get; set; }

        [DisplayName("LCM_STATUS")]
        [OrderGrid(Order = 38)]
        [Default]
        public string LcmStatus { get; set; }

        [IgnoreGrid]
        [OrderGrid(Order = 39)]
        [Default]
        public EOMEnum EOMStatus { get; set; }
        public bool Equals(ViaExport other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return MeSourceAssetId == other.MeSourceAssetId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ViaExport)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(MeSourceAssetId);
            return hashCode.ToHashCode();
        }
        [DisplayName("ResourceKey")]
        [OrderGrid(Order = 4)]
        [Default]
        public string resourceKey { get; set; }

        [DisplayName("Vertical Name")]
        [OrderGrid(Order = 40)]
        [Default]
        public string VerticalName { get; set; }

        
        [IgnoreGrid]
        public int VerticalNameId { get; set; }

    }

    public class ViaExportQuery : QueryObject
    {
        public List<string> MeSourceAssetId { get; set; }
        public List<string> MeName { get; set; }
        public List<string> MeType { get; set; }
        public List<string> LcmProdName { get; set; }
        public List<string> Vendor { get; set; }
        public DateFilter EoslContractDate { get; set; }
        public DateFilter EeoslContractDate { get; set; }
        public List<string> LocationName { get; set; }
        public List<string> MeDescription { get; set; }
        public List<string> MeStatus { get; set; }
        public List<string> MeIpAddress { get; set; }
        public List<string> MeVirtualFlg { get; set; }
        public List<string> OrganisationName { get; set; }

        public List<short> OrganisationNameId { get; set; }
        public List<string> OsName { get; set; }
        public DateFilter OsEoslContractDate { get; set; }
        public DateFilter OsEeoslContractDate { get; set; }
        public List<string> MeCniBcServiceFlg { get; set; }
        public List<string> MeCriticality { get; set; }
        public List<short> MeLCMPolicy { get; set; }
        public List<string> MeDeploymentType { get; set; }
        public List<string> MeEnvironment { get; set; }
        public List<string> MeExternalConnectionFlg { get; set; }
        public List<string> MeFqdn { get; set; }
        public List<string> MeGdprRelevantFlg { get; set; }
        public List<string> MeInstallationDate { get; set; }
        public DateFilter MeLastMajorUpgradeDate { get; set; }
        public DateFilter MeLastScanDate { get; set; }
        public List<string> MeMacAddress { get; set; }
        public List<string> MeSerialNumber { get; set; }
        public List<string> MeServiceType { get; set; }
        public List<string> MeCyberarkIntegrationFlg { get; set; }
        public List<string> MeIdmIntegrationFlg { get; set; }
        public List<string> MeSecurityTier { get; set; }
        public List<string> Me2FaIntegrationFlg { get; set; }
        public List<string> MeSiemIntegrationFlg { get; set; }
        public List<string> SupportContract { get; set; }
        public List<string> LcmStatus { get; set; }
        public List<string>ResourceKey { get; set; }
        public int SkipAmount { get; set; }
        public List<SkipManager> SkipManager { get; set; }

        public List<string> VerticalName { get; set; }

        public List<int> VerticalNameId { get; set; }
    }

    public class SkipManager
    {
        public int Page { get; set; }
        public int SkipAmount { get; set; }
    }
}