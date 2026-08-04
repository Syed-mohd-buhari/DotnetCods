using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Plannedactivityresources
    {
        public Plannedactivityresources()
        {
            Plannedactivities = new HashSet<Plannedactivities>();
            Plannedactivityresourcebenefit = new HashSet<Plannedactivityresourcebenefit>();
            Plannedactivityresourcedriver = new HashSet<Plannedactivityresourcedriver>();
            Plannedactivityresourceplanningrisk = new HashSet<Plannedactivityresourceplanningrisk>();
            SettingsupdateplannedactivityPlannedactivityresource = new HashSet<Settingsupdateplannedactivity>();
            SettingsupdateplannedactivitySuccessorplannedactivityresource = new HashSet<Settingsupdateplannedactivity>();
        }

        public short Plannedactivityresourceid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Plannedactivityresource { get; set; }
        public bool Exportable { get; set; }
        public string Jsonform { get; set; }
        public bool Forlcm { get; set; }
        public bool Fornetworkelement { get; set; }
        public bool Forcreatenetworkelement { get; set; }
        public bool Foreditnetworkelement { get; set; }
        public int Rulenetworkelement { get; set; }
        public int Ruleacticvitydetails { get; set; }
        public int Rulelinkeddc { get; set; }
        public int Ruleactdetailsntkelement { get; set; }
        public bool Lcmhardware { get; set; }
        public bool Lcmsoftware { get; set; }
        public bool Networkelementhardware { get; set; }
        public bool Networkelementsoftware { get; set; }
        public bool Onbaremetalnetworkelement { get; set; }
        public bool Onvirtualizednetworkelement { get; set; }
        public bool Plandesigncompreqnwelement { get; set; }
        public string Activitydetailsnetworkelement { get; set; }
        public string Benefittextnetworkelement { get; set; }
        public string Drivertextnetworkelement { get; set; }
        public string Lcmlabelhardware { get; set; }
        public string Lcmlabelsoftware { get; set; }
        public string Actdetailsforvrtnetworkelement { get; set; }
        public string Activitydetailslcm { get; set; }
        public string Networkelementlabelsoftware { get; set; }
        public string Networkelementlabelhardware { get; set; }
        public string Designaspectlabelsoftware { get; set; }
        public string Designaspectlabelhardware { get; set; }
        public bool? Fordesignaspect { get; set; }
        public string Activitydetailsdesignaspect { get; set; }
        public bool? Designaspectsoftware { get; set; }
        public bool? Designaspecthardware { get; set; }
        public int? Ruledesignaspect { get; set; }
        public bool? Designaspectexportable { get; set; }
        public bool? Foraddasset { get; set; }
        public bool? Foreditasset { get; set; }
        public bool? Forcreateaddasset { get; set; }
        public bool? Foreditaddasset { get; set; }
        public int? Ruleaddasset { get; set; }
        public bool? Addassethardware { get; set; }
        public bool? Addassetsoftware { get; set; }
        public bool? Onbaremetaladdasset { get; set; }
        public bool? Onvirtualizedaddasset { get; set; }
        public string Activitydetailsaddasset { get; set; }
        public string Benefittextaddasset { get; set; }
        public string Drivertextaddasset { get; set; }
        public string Actdetailsforvrtaddasset { get; set; }
        public string Addassetlabelsoftware { get; set; }
        public string Addassetlabelhardware { get; set; }
        public int? Ruleactdetailsaddasset { get; set; }
        public bool? Plandesigncompreqaddasset { get; set; }
        public bool? Forcreateeditasset { get; set; }
        public bool? Forediteditasset { get; set; }
        public int? Ruleeditasset { get; set; }
        public bool? Editassethardware { get; set; }
        public bool? Editassetsoftware { get; set; }
        public bool? Onbaremetaleditasset { get; set; }
        public bool? Onvirtualizededitasset { get; set; }
        public string Activitydetailseditasset { get; set; }
        public string Benefittexteditasset { get; set; }
        public string Drivertexteditasset { get; set; }
        public string Actdetailsforvrteditasset { get; set; }
        public string Editassetlabelsoftware { get; set; }
        public string Editassetlabelhardware { get; set; }
        public int? Ruleactdetailseditasset { get; set; }
        public bool? Plandesigncompreqeditasset { get; set; }
        public bool? Forserviceplan { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Plannedactivitytypes RulelinkeddcNavigation { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
        public virtual ICollection<Plannedactivityresourcebenefit> Plannedactivityresourcebenefit { get; set; }
        public virtual ICollection<Plannedactivityresourcedriver> Plannedactivityresourcedriver { get; set; }
        public virtual ICollection<Plannedactivityresourceplanningrisk> Plannedactivityresourceplanningrisk { get; set; }
        public virtual ICollection<Settingsupdateplannedactivity> SettingsupdateplannedactivityPlannedactivityresource { get; set; }
        public virtual ICollection<Settingsupdateplannedactivity> SettingsupdateplannedactivitySuccessorplannedactivityresource { get; set; }
    }
}
