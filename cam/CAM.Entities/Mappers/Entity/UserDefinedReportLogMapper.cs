using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public class UserDefinedReportLogMapper
    {
        public static UserDefinedReportsLogs GetUserDefinedReportsLogsMapper(Userdefinedreportslogs model)
        {
            if (model == null)
                return null;
            var result = new UserDefinedReportsLogs()
            {
                UserDefinedReportsLogId = model.Userdefinedreportslogid,
                ReportName = model.Reportname,
                ReportStatus = model.Reportstatus,
                ReportDownloadedPath = model.Reportdownloadedpath,
                ReportFormat = model.Reportformat,                 
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,   
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation)               

            };
            return result;
        }

        public static Userdefinedreportslogs SetUserDefinedReportsLogs(UserDefinedReportsLogs model)
        {
            return new Userdefinedreportslogs()
            {
                Userdefinedreportslogid = model.UserDefinedReportsLogId,
                Reportformat = model.ReportFormat,
                Reportname = model.ReportName,
                Reportstatus = model.ReportStatus,
                Reportdownloadedpath = model.ReportDownloadedPath,                 
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                
            };
        }
    }
}
