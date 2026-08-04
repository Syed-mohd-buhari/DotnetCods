using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.TsrPassThrough;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class TsrPassThroughMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public TsrPassThroughMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<TsrPassThrough, TsrPassThroughDtoGrid>()
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
                       .ForMember(x => x.AssetTypeTsr, s => s.MapFrom(src => src.AssetType))
                       .ForMember(x => x.LastUpgradeDateTsr, s => s.MapFrom(src => src.LastUpgradeDate))
                        .ForMember(x => x.IdentifiedActionTsr, s => s.MapFrom(src => src.IdentifiedAction))
                       .ForMember(x => x.SerialNumberTsr, s => s.MapFrom(src => src.SerialNumber))
                                    .ForMember(x => x.BudgetEstimatedTsr, s => s.MapFrom(src => src.BudgetEstimated))
                       .ForMember(x => x.BundleBudgetTsr, s => s.MapFrom(src => src.BundleBudget))
                        .ForMember(x => x.ProjectEndDateTsr, s => s.MapFrom(src => src.ProjectEndDate))
                       .ForMember(x => x.CommentOnProjectStatusTsr, s => s.MapFrom(src => src.CommentOnProjectStatus))
                       .ForMember(x => x.ProjectStatusTsr, s => s.MapFrom(src => src.ProjectStatus))
                       .ForMember(x => x.ProductImportanceTsr, s => s.MapFrom(src => src.ProductImportance))
                       .ForMember(x => x.LastPenTestDateTsr, s => s.MapFrom(src => src.LastPenTestDate))
                       .ForMember(x => x.LastPenTestRefNo, s => s.MapFrom(src => src.LastPenTestRefNo))
                       .ForMember(x => x.MeProductName, s => s.MapFrom(src => src.ProductName))
                       .ForMember(x => x.MeSoftwareVersion, s => s.MapFrom(src => src.SoftwareVersion))
                       .ForMember(x => x.MeSubDomainResponsible, s => s.MapFrom(src => src.SubDomainResponsible))

                       ;

        }
    }

}
