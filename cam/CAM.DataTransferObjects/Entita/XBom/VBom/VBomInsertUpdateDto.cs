using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.XBom.VBom
{
    public class VBomInsertUpdateDto
    {
        public VnfClusterInfoDto _VnfClusterInfoDto { get; set; } 
    }
    public class VnfClusterInfoDto
    {
        public long Vnfclusterinfoid { get; set; }
        public short Opcoid { get; set; }
        public short Locationid { get; set; }
        public long Clusternameid { get; set; }
        public short Noofblades { get; set; }
        public long HardwareTypeId { get; set; }
        public string HardwareType { get; set; }
        public string FileName { get; set; }
        public int? Revision { get; set; }

        public List<VnfInfoDto> vnfinfo { get; set; }
    }
    public class VnfInfoDto
    {
        public long Vnfinfoid { get; set; }
        public long Vnfnameid { get; set; }
        public long Vnfclusterinfoid { get; set; }
        public long Vnfvmtypenameid { get; set; }
        public bool Nsxt { get; set; }
        public long Intravmtypeid { get; set; }
        public long Intervmtypeid { get; set; }
        public long Vmworkloadtypeid { get; set; }
        public string Vmstorageblocksize { get; set; }
        public long Noofvnfinstances { get; set; }
        public long Noofvmspertype { get; set; }
        public bool? Numa { get; set; }
        public string Socket { get; set; }

        public List<VnfVmCapacityDto> vnfvmcapacity { get; set; }
    }
    
    public class VnfVmCapacityDto
    {
        public long Vnfvmcapacityid { get; set; }
        public long Vnfinfoid { get; set; }
        public short Financialyear { get; set; }
        public int Financialversion { get; set; }
        public long Vnfcpupervm { get; set; }
        public bool? Rxtxcpucount { get; set; }
        public long Rampervm { get; set; }
        public long Datadisk { get; set; }
        public long? Osdisk { get; set; }
        public string? Iopsrunning { get; set; }
        public string? Iopsloading { get; set; }
        public string Vmworkloaddistribution { get; set; }
        public string Northsouthboundbandwidth { get; set; }
        public string Eastwestboundbandwidth { get; set; }
        public string Otherrequirements { get; set; }
        public bool? Backuprequired { get; set; }
        public bool? Probingrequired { get; set; }

        public long? Noofvnfinstances { get; set; }
        public long? Noofvmspertype { get; set; }

    }
}
