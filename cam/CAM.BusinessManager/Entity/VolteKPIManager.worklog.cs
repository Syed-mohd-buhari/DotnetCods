using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.ExtensionMethod.VolteKPI;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.VolteKPI;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using CAM.Infrastucture;
using CAM.Infrastucture.Enums;
using CAM.Mail;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Threading.Tasks;
using CAM.Entities.Models.Mail;
using Microsoft.EntityFrameworkCore.Query;
using CAM.Entities.Mappers.Entity;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Mail;

namespace CAM.BusinessManager.Entity
{
    public partial class VolteKPIManager
    {
        public async Task<ResultDto> Add(VolteKPIWorklogDto dto, string hostName, bool? forced = false)
        {
            ResultDto retValue = null;

            
            retValue = await AddBase(dto);

            //Gestione Mail
            if (!retValue.Warning && dto.IsStored == false)
            {
                var users = _userManager.GetKPIAdminForOpCo(dto.OpCoId);//_currentUserService.GetKPIAdminForOpCo(dto.OpCoId);
               
                users = users.Where(x => !x.Email.Contains("userscam.onmicrosoft.com")).ToList();
                if (users != null && users.Count > 0)
                {
                    switch ((VolteKPIType)dto.VolteKPIType)
                    {
                        case VolteKPIType.KPI3_Penetration:
                        case VolteKPIType.KPI1_Capacity:
                            retValue = await SendMailOnSaveOrEdit(users, dto, _currentUserService.EMail, hostName);
                            break;
                        case VolteKPIType.KPI2_Current_Utilization:
                            break;
                        case VolteKPIType.KPI4_3G_ShutDown:
                            break;
                    }
                }
            }
            return retValue;

        }

