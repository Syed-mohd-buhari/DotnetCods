using CAM.Entities.Models;
using CAM.Infrastucture.Enums;

namespace CAM.BusinessManager.ExtensionMethod.VolteKPI
{
    public static class VolteKPIWorklogExtensions
    {
        public static string GetMonthYear(this VolteKPIWorklog volteKPIWorklog)
        {
            return $"{volteKPIWorklog.Month.ToString("00")}/{volteKPIWorklog.Year.ToString()}";
        }
        public static string GetKPIName(this VolteKPIWorklog volteKPIWorklog)
        {
            return ((VolteKPIType)volteKPIWorklog.VolteKPIType).GetDescription();
        }
        public static string GetMonthYearStringFormat(this VolteKPIWorklog volteKPIWorklog)
        {
            return $"{GetMonthName(volteKPIWorklog.Month)}/{volteKPIWorklog.Year.ToString()}";
        }
        public static string GetMonthName(short month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "";
            }
        }
    }
}
