using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Exceltemplateconfiguration
    {
        public int Exceltemplateconfigurationid { get; set; }
        public string Processname { get; set; }
        public string Propertyname { get; set; }
        public string Columnheadername { get; set; }
        public int? Columnorder { get; set; }
        public string Fontcolor { get; set; }
        public string Cellcolor { get; set; }
        public bool? Isimportfield { get; set; }
        public bool? Ismandatory { get; set; }
        public string Datatype { get; set; }
        public string Defaultvalue { get; set; }
        public string Mappingreference { get; set; }
        public bool? Ismerged { get; set; }
        public int? Mergestartcolumn { get; set; }
        public int? Mergeendcolumn { get; set; }
        public int? Rowstarting { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public bool? Isexportfield { get; set; }
        public int? Headerrowstarting { get; set; }
        public string Templatefilename { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
