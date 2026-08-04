using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.BusinessManager.ExtensionMethod.MajorHardwareBuild
{
    public static class MajorHardwareBuildMethod
    {
        public static string toDescription(this Entities.Models.MajorHardwareBuild entity) {
            return entity.HardwareSolution;
        }
    }
}
