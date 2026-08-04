using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.CBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cbom
{
    public class CnfCapacityMapper
    {
        public static CnfCapacity GetCnfCapacityMapper(Cnfcapacity model)
        {
            if (model == null)
                return null;
            var result = new CnfCapacity()
            {
                CnfCapacityId = model.Cnfcapacityid,
                CnfPodInfoId = model.Cnfpodinfoid,
                FinancialYear = model.Financialyear,
                FinancialVersion = model.Financialversion,
                VcpuLimitForPodType = model.Vcpulimitforpodtype,
                PcpuRequestForPodType = model.Pcpurequestforpodtype,
                MemRequestForPodType = model.Memrequestforpodtype,
                MemLimitForPodType = model.Memlimitforpodtype,
                VcpuRequestForPodType = model?.Vcpurequestforpodtype,
                NonPresistentStorageForProdType = model.Nonpresistentstorageforprodtype,
                NoOfCnfInstancesPerSite = model.Noofcnfinstancespersite,
                IsPresistentVolumesRequired = model.Ispresistentvolumesrequired,
                PersistentVolumNeaccessMode = model.Persistentvolumneaccessmode,
                PersistentStorageForPodType = model?.Persistentstorageforpodtype,
                StorageIopsForPodType = model.Storageiopsforpodtype,
                StoragerWorkloadDistribution = model.Storagerworkloaddistribution,
                NorthSouthBandWidthForPodType = model.Northsouthbandwidthforpodtype,
                EastWestBandWidthForPodType = model.Eastwestbandwidthforpodtype,
                SpecialRequirementPerPodType = model.Specialrequirementperpodtype,
                ListOfCapacitySpecialRequirement = model.Capacityspecialrequirement,
                NumberOfPodsPerPodType = model.Numberofpodsperpodtype,
               

                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };

            return result;
        }

        public static Cnfcapacity SetCnfCapacityMapper(CnfCapacity model)
        {
            if (model == null)
                return null;
            var result = new Cnfcapacity()
            {
                Cnfcapacityid = model.CnfCapacityId,
                Cnfpodinfoid = model.CnfPodInfoId,

                Financialyear = model.FinancialYear,
                Financialversion = model.FinancialVersion,

                Vcpulimitforpodtype = model.VcpuLimitForPodType,
                Pcpurequestforpodtype = model.PcpuRequestForPodType,
                Memrequestforpodtype = model.MemRequestForPodType,
                Memlimitforpodtype = model.MemLimitForPodType,
                Vcpurequestforpodtype = model?.VcpuRequestForPodType,

                Nonpresistentstorageforprodtype = model.NonPresistentStorageForProdType,
                Ispresistentvolumesrequired = model.IsPresistentVolumesRequired,
                Persistentvolumneaccessmode = model.PersistentVolumNeaccessMode,

                Persistentstorageforpodtype = model?.PersistentStorageForPodType,
                Storageiopsforpodtype = model.StorageIopsForPodType,
                Storagerworkloaddistribution = model.StoragerWorkloadDistribution,
                Northsouthbandwidthforpodtype = model.NorthSouthBandWidthForPodType,
                Eastwestbandwidthforpodtype = model.EastWestBandWidthForPodType,
                Specialrequirementperpodtype = model.SpecialRequirementPerPodType,
                Capacityspecialrequirement = model.ListOfCapacitySpecialRequirement,
                Noofcnfinstancespersite = model.NoOfCnfInstancesPerSite,
               Numberofpodsperpodtype = model.NumberOfPodsPerPodType,
            };
            return result;
        }
    }
}
