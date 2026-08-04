using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.Entities.Models.Cross;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using OracleModels.DBModels;

namespace CAM.BusinessManager.Entity
{
    public class SystemTypesMajorHardwareBuildManager:BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public SystemTypesMajorHardwareBuildManager(IEnumerable<IRepositoryWrapper> wrappers,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper):base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        private async Task<IEnumerable<Systemtypesmajorhardwarebuilds>> GetAllWithRelations()
        {
            return await _repositoryWrapper.SystemTypesMajorHardwareBuild.GetAllWithRelations();
        }

        public async Task<IEnumerable<Systemtypesmajorhardwarebuilds>> FindAll()
        {
            var data = _repositoryWrapper.SystemTypesMajorHardwareBuild.FindAll();
            return data.AsEnumerable();
        }

        public async Task Add(Systemtypesmajorhardwarebuilds entity)
        {
            _repositoryWrapper.SystemTypesMajorHardwareBuild.Create(entity);
        }
        public async Task Update(Systemtypesmajorhardwarebuilds entity)
        {
            _repositoryWrapper.SystemTypesMajorHardwareBuild.Update(entity);
        }

        public async Task Delete(Systemtypesmajorhardwarebuilds entity)
        {
            _repositoryWrapper.SystemTypesMajorHardwareBuild.Delete(entity);
        }


        public async Task DeleteDeep(Systemtypesmajorhardwarebuilds entity)
        {
            _repositoryWrapper.SystemTypesMajorHardwareBuild.DeleteDeep(entity);
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = rm
                };
            }
            else
                return new ResultDto();
        }

        public async Task FindWithConditio(Expression<Func<Systemtypesmajorhardwarebuilds, bool>> func)
        {
            _repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(func);

        }


    }
}