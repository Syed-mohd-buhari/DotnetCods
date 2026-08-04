using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Budgetprojecttrackers
    {
        public long Budgetprojecttrackerid { get; set; }
        public string Budgetlinecode { get; set; }
        public string Uploadstatus { get; set; }
        public string Uploadmode { get; set; }
        public string Currenttrackingnumber { get; set; }
        public string Newtrackingnumber { get; set; }
        public string Wbs { get; set; }
        public short? Opcoid { get; set; }
        public string Opco { get; set; }
        public string Domain { get; set; }
        public string Team { get; set; }
        public string Budgetowner { get; set; }
        public string Program { get; set; }
        public string Budgetproject { get; set; }
        public string Activity { get; set; }
        public string Priority { get; set; }
        public string Driver { get; set; }
        public string Benefits { get; set; }
        public string Risks { get; set; }
        public string Category { get; set; }
        public short? Categoryid { get; set; }
        public string Nwelement { get; set; }
        public string Virtualizednwelement { get; set; }
        public string Vendor { get; set; }
        public short? Lcmcategoriesid { get; set; }
        public string Lcmcategories { get; set; }
        public string Ohplev1 { get; set; }
        public string Ohplev2 { get; set; }
        public string Hfmlev1 { get; set; }
        public string Hfmlev2 { get; set; }
        public string Fy { get; set; }
        public string Operational { get; set; }
        public string Transfers { get; set; }
        public string Cost1stest { get; set; }
        public string Validation { get; set; }
        public string Signoff { get; set; }
        public string Sub { get; set; }
        public string Finalresub { get; set; }
        public string Latestscenario { get; set; }
        public string Currency { get; set; }
        public string Adjustments { get; set; }
        public string Ytdactuals { get; set; }
        public string Plannedabsorption { get; set; }
        public string Deviation { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Approvedbudget { get; set; }
        public string Commitment { get; set; }
        public string Budgetprojectdependency { get; set; }
        public string Localprogram { get; set; }
        public string Localbudgetproject { get; set; }
        public string Localdriver { get; set; }
        public string Localprioritization { get; set; }
        public string Ppmid { get; set; }
        public string Ppmbudgetprojectid { get; set; }
        public string Costcentre { get; set; }
        public string Wpid { get; set; }
        public string Groupbudgetopcoid { get; set; }
        public string Internalprogram { get; set; }
        public string Verticalproject { get; set; }
        public string Domainspecificlabels { get; set; }
        public string Labelsmarketvsvertical { get; set; }
        public string Otherminorvendors { get; set; }
        public string Externaldemandbudget { get; set; }
        public string Opeximpact { get; set; }
        public string Legalentity { get; set; }
        public string Yearlytransferstrack { get; set; }
        public string Yearlyadjustmentstrack { get; set; }
        public string Mcustom02 { get; set; }
        public string Mcustom03 { get; set; }
        public string Mcustom04 { get; set; }
        public string Mcustom05 { get; set; }
        public string Mcustom06 { get; set; }
        public string Mcustom07 { get; set; }
        public string Mcustom08 { get; set; }
        public string Mcustom09 { get; set; }
        public string Mcustom10 { get; set; }
        public string Vcustom01 { get; set; }
        public string Vcustom02 { get; set; }
        public string Vcustom03 { get; set; }
        public string Vcustom04 { get; set; }
        public string Vcustom05 { get; set; }
        public string Dcustom01 { get; set; }
        public string Dcustom02 { get; set; }
        public string Dcustom03 { get; set; }
        public string Dcustom04 { get; set; }
        public string Dcustom05 { get; set; }
        public string Lsdb { get; set; }
        public string Ls012 { get; set; }
        public string Ls210 { get; set; }
        public string Ls57 { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public bool? Archive { get; set; }
        public long? Plannedactivityid { get; set; }

        public virtual Plannedactivitycategory CategoryNavigation { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos OpcoNavigation { get; set; }
        public virtual Plannedactivities Plannedactivity { get; set; }
    }
}
