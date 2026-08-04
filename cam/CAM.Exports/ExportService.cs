using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.Entita;
using CAM.DataTransferObjects.Entita.AssetPassThrough;
using CAM.DataTransferObjects.Entita.TsrPassThrough;
using CAM.DataTransferObjects.Entita.XBom.CBom;
using CAM.DataTransferObjects.Entita.XBom.VBom;
using CAM.Infrastucture.QueryResult;
using ClosedXML.Excel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Hosting;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using OracleModels.DBModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CAM.Exports
{
    public class ExportService : IExportService
    {
        private readonly IWebHostEnvironment _env;
        private const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public void GetCSVFrom(string filePath)
        {
            //var book = new Workbook("template.xlsx");
            //book.Save("output.csv", Aspose.Cells.SaveFormat.Auto);
        }
        public void GetExcelFromCSV(List<ExportSheet> sheets, string fileName)
        {
            using XLWorkbook wbk = new XLWorkbook();
            using MemoryStream streamFile = new MemoryStream();

            //Worksheet
            foreach (ExportSheet sheet in sheets)
            {
                const int headerRow = 1;
                int dataRow = 2;
                var worksheet = wbk.Worksheets.Add(sheet.TabName);

                if (sheet.Data.Count > 0)
                {
                    //Header
                    var typeOfData = sheet.Data[0].GetType();
                    var headingsBase = typeOfData.BaseType?.GetProperties();
                    var headingsInherited = typeOfData.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    var headings = headingsBase.Concat(headingsInherited);
                    var ignoreColumn = false;

                    var initHeaderColumn = 0;





                    foreach (PropertyInfo headerInfo in headings)
                    {

                        ignoreColumn = headerInfo.GetCustomAttribute<IgnoreAttribute>() == null ? false : headerInfo.GetCustomAttribute<IgnoreAttribute>().Ignore;

                        if (!ignoreColumn)
                        {
                            //Format the header
                            var headerColumnName = headerInfo.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Name : headerInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            var headerBackgroundColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerFontColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            worksheet.Cell(headerRow, initHeaderColumn + 1).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));

                            worksheet.Cell(headerRow, initHeaderColumn + 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                            worksheet.Cell(headerRow, initHeaderColumn + 1).Value = headerColumnName;

                            initHeaderColumn++;
                        }
                    }


                    //Rows
                    foreach (object item in sheet.Data)
                    {
                        var initColumn = 0;

                        var propertyInfoBase = item.GetType().BaseType.GetProperties();
                        var propertyInfoInherited = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        var propertyInfo = propertyInfoBase.Concat(propertyInfoInherited);

                        foreach (PropertyInfo info in propertyInfo)
                        {
                            ignoreColumn = info.GetCustomAttribute<IgnoreAttribute>() == null ? false : info.GetCustomAttribute<IgnoreAttribute>().Ignore;

                            if (!ignoreColumn)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.GetCustomAttribute<FormatAttribute>()?.Format;

                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, initColumn + 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, initColumn + 1).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, initColumn + 1).Style.Border.SetOutsideBorderColor(XLColor.Black);

                                ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                switch (formatType)
                                {
                                    case ExportDataTypeEnum.Text:
                                        worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Text;

                                        break;
                                    case ExportDataTypeEnum.Date:
                                        worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.DateTime;

                                        break;
                                    case ExportDataTypeEnum.Number:
                                        worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Number;

                                        break;
                                    case ExportDataTypeEnum.Boolean:
                                        worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Boolean;
                                        break;
                                    case ExportDataTypeEnum.DateText:
                                        if (!string.IsNullOrEmpty(cellFormatInfo))
                                            worksheet.Cell(dataRow, initColumn + 1).Style.DateFormat.Format = cellFormatInfo;

                                        worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Text;
                                        break;
                                    case ExportDataTypeEnum.NumberText:
                                        if (!string.IsNullOrEmpty(cellFormatInfo))
                                            worksheet.Cell(dataRow, initColumn + 1).Style.NumberFormat.Format = cellFormatInfo;

                                        worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Text;
                                        break;
                                    default:
                                        break;
                                }


                                worksheet.Cell(dataRow, initColumn + 1).Value = info.GetValue(item, null);
                                initColumn++;
                            }
                        }

                        dataRow++;
                    }

                    worksheet.Columns().AdjustToContents(1.0, 50.0);
                    worksheet.RangeUsed().SetAutoFilter();

                }

                var lastCellAddress = worksheet.RangeUsed().LastCell().Address;
                File.WriteAllLines("output.csv", worksheet.Rows(1, lastCellAddress.RowNumber)
                    .Select(r => string.Join(",", r.Cells(1, lastCellAddress.ColumnNumber)
                            .Select(cell =>
                            {
                                var cellValue = cell.GetValue<string>();
                                return cellValue.Contains(",") ? $"\"{cellValue}\"" : cellValue;
                            }))));
            }


        }
        public ExportResult GetExcelFromCSV<T>(ExportSheet sheet, string fileName, CustomGridRender<T> dataRender)
        {
            using XLWorkbook wbk = new XLWorkbook();
            using MemoryStream streamFile = new MemoryStream();
            //Worksheet

            const int headerRow = 1;
            int dataRow = 2;
            IXLWorksheet worksheet;
            if (string.IsNullOrEmpty(sheet.TabName))
            {
                worksheet = wbk.Worksheets.Add("Export");
                sheet.TabName = string.Empty;
            }
            else
                worksheet = wbk.Worksheets.Add(sheet.TabName);

            if (sheet.Data.Count > 0 && dataRender.Render.Any(x => x.Show))
            {
                //Header
                var typeOfData = sheet.Data[0].GetType();
                var headingsBase = typeOfData.BaseType?.GetProperties();
                var headingsInherited = typeOfData.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                var headings = headingsBase.Concat(headingsInherited);
                var ignoreColumn = false;
                var exportableColumn = false;
                var nonExportableColumn = false;

                var initHeaderColumn = 0;

                var render = dataRender.Render.Where(x => x.Show && x.Tab == sheet.TabName).OrderBy(x => x.Order).ToList();

                //for (int i = 0; i < render.Count(); i++)
                //{
                //    var data = render[i];
                //    data.Order = i + 1;
                //}


                foreach (PropertyInfo headerInfo in headings)
                {
                    var name = FirstToLower(headerInfo.Name);
                    var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name && y.Tab == sheet.TabName).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                    ignoreColumn = Attribute.IsDefined(headerInfo, typeof(IgnoreGridAttribute));
                    exportableColumn = Attribute.IsDefined(headerInfo, typeof(ExportableAttribute));
                    nonExportableColumn = Attribute.IsDefined(headerInfo, typeof(NonExportableAttribute));
                    if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                    {
                        //Format the header
                        var headerColumnName = headerInfo.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Name : headerInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                        var headerBackgroundColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                        var headerFontColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                        worksheet.Cell(headerRow, headerCustomProperties.Order).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                        worksheet.Cell(headerRow, headerCustomProperties.Order).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                        worksheet.Cell(headerRow, headerCustomProperties.Order).Value = headerColumnName;
                        initHeaderColumn++;
                    }
                    else if (exportableColumn && headerCustomProperties != null)
                    {
                        var headerColumnName = headerInfo.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Name : headerInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                        var headerBackgroundColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                        var headerFontColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                        worksheet.Cell(headerRow, headerCustomProperties.Order).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                        worksheet.Cell(headerRow, headerCustomProperties.Order).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                        worksheet.Cell(headerRow, headerCustomProperties.Order).Value = headerColumnName;
                        initHeaderColumn++;
                    }
                }
                //Rows
                foreach (object item in sheet.Data)
                {
                    var initColumn = 0;
                    var propertyInfoBase = item.GetType().BaseType.GetProperties();
                    var propertyInfoInherited = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    var propertyInfo = propertyInfoBase.Concat(propertyInfoInherited);

                    foreach (PropertyInfo info in propertyInfo)
                    {
                        var name = FirstToLower(info.Name);
                        var propertyCustomProperties = dataRender.Render.Where(y => y.PropertyName == name && y.Tab == sheet.TabName).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                        ignoreColumn = Attribute.IsDefined(info, typeof(IgnoreGridAttribute));
                        exportableColumn = Attribute.IsDefined(info, typeof(ExportableAttribute));
                        nonExportableColumn = Attribute.IsDefined(info, typeof(NonExportableAttribute));
                        if (!ignoreColumn && !nonExportableColumn && propertyCustomProperties != null && propertyCustomProperties.Show)
                        {
                            //Format the cell
                            var cellBackgroundColor = info.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                            var cellFontColor = info.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.GetCustomAttribute<CellColorAttribute>().FontColor;
                            var cellFormatTypeInfo = info.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.GetCustomAttribute<FormatAttribute>().FormatType;
                            var cellFormatInfo = info.GetCustomAttribute<FormatAttribute>()?.Format;
                            var cellFormatClosetXmlInfo = info.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                            if (cellBackgroundColor != 0)
                                worksheet.Cell(dataRow, propertyCustomProperties.Order).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                            worksheet.Cell(dataRow, propertyCustomProperties.Order).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                            worksheet.Cell(dataRow, propertyCustomProperties.Order).Style.Border.SetOutsideBorderColor(XLColor.Black);


                            if (cellFormatClosetXmlInfo == null)
                            {
                                ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                switch (formatType)
                                {
                                    case ExportDataTypeEnum.Text:
                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = XLDataType.Text;

                                        break;
                                    case ExportDataTypeEnum.Date:
                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = XLDataType.DateTime;

                                        break;
                                    case ExportDataTypeEnum.Number:
                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = XLDataType.Number;

                                        break;
                                    case ExportDataTypeEnum.Boolean:
                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = XLDataType.Boolean;
                                        break;
                                    case ExportDataTypeEnum.DateText:
                                        if (!string.IsNullOrEmpty(cellFormatInfo))
                                            worksheet.Cell(dataRow, propertyCustomProperties.Order).Style.DateFormat.Format = cellFormatInfo;

                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = XLDataType.Text;
                                        break;
                                    case ExportDataTypeEnum.NumberText:
                                        if (!string.IsNullOrEmpty(cellFormatInfo))
                                            worksheet.Cell(dataRow, propertyCustomProperties.Order).Style.NumberFormat.Format = cellFormatInfo;

                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = XLDataType.Text;
                                        break;
                                    default:
                                        break;
                                        //  }

                                }
                            }

                            // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                            if (info.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.PropertyType))
                            {
                                bool isDict = info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                if (isDict)
                                {
                                    var testo = "";
                                    dynamic data = info.GetValue(item, null);
                                    if (data != null && data.Values != null)
                                        foreach (var x in data.Values)
                                        {
                                            var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                            removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                            removedHtml = removedHtml.Replace("</b>", "");



                                            testo += removedHtml + " \n";
                                        }
                                    worksheet.Cell(dataRow, propertyCustomProperties.Order).Value = testo;
                                }
                            }
                            else
                            {


                                if (info.PropertyType == typeof(bool))
                                {
                                    worksheet.Cell(dataRow, propertyCustomProperties.Order).Value = (info == null || (bool)info.GetValue(item, null)) ? "Yes" : "No";
                                }
                                else if (info.PropertyType == typeof(Nullable<bool>))
                                {
                                    worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType =
                                        XLDataType.Text;

                                    var value = info.GetValue(item, null)?.ToString();


                                    worksheet.Cell(dataRow, propertyCustomProperties.Order).Value =
                                        value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                }
                                else
                                {

                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                    if (formatType == ExportDataTypeEnum.Text || info.PropertyType == typeof(string))
                                    {
                                        var text = info.GetValue(item, null);
                                        if (text != null)
                                        {
                                            var removedHtml = text?.ToString();


                                            while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                            {
                                                removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml?.Replace("</b>", "");
                                            }

                                            worksheet.Cell(dataRow, propertyCustomProperties.Order).SetValue<string>(Convert.ToString(removedHtml));
                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, propertyCustomProperties.Order).Value = info.GetValue(item, null);

                                        }


                                    }
                                    else
                                    {
                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).Value = info.GetValue(item, null);

                                    }
                                }
                            }
                            if (cellFormatClosetXmlInfo != null)
                            {
                                worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = cellFormatClosetXmlInfo.Value;
                            }


                            initColumn++;
                        }
                        else if (exportableColumn && propertyCustomProperties != null)
                        {
                            //Format the cell
                            var cellBackgroundColor = info.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                            var cellFontColor = info.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.GetCustomAttribute<CellColorAttribute>().FontColor;
                            var cellFormatTypeInfo = info.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.GetCustomAttribute<FormatAttribute>().FormatType;
                            var cellFormatInfo = info.GetCustomAttribute<FormatAttribute>()?.Format;
                            var cellFormatClosetXmlInfo = info.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                            if (cellBackgroundColor != 0)
                                worksheet.Cell(dataRow, propertyCustomProperties.Order).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                            worksheet.Cell(dataRow, propertyCustomProperties.Order).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                            worksheet.Cell(dataRow, propertyCustomProperties.Order).Style.Border.SetOutsideBorderColor(XLColor.Black);


                            if (cellFormatClosetXmlInfo == null)
                            {
                                ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                switch (formatType)
                                {
                                    case ExportDataTypeEnum.Text:
                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = XLDataType.Text;

                                        break;
                                    case ExportDataTypeEnum.Date:
                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = XLDataType.DateTime;

                                        break;
                                    case ExportDataTypeEnum.Number:
                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = XLDataType.Number;

                                        break;
                                    case ExportDataTypeEnum.Boolean:
                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = XLDataType.Boolean;
                                        break;
                                    case ExportDataTypeEnum.DateText:
                                        if (!string.IsNullOrEmpty(cellFormatInfo))
                                            worksheet.Cell(dataRow, propertyCustomProperties.Order).Style.DateFormat.Format = cellFormatInfo;

                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = XLDataType.Text;
                                        break;
                                    case ExportDataTypeEnum.NumberText:
                                        if (!string.IsNullOrEmpty(cellFormatInfo))
                                            worksheet.Cell(dataRow, propertyCustomProperties.Order).Style.NumberFormat.Format = cellFormatInfo;

                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = XLDataType.Text;
                                        break;
                                    default:
                                        break;
                                        //  }

                                }
                            }

                            // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                            if (info.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.PropertyType))
                            {
                                bool isDict = info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                if (isDict)
                                {
                                    var testo = "";
                                    dynamic data = info.GetValue(item, null);
                                    if (data != null && data.Values != null)
                                        foreach (var x in data.Values)
                                        {
                                            var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                            removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                            removedHtml = removedHtml.Replace("</b>", "");



                                            testo += removedHtml + " \n";
                                        }
                                    worksheet.Cell(dataRow, propertyCustomProperties.Order).Value = testo;
                                }
                            }
                            else
                            {


                                if (info.PropertyType == typeof(bool))
                                {
                                    worksheet.Cell(dataRow, propertyCustomProperties.Order).Value = (info == null || (bool)info.GetValue(item, null)) ? "Yes" : "No";
                                }
                                else if (info.PropertyType == typeof(Nullable<bool>))
                                {
                                    worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType =
                                        XLDataType.Text;

                                    var value = info.GetValue(item, null)?.ToString();


                                    worksheet.Cell(dataRow, propertyCustomProperties.Order).Value =
                                        value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                }
                                else
                                {

                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                    if (formatType == ExportDataTypeEnum.Text || info.PropertyType == typeof(string))
                                    {
                                        var text = info.GetValue(item, null);
                                        if (text != null)
                                        {
                                            var removedHtml = text?.ToString();


                                            while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                            {
                                                removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml?.Replace("</b>", "");
                                            }

                                            worksheet.Cell(dataRow, propertyCustomProperties.Order).SetValue<string>(Convert.ToString(removedHtml));
                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, propertyCustomProperties.Order).Value = info.GetValue(item, null);

                                        }


                                    }
                                    else
                                    {
                                        worksheet.Cell(dataRow, propertyCustomProperties.Order).Value = info.GetValue(item, null);

                                    }
                                }
                            }
                            if (cellFormatClosetXmlInfo != null)
                            {
                                worksheet.Cell(dataRow, propertyCustomProperties.Order).DataType = cellFormatClosetXmlInfo.Value;
                            }


                            initColumn++;
                        }
                    }

                    dataRow++;
                }

                worksheet.Columns().AdjustToContents(1.0, 50.0);
                worksheet.RangeUsed().SetAutoFilter();

            }


            var lastCellAddress = worksheet.RangeUsed().LastCell().Address;
            var result = worksheet.Rows(1, lastCellAddress.RowNumber)
                .Select(r => string.Join(",", r.Cells(1, lastCellAddress.ColumnNumber)
                        .Select(cell =>
                        {
                            var cellValue = cell.GetValue<string>();
                            return cellValue.Contains(",") ? $"\"{cellValue}\"" : cellValue;
                        }))).ToList();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var entity in result)
            {
                stringBuilder.Append(entity + Environment.NewLine);
            }


            //File.WriteAllLines("output.csv", result);
            //string combinedString = string.Join("</n>?", result.ToArray());
            //var myByte = Encoding.ASCII.GetBytes(combinedString);
            //MemoryStream theMemStream = new MemoryStream();

            //theMemStream.Write(myByte, 0, myByte.Length);

            //var content = theMemStream.ToArray();
            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = null,
                FileName = fileName,
                Stringbuilder = stringBuilder
            };


        }
        public ExportResult GetExcelFromCSV<T>(List<ExportSheet> sheets, string fileName, CustomGridRender<T> dataRender)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using XLWorkbook wbk = new XLWorkbook();
            using MemoryStream streamFile = new MemoryStream();
            //Worksheet
            foreach (ExportSheet sheet in sheets)
            {

                const int headerRow = 1;
                int dataRow = 2;
                IXLWorksheet worksheet;
                if (string.IsNullOrEmpty(sheet.TabName))
                {
                    worksheet = wbk.Worksheets.Add("Export");
                    sheet.TabName = string.Empty;
                }
                else
                    worksheet = wbk.Worksheets.Add(sheet.TabName);

                if (sheet.Data.Count > 0 && dataRender.Render.Any(x => x.Show))
                {
                    //Header
                    var typeOfData = sheet.Data[0].GetType();
                    var headingsBase = typeOfData.BaseType?.GetProperties();
                    var headingsInherited = typeOfData.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    var headings = headingsBase.Concat(headingsInherited);
                    var ignoreColumn = false;
                    var exportableColumn = false;
                    var nonExportableColumn = false;

                    var initHeaderColumn = 0;

                    var render = dataRender.Render.Where(x => x.Show).OrderBy(x => x.Order).ToList();

                    foreach (PropertyInfo headerInfo in headings)
                    {
                        var name = FirstToLower(headerInfo.Name);
                        var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                        ignoreColumn = Attribute.IsDefined(headerInfo, typeof(IgnoreGridAttribute));
                        exportableColumn = Attribute.IsDefined(headerInfo, typeof(ExportableAttribute));
                        nonExportableColumn = Attribute.IsDefined(headerInfo, typeof(NonExportableAttribute));
                        if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                        {
                            //Format the header
                            var headerColumnName = headerInfo.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Name : headerInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            var headerBackgroundColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerFontColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            worksheet.Cell(headerRow, initHeaderColumn + 1).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                            worksheet.Cell(headerRow, initHeaderColumn + 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                            worksheet.Cell(headerRow, initHeaderColumn + 1).Value = headerColumnName;
                            initHeaderColumn++;
                        }
                        else if (exportableColumn && headerCustomProperties != null)
                        {
                            var headerColumnName = headerInfo.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Name : headerInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            var headerBackgroundColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerFontColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            worksheet.Cell(headerRow, initHeaderColumn + 1).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                            worksheet.Cell(headerRow, initHeaderColumn + 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                            worksheet.Cell(headerRow, initHeaderColumn + 1).Value = headerColumnName;
                            initHeaderColumn++;
                        }
                    }
                    //Rows
                    foreach (object item in sheet.Data)
                    {
                        var initColumn = 0;
                        var propertyInfoBase = item.GetType().BaseType.GetProperties();
                        var propertyInfoInherited = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        var propertyInfo = propertyInfoBase.Concat(propertyInfoInherited);

                        foreach (PropertyInfo info in propertyInfo)
                        {
                            var name = FirstToLower(info.Name);
                            var propertyCustomProperties = dataRender.Render.Where(y => y.PropertyName == name).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                            ignoreColumn = Attribute.IsDefined(info, typeof(IgnoreGridAttribute));
                            exportableColumn = Attribute.IsDefined(info, typeof(ExportableAttribute));
                            nonExportableColumn = Attribute.IsDefined(info, typeof(NonExportableAttribute));
                            if (!ignoreColumn && !nonExportableColumn && propertyCustomProperties != null && propertyCustomProperties.Show)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, initColumn + 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, initColumn + 1).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, initColumn + 1).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Text;

                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, initColumn + 1).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, initColumn + 1).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;
                                            //  }

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.PropertyType))
                                {
                                    bool isDict = info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, initColumn + 1).Value = testo;
                                    }
                                }
                                else
                                {


                                    if (info.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, initColumn + 1).Value = (info == null || (bool)info.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, initColumn + 1).DataType =
                                            XLDataType.Text;

                                        var value = info.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, initColumn + 1).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.PropertyType == typeof(string))
                                        {
                                            var text = info.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }

                                                worksheet.Cell(dataRow, initColumn + 1).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, initColumn + 1).Value = info.GetValue(item, null);

                                            }


                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, initColumn + 1).Value = info.GetValue(item, null);

                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, initColumn + 1).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }
                            else if (exportableColumn && propertyCustomProperties != null)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, initColumn + 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, initColumn + 1).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, initColumn + 1).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Text;

                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, initColumn + 1).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, initColumn + 1).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;
                                            //  }

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.PropertyType))
                                {
                                    bool isDict = info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, initColumn + 1).Value = testo;
                                    }
                                }
                                else
                                {


                                    if (info.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, initColumn + 1).Value = (info == null || (bool)info.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, initColumn + 1).DataType =
                                            XLDataType.Text;

                                        var value = info.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, initColumn + 1).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.PropertyType == typeof(string))
                                        {
                                            var text = info.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }

                                                worksheet.Cell(dataRow, initColumn + 1).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, initColumn + 1).Value = info.GetValue(item, null);

                                            }


                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, initColumn + 1).Value = info.GetValue(item, null);

                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, initColumn + 1).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }
                        }

                        dataRow++;
                    }

                    worksheet.Columns().AdjustToContents(1.0, 50.0);
                    worksheet.RangeUsed().SetAutoFilter();

                }


                var lastCellAddress = worksheet.RangeUsed().LastCell().Address;
                var result = worksheet.Rows(1, lastCellAddress.RowNumber)
                    .Select(r => string.Join(",", r.Cells(1, lastCellAddress.ColumnNumber)
                            .Select(cell =>
                            {
                                var cellValue = cell.GetValue<string>();
                                return cellValue.Contains(",") ? $"\"{cellValue}\"" : cellValue;
                            }))).ToList();

                foreach (var entity in result)
                {
                    stringBuilder.Append(entity + Environment.NewLine);
                }
            }

            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = null,
                FileName = fileName,
                Stringbuilder = stringBuilder
            };


        }

        public ExportResult GetExcelFromCSV<T>(List<ExportSheet> sheets, string fileName, GenericReportInfrastructureGrid<T> dataRender)
        {
            using XLWorkbook wbk = new XLWorkbook();
            using MemoryStream streamFile = new MemoryStream();
            StringBuilder stringBuilder = new StringBuilder();
            Dictionary<string, int> colorDictionary = new Dictionary<string, int>
            {
                { "blue", 0x2F75B5 },
                { "red", 0xe60000 },
                { "green", 0x00c300 },
                { "gray", 0xcccccc },
                { "black", 0x0F0D0D },
                { "white", 0xcccccc },
            };
            //Worksheet
            foreach (ExportSheet sheet in sheets)
            {
                const int headerRow = 1;
                int dataRow = 2;
                IXLWorksheet worksheet;
                if (string.IsNullOrEmpty(sheet.TabName))
                {
                    worksheet = wbk.Worksheets.Add("Export");
                    sheet.TabName = string.Empty;
                }
                else
                    worksheet = wbk.Worksheets.Add(sheet.TabName);

                if (sheet.Data.Count > 0 && dataRender.Render.Any(x => x.Show))
                {
                    //Header
                    var typeOfData = sheet.Data[0].GetType();
                    var headingsBase = typeOfData.BaseType?.GetProperties();
                    var headingsInherited = typeOfData.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    var headings = headingsBase.Concat(headingsInherited);
                    var ignoreColumn = false;
                    var exportableColumn = false;
                    var nonExportableColumn = false;

                    var initHeaderColumn = 0;

                    var render = dataRender.Render.Where(x => x.Show).OrderBy(x => x.Order).ToList();

                    Dictionary<int, PropertyInfo> propertyInfos = new Dictionary<int, PropertyInfo>();
                    Dictionary<int, PropertyInfo> _copypropertyInfos = new Dictionary<int, PropertyInfo>();

                    foreach (PropertyInfo headerInfo in headings)
                    {
                        var name = FirstToLower(headerInfo.Name);
                        var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name).Select(x => new { x.Show, x.Order, x.ColorHeader, x.UpdatedPropertyName }).SingleOrDefault();
                        ignoreColumn = Attribute.IsDefined(headerInfo, typeof(IgnoreGridAttribute));
                        exportableColumn = Attribute.IsDefined(headerInfo, typeof(ExportableAttribute));
                        nonExportableColumn = Attribute.IsDefined(headerInfo, typeof(NonExportableAttribute));

                        if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                        {
                            if (!propertyInfos.ContainsKey((int)headerCustomProperties.Order))
                                propertyInfos.Add((int)headerCustomProperties.Order, headerInfo);

                        }
                        else if (exportableColumn && headerCustomProperties != null)
                        {
                            if (!propertyInfos.ContainsKey((int)headerCustomProperties.Order))
                                propertyInfos.Add((int)headerCustomProperties.Order, headerInfo);
                        }
                    }

                    propertyInfos = propertyInfos.OrderBy(p => p.Key).ToDictionary(p => p.Key, k => k.Value);
                    int count = 1;

                    var emptyTableList = render.Where(x => x.TableName == "emptyGrid").OrderBy(x => x.Order).ToList();

                    foreach (var item in propertyInfos)
                    {
                        count = count + item.Key;
                        if (!_copypropertyInfos.ContainsKey(count))
                            _copypropertyInfos.Add(count, item.Value);
                        count = 1;
                    }
                    count = 1;
                    foreach (var item in emptyTableList)
                    {
                        count = count + Convert.ToInt16(item.Order);
                        if (!_copypropertyInfos.ContainsKey(count))
                            _copypropertyInfos.Add(count, null);
                        count = 1;
                    }
                    _copypropertyInfos = _copypropertyInfos.OrderBy(x => x.Key)
                        .ToDictionary(x => x.Key, x => x.Value);

                    foreach (var headerInfo in _copypropertyInfos)
                    {
                        if (headerInfo.Value == null)
                        {
                            // Empty Table Row
                            int keyTableCount = headerInfo.Key - 1;
                            var currentEmptyTableObject = emptyTableList.Where(x => x.Order == keyTableCount).FirstOrDefault();
                            if (currentEmptyTableObject.Show)
                            {
                                var headerColumnName = ((currentEmptyTableObject.UpdatedPropertyName != null)) ?
                                   FirstToUpper(currentEmptyTableObject.UpdatedPropertyName)
                                    : "";

                                //  currentEmptyTableObject.TableName  // ---**** No need to display Empty column
                                // Show only Custom Name;
                                var headerBackgroundColor =
                                    XLColor.FromArgb(colorDictionary.ContainsKey(currentEmptyTableObject.ColorHeader.ToLower()) ?
                                    colorDictionary[currentEmptyTableObject.ColorHeader.ToLower()] : 0xcccccc);


                                var headerFontColor = ((headerBackgroundColor == XLColor.FromArgb(0xffffff)) ||
                                    (headerBackgroundColor == XLColor.FromArgb(0xcccccc)))
                                       ? 0x0F0D0D : 0xffffff;

                                worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                                worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(headerBackgroundColor);
                                worksheet.Cell(headerRow, headerInfo.Key).Value = headerColumnName;
                                initHeaderColumn++;
                            }
                        }
                        else
                        {
                            //Format the header
                            // var headerColumnName = headerInfo.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Name : headerInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            // var headerBackgroundColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            // var headerFontColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            var name = FirstToLower(headerInfo.Value.Name);
                            var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name).Select(x => new { x.Show, x.Order, x.ColorHeader, x.UpdatedPropertyName }).SingleOrDefault();
                            ignoreColumn = Attribute.IsDefined(headerInfo.Value, typeof(IgnoreGridAttribute));
                            exportableColumn = Attribute.IsDefined(headerInfo.Value, typeof(ExportableAttribute));
                            nonExportableColumn = Attribute.IsDefined(headerInfo.Value, typeof(NonExportableAttribute));
                            if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                            {
                                var headerColumnName = headerCustomProperties.UpdatedPropertyName == null ?
                                 headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName : headerCustomProperties.UpdatedPropertyName;

                                var headerBackgroundColor = XLColor.FromArgb(colorDictionary.ContainsKey(headerCustomProperties.ColorHeader.ToLower()) ?
                                    colorDictionary[headerCustomProperties.ColorHeader.ToLower()] : 0xcccccc);


                                var headerFontColor = ((headerBackgroundColor == XLColor.FromArgb(0xffffff)) ||
                                  (headerBackgroundColor == XLColor.FromArgb(0xcccccc)))
                                     ? (headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>()?.FontColor ?? 0x0F0D0D) :
                                     (headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>()?.FontColor ?? 0xffffff);

                                worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                                worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(headerBackgroundColor);
                                worksheet.Cell(headerRow, headerInfo.Key).Value = headerColumnName;
                                initHeaderColumn++;
                            }

                            else if (exportableColumn && headerCustomProperties != null)
                            {
                                //var headerColumnName = headerInfo.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Name : headerInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                                //var headerBackgroundColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                                //var headerFontColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.GetCustomAttribute<HeaderColorAttribute>().FontColor;

                                var headerColumnName = headerCustomProperties.UpdatedPropertyName == null ?
                                   headerInfo.Value.Name : headerCustomProperties.UpdatedPropertyName;

                                var headerBackgroundColor = XLColor.FromArgb(colorDictionary.ContainsKey(headerCustomProperties.ColorHeader.ToLower()) ?
                                    colorDictionary[headerCustomProperties.ColorHeader.ToLower()] : 0xcccccc);


                                var headerFontColor = ((headerBackgroundColor == XLColor.FromArgb(0xffffff)) ||
                                   (headerBackgroundColor == XLColor.FromArgb(0xcccccc)))
                                      ? (headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>()?.FontColor ?? 0x0F0D0D) :
                                      (headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>()?.FontColor ?? 0xffffff);

                                worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                                worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(headerBackgroundColor);
                                worksheet.Cell(headerRow, headerInfo.Key).Value = headerColumnName;
                                initHeaderColumn++;
                            }
                        }

                    }
                    //Rows
                    foreach (object item in sheet.Data)
                    {
                        var initColumn = 0;
                        var propertyInfoBase = item.GetType().BaseType.GetProperties();
                        var propertyInfoInherited = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        var propertyInfo = propertyInfoBase.Concat(propertyInfoInherited);


                        foreach (var info in _copypropertyInfos)
                        {
                            if (info.Value == null) continue; // Empty Column Row
                            var name = FirstToLower(info.Value.Name);
                            var propertyCustomProperties = dataRender.Render.Where(y => y.PropertyName == name).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                            ignoreColumn = Attribute.IsDefined(info.Value, typeof(IgnoreGridAttribute));
                            exportableColumn = Attribute.IsDefined(info.Value, typeof(ExportableAttribute));
                            nonExportableColumn = Attribute.IsDefined(info.Value, typeof(NonExportableAttribute));
                            if (!ignoreColumn && !nonExportableColumn && propertyCustomProperties != null && propertyCustomProperties.Show)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.Value.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.Value.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.Value.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.Value.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.Value.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.Value.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, info.Key).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, info.Key).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;

                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;
                                            //  }

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.Value.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.Value.PropertyType))
                                {
                                    bool isDict = info.Value.PropertyType.IsGenericType && info.Value.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.Value.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, info.Key).Value = testo;
                                    }
                                }
                                else
                                {


                                    if (info.Value.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, info.Key).Value = (info.Value == null || (bool)info.Value.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.Value.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, info.Key).DataType =
                                            XLDataType.Text;

                                        var value = info.Value.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, info.Key).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.Value.PropertyType == typeof(string))
                                        {
                                            var text = info.Value.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }

                                                worksheet.Cell(dataRow, info.Key).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                            }


                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, info.Key).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }
                            else if (exportableColumn && propertyCustomProperties != null)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.Value.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.Value.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.Value.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.Value.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.Value.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.Value.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, info.Key).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, info.Key).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;

                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;
                                            //  }

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.Value.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.Value.PropertyType))
                                {
                                    bool isDict = info.Value.PropertyType.IsGenericType && info.Value.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.Value.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, info.Key).Value = testo;
                                    }
                                }
                                else
                                {


                                    if (info.Value.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, info.Key).Value = (info.Value == null || (bool)info.Value.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.Value.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, info.Key).DataType =
                                            XLDataType.Text;

                                        var value = info.Value.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, info.Key).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.Value.PropertyType == typeof(string))
                                        {
                                            var text = info.Value.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }

                                                worksheet.Cell(dataRow, info.Key).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                            }


                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, info.Key).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }
                        }

                        dataRow++;
                    }

                    worksheet.Columns().AdjustToContents(1.0, 50.0);
                    worksheet.RangeUsed().SetAutoFilter();

                }



                var lastCellAddress = worksheet.RangeUsed().LastCell().Address;
                var result = worksheet.Rows(1, lastCellAddress.RowNumber)
                    .Select(r => string.Join(",", r.Cells(1, lastCellAddress.ColumnNumber)
                            .Select(cell =>
                            {
                                var cellValue = cell.GetValue<string>();
                                return cellValue.Contains(",") ? $"\"{cellValue}\"" : cellValue;
                            }))).ToList();

                //StringBuilder stringBuilder = new StringBuilder();
                foreach (var entity in result)
                {
                    stringBuilder.Append(entity + Environment.NewLine);
                }
            }

            //File.WriteAllLines("output.csv", result);
            //string combinedString = string.Join("</n>?", result.ToArray());
            //var myByte = Encoding.ASCII.GetBytes(combinedString);
            //MemoryStream theMemStream = new MemoryStream();

            //theMemStream.Write(myByte, 0, myByte.Length);

            //var content = theMemStream.ToArray();
            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = null,
                FileName = fileName,
                Stringbuilder = stringBuilder
            };



        }

        public ExportResult GetExcelFrom(List<ExportSheet> sheets, string fileName)
        {
            using XLWorkbook wbk = new XLWorkbook();
            using MemoryStream streamFile = new MemoryStream();

            //Worksheet
            foreach (ExportSheet sheet in sheets)
            {
                const int headerRow = 1;
                int dataRow = 2;
                var worksheet = wbk.Worksheets.Add(sheet.TabName);

                if (sheet.Data.Count > 0)
                {
                    //Header
                    var typeOfData = sheet.Data[0].GetType();
                    var headingsBase = typeOfData.BaseType?.GetProperties();
                    var headingsInherited = typeOfData.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    var headings = headingsBase.Concat(headingsInherited);
                    var ignoreColumn = false;

                    var initHeaderColumn = 0;





                    foreach (PropertyInfo headerInfo in headings)
                    {

                        ignoreColumn = headerInfo.GetCustomAttribute<IgnoreAttribute>() == null ? false : headerInfo.GetCustomAttribute<IgnoreAttribute>().Ignore;

                        if (!ignoreColumn)
                        {
                            //Format the header
                            var headerColumnName = headerInfo.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Name : headerInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            var headerBackgroundColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerFontColor = headerInfo.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            worksheet.Cell(headerRow, initHeaderColumn + 1).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));

                            worksheet.Cell(headerRow, initHeaderColumn + 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                            worksheet.Cell(headerRow, initHeaderColumn + 1).Value = headerColumnName;

                            initHeaderColumn++;
                        }
                    }


                    //Rows
                    foreach (object item in sheet.Data)
                    {
                        var initColumn = 0;

                        var propertyInfoBase = item.GetType().BaseType.GetProperties();
                        var propertyInfoInherited = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        var propertyInfo = propertyInfoBase.Concat(propertyInfoInherited);

                        foreach (PropertyInfo info in propertyInfo)
                        {
                            ignoreColumn = info.GetCustomAttribute<IgnoreAttribute>() == null ? false : info.GetCustomAttribute<IgnoreAttribute>().Ignore;

                            if (!ignoreColumn)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.GetCustomAttribute<FormatAttribute>()?.Format;

                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, initColumn + 1).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, initColumn + 1).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, initColumn + 1).Style.Border.SetOutsideBorderColor(XLColor.Black);

                                ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                switch (formatType)
                                {
                                    case ExportDataTypeEnum.Text:
                                        worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Text;

                                        break;
                                    case ExportDataTypeEnum.Date:
                                        worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.DateTime;

                                        break;
                                    case ExportDataTypeEnum.Number:
                                        worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Number;

                                        break;
                                    case ExportDataTypeEnum.Boolean:
                                        worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Boolean;
                                        break;
                                    case ExportDataTypeEnum.DateText:
                                        if (!string.IsNullOrEmpty(cellFormatInfo))
                                            worksheet.Cell(dataRow, initColumn + 1).Style.DateFormat.Format = cellFormatInfo;

                                        worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Text;
                                        break;
                                    case ExportDataTypeEnum.NumberText:
                                        if (!string.IsNullOrEmpty(cellFormatInfo))
                                            worksheet.Cell(dataRow, initColumn + 1).Style.NumberFormat.Format = cellFormatInfo;

                                        worksheet.Cell(dataRow, initColumn + 1).DataType = XLDataType.Text;
                                        break;
                                    default:
                                        break;
                                }


                                worksheet.Cell(dataRow, initColumn + 1).Value = info.GetValue(item, null);
                                initColumn++;
                            }
                        }

                        dataRow++;
                    }

                    worksheet.Columns().AdjustToContents(1.0, 50.0);
                    worksheet.RangeUsed().SetAutoFilter();

                }
            }
            wbk.SaveAs(streamFile);
            var content = streamFile.ToArray();
            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = content,
                FileName = fileName
            };

        }
        public ExportResult GetExcelFrom<T>(List<ExportSheet> sheets, string fileName, CustomGridRender<T> dataRender)
        {
            using XLWorkbook wbk = new XLWorkbook();
            using MemoryStream streamFile = new MemoryStream();
            //Worksheet

            foreach (ExportSheet sheet in sheets)
            {
                const int headerRow = 1;
                int dataRow = 2;
                IXLWorksheet worksheet;
                if (string.IsNullOrEmpty(sheet.TabName))
                {
                    worksheet = wbk.Worksheets.Add("Export");
                    sheet.TabName = string.Empty;
                }
                else
                    worksheet = wbk.Worksheets.Add(sheet.TabName);

                if (sheet.Data.Count > 0 && dataRender.Render.Any(x => x.Show))
                {
                    //Header
                    var typeOfData = sheet.Data[0].GetType();
                    var headingsBase = typeOfData.BaseType?.GetProperties();
                    var headingsInherited = typeOfData.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    var headings = headingsBase.Concat(headingsInherited);
                    var ignoreColumn = false;
                    var exportableColumn = false;
                    var nonExportableColumn = false;

                    var initHeaderColumn = 0;

                    var render = dataRender.Render.Where(x => x.Show && x.Tab == sheet.TabName).OrderBy(x => x.Order).ToList();

                    Dictionary<int, PropertyInfo> propertyInfos = new Dictionary<int, PropertyInfo>();
                    Dictionary<int, PropertyInfo> _copypropertyInfos = new Dictionary<int, PropertyInfo>();
                    foreach (PropertyInfo headerInfo in headings)
                    {
                        var name = FirstToLower(headerInfo.Name);
                        var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name && y.Tab == sheet.TabName).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                        ignoreColumn = Attribute.IsDefined(headerInfo, typeof(IgnoreGridAttribute));
                        exportableColumn = Attribute.IsDefined(headerInfo, typeof(ExportableAttribute));
                        nonExportableColumn = Attribute.IsDefined(headerInfo, typeof(NonExportableAttribute));
                        if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                        {
                            if (!propertyInfos.ContainsKey(headerCustomProperties.Order))
                                propertyInfos.Add(headerCustomProperties.Order, headerInfo);

                        }
                        else if (exportableColumn && headerCustomProperties != null)
                        {
                            if (!propertyInfos.ContainsKey(headerCustomProperties.Order))
                                propertyInfos.Add(headerCustomProperties.Order, headerInfo);
                        }
                    }
                    propertyInfos = propertyInfos.OrderBy(p => p.Key).ToDictionary(p => p.Key, k => k.Value);
                    int count = 1;
                    foreach (var item in propertyInfos)
                    {
                        if (!_copypropertyInfos.ContainsKey(count))
                            _copypropertyInfos.Add(count, item.Value);
                        count++;
                    }
                    foreach (var headerInfo in _copypropertyInfos)
                    {
                        var name = FirstToLower(headerInfo.Value.Name);
                        var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name && y.Tab == sheet.TabName).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                        ignoreColumn = Attribute.IsDefined(headerInfo.Value, typeof(IgnoreGridAttribute));
                        exportableColumn = Attribute.IsDefined(headerInfo.Value, typeof(ExportableAttribute));
                        nonExportableColumn = Attribute.IsDefined(headerInfo.Value, typeof(NonExportableAttribute));
                        if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                        {
                            //Format the header
                            var headerColumnName = headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Value.Name : headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            var headerBackgroundColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerFontColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Value = headerColumnName;
                            initHeaderColumn++;
                        }
                        else if (exportableColumn && headerCustomProperties != null)
                        {
                            var headerColumnName = headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Value.Name : headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            var headerBackgroundColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerFontColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Value = headerColumnName;
                            initHeaderColumn++;
                        }
                    }
                    //Rows
                    foreach (object item in sheet.Data)
                    {
                        var initColumn = 0;
                        var propertyInfoBase = item.GetType().BaseType.GetProperties();
                        var propertyInfoInherited = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        var propertyInfo = propertyInfoBase.Concat(propertyInfoInherited);

                        foreach (var info in _copypropertyInfos)
                        {
                            var name = FirstToLower(info.Value.Name);
                            var propertyCustomProperties = dataRender.Render.Where(y => y.PropertyName == name && y.Tab == sheet.TabName).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                            ignoreColumn = Attribute.IsDefined(info.Value, typeof(IgnoreGridAttribute));
                            exportableColumn = Attribute.IsDefined(info.Value, typeof(ExportableAttribute));
                            nonExportableColumn = Attribute.IsDefined(info.Value, typeof(NonExportableAttribute));
                            if (!ignoreColumn && !nonExportableColumn && propertyCustomProperties != null && propertyCustomProperties.Show)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.Value.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.Value.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.Value.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.Value.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.Value.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.Value.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, info.Key).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, info.Key).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            worksheet.Cell(dataRow, info.Key).Style.Alignment.SetWrapText(true);
                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;
                                            //  }

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.Value.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.Value.PropertyType))
                                {
                                    bool isDict = info.Value.PropertyType.IsGenericType && info.Value.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.Value.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, info.Key).Value = testo;
                                    }
                                }
                                else
                                {


                                    if (info.Value.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, info.Key).Value = (info.Value == null || (bool)info.Value.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.Value.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, info.Key).DataType =
                                            XLDataType.Text;

                                        var value = info.Value.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, info.Key).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.Value.PropertyType == typeof(string))
                                        {
                                            var text = info.Value.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }
                                                worksheet.Cell(dataRow, info.Key).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);
                                            }
                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);
                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, info.Key).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }
                            else if (exportableColumn && propertyCustomProperties != null)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.Value.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.Value.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.Value.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.Value.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.Value.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.Value.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, info.Key).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, info.Key).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;

                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.Value.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.Value.PropertyType))
                                {
                                    bool isDict = info.Value.PropertyType.IsGenericType && info.Value.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.Value.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, info.Key).Value = testo;
                                    }
                                }
                                else
                                {
                                    if (info.Value.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, info.Key).Value = (info.Value == null || (bool)info.Value.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.Value.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, info.Key).DataType =
                                            XLDataType.Text;

                                        var value = info.Value.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, info.Key).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.Value.PropertyType == typeof(string))
                                        {
                                            var text = info.Value.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }

                                                worksheet.Cell(dataRow, info.Key).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                            }


                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, info.Key).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }
                        }

                        dataRow++;
                    }

                    worksheet.Columns().AdjustToContents(1.0, 50.0);
                    worksheet.RangeUsed().SetAutoFilter();
                }
            }
            wbk.SaveAs(streamFile);
            var content = streamFile.ToArray();
            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = content,
                FileName = fileName
            };

        }


        public ExportResult GetExcelFrom<T>(List<ExportSheet> sheets, string fileName, CustomGridRender<T> dataRender, string IDColumn, List<string> HighlightColumns, IEnumerable<GlossaryItemsGridDto> tsrDescription = null)
        {
            using XLWorkbook wbk = new XLWorkbook();
            using MemoryStream streamFile = new MemoryStream();
            int columnNo = 1;
            List<int> EditableColumnNoList = new List<int>();
            //Worksheet


            foreach (ExportSheet sheet in sheets)
            {
                EditableColumnNoList = new List<int>();
                const int headerRow = 1;
                int dataRow = 2;
                columnNo = 1;

                IXLWorksheet worksheet;
                if (string.IsNullOrEmpty(sheet.TabName))
                {
                    worksheet = wbk.Worksheets.Add("Export");
                    sheet.TabName = string.Empty;
                }
                else
                    worksheet = wbk.Worksheets.Add(sheet.TabName);

                if (sheet.Data.Count > 0 || dataRender.Render.Any(x => x.Show))
                {
                    //Header
                    var typeOfData = sheet.Data.Count > 0 ? sheet.Data[0].GetType() : new TsrPassThroughDtoGrid().GetType();
                    var headingsBase = sheet.Data.Count > 0 ? typeOfData.BaseType.GetProperties() : typeOfData.GetProperties();
                    var headingsInherited = typeOfData.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    var headings = headingsBase.Concat(headingsInherited);
                    var ignoreColumn = false;
                    var exportableColumn = false;
                    var nonExportableColumn = false;
                    if (sheet.Data.Count == 0)
                    {
                        worksheet.Row(2).InsertRowsAbove(1);
                    }
                    var initHeaderColumn = 0;

                    var render = dataRender.Render.Where(x => x.Show).OrderBy(x => x.Order).ToList();

                    Dictionary<int, PropertyInfo> propertyInfos = new Dictionary<int, PropertyInfo>();
                    Dictionary<int, PropertyInfo> _copypropertyInfos = new Dictionary<int, PropertyInfo>();
                    foreach (PropertyInfo headerInfo in headings)
                    {
                        var name = FirstToLower(headerInfo.Name);
                        var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                        ignoreColumn = Attribute.IsDefined(headerInfo, typeof(IgnoreGridAttribute));
                        exportableColumn = Attribute.IsDefined(headerInfo, typeof(ExportableAttribute));
                        nonExportableColumn = Attribute.IsDefined(headerInfo, typeof(NonExportableAttribute));
                        if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                        {
                            if (!propertyInfos.ContainsKey(headerCustomProperties.Order))
                                propertyInfos.Add(headerCustomProperties.Order, headerInfo);

                        }
                        else if (exportableColumn && headerCustomProperties != null)
                        {
                            if (!propertyInfos.ContainsKey(headerCustomProperties.Order))
                                propertyInfos.Add(headerCustomProperties.Order, headerInfo);
                        }
                    }
                    propertyInfos = propertyInfos.OrderBy(p => p.Key).ToDictionary(p => p.Key, k => k.Value);
                    int count = 1;
                    foreach (var item in propertyInfos)
                    {
                        if (!_copypropertyInfos.ContainsKey(count))
                            _copypropertyInfos.Add(count, item.Value);
                        count++;
                    }
                    foreach (var headerInfo in _copypropertyInfos)
                    {
                        var name = FirstToLower(headerInfo.Value.Name);
                        var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                        ignoreColumn = Attribute.IsDefined(headerInfo.Value, typeof(IgnoreGridAttribute));
                        exportableColumn = Attribute.IsDefined(headerInfo.Value, typeof(ExportableAttribute));
                        nonExportableColumn = Attribute.IsDefined(headerInfo.Value, typeof(NonExportableAttribute));
                        if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                        {
                            //Format the header
                            var headerColumnName = headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Value.Name : headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            var headerBackgroundColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerIDColumnBackgroundColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0x00ff00 : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerEditColumnBackgroundColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xbfbfbf : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerEditColumnFontColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0x000000 : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            var headerFontColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                            columnNo++;
                            if (HighlightColumns.Any(x => x == headerColumnName))
                            {
                                worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerEditColumnBackgroundColor));
                                worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerEditColumnFontColor));
                                EditableColumnNoList.Add(columnNo);
                            }

                            else
                            {
                                worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                            }
                            worksheet.Cell(headerRow, headerInfo.Key).Value = headerColumnName;
                            initHeaderColumn++;
                        }
                        else if (exportableColumn && headerCustomProperties != null)
                        {
                            var headerColumnName = headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Value.Name : headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            var headerBackgroundColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerFontColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Value = headerColumnName;
                            initHeaderColumn++;
                        }
                    }
                    int rowNo = 0;

                    if (tsrDescription != null && tsrDescription.Count() > 0)
                    {
                        foreach (var info in _copypropertyInfos)
                        {
                            var tsrDesRow = 2;
                            var headerColumnName = info.Value.GetCustomAttribute<DisplayNameAttribute>() == null ?
                                info.Value.Name : info.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName;

                            foreach (var tsr in tsrDescription)
                            {
                                if (headerColumnName.ToLower().Trim() == tsr.Header.ToLower().Trim())
                                {
                                    worksheet.Cell(tsrDesRow, info.Key).DataType = XLDataType.Text;
                                    worksheet.Cell(tsrDesRow, info.Key).Style.Alignment.SetWrapText(true);
                                    worksheet.Cell(tsrDesRow, info.Key).Value = tsr.Description;
                                }

                            }
                        }
                        dataRow = 3;
                    }

                    //Rows
                    foreach (object item in sheet.Data)
                    {
                        var initColumn = 0;
                        var propertyInfoBase = item.GetType().BaseType.GetProperties();
                        var propertyInfoInherited = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        var propertyInfo = propertyInfoBase.Concat(propertyInfoInherited);
                        columnNo = 1;
                        rowNo = rowNo + 1;
                        foreach (var info in _copypropertyInfos)
                        {
                            columnNo++;
                            var name = FirstToLower(info.Value.Name);
                            var propertyCustomProperties = dataRender.Render.Where(y => y.PropertyName == name).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                            ignoreColumn = Attribute.IsDefined(info.Value, typeof(IgnoreGridAttribute));
                            exportableColumn = Attribute.IsDefined(info.Value, typeof(ExportableAttribute));
                            nonExportableColumn = Attribute.IsDefined(info.Value, typeof(NonExportableAttribute));
                            if (!ignoreColumn && !nonExportableColumn && propertyCustomProperties != null && propertyCustomProperties.Show)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.Value.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.Value.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.Value.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.Value.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.Value.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.Value.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                if (rowNo != 0 && EditableColumnNoList.Contains(columnNo))
                                {
                                    var EditColumnBackgroundColor = info.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xbfbfbf : info.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor; worksheet.Cell(headerRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(EditColumnBackgroundColor));
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(EditColumnBackgroundColor));
                                }

                                worksheet.Cell(dataRow, info.Key).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, info.Key).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            worksheet.Cell(dataRow, info.Key).Style.Alignment.SetWrapText(true);
                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;
                                            //  }

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.Value.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.Value.PropertyType))
                                {
                                    bool isDict = info.Value.PropertyType.IsGenericType && info.Value.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.Value.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, info.Key).Value = testo;
                                    }
                                }
                                else
                                {


                                    if (info.Value.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, info.Key).Value = (info.Value == null || (bool)info.Value.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.Value.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, info.Key).DataType =
                                            XLDataType.Text;

                                        var value = info.Value.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, info.Key).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.Value.PropertyType == typeof(string))
                                        {
                                            var text = info.Value.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }
                                                worksheet.Cell(dataRow, info.Key).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);
                                            }
                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);
                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, info.Key).DataType = cellFormatClosetXmlInfo.Value;
                                }

                                //if (EditableColumnNoList.Contains(columnNo))
                                //{
                                //    var EditColumnBackgroundColor = info.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0x808080 : info.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor; worksheet.Cell(headerRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(EditColumnBackgroundColor));
                                //    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(EditColumnBackgroundColor));
                                //}
                                initColumn++;
                            }
                            else if (exportableColumn && propertyCustomProperties != null)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.Value.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.Value.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.Value.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.Value.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.Value.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.Value.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));
                                worksheet.Cell(dataRow, info.Key).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, info.Key).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;

                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;
                                            //  }

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.Value.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.Value.PropertyType))
                                {
                                    bool isDict = info.Value.PropertyType.IsGenericType && info.Value.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.Value.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, info.Key).Value = testo;
                                    }
                                }
                                else
                                {
                                    if (info.Value.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, info.Key).Value = (info.Value == null || (bool)info.Value.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.Value.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, info.Key).DataType =
                                            XLDataType.Text;

                                        var value = info.Value.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, info.Key).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.Value.PropertyType == typeof(string))
                                        {
                                            var text = info.Value.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }

                                                worksheet.Cell(dataRow, info.Key).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                            }


                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, info.Key).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }

                        }

                        dataRow++;
                    }

                    worksheet.Columns().AdjustToContents(1.0, 50.0);
                    worksheet.RangeUsed().SetAutoFilter();
                }

            }
            wbk.SaveAs(streamFile);
            var content = streamFile.ToArray();
            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = content,
                FileName = fileName
            };

        }

        public ExportResult GetExcelFrom(List<ExportSheetCustom> sheets, string fileName)
        {
            using XLWorkbook wbk = new XLWorkbook();
            using MemoryStream streamFile = new MemoryStream();
            Dictionary<string, int> colorDictionary = new Dictionary<string, int>
            {
                { "blue", 0x2F75B5 },
                { "red", 0xe60000 },
                { "green", 0x00c300 },
                { "gray", 0xbfbfbf },
                { "black", 0x0F0D0D },
                { "white", 0xFFFFFF },
            };
            //Worksheet
            foreach (ExportSheetCustom sheet in sheets)
            {
                int iHeaderRow = 1;
                int iDataRow = 2;
                var worksheet = wbk.Worksheets.Add(sheet.TabName);

                if (sheet.Data.Count > 0)
                {
                    //Headers
                    foreach (IList<ExportSheetCustomHeader> headerRow in sheet.CustomHeaders)
                    {
                        int initHeaderColumn = 1;
                        foreach (ExportSheetCustomHeader header in headerRow)
                        {
                            IXLRange range;
                            if (header.ColSpan > 1)
                            {
                                range = worksheet.Range(iHeaderRow, initHeaderColumn, iHeaderRow, initHeaderColumn + header.ColSpan - 1);
                                range = range.Merge();
                            }
                            else
                            {
                                range = worksheet.Range(iHeaderRow, initHeaderColumn, iHeaderRow, initHeaderColumn);
                            }

                            range.Style.Font.SetFontColor(XLColor.FromArgb(header.ForeColor ?? 0xffffff));
                            range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(header.BackgroundColor ?? 0xe60000));
                            range.DataType = XLDataType.Text;
                            range.Style.Alignment.SetHorizontal((XLAlignmentHorizontalValues)header.Alignment);
                            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            range.Style.Border.SetOutsideBorderColor(XLColor.Black);
                            range.Style.Font.SetBold(true);

                            range.SetValue<string>((header.Show) ? header.Name : "");

                            initHeaderColumn += header.ColSpan;
                        }
                        iHeaderRow++;
                    }
                    iDataRow = iHeaderRow;

                    //Rows
                    foreach (IList<ExportSheetCustomCell> itemRow in sheet.Data)
                    {
                        var initColumn = 1;
                        foreach (ExportSheetCustomCell item in itemRow)
                        {
                            var cellBackgroundColor = item.BackgroundColor ?? 0;
                            var cellFontColor = item.ForeColor ?? 0x000000;
                            var cellFormatTypeInfo = item.FormatTypeInfo ?? ExportDataTypeEnum.Text;
                            var cellFormatInfo = item.FormatInfo;

                            if (cellBackgroundColor != 0)
                            {
                                worksheet.Cell(iDataRow, initColumn).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));
                            }
                            worksheet.Cell(iDataRow, initColumn).Style.Font.SetFontColor(XLColor.Black);
                            worksheet.Cell(iDataRow, initColumn).Style.Font.SetBold(item.Bold ?? false);
                            worksheet.Cell(iDataRow, initColumn).Style.Border.SetOutsideBorderColor(XLColor.Black);
                            worksheet.Cell(iDataRow, initColumn).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                            switch (cellFormatTypeInfo)
                            {
                                case ExportDataTypeEnum.Text:
                                    worksheet.Cell(iDataRow, initColumn).DataType = XLDataType.Text;

                                    break;
                                case ExportDataTypeEnum.Date:
                                    worksheet.Cell(iDataRow, initColumn).DataType = XLDataType.DateTime;

                                    break;
                                case ExportDataTypeEnum.Number:
                                    worksheet.Cell(iDataRow, initColumn).DataType = XLDataType.Number;

                                    break;
                                case ExportDataTypeEnum.Boolean:
                                    worksheet.Cell(iDataRow, initColumn).DataType = XLDataType.Boolean;
                                    break;
                                case ExportDataTypeEnum.DateText:
                                    if (!string.IsNullOrEmpty(cellFormatInfo))
                                        worksheet.Cell(iDataRow, initColumn).Style.DateFormat.Format = cellFormatInfo;

                                    worksheet.Cell(iDataRow, initColumn).DataType = XLDataType.Text;
                                    break;
                                case ExportDataTypeEnum.NumberText:
                                    if (!string.IsNullOrEmpty(cellFormatInfo))
                                        worksheet.Cell(iDataRow, initColumn).Style.NumberFormat.Format = cellFormatInfo;

                                    worksheet.Cell(iDataRow, initColumn).DataType = XLDataType.Text;
                                    break;
                                default:
                                    break;
                            }


                            worksheet.Cell(iDataRow, initColumn).SetValue(item.Value);
                            initColumn++;

                        }

                        iDataRow++;
                    }

                    worksheet.Columns().AdjustToContents(1.0, 50.0);
                    //worksheet.RangeUsed().SetAutoFilter();

                }
            }
            wbk.SaveAs(streamFile);
            var content = streamFile.ToArray();
            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = content,
                FileName = fileName
            };

        }

        public ExportResult GetExcelFromPAT(List<ExportSheetCustom> sheets, string fileName)
        {

            using (XLWorkbook wbk = new XLWorkbook())
            using (MemoryStream streamFile = new MemoryStream())
            {
                foreach (ExportSheetCustom sheet in sheets)
                {
                    int iHeaderRow = 1;
                    int iDataRow = 2;
                    var worksheet = wbk.Worksheets.Add(sheet.TabName);

                    if (sheet.Data.Count > 0)
                    {
                        // Headers
                        foreach (IList<ExportSheetCustomHeader> headerRow in sheet.CustomHeaders)
                        {
                            int initHeaderColumn = 1;
                            foreach (ExportSheetCustomHeader header in headerRow)
                            {
                                IXLRange range;
                                if (header.ColSpan > 1)
                                {
                                    range = worksheet.Range(iHeaderRow, initHeaderColumn, iHeaderRow, initHeaderColumn + header.ColSpan - 1);
                                    range = range.Merge();
                                }
                                else
                                {
                                    range = worksheet.Range(iHeaderRow, initHeaderColumn, iHeaderRow, initHeaderColumn);
                                }

                                range.Style.Font.SetFontColor(XLColor.FromArgb(header.ForeColor ?? 0xffffff));
                                range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(header.BackgroundColor ?? 0xe60000));
                                range.DataType = XLDataType.Text;
                                range.Style.Alignment.SetHorizontal((XLAlignmentHorizontalValues)header.Alignment);
                                range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                range.Style.Border.SetOutsideBorderColor(XLColor.Black);
                                range.Style.Font.SetBold(true);

                                range.SetValue<string>((header.Show) ? header.Name : "");

                                initHeaderColumn += header.ColSpan;
                            }
                            iHeaderRow++;
                        }
                        iDataRow = iHeaderRow;

                        // Rows
                        foreach (IList<ExportSheetCustomCell> itemRow in sheet.Data)
                        {
                            var initColumn = 1;
                            foreach (ExportSheetCustomCell item in itemRow)
                            {
                                var cell = worksheet.Cell(iDataRow, initColumn);

                                if (!string.IsNullOrEmpty(item.CellColor))
                                {
                                    //    cell.Style.Fill.SetPatternType(XLFillPatternValues.Solid);
                                    cell.Style.Fill.SetBackgroundColor(XLColor.FromName(item.CellColor));
                                }


                                cell.Style.Font.SetFontColor(XLColor.FromArgb(item.ForeColor ?? 0x000000));
                                cell.Style.Font.SetBold(item.Bold ?? false);
                                cell.Style.Border.SetOutsideBorderColor(XLColor.Black);
                                cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                cell.Style.Fill.SetPatternType(XLFillPatternValues.DarkGray);

                                switch (item.FormatTypeInfo)
                                {
                                    case ExportDataTypeEnum.Text:
                                        cell.DataType = XLDataType.Text;
                                        break;
                                    case ExportDataTypeEnum.Date:
                                        cell.DataType = XLDataType.DateTime;
                                        break;
                                    case ExportDataTypeEnum.Number:
                                        cell.DataType = XLDataType.Number;
                                        break;
                                    case ExportDataTypeEnum.Boolean:
                                        cell.DataType = XLDataType.Boolean;
                                        break;
                                    case ExportDataTypeEnum.DateText:
                                        if (!string.IsNullOrEmpty(item.FormatInfo))
                                            cell.Style.DateFormat.Format = item.FormatInfo;
                                        cell.DataType = XLDataType.Text;
                                        break;
                                    case ExportDataTypeEnum.NumberText:
                                        if (!string.IsNullOrEmpty(item.FormatInfo))
                                            cell.Style.NumberFormat.Format = item.FormatInfo;
                                        cell.DataType = XLDataType.Text;
                                        break;
                                    default:
                                        break;
                                }

                                cell.SetValue(item.Value);
                                initColumn++;
                            }

                            iDataRow++;
                        }

                        worksheet.Columns().AdjustToContents(1.0, 50.0);
                    }
                }
                wbk.SaveAs(streamFile);
                var content = streamFile.ToArray();
                return new ExportResult()
                {
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    FileInByteArray = content,
                    FileName = fileName
                };
            }
        }
        string FirstToLower(string s)
        {
            if (s != string.Empty && char.IsUpper(s[0]))
            {
                s = char.ToLower(s[0]) + s.Substring(1);
            }
            return s;
        }

        public ExportResult GetExcelFrom<T>(List<ExportSheet> sheets, string fileName, GenericReportInfrastructureGrid<T> dataRender)
        {
            using XLWorkbook wbk = new XLWorkbook();
            using MemoryStream streamFile = new MemoryStream();
            Dictionary<string, int> colorDictionary = new Dictionary<string, int>
            {
                { "blue", 0x2F75B5 },
                { "red", 0xe60000 },
                { "green", 0x00c300 },
                { "gray", 0xbfbfbf },
                { "black", 0x0F0D0D },
                { "white", 0xcccccc },
            };
            //Worksheet

            foreach (ExportSheet sheet in sheets)
            {
                const int headerRow = 1;
                int dataRow = 2;
                IXLWorksheet worksheet;
                if (string.IsNullOrEmpty(sheet.TabName))
                {
                    worksheet = wbk.Worksheets.Add("Export");
                    sheet.TabName = string.Empty;
                }
                else
                    worksheet = wbk.Worksheets.Add(sheet.TabName);

                if (sheet.Data.Count > 0 && dataRender.Render.Any(x => x.Show))
                {
                    //Header
                    var typeOfData = sheet.Data[0].GetType();
                    var headingsBase = typeOfData.BaseType?.GetProperties();
                    var headingsInherited = typeOfData.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    var headings = headingsBase.Concat(headingsInherited);
                    var ignoreColumn = false;
                    var exportableColumn = false;
                    var nonExportableColumn = false;

                    var initHeaderColumn = 0;

                    var render = dataRender.Render.Where(x => x.Show).OrderBy(x => x.Order).ToList();

                    Dictionary<int, PropertyInfo> propertyInfos = new Dictionary<int, PropertyInfo>();
                    Dictionary<int, PropertyInfo> _copypropertyInfos = new Dictionary<int, PropertyInfo>();
                    foreach (PropertyInfo headerInfo in headings)
                    {
                        var name = FirstToLower(headerInfo.Name);
                        var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                        ignoreColumn = Attribute.IsDefined(headerInfo, typeof(IgnoreGridAttribute));
                        exportableColumn = Attribute.IsDefined(headerInfo, typeof(ExportableAttribute));
                        nonExportableColumn = Attribute.IsDefined(headerInfo, typeof(NonExportableAttribute));
                        if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                        {
                            if (!propertyInfos.ContainsKey((int)headerCustomProperties.Order))
                                propertyInfos.Add((int)headerCustomProperties.Order, headerInfo);

                        }
                        else if (exportableColumn && headerCustomProperties != null)
                        {
                            if (!propertyInfos.ContainsKey((int)headerCustomProperties.Order))
                                propertyInfos.Add((int)headerCustomProperties.Order, headerInfo);
                        }
                    }
                    propertyInfos = propertyInfos.OrderBy(p => p.Key).ToDictionary(p => p.Key, k => k.Value);
                    int count = 1;

                    var emptyTableList = render.Where(x => x.TableName == "emptyGrid").OrderBy(x => x.Order).ToList();

                    foreach (var item in propertyInfos)
                    {
                        count = count + item.Key;
                        if (!_copypropertyInfos.ContainsKey(count))
                            _copypropertyInfos.Add(count, item.Value);
                        count = 1;
                    }
                    count = 1;
                    foreach (var item in emptyTableList)
                    {
                        count = count + Convert.ToInt16(item.Order);
                        if (!_copypropertyInfos.ContainsKey(count))
                            _copypropertyInfos.Add(count, null);
                        count = 1;
                    }
                    _copypropertyInfos = _copypropertyInfos.OrderBy(x => x.Key)
                        .ToDictionary(x => x.Key, x => x.Value);

                    foreach (var headerInfo in _copypropertyInfos)
                    {
                        if (headerInfo.Value == null)
                        {
                            // Empty Table Row
                            int keyTableCount = headerInfo.Key - 1;
                            var currentEmptyTableObject = emptyTableList.Where(x => x.Order == keyTableCount).FirstOrDefault();
                            if (currentEmptyTableObject.Show)
                            {
                                var headerColumnName = ((currentEmptyTableObject.UpdatedPropertyName != null)) ?
                                   FirstToUpper(currentEmptyTableObject.UpdatedPropertyName)
                                    : "";

                                //  currentEmptyTableObject.TableName  // ---**** No need to display Empty column
                                // Show only Custom Name;
                                var headerBackgroundColor =
                                    XLColor.FromArgb(colorDictionary.ContainsKey(currentEmptyTableObject.ColorHeader.ToLower()) ?
                                    colorDictionary[currentEmptyTableObject.ColorHeader.ToLower()] : 0xcccccc);


                                var headerFontColor = ((headerBackgroundColor == XLColor.FromArgb(0xffffff)) ||
                                    (headerBackgroundColor == XLColor.FromArgb(0xcccccc)))
                                       ? 0x0F0D0D : 0xffffff;

                                worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                                worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(headerBackgroundColor);
                                worksheet.Cell(headerRow, headerInfo.Key).Value = headerColumnName;
                                initHeaderColumn++;
                            }
                        }
                        else
                        {
                            var name = FirstToLower(headerInfo.Value.Name);
                            var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name).Select(x => new { x.Show, x.Order, x.ColorHeader, x.UpdatedPropertyName }).SingleOrDefault();
                            ignoreColumn = Attribute.IsDefined(headerInfo.Value, typeof(IgnoreGridAttribute));
                            exportableColumn = Attribute.IsDefined(headerInfo.Value, typeof(ExportableAttribute));
                            nonExportableColumn = Attribute.IsDefined(headerInfo.Value, typeof(NonExportableAttribute));
                            if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                            {
                                //Format the header
                                // var headerColumnName = headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Value.Name : headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                                var headerColumnName = headerCustomProperties.UpdatedPropertyName == null ?
                                     headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName : headerCustomProperties.UpdatedPropertyName;

                                var headerBackgroundColor = XLColor.FromArgb(colorDictionary.ContainsKey(headerCustomProperties.ColorHeader.ToLower()) ?
                                    colorDictionary[headerCustomProperties.ColorHeader.ToLower()] : 0xcccccc);

                                // headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                                var headerFontColor = ((headerBackgroundColor == XLColor.FromArgb(0xffffff)) ||
                                    (headerBackgroundColor == XLColor.FromArgb(0xcccccc)))
                                       ? (headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>()?.FontColor ?? 0x0F0D0D) :
                                       (headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>()?.FontColor ?? 0xffffff);


                                worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                                worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(headerBackgroundColor);
                                worksheet.Cell(headerRow, headerInfo.Key).Value = FirstToUpper(headerColumnName);
                                initHeaderColumn++;
                            }
                            else if (exportableColumn && headerCustomProperties != null)
                            {
                                // var headerColumnName = headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Value.Name : headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                                var headerColumnName = headerCustomProperties.UpdatedPropertyName == null ? headerInfo.Value.Name : headerCustomProperties.UpdatedPropertyName;

                                var headerBackgroundColor = XLColor.FromArgb(colorDictionary.ContainsKey(headerCustomProperties.ColorHeader.ToLower()) ?
                                    colorDictionary[headerCustomProperties.ColorHeader.ToLower()] : 0xcccccc);

                                var headerFontColor = ((headerBackgroundColor == XLColor.FromArgb(0xffffff)) ||
                                    (headerBackgroundColor == XLColor.FromArgb(0xcccccc)))
                                       ? (headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>()?.FontColor ?? 0x0F0D0D) :
                                       (headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>()?.FontColor ?? 0xffffff);

                                worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                                worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(headerBackgroundColor);
                                worksheet.Cell(headerRow, headerInfo.Key).Value = FirstToUpper(headerColumnName);
                                initHeaderColumn++;
                            }

                        }

                    }
                    //Rows
                    foreach (object item in sheet.Data)
                    {
                        var initColumn = 0;
                        var propertyInfoBase = item.GetType().BaseType.GetProperties();
                        var propertyInfoInherited = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        var propertyInfo = propertyInfoBase.Concat(propertyInfoInherited);

                        foreach (var info in _copypropertyInfos)
                        {
                            if (info.Value == null) continue; // Empty Column Row
                            var name = FirstToLower(info.Value.Name);
                            var propertyCustomProperties = dataRender.Render.Where(y => y.PropertyName == name).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                            ignoreColumn = Attribute.IsDefined(info.Value, typeof(IgnoreGridAttribute));
                            exportableColumn = Attribute.IsDefined(info.Value, typeof(ExportableAttribute));
                            nonExportableColumn = Attribute.IsDefined(info.Value, typeof(NonExportableAttribute));
                            if (!ignoreColumn && !nonExportableColumn && propertyCustomProperties != null && propertyCustomProperties.Show)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.Value.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.Value.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.Value.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.Value.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.Value.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.Value.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, info.Key).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, info.Key).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            worksheet.Cell(dataRow, info.Key).Style.Alignment.SetWrapText(true);
                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;
                                            //  }

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.Value.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.Value.PropertyType))
                                {
                                    bool isDict = info.Value.PropertyType.IsGenericType && info.Value.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.Value.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, info.Key).Value = testo;
                                    }
                                }
                                else
                                {


                                    if (info.Value.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, info.Key).Value = (info.Value == null || (bool)info.Value.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.Value.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, info.Key).DataType =
                                            XLDataType.Text;

                                        var value = info.Value.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, info.Key).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.Value.PropertyType == typeof(string))
                                        {
                                            var text = info.Value.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }
                                                worksheet.Cell(dataRow, info.Key).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);
                                            }
                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);
                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, info.Key).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }
                            else if (exportableColumn && propertyCustomProperties != null)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.Value.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.Value.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.Value.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.Value.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.Value.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.Value.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, info.Key).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, info.Key).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;

                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;
                                            //  }

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.Value.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.Value.PropertyType))
                                {
                                    bool isDict = info.Value.PropertyType.IsGenericType && info.Value.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.Value.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, info.Key).Value = testo;
                                    }
                                }
                                else
                                {
                                    if (info.Value.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, info.Key).Value = (info.Value == null || (bool)info.Value.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.Value.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, info.Key).DataType =
                                            XLDataType.Text;

                                        var value = info.Value.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, info.Key).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.Value.PropertyType == typeof(string))
                                        {
                                            var text = info.Value.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }

                                                worksheet.Cell(dataRow, info.Key).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                            }


                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, info.Key).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }
                        }

                        dataRow++;
                    }
                    worksheet.Columns().AdjustToContents(1.0, 50.0);
                    worksheet.RangeUsed().SetAutoFilter();
                }
            }
            wbk.SaveAs(streamFile);
            var content = streamFile.ToArray();
            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = content,
                FileName = fileName
            };
        }

        public void GetExcelFromFile<T>(string filePath, List<T> data)
        {
            var headerRow = 3;
            var columnStart = 5;
            using (XLWorkbook workbook = new XLWorkbook(filePath))
            {
                var workSheet = workbook.Worksheet(1);

                var rows = workSheet.Row(headerRow);
                var sheetName = workSheet.Name;
                var headerCells = workSheet.Row(headerRow).CellsUsed();
                var headers = headerCells
                              .Where(c => c.Address.ColumnNumber >= columnStart)
                              .ToDictionary(
                                  c => c.Address.ColumnNumber,
                                  c => c.GetString().Trim());


                int startWriteRow = headerRow + 1;

                for (int i = 0; i < data.Count; i++)
                {
                    var dataRow = data[i];

                    foreach (var kv in headers)
                    {
                        var colIndex = kv.Key;
                        var headerName = kv.Value;

                        if (dataRow != null)
                        {
                            workSheet.Cell(startWriteRow + i, colIndex).Value = "test";
                        }
                    }
                }

                workbook.Save();
            }

        }

        #region ExcelTempleConfiguration based Export the data

        public byte[] ReadXlsHeaders(string filePath, string fileName)
        {
            var headers = new List<string>();

            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var workbook = new HSSFWorkbook(fileStream); // For .xls
            var sheet = workbook.GetSheetAt(0);
            var headerRow = sheet.GetRow(10); // Assumes headers are in first row start from 0 

            for (int i = 3; i < headerRow.LastCellNum; i++)
            {
                var cell = headerRow.GetCell(i);
                headers.Add(cell?.ToString() ?? $"Column{i + 1}");
            }


            var rowToUpdate = sheet.GetRow(1); // Second row
            rowToUpdate.GetCell(1).SetCellValue(35);


            // Save to file
            var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            workbook.Write(stream);
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                fileBytes = ms.ToArray();
            }

            using var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            byte[] newfileBytes = memoryStream.ToArray();

            return newfileBytes;
        }

        public ExportResult ExportLegacyExcelBasedOnConfiguration<T>(List<ExportSheet> sheets, string fileName,
              string filePath, List<T> data, List<Exceltemplateconfiguration> excelConfigData)
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var workbook = new HSSFWorkbook(fileStream);// for .xls files
            var sheet = workbook.GetSheetAt(0);




            // Map configuration by column order
            var orderedConfigs = excelConfigData
                .Where(x => x.Columnorder != null)
                .OrderBy(x => x.Columnorder)
                .ToList();

            int headerRowIndex = Convert.ToInt16(excelConfigData.FirstOrDefault(x => x.Headerrowstarting != null)?.Headerrowstarting ?? 0);
            int dataRowIndex = Convert.ToInt16(excelConfigData.FirstOrDefault(x => x.Rowstarting != null)?.Rowstarting ?? 1);

            #region
            for (int rowIndex = 0; rowIndex < data.Count; rowIndex++)
            {
                int index = dataRowIndex + rowIndex;
                var row = sheet.GetRow(index) ?? sheet.CreateRow(index);
                var item = data[rowIndex];

                if (row == null)
                    continue;

                foreach (var config in orderedConfigs)
                {
                    int colIndex = config.Columnorder ?? 0;

                    var prop = typeof(T).GetProperty(config.Columnheadername, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (prop != null)
                    {
                        var value = prop.GetValue(item)?.ToString() ?? "";
                        ICell cell = row.GetCell(colIndex);
                        var existingStyle = cell?.CellStyle;
                        if (cell == null)
                        {
                            cell = row.CreateCell(colIndex);
                        }
                        else
                        {
                            var style = cell.CellStyle;
                            cell = row.CreateCell(colIndex);
                            cell.CellStyle = style;
                        }
                        //if (existingStyle != null)
                        //{
                        //    cell.CellStyle = existingStyle;
                        //}
                        cell.SetCellValue(value);
                    }
                    else
                    {
                        prop = typeof(T).GetProperty(config.Propertyname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        if (prop != null)
                        {
                            var value = prop.GetValue(item)?.ToString() ?? "";
                            ICell cell = row.GetCell(colIndex);
                            if (cell == null)
                            {
                                cell = row.CreateCell(colIndex);
                            }
                            else
                            {
                                var style = cell.CellStyle;
                                cell = row.CreateCell(colIndex);
                                cell.CellStyle = style;
                            }

                            cell.SetCellValue(value);
                        }
                    }
                }
            }


            #endregion

            using var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            byte[] newfileBytes = memoryStream.ToArray();

            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = newfileBytes,
                FileName = fileName
            };

        }

        public ExportResult ExportExcelBasedOnConfiguration<T>(List<ExportSheet> sheets, string fileName,
      string filePath, List<T> data, List<Exceltemplateconfiguration> excelConfigData)
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var workbook = new XSSFWorkbook(fileStream);
            var sheet = workbook.GetSheetAt(0);

            // Map configuration by column order
            var orderedConfigs = excelConfigData
                .Where(x => x.Columnorder != null)
                .OrderBy(x => x.Columnorder)
                .ToList();

            int headerRowIndex = Convert.ToInt16(excelConfigData.FirstOrDefault(x => x.Headerrowstarting != null)?.Headerrowstarting ?? 0);
            int dataRowIndex = Convert.ToInt16(excelConfigData.FirstOrDefault(x => x.Rowstarting != null)?.Rowstarting ?? 1);

           
            #region
            for (int rowIndex = 0; rowIndex < data.Count; rowIndex++)
            {
                int index = dataRowIndex + rowIndex;
                var row = sheet.GetRow(index) ?? sheet.CreateRow(index);
                var item = data[rowIndex];

                if (row == null)
                    continue;

                foreach (var config in orderedConfigs)
                {
                    int colIndex = config.Columnorder ?? 0;

                    var prop = typeof(T).GetProperty(config.Columnheadername, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (prop == null)
                    {
                        prop = typeof(T).GetProperty(config.Propertyname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    }
                    if (prop != null)
                    {
                        var value = prop.GetValue(item)?.ToString() ?? "";
                        var colour = string.Empty;

                        if (value.Contains("|"))
                        {
                            var split = value.Split("|");
                            value = split[0];
                            colour = split[1];
                        }

                        var cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);

                        cell.SetCellValue(value);

                        if (!string.IsNullOrEmpty(colour.Trim()) && colour != " ")
                        {
                            System.Drawing.Color sysColor;

                            var namedColor = System.Drawing.Color.FromName(colour.Trim());
                            if (namedColor.IsKnownColor)
                            {
                                sysColor = namedColor;
                            }
                            else
                            {
                                if (!colour.StartsWith("#"))
                                    colour = "#" + colour;

                                sysColor = System.Drawing.ColorTranslator.FromHtml(colour.Trim());
                            }

                            // Convert to RGB bytes
                            byte[] rgb = { sysColor.R, sysColor.G, sysColor.B };

                            var style = (XSSFCellStyle)workbook.CreateCellStyle();
                            var xColor = new XSSFColor(rgb);

                            style.SetFillForegroundColor(xColor);
                            style.FillPattern = FillPattern.SolidForeground;

                            cell.CellStyle = style;
                        }
                    }

                }
            }


            #endregion

            using var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            byte[] newfileBytes = memoryStream.ToArray();

            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = newfileBytes,
                FileName = fileName
            };

        }


        #region
        [Obsolete]
        public ExportResult ExportLegacyExcelBasedOnConfigurationForXBOM<T>(List<ExportSheet> sheets, string fileName, string filePath, string logoPath, List<T> data, List<Exceltemplateconfiguration> excelConfigData)
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var templateSheetIndex = 6; // 6th sheet 
            var workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = null;
            ISheet targetSheet = null;



            var borderStyle = BorderStyle(workbook);

            // Map configuration by column order
            var orderedConfigs = excelConfigData
                .Where(x => x.Columnorder != null)
                .OrderBy(x => x.Columnorder)
                .ToList();

            int headerRowIndex = Convert.ToInt16(excelConfigData
                .FirstOrDefault(x => x.Headerrowstarting != null)?.Headerrowstarting ?? 0);

            int dataRowIndex = Convert.ToInt16(excelConfigData
                .FirstOrDefault(x => x.Rowstarting != null)?.Rowstarting ?? 1);

            int targetSheetIndex = templateSheetIndex;
            var previousFinancialYear = string.Empty;

            foreach (var sheetData in sheets)
            {
                var sheetName = sheetData.TabName;

                // First sheet 

                sheet = workbook.GetSheetAt(templateSheetIndex);
                targetSheet = CreateModelSheet(workbook, sheetName, targetSheetIndex + 1, logoPath, sheet);

                Dictionary<string, Dictionary<string, int>> fyColumnMappings = new Dictionary<string, Dictionary<string, int>>();
                Dictionary<string, Dictionary<string, int>> checkExistingFy = new Dictionary<string, Dictionary<string, int>>();

                Dictionary<string, int> subCols = new Dictionary<string, int>();
                var mergedColumnposition = 4;
                var financialYear = string.Empty;


                // Write new data
                for (int rowIndex = 0; rowIndex < sheetData.Data.Count; rowIndex++)
                {
                    fyColumnMappings = new Dictionary<string, Dictionary<string, int>>();
                    int targetRowIndex = dataRowIndex + rowIndex;
                    var row = targetSheet.GetRow(targetRowIndex) ?? targetSheet.CreateRow(targetRowIndex);
                    var item = sheetData.Data[rowIndex];

                    var entityFinaacialYearprop = typeof(T).GetProperty("financialYear", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    var entityFy = entityFinaacialYearprop?.GetValue(item)?.ToString();

                    #region
                    var fyProp = item.GetType().GetProperty("vnfVbomCapacityDtoGrids");

                    var financialDetails = (List<VnfVbomCapacityDtoGrid>)fyProp.GetValue(item);

                    if (financialDetails.Count > 0)
                    {
                        int offSet = 17;// declear the financial year below column count
                        var ignoreFyear = financialDetails.Count; // ignore financial  year cell in sheet
                        foreach (var fy in financialDetails)
                        {
                            financialYear = fy.FinancialYear;

                            CellRangeAddress merged = targetSheet.GetMergedRegion(mergedColumnposition); // 5 is a financial cell 
                            IRow mergrow = targetSheet.GetRow(merged.FirstRow);
                            ICell cell = mergrow.GetCell(merged.FirstColumn);
                            int firstCol = merged.FirstColumn;
                            int lastCol = merged.LastColumn;
                            int firstRow = merged.FirstRow;
                            int lastRow = 3; // copy till last row
                            // Build dictionary for this FY’s sub-columns (row below header)
                            int subHeaderRowIndex = firstRow + 1; // row below FY
                            IRow subHeaderRow = targetSheet.GetRow(subHeaderRowIndex);
                            IRow changeMergeFyValue = targetSheet.GetRow(firstRow);

                            if (financialDetails.Count == ignoreFyear && rowIndex == 0)
                            {
                                if (changeMergeFyValue != null)
                                {
                                    ICell fycell = changeMergeFyValue.GetCell(firstCol);
                                    if (fycell != null)
                                    {
                                        string val = fycell.ToString();
                                        if (val == "FY 24/25")
                                        {
                                            fycell.SetCellValue("FY" + GetFinancialYear(financialYear.ToString()));
                                        }
                                    }
                                }

                                if (subHeaderRow != null)
                                {
                                    // Use PhysicalNumberOfCells to loop all populated cells in this row
                                    subCols = new Dictionary<string, int>();

                                    for (int c = firstCol; c <= lastCol; c++)
                                    {
                                        ICell subCell = subHeaderRow.GetCell(c);
                                        if (subCell != null && !string.IsNullOrWhiteSpace(subCell.ToString()))
                                        {
                                            string key = subCell.ToString().Trim().ToLower().Replace(" ", ""); // e.g. "fymode"
                                            if (!subCols.ContainsKey(key))
                                                subCols[key] = c;
                                        }
                                    }
                                }
                                fyColumnMappings[financialYear] = subCols;
                                checkExistingFy[financialYear] = subCols;
                            }

                            // Store mapping by FY

                            if ((ignoreFyear < financialDetails.Count || rowIndex != 0) && checkExistingFy.ContainsKey(financialYear) == false)
                            {
                                for (int r = firstRow; r <= lastRow; r++)
                                {
                                    IRow sourceRow = targetSheet.GetRow(r);
                                    if (sourceRow == null) continue;

                                    IRow destRow = targetSheet.GetRow(r) ?? targetSheet.CreateRow(r);

                                    for (int c = firstCol; c <= lastCol; c++)
                                    {
                                        ICell sourceCell = sourceRow.GetCell(c);
                                        if (sourceCell == null) continue;

                                        int destCol = firstCol + offSet + (c - firstCol);
                                        ICell destCell = destRow.GetCell(destCol) ?? destRow.CreateCell(destCol);

                                        // Copy value
                                        if (sourceCell.ToString().Contains("FY 24/25"))
                                        {
                                            destCell.SetCellValue("FY" + GetFinancialYear(financialYear.ToString()));
                                        }
                                        else if (sourceCell.ToString().Contains("FY" + previousFinancialYear))
                                        {
                                            destCell.SetCellValue("FY" + GetFinancialYear(financialYear));
                                        }
                                        else if (sourceCell.ToString().Contains("FY"))
                                        {
                                            destCell.SetCellValue("FY" + GetFinancialYear(financialYear));
                                        }
                                        else
                                        {
                                            destCell.SetCellValue(sourceCell.ToString());
                                        }


                                        // Copy style
                                        destCell.CellStyle = sourceCell.CellStyle;
                                    }
                                }

                                // Copy merged region itself
                                CellRangeAddress newMerged = new CellRangeAddress(
                                    merged.FirstRow,
                                    merged.LastRow,
                                    firstCol + offSet,
                                    lastCol + offSet
                                );
                                targetSheet.AddMergedRegion(newMerged);
                                mergedColumnposition++;

                                if (subHeaderRow != null)
                                {
                                    // Use PhysicalNumberOfCells to loop all populated cells in this row
                                    subCols = new Dictionary<string, int>();

                                    for (int c = firstCol + offSet; c <= lastCol + offSet; c++)
                                    {
                                        ICell subCell = subHeaderRow.GetCell(c);
                                        if (subCell != null && !string.IsNullOrWhiteSpace(subCell.ToString()))
                                        {
                                            string key = subCell.ToString().Trim().ToLower().Replace(" ", ""); // e.g. "fymode"
                                            if (!subCols.ContainsKey(key))
                                                subCols[key] = c;
                                        }
                                        //attributePosition = c;
                                    }
                                }
                                fyColumnMappings[financialYear] = subCols;
                                checkExistingFy[financialYear] = subCols;

                            }
                            else
                            {
                                var val = checkExistingFy[financialYear];
                                fyColumnMappings[financialYear] = val;
                            }
                            ignoreFyear--;
                            previousFinancialYear = GetFinancialYear(financialYear);
                        }
                    }


                    #endregion

                    if (row == null) continue;

                    foreach (var config in orderedConfigs)
                    {
                        int colIndex = config.Columnorder ?? 0;
                        if (config.Mappingreference.IsNullOrEmpty())
                        {
                            // Try Columnheadername first
                            var prop = typeof(T).GetProperty(config.Columnheadername,
                                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                            // If not found, try Propertyname
                            if (prop == null)
                            {
                                prop = typeof(T).GetProperty(config.Propertyname,
                                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                            }

                            if (prop != null)
                            {
                                var value = prop.GetValue(item)?.ToString() ?? "";


                                // Override the site name in based on the our data
                                if (config.Propertyname == "siteLocation")
                                {
                                    IRow siteRow = targetSheet.GetRow(1);
                                    if (siteRow != null)
                                    {
                                        ICell siteCell = siteRow.GetCell(5) ?? siteRow.CreateCell(5);
                                        siteCell.SetCellValue(value);

                                    }
                                }

                                if (config.Columnheadername == "FY 24/25" || config.Propertyname == "financialYear")
                                {
                                    IRow FYrow = targetSheet.GetRow(config.Headerrowstarting.Value);
                                    if (FYrow != null && rowIndex == 0)
                                    {
                                        ICell FYcell = FYrow.GetCell(config.Columnorder.Value) ?? FYrow.CreateCell(config.Columnorder.Value);
                                        FYcell.SetCellValue("FY" + GetFinancialYear(value));

                                    }
                                }
                                else
                                {
                                    var cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);
                                    cell.SetCellValue(value);
                                }
                            }
                        }
                        else if (fyColumnMappings.Count > 0)
                        {
                            foreach (var fyEntity in financialDetails)
                            {
                                var fydata = fyColumnMappings[fyEntity.FinancialYear];
                                colIndex = fydata.ContainsKey(config.Columnheadername.Trim().ToLower().Replace(" ", "")) ? fydata[config.Columnheadername.Trim().ToLower().Replace(" ", "")] : colIndex;
                                // Try Columnheadername first
                                var props = typeof(T).GetProperty(config.Columnheadername,
                                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                                // If not found, try Propertyname
                                if (props == null)
                                {
                                    props = typeof(T).GetProperty(config.Propertyname,
                                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                                }

                                if (props != null)
                                {
                                    var value = props.GetValue(item)?.ToString() ?? "";

                                    var cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);
                                    cell.SetCellValue(value);
                                }
                            }
                        }
                    }

                }
                // autosize the column based on the value
                var headerRow = targetSheet.GetRow(headerRowIndex);
                if (headerRow != null)
                {
                    for (int col = 0; col < headerRow.LastCellNum; col++)
                    {

                        targetSheet.AutoSizeColumn(col);
                    }
                }

                targetSheetIndex++;
            }

            if (sheets.Count > 0)
            {
                // Delete model sheet
                DeleteSheet(workbook, templateSheetIndex);
            }


            //Ensure final sheet order matches the sheets list
            for (int i = 0; i < sheets.Count; i++)
            {
                workbook.SetSheetOrder(sheets[i].TabName, templateSheetIndex + i);
            }
            workbook.SetActiveSheet(0);
            workbook.SetSelectedTab(0);

            using var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            byte[] newfileBytes = memoryStream.ToArray();

            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = newfileBytes,
                FileName = fileName
            };
        }

        public ExportResult ExportLegacyExcelBasedOnConfigurationForCBOM<T>(List<ExportSheet> sheets, string fileName, string filePath, string logoPath, List<T> data, List<Exceltemplateconfiguration> excelConfigData, List<Exceltemplateconfiguration> excelConfigDataCNFInfo)
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var templateSheetIndex = 9;
            var workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = null;
            ISheet targetSheet = null;


            var borderStyle = BorderStyle(workbook);

            // Map configuration by column order
            var orderedConfigs = excelConfigData
                .Where(x => x.Columnorder != null)
                .OrderBy(x => x.Columnorder)
                .ToList();

            int headerRowIndex = Convert.ToInt16(excelConfigData
                .FirstOrDefault(x => x.Headerrowstarting != null)?.Headerrowstarting ?? 0);

            int dataRowIndex = Convert.ToInt16(excelConfigData
                .FirstOrDefault(x => x.Rowstarting != null)?.Rowstarting ?? 1);

            int targetSheetIndex = templateSheetIndex;
            var previousFinancialYear = string.Empty;

            foreach (var sheetData in sheets)
            {
                var sheetName = sheetData.TabName;

                if (sheetName.Equals("CNF_info", StringComparison.OrdinalIgnoreCase))
                {
                    var orderedConfigsCnfinfo = excelConfigDataCNFInfo
                                        .Where(x => x.Columnorder != null)
                                        .OrderBy(x => x.Columnorder)
                                        .ToList();
                    ISheet ganesSheet = workbook.GetSheet("CNF_info");
                    if (ganesSheet != null)
                    {
                        // Build header map (support duplicate headers)
                        Dictionary<string, List<int>> headerMap = new();
                        IRow headerRow1 = ganesSheet.GetRow(0);
                        if (headerRow1 != null)
                        {
                            for (int c = 0; c < headerRow1.LastCellNum; c++)
                            {
                                ICell cell = headerRow1.GetCell(c);
                                if (cell != null && !string.IsNullOrWhiteSpace(cell.ToString()))
                                {
                                    string headerValue = NormalizeKey(cell.ToString());
                                    if (!headerMap.ContainsKey(headerValue))
                                        headerMap[headerValue] = new List<int>();
                                    headerMap[headerValue].Add(c);
                                }
                            }
                        }

                        // Write data starting from row 2 (index 1)
                        for (int rowIndex = 0; rowIndex < sheetData.Data.Count; rowIndex++)
                        {
                            var item = sheetData.Data[rowIndex];
                            int targetRowIndex = rowIndex + 1; // skip header row
                            IRow row = ganesSheet.GetRow(targetRowIndex) ?? ganesSheet.CreateRow(targetRowIndex);

                            foreach (var config in orderedConfigsCnfinfo)
                            {
                                if (config.Mappingreference.IsNullOrEmpty())
                                {
                                    var prop = typeof(CnfInfoSheetExportGridDto).GetProperty(config.Propertyname,
                                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                                            ?? typeof(CnfInfoSheetExportGridDto).GetProperty(config.Columnheadername,
                                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                                    if (prop != null)
                                    {
                                        var value = prop.GetValue(item)?.ToString() ?? "";
                                        string normalizedKey = NormalizeKey(config.Columnheadername);

                                        if (headerMap.TryGetValue(normalizedKey, out var colIndexes))
                                        {
                                            // Write the same value into all duplicate columns
                                            foreach (int colIndex in colIndexes)
                                            {
                                                ICell cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);
                                                cell.SetCellValue(value);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    continue; 
                }

                // First sheet 

                sheet = workbook.GetSheetAt(templateSheetIndex);
                targetSheet = CreateModelSheet(workbook, sheetName, targetSheetIndex + 1, logoPath, sheet);

                Dictionary<string, Dictionary<string, int>> fyColumnMappings = new Dictionary<string, Dictionary<string, int>>();
                Dictionary<string, Dictionary<string, int>> checkExistingFy = new Dictionary<string, Dictionary<string, int>>();

                Dictionary<string, int> subCols = new Dictionary<string, int>();
                var mergedColumnposition = 2;
                var financialYear = string.Empty;


                // Write new data
                for (int rowIndex = 0; rowIndex < sheetData.Data.Count; rowIndex++)
                {
                    fyColumnMappings = new Dictionary<string, Dictionary<string, int>>();
                    int targetRowIndex = dataRowIndex + rowIndex;
                    var row = targetSheet.GetRow(targetRowIndex) ?? targetSheet.CreateRow(targetRowIndex);
                    var item = sheetData.Data[rowIndex];

                    var entityFinaacialYearprop = typeof(CbomExportGridDto).GetProperty("financialYear", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    var entityFy = entityFinaacialYearprop?.GetValue(item)?.ToString();

                    #region
                    var fyProp = item.GetType().GetProperty("_cnfCapacityDtoGrid");

                    var financialDetails = (List<CnfCapacityDtoGrid>)fyProp.GetValue(item);

                    if (financialDetails.Count > 0)
                    {
                        int offSet = 17;// declear the financial year below column count
                        var ignoreFyear = financialDetails.Count; // ignore financial  year cell in sheet
                        foreach (var fy in financialDetails)
                        {
                            financialYear = fy.FinancialYear;

                            CellRangeAddress merged = targetSheet.GetMergedRegion(mergedColumnposition); // 5 is a financial cell 
                            IRow mergrow = targetSheet.GetRow(merged.FirstRow);
                            ICell cell = mergrow.GetCell(merged.FirstColumn);
                            int firstCol = merged.FirstColumn;
                            int lastCol = merged.LastColumn;
                            int firstRow = merged.FirstRow;
                            int lastRow = 3; // copy till last row
                            // Build dictionary for this FY’s sub-columns (row below header)
                            int subHeaderRowIndex = firstRow + 1; // row below FY
                            IRow subHeaderRow = targetSheet.GetRow(subHeaderRowIndex);
                            IRow changeMergeFyValue = targetSheet.GetRow(firstRow);

                            if (financialDetails.Count == ignoreFyear && rowIndex == 0)
                            {
                                if (changeMergeFyValue != null)
                                {
                                    ICell fycell = changeMergeFyValue.GetCell(firstCol);
                                    if (fycell != null)
                                    {
                                        string val = fycell.ToString();
                                        if (val == "Requirements - Y1 (FY 21/22)")
                                        {
                                            fycell.SetCellValue("FY" + GetFinancialYear(financialYear.ToString()));
                                        }
                                    }
                                }

                                if (subHeaderRow != null)
                                {
                                    // Use PhysicalNumberOfCells to loop all populated cells in this row
                                    subCols = new Dictionary<string, int>();

                                    for (int c = firstCol; c <= lastCol; c++)
                                    {
                                        ICell subCell = subHeaderRow.GetCell(c);
                                        if (subCell != null && !string.IsNullOrWhiteSpace(subCell.ToString()))
                                        {
                                            string key = NormalizeKey(subCell.ToString()); // e.g. "fymode"
                                            if (!subCols.ContainsKey(key))
                                                subCols[key] = c;
                                        }
                                    }
                                }
                                fyColumnMappings[financialYear] = subCols;
                                checkExistingFy[financialYear] = subCols;
                            }

                            // Store mapping by FY

                            if ((ignoreFyear < financialDetails.Count || rowIndex != 0) && checkExistingFy.ContainsKey(financialYear) == false)
                            {
                                for (int r = firstRow; r <= lastRow; r++)
                                {
                                    IRow sourceRow = targetSheet.GetRow(r);
                                    if (sourceRow == null) continue;

                                    IRow destRow = targetSheet.GetRow(r) ?? targetSheet.CreateRow(r);

                                    for (int c = firstCol; c <= lastCol; c++)
                                    {
                                        ICell sourceCell = sourceRow.GetCell(c);
                                        if (sourceCell == null) continue;

                                        int destCol = firstCol + offSet + (c - firstCol);
                                        ICell destCell = destRow.GetCell(destCol) ?? destRow.CreateCell(destCol);

                                        // Copy value
                                        if (sourceCell.ToString().Contains("Requirements - Y1 (FY 21/22)"))
                                        {
                                            destCell.SetCellValue("FY" + GetFinancialYear(financialYear.ToString()));
                                        }
                                        else if (sourceCell.ToString().Contains("FY" + previousFinancialYear))
                                        {
                                            destCell.SetCellValue("FY" + GetFinancialYear(financialYear));
                                        }
                                        else if (sourceCell.ToString().Contains("FY"))
                                        {
                                            destCell.SetCellValue("FY" + GetFinancialYear(financialYear));
                                        }
                                        else
                                        {
                                            destCell.SetCellValue(sourceCell.ToString());
                                        }


                                        // Copy style
                                        destCell.CellStyle = sourceCell.CellStyle;
                                    }
                                }

                                // Copy merged region itself
                                CellRangeAddress newMerged = new CellRangeAddress(
                                    merged.FirstRow,
                                    merged.LastRow,
                                    firstCol + offSet,
                                    lastCol + offSet
                                );
                                targetSheet.AddMergedRegion(newMerged);
                                mergedColumnposition++;

                                if (subHeaderRow != null)
                                {
                                    // Use PhysicalNumberOfCells to loop all populated cells in this row
                                    subCols = new Dictionary<string, int>();

                                    for (int c = firstCol + offSet; c <= lastCol + offSet; c++)
                                    {
                                        ICell subCell = subHeaderRow.GetCell(c);
                                        if (subCell != null && !string.IsNullOrWhiteSpace(subCell.ToString()))
                                        {
                                            string key = NormalizeKey(subCell.ToString()); // e.g. "fymode"
                                            if (!subCols.ContainsKey(key))
                                                subCols[key] = c;
                                        }
                                        //attributePosition = c;
                                    }
                                }
                                fyColumnMappings[financialYear] = subCols;
                                checkExistingFy[financialYear] = subCols;

                            }
                            else
                            {
                                var val = checkExistingFy[financialYear];
                                fyColumnMappings[financialYear] = val;
                            }
                            ignoreFyear--;
                            previousFinancialYear = GetFinancialYear(financialYear);
                        }
                    }


                    #endregion

                    if (row == null) continue;

                    foreach (var config in orderedConfigs)
                    {
                        int colIndex = config.Columnorder ?? 0;
                        if (config.Mappingreference.IsNullOrEmpty())
                        {
                            // Try Columnheadername first
                            var prop = typeof(CbomExportGridDto).GetProperty(config.Columnheadername,
                                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                            // If not found, try Propertyname
                            if (prop == null)
                            {
                                prop = typeof(CbomExportGridDto).GetProperty(config.Propertyname,
                                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                            }

                            if (prop != null)
                            {
                                var value = prop.GetValue(item)?.ToString() ?? "";


                                // Override the site name in based on the our data
                                if (config.Propertyname == "Site")
                                {
                                    IRow siteRow = targetSheet.GetRow(1);
                                    if (siteRow != null)
                                    {
                                        ICell siteCell = siteRow.GetCell(5) ?? siteRow.CreateCell(5);
                                        siteCell.SetCellValue(value);

                                    }
                                }

                                if (config.Columnheadername == "Requirements - Y1 (FY 21/22)" || config.Propertyname == "financialYear")
                                {
                                    IRow FYrow = targetSheet.GetRow(config.Headerrowstarting.Value);
                                    if (FYrow != null && rowIndex == 0)
                                    {
                                        ICell FYcell = FYrow.GetCell(config.Columnorder.Value) ?? FYrow.CreateCell(config.Columnorder.Value);
                                        FYcell.SetCellValue("Requirements - FY" + GetFinancialYear(value));

                                    }
                                }
                                else
                                {
                                    var cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);
                                    cell.SetCellValue(value);
                                }
                            }
                        }
                        else if (fyColumnMappings.Count > 0)
                        {
                            foreach (var fyEntity in financialDetails)
                            {
                                //var fydata = fyColumnMappings[fyEntity.FinancialYear];
                                //colIndex = fydata.ContainsKey(config.Columnheadername.Trim().ToLower().Replace(" ","")) ? fydata[config.Columnheadername.Trim().ToLower().Replace(" ", "")] : colIndex;

                                var fydata = fyColumnMappings[fyEntity.FinancialYear];
                                string normalizedKey = NormalizeKey(config.Columnheadername);

                                if (fydata.TryGetValue(normalizedKey, out int mappedIndex))
                                {
                                    colIndex = mappedIndex;
                                }

                                // Try Columnheadername first
                                var props = typeof(CbomExportGridDto).GetProperty(config.Columnheadername,
                                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                                // If not found, try Propertyname
                                if (props == null)
                                {
                                    props = typeof(CbomExportGridDto).GetProperty(config.Propertyname,
                                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                                }

                                if (props != null)
                                {
                                    var value = props.GetValue(item)?.ToString() ?? "";

                                    var cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);
                                    cell.SetCellValue(value);
                                }
                            }
                        }
                    }

                }
                // autosize the column based on the value
                var headerRow = targetSheet.GetRow(headerRowIndex);
                if (headerRow != null)
                {
                    for (int col = 0; col < headerRow.LastCellNum; col++)
                    {

                        targetSheet.AutoSizeColumn(col);
                    }
                }

                targetSheetIndex++;
            }

            if (sheets.Any(s => s.TabName.Equals("cBOM", StringComparison.OrdinalIgnoreCase)))
            {
                    workbook.RemoveSheetAt(templateSheetIndex);
            }



            //Ensure final sheet order matches the sheets list
            for (int i = 0; i < sheets.Count; i++)
            {
                //sheets[i].TabName = sheets[i].TabName == "Sheet2" ? "cBOM" : sheets[i].TabName;
                workbook.SetSheetOrder(sheets[i].TabName, templateSheetIndex - i);
            }
            workbook.SetActiveSheet(0);
            workbook.SetSelectedTab(0);


            using var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            byte[] newfileBytes = memoryStream.ToArray();

            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = newfileBytes,
                FileName = fileName
            };
        }

        #endregion

        private ICellStyle BorderStyle(XSSFWorkbook workbook)
        {
            ICellStyle borderStyle = null;
            try
            {
                borderStyle = workbook.CreateCellStyle();
                borderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                borderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                borderStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                borderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                return borderStyle;
            }
            catch
            {

                return borderStyle;
            }
        }

        private void KeepFirstFYBlock(ISheet sheet, int fyHeaderRow = 0, int subHeaderRow = 0)
        {
            IRow headerRow = sheet.GetRow(fyHeaderRow);
            if (headerRow == null) return;

            int keepStart = 0;
            int keepEnd = 25;

            for (int i = sheet.NumMergedRegions - 1; i >= 0; i--)
            {
                var reg = sheet.GetMergedRegion(i);
                if (reg.FirstRow == fyHeaderRow)
                {
                    var cell = sheet.GetRow(reg.FirstRow)?.GetCell(reg.FirstColumn);
                    if (cell != null && cell.ToString().StartsWith("FY", StringComparison.OrdinalIgnoreCase))
                    {
                        if (reg.LastColumn < keepStart || reg.FirstColumn > keepEnd)
                        {
                            // Remove merged region
                            sheet.RemoveMergedRegion(i);

                            // Clear all columns under this FY block (header + children)
                            for (int r = 0; r <= sheet.LastRowNum; r++)
                            {
                                var row = sheet.GetRow(r);
                                if (row == null) continue;

                                for (int c = reg.FirstColumn; c <= reg.LastColumn; c++)
                                {
                                    var targetCell = row.GetCell(c);
                                    if (targetCell != null)
                                        row.RemoveCell(targetCell);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DeleteSheet(XSSFWorkbook workbook, int deletedSheetIndex)
        {
            var deletedSheet = workbook.GetSheetAt(deletedSheetIndex);
            if (deletedSheet.SheetName == "Export")
            {
                workbook.RemoveAt(deletedSheetIndex);
            }
        }

        private void CopyLogoToNewSheet(XSSFWorkbook workbook, ISheet sourceSheet, ISheet targetSheet, string logoPath)
        {
            var srcSheet = (XSSFDrawing)sourceSheet.CreateDrawingPatriarch();
            var tgtSheet = (XSSFDrawing)targetSheet.CreateDrawingPatriarch();

            byte[] logoBytes = File.ReadAllBytes(logoPath);
            int pictureIndex = workbook.AddPicture(logoBytes, PictureType.PNG);

            // Anchor at the found cell
            var anchor = new XSSFClientAnchor
            {
                Col1 = 0,
                Row1 = 0,
                Col2 = 1,
                Row2 = 1
            };

            var picture = tgtSheet.CreatePicture(anchor, pictureIndex);


        }

        private ISheet CreateModelSheet(XSSFWorkbook workbook, string sheetName, int targetSheetIndex, string logoPath, ISheet sheet = null)
        {
            ISheet targetSheet = workbook.CreateSheet(sheetName);
            workbook.SetSheetOrder(targetSheet.SheetName, targetSheetIndex);

            int firstRow = sheet.FirstRowNum;
            int lastRow = sheet.LastRowNum;

            // Copy rows
            for (int r = firstRow; r <= lastRow; r++)
            {
                IRow sourceRow = sheet.GetRow(r);
                if (sourceRow == null) continue;

                IRow targetRow = targetSheet.CreateRow(r);
                targetRow.Height = sourceRow.Height;

                for (int c = 0; c < sourceRow.LastCellNum; c++)
                {
                    ICell sourceCell = sourceRow.GetCell(c);
                    if (sourceCell == null) continue;

                    ICell targetCell = targetRow.CreateCell(c);

                    // Copy value
                    switch (sourceCell.CellType)
                    {
                        case NPOI.SS.UserModel.CellType.String:
                            targetCell.SetCellValue(sourceCell.StringCellValue);
                            break;
                        case NPOI.SS.UserModel.CellType.Numeric:
                            targetCell.SetCellValue(sourceCell.NumericCellValue);
                            break;
                        case NPOI.SS.UserModel.CellType.Boolean:
                            targetCell.SetCellValue(sourceCell.BooleanCellValue);
                            break;
                        case NPOI.SS.UserModel.CellType.Formula:
                            targetCell.SetCellFormula(sourceCell.CellFormula);
                            break;
                        default:
                            targetCell.SetCellValue(sourceCell.ToString());
                            break;
                    }

                    // Copy style
                    if (sourceCell.CellStyle != null)
                    {
                        ICellStyle newStyle = workbook.CreateCellStyle();
                        newStyle.CloneStyleFrom(sourceCell.CellStyle);
                        targetCell.CellStyle = newStyle;
                    }
                }
            }

            // Copy column widths
            for (int c = 0; c <= sheet.GetRow(firstRow).LastCellNum; c++)
            {
                targetSheet.SetColumnWidth(c, sheet.GetColumnWidth(c));
            }

            // Copy merged regions
            for (int i = 0; i < sheet.NumMergedRegions; i++)
            {
                var region = sheet.GetMergedRegion(i);
                targetSheet.AddMergedRegion(region);
            }
            CopyLogoToNewSheet(workbook, sheet, targetSheet, logoPath);
            targetSheet.CreateFreezePane(9, 4, 9, 4);
            return targetSheet;
        }

        private string GetFinancialYear(string year)
        {

            string startYear = Convert.ToInt32(year).ToString("0000").Substring(2, 2);   // e.g., 23

            if (startYear == "00")
            {
                startYear = (Convert.ToInt32(year) / 100).ToString("D2");
            }
            string endYear = (Convert.ToInt32(year) + 1).ToString("0000").Substring(2, 2);   // e.g., 24

            return $"{startYear}/{endYear}";
        }

        private void WriteCell<T>(IRow row, int colIndex, T item, string propertyName)
        {
            var prop = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                var value = prop.GetValue(item)?.ToString() ?? "";
                ICell cell = row.GetCell(colIndex);

                if (cell == null)
                {
                    cell = row.CreateCell(colIndex);
                }
                else
                {
                    var style = cell.CellStyle;
                    cell = row.CreateCell(colIndex);
                    cell.CellStyle = style;
                }

                cell.SetCellValue(value);
            }
        }
        public ExportResult GetExcelBasedOnConfiguration<T>(List<ExportSheet> sheets, string fileName, string filePath, List<T> data)
        {

            using XLWorkbook wbk = new XLWorkbook();
            using MemoryStream streamFile = new MemoryStream();

            var headerRow = 11;
            var columnStart = 4;


            using (XLWorkbook workbook = new XLWorkbook(filePath))
            {
                var workSheet = workbook.Worksheet(1);

                var rows = workSheet.Row(headerRow);
                var sheetName = workSheet.Name;
                var headerCells = workSheet.Row(headerRow).CellsUsed();
                var headers = headerCells
                              .Where(c => c.Address.ColumnNumber >= columnStart)
                              .ToDictionary(
                                  c => c.Address.ColumnNumber,
                                  c => c.HasFormula
                                  ? c.CachedValue?.ToString() ?? ""
                        : c.GetValue<string>());



                int startWriteRow = headerRow + 1;

                for (int i = 0; i < data.Count; i++)
                {
                    var dataRow = data[i];

                    foreach (var kv in headers)
                    {
                        var colIndex = kv.Key;
                        var headerName = kv.Value;

                        if (dataRow != null)
                        {
                            workSheet.Cell(startWriteRow + i, colIndex).Value = headerName;
                        }
                    }
                }

                workbook.SaveAs(streamFile);
            }

            var content = streamFile.ToArray();
            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = content,
                FileName = fileName
            };
        }


        #endregion
        string FirstToUpper(string s)
        {
            if (s != string.Empty && char.IsLower(s[0]))
            {
                s = char.ToUpper(s[0]) + s.Substring(1);
            }
            return s;
        }

        private string NormalizeKey(string input)
        {
            return input?
                .Trim()
                .ToLowerInvariant()
                .Replace(" ", "")
                .Replace("\r", "")
                .Replace("\n", "");
        }

        public ExportResult GetExcelForLcmPassthroughSoftware<T>(List<ExportSheet> sheets, string fileName, CustomGridRender<T> dataRender)
        {
            using XLWorkbook wbk = new XLWorkbook();
            using MemoryStream streamFile = new MemoryStream();
            //Worksheet

            foreach (ExportSheet sheet in sheets)
            {
                const int headerRow = 1;
                int dataRow = 2;
                IXLWorksheet worksheet;
                if (string.IsNullOrEmpty(sheet.TabName))
                {
                    worksheet = wbk.Worksheets.Add("Export");
                    sheet.TabName = string.Empty;
                }
                else
                    worksheet = wbk.Worksheets.Add(sheet.TabName);

                if (sheet.Data.Count > 0 || dataRender.Render.Any(x => x.Show))
                {
                    var typeOfData = sheet.Data.Count > 0 ? sheet.Data[0].GetType() : new PassThroughLcmSoftwareDtoGrid().GetType();
                    var headingsBase = sheet.Data.Count > 0 ? typeOfData.BaseType.GetProperties() : typeOfData.GetProperties();
                    var headingsInherited = typeOfData.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    var headings = headingsBase.Concat(headingsInherited);
                    var ignoreColumn = false;
                    var exportableColumn = false;
                    var nonExportableColumn = false;

                    var initHeaderColumn = 0;

                    var render = dataRender.Render.Where(x => x.Show && x.Tab == sheet.TabName).OrderBy(x => x.Order).ToList();

                    Dictionary<int, PropertyInfo> propertyInfos = new Dictionary<int, PropertyInfo>();
                    Dictionary<int, PropertyInfo> _copypropertyInfos = new Dictionary<int, PropertyInfo>();
                    foreach (PropertyInfo headerInfo in headings)
                    {
                        var name = FirstToLower(headerInfo.Name);
                        var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name && y.Tab == sheet.TabName).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                        ignoreColumn = Attribute.IsDefined(headerInfo, typeof(IgnoreGridAttribute));
                        exportableColumn = Attribute.IsDefined(headerInfo, typeof(ExportableAttribute));
                        nonExportableColumn = Attribute.IsDefined(headerInfo, typeof(NonExportableAttribute));
                        if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                        {
                            if (!propertyInfos.ContainsKey(headerCustomProperties.Order))
                                propertyInfos.Add(headerCustomProperties.Order, headerInfo);

                        }
                        else if (exportableColumn && headerCustomProperties != null)
                        {
                            if (!propertyInfos.ContainsKey(headerCustomProperties.Order))
                                propertyInfos.Add(headerCustomProperties.Order, headerInfo);
                        }
                    }
                    propertyInfos = propertyInfos.OrderBy(p => p.Key).ToDictionary(p => p.Key, k => k.Value);
                    int count = 1;
                    foreach (var item in propertyInfos)
                    {
                        if (!_copypropertyInfos.ContainsKey(count))
                            _copypropertyInfos.Add(count, item.Value);
                        count++;
                    }
                    foreach (var headerInfo in _copypropertyInfos)
                    {
                        var name = FirstToLower(headerInfo.Value.Name);
                        var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name && y.Tab == sheet.TabName).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                        ignoreColumn = Attribute.IsDefined(headerInfo.Value, typeof(IgnoreGridAttribute));
                        exportableColumn = Attribute.IsDefined(headerInfo.Value, typeof(ExportableAttribute));
                        nonExportableColumn = Attribute.IsDefined(headerInfo.Value, typeof(NonExportableAttribute));
                        if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                        {
                            //Format the header
                            var headerColumnName = headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Value.Name : headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            var headerBackgroundColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerFontColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Value = headerColumnName;
                            initHeaderColumn++;
                        }
                        else if (exportableColumn && headerCustomProperties != null)
                        {
                            var headerColumnName = headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Value.Name : headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            var headerBackgroundColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerFontColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Value = headerColumnName;
                            initHeaderColumn++;
                        }
                    }
                    //Rows
                    foreach (object item in sheet.Data)
                    {
                        var initColumn = 0;
                        var propertyInfoBase = item.GetType().BaseType.GetProperties();
                        var propertyInfoInherited = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        var propertyInfo = propertyInfoBase.Concat(propertyInfoInherited);

                        foreach (var info in _copypropertyInfos)
                        {
                            var name = FirstToLower(info.Value.Name);
                            var propertyCustomProperties = dataRender.Render.Where(y => y.PropertyName == name && y.Tab == sheet.TabName).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                            ignoreColumn = Attribute.IsDefined(info.Value, typeof(IgnoreGridAttribute));
                            exportableColumn = Attribute.IsDefined(info.Value, typeof(ExportableAttribute));
                            nonExportableColumn = Attribute.IsDefined(info.Value, typeof(NonExportableAttribute));
                            if (!ignoreColumn && !nonExportableColumn && propertyCustomProperties != null && propertyCustomProperties.Show)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.Value.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.Value.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.Value.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.Value.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.Value.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.Value.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, info.Key).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, info.Key).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            worksheet.Cell(dataRow, info.Key).Style.Alignment.SetWrapText(true);
                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;
                                            //  }

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.Value.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.Value.PropertyType))
                                {
                                    bool isDict = info.Value.PropertyType.IsGenericType && info.Value.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.Value.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, info.Key).Value = testo;
                                    }
                                }
                                else
                                {


                                    if (info.Value.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, info.Key).Value = (info.Value == null || (bool)info.Value.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.Value.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, info.Key).DataType =
                                            XLDataType.Text;

                                        var value = info.Value.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, info.Key).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.Value.PropertyType == typeof(string))
                                        {
                                            var text = info.Value.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }
                                                worksheet.Cell(dataRow, info.Key).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);
                                            }
                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);
                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, info.Key).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }
                            else if (exportableColumn && propertyCustomProperties != null)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.Value.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.Value.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.Value.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.Value.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.Value.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.Value.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, info.Key).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, info.Key).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;

                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.Value.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.Value.PropertyType))
                                {
                                    bool isDict = info.Value.PropertyType.IsGenericType && info.Value.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.Value.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, info.Key).Value = testo;
                                    }
                                }
                                else
                                {
                                    if (info.Value.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, info.Key).Value = (info.Value == null || (bool)info.Value.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.Value.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, info.Key).DataType =
                                            XLDataType.Text;

                                        var value = info.Value.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, info.Key).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.Value.PropertyType == typeof(string))
                                        {
                                            var text = info.Value.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }

                                                worksheet.Cell(dataRow, info.Key).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                            }


                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, info.Key).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }
                        }

                        dataRow++;
                    }

                    worksheet.Columns().AdjustToContents(1.0, 50.0);
                    worksheet.RangeUsed().SetAutoFilter();
                }
            }
            wbk.SaveAs(streamFile);
            var content = streamFile.ToArray();
            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = content,
                FileName = fileName
            };

        }
        public ExportResult GetExcelForLcmPassthroughHardware<T>(List<ExportSheet> sheets, string fileName, CustomGridRender<T> dataRender)
        {
            using XLWorkbook wbk = new XLWorkbook();
            using MemoryStream streamFile = new MemoryStream();
            //Worksheet

            foreach (ExportSheet sheet in sheets)
            {
                const int headerRow = 1;
                int dataRow = 2;
                IXLWorksheet worksheet;
                if (string.IsNullOrEmpty(sheet.TabName))
                {
                    worksheet = wbk.Worksheets.Add("Export");
                    sheet.TabName = string.Empty;
                }
                else
                    worksheet = wbk.Worksheets.Add(sheet.TabName);

                if (sheet.Data.Count > 0 || dataRender.Render.Any(x => x.Show))
                {
                    var typeOfData = sheet.Data.Count > 0 ? sheet.Data[0].GetType() : new PassThroughLcmHardwareDtoGrid().GetType();
                    var headingsBase = sheet.Data.Count > 0 ? typeOfData.BaseType.GetProperties() : typeOfData.GetProperties();
                    var headingsInherited = typeOfData.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    var headings = headingsBase.Concat(headingsInherited);
                    var ignoreColumn = false;
                    var exportableColumn = false;
                    var nonExportableColumn = false;

                    var initHeaderColumn = 0;

                    var render = dataRender.Render.Where(x => x.Show && x.Tab == sheet.TabName).OrderBy(x => x.Order).ToList();

                    Dictionary<int, PropertyInfo> propertyInfos = new Dictionary<int, PropertyInfo>();
                    Dictionary<int, PropertyInfo> _copypropertyInfos = new Dictionary<int, PropertyInfo>();
                    foreach (PropertyInfo headerInfo in headings)
                    {
                        var name = FirstToLower(headerInfo.Name);
                        var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name && y.Tab == sheet.TabName).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                        ignoreColumn = Attribute.IsDefined(headerInfo, typeof(IgnoreGridAttribute));
                        exportableColumn = Attribute.IsDefined(headerInfo, typeof(ExportableAttribute));
                        nonExportableColumn = Attribute.IsDefined(headerInfo, typeof(NonExportableAttribute));
                        if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                        {
                            if (!propertyInfos.ContainsKey(headerCustomProperties.Order))
                                propertyInfos.Add(headerCustomProperties.Order, headerInfo);

                        }
                        else if (exportableColumn && headerCustomProperties != null)
                        {
                            if (!propertyInfos.ContainsKey(headerCustomProperties.Order))
                                propertyInfos.Add(headerCustomProperties.Order, headerInfo);
                        }
                    }
                    propertyInfos = propertyInfos.OrderBy(p => p.Key).ToDictionary(p => p.Key, k => k.Value);
                    int count = 1;
                    foreach (var item in propertyInfos)
                    {
                        if (!_copypropertyInfos.ContainsKey(count))
                            _copypropertyInfos.Add(count, item.Value);
                        count++;
                    }
                    foreach (var headerInfo in _copypropertyInfos)
                    {
                        var name = FirstToLower(headerInfo.Value.Name);
                        var headerCustomProperties = dataRender.Render.Where(y => y.PropertyName == name && y.Tab == sheet.TabName).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                        ignoreColumn = Attribute.IsDefined(headerInfo.Value, typeof(IgnoreGridAttribute));
                        exportableColumn = Attribute.IsDefined(headerInfo.Value, typeof(ExportableAttribute));
                        nonExportableColumn = Attribute.IsDefined(headerInfo.Value, typeof(NonExportableAttribute));
                        if (!ignoreColumn && !nonExportableColumn && headerCustomProperties != null && headerCustomProperties.Show)
                        {
                            //Format the header
                            var headerColumnName = headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Value.Name : headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            var headerBackgroundColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerFontColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Value = headerColumnName;
                            initHeaderColumn++;
                        }
                        else if (exportableColumn && headerCustomProperties != null)
                        {
                            var headerColumnName = headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>() == null ? headerInfo.Value.Name : headerInfo.Value.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                            var headerBackgroundColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xe60000 : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().BackgroundColor;
                            var headerFontColor = headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>() == null ? 0xffffff : headerInfo.Value.GetCustomAttribute<HeaderColorAttribute>().FontColor;
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Font.SetFontColor(XLColor.FromArgb(headerFontColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(headerBackgroundColor));
                            worksheet.Cell(headerRow, headerInfo.Key).Value = headerColumnName;
                            initHeaderColumn++;
                        }
                    }
                    //Rows
                    foreach (object item in sheet.Data)
                    {
                        var initColumn = 0;
                        var propertyInfoBase = item.GetType().BaseType.GetProperties();
                        var propertyInfoInherited = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        var propertyInfo = propertyInfoBase.Concat(propertyInfoInherited);

                        foreach (var info in _copypropertyInfos)
                        {
                            var name = FirstToLower(info.Value.Name);
                            var propertyCustomProperties = dataRender.Render.Where(y => y.PropertyName == name && y.Tab == sheet.TabName).Select(x => new { x.Show, x.Order }).SingleOrDefault();
                            ignoreColumn = Attribute.IsDefined(info.Value, typeof(IgnoreGridAttribute));
                            exportableColumn = Attribute.IsDefined(info.Value, typeof(ExportableAttribute));
                            nonExportableColumn = Attribute.IsDefined(info.Value, typeof(NonExportableAttribute));
                            if (!ignoreColumn && !nonExportableColumn && propertyCustomProperties != null && propertyCustomProperties.Show)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.Value.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.Value.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.Value.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.Value.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.Value.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.Value.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, info.Key).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, info.Key).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            worksheet.Cell(dataRow, info.Key).Style.Alignment.SetWrapText(true);
                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;
                                            //  }

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.Value.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.Value.PropertyType))
                                {
                                    bool isDict = info.Value.PropertyType.IsGenericType && info.Value.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.Value.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, info.Key).Value = testo;
                                    }
                                }
                                else
                                {


                                    if (info.Value.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, info.Key).Value = (info.Value == null || (bool)info.Value.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.Value.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, info.Key).DataType =
                                            XLDataType.Text;

                                        var value = info.Value.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, info.Key).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.Value.PropertyType == typeof(string))
                                        {
                                            var text = info.Value.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }
                                                worksheet.Cell(dataRow, info.Key).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);
                                            }
                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);
                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, info.Key).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }
                            else if (exportableColumn && propertyCustomProperties != null)
                            {
                                //Format the cell
                                var cellBackgroundColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0 : info.Value.GetCustomAttribute<CellColorAttribute>().BackgroundColor;
                                var cellFontColor = info.Value.GetCustomAttribute<CellColorAttribute>() == null ? 0x000000 : info.Value.GetCustomAttribute<CellColorAttribute>().FontColor;
                                var cellFormatTypeInfo = info.Value.GetCustomAttribute<FormatAttribute>() == null ? ExportDataTypeEnum.Text.ToString() : info.Value.GetCustomAttribute<FormatAttribute>().FormatType;
                                var cellFormatInfo = info.Value.GetCustomAttribute<FormatAttribute>()?.Format;
                                var cellFormatClosetXmlInfo = info.Value.GetCustomAttribute<FormatClosetXmlAttribute>()?.Type;
                                if (cellBackgroundColor != 0)
                                    worksheet.Cell(dataRow, info.Key).Style.Fill.SetBackgroundColor(XLColor.FromArgb(cellBackgroundColor));

                                worksheet.Cell(dataRow, info.Key).Style.Font.SetFontColor(XLColor.FromArgb(cellFontColor));
                                worksheet.Cell(dataRow, info.Key).Style.Border.SetOutsideBorderColor(XLColor.Black);


                                if (cellFormatClosetXmlInfo == null)
                                {
                                    ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);

                                    switch (formatType)
                                    {
                                        case ExportDataTypeEnum.Text:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;

                                            break;
                                        case ExportDataTypeEnum.Date:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.DateTime;

                                            break;
                                        case ExportDataTypeEnum.Number:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Number;

                                            break;
                                        case ExportDataTypeEnum.Boolean:
                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Boolean;
                                            break;
                                        case ExportDataTypeEnum.DateText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.DateFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        case ExportDataTypeEnum.NumberText:
                                            if (!string.IsNullOrEmpty(cellFormatInfo))
                                                worksheet.Cell(dataRow, info.Key).Style.NumberFormat.Format = cellFormatInfo;

                                            worksheet.Cell(dataRow, info.Key).DataType = XLDataType.Text;
                                            break;
                                        default:
                                            break;

                                    }
                                }

                                // convenzione per i dictionary poiche sono gli oggetti che usiamo per le collezioni la maggiorparte delle volte facciamo che restino quelli, cosi il codice dynamic non si rompe
                                if (info.Value.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(info.Value.PropertyType))
                                {
                                    bool isDict = info.Value.PropertyType.IsGenericType && info.Value.PropertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>);
                                    if (isDict)
                                    {
                                        var testo = "";
                                        dynamic data = info.Value.GetValue(item, null);
                                        if (data != null && data.Values != null)
                                            foreach (var x in data.Values)
                                            {
                                                var removedHtml = x?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                removedHtml = removedHtml.Replace("</b>", "");



                                                testo += removedHtml + " \n";
                                            }
                                        worksheet.Cell(dataRow, info.Key).Value = testo;
                                    }
                                }
                                else
                                {
                                    if (info.Value.PropertyType == typeof(bool))
                                    {
                                        worksheet.Cell(dataRow, info.Key).Value = (info.Value == null || (bool)info.Value.GetValue(item, null)) ? "Yes" : "No";
                                    }
                                    else if (info.Value.PropertyType == typeof(Nullable<bool>))
                                    {
                                        worksheet.Cell(dataRow, info.Key).DataType =
                                            XLDataType.Text;

                                        var value = info.Value.GetValue(item, null)?.ToString();


                                        worksheet.Cell(dataRow, info.Key).Value =
                                            value == null ? "Unspecified" : value == "True" ? "Yes" : "No";
                                    }
                                    else
                                    {

                                        ExportDataTypeEnum formatType = (ExportDataTypeEnum)System.Enum.Parse(typeof(ExportDataTypeEnum), cellFormatTypeInfo);
                                        if (formatType == ExportDataTypeEnum.Text || info.Value.PropertyType == typeof(string))
                                        {
                                            var text = info.Value.GetValue(item, null);
                                            if (text != null)
                                            {
                                                var removedHtml = text?.ToString();


                                                while (removedHtml.Contains("<b class=\"text-lowercase\">") || removedHtml.Contains("</b>") || removedHtml.Contains("<b class=\"text-lowercase\" >"))
                                                {
                                                    removedHtml = text?.ToString()?.Replace("<b class=\"text-lowercase\">", "");
                                                    removedHtml = removedHtml.Replace("<b class=\"text-lowercase\" >", "");
                                                    removedHtml = removedHtml?.Replace("</b>", "");
                                                }

                                                worksheet.Cell(dataRow, info.Key).SetValue<string>(Convert.ToString(removedHtml));
                                            }
                                            else
                                            {
                                                worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                            }


                                        }
                                        else
                                        {
                                            worksheet.Cell(dataRow, info.Key).Value = info.Value.GetValue(item, null);

                                        }
                                    }
                                }
                                if (cellFormatClosetXmlInfo != null)
                                {
                                    worksheet.Cell(dataRow, info.Key).DataType = cellFormatClosetXmlInfo.Value;
                                }


                                initColumn++;
                            }
                        }

                        dataRow++;
                    }

                    worksheet.Columns().AdjustToContents(1.0, 50.0);
                    worksheet.RangeUsed().SetAutoFilter();
                }
            }
            wbk.SaveAs(streamFile);
            var content = streamFile.ToArray();
            return new ExportResult()
            {
                ContentType = ExcelContentType,
                FileInByteArray = content,
                FileName = fileName
            };

        }




    }

}
