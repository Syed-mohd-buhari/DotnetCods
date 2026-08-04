using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using CAM.Entities.Models.Engine;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class DynamicReportMapper
    {
        public static DynamicReports Get(Dynamicreports model)
        {
            if (model == null)
                return null;
            return new DynamicReports()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                ReportName = model.Reportname,
                DynamicReportsId = model.Dynamicreportsid,
                JsongridCustomizationData = model.Jsongridcustomizationdata,
                UserId = model.Userid,
                Published=model.Published,
                Isscheduled=model.Isscheduled,
                OpcoId = model.Opcoid,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                Users = ApplicationUserMapper.GetApplicationUserMapper(model.User),
                ExportFileFormat = model.Exportfileformat,
                ExportFilePath = model.Exportfilepath,
                ExportType = model.Exporttype,
                ScheduledDate = model.Scheduleddate,
                ScheduledDayInWeek = model.Scheduleddayinweek,
                ScheduledType = model.Scheduledtype,
                IsTestNodeRequired=model.Istestnoderequired,
            };
        }

        public static Dynamicreports Set(DynamicReports model)
        {
            return new Dynamicreports()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Reportname = model.ReportName,
                Dynamicreportsid = model.DynamicReportsId,
                Jsongridcustomizationdata = model.JsongridCustomizationData,
                Userid = model.UserId,
                Published = model.Published,
                Isscheduled = model.Isscheduled,
                Opcoid = model.OpcoId,
                Exportfileformat = model.ExportFileFormat,
                Exportfilepath= model.ExportFilePath,
                Exporttype = model.ExportType,
                Scheduleddate= model.ScheduledDate,
                Scheduleddayinweek = model.ScheduledDayInWeek,
                Scheduledtype = model.ScheduledType,
                Istestnoderequired = model.IsTestNodeRequired,
            };
        }


    }
}
