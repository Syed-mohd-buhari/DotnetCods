using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.Rules;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.LcmAncillaryData;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class LcmAncillaryDataMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public LcmAncillaryDataMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<LcmAncillaryData, LcmAncillaryDataDtoGrid>()
            .ForMember(x => x.ModificationUser, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.ModificationDate, s => s.MapFrom(src => src.ModificationDate))
            .ForMember(x => x.CreationUser, s => s.MapFrom(src => src.CreationUserEntity.Email))
            .ForMember(x => x.CreationDate, s => s.MapFrom(src => src.CreationDate))
            .ForMember(x => x.OpCo, s => s.MapFrom(src => src.LcmEngineering.OpCo.OpCoDescription))
            .ForMember(x=>x.DesignComponentIndex, s=>s.MapFrom(src=>src.LcmEngineering.DesignComponentId))
            .ForMember(x => x.ResourceKey, s => s.MapFrom(src => src.LcmEngineering.ResourceKey))
            .ForMember(x => x.LabSwRelease, s => s.MapFrom(src =>
             (src.LcmEngineering.NumberOfNodesInLab > 0) ?  
             (src.LcmEngineering.DesignComponent.SystemType.MajorSoftwareBuilds.SoftwareVersion) : ""
             ))
            #region // Dev 719 Regulatory fields chnages
            .ForMember(x => x.RegulatoryFields, s => s.MapFrom(src =>
             LCMEngineeringRulesExtension.GetRegulatoryFieldsValue(src.Ispecn,src.Ispecs,src.Isscf,src.Isnof)))
            #endregion
            .ForMember(x => x.NewopsRiskEvaluation, s => s.MapFrom(src => LCMEngineeringRulesExtension.GetNewOpsRiskEvaluationValue(src.IncidentClass,src.OccurenceProbability)))
            .ForMember(x => x.SecurityRiskOverall, s => s.MapFrom(src => LCMEngineeringRulesExtension.GetSecurityRiskOverAllValue(src.SecurityRiskEffective, LCMEngineeringRulesExtension.GetRiskLevelValue(src.LcmEngineering.DesignComponent.SystemType.VodafoneName.Id,_repositoryWrapper))))
            .ForMember(dest => dest.DesignComponent, opt => opt.MapFrom(src => CAM.Entities.Mappers.Entity.DesignComponentMapper.SetDesignComponentMapper(src.LcmEngineering.DesignComponent).toDesignComponentNameLcm(_repositoryWrapper)))
            .ForMember(x => x.RiskComment, s => s.MapFrom(src => src.RiskComment))
            .ForMember(x => x.QId, s => s.MapFrom(src => src.Qid))
            .ForMember(x => x.RequestId, s => s.MapFrom(src => src.RequestId))
            ;



            CreateMap<LcmAncillaryDataCRUDDto, LcmAncillaryData>();

        }
    }

}
