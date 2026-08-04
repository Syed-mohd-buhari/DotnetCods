using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Enum
{
    public class LcmEnum
    {
        public enum ReasonCheckboxResourceSoftware
        {

            PendingOMRenewal = 1,
            OpenSourceSW = 2,
            ItCanBeSupportedInternally = 3,
            VodafoneDecision = 4,
            Other = 5,
        }

        public enum ReasonCheckboxResourceHardware
        {

            HardwareRefreshLate = 6,
            BestEffortContractOnly = 7,
            Other = 8,
        }

        public enum SupportedResourceEnums
        {
            NoRule = 0,
            AsEquipmentManufacturer = 1,
            AsThirdParty = 2,
            AsNone = 3,
           
        }

        public enum DeploymentStatus
        {

            Planned = 1,
            Commissioning = 2,
            Decommissioning = 3,
            InService = 4,
            Removed = 5,
        }
    }
}
