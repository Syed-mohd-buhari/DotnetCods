using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.SystemType;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using OracleModels.DBModels;

namespace CAM.BusinessManager.Entity
{
 public   class ProductLifecycleConstraintsManager:BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager manager;
        private readonly MajorSoftwareBuildManager _majorSoftwareBuildManager;
        private readonly MajorHardwareBuildManager _majorHardwareBuildManager;
        private readonly SystemTypeManager _systemTypeManager;

        public ProductLifecycleConstraintsManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, IRepositoryWrapper repositoryWrapper,
            MajorSoftwareBuildManager majorSoftwareBuildManager, GridCustomColumnManager manager, MajorHardwareBuildManager majorHardwareBuildManager, 
            SystemTypeManager systemTypeManager, IHttpContextAccessor contextAccessor):base(contextAccessor,wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            this.manager = manager;
            _majorHardwareBuildManager = majorHardwareBuildManager;
             _majorSoftwareBuildManager = majorSoftwareBuildManager;
            _systemTypeManager = systemTypeManager;

        }

        public async Task<ResultDto> SaveLifecycleConstraint(LifecycleConstraintDto dto)
        {
            //if (
            //    //dto.MajorSoftwareBuildDto.GeneraAvailableDate == null ||
            //    //dto.MajorSoftwareBuildDto.GeneraAvailableDate == DateTime.MinValue ||
            //    dto.MajorSoftwareBuildDto.EndOfMaintenance == null ||
            //    dto.MajorSoftwareBuildDto.EndOfMaintenance == DateTime.MinValue ||
            //    //dto.MajorSoftwareBuildDto.EndOfsupport == null ||
            //    //dto.MajorSoftwareBuildDto.EndOfsupport == DateTime.MinValue ||
            //    dto.MajorHardwareBuildDto.EndOfMaintenance == null ||
            //    dto.MajorHardwareBuildDto.EndOfMaintenance == DateTime.MinValue
            //)
            //    return new ResultDto() { Warning = true, Info = ResultMessages.ErrorValidation };
            //else
            //{
                //update ms
                await _majorSoftwareBuildManager.UpdateMajorSoftwareBuildEntity(dto.MajorSoftwareBuildDto);
                ////update mh

                await _majorHardwareBuildManager.UpdateMajorHardwareBuildEntity(dto.MajorHardwareBuildDto);

                var res= await _systemTypeManager.UpdateBase(dto.SystemTypeDto, false);
                var st = (Systemtypes)res.Data;
                st = _systemTypeManager.SetSystemTypeValue(st);
                 _repositoryWrapper.SystemType.Update(st);
                 _repositoryWrapper.Save();
                 return new ResultDto
                 {
                     Warning = false,
                     Info = ResultMessages.EntryUpdateSuccess
                 };
            //}

        }

        public ConstraintInfoDto GetLifecycleCostraintInfo(LifecycleConstraintQueryDto dto)
        {
            var date = new List<DateTime?>();
            var constrainInfo = new ConstraintInfoDto();

            date.Add(dto?.MsLastTimeBuyNew );
            date.Add(dto?.MsLastTimeBuyUpgrades);
            date.Add(dto?.MsLastTimeBuyExpansions);
            date.Add(dto?.MsEndOfMaintenance);
            date.Add(dto?.MsEndOfsupport);
                        
            date.Add(dto?.MhLastTimeBuyNew);
            date.Add(dto?.MhLastTimeBuyUpgrades);
            date.Add(dto?.MhLastTimeBuyExpansions);
            date.Add(dto?.MhEndOfMaintenance);
            date.Add(dto?.MhEndOfsupport);

            var minorDate = date.Where(x => x.HasValue).OrderBy(x => x.Value).FirstOrDefault();
            var today = DateTime.Now;
            var nextMonth = today.AddDays(30);

            constrainInfo.ConstraintScaling = minorDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            string status = "";
            string constraintLcm = "";

            if (minorDate >= today && minorDate <= nextMonth)
            {
                status = "amber";
                constraintLcm = "On expiration";
            }
            else if (minorDate < today)
            {
                status = "red";
                constraintLcm = "Expired";
            }
            else if (minorDate >= nextMonth)
            {
                status = "green";
                constraintLcm = "On support";
            }
            constrainInfo.LcmStatus = status;
            constrainInfo.ConstraintLcm = constraintLcm;

            return constrainInfo;
        }
    }
}
