using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.XBom.VBom;
using CAM.Entities.Models.VBom;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration.XBom
{
    public class VBomMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        private CommonManager _commonManager { get; set; }
        public VBomMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, CommonManager commonManager)
        {
 

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<VnfInfo, VnfClusterInfoDtoGrid>();
            //.ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            //.ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate));

        }
    }
}
