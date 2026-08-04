using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.VBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Vbom
{
    public class VnfVmCapacityMapper
    {
        public static VnfVmCapacity GetVnfVmCapacity(Vnfvmcapacity model, bool Include = true)
        {
            if (model == null)
                return null;
            var result = new VnfVmCapacity()
            {
                VnfVmCapacityId = model.Vnfvmcapacityid,
                VnfInfoId = model.Vnfinfoid,
                FinancialYear = model.Financialyear,
                VnfCpuPerVm = model.Vnfcpupervm,
                RxTxCpuCount = model.Rxtxcpucount,
                RamPerVm = model.Rampervm,
                DataDisk = model.Datadisk,
                OsDisk = model.Osdisk,
                IopsRunning = model.Iopsrunning,
                IopsLoading = model.Iopsloading,
                VmWorkLoadDistribution = model.Vmworkloaddistribution,
                NorthDouthBoundBandWidth = model.Northsouthboundbandwidth,
                EastWestBoundBandWidth = model.Eastwestboundbandwidth,
                OtherRequirements = model.Otherrequirements,
                BackupRequired = model.Backuprequired,
                ProbIngRequired = model.Probingrequired,
                FinancialVersion = model.Financialversion,
               NoOfVmsPerType = model.Noofvmspertype,
               NoOfVnfInstances = model.Noofvnfinstances,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };
            return result;
        }

        public static Vnfvmcapacity SetVnfVmCapacity(VnfVmCapacity model)
        {
            if (model == null)
                return null;
            var result = new Vnfvmcapacity()
            {
                Vnfvmcapacityid = model.VnfVmCapacityId,
                Vnfinfoid = model.VnfInfoId,
                Financialyear = model.FinancialYear,
                Vnfcpupervm = model.VnfCpuPerVm,
                Rxtxcpucount = model.RxTxCpuCount,
                Rampervm = model.RamPerVm,
                Datadisk = model.DataDisk,
                Osdisk = model.OsDisk,
                Iopsrunning = model.IopsRunning,
                Iopsloading = model.IopsLoading,
                Vmworkloaddistribution = model.VmWorkLoadDistribution,
                Northsouthboundbandwidth = model.NorthDouthBoundBandWidth,
                Eastwestboundbandwidth = model.EastWestBoundBandWidth,
                Otherrequirements = model.OtherRequirements,
                Backuprequired = model.BackupRequired,
                Probingrequired = model.ProbIngRequired,
                Financialversion = model.FinancialVersion
            };
            return result;
        }
    }
}