        public async Task<ResultDto> AddBase(VolteKPIWorklogDto dto)
        {
            

            var entity = //_mapper.Map<VolteKPIWorklog>(dto);
            new VolteKPIWorklog
            {
                VolteKPIId = dto.VolteKPIId,
                OpCoId = dto.OpCoId,
                Month = dto.Month,
                Year = dto.Year,
                VolteKPIType = dto.VolteKPIType,
                TargetMonthlyValueOld = dto.TargetMonthlyValueOld,
                TargetMonthlyValueProposed = dto.TargetMonthlyValueProposed,
                TargetMonthlyValueNew = dto.TargetMonthlyValueNew,
                EoyTargetOld = dto.EoyTargetOld,
                EoyTargetNew = dto.EoyTargetNew,
                ActualMonthlyValueOld = dto.ActualMonthlyValueOld,
                ActualMonthlyValueNew = dto.ActualMonthlyValueNew,
                ActualNumberOfRegisteredOld = dto.ActualNumberOfRegisteredOld,
                ActualNumberOfRegisteredNew = dto.ActualNumberOfRegisteredNew,
                ActualNumberOfProvisionedOld = dto.ActualNumberOfProvisionedOld,
                ActualNumberOfProvisionedNew = dto.ActualNumberOfProvisionedNew,
                Comments = dto.Comments,
                IsStored = dto.IsStored,
                CreationDate = DateTime.Now,
                CreationUser = _currentUserService.UserId,
                Approved = dto.IsStored ? dto.Approved : null,
                Deleted = false
            };

            _repositoryWrapper.VolteKPIWorklog.Create(VolteKPIWorklogMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }

        //public async Task<ResultDto> Update(VolteKPIWorklogDto dto, bool? forced = false)
        //{
        //    var originalEntityWithSameNaturalKey = await _repositoryWrapper.VolteKPIWorklog
        //        .FindByCondition(x => x.VolteKPIWorklogId == dto.VolteKPIWorklogId, true)
        //        .SingleOrDefaultAsync();

        //    if (originalEntityWithSameNaturalKey == null)
        //    {
        //        return new ResultDto
        //        {
        //            Warning = true,
        //            Info = ResultMessages.EntryUpdateNotExists,
        //            Data = new { id = dto.VolteKPIId, orphanDeleted = false }
        //        };
        //    }
        //    return await UpdateBase(dto, forced);
        //}

        //private async Task<ResultDto> UpdateBase(VolteKPIWorklogDto dto, bool? forced)
        //{
        //    var entity = _mapper.Map<VolteKPIWorklog>(dto);

        //    if (forced == true)
        //    {
        //        entity.Deleted = false;
        //        entity.DeletionDate = null;
        //    }
        //    _repositoryWrapper.VolteKPIWorklog.Update(entity);
        //    await _repositoryWrapper.SaveAsync();

        //    return new ResultDto
        //    {
        //        Info = ResultMessages.EntryUpdateSuccess,
        //        Data = entity.VolteKPIId
        //    };
        //}

        private static ExpressionStarter<Voltekpiworklog> ApplyFilter(VolteKPIWorklogQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Voltekpiworklog>();
            var predicateInner = PredicateBuilder.New<Voltekpiworklog>();

            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.OpCo)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.KpiIdName != null && buildFilterDto.KpiIdName .Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.KpiIdName)
                    predicateInner.Or(x => x.Voltekpitype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MonthYear != null && buildFilterDto.MonthYear.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.MonthYear)
                {
                    string[] values = item.Split('/');
                    if (values != null && values.Length == 2)
                    {
                        short month = 0;
                        short.TryParse(values[0], out month);
                        short year = 0;
                        short.TryParse(values[1], out year);
                        if (month > 0 && year > 0)
                        {
                            predicateInner.Or(x => x.Month == month && x.Year == year);
                        }
                    }
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TargetMonthlyValueOld != null && buildFilterDto.TargetMonthlyValueOld.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.TargetMonthlyValueOld)
                    predicateInner.Or(x => x.Targetmonthlyvalueold.HasValue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TargetMonthlyValueProposed != null && buildFilterDto.TargetMonthlyValueProposed.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.TargetMonthlyValueProposed)
                    predicateInner.Or(x => x.Targetmonthlyvalueproposed.HasValue == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.TargetMonthlyValueNew != null && buildFilterDto.TargetMonthlyValueNew.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.TargetMonthlyValueNew)
                    predicateInner.Or(x => x.Targetmonthlyvaluenew.HasValue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EoyTargetOld != null && buildFilterDto.EoyTargetOld.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.EoyTargetOld)
                    predicateInner.Or(x => x.Eoytargetold.HasValue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EoyTargetNew != null && buildFilterDto.EoyTargetNew.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.EoyTargetNew)
                    predicateInner.Or(x => x.Eoytargetnew.HasValue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ActualMonthlyValueOld != null && buildFilterDto.ActualMonthlyValueOld.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.ActualMonthlyValueOld)
                    predicateInner.Or(x => x.Actualmonthlyvalueold.HasValue == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto.ActualMonthlyValueNew != null && buildFilterDto.ActualMonthlyValueNew.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.ActualMonthlyValueNew)
                    predicateInner.Or(x => x.Actualmonthlyvaluenew.HasValue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ActualNumberOfRegisteredOld != null && buildFilterDto.ActualNumberOfRegisteredOld.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.ActualNumberOfRegisteredOld)
                    predicateInner.Or(x => x.Actualnumberofregisteredold.HasValue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ActualNumberOfRegisteredNew != null && buildFilterDto.ActualNumberOfRegisteredNew.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.ActualNumberOfRegisteredNew)
                    predicateInner.Or(x => x.Actualnumberofregisterednew.HasValue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ActualNumberOfProvisionedOld != null && buildFilterDto.ActualNumberOfProvisionedOld.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.ActualNumberOfProvisionedOld)
                    predicateInner.Or(x => x.Actualnumberofprovisionedold.HasValue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ActualNumberOfProvisionedNew != null && buildFilterDto.ActualNumberOfProvisionedNew.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.ActualNumberOfProvisionedNew)
                    predicateInner.Or(x => x.Actualnumberofprovisionednew.HasValue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Comments != null && buildFilterDto.Comments.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.Comments)
                    predicateInner.Or(x => x.Comments == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Approved != null && buildFilterDto.Approved.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.Approved)
                    switch (item)
                    {
                        case 0: //Approved
                            predicateInner.Or(x => x.Approved.HasValue && x.Approved.Value);
                            break;
                        case 1: //Rejected
                            predicateInner.Or(x => x.Approved.HasValue && !x.Approved.Value);
                            break;
                        case 2: //NewRequest
                            predicateInner.Or(x => !x.Isstored);
                            break;
                    }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubmissionDate != null )
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                if (buildFilterDto.SubmissionDate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.SubmissionDate.StartDate);

