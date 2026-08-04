using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
using System;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public class BudgetProjectTrackerMapper
    {
        public static BudgetProjectTrackers Get(Budgetprojecttrackers model)
        {
            if (model == null)
                return null;
            var result = new BudgetProjectTrackers()
            {

                BudgetProjectTackerId = model.Budgetprojecttrackerid,
                BudgetLineCode = model.Budgetlinecode,
                UploadStatus = model.Uploadstatus,
                UploadMode = model.Uploadmode,
                CurrentTrackingNumber = model.Currenttrackingnumber,
                NewTrackingNumber = model.Newtrackingnumber,
                Wbs = model.Wbs,
                OpcoId = model.Opcoid,
                Opco = model.Opco,
                Domain = model.Domain,
                Team = model.Team,
                BudgetOwner = model.Budgetowner,
                Program = model.Program,
                BudgetProject = model.Budgetproject,
                Activity = model.Activity,
                Priority = model.Priority,
                Driver = model.Plannedactivity?.Driver?.Bptdriverdetails,
                Benefits = model.Benefits,
                Risks = model.Risks,
                Category = model.Category,
                CategoryId = model.Categoryid,
                Nwelement = model.Nwelement,
                VirtualizedNwElement = model.Virtualizednwelement,
                Vendor = model.Vendor,
                LcmCategoriesId = model.Lcmcategoriesid,
                LcmCategories = model.Lcmcategories,
                OhpLev1 = model.Ohplev1,
                OhpLev2 = model.Ohplev2,
                HfmLev1 = model.Hfmlev1,
                HfmLev2 = model.Hfmlev2,
                Fy = model.Fy,
                Operational = model.Operational,
                Transfers = model.Transfers,
                Cost1sTest = model.Cost1stest,
                Validation = model.Validation,
                SignOff = model.Signoff,
                Sub = model.Sub,
                FinalReSub = model.Finalresub,
                LatestScenario = model.Latestscenario,
                Currency = model.Currency,
                Adjustments = model.Adjustments,
                YtdActuals = model.Ytdactuals,
                PlannedAbsorption = model.Plannedabsorption,
                Deviation = model.Deviation,
                Apr = model.Apr,
                May = model.May,
                Jun = model.Jun,
                Jul = model.Jul,
                Aug = model.Aug,
                Sep = model.Sep,
                Oct = model.Oct,
                Nov = model.Nov,
                Dec = model.Dec,
                Jan = model.Jan,
                Feb = model.Feb,
                Mar = model.Mar,
                ApprovedBudget = model.Approvedbudget,
                Commitment = model.Commitment,
                BudgetProjectDependency = model.Budgetprojectdependency,
                LocalProgram = model.Localprogram,
                LocalBudgetProject = model.Localbudgetproject,
                LocalDriver = model.Localdriver,
                LocalPrioritization = model.Localprioritization,
                PpmId = model.Ppmid,
                PpmBudgetProjectId = model.Ppmbudgetprojectid,
                CostCentre = model.Costcentre,
                WpId = model.Wpid,
                GroupBudgetOpcoId = model.Groupbudgetopcoid,
                InternalProgram = model.Internalprogram,
                VerticalProject = model.Verticalproject,
                DomainSpecificLabels = model.Domainspecificlabels,
                LabelsMarketVsVertical = model.Labelsmarketvsvertical,
                OtherMinorVendors = model.Otherminorvendors,
                ExternalDemandBudget = model.Externaldemandbudget,
                OpexImpact = model.Opeximpact,
                LegalEntity = model.Legalentity,
                YearlyTransfersTrack = model.Yearlytransferstrack,
                YearlyAdjustmentsTrack = model.Yearlyadjustmentstrack,
                Mcustom02 = model.Mcustom02,
                Mcustom03 = model.Mcustom03,
                Mcustom04 = model.Mcustom04,
                Mcustom05 = model.Mcustom05,
                Mcustom06 = model.Mcustom06,
                Mcustom07 = model.Mcustom07,
                Mcustom08 = model.Mcustom08,
                Mcustom09 = model.Mcustom09,
                Mcustom10 = model.Mcustom10,
                Vcustom01 = model.Vcustom01,
                Vcustom02 = model.Vcustom02,
                Vcustom03 = model.Vcustom03,
                Vcustom04 = model.Vcustom04,
                Vcustom05 = model.Vcustom05,
                Dcustom01 = model.Dcustom01,
                Dcustom02 = model.Dcustom02,
                Dcustom03 = model.Dcustom03,
                Dcustom04 = model.Dcustom04,
                Dcustom05 = model.Dcustom05,
                Lsdb = model.Lsdb,
                Ls012 = model.Ls012,
                Ls210 = model.Ls210,
                Ls57 = model.Ls57,
                Archive = model.Archive,
                PlannedActivityId = model.Plannedactivityid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                PlannedActivity = PlannedActivityMapper.GetForBpt(model.Plannedactivity),
                DesignAspectId=model.Plannedactivity?.Designaspectid,
                Serviceplanid=model.Plannedactivity?.Serviceplanid,
                LcmEngineeringId= model.Plannedactivity?.Lcmengineeringid,
                NetworkElementAsPlannedId = model.Plannedactivity?.Networkelementasplannedid,

            };
            if (result.PlannedActivity?.NetworkElementAsPlannedId != null)
            {
                result.NetworkElementAsPlannedSubdomainSpoc = model.Plannedactivity?.Networkelementasplannedid != null ?
                                            model.Plannedactivity?.Networkelementasplanned?.Networkelementasplannedsubdomainspoc != null && model.Plannedactivity?.Networkelementasplanned?.Networkelementasplannedsubdomainspoc.Count > 0 ?
                                            model.Plannedactivity?.Networkelementasplanned?.Networkelementasplannedsubdomainspoc.Select(x => x.Subdomainspocid).ToList() : null : null;
            }
            else if (result.PlannedActivity?.LcmEngineeringId != null)
            {
                result.LcmEngineeringSubdomainSpoc = model.Plannedactivity?.Lcmengineeringid != null ?
                                            model.Plannedactivity?.Lcmengineering?.Lcmengineeringsubdomainspoc != null && model.Plannedactivity?.Lcmengineering?.Lcmengineeringsubdomainspoc.Count > 0 ?
                                            model.Plannedactivity?.Lcmengineering?.Lcmengineeringsubdomainspoc.Select(x => x.Subdomainspocid).ToList() : null : null;
            }
            if (result.PlannedActivity?.DesignAspectId != null)
            {
                result.DesignContactDto = model?.Plannedactivity.Designaspect != null ?
                                          model?.Plannedactivity.Designaspect?.Designcomponentfamily?.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts
                                         .Where(x => x.Deleted == false)
                                         .Select(m => (int?)m.Designcontactid))?.
                                          Distinct().ToList() : null;

                var majorHardwareDesignContact = model?.Plannedactivity.Designaspect != null ?
                                          model?.Plannedactivity.Designaspect?.Designcomponentfamily?.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Systemtypesmajorhardwarebuilds?
                                          .SelectMany(m => m.Majorhardware?.Majorhwbuildsdesigncontacts.
                                          Where(x => x.Deleted == false)
                                          .Select(n => (int?)n.Designcontactid))?.
                                          Distinct().ToList())?.ToList() : null;

                if (result.DesignContactDto?.Any() == true && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactDto = result.DesignContactDto.Union(majorHardwareDesignContact).ToList();
                else if (result.DesignContactDto?.Any() == false && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactDto = majorHardwareDesignContact;
            }
            if (result.PlannedActivity?.Serviceplanid != null)
            {
                result.DesignContactDto = model?.Plannedactivity.Serviceplan != null ?
                                          model?.Plannedactivity.Serviceplan?.Serviceplandcfmappings?.SelectMany(x => x.Dcf.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts
                                         .Where(x => x.Deleted == false)
                                         .Select(m => (int?)m.Designcontactid)))?.
                                          Distinct().ToList() : null;
                var majorHardwareDesignContact = model?.Plannedactivity.Serviceplan != null ?
                                          model?.Plannedactivity.Serviceplan?.Serviceplandcfmappings?.SelectMany(x => x.Dcf.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Systemtypesmajorhardwarebuilds?
                                          .SelectMany(m => m.Majorhardware?.Majorhwbuildsdesigncontacts.
                                          Where(x => x.Deleted == false)
                                          .Select(n => (int?)n.Designcontactid))?.
                                          Distinct().ToList()))?.ToList() : null;
                if (result.DesignContactDto?.Any() == true && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactDto = result.DesignContactDto.Union(majorHardwareDesignContact).ToList();
                else if (result.DesignContactDto?.Any() == false && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactDto = majorHardwareDesignContact;
            }
            return result;
        }


        public static Budgetprojecttrackers Set(BudgetProjectTrackers model)
        {
            if (model == null)
                return null;

            var result = new Budgetprojecttrackers
            {
                Budgetprojecttrackerid = model.BudgetProjectTackerId,
                Budgetlinecode = model.BudgetLineCode,
                Uploadstatus = model.UploadStatus,
                Uploadmode = model.UploadMode,
                Currenttrackingnumber = model.CurrentTrackingNumber,
                Newtrackingnumber = model.NewTrackingNumber,
                Wbs = model.Wbs,
                Opcoid = model.OpcoId,
                Opco = model.Opco,
                Domain = model.Domain,
                Team = model.Team,
                Budgetowner = model.BudgetOwner,
                Program = model.Program,
                Budgetproject = model.BudgetProject,
                Activity = model.Activity,
                Priority = model.Priority,
                Driver = model.Driver,
                Benefits = model.Benefits,
                Risks = model.Risks,
                Category = model.Category,
                Categoryid = model.CategoryId,
                Nwelement = model.Nwelement,
                Virtualizednwelement = model.VirtualizedNwElement,
                Vendor = model.Vendor,
                Lcmcategoriesid = model.LcmCategoriesId,
                Lcmcategories = model.LcmCategories,
                Ohplev1 = model.OhpLev1,
                Ohplev2 = model.OhpLev2,
                Hfmlev1 = model.HfmLev1,
                Hfmlev2 = model.HfmLev2,
                Fy = model.Fy,
                Operational = model.Operational,
                Transfers = model.Transfers,
                Cost1stest = model.Cost1sTest,
                Validation = model.Validation,
                Signoff = model.SignOff,
                Sub = model.Sub,
                Finalresub = model.FinalReSub,
                Latestscenario = model.LatestScenario,
                Currency = model.Currency,
                Adjustments = model.Adjustments,
                Ytdactuals = model.YtdActuals,
                Plannedabsorption = model.PlannedAbsorption,
                Deviation = model.Deviation,
                Apr = model.Apr,
                May = model.May,
                Jun = model.Jun,
                Jul = model.Jul,
                Aug = model.Aug,
                Sep = model.Sep,
                Oct = model.Oct,
                Nov = model.Nov,
                Dec = model.Dec,
                Jan = model.Jan,
                Feb = model.Feb,
                Mar = model.Mar,
                Approvedbudget = model.ApprovedBudget,
                Commitment = model.Commitment,
                Budgetprojectdependency = model.BudgetProjectDependency,
                Localprogram = model.LocalProgram,
                Localbudgetproject = model.LocalBudgetProject,
                Localdriver = model.LocalDriver,
                Localprioritization = model.LocalPrioritization,
                Ppmid = model.PpmId,
                Ppmbudgetprojectid = model.PpmBudgetProjectId,
                Costcentre = model.CostCentre,
                Wpid = model.WpId,
                Groupbudgetopcoid = model.GroupBudgetOpcoId,
                Internalprogram = model.InternalProgram,
                Verticalproject = model.VerticalProject,
                Domainspecificlabels = model.DomainSpecificLabels,
                Labelsmarketvsvertical = model.LabelsMarketVsVertical,
                Otherminorvendors = model.OtherMinorVendors,
                Externaldemandbudget = model.ExternalDemandBudget,
                Opeximpact = model.OpexImpact,
                Legalentity = model.LegalEntity,
                Yearlytransferstrack = model.YearlyTransfersTrack,
                Yearlyadjustmentstrack = model.YearlyAdjustmentsTrack,
                Mcustom02 = model.Mcustom02,
                Mcustom03 = model.Mcustom03,
                Mcustom04 = model.Mcustom04,
                Mcustom05 = model.Mcustom05,
                Mcustom06 = model.Mcustom06,
                Mcustom07 = model.Mcustom07,
                Mcustom08 = model.Mcustom08,
                Mcustom09 = model.Mcustom09,
                Mcustom10 = model.Mcustom10,
                Vcustom01 = model.Vcustom01,
                Vcustom02 = model.Vcustom02,
                Vcustom03 = model.Vcustom03,
                Vcustom04 = model.Vcustom04,
                Vcustom05 = model.Vcustom05,
                Dcustom01 = model.Dcustom01,
                Dcustom02 = model.Dcustom02,
                Dcustom03 = model.Dcustom03,
                Dcustom04 = model.Dcustom04,
                Dcustom05 = model.Dcustom05,
                Lsdb = model.Lsdb,
                Ls012 = model.Ls012,
                Ls210 = model.Ls210,
                Ls57 = model.Ls57,
                Archive = model.Archive,
                Plannedactivityid = model.PlannedActivityId

            };
            return result;
        }

    }
}
