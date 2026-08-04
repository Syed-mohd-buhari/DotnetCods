using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Infrastucture.Enums
{
    public enum VolteKPIType
    {
        KPI1_Capacity = 1,
        KPI2_Current_Utilization = 2,
        KPI3_Penetration = 3,
        KPI4_3G_ShutDown = 4
    }

    public static class VolteKPITypeExtension
    {
        public static string GetDescription(this VolteKPIType value)
        {
            var name = System.Enum.GetName(typeof(VolteKPIType), value);
            if (name == null) return "";
            return name.Replace("_", " ");
        }
    }
}
