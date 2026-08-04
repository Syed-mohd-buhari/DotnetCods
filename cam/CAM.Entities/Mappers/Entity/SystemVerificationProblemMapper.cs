using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public static class SystemVerificationProblemMapper
    {
        public static SystemVerificationProblems Get (Systemverificationproblems model)
        {
            if (model == null)
                return null;
            var result = new SystemVerificationProblems()
            {
                SystemVerificationProblemId = model.Systemverificationproblemid,
                ProblemId = model.Problemid,
                OpcoId = model.Opcoid,
                SystemTypeId = model.Systemtypeid,
                EnvironmentId = model.Environmentid,
                DateFound = model.Datefound,
                ProblemCategoryId = model.Problemcategoryid,
                ProblemDescription = model.Problemdescription,
                MaintenanceReference = model.Maintenancereference,
                Severity = model.Severity,
                StatusUrl = model.Status,
                Mitigation = model.Mitigation,
                SolutionDescription = model.Solutiondescription,
                PatchReference = model.Patchreference,
                ProductUpgradeReference = model.Productupgradereference,
                SuppleMental = model.Supplemental,
                SubNetwork = model.Subnetwork,
                VendorCsr = model.Vendorcsr,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                Environment =EnvironmentMapper.GetEnvironmentMapper(model.Environment),
                OpCo=OpCoMapper.GetOpCoMapper(model.Opco),
                Systemtype = SystemTypeMapper.GetSystemTypeMapper(model.Systemtype),
                ProblemCategory = ProblemCategoryMapper.Get(model.Problemcategory),
                SeverityEntity = SeverityMapper.Get(model.SeverityNavigation),
                TestReport = model.Testreport,
                StandardNir = model.Standardnir,
                EricssonSecReport = model.Ericssonsecreport,
                SwAndStEntries = model.Swandstentries,
                PenTestingReport = model.Pentestingreport,
            };

            return result;
        }

        public static Systemverificationproblems Set(SystemVerificationProblems model)
        {
            var result = new Systemverificationproblems()
            {
                Systemverificationproblemid = model.SystemVerificationProblemId,
                Problemid = model.ProblemId,
                Opcoid = model.OpcoId,
                Systemtypeid = model.SystemTypeId,
                Environmentid = model.EnvironmentId,
                Datefound = model.DateFound,
                Problemcategoryid = model.ProblemCategoryId,
                Problemdescription = model.ProblemDescription,
                Maintenancereference = model.MaintenanceReference,
                Severity = model.Severity,
                Status = model.StatusUrl,
                Mitigation = model.Mitigation,
                Solutiondescription = model.SolutionDescription,
                Patchreference = model.PatchReference,
                Productupgradereference = model.ProductUpgradeReference,
                Supplemental = model.SuppleMental,
                Vendorcsr = model.VendorCsr,
                Subnetwork = model.SubNetwork,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Testreport = model.TestReport,
                Standardnir = model.StandardNir,
                Ericssonsecreport = model.EricssonSecReport,
                Swandstentries = model.SwAndStEntries,
                Pentestingreport = model.PenTestingReport,
    };
            return result;
        }
    }
}
