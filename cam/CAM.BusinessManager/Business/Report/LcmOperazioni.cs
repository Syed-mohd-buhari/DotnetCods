using System;

namespace CAM.BusinessManager.Business.Report
{
    public static class LcmOperazioni
    {

        public static string LcnStatusEng(DateTime? value)
        {

            if (!value.HasValue)
            {
                return "ENG info missing";
            }
            if (value < DateTime.Now.AddYears(1) && value > DateTime.Now)
            {
                return "On expiration";
            }
            if (value > DateTime.Now.AddYears(1))
            {
                return "On support";
            }
            if (value < DateTime.Now)
            {
                return "Expired";
            }
            return "ENG info missing";
        }

        public static string LcnStatusOps(string value)
        {
            if (value == "No (VF decision)" || value == "No" || value == "Partial extended maintenance")
            {
                return "Expired";
            }

            if(string.IsNullOrEmpty(value))
            {
                return "OPS info missing";
            }

            return "On support";
        }

        public static string LcnStatus(string value, DateTime? date)
        {

            if (!string.IsNullOrEmpty(value) && !date.HasValue)
            {
                return "ENG info Missing";
            }
            if (string.IsNullOrEmpty(value) && date.HasValue)
            {
                return "OPS Missing";
            }
            if (value == "Yes" && date > DateTime.Now.AddYears(1))
            {
                return "On support";
            }
            if (value == "Yes" && date >= DateTime.Now && date <= DateTime.Now.AddYears(1))
            {
                return "On expiration";
            }
            if (value == "Full Extended Maintenance" && date >= DateTime.Now)
            {
                return "Wrong Combination";
            }
            if (value == "Full Extended Maintenance" && date >= DateTime.Now.Date && date <= DateTime.Now.AddYears(1).Date)
            {
                return "Wrong Combination";
            }
            if (value == "Full Extended Maintenance" && date < DateTime.Now)
            {
                return "On support";
            }
            if (value == "Partial Extended Maintenance" && !date.HasValue)
            {
                return "Wrong Combination";
            }
            
            if (value == "Partial Extended Maintenance" && date >= DateTime.Now)
            {
                return "Wrong Combination";
            }
            if (value == "Partial Extended Maintenance" && date < DateTime.Now)
            {
                return "Expired";
            }
            if ((value == "No" || value == "No VF (Decision)") && date < DateTime.Now)
            {
                return "Expired";
            }
            if (value == "Yes" && date < DateTime.Now)
            {
                return "Expired";
            }
            return "OPS & ENG info missing";
        }
    }
}