using System;
using System.Collections.Generic;
using System.Linq;

namespace CAM.BusinessManager.ExtensionMethod.NetworkElementAsIs
{
    public static class NetworkElementAsIsExtensionMethod
    {
        public static string toSoftwareReleaseInformation(this Entities.Models.NetworkElementAsIs src)
        {

            var name = ""; 
            if (src.SystemType != null && src.SystemType.MajorSoftwareBuilds != null) {
                name += src.SystemType.MajorSoftwareBuilds.SoftwareVersion;
            }
            if (src.PatchDetails != null) {
                name += " - " + src.PatchDetails;
            }

            return name;
        }
        public static string toDescription(this Entities.Models.NetworkElementAsIs src) {
            return src.ElementDeploymentName;
        }
    }
}
