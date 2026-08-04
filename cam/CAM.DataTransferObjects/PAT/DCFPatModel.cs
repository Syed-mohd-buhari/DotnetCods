using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CAM.DataTransferObjects.PAT
{
    public class PTModel{
        public string OpcoName { get; set; }
        public short OpcoId { get; set; }
        public long DCFId { get; set; }
        public string DCFName { get; set; }
        public string ProductName { get; set; }
        public List<string> Months { get; set; } 
        public List<string> Releases { get; set; }
        public int order { get; set; }

        public DateTime LastModifiedDate { get; set; }
        public string VerticalName { get; set; }
        public List<FilterValueDto> VerticalResponsible { get; set; }
        public int VerticalNameId { get; set; }
        public ColorGrid ColorGrids { get; set; }
    }
    public class DCFPatModel
    {
        public int order { get; set; }

        [OrderGrid(Order = 1)]
        [DisplayName("OPCO")]
        public string OpcoName { get; set; }
        public short OpcoId { get; set; }

        public DateTime LastModifiedDate { get; set; }
        public long DCFId { get; set; }
        [OrderGrid(Order = 2)]
        [DisplayName("DCF")]
        public string DCFName { get; set; }

        [IgnoreGrid]
        public string ProductName { get; set; }
        public List<PlannedActivityReleaseModel> PlannedActivities { get; set; }
        public List<string> Months { get; set; } = GetMonths();
        public List<int> MonthsNumbers { get; set; } = GetMonthsNumber();

        public List<string> Releases { get; set; }
        [OrderGrid(Order = 3)]
        [DisplayName("Vertical Name")]
        public string VerticalName { get; set; }

        [IgnoreGrid]
        public List<FilterValueDto> VerticalResponsible { get; set; }


        [IgnoreGrid]
        public int VerticalNameId { get; set; }

        public ColorGrid ColorGrids { get; set; }
        public static IEnumerable<string> MonthsBetween(DateTime startDate, DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return (string.Format("{1}-{0}", (iterator.Year % 100).ToString(), dateTimeFormat.GetAbbreviatedMonthName(iterator.Month)));

                iterator = iterator.AddMonths(1);
            }
        }
        public static IEnumerable<int> MonthsBetweenNumbers(DateTime startDate, DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            iterator = new DateTime(startDate.Year, startDate.Month, 1);
            limit = endDate;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            //else
            //{
            //    iterator = new DateTime(endDate.Year, endDate.Month, 1);
            //    limit = startDate;
            //}

            while (iterator <= limit)
            {
                var equation = iterator.Year + iterator.Month;
                var totalMonths = ((iterator.Year - DateTime.Today.Year) * 12) + iterator.Month - DateTime.Today.Month;
                yield return (equation + totalMonths);

                iterator = iterator.AddMonths(1);
            }
        }
        private static List<string> GetMonths()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(17);
            return MonthsBetween(startDate, endDate).ToList();
        }
        private static List<int> GetMonthsNumber()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(17);
            return MonthsBetweenNumbers(startDate, endDate).ToList();
        }
      
    }

    public class ColorGrid
    {
        public List<string> ColorCode { get; set; }
        public List<string> ReleaseNames { get; set;}
    }

}
