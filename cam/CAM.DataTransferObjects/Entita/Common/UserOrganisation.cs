using CAM.DataTransferObjects.FunctionalityDto;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.Common
{
    public class UserOrganisation
    {
        public long UserId { get; set; }     
      
        public string Email { get; set; }
       
        public Dictionary<short, string> OpCosDic { get; set; }
        public Dictionary<int, string> VerticleResponseDic { get; set; }       
        public Dictionary<int, string> subDomainResponseDic { get;set; }

        public IEnumerable<FilterValueDto> OpCosFilterValue { get;set; }
        public IEnumerable<FilterValueDto> VerticalResponseFilterValue { get; set; }
        public IEnumerable<FilterValueDto> SubDomainResponseFilterValue { get; set; }
        public FilterValueDto SubDomainResponseFilterValues { get; set; }


        public string SubDomainResponsible { get; set; }
        public string OrganizationName { get; set; }

        public string Practicedescription { get; set; }
    }

    public class LcmEngineeringEduSpoc
    {
        public long Lcmengineeringeduspocid { get; set; }        
        public long Lcmengineeringid { get; set; }
     
        public string ContactEmail { get; set; }
        public int? Eduspocid { get; set; }         
        public Dictionary<int, string> SubdomainresponsiblesDic { get; set; }

        public Dictionary<int, string> VerticalDic { get; set; }
      
     public bool isAdminRole   { get; set; }
    }

    public class LcmEngineeringSubDpomainSpoc
    {
        public long Lcmengineeringsubdomainspocid { get; set; }
        public long Lcmengineeringid { get; set; }
       
        public string ContactEmail { get; set; }
        public Dictionary<int, string> SubdomainresponsiblesDic { get; set; }

        public Dictionary<int, string> VerticalDic { get; set; }

        public bool isAdminRole { get; set; }
    }


    public class NetWorkElementAsPlannedEduSpoc
    {
        public long NetWorkElementAsPlannedEduSpocId { get; set; }
        public long NetWorkElementAsPlannedId { get; set; }
        public string ContactEmail { get; set; }
        public int? Eduspocid { get; set; }
        public Dictionary<int, string> SubdomainresponsiblesDic { get; set; }

        public Dictionary<int, string> VerticalDic { get; set; }
        public bool isAdminRole { get; set; }
    }

    public class NetWorkElementAsPlannedSubDpomainSpoc
    {
        public long NetWorkElementAsPlannedsubdomainspocid { get; set; }
        public long NetWorkElementAsPlannedId { get; set; }

        public string ContactEmail { get; set; }
        public Dictionary<int, string> SubdomainresponsiblesDic { get; set; }

        public Dictionary<int, string> VerticalDic { get; set; }

        public bool isAdminRole { get; set; }
    }

    public class LcmOperationalContracts
    {
        public long LcmengineeringId { get; set; }
        public string OperationDescription { get; set; }
    }


    public class HardwareConfigurations
    {
        public string ElementName { get; set; }
        public string HardwareType { get; set; }
        public string ProductName { get; set; }
        public string ProductNumber { get; set; }
        public string Revision { get; set; }
        public string SerialNumber { get; set; }
        public string UnitLocation { get; set; }
        public string Vendor { get; set; }
        public DateTime Modificationdate { get; set; }
    }
    public class MIlestoneStatus
    {
        public int MS1Status { get; set; }
        public int MS2Status { get; set; }
        public int MS3Status { get; set; }
        public int MS4Status { get; set; }
        public bool isSuccess { get; set; } = false;
        public bool isNodeCountApplicable { get; set; } = false;


    }

}
