using CAM.DataTransferObjects.Entita.VolteKPI;
using CAM.Exports;
using CAM.Infrastucture.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using CAM.Entities.Models;

namespace CAM.BusinessManager.ExtensionMethod.VolteKPI
{
    public static class VolteKPIExtensionMethod
    {
        public static List<List<ExportSheetCustomHeader>> GetHeaders(this List<VolteKPIReportRow> list, short year)
        {

            List<List<ExportSheetCustomHeader>> RetVal = new List<List<ExportSheetCustomHeader>>();

            RetVal.Add(new List<ExportSheetCustomHeader>() {
                new ExportSheetCustomHeader { Name = "", ColSpan = 1, Show = false, ForeColor = null, BackgroundColor = 0x000000, Alignment = TextAlignmentEnum.Center,  },
                new ExportSheetCustomHeader { Name = "Q1", ColSpan = 3, Show = true, ForeColor = null, BackgroundColor = 0xff0000, Alignment = TextAlignmentEnum.Center, },
                new ExportSheetCustomHeader { Name = "Q2", ColSpan = 3, Show = true, ForeColor = null, BackgroundColor = 0xff0000, Alignment = TextAlignmentEnum.Center, },
                new ExportSheetCustomHeader { Name = "Q3", ColSpan = 3, Show = true, ForeColor = null, BackgroundColor = 0xff0000, Alignment = TextAlignmentEnum.Center, },
                new ExportSheetCustomHeader { Name = "Q4", ColSpan = 3, Show = true, ForeColor = null, BackgroundColor = 0xff0000, Alignment = TextAlignmentEnum.Center, }
            });

            var secondRow = new List<ExportSheetCustomHeader>();
            secondRow.Add(new ExportSheetCustomHeader { Name = "", ColSpan = 1, Show = false, ForeColor = null, BackgroundColor = 0x000000, Alignment = TextAlignmentEnum.Center, });
            for (int i = 4; i <= 12; i++)
            {
                var mese = CultureInfo.GetCultureInfo("us-EN").DateTimeFormat.GetAbbreviatedMonthName(i);
                secondRow.Add(new ExportSheetCustomHeader { Name = $"{mese}-{year.ToString().Substring(2, 2)}", ColSpan = 1, Show = true, ForeColor = null, BackgroundColor = 0xff0000, Alignment = TextAlignmentEnum.Center, });
            }
            for (int i = 1; i < 4; i++)
            {
                var mese = CultureInfo.GetCultureInfo("us-EN").DateTimeFormat.GetAbbreviatedMonthName(i);
                secondRow.Add(new ExportSheetCustomHeader { Name = $"{mese}-{(year + 1).ToString().Substring(2, 2)}", ColSpan = 1, Show = true, ForeColor = null, BackgroundColor = 0xff0000, Alignment = TextAlignmentEnum.Center, });
            }
            RetVal.Add(secondRow);
            return RetVal;
        }

        public static List<List<ExportSheetCustomCell>> GetData(this List<VolteKPIReportRow> list)
        {

            List<List<ExportSheetCustomCell>> RetVal = new List<List<ExportSheetCustomCell>>();
            foreach (var row in list)
            {
                List<ExportSheetCustomCell> newRow = new List<ExportSheetCustomCell>();
                newRow.Add(new ExportSheetCustomCell
                {
                    Value = row.OpCo,
                    BackgroundColor = null,
                    ForeColor = null,
                    FormatInfo = "",
                    FormatTypeInfo = ExportDataTypeEnum.Text,
                    Bold = true
                });
                foreach (var col in row.MonthValues.OrderBy(x => x.Quarter).ThenBy(x => x.Month))
                {
                    newRow.Add(new ExportSheetCustomCell
                    {
                        Value = float.Parse(col.Value),
                        BackgroundColor = Color.FromName(col.BackgroundColor).ToArgb(),
                        ForeColor = null,
                        FormatInfo = "###,#0.000",
                        FormatTypeInfo = ExportDataTypeEnum.NumberText
                    });
                }
                RetVal.Add(newRow);
            }
            return RetVal;
        }

        public static VolteKPITargetApprovalDto ToApprovalRecord(this CAM.Entities.Models.VolteKPI rec, VolteKPIType volteKPIType)
        {
            decimal monthlyTargetPrevious = 0;
            decimal monthlyTargetRequested = 0;
            string comment = "";


            switch (volteKPIType)
            {
                case VolteKPIType.KPI1_Capacity:
                    monthlyTargetPrevious = rec.KPIOneMonthlyTarget ?? 0;
                    monthlyTargetRequested = rec.KPIOneTargetValueChangeProposal ?? 0;
                    comment = rec.KPIOneComment;
                    break;
                    //case VolteKPIType.KPI2_Current_Utilization:
                    //    monthlyTargetPrevious = rec.KPITwoActualNumberOfProvisionedSubscriber ?? 0;
                    //    monthlyTargetRequested = rec.KPITwoTargetValueChangeProposal ?? 0;
                    //    comment = rec.KPITwoComment;
                    //break;
                case VolteKPIType.KPI3_Penetration:
                    monthlyTargetPrevious = rec.KPIThreeMonthlyTarget ?? 0;
                    monthlyTargetRequested = rec.KPIThreeTargetValueChangeProposal ?? 0;
                    comment = rec.KPIThreeComment;
                    break;
                case VolteKPIType.KPI4_3G_ShutDown:
                    monthlyTargetPrevious = rec.KPIFourTargetMonthly ?? 0;
                    monthlyTargetRequested = rec.KPIFourTargetValueChangeProposal ?? 0;
                    comment = rec.KPIFourComment;
                    break;
            }

            return new VolteKPITargetApprovalDto
            {
                VolteKPIId = rec.VolteKPIId,
                Month = rec.Month,
                Year = rec.Year,
                Type = volteKPIType,
                OpCoId = rec.OpCoId,
                KPIIDName = volteKPIType.GetDescription(),
                OpCo = rec.OpCo.OpCoDescription,
                MonthlyTargetPrevious = monthlyTargetPrevious,
                MonthlyTargetRequested = monthlyTargetRequested,
                MonthlyTargetNew = monthlyTargetRequested,
                Comment = comment,
                ApproveValueChangeProposal = false,
                ProposalDate = ((rec.ModificationUserEntity != null) ? rec.ModificationDate.ToShortDateString() : rec.CreationDate.ToShortDateString()),
                ProposedBy = ((rec.ModificationUserEntity != null) ? rec.ModificationUserEntity.Email : rec.CreationUserEntity.Email),

            };
        }
    }
}
