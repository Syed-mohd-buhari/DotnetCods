using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class ReportSchedulerMapper
    {
        public static ReportScheduler Get(Reportscheduler model)
        {
            if (model == null)
                return null;
            var result = new ReportScheduler()
            {
                ReportSchedulerId = model.Reportschedulerid,
                ReportName = model.Reportname,
                IsScheduled = model.Isscheduled,
                OpcoId = model.Opcoid,
                ReportVertical = model.Reportvertical,
                ExportFilePath = model.Exportfilepath,
                ExportFileFormat = model.Exportfileformat,
                ScheduledDate = model.Scheduleddate,
                ScheduledDayinWeek = model.Scheduleddayinweek,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),                              
            };
            return result;
        }

        public static Reportscheduler Set(ReportScheduler model)
        {
            if (model == null)
                return null;
            var result = new Reportscheduler()
            {
                Reportschedulerid = model.ReportSchedulerId,
                Reportname = model.ReportName,
                Isscheduled = model.IsScheduled,
                Opcoid = model.OpcoId,
                Reportvertical = model.ReportVertical,
                Exportfilepath = model.ExportFilePath,
                Exportfileformat = model.ExportFileFormat,
                Scheduleddate = model.ScheduledDate,
                Scheduleddayinweek = model.ScheduledDayinWeek,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
            };
            return result;
        }
    }
}
