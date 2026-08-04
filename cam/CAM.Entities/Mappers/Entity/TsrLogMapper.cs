using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class TsrLogMapper
    {
        public static TsrLog Get(Tsrlogs model)
        {
            if (model == null)
                return null;
            var result = new TsrLog()
            {
                TsrLogId = model.Tsrlogid,
                TypeOfOperation = model.Typeofoperation,
                FileName = model.Filename,
                TotalRecord = model.Totalrecord,
                ProcessedRecord = model.Processedrecord,
                StartTime = model.Starttime,
                EndTime = model.Endtime,
                Status = model.Status,
                Domain = model.Domain,
                BatchIdentifier = model.Batchidentifier,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),                              
            };
            return result;
        }

        public static Tsrlogs Get(TsrLog model)
        {
            if (model == null)
                return null;
            var result = new Tsrlogs()
            {
                Tsrlogid = model.TsrLogId,
                Typeofoperation = model.TypeOfOperation,
                Filename = model.FileName,
                Totalrecord = model.TotalRecord,
                Processedrecord = model.ProcessedRecord,
                Starttime = model.StartTime,
                Endtime = model.EndTime,
                Status = model.Status,
                Domain = model.Domain,
                Batchidentifier = model.BatchIdentifier,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
            };
            return result;
        }
    }
}
