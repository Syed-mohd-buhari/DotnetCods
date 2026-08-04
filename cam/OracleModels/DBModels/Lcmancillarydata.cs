using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Lcmancillarydata
    {
        public long Lcmancillarydataid { get; set; }
        public long Lcmengineeringid { get; set; }
        public string Productcode { get; set; }
        public bool? Handedovertooperation { get; set; }
        public string Contractrenewalplan { get; set; }
        public string Reasonfornoplan { get; set; }
        public string Commentonprojectstatus { get; set; }
        public string Scopeofsimplification { get; set; }
        public string Datasource { get; set; }
        public string Incidentclass { get; set; }
        public string Occurenceprobability { get; set; }
        public string Securityriskeffective { get; set; }
        public string Securitymitigation { get; set; }
        public string Assetoutofscope { get; set; }
        public bool? Includedinsecurityscanning { get; set; }
        public string Raid { get; set; }
        public string Requestid { get; set; }
        public DateTime? Lastscandate { get; set; }
        public DateTime? Lastupgradedate { get; set; }
        public string Eomcontrol { get; set; }
        public string Engupdatetracker { get; set; }
        public string Opsupdatetracker { get; set; }
        public string Exnetworks { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Originalhwlcmid { get; set; }
        public string Originalswlcmid { get; set; }
        public bool? Externalfacingflag { get; set; }
        public string Locationinfrastructure { get; set; }
        public bool? Ispecn { get; set; }
        public bool? Ispecs { get; set; }
        public bool? Isscf { get; set; }
        public bool? Isnof { get; set; }
        public string Vulnerabilityrating { get; set; }
        public string Cyberriskrequestid { get; set; }
        public string Lastscanrefnumber { get; set; }
        public string Lastpentestreferencenumber { get; set; }
        public DateTime? Lastpentestdate { get; set; }
        public bool? Isexposededge { get; set; }
        public string Riskcomment { get; set; }
        public string Qid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Lcmengineering Lcmengineering { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
