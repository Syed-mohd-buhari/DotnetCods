using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.SoftwareBuildCompatibility;
using CAM.Enum;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Http;
using OracleModels.DBModels;

namespace CAM.BusinessManager.Entity
{
    public class SoftwareBuildCompatibilityManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
 
        protected readonly ILoggerManager _logger;
 
        
        public SoftwareBuildCompatibilityManager(IEnumerable<IRepositoryWrapper> wrappers,            
            IRepositoryWrapper repositoryWrapper,            
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _logger = logger; 

        }
        public async Task<ResultDto> Add(SoftwareBuildCompatibilityListDtoCreateUpdate dto)
        { 
    
                try
                { 
                  return   await AddBase(dto);
                       
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.StackTrace);
                }
            return null;
        }

        public async Task<ResultDto> AddBase(SoftwareBuildCompatibilityListDtoCreateUpdate dto)
        {

            try
            {
                if(dto != null)
                {
                    long majorSoftwareBuildId = dto.MajorSoftwareBuildId;
                    var recordExistBasedMajorSWBuildId = _repositoryWrapper.SoftwareBuildCompatibility.FindByCondition(x => x.Majorsoftwarebuildid == (long)majorSoftwareBuildId);
                                   

                if ( dto.TCISoftwareCompatibilityId?.Count() > 0)
                {
                   
                    #region TCL Record
                    foreach (var tclItem in dto.TCISoftwareCompatibilityId)
                    {
                        var getExistRecords = recordExistBasedMajorSWBuildId.Where(x => x.Bundlemajorsoftwarebuildid == tclItem).ToList();

                        if ((getExistRecords.Count == 0) || (getExistRecords.Where(x => x.Bundletype == (short)SoftwareCompatibilityEnum.TCIBundle).FirstOrDefault() == null))
                        {
                            _repositoryWrapper.SoftwareBuildCompatibility.Create(new Softwarebuildcompatibility
                            {
                                Majorsoftwarebuildid = majorSoftwareBuildId, // c.MajorSoftwareBuildId,
                                Bundlemajorsoftwarebuildid = tclItem,
                                Bundletype = (short)SoftwareCompatibilityEnum.TCIBundle
                            });
                        }
                    }

                    await _repositoryWrapper.SaveAsync();

                    var tciDeleteRecord = recordExistBasedMajorSWBuildId.Where(x => x.Bundletype == (short)SoftwareCompatibilityEnum.TCIBundle).ToList();
                    foreach (var item in tciDeleteRecord)
                    {
                        if (!dto.TCISoftwareCompatibilityId.Contains(item.Bundlemajorsoftwarebuildid))
                        {
                            _repositoryWrapper.SoftwareBuildCompatibility.DeleteDeep(item);
                        }
                    }

                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                    #endregion
                }
                if ( dto.TCPSoftwareCompatibilityId?.Count() > 0)
                { 
                    #region TCP Record
                    foreach (var tcpItem in dto.TCPSoftwareCompatibilityId)
                    {
                         var getExistRecords = recordExistBasedMajorSWBuildId.Where(x => x.Bundlemajorsoftwarebuildid == tcpItem).ToList();

                        if ((getExistRecords.Count == 0) || (getExistRecords.Where(x => x.Bundletype == (short)SoftwareCompatibilityEnum.TCPBundle).FirstOrDefault() == null))
                        {
                            _repositoryWrapper.SoftwareBuildCompatibility.Create(new Softwarebuildcompatibility
                            {
                                Majorsoftwarebuildid = majorSoftwareBuildId, // c.MajorSoftwareBuildId,
                                Bundlemajorsoftwarebuildid = tcpItem,
                                Bundletype = (short)SoftwareCompatibilityEnum.TCPBundle
                            });
                        }
                    }

                    await _repositoryWrapper.SaveAsync();

                    var tcpDeleteRecord = recordExistBasedMajorSWBuildId.Where(x => x.Bundletype == (short)SoftwareCompatibilityEnum.TCPBundle).ToList();
                    foreach (var item1 in tcpDeleteRecord)
                    {
                        if (!dto.TCPSoftwareCompatibilityId.Contains(item1.Bundlemajorsoftwarebuildid))
                        {
                            _repositoryWrapper.SoftwareBuildCompatibility.DeleteDeep(item1);
                        }
                    }

                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                        #endregion
                    }
                }




            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }

            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }        
      
        public async Task<ResultDto> DeleteDeep(SoftwareBuildCompatibilityListDtoCreate dto)
        {

            foreach(var item in dto.SoftwareBuildCompatibilityList)
            {
                var entity =  _repositoryWrapper.SoftwareBuildCompatibility.FindByCondition(x => x.Majorsoftwarebuildid == item.MajorSoftwareBuildId 
                && x.Bundlemajorsoftwarebuildid == item.BundleMajorSoftwareBuildId && x.Bundletype == item.BundleType).FirstOrDefault();

                _repositoryWrapper.SoftwareBuildCompatibility.DeleteDeep(entity);
               

            }
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess 
            };
        }
       
    }
}