                if (buildFilterDto.SubmissionDate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.SubmissionDate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubmittedBy != null && buildFilterDto.SubmittedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.SubmittedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IsStored != null && buildFilterDto.IsStored.Any())
            {
                predicateInner = PredicateBuilder.New<Voltekpiworklog>();
                foreach (var item in buildFilterDto.IsStored)
                    switch (item)
                    {
                        case 0: //Saved
                            predicateInner.Or(x => x.Isstored && x.Approved.HasValue);
                            break;
                        case 1: //Not Saved
                            predicateInner.Or(x => !x.Isstored);
                            break;
                        case 2:
                            predicateInner.Or(x => x.Isstored && !x.Approved.HasValue);
                            break;
                    }
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        public QueryResultDto<VolteKPIWorklogDto> FindWithCondition(VolteKPIWorklogQueryDto volteKPIFilterDto)
        {
            var opCodIds =
              (volteKPIFilterDto.OpCo != null && volteKPIFilterDto.OpCo.Any()) ?
                 _opcoResource.Where(x => volteKPIFilterDto.OpCo.Contains(x.OpCoId)).Select(o => o.OpCoId):
                 _opcoResource.Select(o => o.OpCoId);


            var predicateResult = ApplyFilter(volteKPIFilterDto);
            predicateResult = predicateResult.And(x => x.Deleted == false);
            predicateResult = predicateResult.And(x => opCodIds.Contains(x.Opcoid));

            var rtn = new QueryResultDto<VolteKPIWorklogDto>(new GenerateRenderForGrid<VolteKPIWorklogDto>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.VolteKPIWorklog.Count(predicateResult) : _repositoryWrapper.VolteKPIWorklog.Count(),
            };
            var query = GetQuery(predicateResult, volteKPIFilterDto.Deleted ?? false)
                    .OrderByDescending(x => x.ModificationDate)
                    .ApplyOrdering(volteKPIFilterDto, GetWorklogColumnsMap())
                    .ApplyPaging(volteKPIFilterDto);

            var data = query.ToList();

            List<VolteKPIWorklogDto> volteKPIResult = new List<VolteKPIWorklogDto>();

            var volteKPITypesResource = GetVolteKPITypesResource();

            foreach (VolteKPIWorklog rec in data)
            {
                VolteKPIWorklogDto volteKPIWorklogDto =
                new VolteKPIWorklogDto
                {
                    VolteKPIWorklogId = rec.VolteKPIWorklogId,
                    VolteKPIId = rec.VolteKPIId,
                    OpCoId = rec.OpCoId,
                    Month = rec.Month,
                    Year = rec.Year,
                    VolteKPIType = rec.VolteKPIType,
                    OpCo = rec.OpCo.OpCoDescription,
                    KpiIdName = rec.GetKPIName(),
                    MonthYear = rec.GetMonthYearStringFormat(), // $"{rec.Month.ToString("00")}/{rec.Year.ToString()}",
                    ActualNumberOfRegisteredOld = rec.ActualNumberOfRegisteredOld,
                    ActualNumberOfRegisteredNew = rec.ActualNumberOfRegisteredNew,
                    ActualNumberOfProvisionedOld = rec.ActualNumberOfProvisionedOld,
                    ActualNumberOfProvisionedNew = rec.ActualNumberOfProvisionedNew,
                    Comments = rec.Comments,
                    IsStored = rec.IsStored,
                    SubmissionDate = rec.ModificationDate,
                    SubmittedBy = rec.ModificationUserEntity.Email,
                    Approved = rec.IsStored ? rec.Approved : null,
                    Deleted = rec.Deleted,
                    LastModified = rec.ModificationDate,
                    LastModifiedBy = rec.ModificationUserEntity.Email,
                    Orphan = true
                };
                switch ((VolteKPIType)rec.VolteKPIType)
                {
                    case VolteKPIType.KPI2_Current_Utilization:
                    case VolteKPIType.KPI1_Capacity:
                        volteKPIWorklogDto.TargetMonthlyValueOld = rec.TargetMonthlyValueOld;
                        volteKPIWorklogDto.TargetMonthlyValueProposed = rec.TargetMonthlyValueProposed;
                        volteKPIWorklogDto.TargetMonthlyValueNew = rec.TargetMonthlyValueNew;
                        volteKPIWorklogDto.EoyTargetOld = rec.EoyTargetOld;
                        volteKPIWorklogDto.EoyTargetNew = rec.EoyTargetNew;
                        volteKPIWorklogDto.ActualMonthlyValueOld = rec.ActualMonthlyValueOld;
                        volteKPIWorklogDto.ActualMonthlyValueNew = rec.ActualMonthlyValueNew;
                        break;
                    case VolteKPIType.KPI3_Penetration:
                    case VolteKPIType.KPI4_3G_ShutDown:
                        volteKPIWorklogDto.TargetMonthlyValueOld = rec.TargetMonthlyValueOld != null ? (decimal?)(int)(rec.TargetMonthlyValueOld * 100) : null;
                        volteKPIWorklogDto.TargetMonthlyValueProposed = rec.TargetMonthlyValueProposed != null ? (decimal?)(int)(rec.TargetMonthlyValueProposed * 100) : null;
                        volteKPIWorklogDto.TargetMonthlyValueNew = rec.TargetMonthlyValueNew != null ? (decimal?)(int)(rec.TargetMonthlyValueNew * 100) : null;
                        volteKPIWorklogDto.EoyTargetOld = rec.EoyTargetOld != null ? (decimal?)(int)(rec.EoyTargetOld * 100) : null;
                        volteKPIWorklogDto.EoyTargetNew = rec.EoyTargetNew != null ? (decimal?)(int)(rec.EoyTargetNew * 100) : null;
                        volteKPIWorklogDto.ActualMonthlyValueOld = rec.ActualMonthlyValueOld != null ? (decimal?)(int)(rec.ActualMonthlyValueOld * 100) : null;
                        volteKPIWorklogDto.ActualMonthlyValueNew = rec.ActualMonthlyValueNew != null ? (decimal?)(int)(rec.ActualMonthlyValueNew * 100) : null;
                        break;
                }
                volteKPIResult.Add(volteKPIWorklogDto);
            }

            //volteKPIResult = _mapper.Map<IEnumerable<VolteKPIWorklogDto>>(data);

            rtn.Items = volteKPIResult.ToArray();
            return rtn;
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, VolteKPIWorklogQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "opCo" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.OpCo.OpCoDescription,
                        Value = p.OpCoId.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x => x.OpCo.OpCoDescription.ToUpper().Contains(propertyFilter.ToUpper()))
                        .Select(p => new FilterValueDto
                        {
                            Text = p.OpCo.OpCoDescription,
                            Value = p.OpCoId.ToString()
                        }).Distinct().ToList(),

                "kpiIdName" => string.IsNullOrEmpty(propertyFilter)
                    ? System.Enum.GetValues(typeof(VolteKPIType)).Cast<VolteKPIType>().Select(p => new FilterValueDto((int)p, p.GetDescription())).ToList()
                    : System.Enum.GetValues(typeof(VolteKPIType)).Cast<VolteKPIType>().Select(p => new FilterValueDto((int)p, p.GetDescription())).Where(x => x.Text.ToUpper().Contains(propertyFilter.ToUpper())).ToList(),
                "monthYear" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Select(p => new FilterValueDto(p.GetMonthYear(), p.GetMonthYearStringFormat())).Distinct().ToList()
                    : query.ToList()
                        .Where(x => x.GetMonthYearStringFormat().ToUpper().Contains(propertyFilter.ToUpper()))
                        .Select(p => new FilterValueDto(p.GetMonthYear(), p.GetMonthYearStringFormat())).Distinct().ToList(),
                "targetMonthlyValueOld" => new List<FilterValueDto> { new FilterValueDto(true, "Valorized"), new FilterValueDto(false, "Not Valorized") },
                "targetMonthlyValueProposed" => new List<FilterValueDto> { new FilterValueDto(true, "Valorized"), new FilterValueDto(false, "Not Valorized") },
                "targetMonthlyValueNew" => new List<FilterValueDto> { new FilterValueDto(true, "Valorized"), new FilterValueDto(false, "Not Valorized") },
                "eoyTargetOld" => new List<FilterValueDto> { new FilterValueDto(true, "Valorized"), new FilterValueDto(false, "Not Valorized") },
                "eoyTargetNew" => new List<FilterValueDto> { new FilterValueDto(true, "Valorized"), new FilterValueDto(false, "Not Valorized") },
                "actualMonthlyValueOld" => new List<FilterValueDto> { new FilterValueDto(true, "Valorized"), new FilterValueDto(false, "Not Valorized") },
                "actualMonthlyValueNew" => new List<FilterValueDto> { new FilterValueDto(true, "Valorized"), new FilterValueDto(false, "Not Valorized") },
                "actualNumberOfRegisteredOld" => new List<FilterValueDto> { new FilterValueDto(true, "Valorized"), new FilterValueDto(false, "Not Valorized") },
                "actualNumberOfRegisteredNew" => new List<FilterValueDto> { new FilterValueDto(true, "Valorized"), new FilterValueDto(false, "Not Valorized") },
                "actualNumberOfProvisionedOld" => new List<FilterValueDto> { new FilterValueDto(true, "Valorized"), new FilterValueDto(false, "Not Valorized") },
                "actualNumberOfProvisionedNew" => new List<FilterValueDto> { new FilterValueDto(true, "Valorized"), new FilterValueDto(false, "Not Valorized") },
                "comments" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Select(p => new FilterValueDto(p.Comments)).Distinct().ToList()
                    : query.ToList()
                        .Where(x => x.Comments.ToUpper().Contains(propertyFilter.ToUpper()))
                        .Select(p => new FilterValueDto(p.Comments)).Distinct().ToList(),
                "approved" => new List<FilterValueDto> { new FilterValueDto(0, "Approved"), new FilterValueDto(1, "Rejected"), new FilterValueDto(2, "Pending Request") },
                "submittedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : query.ToList()
                        .Where(x => x.ModificationUserEntity.Email.ToUpper().Contains(propertyFilter.ToUpper()))
                        .Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "isStored" => new List<FilterValueDto> { new FilterValueDto(0, "Completed"), new FilterValueDto(1, "Pending"), new FilterValueDto(2, "No Approval Required") },

                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

        private IQueryable<VolteKPIWorklog> GetQuery(ExpressionStarter<Voltekpiworklog> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted ? _repositoryWrapper.VolteKPIWorklog.FindByCondition(predicateResult, includeDeleted)
                    .Include(x => x.Opco)
                    .Include(x => x.ModificationuserNavigation)
                    .Include(x => x.CreationuserNavigation)
                :
                _repositoryWrapper.VolteKPIWorklog.FindAll()
                    .Include(x => x.Opco)
                    .Include(x => x.ModificationuserNavigation)
                    .Include(x => x.CreationuserNavigation);
           var result = query.AsEnumerable().Select(p => VolteKPIWorklogMapper.Get(p)).AsEnumerable().AsQueryable();
            return result;
        }

        private Dictionary<string, Expression<Func<VolteKPIWorklog, object>>[]> GetWorklogColumnsMap()
        {
            return new Dictionary<string, Expression<Func<VolteKPIWorklog, object>>[]>
            {
                ["opCo"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.OpCoId },
                ["kpiIdName"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.GetKPIName() },
                ["monthYear"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.GetMonthYearStringFormat() },
                ["targetMonthlyValueOld"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.TargetMonthlyValueOld },
                ["targetMonthlyValueProposed"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.TargetMonthlyValueProposed },
                ["targetMonthlyValueNew"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.TargetMonthlyValueNew },
                ["eoyTargetOld"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.EoyTargetOld },
                ["eoyTargetNew"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.EoyTargetNew },
                ["actualMonthlyValueOld"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.ActualMonthlyValueOld },
                ["actualMonthlyValueNew"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.ActualMonthlyValueNew },
                ["actualNumberOfRegisteredOld"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.ActualNumberOfRegisteredOld },
                ["actualNumberOfRegisteredNew"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.ActualNumberOfRegisteredNew },
                ["actualNumberOfProvisionedOld"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.ActualNumberOfProvisionedOld },
                ["actualNumberOfProvisionedNew"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.ActualNumberOfProvisionedNew },
                ["comments"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.Comments },
                ["approved"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.Approved },
                ["submittedBy"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.ModificationUserEntity.Email },
                ["isStored"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.IsStored },
                ["lastModifiedBy"] = new Expression<Func<VolteKPIWorklog, object>>[] { p => p.IsStored },
            };
        }

        public async Task<ResultDto> GetDuplicates(long VolteKPIId, int VolteKPIType, long VolteKPIWorklogId)
        {

            var duplicates = await _repositoryWrapper.VolteKPIWorklog
                .FindByCondition(x => x.Voltekpiid == VolteKPIId && x.Isstored == false && x.Voltekpitype == VolteKPIType && x.Voltekpiworklogid != VolteKPIWorklogId,
                    true).ToListAsync();
            if (duplicates.Any())
            {
                return new ResultDto
                {
                    Warning = false,
                    Info = "",
                    Data = true
                };
            }

            return new ResultDto
            {
                Warning = false,
                Info = "",
                Data = false
            };


        }


        public async Task<ResultDto> Update(VolteKPIWorklogDto dto)
        {

            var originalEntityWithSameNaturalKey = await _repositoryWrapper.VolteKPIWorklog
                .FindByCondition(x => x.Voltekpiworklogid == dto.VolteKPIWorklogId && x.Isstored == false, true)
                .Include(x => x.ModificationuserNavigation)
                .SingleOrDefaultAsync();

            if (originalEntityWithSameNaturalKey == null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.NoVolteKPIWorklog,
                    Data = dto.VolteKPIWorklogId
                };
            }

            var volteKpiEntity = await _repositoryWrapper.VolteKPI
                .FindByCondition(x => x.Voltekpiid == dto.VolteKPIId)
                .Include(x => x.ModificationuserNavigation)
                .SingleOrDefaultAsync();

            if (volteKpiEntity == null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.NoVolteKPI,
                    Data = dto.VolteKPIId
                };
            }

