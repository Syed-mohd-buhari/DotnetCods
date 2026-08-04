using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.Entities.Models.Lookup;
using CAM.Enum;
using CAM.Repository;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Dapper
{
    public class DapperCommonManager :BaseManager
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerManager _logger;         
        private readonly CommonDapperRepository _commonDapperRepository;
        private readonly CommonManager _commonManager;        
        private readonly string dapperDatabaseMode = "normal";
        public DapperCommonManager(IEnumerable<IRepositoryWrapper> wrappers, IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, ILoggerManager logger
            , CommonDapperRepository commonDapperRepository , CommonManager commonManager
       ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _commonDapperRepository = commonDapperRepository;
            _commonManager = commonManager;
            dapperDatabaseMode = GlobalDbMode.DbMode.ContainsKey(CurrentLogLevelConfig._UserName) ? GlobalDbMode.DbMode[CurrentLogLevelConfig._UserName] : "normal";

        }

        #region Common Method
        public Task<IEnumerable<int>> GetSwPlannedActivityRulesAsync()
        {
            return _commonDapperRepository.QueryAsync<int>(
                dapperDatabaseMode,
                @"
            SELECT Plannedactivityresourceid
            FROM plannedactivityresources
            WHERE rulelinkeddc IN :RuleLinkedDc",
                new
                {
                    RuleLinkedDc = PlannedActivityResourceEnum
                        .SwArchitectureUpgrade_SwMajorRelease
                });
        }
        public Task<int?> GetMessagingDateRangeAsync(int userId)
        {
            return _commonDapperRepository.QueryFirstOrDefaultAsync<int?>(
                dapperDatabaseMode,
                @"
            SELECT Messagingdate
            FROM gridcustomcolumn
            WHERE Userid = :UserId
              AND Classname = :ClassName
              AND Messagingdate IS NOT NULL
            FETCH FIRST 1 ROW ONLY",
                new
                {
                    UserId = userId,
                    ClassName = ConstantValueFilter.UserPreferenceName
                });
        }
        public Task<IEnumerable<Gridcustomcolumn>> GetGridCustomColumnsAsync(
    long userId)
        {
            return _commonDapperRepository.QueryAsync<Gridcustomcolumn>(
                dapperDatabaseMode,
                @"
            SELECT *
            FROM gridcustomcolumn
            WHERE Userid IN :UserId
              AND Classname = :ClassName",
                new
                {
                    UserId = userId,
                    ClassName = ConstantValueFilter.UserPreferenceName
                });
        }


        public Task<int?> GetLcmProductImportanceIdAsync()
        {
            var lcmProductImportanceRecord =
                             _commonDapperRepository.QueryFirstOrDefaultAsync<int?>(
                                 dapperDatabaseMode,
                                 @"
                                            SELECT Productimportanceid
                                            FROM productimportances
                                             WHERE LOWER(REPLACE(Productimportance, ' ', '')) = :Productimportance
                                            FETCH FIRST 1 ROW ONLY",
                                 new
                                 {
                                     Productimportance = ConstantValueFilter.ProdImportanceStrategic
                                 });

            return lcmProductImportanceRecord;
        }


        #endregion

        #region Get MajorHardware Vendor and Platform for Exodus filter
        public Task<IEnumerable<Originalequipmentmanufacturers>> GetHardwareOemForExodusFilterAsync()
        {
            return _commonDapperRepository.QueryAsync<Originalequipmentmanufacturers>(
                dapperDatabaseMode,
                @"select ORGEQPMANUFACTURERID,ORIGINALEQUIPMENTMANUFACTURER from originalequipmentmanufacturers oem
                            join appsettingsconfiguration ac ON  REPLACE(LOWER(ac.settingsvalue), ' ', '') =
                             REPLACE(LOWER(oem.ORIGINALEQUIPMENTMANUFACTURER), ' ', '')
                            join appsettings aset on aset.appsettingid = ac.appsettingid
                            where oem.deleted = 0 and ac.deleted = 0 and aset.deleted = 0 and ac.appsettingid = 3", null);
        }
        public Task<IEnumerable<Platforms>> GetHardwarePlatformForExodusFilterAsync()
        {
            return _commonDapperRepository.QueryAsync<Platforms>(
                dapperDatabaseMode,
                @"select PLATFORMID,PLATFORM from platforms pl
                 join appsettingsconfiguration ac ON  REPLACE(LOWER(ac.settingsvalue), ' ', '') =
                 REPLACE(LOWER(pl.PLATFORM), ' ', '')
                 join appsettings aset on aset.appsettingid = ac.appsettingid
                  where pl.deleted = 0 and ac.deleted = 0 and aset.deleted = 0 and ac.appsettingid = 4", null);
        }

        public Task<IEnumerable<Platforms>> GetTargetHWPlatformForExodusFilterAsync()
        {
            return _commonDapperRepository.QueryAsync<Platforms>(
                dapperDatabaseMode,
                @"select PLATFORMID,PLATFORM from platforms pl
                 join appsettingsconfiguration ac ON  REPLACE(LOWER(ac.settingsvalue), ' ', '') =
                 REPLACE(LOWER(pl.PLATFORM), ' ', '')
                 join appsettings aset on aset.appsettingid = ac.appsettingid
                  where pl.deleted = 0 and ac.deleted = 0 and aset.deleted = 0 and ac.appsettingid = 6", null);
        }
        #endregion

        public Task<IEnumerable<Opcos>> GetRestrictedOpcoAsync()
        {
            return _commonDapperRepository.QueryAsync<Opcos>(
                dapperDatabaseMode,
                @"select OPCOID,OPCO from OPCOS o1 
                  join appsettingsconfiguration ac ON  REPLACE(LOWER(ac.settingsvalue), ' ', '') =REPLACE(LOWER(o1.OPCO), ' ', '')
                  join appsettings aset on aset.appsettingid = ac.appsettingid
                  where o1.deleted = 0 and ac.deleted = 0 and aset.deleted = 0 and ac.appsettingid = 5", null);
        }

        public Task<IEnumerable<Aspnetroles>> GetAspnetRoleDetialsAsync()
        {
            return _commonDapperRepository.QueryAsync<Aspnetroles>(
                dapperDatabaseMode,
                @"select id,name,portalroleid from aspnetroles role 
                  join appsettingsconfiguration ac ON  REPLACE(LOWER(ac.settingsvalue), ' ', '') =REPLACE( role.id , ' ', '')
                  join appsettings aset on aset.appsettingid = ac.appsettingid
                  where o1.deleted = 0 and ac.deleted = 0 and aset.deleted = 0 and ac.appsettingid = 6", null);
        }
    }
}
