using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.PAT;
using CAM.Exports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CAM.BusinessManager.ExtensionMethod.Report
{
    public static class AssetPivotExtensionMethod
    {
        public static List<List<ExportSheetCustomHeader>> GetHeaders(this List<NetworkElementAsPlannedPivotDtoGrid> assetPivotModel)
        {

            List<List<ExportSheetCustomHeader>> RetVal = new List<List<ExportSheetCustomHeader>>();

            var secondRow = new List<ExportSheetCustomHeader>();

            secondRow.Add(new ExportSheetCustomHeader { Name = "Implementation Year", ColSpan = 1, Show = true, Alignment = TextAlignmentEnum.Center, });
            secondRow.Add(new ExportSheetCustomHeader { Name = "Design Component", ColSpan = 1, Show = true, Alignment = TextAlignmentEnum.Center, });
            secondRow.Add(new ExportSheetCustomHeader { Name = "Vertical Name", ColSpan = 1, Show = true, Alignment = TextAlignmentEnum.Center, });

            var location = assetPivotModel.FirstOrDefault()?.Locations.Select(t => t.Key).ToList();
            foreach (var item in location)
            {
                if (item.Contains("|"))
                {
                    var separateName = item.Split("|");
                    var rmColourName = separateName[0];
                    secondRow.Add(new ExportSheetCustomHeader { Name = rmColourName, ColSpan = 1, Show = true, Alignment = TextAlignmentEnum.Center, });
                }
                else
                {
                    secondRow.Add(new ExportSheetCustomHeader { Name = item, ColSpan = 1, Show = true, Alignment = TextAlignmentEnum.Center, });
                }
            }

            secondRow.Add(new ExportSheetCustomHeader { Name = "Total", ColSpan = 1, Show = true, Alignment = TextAlignmentEnum.Center, });

            RetVal.Add(secondRow);
            return RetVal;
        }

        public static List<List<ExportSheetCustomCell>> GetData(this List<NetworkElementAsPlannedPivotDtoGrid> list)
        { 
            List<List<ExportSheetCustomCell>> RetVal = new List<List<ExportSheetCustomCell>>();
            foreach (var row in list)
            {
                //get rid of HTML tags
               var DCFName = Regex.Replace(row.DesignComponent, "<[^>]*>", string.Empty);

                //get rid of multiple blank lines
                DCFName = Regex.Replace(DCFName, @"^\s*$\n", string.Empty, RegexOptions.Multiline);

                List<ExportSheetCustomCell> newRow = new List<ExportSheetCustomCell>();

                if (row.PaImplementaionYear != "")
                {
                    var paYear = row.PaImplementaionYear.Split("|");

                    if (row.PaImplementaionYear.Contains("|"))
                    {
                        newRow.Add(new ExportSheetCustomCell
                        {
                            Value = paYear[0],
                            BackgroundColor = 0xFBCEB1
                        });
                    }
                    else
                    {
                        newRow.Add(new ExportSheetCustomCell
                        {
                            Value = paYear[0],
                        });
                    }
                }
                else
                {
                    newRow.Add(new ExportSheetCustomCell
                    {
                        Value = row.PaImplementaionYear,
                        
                    });
                }

                newRow.Add(new ExportSheetCustomCell
                {
                    Value = DCFName,
                });

                newRow.Add(new ExportSheetCustomCell
                {
                    Value = row.VerticalName,
                }); 

                    if (row.Locations != null)
                    {
                    var locationCount = row.Locations.ToList();
                        foreach (var col in locationCount)
                        {
                        if (col.Key.Contains("|"))
                        {
                            newRow.Add(new ExportSheetCustomCell
                            {
                                Value = col.Value,
                                BackgroundColor = 0xFBCEB1
                            });
                        }
                        else
                        {
                            newRow.Add(new ExportSheetCustomCell
                            {
                                Value = col.Value
                            });
                        }                          
                        }
                    }

                newRow.Add(new ExportSheetCustomCell
                {
                    Value = row.Total,
                });
                RetVal.Add(newRow);
            }
            return RetVal;
        }

        //Need to write a function get the cell colours
    }
}
