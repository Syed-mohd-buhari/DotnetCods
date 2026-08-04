using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.AbstractionLayer;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Engine;
using CAM.Infrastucture.QueryResult;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CAM.BusinessManager.Grid
{
    public class GridCustomColumnManager : BaseManager
    {
        private ICurrentUserService _currentUserService;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly Lazy<AspnetuserroleManager> _aspnetuserroleManager;

        public GridCustomColumnManager(ICurrentUserService currentUserService,
            IEnumerable<IRepositoryWrapper> wrappers, IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper,Lazy<AspnetuserroleManager> aspnetuserroleManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _currentUserService = currentUserService;
            _aspnetuserroleManager = aspnetuserroleManager;

        }

        //todo: we need revamp this part
        public List<RenderDetail> GetMapping(string className)
        {
            var mapping = _repositoryWrapper.GridCustomColumnRepository.FindByCondition(x =>
               x.Userid == _currentUserService.UserId && x.Classname == className).FirstOrDefault();
            if (mapping == null)
            {
                return new List<RenderDetail>();
            }
            return JsonSerializer.Deserialize<List<RenderDetail>>(mapping.Jsongridcustomizationdata);
        }

        public bool DeleteMapping(string className)
        {
            try
            {
                var mapping = _repositoryWrapper.GridCustomColumnRepository.FindByCondition(x =>
                     x.Userid == _currentUserService.UserId && x.Classname == className).SingleOrDefault();
                _repositoryWrapper.GridCustomColumnRepository.Delete(mapping);
                _repositoryWrapper.Save();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public void SaveMapping(string className, List<RenderDetail> renderDetails, List<UserPrefrenceDetails> userPrefrenceDetails = null
            , int WhatsGoingOnDate = 0, int MessagingDate = 0, int userId = 0)
        {
            var jsonData = string.Empty;

            if (renderDetails?.Any() == true && !(className.IsNullOrEmpty()))
            {
                jsonData = JsonSerializer.Serialize(renderDetails);
            }
            else if (userPrefrenceDetails?.Any() == true &&  (className.IsNullOrEmpty() || className == ConstantValueFilter.RoleWisePreferenceName))
            {
                className = (className == ConstantValueFilter.RoleWisePreferenceName ? ConstantValueFilter.RoleWisePreferenceName : ConstantValueFilter.UserPreferenceName);
                jsonData = JsonSerializer.Serialize(userPrefrenceDetails);
                var result = _aspnetuserroleManager.Value.AddOrUpdate(userPrefrenceDetails, userId != 0 ? userId : _currentUserService.UserId,true);
            }
            else if ((WhatsGoingOnDate != 0 || MessagingDate != 0) && (className.IsNullOrEmpty() || className == ConstantValueFilter.RoleWisePreferenceName)) 
            {
                className = (className == ConstantValueFilter.RoleWisePreferenceName ? ConstantValueFilter.RoleWisePreferenceName : ConstantValueFilter.UserPreferenceName);
            }
            var model = new Gridcustomcolumn();

            if (userId != 0)
            {
                model = _repositoryWrapper.GridCustomColumnRepository.FindByCondition(x =>
                       x.Userid == userId && x.Classname == className && !x.Deleted.Value).OrderByDescending(p => p.Creationdate).FirstOrDefault();
            }
            else
            {
                model = _repositoryWrapper.GridCustomColumnRepository.FindByCondition(x =>
                       x.Userid == _currentUserService.UserId && x.Classname == className && !x.Deleted.Value).OrderByDescending(p => p.Creationdate).FirstOrDefault();

                userId = _currentUserService.UserId;
            }
            var mapping = GridCustomColumnMapper.Get(model);

            if (mapping != null)
            {
                mapping.JsonGridCustomizationData = jsonData;
                if(WhatsGoingOnDate !=0)
                mapping.Preferencedate = mapping.Preferencedate == null && WhatsGoingOnDate != 0 ? WhatsGoingOnDate : WhatsGoingOnDate != 0 ? WhatsGoingOnDate : mapping.Preferencedate;
                if (MessagingDate != 0)
                    mapping.MessagingDate = mapping.MessagingDate == null && MessagingDate != 0 ? MessagingDate : MessagingDate != 0 ? MessagingDate : mapping.MessagingDate;

                _repositoryWrapper.GridCustomColumnRepository.Update(GridCustomColumnMapper.Set(mapping));
            }
            else
            {
                _repositoryWrapper.GridCustomColumnRepository.Create(GridCustomColumnMapper.Set(new GridCustomColumn()
                {
                    ClassName = className,
                    JsonGridCustomizationData = jsonData,
                    UserId = userId,
                    Preferencedate = WhatsGoingOnDate != 0 ? WhatsGoingOnDate : null,
                    MessagingDate = MessagingDate != 0 ? MessagingDate : null,
                   
                }));
            }

            _repositoryWrapper.Save();
        }

 
    }
}
