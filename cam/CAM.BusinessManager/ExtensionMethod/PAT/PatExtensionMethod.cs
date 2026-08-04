using CAM.BusinessManager.Entity.Pat;
using CAM.DataTransferObjects.PAT;
using CAM.Exports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CAM.BusinessManager.ExtensionMethod.PAT
{
    public static class PatExtensionMethod
    {
        public static List<List<ExportSheetCustomHeader>> GetHeaders(this List<PTModel> patModel)
        {

            List<List<ExportSheetCustomHeader>> RetVal = new List<List<ExportSheetCustomHeader>>();

            var secondRow = new List<ExportSheetCustomHeader>();

            secondRow.Add(new ExportSheetCustomHeader { Name = "OPCO", ColSpan = 1, Show = true, Alignment = TextAlignmentEnum.Center, });

            secondRow.Add(new ExportSheetCustomHeader { Name = "DCF", ColSpan = 1, Show = true, Alignment = TextAlignmentEnum.Center, });
            secondRow.Add(new ExportSheetCustomHeader { Name = "Vertical Id", ColSpan = 1, Show = true, Alignment = TextAlignmentEnum.Center, });
            secondRow.Add(new ExportSheetCustomHeader { Name = "ProductName", ColSpan = 1, Show = true, Alignment = TextAlignmentEnum.Center, });

            foreach (var item in patModel[0].Months)
            {
                secondRow.Add(new ExportSheetCustomHeader { Name = item, ColSpan = 1, Show = true, Alignment = TextAlignmentEnum.Center, });
            }

            RetVal.Add(secondRow);
            return RetVal;
        }

        public static List<List<ExportSheetCustomCell>> GetData(this List<PTModel> list)
        {
            Dictionary<string, int> colorDictionary = new Dictionary<string, int>
            {
                { "blue", 0x2F75B5 },
                { "red", 0xe60000 },
                { "green", 0x00c300 },
                { "gray", 0xcccccc },
                { "black", 0x0F0D0D },
                { "white", 0xFFFFFF },
                { "yellow", 0xFFFF00 },
            };
            List<List<ExportSheetCustomCell>> RetVal = new List<List<ExportSheetCustomCell>>();
            foreach (var row in list)
            {
                //get rid of HTML tags
               var DCFName = Regex.Replace(row.DCFName, "<[^>]*>", string.Empty);

                //get rid of multiple blank lines
                DCFName = Regex.Replace(DCFName, @"^\s*$\n", string.Empty, RegexOptions.Multiline);

                List<ExportSheetCustomCell> newRow = new List<ExportSheetCustomCell>();
                newRow.Add(new ExportSheetCustomCell
                {
                    Value = row.OpcoName,
                });
                newRow.Add(new ExportSheetCustomCell
                {
                    Value = DCFName,
                });
                newRow.Add(new ExportSheetCustomCell
                {
                    Value = row.VerticalName,
                });
                newRow.Add(new ExportSheetCustomCell
                {
                    Value = row.ProductName,
                });
                if (row.Releases != null)
                {
                    if (row.Releases != null)
                    {
                        foreach (var col in row.Releases)
                        {
                            newRow.Add(new ExportSheetCustomCell
                            {
                                Value = col,
                            });
                        }
                    }

                }

                RetVal.Add(newRow);
            }
            return RetVal;
        }

        //Need to write a function get the cell colours
    }
}