            ApplicationUser lastModificationUser = ApplicationUserMapper.GetApplicationUserMapper(originalEntityWithSameNaturalKey.ModificationuserNavigation);

            //Update Approval
            switch ((VolteKPIType)dto.VolteKPIType)
            {
                case VolteKPIType.KPI2_Current_Utilization:
                case VolteKPIType.KPI1_Capacity:
                    originalEntityWithSameNaturalKey.Targetmonthlyvaluenew = dto.TargetMonthlyValueNew;
                    originalEntityWithSameNaturalKey.Eoytargetnew = dto.EoyTargetNew;
                    originalEntityWithSameNaturalKey.Actualmonthlyvaluenew = dto.ActualMonthlyValueNew;
                    break;
                case VolteKPIType.KPI3_Penetration:
                case VolteKPIType.KPI4_3G_ShutDown:
                    originalEntityWithSameNaturalKey.Targetmonthlyvaluenew = dto.TargetMonthlyValueNew != null ? (dto.TargetMonthlyValueNew / (decimal)100.0) : null;
                    originalEntityWithSameNaturalKey.Eoytargetnew = dto.EoyTargetNew != null ? (dto.EoyTargetNew / (decimal)100.0) : null;
                    originalEntityWithSameNaturalKey.Actualmonthlyvaluenew = dto.ActualMonthlyValueNew != null ? (dto.ActualMonthlyValueNew / (decimal)100.0) : null;
                    break;
            }
            originalEntityWithSameNaturalKey.Actualnumberofprovisionednew = dto.ActualNumberOfProvisionedNew;
            originalEntityWithSameNaturalKey.Actualnumberofregisterednew = dto.ActualNumberOfRegisteredNew;
            originalEntityWithSameNaturalKey.Approved = dto.Approved ?? false;
            originalEntityWithSameNaturalKey.Isstored = true;
            originalEntityWithSameNaturalKey.Modificationdate = DateTime.Now;
            originalEntityWithSameNaturalKey.Modificationuser = _currentUserService.UserId;
            originalEntityWithSameNaturalKey.ModificationuserNavigation = null;
            originalEntityWithSameNaturalKey.Opco = null;
            originalEntityWithSameNaturalKey.CreationuserNavigation = null;

