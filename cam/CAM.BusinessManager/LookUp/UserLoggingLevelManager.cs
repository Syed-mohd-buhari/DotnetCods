using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.AbstractionLayer;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using CAM.DataTransferObjects.LookUp.UserLogginLevels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CAM.BusinessManager.LookUp
{
    public class UserLoggingLevelManager : BaseManager, IUserLoggingLevelManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        public UserLoggingLevelManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, IMapper mapper,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, ILoggerManager logger) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<string> GetCurrentLogLevel()
        {
            string relatedLogLevel = await _repositoryWrapper.UsersLoggingLevelRepository
                .FindAll().Select(x => x.Loglevel).FirstOrDefaultAsync();
            return relatedLogLevel;
        }
        public UsersLoggingLevelsDtoGrid GetCreatePage()
        {
            var dto = new UsersLoggingLevelsDtoCreate();
            dto.AllLoggingLevels = "Trace,Debug,Info,Error,Warn,Fatal,Off";
            dto.CurrentLogLevel = _repositoryWrapper.UsersLoggingLevelRepository
                .FindAll().Select(x => x.Loglevel).FirstOrDefault();
            return dto;
        }
        public async Task<ResultDto> AddOrEdit(UserLoggingLevelDto dto)
        {
            UsersLoggingLevels entity;

            if (!string.IsNullOrEmpty(dto.CurrentLogLevel))
            {
                var dbUserLoggingLevelObject = _repositoryWrapper.UsersLoggingLevelRepository
                        .FindAll().FirstOrDefault();
                if (dbUserLoggingLevelObject != null)
                {
                    // delete
                    _repositoryWrapper.UsersLoggingLevelRepository.DeleteDeep(dbUserLoggingLevelObject);
                }
                // add
                entity = new UsersLoggingLevels()
                {
                    LogLevel = dto.CurrentLogLevel,
                };

                var LoggingLevelObjectToBeSaved = UserLoggingLevelMapper.SetUsersLoggingLevelsMapper(entity);
                _repositoryWrapper.UsersLoggingLevelRepository.Create(LoggingLevelObjectToBeSaved);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto { Info = ResultMessages.EntryAddSuccess };
            }
            else
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = "You did not select a log level",
                    Data = null
                };
            }
        }

        public long GetPageSize()
        {
            long pageSize = 10;
            try
            {
                var appSettingEntity = _repositoryWrapper.AppConfigurationSettingsRepository.FindByCondition(x => x.Appsettingid == 1).FirstOrDefault();

                if (appSettingEntity != null)
                {
                    pageSize = Convert.ToInt64(appSettingEntity.Settingsvalue);
                }
                return pageSize;
            }
            catch (Exception ex)
            {
                return pageSize;
            }
        }

        #region Rbac 

        #region RBAC Changes
        public class AuthRoleDetailDto
        {

            public int RoleId { get; set; }  
            public int PortalRoleId { get; set; }
            public string RoleName { get; set; }
        }
        public class UserPagePrefrenceDetails
        {
            //public int PrefrenceDate { get; set; }
            public int WhatsGoingOnDate { get; set; }  //its PrefrenceDate
            public int MessagingDate { get; set; }
            public List<PagePrefrenceDetails> pagePrefrenceDetail { get; set; }
            public List<PagePrefrenceDetails> masterPagePrefrenceDetail { get; set; }
            public List<PagePrefrenceDetails> popupPagePrefrenceDetail { get; set; }
        }
        public class PagePrefrenceDetails
        {
            public string Id { get; set; }
            public string Text { get; set; }
            public string Path { get; set; }
            public string Menu { get; set; }
            public int Order { get; set; }
            public bool Default { get; set; }
            public short? ScreenPermission { get; set; }
        }
        #endregion
        #endregion
        public void CreateUserPrefrenceDetails(long userId)
        {

            try
            {
                var gridCustomeEntitiy = _repositoryWrapper.GridCustomColumnRepository.FindByCondition(x => x.Userid == userId && x.Classname == ConstantValueFilter.RoleWisePreferenceName).FirstOrDefault();

                var userPageDetails = _repositoryWrapper.UserRoleRepository.FindByCondition(x => x.Userid == userId && x.Role.Aspnetuserrolepermissions != null)
                    .Include(s => s.Role.Aspnetuserrolepermissions).ThenInclude(x => x.Module)
                    ?.SelectMany(x => x.Role.Aspnetuserrolepermissions)?.Select(t => new UserPrefrenceDetails
                    {
                        Id = t.Aspnetuserrolepermissionid.ToString(),
                        Text = t.Module.Module,
                        Path = t.Module.Modulepath,
                        Default = (bool)(t.Module.Isdefault ?? false),
                        Menu = t.Module.Menu,
                        ScreenId = t.Moduleid,
                        ScreenPermission = t.Permissionlevel
                    })?.ToList();   //OrderByDescending(x => x.ScreenPermission)?.DistinctBy(x => x.Text).ToList();
                if (userPageDetails != null && userPageDetails.Count() > 0)
                {
                    _columnManager.SaveMapping(ConstantValueFilter.RoleWisePreferenceName, null, userPageDetails, ConstantValueFilter.WhatsGoingOnDate
                        , ConstantValueFilter.MessagingDate );

                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }
        }

        public UserPagePrefrenceDetails GetUserPrefrenceDetail(long userId)
        {
            var _userPagePrefrenceDetails = new UserPagePrefrenceDetails();
            try
            {
                var gridCustomeEntitiy = _repositoryWrapper.GridCustomColumnRepository.FindByCondition(x => x.Userid == userId && x.Classname == ConstantValueFilter.RoleWisePreferenceName).FirstOrDefault();

                if (gridCustomeEntitiy == null)
                {
                    // CreateUserPrefrenceDetails(userId);
                    //gridCustomeEntitiy = _repositoryWrapper.GridCustomColumnRepository.FindByCondition(x => x.Userid == userId && x.Classname == ConstantValueFilter.RoleWisePreferenceName).FirstOrDefault();
                    var userPageDetails = _repositoryWrapper.UserRoleRepository.FindByCondition(x => x.Userid == userId && x.Role.Aspnetuserrolepermissions != null)
                          .Include(s => s.Role.Aspnetuserrolepermissions).ThenInclude(x => x.Module)
                          ?.SelectMany(x => x.Role.Aspnetuserrolepermissions)?.Select(t => new UserPrefrenceDetails
                          {
                              Id = t.Aspnetuserrolepermissionid.ToString(), // need to check with ezhil, In user management we will send model is id but here we are passing PK id of permissiontable
                              Text = t.Module.Module,
                              Path = t.Module.Modulepath,
                              Default = (bool)(t.Module.Isdefault ?? false),
                              Menu = t.Module.Menu,
                              ScreenId = t.Moduleid,
                              ScreenPermission = t.Permissionlevel
                          })?.ToList();

                    if (userPageDetails != null && userPageDetails.Count > 0)
                    {
                        userPageDetails = userPageDetails?.DistinctBy(x => x.ScreenId).ToList();
                    }
                    gridCustomeEntitiy = new OracleModels.DBModels.Gridcustomcolumn();
                   var jsonData = JsonSerializer.Serialize(userPageDetails);
                    gridCustomeEntitiy.Jsongridcustomizationdata = jsonData;
                }

                if (gridCustomeEntitiy != null)
                {
                    var data = new List<PagePrefrenceDetails>();
                    if (gridCustomeEntitiy?.Jsongridcustomizationdata != null)
                    {
                        data = JsonSerializer.Deserialize<List<PagePrefrenceDetails>>(gridCustomeEntitiy?.Jsongridcustomizationdata);
                    }

                    _userPagePrefrenceDetails = new UserPagePrefrenceDetails
                    {
                       // PrefrenceDate = gridCustomeEntitiy?.Preferencedate ?? ConstantValueFilter.PrefrenceDate,
                        WhatsGoingOnDate = gridCustomeEntitiy?.Preferencedate ?? ConstantValueFilter.WhatsGoingOnDate,
                        MessagingDate = gridCustomeEntitiy?.Messagingdate ?? ConstantValueFilter.MessagingDate,
                        pagePrefrenceDetail = data.Where(x => x.Path.StartsWith('/')).ToList(),
                        masterPagePrefrenceDetail = data.Where(x => !x.Path.StartsWith('/') && !x.Path.StartsWith('-')).ToList(),
                        popupPagePrefrenceDetail = data.Where(x => x.Path.StartsWith('-')).ToList()
                    };


                }
                return _userPagePrefrenceDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException);
                return _userPagePrefrenceDetails;
            }
        }
        public async Task<List<AuthRoleDetailDto>> GetRoleDetails(long userId)
        {
            var _roleEntity = new List<AuthRoleDetailDto>();
            try
            {

                _roleEntity = await _repositoryWrapper.UserRoleRepository.FindByCondition(x => x.Userid == userId).Include(x => x.Role)
                  .Select(x => new AuthRoleDetailDto
                  {
                      RoleId = x.Role.Id,
                      RoleName = x.Role.Name,
                      PortalRoleId = (int)(x.Role.Portalroleid ?? 0)
                  }).ToListAsync();


                return _roleEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException);
                return _roleEntity;
            }
        }
        public async Task<UserPagePrefrenceDetails> GetUserPrefrenceDetails(long userId)
        {
            var _userPagePrefrenceDetails = new UserPagePrefrenceDetails();
            try
            {
                var data = new List<PagePrefrenceDetails>();
                var gridCustomeEntitiy = await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(x => x.Userid == userId && x.Classname == ConstantValueFilter.RoleWisePreferenceName).FirstOrDefaultAsync();
                var aspnNetUserpreferenceEntity = await _repositoryWrapper.aspNetUserPreferenceRepository.FindByCondition(x => x.Userid == userId).Include(x => x.Aspnetmodule).ToListAsync();

                if (aspnNetUserpreferenceEntity != null && aspnNetUserpreferenceEntity.Count <= 0)
                {
                    var userPageDetails = await _repositoryWrapper.UserRoleRepository.FindByCondition(x => x.Userid == userId && x.Role.Aspnetuserrolepermissions != null)
                          .Include(s => s.Role.Aspnetuserrolepermissions).ThenInclude(x => x.Module)
                          ?.SelectMany(x => x.Role.Aspnetuserrolepermissions)?.Select(t => new PagePrefrenceDetails
                          {
                              Id = t.Moduleid.ToString(), // need to check with ezhil, In user management we will send model is id but here we are passing PK id of permissiontable
                              Text = t.Module.Module,
                              Path = t.Module.Modulepath,
                              Default = (bool)(t.Module.Isdefault ?? false),
                              Menu = t.Module.Menu,
                              ScreenPermission = t.Permissionlevel
                          })?.ToListAsync();

                    if (userPageDetails != null && userPageDetails.Count > 0)
                    {
                        userPageDetails = userPageDetails?.GroupBy(x => x.Id)
                            .Select(r => r.OrderByDescending(o => o.ScreenPermission).FirstOrDefault()).ToList();
                    }
                    data = userPageDetails;
                }

                if (aspnNetUserpreferenceEntity != null && aspnNetUserpreferenceEntity.Count > 0)
                {
                    data = aspnNetUserpreferenceEntity.Select(x => new PagePrefrenceDetails
                    {
                        Id = x.Aspnetmodule?.Aspnetmoduleid.ToString(),
                        Text = x.Aspnetmodule?.Module,
                        Path = x.Aspnetmodule?.Modulepath,
                        Default = x.Aspnetmodule?.Isdefault ?? false,
                        Menu = x.Aspnetmodule?.Menu,
                        ScreenPermission = x.Permission
                    }).ToList();               
                }
                _userPagePrefrenceDetails = new UserPagePrefrenceDetails
                {
                    WhatsGoingOnDate = gridCustomeEntitiy?.Preferencedate ?? ConstantValueFilter.WhatsGoingOnDate,
                    MessagingDate = gridCustomeEntitiy?.Messagingdate ?? ConstantValueFilter.MessagingDate,
                    pagePrefrenceDetail = data.Where(x => x.Path.StartsWith('/')).ToList(),
                    masterPagePrefrenceDetail = data.Where(x => !x.Path.StartsWith('/') && !x.Path.StartsWith('-')).ToList(),
                    popupPagePrefrenceDetail = data.Where(x => x.Path.StartsWith('-')).ToList()
                };
                return _userPagePrefrenceDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException);
                return _userPagePrefrenceDetails;
            }
        }

       
    }
}
