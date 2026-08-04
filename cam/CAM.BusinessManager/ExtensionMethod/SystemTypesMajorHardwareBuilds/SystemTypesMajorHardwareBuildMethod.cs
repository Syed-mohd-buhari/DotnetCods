namespace CAM.BusinessManager.ExtensionMethod.SystemTypesMajorHardwareBuilds
{
    public static class SystemTypesMajorHardwareBuildMethod
    {
        public static string toHardwareSolution(this Entities.Models.Cross.SystemTypesMajorHardwareBuild entity)
        {
            if (entity == null) return "";
            return $"{entity.MajorHardware.HardwareSolution}-{entity.MajorHardware.Platform.PlatformDescription} - {entity.MajorHardware.HardwareType}";
        }
    }
}
