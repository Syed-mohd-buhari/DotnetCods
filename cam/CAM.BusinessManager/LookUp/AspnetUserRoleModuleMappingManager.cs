using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.LookUp.AspnetUserRoleModuleMapping;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.InkML;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.LookUp
{
    public class AspnetUserRoleModuleMappingManager : BaseManager
    {
        private readonly DropdownDataServiceManager _dropdownManager;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        public AspnetUserRoleModuleMappingManager(DropdownDataServiceManager dropdownManager,IMapper mapper, GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _dropdownManager= dropdownManager;
        }

        public async Task<ResultDto> AddOrUpdateRoleModuleMapping(AspnetUserRoleModuleMappingUpdateDto dto)
        {
            var existingRoleEntities = await _repositoryWrapper.AspNetUserRolePermissionsRepository
                                                  .FindByCondition(x => x.Roleid == dto.RoleId).Include(x => x.Role)
                                                  .ToListAsync();


            if (!string.IsNullOrEmpty(dto.ModuleId))
            {
                var modelAndPermissionDetailsIds = SplitModelAndPermissionDetails(dto.ModuleId);

                var moduleIdSeparated = modelAndPermissionDetailsIds.ModuleId;
                var dicPermissionIds = modelAndPermissionDetailsIds.PermissionIds;

                using (var transaction = await _repositoryWrapper.BeginTransactionAsync())
                {
                    try
                    {
                        var roleEntryExist = await _repositoryWrapper.RoleRepository.FindByCondition(x => x.Normalizedname.Trim().ToLower().Replace(" ", "") ==
                           dto.RoleName.ToLower().Trim().Replace(" ", "")).FirstOrDefaultAsync();

                        #region update
                        if (existingRoleEntities != null && existingRoleEntities.Count() > 0)
                        {
                            var roleModuleToAdd = moduleIdSeparated
                                                           .Where(x =>
                                                                    !existingRoleEntities.Any(y =>
                                                                    Convert.ToInt32(x) == y.Moduleid
                                                                    )
                                                           )
                                                           .Select(a => a).ToList();

                            var permissionChangeOnModle = existingRoleEntities.Where(f => f.Permissionlevel != dicPermissionIds.GetValueOrDefault(f.Moduleid.Value)).ToList();
                            #region Role Update operation
                            var rolename = await _repositoryWrapper.RoleRepository.FindByCondition(x => x.Id == dto.RoleId).FirstOrDefaultAsync();

                            if (dto.RoleName != existingRoleEntities.Select(x => x.Role.Name).FirstOrDefault())
                            {
                                if(roleEntryExist != null)
                                {
                                    return new ResultDto
                                    {
                                        Warning = true,
                                        Info = ResultMessages.EntryAlreadyExists
                                    };
                                }
                                rolename.Name = dto.RoleName;
                                rolename.Normalizedname = dto.RoleName.ToUpper();
                                rolename.Description = dto.Description;
                                rolename.Abstractiontaborder = (dto.AbstractionRoleOrders != null && dto.AbstractionRoleOrders.Count > 0) ? JsonSerializer.Serialize(dto.AbstractionRoleOrders) : string.Empty;
                                _repositoryWrapper.RoleRepository.Update(rolename);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();
                            }
                            else
                            {
                                rolename.Description = dto.Description;
                                rolename.Abstractiontaborder = (dto.AbstractionRoleOrders != null && dto.AbstractionRoleOrders.Count > 0) ? JsonSerializer.Serialize(dto.AbstractionRoleOrders) : string.Empty;
                                _repositoryWrapper.RoleRepository.Update(rolename);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();
                            }
                            #endregion

                            #region Add New Modules for the existing role

                            if (roleModuleToAdd != null && roleModuleToAdd.Count() > 0)
                            {
                                var entityToAdd = roleModuleToAdd
                                     .Select(mId => new Aspnetuserrolepermissions
                                     {
                                         Roleid = dto.RoleId,
                                         Moduleid = Convert.ToInt32(mId),
                                         Permissionlevel = dicPermissionIds.GetValueOrDefault(mId),
                                     }).ToList();

                                _repositoryWrapper.AspNetUserRolePermissionsRepository.BulkCreate(entityToAdd);
                                await _repositoryWrapper.SaveAsync();
                            }

                            #endregion

                            #region // Update permission for modules
                            if(permissionChangeOnModle != null && permissionChangeOnModle.Count > 0)
                            {
                                permissionChangeOnModle.ForEach(c => { c.Permissionlevel = dicPermissionIds.GetValueOrDefault(c.Moduleid.Value); c.Role = null; });
                                _repositoryWrapper.AspNetUserRolePermissionsRepository.BulkUpdate(permissionChangeOnModle);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();
                            }
                            #endregion

                            #region Remove Modules for the existing role
                            List<Aspnetuserrolepermissions> recordsToDelete = existingRoleEntities
                                                                             .Where(y => !moduleIdSeparated.Any(x =>
                                                                                         Convert.ToInt32(x) == y.Moduleid))
                                                                             //.DistinctBy(x => x.Roleid)
                                                                             .ToList();

                            foreach (Aspnetuserrolepermissions record in recordsToDelete)
                            {
                                record.Roleid = null;
                                record.Moduleid = null;
                                record.Role = null;
                                _repositoryWrapper.AspNetUserRolePermissionsRepository.DeleteDeep(record);
                            }

                            if (recordsToDelete.Any())
                            {
                                await _repositoryWrapper.SaveAsync();
                            }
                            #endregion

                            await transaction.CommitAsync();
                            await _repositoryWrapper.ClearTracker();
                            return new ResultDto { Warning = false, Info = ResultMessages.EntryAddUpdateSuccess, Data = existingRoleEntities };
                        }
                        #endregion

                        #region add
                        else
                        {

                            #region Role Create Operation
                            if (dto.RoleId == 0)
                            {                              

                                if (roleEntryExist != null)
                                {
                                    return new ResultDto
                                    {
                                        Warning = true,
                                        Info = ResultMessages.EntryAlreadyExists,
                                        Data = dto.RoleName,
                                    };
                                }
                                var roleEntity = new Aspnetroles()
                                {
                                    Name = dto.RoleName,
                                    Normalizedname = dto.RoleName.ToUpper(),
                                    Description = dto.Description,
                                    Abstractiontaborder = dto.AbstractionRoleOrders != null && dto.AbstractionRoleOrders.Count > 0 ?
                                    JsonSerializer.Serialize(dto.AbstractionRoleOrders) : string.Empty,
                                };
                                _repositoryWrapper.RoleRepository.Create(roleEntity);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();
                                dto.RoleId = roleEntity.Id;
                            }
                            #endregion

                            var entityToAdd = moduleIdSeparated
                                             .Select(mId => new Aspnetuserrolepermissions
                                             {
                                                 Roleid = dto.RoleId,
                                                 Moduleid = Convert.ToInt32(mId),
                                                 Permissionlevel = dicPermissionIds.GetValueOrDefault(mId),
                                             }).ToList();

                            _repositoryWrapper.AspNetUserRolePermissionsRepository.BulkCreate(entityToAdd);
                            await _repositoryWrapper.SaveAsync();
                            await transaction.CommitAsync();
                            return new ResultDto { Warning = false, Info = ResultMessages.EntryAddUpdateSuccess, Data = existingRoleEntities };
                        }
                        #endregion

                       
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        return new ResultDto
                        {
                            Info = ResultMessages.SystemError,
                            Warning = true
                        };
                    }
                }
            }
            else
            {
                return new ResultDto { Warning = false, Info = ResultMessages.NoModuleSelected };
            }
        }

        public ModelPermissionDetials SplitModelAndPermissionDetails(string ids)
        {
            var result = new ModelPermissionDetials();

            if (string.IsNullOrWhiteSpace(ids))
                return result;

            var matches = Regex.Matches(ids, @"\((\d+),(\d+)\)");

            foreach (Match match in matches)
            {
                int moduleId = int.Parse(match.Groups[1].Value);
                short permissionId = short.Parse(match.Groups[2].Value);

                result.ModuleId.Add(moduleId);

                if (!result.PermissionIds.ContainsKey(moduleId))
                {
                    result.PermissionIds.Add(moduleId, permissionId);
                }
            }

            return result;
        }

        public async Task<AspnetUserRoleModuleMappingUpdateDto> GetUpdatedPage()
        {
            var roleModuleMappingCommaSeparated = new AspnetUserRoleModuleMappingUpdateDto();
            var existedRoleModuleEntity = await _repositoryWrapper.RoleRepository.FindAll().Include(x=>x.Aspnetuserrolepermissions).ToListAsync();

            if (existedRoleModuleEntity != null && existedRoleModuleEntity.Count()>0)
            {
                var modulesEntity = existedRoleModuleEntity
                                     .Select(y => new UserRoleModuleMappingDto
                                     {
                                         RoleId = y.Id,
                                         RoleDescription = y.Name,
                                         Description = y.Description,
                                         AbstractionRoleOrders = !y.Abstractiontaborder.IsNullOrEmpty() ? JsonSerializer.Deserialize<List<AbstractionRoleOrder>>(y.Abstractiontaborder) : new List<AbstractionRoleOrder>(),
                                         ModuleId = y?.Aspnetuserrolepermissions != null ? string.Join(",", y.Aspnetuserrolepermissions?
                                                            .Select(y1 => $"({y1.Moduleid},{y1.Permissionlevel})").ToList()) : string.Empty,
                                         //PermissionLevel = y?.
                                     }).ToList();

                roleModuleMappingCommaSeparated.RoleModuleResources = modulesEntity;
            }

            roleModuleMappingCommaSeparated.ModuleResources = await _dropdownManager.GetAllAspnetModules();
            return roleModuleMappingCommaSeparated;
        }

    }
}
