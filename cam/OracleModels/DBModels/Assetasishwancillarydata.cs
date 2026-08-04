using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Assetasishwancillarydata
    {
        public long Assetasishwancillarydataid { get; set; }
        public long? Networkelementasisid { get; set; }
        public string Site { get; set; }
        public string Host { get; set; }
        public string Datasourcename { get; set; }
        public string Provider { get; set; }
        public string Providertype { get; set; }
        public string Consumer { get; set; }
        public string Consumertype { get; set; }
        public string Consumerrole { get; set; }
        public string Clustername { get; set; }
        public string Partnumber { get; set; }
        public string Systemtype { get; set; }
        public string Manufacturer { get; set; }
        public string Biosversion { get; set; }
        public string Model { get; set; }
        public string Sku { get; set; }
        public int? Cpucapacity { get; set; }
        public int? Ephemeralstoragecapacity { get; set; }
        public int? Memorycapacity { get; set; }
        public string Processorsummarymodel { get; set; }
        public string Kubernetesnodetype { get; set; }
        public string Managementip { get; set; }
        public string Kubernetesnodename { get; set; }
        public string Kubeletversion { get; set; }
        public string Kubernetesnodeos { get; set; }
        public string Kubernetesnoderesourcetype { get; set; }
        public string Kubernetesnodestate { get; set; }
        public DateTime? Kubernetesnodestatusupdatetime { get; set; }
        public string Chassisdetails { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
    }
}
