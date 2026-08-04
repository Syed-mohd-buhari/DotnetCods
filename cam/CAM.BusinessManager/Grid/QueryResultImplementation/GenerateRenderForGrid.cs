using System;
using System.Linq;
using System.Reflection;
using CAM.DataAttributes.Grid;
using CAM.Infrastucture.Enums;
using CAM.Infrastucture.QueryResult;

namespace CAM.BusinessManager.Grid.QueryResultImplementation
{
    public class GenerateRenderForGrid<T> : IGenerateRender
    {
        private GridCustomColumnManager _manager;

        public GenerateRenderForGrid(GridCustomColumnManager manager)
        {
            _manager = manager;
        }

        public CustomGridRender<T> GenerateRender<T>()
        {
            var rtn = new CustomGridRender<T>();
            var properties = typeof(T).GetProperties();
            rtn.Render = _manager.GetMapping(typeof(T).Name);
            if (rtn.Render.Any()) return rtn;
            for (var i = 0; i < properties.Length; i++)
            {
                var propertyInfo = properties[i];

                if (!Attribute.IsDefined(propertyInfo, typeof(IgnoreGridAttribute)))
                {
                    if (!Attribute.IsDefined(propertyInfo, typeof(TabGridAttribute)) && !Attribute.IsDefined(propertyInfo, typeof(ExportableAttribute)))
                    {
                        rtn.Render.Add(new RenderDetail()
                        {
                            PropertyName = propertyInfo.GetCustomAttribute<DisplayNameGridAttribute>()?.DisplayName ??
                                           FirtToLower(propertyInfo.Name),
                            Order = propertyInfo.GetCustomAttribute<OrderGridAttribute>()?.Order ?? i,
                            Show = Attribute.IsDefined(propertyInfo, typeof(DefaultAttribute)) ? true : false,
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
                            Ignore =false,


                        }) ;
                    }
                    else if (Attribute.IsDefined(propertyInfo, typeof(ExportableAttribute)))
                    {
                        rtn.Render.Add(new RenderDetail()
                        {
                            PropertyName = propertyInfo.GetCustomAttribute<DisplayNameGridAttribute>()?.DisplayName ??
                                          FirtToLower(propertyInfo.Name),
                            Order = propertyInfo.GetCustomAttribute<OrderGridAttribute>()?.Order ?? i,
                            Show = false,
                            Archive = false,
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
                            Ignore = true,


                        }) ;
                    }
                    else
                    {
                        var tab = propertyInfo.GetCustomAttribute<TabGridAttribute>()?.TabName ?? "";
                        var test = tab.Split(",");
                        foreach (var x in test)
                        {
                            rtn.Render.Add(new RenderDetail()
                            {
                                PropertyName = propertyInfo.GetCustomAttribute<DisplayNameGridAttribute>()?.DisplayName ??
                                               FirtToLower(propertyInfo.Name),
                                Order = propertyInfo.GetCustomAttribute<OrderGridAttribute>()?.Order ?? i,
                                Show = Attribute.IsDefined(propertyInfo, typeof(DefaultAttribute)) ? true : false,
                                Archive = Attribute.IsDefined(propertyInfo, typeof(ArchiveAttribute)) ? true : false,
                                //Type = Attribute.IsDefined(propertyInfo, typeof(DateRangeGridAttribute))
                                //    ? GridFilterType.DateRangeFilter
                                //    : GridFilterType.CheckBoxFilter,
                                Type = Attribute.IsDefined(propertyInfo, typeof(DateRangeGridAttribute))
                                    ? GridFilterType.DateRangeFilter
                                    : (Attribute.IsDefined(propertyInfo, typeof(MailTo))
                                        ? GridFilterType.MailTo : Attribute.IsDefined(propertyInfo, typeof(DateRangeGridStringAttribute))
                                            ? GridFilterType.DateRangeFilterString
                                        : GridFilterType.CheckBoxFilter),

                                Tab = x,
                                ColorHeader = propertyInfo.GetCustomAttribute<ColorGridAttribute>()?.Color ?? "",
                                Ignore =true,

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