            _repositoryWrapper.VolteKPIWorklog.Update(originalEntityWithSameNaturalKey);
            await _repositoryWrapper.SaveAsync();

            var retValue = new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = originalEntityWithSameNaturalKey.Voltekpiworklogid
            };

            if (dto.Approved.HasValue)
            {
                switch ((VolteKPIType)dto.VolteKPIType)
                {
                    case VolteKPIType.KPI1_Capacity:
                        //if (dto.Approved.Value)
                        {
                            volteKpiEntity.Kpionemonthlytarget = dto.TargetMonthlyValueNew;
                        }
                        volteKpiEntity.Kpi1targetcluchngproposal = null;
                        volteKpiEntity.Kpionecomment = "";
                        break;
                    case VolteKPIType.KPI2_Current_Utilization:
                        volteKpiEntity.Kpitwocomment = "";
                        break;
                    case VolteKPIType.KPI3_Penetration:
                        //if (dto.Approved.Value)
                        {
                            volteKpiEntity.Kpithreemonthlytarget = dto.TargetMonthlyValueNew / (decimal)100.0;
                        }
                        volteKpiEntity.Kpi3targetvluchngproposal = null;
                        volteKpiEntity.Kpithreecomment = "";
                        break;
                    case VolteKPIType.KPI4_3G_ShutDown:
                        volteKpiEntity.Kpifourcomment = "";
                        break;
                }

                volteKpiEntity.CreationuserNavigation = null;
                volteKpiEntity.ModificationuserNavigation = null;
                volteKpiEntity.Opco = null;

                _repositoryWrapper.VolteKPI.Update(volteKpiEntity);
                await _repositoryWrapper.SaveAsync();

                retValue = new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = volteKpiEntity.Voltekpiid
                };

                if (!retValue.Warning && !lastModificationUser.Email.Contains("userscam.onmicrosoft.com"))
                {
                    retValue = await SendMailOnApproval(dto.Approved.Value, lastModificationUser, dto);
                }
            }
            return retValue;
        }

        private async Task<ResultDto> SendMailOnApproval(bool approve, ApplicationUser lastModificationUser, VolteKPIWorklogDto originalRecord)
        {
            var from = _configuration["ApplicationSettings:MailSender"];
            var mailScheduled = bool.Parse(_configuration["ApplicationSettings:SendScheduled"]);

            decimal kpiValue = originalRecord.TargetMonthlyValueOld ?? 0;
            bool approveWithReserve = !approve && (kpiValue != originalRecord.TargetMonthlyValueNew);
            string skpiValue = (originalRecord.VolteKPIType == (int)VolteKPIType.KPI3_Penetration) ? $"{(kpiValue * 100).ToString("###,#0")} %" : $"{kpiValue.ToString("###,#0")} [K SUBS]";

            var body = $"Hello {lastModificationUser.UserName},<br />" +
                $"<br />Modification for <b>{originalRecord.KpiIdName}</b><br /><br />" +
                $"<table>" +
                $"  <tr>" +
                $"      <td>Selected Month</td>" +
                $"      <td>:</td>" +
                $"      <td><b>{originalRecord.Month + "/" + originalRecord.Year}</b></td>" +
                $"  </tr>" +
                $"  <tr>" +
                $"      <td>Selected OpCo</td>" +
                $"      <td>:</td>" +
                $"      <td><b>{originalRecord.OpCo}</b></td>" +
                $"  </tr>" +
                $"  <tr>" +
                $"      <td>Previous Target Value</td>" +
                $"      <td>:</td>" +
                $"      <td><b>{skpiValue}</b></td>" +
                $"  </tr>" +
                $"  <tr>" +
                $"      <td>Comment</td>" +
                $"      <td>:</td>" +
                $"      <td><b>{originalRecord.Comments}</b></td>" +
                $"  </tr>" +
                $"</table>" +
                $"<br />" +
                $"<h1 style=\"color: {(approve ? "green" : (approveWithReserve ? "yellow" : "red"))};\">{(approve ? "Was Approved" : (approveWithReserve ? "Approved With Reservation" : "Was Not Approved"))}</h1>" +
                $"<br />" +
                $"The new value for Monthly Target Is : <b>{(approve || approveWithReserve ? originalRecord.TargetMonthlyValueNew?.ToString("###,#0.000") : "INVARIATED")}</b>";


            MailQueue mailQueue = new MailQueue
            {
                From = from,
                Subject = "VoLTE KPI Monthly Target Variation",
                Body = body,
                To = lastModificationUser.Email,
                State = mailScheduled ? MailStateEnum.Insert : MailStateEnum.Sending,
                ApprovalUserId = _currentUserService.UserId
            };

            var mailQueueValue = MailQueueMapper.SetMailQueueMapper(mailQueue);
            _repositoryWrapper.MailQueue.Create(mailQueueValue);
            await _repositoryWrapper.SaveAsync();
            ResultDto resultDto = new ResultDto
            {
                Warning = false,
                Info = ResultMessages.MailQueueInsert,
                Data = originalRecord.VolteKPIId
            };

            if (!mailScheduled)
            {
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(mailQueue.From),
                    Subject = mailQueue.Subject,
                    Body = mailQueue.Body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(mailQueue.To);
                try
                {
                    _mailManager.SendMail(mailMessage);
                    mailQueueValue.State = (int)MailStateEnum.Sended;
                    mailQueueValue.Errormessage = "";
                    resultDto.Warning = false;
                    resultDto.Info = ResultMessages.MailQueueSended;
                }
                catch (Exception ex)
                {
                    mailQueueValue.State = (int)MailStateEnum.ErrorOnSend;
                    mailQueueValue.Errormessage = ex.ToString();
                    resultDto.Warning = true;
                    resultDto.Info = ResultMessages.MailQueueErrorOnSend;
                }
                _repositoryWrapper.MailQueue.Update(mailQueueValue);
                await _repositoryWrapper.SaveAsync();
            }
            return resultDto;
        }
        private async Task<ResultDto> SendMailOnSaveOrEdit(List<Aspnetusers> kpiAdministrators, VolteKPIWorklogDto entity, string lastModificationUserEmail, string hostName)
        {
            var from = _configuration["ApplicationSettings:MailSender"];
            var mailScheduled = bool.Parse(_configuration["ApplicationSettings:SendScheduled"]);

            ResultDto resultDto = new ResultDto
            {
                Warning = false,
                Info = "",
                Data = entity.VolteKPIId
            };

            VolteKPIType volteKPIType = (VolteKPIType)entity.VolteKPIType;
            decimal kpiValue = entity.TargetMonthlyValueProposed ?? 0;
            string kpiName = entity.KpiIdName;
            string kpiComment = entity.Comments;

            

            string skpiValue = (volteKPIType == VolteKPIType.KPI3_Penetration) ? $"{(kpiValue * 100).ToString("###,#0")} %" : $"{kpiValue.ToString("###,#0")} [K SUBS]";
            foreach (var kpiAdmin in kpiAdministrators)
            {


                var body = $"Hello {kpiAdmin.Username},<br />" +
                    $"<br />Request approval for <b>{kpiName}</b><br /><br />" +
                    $"<table>" +
                    $"  <tr>" +
                    $"      <td>Selected Month</td>" +
                    $"      <td>:</td>" +
                    $"      <td><b>{entity.Month + "/" + entity.Year}</b></td>" +
                    $"  </tr>" +
                    $"  <tr>" +
                    $"      <td>Selected OpCo</td>" +
                    $"      <td>:</td>" +
                    $"      <td><b>{entity.OpCo}</b></td>" +
                    $"  </tr>" +
                    $"  <tr>" +
                    $"      <td>Proposal Monthly Target Value</td>" +
                    $"      <td>:</td>" +
                    $"      <td><b>{skpiValue}</b></td>" +
                    $"  </tr>" +
                    $"  <tr>" +
                    $"      <td>Comment</td>" +
                    $"      <td>:</td>" +
                    $"      <td><b>{kpiComment}</b></td>" +
                    $"  </tr>" +
                    $"  <tr>" +
                    $"      <td>Request Submitted by</td>" +
                    $"      <td>:</td>" +
                    $"      <td><b>{lastModificationUserEmail}</b></td>" +
                    $"  </tr>" +
                    $"  <tr>" +
                    $"      <td>&nbsp;</td>" +
                    $"      <td>&nbsp;</td>" +
                    $"      <td>&nbsp;</td>" +
                    $"  </tr>" +
                    $"  <tr>" +
                    $"      <td>To approve</td>" +
                    $"      <td>:</td>" +
                    $"      <td><b><a href='{hostName}{"/targetmonthlyapprovals"}'>click here</a></b></td>" +
                    $"  </tr>" +
                    $"</table>" +
                    $"<br />";


                MailQueue mailQueue = new MailQueue
                {
                    From = from,
                    Subject = "VoLTE KPI Monthly Target Proposal",
                    Body = body,
                    To = kpiAdmin.Email,
                    State = mailScheduled ? MailStateEnum.Insert : MailStateEnum.Sending,
                    ApprovalUserId = kpiAdmin.Id  //_currentUserService.UserId
                };
                var mailQueueValue = MailQueueMapper.SetMailQueueMapper(mailQueue);
                _repositoryWrapper.MailQueue.Create(mailQueueValue);
                await _repositoryWrapper.SaveAsync();


                if (!mailScheduled)
                {
                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(mailQueue.From),
                        Subject = mailQueue.Subject,
                        Body = mailQueue.Body,
                        IsBodyHtml = true,
                    };
                    mailMessage.To.Add(mailQueue.To);
                    try
                    {
                        _mailManager.SendMail(mailMessage);
                        mailQueueValue.State = (int)MailStateEnum.Sended;
                        mailQueueValue.Errormessage = "";
                        resultDto.Warning |= false;
                        resultDto.Info += $"{kpiAdmin.Email} - {ResultMessages.MailQueueSended}\n";
                    }
                    catch (Exception ex)
                    {
                        mailQueueValue.State = (int)MailStateEnum.ErrorOnSend;
                        mailQueueValue.Errormessage = ex.ToString();
                        resultDto.Warning |= true;
                        resultDto.Info += $"{kpiAdmin.Email} - {ResultMessages.MailQueueErrorOnSend}\n";
                    }
                    _repositoryWrapper.MailQueue.Update(mailQueueValue);
                    await _repositoryWrapper.SaveAsync();
                }
                else
                {
                    resultDto.Warning |= false;
                    resultDto.Info += $"{kpiAdmin.Email} - {ResultMessages.MailQueueInsert}\n";
                }
            }
            return resultDto;
        }
    }
}
