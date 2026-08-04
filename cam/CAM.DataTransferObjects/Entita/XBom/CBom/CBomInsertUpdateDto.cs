using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.XBom.CBom
{
    public class CBomInsertUpdateDto
    {
        public CnfClusterInfoDto _cnfClusterInfoEntity { get; set; } 
    }
    public class CnfClusterInfoDto
    {
        public long Cnfclusterinfoid { get; set; }
        public long Cnfnameid { get; set; }
        public long Cnfclusterid { get; set; }
        public long? CnfClusterNodePoolId { get; set; }
        public short Opcoid { get; set; }
        public short Siteid { get; set; }
        public long Cnfhardwareid { get; set; }
        public int? Noofcnfinstancespersite { get; set; }
        public bool? Nodepoolbreakup { get; set; }
        public string Specialrequirements { get; set; }
        public string Hyperthreading { get; set; }
        public string Overprovisioning { get; set; }
        public string Workernodeconfiguration { get; set; }
        public string Hardware { get; set; }
        public decimal Cpukubelet { get; set; }
        public int Memkubelet { get; set; }
        public decimal Cpusystem { get; set; }
        public int Memsystem { get; set; }
        public int Verticalresponsibleid { get; set; }
        public string Comments { get; set; }
        public string Notes { get; set; }
        public string Filename { get; set; }
        public string Revision { get; set; }
        public string Aggregateimageclustersize { get; set; }

        public List<CnfPodInfoDto> Cnfpodinfo { get; set; }

    }
    public class CnfPodInfoDto
    {
        public long Cnfpodinfoid { get; set; }
        public long Cnfclusterinfoid { get; set; }
        public long Podtypeinfoid { get; set; }
        public long Functionstandardid { get; set; }
        public long Priorityid { get; set; }
        public long? Podroledescriptionid { get; set; }
        public int? Numberofpodsperpodtype { get; set; }
        public bool? Daemonsetpod { get; set; }
        public string Intrapodrules { get; set; }
        public string Interpodrules { get; set; }
        public bool? Isenhancedha { get; set; }
        public string Podtypeqos { get; set; }
        public string Ispersistancestorageflag { get; set; }
        public bool? Isprodhpaenable { get; set; }

        public List<CnfCapacityDto> Cnfcapacity { get; set; }
    }
    public class CnfCapacityDto
    {
        public long Cnfcapacityid { get; set; }
        public long Cnfpodinfoid { get; set; }
        public short Financialyear { get; set; }
        public int Financialversion { get; set; }
        public long? Vcpurequestforpodtype { get; set; }
        public long? Vcpulimitforpodtype { get; set; }
        public long? Pcpurequestforpodtype { get; set; }
        public decimal? Memrequestforpodtype { get; set; }
        public decimal? Memlimitforpodtype { get; set; }
        public string Nonpresistentstorageforprodtype { get; set; }
        public bool? Ispresistentvolumesrequired { get; set; }
        public string Persistentvolumneaccessmode { get; set; }
        public string Persistentstorageforpodtype { get; set; }
        public string Storageiopsforpodtype { get; set; }
        public string Storagerworkloaddistribution { get; set; }
        public string Northsouthbandwidthforpodtype { get; set; }
        public string Eastwestbandwidthforpodtype { get; set; }
        public string Specialrequirementperpodtype { get; set; }
        public string Capacityspecialrequirement { get; set; }

        public int? Noofcnfinstancespersite { get; set; }
        public int? Numberofpodsperpodtype { get; set; }
    }
   
}
