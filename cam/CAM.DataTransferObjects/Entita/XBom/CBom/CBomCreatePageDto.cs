using CAM.DataTransferObjects.Entita.XBom.VBom;
using CAM.DataTransferObjects.FunctionalityDto;
using OracleModels.DBModels;
using System.Collections.Generic;
 
namespace CAM.DataTransferObjects.Entita.XBom.CBom
{
    public class CBomCreatePageEntityDto
    {
 
       
        public List<KeyValuePairDto> CnfNameResources { get; set; }
        public List<Cnfcluster> cnfClusterNameResource { get; set; }

        public List<Cnfcluster> cnfClusterNodePoolResource { get; set; }
        public List<Podtypeinfo> PodTypeInfoResource { get; set; }
        public List<Podtypeinfo> PodTypeDescriptionInfoResource { get; set; }

        public List<VBom.OpcoBasedLocation> OpcoBasedLocationResource { get; set; }
        public List<Functionstandardname> FunctionStandardedNameResource { get; set; }

        public List<KeyValuePairDto> priorityResource { get; set; }
        public List<KeyValuePairDto> hardwareResource { get; set; }
        public List<KeyValuePairDto> verticalResource { get; set; }       

        public Cnfclusterinfo _cnfClusterInfoEntity { get; set; }
        public List<FilterValueDto> FinancialVersion { get; set; }
    }
   

}
