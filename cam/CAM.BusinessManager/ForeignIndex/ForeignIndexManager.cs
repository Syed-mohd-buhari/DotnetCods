using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.ForeignIndex;
using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.ForeignIndex;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.ForeignIndex;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.ForeignIndex
{
    public class ForeignIndexManager:BaseManager
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly LcmEngineeringManager _lcmEngineeringManager;
        private GridCustomColumnManager manager;

        public ForeignIndexManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper,
            GridCustomColumnManager manager, LcmEngineeringManager lcmEngineeringManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper):base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            this.manager = manager;
            _lcmEngineeringManager = lcmEngineeringManager;
        }

        public async Task<ResultDto<FI_Session>> CheckSession(long? sessionId, ForeignIndexStatus? foreignIndexStatus)
        {
            #if DEBUG
                var data = _repositoryWrapper.FI_Sessions.FindByCondition(x => x.Sessionid == sessionId).FirstOrDefault();
                var session =FI_SessionMapper.Get(data);
                if (session == null && !foreignIndexStatus.HasValue)
                {
                    session = new FI_Session { Status = ForeignIndexStatus.DesignComponentOrphan, Source = ForeignIndexSource.NotSpecified };
                   var model = FI_SessionMapper.Set(session);
                   _repositoryWrapper.FI_Sessions.Create(model);
                    await _repositoryWrapper.SaveAsync();
                   session = FI_SessionMapper.Get(model);
            }

                return new ResultDto<FI_Session>
                {
                    Data = session
                };


            #else
            var session = _repositoryWrapper.FI_Sessions.FindByCondition(x =>
                x.Status != (int)ForeignIndexStatus.Canceled && x.Status != (int)ForeignIndexStatus.Committed ||
                x.Sessionid == sessionId).Include(x=> x.CreationuserNavigation).FirstOrDefault();
            if (session != null && session.Sessionid != sessionId)
                return new ResultDto<FI_Session>
                {
                    Data = null,
                    Info = $"There is another Foreign Index session pending from {session.CreationuserNavigation.Email}.",
                    Warning = true
                };

            if (session != null && session.Sessionid == sessionId && (session.Status == (int)ForeignIndexStatus.Canceled ||
                                                                      session.Status == (int)ForeignIndexStatus.Committed))
                return new ResultDto<FI_Session>
                {
                    Data = null,
                    Info = $"This Foreign Index session was {session.Status.ToString()}.",
                    Warning = true
                };

            if (session == null && !foreignIndexStatus.HasValue)
            {
                session = FI_SessionMapper.Set( new FI_Session{ Status = (int)ForeignIndexStatus.DesignComponentOrphan, Source = (int)ForeignIndexSource.NotSpecified });
                _repositoryWrapper.FI_Sessions.Create(session);
                await _repositoryWrapper.SaveAsync();
            }

            if (session != null && (!foreignIndexStatus.HasValue || foreignIndexStatus.Value == (ForeignIndexStatus)session.Status))
                return new ResultDto<FI_Session>
                {
                    Data = FI_SessionMapper.Get( session),
                    Info = "",
                    Warning = false
                };
            return new ResultDto<FI_Session>
            {
                Data = null,
                Info = $"This Foreign Index has wrong status {session.Status.ToString()}.",
                Warning = true
            };
#endif
        }


        public async Task<ResultDto<ForeignIndexDto>> GetOrphanDesignComponents(long? sessionId)
        {
            var resCheck = await CheckSession(sessionId, null);
            if (resCheck.Warning)
                return new ResultDto<ForeignIndexDto>
                {
                    Data = null,
                    Info = resCheck.Info,
                    Warning = true
                };
            var session = resCheck.Data;

            List<FI_DesignComponent> orphans;
            List<FiDesigncomponents> data;
            if (sessionId != null)
            {
                data = _repositoryWrapper.FI_DesignComponents.FindByCondition(x => x.Sessionid == sessionId.Value)
                    .ToList();
                orphans = data.Select(p => FI_DesignComponentMapper.Get(p)).ToList();
            }
            else
            {
                orphans = _repositoryWrapper.DesignComponent.FindAll()
                    .Include(x => x.Lcmengineering)
                    .Include(x => x.Plannedactivities)
                    .Include(x => x.Networkelementsasplanned)
                    .Where(x =>
                        (x.Lcmengineering == null || x.Lcmengineering.Count == 0) &&
                        (x.Plannedactivities == null || x.Plannedactivities.Count == 0) &&
                        (x.Networkelementsasplanned == null || x.Networkelementsasplanned.Count == 0)
                    )
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                    .ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x=>x.Subnetwrokboundarysystemfunction)
                    .ThenInclude(x => x.Systemfunction)
                    .ToList()
                    .Select(x => new FI_DesignComponent
                    {
                        DesignComponentId = x.Designcomponentid,
                        SessionId = session.SessionId,
                        Description = x.toDesignComponentNameLcm(_repositoryWrapper),
                        ToDelete = true
                    }).ToList();

                var toBeDeletedItems = _repositoryWrapper.FI_DesignComponents.FindByCondition(x => x.Sessionid == sessionId).ToList();
                foreach (var toDelete in toBeDeletedItems)
                {
                    _repositoryWrapper.FI_DesignComponents.DeleteDeep(toDelete);
                }
                await _repositoryWrapper.SaveAsync();
                var newItems = orphans.Select(p => FI_DesignComponentMapper.Set(p)).ToList();
                foreach (var toAdd in newItems)
                {
                    _repositoryWrapper.FI_DesignComponents.Create(toAdd);
                }
                await _repositoryWrapper.SaveAsync();
                orphans = newItems.Select(p => FI_DesignComponentMapper.Get(p)).ToList();
            }
            await _repositoryWrapper.ClearTracker();
            session.Status = ForeignIndexStatus.DesignComponentOrphan;
            var sessionModel = FI_SessionMapper.Set(session);
            _repositoryWrapper.FI_Sessions.Update(sessionModel);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto<ForeignIndexDto>
            {
                Data = new ForeignIndexDto
                {
                    Session = _mapper.Map<FI_SessionDto>(session),
                    DesignComponents = orphans.Select(x => _mapper.Map<FI_DesignComponentDto>(x)).ToList()
                },
                Info = "",
                Warning = false
            };
        }

        public async Task<ResultDto<ForeignIndexDto>> GetOrphanSystemTypes(ForeignIndexDto foreignIndexDto)
        {
            var sessionId = foreignIndexDto.Session.SessionId;
            var resCheck = await CheckSession(sessionId, ForeignIndexStatus.DesignComponentOrphan);
            if (resCheck.Warning)
                return new ResultDto<ForeignIndexDto>
                {
                    Data = null,
                    Info = resCheck.Info,
                    Warning = true
                };
            var session = resCheck.Data;

            foreach (var toUpdate in foreignIndexDto.DesignComponents)
            {
                var model = _mapper.Map<FI_DesignComponent>(toUpdate);
                _repositoryWrapper.FI_DesignComponents.Update(FI_DesignComponentMapper.Set(model));
            }
            await _repositoryWrapper.SaveAsync();

            var dcIds = foreignIndexDto.DesignComponents.Where(x => x.ToDelete).Select(x => x.DesignComponentId)
                .ToList();


            var orphans = _repositoryWrapper.SystemType.FindAll()
                .Include(x => x.Designcomponents)
                .Include(x => x.Networkelementsasis)
                .Where(x =>
                    (x.Designcomponents == null || x.Designcomponents.Count == 0 ||
                     x.Designcomponents.All(z => dcIds.Contains(z.Designcomponentid)))
                    && (x.Networkelementsasis == null || x.Networkelementsasis.Count == 0)
                )
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Platform)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .ToList()
                .Select(x => new FI_SystemTypes
                {
                    SystemTypesId = x.Systemtypeid,
                    SessionId = session.SessionId,
                    ToDelete = true,
                    DesignComponentId = x.Designcomponents?.FirstOrDefault()?.Designcomponentid,
                    Description = x.toSystemTypeName(_repositoryWrapper)
                }).ToList();

            foreach (var toDelete in _repositoryWrapper.FI_SystemTypes.FindByCondition(x => x.Sessionid == sessionId)
                .ToList()) _repositoryWrapper.FI_SystemTypes.DeleteDeep(toDelete);
            await _repositoryWrapper.SaveAsync();

            var items = orphans.Select(p => FI_SystemTypeMapper.Set(p)).ToList();
            foreach (var toAdd in items)
            {
                _repositoryWrapper.FI_SystemTypes.Create(toAdd);
            
            }

            await _repositoryWrapper.SaveAsync();

            orphans = items.Select(p => FI_SystemTypeMapper.Get(p)).ToList();

            foreignIndexDto.SystemTypes = orphans.Select(x => _mapper.Map<FI_SystemTypesDto>(x)).ToList();
            foreignIndexDto.Session.Status = ForeignIndexStatus.SystemTypeOrphan;

            var _fiSessions = _mapper.Map<FI_Session>(foreignIndexDto.Session);
            _repositoryWrapper.FI_Sessions.Update(FI_SessionMapper.Set(_fiSessions));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto<ForeignIndexDto>
            {
                Data = foreignIndexDto,
                Info = "",
                Warning = false
            };
        }

        public async Task<ResultDto<ForeignIndexDto>> GetGroups(ForeignIndexDto foreignIndexDto)
        {
            if (foreignIndexDto == null)
                return new ResultDto<ForeignIndexDto> { Data = null, Info = "Invalid parameters", Warning = true };
            var resCheck = await CheckSession(foreignIndexDto.Session.SessionId, foreignIndexDto.Session.Status);
            if (resCheck.Warning)
                return new ResultDto<ForeignIndexDto>
                {
                    Data = null,
                    Info = resCheck.Info,
                    Warning = true
                };
            var session = resCheck.Data;
            var systemTypesIds = foreignIndexDto?.SystemTypes?.Select(x => x.FI_SystemTypesId).ToList() ??
                                 new List<long>();
            var groupsOEM = new List<FIGroupKey>();
            switch (foreignIndexDto.Session.Source)
            {
                case ForeignIndexSource.DesignComponentFamily:
                    groupsOEM.Add(new FIGroupKey
                    {
                        OEM = -1,
                        EOM = DateTime.MinValue,
                        Records = _repositoryWrapper.DesignComponentFamily
                            .FindByCondition(x => x.Systemtypeidentityname.Trim() != "")
                            .Include(x => x.Designcomponents)
                            .Select(z => new FIGroupRecord
                            {
                                Correct = false,
                                Description = z.Systemtypeidentityname,
                                ToAnalyze = z.Systemtypeidentityname,
                                Key = z.Designcomponentfamilyid,
                                Selected = false,
                                ModificationDate = z.Modificationdate,
                                Count = z.Designcomponents != null ? z.Designcomponents.Count : 0
                            }).ToDictionary(d => d.Key, d => d)
                    });
                    break;
                case ForeignIndexSource.MajorHardwareBuild:
                    groupsOEM = _repositoryWrapper.MajorHardwareBuild
                        .FindByCondition(x => x.Endofmaintenance.HasValue)
                        .Include(x => x.Systemtypesmajorhardwarebuilds)
                        .Include(x => x.Platform)
                        .Include(x => x.Orgeqpmanufacturer)
                        .Where(x => x.Systemtypesmajorhardwarebuilds.Any(s => !systemTypesIds.Contains(s.Systemtypeid)))
                        .ToList().GroupBy(x => new { x.Orgeqpmanufacturerid, x.Endofmaintenance })
                        .Select(x => new FIGroupKey
                        {
                            OEM = x.Key.Orgeqpmanufacturerid,
                            EOM = x.Key.Endofmaintenance.Value,
                            Records = x.Select(z => new FIGroupRecord
                            {
                                Correct = false,
                                Description =
                                    $"{z.Orgeqpmanufacturer.Originalequipmentmanufacturer} - {z.Platform.Platform} - {z.Hardwaretype}",
                                ToAnalyze = $"{z.Hardwaretype}",
                                Key = z.Majorhardwareid,
                                Selected = false,
                                ModificationDate = z.Modificationdate,
                                Count = z.Systemtypesmajorhardwarebuilds != null
                                    ? z.Systemtypesmajorhardwarebuilds.Count
                                    : 0
                            }).ToDictionary(d => d.Key, d => d)
                        }).ToList();
                    break;
                case ForeignIndexSource.MajorSoftwareBuild:
                    groupsOEM = _repositoryWrapper.MajorSoftwareBuild
                        .FindByCondition(x => x.Endofmaintenance.HasValue)
                        .Include(x => x.Systemtypes)
                        .Include(x => x.Orgeqpmanufacturer)
                        .Where(x => x.Systemtypes.Any(s => !systemTypesIds.Contains(s.Systemtypeid)))
                        .ToList().GroupBy(x => new { x.Orgeqpmanufacturerid, x.Endofmaintenance })
                        .Select(x => new FIGroupKey
                        {
                            OEM = x.Key.Orgeqpmanufacturerid,
                            EOM = x.Key.Endofmaintenance.Value,
                            Records = x.Select(z => new FIGroupRecord
                            {
                                Correct = false,
                                Description =
                                    $"{z.Orgeqpmanufacturer.Originalequipmentmanufacturer} - {z.Productname.Description} - {z.Softwareversion}",
                                ToAnalyze = (z.Productname != null ? z.Productname.Description:"" )+ z.Softwareversion,
                                Key = z.Majorsoftwarebuildsid,
                                Selected = false,
                                ModificationDate = z.Modificationdate,
                                Count = z.Systemtypes != null ? z.Systemtypes.Count : 0
                            }).ToDictionary(d => d.Key, d => d)
                        }).ToList();
                    break;
                case ForeignIndexSource.NotSpecified:
                    return new ResultDto<ForeignIndexDto>
                    {
                        Data = null,
                        Info = "Source table not specified",
                        Warning = true
                    };
            }

            foreach (var recordGroupedOEM in groupsOEM)
            {
                //var recs = gr.Records.Select(x => x.Value).ToArray();
                //gr.Similarity = new int[recs.Length, recs.Length];
                //for (int i = 0; i < recs.Length; i++)
                //{
                //    for (int j = i; j < recs.Length; j++)
                //    {
                //        gr.Similarity[i, j] = Infrastucture.Levenshtein.CalculateDistance(recs[i].Description.Replace(" ", "").ToUpper(), recs[j].Description.Replace(" ", "").ToUpper(), 1);
                //        gr.Similarity[j, i] = Infrastucture.Levenshtein.CalculateDistance(recs[i].Description.Replace(" ", "").Replace("[0-9]", "").ToUpper(), recs[j].Description.Replace(" ", "").Replace("[0-9]", "").ToUpper(), 1);
                //    }
                //}

                recordGroupedOEM.SimilarityGroups = new List<Dictionary<long, FIGroupRecord>>();
                recordGroupedOEM.CharDiffGroups = new List<Dictionary<long, FIGroupRecord>>();
                var similarityFirstStep = recordGroupedOEM.Records.Select(x => x.Value).ToList();
                var similaritySecondStep = new List<FIGroupRecord>();
                var similarityThirdStep = new List<FIGroupRecord>();
                var charDifferenceStep = new List<FIGroupRecord>();
                var dropDownResourceStep = new List<FIGroupRecord>();
                while (similarityFirstStep.Count > 0)
                {
                    var current = similarityFirstStep[0];
                    var dict = new Dictionary<long, FIGroupRecord>();
                    dict.Add(current.Key, current);
                    similarityFirstStep.RemoveAt(0);
                    var curIndex = 0;
                    while (similarityFirstStep.Count > 0 && curIndex < similarityFirstStep.Count)
                    {
                        var dis = Levenshtein.CalculateDistance(current.ToAnalyze.Replace(" ", "").ToUpper(),
                            similarityFirstStep[curIndex].ToAnalyze.Replace(" ", "").ToUpper(), 1);
                        if (dis == 0)
                        {
                            dict.Add(similarityFirstStep[curIndex].Key, similarityFirstStep[curIndex]);
                            similarityFirstStep.RemoveAt(curIndex);
                        }
                        else
                        {
                            curIndex++;
                        }
                    }

                    if (dict.Count > 0)
                    {
                        if (dict.Count == 1)
                        {
                            similaritySecondStep.Add(dict.First().Value);
                        }
                        else
                        {
                            recordGroupedOEM.SimilarityGroups.Add(dict);
                            recordGroupedOEM.CharDiffGroups.Add(new Dictionary<long, FIGroupRecord>());
                        }
                    }
                }

                while (similaritySecondStep.Count > 0)
                {
                    var current = similaritySecondStep[0];
                    similaritySecondStep.RemoveAt(0);
                    foreach (var dict in recordGroupedOEM.SimilarityGroups)
                    {
                        foreach (var r in dict)
                        {
                            var dictString = Regex.Replace(r.Value.ToAnalyze.Replace(" ", ""), "[0-9]", string.Empty)
                                .ToUpper();
                            var currentString = Regex
                                .Replace(current.ToAnalyze.Replace(" ", ""), "[0-9]", string.Empty).ToUpper();
                            var dis = Levenshtein.CalculateDistance(dictString, currentString, 1);
                            if (dis > 0 && dis < 3)
                            {
                                dict.Add(current.Key, current);
                                current = null;
                                break;
                            }
                        }

                        if (current == null) break;
                    }

                    if (current != null) similarityThirdStep.Add(current);
                }

                while (similarityThirdStep.Count > 0)
                {
                    var current = similarityThirdStep[0];
                    var dict = new Dictionary<long, FIGroupRecord>();
                    dict.Add(current.Key, current);
                    similarityThirdStep.RemoveAt(0);
                    var curIndex = 0;
                    while (similarityThirdStep.Count > 0 && curIndex < similarityThirdStep.Count)
                    {
                        var similarityString = Regex.Replace(similarityThirdStep[curIndex].ToAnalyze.Replace(" ", ""),
                            "[0-9]", string.Empty).ToUpper();
                        var currentDescriptionString =
                            Regex.Replace(current.ToAnalyze.Replace(" ", ""), "[0-9]", string.Empty).ToUpper();
                        var dis = Levenshtein.CalculateDistance(
                            similarityString,
                            currentDescriptionString,
                            1
                        );
                        if (dis > 0 && dis < 3)
                        {
                            dict.Add(similarityThirdStep[curIndex].Key, similarityThirdStep[curIndex]);
                            similarityThirdStep.RemoveAt(curIndex);
                        }
                        else
                        {
                            curIndex++;
                        }
                    }

                    if (dict.Count > 0)
                    {
                        if (dict.Count == 1)
                        {
                            charDifferenceStep.Add(dict.First().Value);
                        }
                        else
                        {
                            recordGroupedOEM.SimilarityGroups.Add(dict);
                            recordGroupedOEM.CharDiffGroups.Add(new Dictionary<long, FIGroupRecord>());
                        }
                    }
                }

                while (charDifferenceStep.Count > 0)
                {
                    var current = charDifferenceStep[0];
                    charDifferenceStep.RemoveAt(0);
                    var index = 0;
                    foreach (var dict in recordGroupedOEM.SimilarityGroups)
                    {
                        foreach (var r in dict)
                        {
                            var dictString = Regex.Replace(r.Value.ToAnalyze.Replace(" ", ""), "[0-9]", string.Empty)
                                .ToUpper();
                            var currentString = Regex
                                .Replace(current.ToAnalyze.Replace(" ", ""), "[0-9]", string.Empty).ToUpper();

                            var dis = CharDifference.CalculateDifference(
                                dictString,
                                currentString
                            );
                            if (dis <= 2)
                            {
                                recordGroupedOEM.CharDiffGroups[index].Add(current.Key, current);
                                //dict.Add(r.Key, r.Value);
                                current = null;
                                break;
                            }
                        }

                        if (current == null) break;
                        index++;
                    }

                    if (current != null) dropDownResourceStep.Add(current);
                }

                recordGroupedOEM.DropDownResource = dropDownResourceStep.ToDictionary(x => x.Key, x => x.Description);

                foreach (var dict in recordGroupedOEM.SimilarityGroups)
                    if (dict.Count > 0)
                    {
                        var mostUsedRecord = dict.OrderByDescending(x => x.Value.Count)
                            .ThenByDescending(x => x.Value.ModificationDate).First();
                        mostUsedRecord.Value.Correct = true;
                    }
            }

            foreignIndexDto.Groups = groupsOEM;
            foreignIndexDto.Session.Status = ForeignIndexStatus.SimilarityGroups;

            return new ResultDto<ForeignIndexDto> { Data = foreignIndexDto };
        }

        public async Task<ResultDto> CancelForeignIndex(ForeignIndexDto foreignIndexDto)
        {
            var session = _mapper.Map<FI_Session>(foreignIndexDto.Session);
            session.Status = ForeignIndexStatus.Canceled;
            _repositoryWrapper.FI_Sessions.Update(FI_SessionMapper.Set(session));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Data = null,
                Info = "This Foreign Index is canceled.",
                Warning = false
            };
        }

        public async Task<ResultDto> ResetForeignIndex()
        {
            var entities = _repositoryWrapper.FI_Sessions.FindByCondition(x =>
            x.Status != (int)ForeignIndexStatus.Canceled && x.Status != (int)ForeignIndexStatus.Committed).ToList();

            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    entity.Status = (int)ForeignIndexStatus.Canceled;
                    _repositoryWrapper.FI_Sessions.Update(entity);
                    await _repositoryWrapper.SaveAsync();
                }

                return new ResultDto
                {
                    Data = null,
                    Info = "Foreign Index Reset successfully",
                    Warning = false
                };
            }

            return new ResultDto
            {
                Data = null,
                Info = "There are no active sessions at the moment",
                Warning = true
            };
        }

        public async Task<ResultDto<ForeignIndexDto>> ImpactCheck(ForeignIndexDto foreignIndexDto)
        {
            //var resCheck = await CheckSession(foreignIndexDto.Session.SessionId, null);
            //if (resCheck.Warning)
            //    return new ResultDto<ForeignIndexDto>
            //    {
            //        Data = null,
            //        Info = resCheck.Info,
            //        Warning = true
            //    };

            var correct = foreignIndexDto.GroupToUpdate.Single(x => x.Correct);
            var check = new List<ImpactCheckDto>();

            foreach (var groupRecord in foreignIndexDto.GroupToUpdate.Where(x =>
                !(x.Selected == false && x.Correct == false)))
            {
                var lcms = await GetLcmEngeenerings(foreignIndexDto.Session.Source, groupRecord.Key);
                var nep = await GetNetworkElements(foreignIndexDto.Session.Source, groupRecord.Key);
                groupRecord.ImpactChecks = new List<ImpactCheckDto>();
                foreach (var lcmEngineering in lcms)
                {
                    var impact = await GetImpact(foreignIndexDto, correct, groupRecord,
                        lcmEngineering.DesignComponentId, lcmEngineering.OpCo.OpCoDescription,
                        foreignIndexDto.Session.Source);
                    impact.LcmEngeeneringId = lcmEngineering.LcmengineeringId;
                    impact.Type = "Lcm";
                    check.Add(impact);
                    groupRecord.ImpactChecks.Add(impact);
                }

                foreach (var networkElement in nep)
                {
                    var impact = await GetImpact(foreignIndexDto, correct, groupRecord,
                        networkElement.DesignComponentId, networkElement.OpCo.OpCoDescription,
                        foreignIndexDto.Session.Source);
                    impact.NetworkElementId = networkElement.NetworkElementAsPlannedId;

                    impact.Type = "NetworkElements";
                    check.Add(impact);
                    groupRecord.ImpactChecks.Add(impact);
                }
            }

            switch (foreignIndexDto.Session.Source)
            {
                case ForeignIndexSource.MajorHardwareBuild:
                    check = CheckMajor(check);
                    break;
                case ForeignIndexSource.MajorSoftwareBuild:
                    check = CheckMajor(check);
                    break;
                case ForeignIndexSource.DesignComponentFamily:
                    check = CheckDesignComponentFamily(check);
                    break;
                case ForeignIndexSource.NotSpecified:
                default:
                    throw new ArgumentOutOfRangeException();
            }


            foreignIndexDto.ImpactChecks = check;
            foreignIndexDto.Session.Status = ForeignIndexStatus.GroupCorrection;

            var session = _mapper.Map<FI_Session>(foreignIndexDto.Session);
            _repositoryWrapper.FI_Sessions.Update(FI_SessionMapper.Set(session));
            await _repositoryWrapper.SaveAsync();

            return new ResultDto<ForeignIndexDto>
            {
                Data = foreignIndexDto,
                Warning = false
            };
        }

        private List<ImpactCheckDto> CheckDesignComponentFamily(List<ImpactCheckDto> check)
        {
            var ids = check.Select(x => x.DesignComponentId);
            var designComponentsDuplicated =
                _repositoryWrapper.DesignComponent
                    .FindByCondition(x => ids.Contains(x.Designcomponentid))
                    .Include(x => x.Lcmengineering)
                    .Include(x => x.Networkelementsasplanned)
                    .ToList()
                    .GroupBy(x => new { x.Designcomponentfamilyid, x.Systemtypeid });

            foreach (var designComponentGroup in designComponentsDuplicated)
                if (designComponentGroup.Count() > 1)
                {
                    var duplicatedStores = new List<PiuUsatoSelector>();

                    foreach (var x in designComponentGroup)
                        duplicatedStores.Add(new PiuUsatoSelector
                        {
                            id = x.Designcomponentid,
                            Count = x.Lcmengineering.Count() + x.Networkelementsasplanned.Count(),
                            DataModifica = x.Modificationdate
                        });

                    var correctDesignComponent = duplicatedStores.OrderByDescending(x => x.Count)
                        .ThenByDescending(x => x.DataModifica).ToList();


                    for (var index = 0; index < correctDesignComponent.Count; index++)
                    {
                        var piuUsatoSelector = correctDesignComponent.ToList()[index];

                        if (index == 0)
                            foreach (var impactCheckDto in check.Where(x => x.DesignComponentId == piuUsatoSelector.id))
                                impactCheckDto.Correct = true;
                        else
                            foreach (var impactCheckDto in check.Where(x => x.DesignComponentId == piuUsatoSelector.id))
                            {
                                impactCheckDto.Correct = false;
                                impactCheckDto.ToDelete = true;
                                impactCheckDto.key = "key" + correctDesignComponent[0].id;

                                if (impactCheckDto.Type == "Lcm")
                                    impactCheckDto.PlannedActivity =
                                        _repositoryWrapper.PlannedActivity.FindByCondition(x =>
                                                x.Lcmengineeringid == impactCheckDto.LcmEngeeneringId)
                                            .Include(x => x.Lcmengineering)
                                            .ThenInclude(x => x.Lcmengineeringsubdomainspoc)
                                            .Include(x => x.Lcmengineering)
                                            .ThenInclude(x => x.Lcmengineeringeduspoc)
                                            .ToList().Select(x =>
                                                new PlannedActivityImpactResult
                                                {
                                                    DesignComponent =
                                                        x.Lcmengineering.Designcomponent.toDesignComponentNameLcm(
                                                            _repositoryWrapper),
                                                    NumberOfNodes = x.Lcmengineering.Numberofnodes,
                                                    //Eduspoc = x.Lcmengineering.Lcmengineeringeduspoc
                                                    //    .Where(x => x.Deleted == false).Select(fx =>
                                                    //        fx.Subdomainspoc.Subdomainspoc).Distinct()
                                                    //    .Aggregate(
                                                    //        "", (current, next) => current + " " + next),
                                                    ActivityIndex = x.Plannedactivityid.ToString(),
                                                    NumberOfNodesInLab = x.Lcmengineering.Numberofnodesinlab,
                                                    Opco = x.Lcmengineering.Opco.Opco,
                                                    //SubDomainSpoc = x.Lcmengineering.Lcmengineeringsubdomainspoc
                                                    //    .Where(x => x.Deleted == false).Select(fx =>
                                                    //        fx.Subdomainspoc.Subdomainspoc).Distinct()
                                                    //    .Aggregate(
                                                    //        "", (current, next) => current + " " + next),
                                                    PlannedAction =
                                                        $"{x.Plannedimplementationyear} | {x.Activitystatus.Activitystatus} | {x.Planningactivitystatus.Planningactivitystatus}",
                                                    PproductImportance = x.Lcmengineering.Productimportance
                                                        .Productimportance
                                                }
                                            ).ToList();
                                else
                                    impactCheckDto.PlannedActivity =
                                        _repositoryWrapper.PlannedActivity.FindByCondition(x =>
                                                x.Networkelementasplannedid == impactCheckDto.NetworkElementId)
                                            .Include(x => x.Networkelementasplanned)
                                            .ThenInclude(x => x.Networkelementasplannedsubdomainspoc)
                                            .Include(x => x.Networkelementasplanned)
                                            .ThenInclude(x => x.Networkelementasplannededuspoc)
                                            .ToList().Select(x =>
                                                new PlannedActivityImpactResult
                                                {
                                                    DesignComponent =
                                                        x.Networkelementasplanned.Designcomponent
                                                            .toDesignComponentNameLcm(_repositoryWrapper),
                                                    //Eduspoc = x.Networkelementasplanned.Networkelementasplannededuspoc
                                                    //    .Where(x => x.Deleted == false).Select(fx =>
                                                    //        fx.Subdomainspoc.Subdomainspoc).Distinct()
                                                    //    .Aggregate(
                                                    //        "", (current, next) => current + " " + next),
                                                    ActivityIndex = x.Plannedactivityid.ToString(),
                                                    Opco = x.Lcmengineering.Opco.Opco,
                                                    //SubDomainSpoc = x.Networkelementasplanned
                                                    //    .Networkelementasplannedsubdomainspoc
                                                    //    .Where(x => x.Deleted == false).Select(fx =>
                                                    //        fx.Subdomainspoc.Subdomainspoc).Distinct()
                                                    //    .Aggregate(
                                                    //        "", (current, next) => current + " " + next),
                                                    PlannedAction =
                                                        $"{x.Plannedimplementationyear} | {x.Activitystatus.Activitystatus} | {x.Planningactivitystatus.Planningactivitystatus}"
                                                }
                                            ).ToList();
                            }
                    }
                }

            return check;
        }

        private List<ImpactCheckDto> CheckMajor(List<ImpactCheckDto> check)
        {
            foreach (var systemType in check.Select(x => x.SystemTypeChecker)
                .GroupBy(x => new { x.MajorHardwareId, x.MajorSoftwareId, x.SystemTypeNameOem }))
                if (systemType.Count() > 1)
                {
                    var records = check.Where(x => x.SystemTypeChecker.Equals(new SystemTypeChecker
                    {
                        MajorHardwareId = systemType.Key.MajorHardwareId,
                        MajorSoftwareId = systemType.Key.MajorSoftwareId,
                        //todo gestire una funzione per ricreare il NameOem
                        SystemTypeNameOem = systemType.Key.SystemTypeNameOem
                    }));
                    var impactCheckDtos = records.ToList();

                    var ids = impactCheckDtos.Select(x => x.DesignComponentId);

                    check = CheckDesignComponent(check, ids);
                }

            var test = JsonSerializer.Serialize(check);
            Debug.WriteLine(test);
            return check;
        }


        private List<ImpactCheckDto> CheckDesignComponent(List<ImpactCheckDto> check, IEnumerable<long> ids)
        {
            var designComponents =
                _repositoryWrapper.DesignComponent
                    .FindByCondition(x => ids.Contains(x.Designcomponentid))
                    .Include(x => x.Lcmengineering)
                    .Include(x => x.Networkelementsasplanned)
                    .ToList();
            var designComponentsDuplicated = designComponents.GroupBy(x => x.Designcomponentfamilyid);

            foreach (var designComponentGroup in designComponentsDuplicated)
                if (designComponentGroup.Count() > 1)
                {
                    var duplicatedStores = new List<PiuUsatoSelector>();

                    foreach (var x in designComponentGroup)
                        duplicatedStores.Add(new PiuUsatoSelector
                        {
                            id = x.Designcomponentid,
                            Count = x.Lcmengineering.Count() + x.Networkelementsasplanned.Count(),
                            DataModifica = x.Modificationdate
                        });

                    var correctDesignComponent = duplicatedStores.OrderByDescending(x => x.Count)
                        .ThenByDescending(x => x.DataModifica).ToList();

                    var checkOpco = check
                        .Where(x => correctDesignComponent.Select(x => x.id).Contains(x.DesignComponentId))
                        .GroupBy(x => x.Opco);

                    if (checkOpco.Count() == 1)
                        for (var index = 0; index < correctDesignComponent.Count; index++)
                        {
                            var piuUsatoSelector = correctDesignComponent.ToList()[index];
                            if (index == 0)
                                foreach (var impactCheckDto in check.Where(x =>
                                    x.DesignComponentId == piuUsatoSelector.id))
                                {
                                    impactCheckDto.key = "key" + correctDesignComponent[0].id;
                                    impactCheckDto.Correct = true;
                                }
                            else
                                foreach (var impactCheckDto in check.Where(x =>
                                    x.DesignComponentId == piuUsatoSelector.id))
                                {
                                    impactCheckDto.Correct = false;
                                    impactCheckDto.ToDelete = true;
                                    impactCheckDto.key = "key" + correctDesignComponent[0].id;

                                    if (impactCheckDto.Type == "Lcm")
                                        impactCheckDto.PlannedActivity =
                                            _repositoryWrapper.PlannedActivity.FindByCondition(x =>
                                                    x.Lcmengineeringid == impactCheckDto.LcmEngeeneringId)
                                                .Include(x => x.Lcmengineering)
                                                .ThenInclude(x => x.Lcmengineeringsubdomainspoc)
                                                .Include(x => x.Lcmengineering)
                                                .ThenInclude(x => x.Lcmengineeringeduspoc)
                                                .ToList().Select(x =>
                                                    new PlannedActivityImpactResult
                                                    {
                                                        DesignComponent =
                                                            x.Lcmengineering.Designcomponent.toDesignComponentNameLcm(
                                                                _repositoryWrapper),
                                                        NumberOfNodes = x.Lcmengineering.Numberofnodes,
                                                        //Eduspoc = x.Lcmengineering.Lcmengineeringeduspoc
                                                        //    .Where(x => x.Deleted == false).Select(fx =>
                                                        //        fx.Subdomainspoc.Subdomainspoc).Distinct()
                                                        //    .Aggregate(
                                                        //        "", (current, next) => current + " " + next),
                                                        ActivityIndex = x.Plannedactivityid.ToString(),
                                                        NumberOfNodesInLab = x.Lcmengineering.Numberofnodesinlab,
                                                        Opco = x.Lcmengineering.Opco.Opco,
                                                        //SubDomainSpoc = x.Lcmengineering.Lcmengineeringsubdomainspoc
                                                        //    .Where(x => x.Deleted == false).Select(fx =>
                                                        //        fx.Subdomainspoc.Subdomainspoc).Distinct()
                                                        //    .Aggregate(
                                                        //        "", (current, next) => current + " " + next),
                                                        PlannedAction =
                                                            $"{x.Plannedimplementationyear} | {x.Activitystatus.Activitystatus} | {x.Planningactivitystatus.Planningactivitystatus}",
                                                        PproductImportance = x.Lcmengineering.Productimportance
                                                            .Productimportance
                                                    }
                                                ).ToList();
                                    else
                                        impactCheckDto.PlannedActivity =
                                            _repositoryWrapper.PlannedActivity.FindByCondition(x =>
                                                    x.Networkelementasplannedid == impactCheckDto.NetworkElementId)
                                                .Include(x => x.Networkelementasplanned)
                                                .ThenInclude(x => x.Networkelementasplannedsubdomainspoc)
                                                .Include(x => x.Networkelementasplanned)
                                                .ThenInclude(x => x.Networkelementasplannededuspoc)
                                                .ToList().Select(x =>
                                                    new PlannedActivityImpactResult
                                                    {
                                                        DesignComponent =
                                                            x.Networkelementasplanned.Designcomponent
                                                                .toDesignComponentNameLcm(_repositoryWrapper),
                                                        //Eduspoc = x.Networkelementasplanned
                                                        //    .Networkelementasplannededuspoc
                                                        //    .Where(x => x.Deleted == false).Select(fx =>
                                                        //        fx.Subdomainspoc.Subdomainspoc).Distinct()
                                                        //    .Aggregate(
                                                        //        "", (current, next) => current + " " + next),
                                                        ActivityIndex = x.Plannedactivityid.ToString(),
                                                        Opco = x.Lcmengineering.Opco.Opco,
                                                        //SubDomainSpoc = x.Networkelementasplanned
                                                        //    .Networkelementasplannedsubdomainspoc
                                                        //    .Where(x => x.Deleted == false).Select(fx =>
                                                        //        fx.Subdomainspoc.Subdomainspoc).Distinct()
                                                        //    .Aggregate(
                                                        //        "", (current, next) => current + " " + next),
                                                        PlannedAction =
                                                            $"{x.Plannedimplementationyear} | {x.Activitystatus.Activitystatus} | {x.Planningactivitystatus.Planningactivitystatus}"
                                                    }
                                                ).ToList();
                                }
                        }
                }

            return check;
        }

        public async Task<ResultDto<ForeignIndexDto>> Apply(ForeignIndexDto foreignIndexDto)
        {
            var resCheck = await CheckSession(foreignIndexDto.Session.SessionId, ForeignIndexStatus.GroupCorrection);
            if (resCheck.Warning)
                return new ResultDto<ForeignIndexDto>
                {
                    Data = null,
                    Info = resCheck.Info,
                    Warning = true
                };

            using var transaction = _repositoryWrapper.BeginTransaction();

            var updateData = foreignIndexDto.GroupToUpdate;
            var correct = updateData.Single(x => x.Correct);
            var toDelete = updateData.Where(x => x.Selected).Select(x => x.Key).ToList();


            List<DuplicatedStore<Lcmengineering>> lcmEngineeringsDuplicated = null;
            List<DuplicatedStore<Networkelementsasplanned>> networksDuplicated = null;

            List<DuplicatedStore<List<Systemtypesmajorhardwarebuilds>>> systemTypesMajorHardwareBuildsToUpdate = null;
            //List<DuplicatedStore<List<SystemTypesMajorHardwareBuild>>> systemTypesMajorHardwareBuildsDuplicated = null;

            List<DuplicatedStore<Systemtypes>> systemTypesToUpdate = null;
            List<DuplicatedStore<Systemtypes>> systemTypesDuplicated = null;
            List<DuplicatedStore<Designcomponents>> designComponentsToUpdate = null;
            List<DuplicatedStore<Designcomponents>> designComponentsDuplicated = null;

            var lcmCorrectIds = new List<long>();
            var networkCorrectIds = new List<long>();

            try
            {
                switch (foreignIndexDto.Session.Source)
                {
                    case ForeignIndexSource.MajorHardwareBuild:
                        lcmCorrectIds.AddRange(
                            _repositoryWrapper.Lcmengineering
                                .FindByCondition(x =>
                                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(z =>
                                        z.Majorhardwareid == correct.Key && z.Ismain)).AsNoTracking()
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
                                .ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                                .Select(x => x.Lcmengineeringid).ToList()
                        );

                        networkCorrectIds.AddRange(
                            _repositoryWrapper.NetworkElementAsPlanned
                                .FindByCondition(x =>
                                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(z =>
                                        z.Majorhardwareid == correct.Key && z.Ismain)).AsNoTracking()
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
                                .ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                                .Select(x => x.Networkelementasplannedid).ToList()
                        );

                        var majorHardwareBuildsToDelete = _repositoryWrapper.MajorHardwareBuild
                            .FindByCondition(x => toDelete.Contains(x.Majorhardwareid)).AsNoTracking().ToList();

                        systemTypesMajorHardwareBuildsToUpdate = _repositoryWrapper.SystemTypesMajorHardwareBuild
                            .FindByCondition(x => toDelete.Contains(x.Majorhardwareid)).AsNoTracking()// && x.IsMain)
                            .ToList()
                            .GroupBy(x => x.Majorhardwareid)
                            .Select(x => new DuplicatedStore<List<Systemtypesmajorhardwarebuilds>>
                            { Id = x.Key, Used = x.Count(), Obj = x.ToList() })
                            .ToList();
                        foreach (var stmhToUpdate in systemTypesMajorHardwareBuildsToUpdate)
                        {
                            var items = stmhToUpdate.Obj.ToList();
                            foreach (var rec in items)
                            {
                                var data = rec;
                                _repositoryWrapper.SystemTypesMajorHardwareBuild.DeleteDeep(data);
                                rec.Majorhardwareid = correct.Key;
                                if (!_repositoryWrapper.SystemTypesMajorHardwareBuild
                                    .FindByCondition(x => x.Majorhardwareid == correct.Key && x.Systemtypeid == rec.Systemtypeid).AsNoTracking().Any())
                                {
                                    _repositoryWrapper.SystemTypesMajorHardwareBuild.Create(data);
                                }
                            }
                        }

                        await _repositoryWrapper.SaveAsync();

                        foreach (var msToDelete in majorHardwareBuildsToDelete)
                        {
                            _repositoryWrapper.MajorHardwareBuild.DeleteDeep(msToDelete);
                            await _repositoryWrapper.SaveAsync();
                        }

                        systemTypesDuplicated = _repositoryWrapper.SystemType
                            .FindByCondition(x =>
                                x.Systemtypesmajorhardwarebuilds.Any(z => z.Majorhardwareid == correct.Key)).AsNoTracking()
                            .Include(x => x.Designcomponents)
                            .Include(x => x.Systemtypesmajorhardwarebuilds)
                            .Select(x => new DuplicatedStore<Systemtypes>
                            { Id = x.Majorsoftwarebuildsid.Value, Used = x.Designcomponents.Count(), Obj =x })
                            .ToList();

                        break;
                    case ForeignIndexSource.MajorSoftwareBuild:
                        lcmCorrectIds.AddRange(
                            _repositoryWrapper.Lcmengineering
                                .FindByCondition(x => x.Designcomponent.Systemtype.Majorsoftwarebuildsid == correct.Key).AsNoTracking()
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
                                .Select(x => x.Lcmengineeringid).ToList()
                        );

                        networkCorrectIds.AddRange(
                            _repositoryWrapper.NetworkElementAsPlanned
                                .FindByCondition(x => x.Designcomponent.Systemtype.Majorsoftwarebuildsid == correct.Key).AsNoTracking()
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
                                .Select(x => x.Networkelementasplannedid).ToList()
                        );

                        var correctMajorSoftware = _repositoryWrapper.MajorSoftwareBuild
                            .FindByCondition(x => x.Majorsoftwarebuildsid == correct.Key).AsNoTracking().Single();

                        var majorSoftwareBuildsToDelete = _repositoryWrapper.MajorSoftwareBuild
                            .FindByCondition(x => toDelete.Contains(x.Majorsoftwarebuildsid)).AsNoTracking().ToList();

                        systemTypesToUpdate = _repositoryWrapper.SystemType
                            .FindByCondition(x =>
                                x.Majorsoftwarebuildsid.HasValue && toDelete.Contains(x.Majorsoftwarebuildsid.Value))
                            .Include(x => x.Designcomponents).AsNoTracking()
                            .Select(x => new DuplicatedStore<Systemtypes>
                            { Id = x.Systemtypeid, Used = x.Designcomponents.Count(), Obj = x })
                            .ToList();
                        foreach (var stToUpdate in systemTypesToUpdate)
                        {
                            stToUpdate.Obj.Majorsoftwarebuildsid = correct.Key;
                            //todo gestire una funzione per ricreare il NameOem
                            stToUpdate.Obj.Systemtypenameoem = correctMajorSoftware.Productname != null ? correctMajorSoftware.Productname.Description:"";
                            stToUpdate.Obj.Designcomponents = null;
                            stToUpdate.Obj.Systemtypesmajorhardwarebuilds = null;
                            _repositoryWrapper.SystemType.Update(stToUpdate.Obj);
                        }

                        await _repositoryWrapper.SaveAsync();


                        foreach (var msToDelete in majorSoftwareBuildsToDelete)
                            _repositoryWrapper.MajorSoftwareBuild.DeleteDeep(msToDelete);

                        await _repositoryWrapper.SaveAsync();

                        systemTypesDuplicated = _repositoryWrapper.SystemType
                            .FindByCondition(x =>
                                x.Majorsoftwarebuildsid.HasValue && x.Majorsoftwarebuildsid.Value == correct.Key)
                            .Include(x => x.Designcomponents)
                            .Include(x => x.Systemtypesmajorhardwarebuilds)
                            .Select(x => new DuplicatedStore<Systemtypes>
                            { Id = x.Majorsoftwarebuildsid.Value, Used = x.Designcomponents.Count(), Obj = x}).AsNoTracking()
                            .ToList();
                        break;
                    case ForeignIndexSource.DesignComponentFamily:
                        lcmCorrectIds.AddRange(
                            _repositoryWrapper.Lcmengineering
                                .FindByCondition(x => x.Designcomponent.Designcomponentfamilyid == correct.Key)
                                .Include(x => x.Designcomponent).AsNoTracking()
                                .Select(x => x.Lcmengineeringid).ToList()
                        );

                        networkCorrectIds.AddRange(
                            _repositoryWrapper.NetworkElementAsPlanned
                                .FindByCondition(x => x.Designcomponent.Designcomponentfamilyid == correct.Key)
                                .Include(x => x.Designcomponent).AsNoTracking()
                                .Select(x => x.Networkelementasplannedid).ToList()
                        );

                        var designComponentFamiliesToDelete = _repositoryWrapper.DesignComponentFamily
                            .FindByCondition(x => toDelete.Contains(x.Designcomponentfamilyid)).AsNoTracking().ToList();

                        designComponentsToUpdate = _repositoryWrapper.DesignComponent
                            .FindByCondition(x =>
                                x.Designcomponentfamilyid.HasValue &&
                                toDelete.Contains(x.Designcomponentfamilyid.Value))
                            .Include(x => x.Lcmengineering)
                            .Include(x => x.Networkelementsasplanned).AsNoTracking()
                            .Select(x => new DuplicatedStore<Designcomponents>
                            {
                                Id = x.Designcomponentid,
                                Used = x.Lcmengineering.Count() + x.Networkelementsasplanned.Count(),
                                Obj = x,
                            })
                            .ToList();
                        foreach (var dcToUpdate in designComponentsToUpdate)
                        {
                            dcToUpdate.Obj.Designcomponentfamilyid = correct.Key;
                            dcToUpdate.Obj.Lcmengineering = null;
                            dcToUpdate.Obj.Networkelementsasplanned = null;
                            _repositoryWrapper.DesignComponent.Update(dcToUpdate.Obj);
                        }

                        await _repositoryWrapper.SaveAsync();

                        foreach (var dcfToDelete in designComponentFamiliesToDelete)
                            _repositoryWrapper.DesignComponentFamily.DeleteDeep(dcfToDelete);

                        await _repositoryWrapper.SaveAsync();

                        systemTypesDuplicated = new List<DuplicatedStore<Systemtypes>>();

                        designComponentsDuplicated = _repositoryWrapper.DesignComponent
                            .FindByCondition(x =>
                                x.Designcomponentfamilyid.HasValue && x.Designcomponentfamilyid.Value == correct.Key)
                            .Include(x => x.Lcmengineering)
                            .Include(x => x.Networkelementsasplanned).AsNoTracking()
                            .Select(x => new DuplicatedStore<Designcomponents>
                            {
                                Id = x.Designcomponentid,
                                Used = x.Lcmengineering.Count() + x.Networkelementsasplanned.Count(),
                                Obj = x,
                            })
                            .ToList();

                        break;

                    case ForeignIndexSource.NotSpecified:
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (systemTypesDuplicated != null && systemTypesDuplicated.Count > 0)
                {
                    #region System Type Duplicates

                    var stDuplicates = systemTypesDuplicated.GroupBy(x => new
                    {
                        x.Obj.Majorsoftwarebuildsid,
                        x.Obj.Systemtypenameoem,
                        x.Obj.Systemtypesmajorhardwarebuilds.Where(m => m.Ismain).SingleOrDefault().Majorhardwareid
                    }).Where(x => x.Count() > 1).ToList();

                    var stDuplicatedIds = new List<long>();
                    foreach (var rec in stDuplicates)
                    {
                        _repositoryWrapper.DesignComponent.Detach();
                        var ordered = rec.OrderByDescending(x => x.Used).ThenByDescending(x => x.Obj.Modificationdate);
                        var stCorrect = ordered.FirstOrDefault();
                        stDuplicatedIds.Add(stCorrect.Obj.Systemtypeid);
                        var stsToDelete = ordered.Skip(1).ToList();
                        var stToDeleteIds = stsToDelete.Select(x => x.Obj.Systemtypeid).ToList();


                        designComponentsToUpdate = _repositoryWrapper.DesignComponent
                            .FindByCondition(x => stToDeleteIds.Contains(x.Systemtypeid))
                            .Include(x => x.Lcmengineering)
                            .Include(x => x.Networkelementsasplanned).AsNoTracking()
                            .Select(x => new DuplicatedStore<Designcomponents>
                            {
                                Id = x.Designcomponentid,
                                Used = x.Lcmengineering.Count() + x.Networkelementsasplanned.Count(),
                                Obj =x,
                            })
                            .ToList();
                        foreach (var dcToUpdate in designComponentsToUpdate)
                        {
                            dcToUpdate.Obj.Systemtypeid = stCorrect.Obj.Systemtypeid;
                            dcToUpdate.Obj.Lcmengineering = null;
                            dcToUpdate.Obj.Networkelementsasplanned = null;
                            _repositoryWrapper.DesignComponent.Update(dcToUpdate.Obj);
                        }

                        await _repositoryWrapper.SaveAsync();

                        var networksAsIsToUpdate = _repositoryWrapper.NetworkElementAsIs
                            .FindByCondition(x => stToDeleteIds.Contains(x.Systemtypeid)).AsNoTracking()
                            .ToList();

                        foreach (var naiToUpdate in networksAsIsToUpdate)
                        {
                            naiToUpdate.Systemtypeid = stCorrect.Obj.Systemtypeid;
                            _repositoryWrapper.NetworkElementAsIs.Update(naiToUpdate);
                        }

                        await _repositoryWrapper.SaveAsync();

                        _repositoryWrapper.SystemTypesMajorHardwareBuild.Detach();
                        _repositoryWrapper.SystemType.Detach();

                        foreach (var stToDelete in stsToDelete)
                        {
                            stToDelete.Obj.Systemtypesmajorhardwarebuilds = null;
                            stToDelete.Obj.Designcomponents = null;
                             RemoveSystemTypeRelation(stToDelete.Obj);
                            _repositoryWrapper.SystemType.DeleteDeep(stToDelete.Obj);
                            
                        }
                    }

                    await _repositoryWrapper.SaveAsync();

                    designComponentsDuplicated = _repositoryWrapper.DesignComponent
                        .FindByCondition(x => stDuplicatedIds.Contains(x.Systemtypeid))
                        .Include(x => x.Lcmengineering)
                        .Include(x => x.Networkelementsasplanned).AsNoTracking()
                        .Select(x => new DuplicatedStore<Designcomponents>
                        {
                            Id = x.Designcomponentid,
                            Used = x.Lcmengineering.Count() + x.Networkelementsasplanned.Count(),
                            Obj = x,
                        })
                        .ToList();

                    #endregion
                }

                if (designComponentsDuplicated != null && designComponentsDuplicated.Count > 0)
                {
                    #region Design Componets Duplicated

                    var dcDuplicatedIds = new List<long>();
                    var dcDuplicates = designComponentsDuplicated
                        .GroupBy(x => new { x.Obj.Designcomponentfamilyid, x.Obj.Systemtypeid })
                        .Where(x => x.Count() > 1).ToList();
                    foreach (var rec in dcDuplicates)
                    {
                        var ordered = rec.OrderByDescending(x => x.Used).ThenByDescending(x => x.Obj.Modificationdate);
                        var dcCorrect = ordered.FirstOrDefault();
                        dcDuplicatedIds.Add(dcCorrect.Id);
                        var dcsToDelete = ordered.Skip(1).ToList();
                        var dcToDeleteIds = dcsToDelete.Select(x => x.Id).ToList();


                        var lcmEngineeringsToUpdate = _repositoryWrapper.Lcmengineering
                            .FindByCondition(x => dcToDeleteIds.Contains(x.Designcomponentid)).AsNoTracking()
                            .ToList();
                        foreach (var lcmToUpdate in lcmEngineeringsToUpdate)
                        {
                            lcmToUpdate.Designcomponentid = dcCorrect.Id;
                            _repositoryWrapper.Lcmengineering.Update(lcmToUpdate);
                        }

                        var networksToUpdate = _repositoryWrapper.NetworkElementAsPlanned
                            .FindByCondition(x => dcToDeleteIds.Contains(x.Designcomponentid)).AsNoTracking()
                            .ToList();
                        foreach (var networkToUpdate in networksToUpdate)
                        {
                            var lcm = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponentid == dcCorrect.Id && x.Opcoid == networkToUpdate.Opcoid && x.Archived != true).FirstOrDefault();

                            networkToUpdate.Designcomponentid = dcCorrect.Id;
                            networkToUpdate.Lcmengineeringid = lcm?.Lcmengineeringid;
                            _repositoryWrapper.NetworkElementAsPlanned.Update(networkToUpdate);
                        }

                        var plannedActivitiesToUpdate = _repositoryWrapper.PlannedActivity
                            .FindByCondition(x =>
                                x.Designcomponentid.HasValue && dcToDeleteIds.Contains(x.Designcomponentid.Value)).AsNoTracking()
                            .ToList();
                        foreach (var plannedActivityToUpdate in plannedActivitiesToUpdate)
                        {
                            plannedActivityToUpdate.Designcomponentid = dcCorrect.Id;
                            plannedActivityToUpdate.Archived = plannedActivityToUpdate.Archived == null ? false : plannedActivityToUpdate.Archived;
                            _repositoryWrapper.PlannedActivity.Update(plannedActivityToUpdate);
                        }

                        await _repositoryWrapper.SaveAsync();

                        foreach (var dcToDelete in dcsToDelete)
                        {
                            dcToDelete.Obj.Lcmengineering = null;
                            dcToDelete.Obj.Networkelementsasplanned = null;
                            _repositoryWrapper.DesignComponent.DeleteDeep(dcToDelete.Obj);
                        }
                    }


                    await _repositoryWrapper.SaveAsync();

                    lcmEngineeringsDuplicated = _repositoryWrapper.Lcmengineering
                        .FindByCondition(x => dcDuplicatedIds.Contains(x.Designcomponentid))
                        .Include(x => x.PlannedactivitiesLcmengineering).AsNoTracking()
                        .Select(x => new DuplicatedStore<Lcmengineering>
                        {
                            Id = x.Designcomponentid,
                            Used = x.PlannedactivitiesLcmengineering != null ? x.PlannedactivitiesLcmengineering.Count() : 0,
                            Obj = x,
                        })
                        .ToList();
                    networksDuplicated = _repositoryWrapper.NetworkElementAsPlanned
                        .FindByCondition(x => dcDuplicatedIds.Contains(x.Designcomponentid)).AsNoTracking()
                        .Include(x => x.Plannedactivities)
                        .Select(x => new DuplicatedStore<Networkelementsasplanned>
                        {
                            Id = x.Designcomponentid,
                            Used = x.Plannedactivities != null ? x.Plannedactivities.Count() : 0,
                            Obj = x,
                        })
                        .ToList();

                    #endregion
                }

                #region LCM Engineering Duplicates

                if (lcmEngineeringsDuplicated != null && lcmEngineeringsDuplicated.Count > 0)
                {
                    var lcmDuplicates = lcmEngineeringsDuplicated.GroupBy(x => new { x.Obj.Opcoid, x.Obj.Designcomponentid }).Where(x => x.Count() > 1).ToList();
                    foreach (var rec in lcmDuplicates)
                    {
                        var ordered = rec.OrderByDescending(x => x.Used).ThenByDescending(x => x.Obj.Modificationdate);
                        var lcmCorrect = ordered.FirstOrDefault();
                        var lcmsToDelete = ordered.Skip(1).ToList();
                        var lcmToDeleteIds = lcmsToDelete.Select(x => x.Id).ToList();


                        var lcmEngineeringsToUpdate = _repositoryWrapper.Lcmengineering
                            .FindByCondition(x => lcmToDeleteIds.Contains(x.Lcmengineeringid)).AsNoTracking()
                            .ToList();
                        foreach (var lcmToUpdate in lcmEngineeringsToUpdate)
                            foreach (var pa in lcmToUpdate.PlannedactivitiesLcmengineering)
                            {
                                pa.Lcmengineeringid = lcmCorrect.Id;
                                pa.Archived = pa.Archived == null ? false : pa.Archived;
                                _repositoryWrapper.PlannedActivity.Update(pa);
                            }

                        await _repositoryWrapper.SaveAsync();

                        foreach (var lcmToDelete in lcmsToDelete)
                        {
                            lcmToDelete.Obj.PlannedactivitiesLcmengineering = null;
                            _repositoryWrapper.Lcmengineering.DeleteDeep(lcmToDelete.Obj);
                        }

                        var lcmSetValue =
                            _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == lcmCorrect.Id).AsNoTracking().Single();
                        var lcm = await _lcmEngineeringManager.SetLcmValue(lcmSetValue);
                        _repositoryWrapper.Lcmengineering.Update(lcm);

                    }

                    await _repositoryWrapper.SaveAsync();



                }

                #endregion

                #region Network As Planned Duplicates

                if (networksDuplicated != null && networksDuplicated.Count > 0)
                {
                    var networksDuplicates = networksDuplicated.GroupBy(x => new
                    {
                        x.Obj.Opcoid,
                        x.Obj.Elementname,
                        x.Obj.Orgeqpmanufacturerid,
                        x.Obj.Designcomponentid
                    }).Where(x => x.Count() > 1).ToList();
                    foreach (var rec in networksDuplicates)
                    {
                        var ordered = rec.OrderByDescending(x => x.Used).ThenByDescending(x => x.Obj.Modificationdate);
                        var networkCorrect = ordered.FirstOrDefault();
                        var networksToDelete = ordered.Skip(1).ToList();
                        var networkToDeleteIds = networksToDelete.Select(x => x.Id).ToList();


                        var networksToUpdate = _repositoryWrapper.NetworkElementAsPlanned
                            .FindByCondition(x => networkToDeleteIds.Contains(x.Networkelementasplannedid))
                            .ToList();
                        foreach (var networkToUpdate in networksToUpdate)
                            foreach (var pa in networkToUpdate.Plannedactivities)
                            {
                                pa.Networkelementasplannedid = networkCorrect.Id;
                                pa.Archived = pa.Archived == null ? false : pa.Archived;
                                _repositoryWrapper.PlannedActivity.Update(pa);
                            }

                        await _repositoryWrapper.SaveAsync();

                        foreach (var networkToDelete in networksToDelete)
                        {
                            networkToDelete.Obj.Plannedactivities = null;
                            _repositoryWrapper.NetworkElementAsPlanned.DeleteDeep(networkToDelete.Obj);
                        }
                    }

                    await _repositoryWrapper.SaveAsync();
                }

                #endregion

                #region DesignComponentRemove
                var designComponentIds = foreignIndexDto.DesignComponents.Where(x => x.ToDelete).Select(x => x.DesignComponentId);
                var designComponent = _repositoryWrapper.DesignComponent
                     .FindByCondition(x => designComponentIds.Contains(x.Designcomponentid)).ToList();
                foreach (var x in designComponent)
                {
                    _repositoryWrapper.DesignComponent.DeleteDeep(x);
                }
                #endregion

                #region SystemTypeRemove
                var systemTypeIds = foreignIndexDto.SystemTypes.Where(x => x.ToDelete).Select(x => x.SystemTypesId);
                var systemTypes = _repositoryWrapper.SystemType.FindByCondition(x => systemTypeIds.Contains(x.Systemtypeid)).ToList();
                foreach (var systemType in systemTypes)
                {
                    RemoveSystemTypeRelation(systemType);
                    _repositoryWrapper.SystemType.DeleteDeep(systemType);
                }
                #endregion


                var session = _mapper.Map<FI_Session>(foreignIndexDto.Session);
                session.Status = ForeignIndexStatus.Committed;
                _repositoryWrapper.FI_Sessions.Update(FI_SessionMapper.Set(session));
                await _repositoryWrapper.SaveAsync();

                transaction.Commit();
                //transaction.Rollback();
                return new ResultDto<ForeignIndexDto>
                {
                    Data = null,
                    Info = "Changes was successfully committed.",
                    Warning = false
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new ResultDto<ForeignIndexDto>
                {
                    Data = foreignIndexDto,
                    Info = "Error on apply : " + ex,
                    Warning = true
                };
            }
        }

        private void RemoveSystemTypeRelation(Systemtypes systemType)
        {
            RemoveSystemTypeMajorHardwareBuild(systemType);
            RemoveSystemTypeSubDomainSpoc(systemType);
        }

        private void RemoveSystemTypeMajorHardwareBuild(Systemtypes systemType)
        {
            var data = _repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(x => x.Systemtypeid == systemType.Systemtypeid, true).AsNoTracking().ToList();
            foreach (var mjHw in data)
            {            
                  _repositoryWrapper.SystemTypesMajorHardwareBuild.DeleteDeep(mjHw);            
            }
        }
        private void RemoveSystemTypeSubDomainSpoc(Systemtypes systemType)
        {
            foreach (var data in _repositoryWrapper.SystemTypesSubDomainSpoc.FindByCondition(x => x.Systemtypeid == systemType.Systemtypeid, true).ToList())
            {
                _repositoryWrapper.SystemTypesSubDomainSpoc.DeleteDeep(data);
            }
        }
        private async Task<ImpactCheckDto> GetImpact(ForeignIndexDto foreignIndexDto, FIGroupRecord correct,
            FIGroupRecord groupRecord, long designComponentId, string opcoDescription,
            ForeignIndexSource foreignIndexSource)
        {
            var designComponent = await _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentid == designComponentId)
                .Include(x => x.Designcomponentfamily)
                .ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Systemtype)
                .SingleOrDefaultAsync();

            var currentName = designComponent.toDesignComponentNameLcmToList(_repositoryWrapper);
            var systemType =
                await _repositoryWrapper.SystemType
                    .FindByCondition(x => x.Systemtypeid == designComponent.Systemtypeid)
                    .Include(x => x.Systemtypesmajorhardwarebuilds)
                    .Include(x => x.Majorsoftwarebuilds)
                    .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                    .SingleOrDefaultAsync();

            var newName = GenerateNewLcmName(foreignIndexDto.Session.Source, correct.Key,SystemTypeMapper.GetSystemTypeMapper(systemType), designComponent);
            return new ImpactCheckDto
            {
                LcmName = currentName.Item1,
                NotToShow = groupRecord.Correct,
                LcmNameNew = newName.Item1,
                Opco = opcoDescription,
                DesignComponentId = designComponentId,
                SystemTypeChecker = GetSystemTypeChecker(SystemTypeMapper.GetSystemTypeMapper(systemType), correct.Key, foreignIndexSource)
            };
        }

        private SystemTypeChecker GetSystemTypeChecker(SystemType systemType, long Key,
            ForeignIndexSource foreignIndexSource)
        {
            return foreignIndexSource switch
            {
                ForeignIndexSource.MajorHardwareBuild => new SystemTypeChecker
                {
                    MajorHardwareId = Key,
                    MajorSoftwareId = systemType.MajorSoftwareBuildsId ?? 0,
                    //todo gestire una funzione per ricreare il NameOem
                    SystemTypeNameOem = systemType.MajorSoftwareBuilds.ProductName != null? systemType.MajorSoftwareBuilds.ProductName.Description:""
                },
                ForeignIndexSource.MajorSoftwareBuild => new SystemTypeChecker
                {
                    MajorHardwareId =
                        systemType.SystemTypesMajorHardwareBuilds.First(x => x.IsMain).MajorHardwareId,
                    MajorSoftwareId = Key,
                    //todo gestire una funzione per ricreare il NameOem
                    SystemTypeNameOem = _repositoryWrapper.MajorSoftwareBuild
                        .FindByCondition(x => x.Majorsoftwarebuildsid == Key && x.Productname  != null)
                        .Single()
                        .Productname.Description
                },
                ForeignIndexSource.DesignComponentFamily => new SystemTypeChecker
                {
                    MajorHardwareId =
                        systemType.SystemTypesMajorHardwareBuilds.First(x => x.IsMain).MajorHardwareId,
                    MajorSoftwareId = systemType.MajorSoftwareBuildsId ?? 0,
                    //todo gestire una funzione per ricreare il NameOem
                    SystemTypeNameOem = systemType.MajorSoftwareBuilds.ProductName != null? systemType.MajorSoftwareBuilds.ProductName.Description:""
                },
                ForeignIndexSource.NotSpecified => throw new ArgumentOutOfRangeException(nameof(foreignIndexSource),
                    foreignIndexSource, null),
                _ => throw new ArgumentOutOfRangeException(nameof(foreignIndexSource), foreignIndexSource, null)
            };
        }

        private (List<string>, string) GenerateNewLcmName(ForeignIndexSource source, long key, SystemType systemType,
            Designcomponents designComponent)
        {
            var newName = new List<string>();
            var newNameString = "";
            switch (source)
            {
                case ForeignIndexSource.MajorHardwareBuild:
                    newName = designComponent.toDestructuredDesignComponentNameLcmToList(
                        _repositoryWrapper,
                        key,
                        systemType.MajorSoftwareBuildsId.Value,
                        designComponent.Designcomponentfamilyid.Value).Item1;

                    newNameString = designComponent.toDestructuredDesignComponentNameLcmToList(
                        _repositoryWrapper,
                        key,
                        systemType.MajorSoftwareBuildsId.Value,
                        designComponent.Designcomponentfamilyid.Value).Item2;
                    break;

                case ForeignIndexSource.MajorSoftwareBuild:
                    newName = designComponent.toDestructuredDesignComponentNameLcmToList(
                        _repositoryWrapper,
                        systemType.SystemTypesMajorHardwareBuilds
                            .FirstOrDefault(x => x.IsMain).MajorHardwareId,
                        key,
                        designComponent.Designcomponentfamilyid.Value).Item1;

                    newNameString = designComponent.toDestructuredDesignComponentNameLcmToList(
                        _repositoryWrapper,
                        systemType.SystemTypesMajorHardwareBuilds
                            .FirstOrDefault(x => x.IsMain).MajorHardwareId,
                        key,
                        designComponent.Designcomponentfamilyid.Value).Item2;
                    break;
                case ForeignIndexSource.DesignComponentFamily:
                    newName = designComponent.toDestructuredDesignComponentNameLcmToList(
                        _repositoryWrapper,
                        systemType.SystemTypesMajorHardwareBuilds
                            .FirstOrDefault(x => x.IsMain).MajorHardwareId,
                        systemType.MajorSoftwareBuildsId.Value,
                        key).Item1;

                    newNameString = designComponent.toDestructuredDesignComponentNameLcmToList(
                        _repositoryWrapper,
                        systemType.SystemTypesMajorHardwareBuilds
                            .FirstOrDefault(x => x.IsMain).MajorHardwareId,
                        systemType.MajorSoftwareBuildsId.Value,
                        key).Item2;
                    break;
                default:
                case ForeignIndexSource.NotSpecified:
                    throw new ArgumentOutOfRangeException();
            }

            return (newName, newNameString);
        }

        private async Task<List<LcmEngineering>> GetLcmEngeenerings(ForeignIndexSource source, long dataId)
        {
            List<LcmEngineering> lcmEngineerings;
            switch (source)
            {
                case ForeignIndexSource.MajorHardwareBuild:

                    lcmEngineerings = await _repositoryWrapper.MajorHardwareBuild
                        .FindByCondition(x => dataId == x.Majorhardwareid)
                        .SelectMany(x => x.Systemtypesmajorhardwarebuilds).Where(x => x.Ismain)
                        .Select(x => x.Systemtype).SelectMany(x => x.Designcomponents)
                        .SelectMany(x => x.Lcmengineering).Include(x => x.Opco)
                        .Select(p=> LCMEngineeringMapper.GetLcmEngineeringMapper(p,true, PatBuildConstructionEnum.AllSoftware))
                        .ToListAsync();
                    break;
                case ForeignIndexSource.MajorSoftwareBuild:
                    lcmEngineerings = await _repositoryWrapper.MajorSoftwareBuild
                        .FindByCondition(x => dataId == x.Majorsoftwarebuildsid)
                        .SelectMany(x => x.Systemtypes).SelectMany(x => x.Designcomponents)
                        .SelectMany(x => x.Lcmengineering).Include(x => x.Opco)
                        .Select(p => LCMEngineeringMapper.GetLcmEngineeringMapper(p, true, PatBuildConstructionEnum.AllSoftware))
                        .ToListAsync();
                    break;
                case ForeignIndexSource.DesignComponentFamily:
                    lcmEngineerings = await _repositoryWrapper.DesignComponentFamily
                        .FindByCondition(x => dataId == x.Designcomponentfamilyid)
                        .SelectMany(x => x.Designcomponents).SelectMany(x => x.Lcmengineering).Include(x => x.Opco)
                        .Select(p => LCMEngineeringMapper.GetLcmEngineeringMapper(p, true, PatBuildConstructionEnum.AllSoftware))
                        .ToListAsync();
                    break;
                case ForeignIndexSource.NotSpecified:
                default:
                    lcmEngineerings = new List<LcmEngineering>();
                    break;
            }

            return lcmEngineerings;
        }

        private async Task<List<NetworkElementAsPlanned>> GetNetworkElements(ForeignIndexSource source, long dataId)
        {
            List<NetworkElementAsPlanned> lcmEngineerings;

            switch (source)
            {
                case ForeignIndexSource.MajorHardwareBuild:

                    lcmEngineerings = await _repositoryWrapper.MajorHardwareBuild
                        .FindByCondition(x => dataId == x.Majorhardwareid)
                        .SelectMany(x => x.Systemtypesmajorhardwarebuilds)
                        .Select(x => x.Systemtype).SelectMany(x => x.Designcomponents)
                        .SelectMany(x => x.Networkelementsasplanned).Include(x => x.Opco)
                        .Select(p => NetworkElementAsPlannedMapper.Get(p, true))
                        .ToListAsync();
                    break;
                case ForeignIndexSource.MajorSoftwareBuild:
                    lcmEngineerings = await _repositoryWrapper.MajorSoftwareBuild
                        .FindByCondition(x => dataId == x.Majorsoftwarebuildsid)
                        .SelectMany(x => x.Systemtypes).SelectMany(x => x.Designcomponents)
                        .SelectMany(x => x.Networkelementsasplanned).Include(x => x.Opco)
                        .Select(p => NetworkElementAsPlannedMapper.Get(p, true))
                        .ToListAsync();
                    break;
                case ForeignIndexSource.DesignComponentFamily:
                    lcmEngineerings = await _repositoryWrapper.DesignComponentFamily
                        .FindByCondition(x => dataId == x.Designcomponentfamilyid)
                        .SelectMany(x => x.Designcomponents).SelectMany(x => x.Networkelementsasplanned)
                        .Include(x => x.Opco)
                        .Select(p => NetworkElementAsPlannedMapper.Get(p, true))
                        .ToListAsync();
                    break;
                case ForeignIndexSource.NotSpecified:
                default:
                    lcmEngineerings = new List<NetworkElementAsPlanned>();
                    break;
            }

            return lcmEngineerings;
        }

        public class PiuUsatoSelector
        {
            public long id { get; set; }
            public int Count { get; set; }
            public DateTime DataModifica { get; set; }
        }
    }

    public class DuplicatedStore<T>
    {
        public long Id { get; set; }
        public int Used { get; set; }
        public T Obj { get; set; }
    }
}