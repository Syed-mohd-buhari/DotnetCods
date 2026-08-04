using System;
using System.Linq;
using System.Reflection;
using CAM.BusinessManager.GenericReports;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataAttributes.Grid;
using CAM.Infrastucture.Enums;
using CAM.Infrastucture.QueryResult;
using Newtonsoft.Json.Schema;

namespace CAM.BusinessManager.Grid.QueryResultImplementation
{
    public class GenericReportRender<T> : IGenericReportInfrastructureGrid
    {
        private GenericReportManager _manager;
        private readonly IRepositoryWrapper _repositoryWrapper;
        public GenericReportRender(GenericReportManager manager, IRepositoryWrapper repositoryWrapper)
        {
            _manager = manager;
            _repositoryWrapper = repositoryWrapper;
        }

        public GenericReportInfrastructureGrid<T> GenerateReport<T>(long id)
        { 
            var rtn = new GenericReportInfrastructureGrid<T>();
            var properties = typeof(T).GetProperties();
            var dynamicEntity = _repositoryWrapper.GenericReportRepository.FindByCondition(x => x.Dynamicreportsid == id).FirstOrDefault();
            if (dynamicEntity != null)
            {
                rtn.IsTestNodeRequired = dynamicEntity.Istestnoderequired ?? false;
                rtn.ReportName = dynamicEntity.Reportname;
                rtn.DynamicReportId = id;
                rtn.Published = (bool)dynamicEntity.Published;
                rtn.Render = _manager.GetMapping(id);
                rtn.OpcoId = (dynamicEntity.Opcoid != null) ? dynamicEntity.Opcoid.Split(",")?.Select(int.Parse)?.ToList() : null;
                rtn.ExportFileFormat = dynamicEntity.Exportfileformat;
                rtn.ExportFilePath = dynamicEntity.Exportfilepath;
                rtn.ScheduledDate = dynamicEntity.Scheduleddate;
                rtn.ScheduledDayInWeek = dynamicEntity.Scheduleddayinweek;
                rtn.ScheduledType = dynamicEntity.Scheduledtype;
                rtn.ExportType = dynamicEntity.Exporttype;
                rtn.isExportReport = (!string.IsNullOrEmpty(Convert.ToString(dynamicEntity.Scheduleddate)) ||
                    !string.IsNullOrEmpty(Convert.ToString(dynamicEntity.Scheduleddayinweek))) ? true : false;
                rtn.isScheduled = Convert.ToBoolean ( dynamicEntity.Isscheduled);
                if (rtn.Render != null)
                {
                    foreach (GenericReportInfrastructureDto render in rtn.Render)
                    {
                        if (string.IsNullOrEmpty(render.UpdatedPropertyName) )
                        {
                            
                            if (render.TableName.Trim()  == "emptyGrid")
                                render.UpdatedPropertyName = null;
                            else
                                render.UpdatedPropertyName = render.PropertyName;

                           
                        }
                    }
                }
            }
            else
            {
                rtn.Render = _manager.GetMapping(id);
            }

            if (rtn.Render.Any()) return rtn;

            for (var i = 0; i < properties.Length; i++)
            {
                var propertyInfo = properties[i];

                if (!Attribute.IsDefined(propertyInfo, typeof(IgnoreGridAttribute)))
                {
                    if (!Attribute.IsDefined(propertyInfo, typeof(TabGridAttribute)) && !Attribute.IsDefined(propertyInfo, typeof(ExportableAttribute)))
                    {
                        rtn.Render.Add(new GenericReportInfrastructureDto()
                        {
                            PropertyName = propertyInfo.GetCustomAttribute<DisplayNameGridAttribute>()?.DisplayName ??
                                           FirtToLower(propertyInfo.Name),
                            Archive = Attribute.IsDefined(propertyInfo, typeof(ArchiveAttribute)) ? true : false,
                            Type = (Attribute.IsDefined(propertyInfo, typeof(DateRangeGridAttribute))
                                ? GridFilterType.DateRangeFilter
                                : Attribute.IsDefined(propertyInfo, typeof(MailTo))
                                    ? GridFilterType.MailTo
                                    : Attribute.IsDefined(propertyInfo, typeof(TextTooltip))
                                        ? GridFilterType.TextTooltip
                                        : Attribute.IsDefined(propertyInfo, typeof(DateRangeGridStringAttribute))
                                            ? GridFilterType.DateRangeFilterString
                                        : GridFilterType.CheckBoxFilter),
                            Tab = "",
                            Ignore = false,

                        });
                    }
                    else if (Attribute.IsDefined(propertyInfo, typeof(ExportableAttribute)))
                    {
                        rtn.Render.Add(new GenericReportInfrastructureDto()
                        {
                            PropertyName = propertyInfo.GetCustomAttribute<DisplayNameGridAttribute>()?.DisplayName ??
                                            FirtToLower(propertyInfo.Name),
                            Archive = Attribute.IsDefined(propertyInfo, typeof(ArchiveAttribute)) ? true : false,
                            Type = (Attribute.IsDefined(propertyInfo, typeof(DateRangeGridAttribute))
                                ? GridFilterType.DateRangeFilter
                                : Attribute.IsDefined(propertyInfo, typeof(MailTo))
                                    ? GridFilterType.MailTo
                                    : Attribute.IsDefined(propertyInfo, typeof(TextTooltip))
                                        ? GridFilterType.TextTooltip
                                        : Attribute.IsDefined(propertyInfo, typeof(DateRangeGridStringAttribute))
                                            ? GridFilterType.DateRangeFilterString
                                        : GridFilterType.CheckBoxFilter),
                            Tab = "",
                            ColorHeader = propertyInfo.GetCustomAttribute<ColorGridAttribute>()?.Color ?? "",
                            Ignore = false,
                        });
                    }
                    else
                    {
                        var tab = propertyInfo.GetCustomAttribute<TabGridAttribute>()?.TabName ?? "";
                        var test = tab.Split(",");
                        foreach (var x in test)
                        {
                            rtn.Render.Add(new GenericReportInfrastructureDto()
                            {
                                PropertyName = propertyInfo.GetCustomAttribute<DisplayNameGridAttribute>()?.DisplayName ??
                                               FirtToLower(propertyInfo.Name),
                                Archive = Attribute.IsDefined(propertyInfo, typeof(ArchiveAttribute)) ? true : false,
                                Type = (Attribute.IsDefined(propertyInfo, typeof(DateRangeGridAttribute))
                                ? GridFilterType.DateRangeFilter
                                : Attribute.IsDefined(propertyInfo, typeof(MailTo))
                                    ? GridFilterType.MailTo
                                    : Attribute.IsDefined(propertyInfo, typeof(TextTooltip))
                                        ? GridFilterType.TextTooltip
                                        : Attribute.IsDefined(propertyInfo, typeof(DateRangeGridStringAttribute))
                                            ? GridFilterType.DateRangeFilterString
                                        : GridFilterType.CheckBoxFilter),
                                Tab = "",
                                ColorHeader = propertyInfo.GetCustomAttribute<ColorGridAttribute>()?.Color ?? "",
                                Ignore = false,

                            });
                        }
                    }

                }
            }
            return rtn;
        }


        string FirtToLower(string s)
        {
            if (s != string.Empty && char.IsUpper(s[0]))
            {
                s = char.ToLower(s[0]) + s.Substring(1);
            }
            return s;
        }


    }
}
