using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class CopyGridcustomcolumn
    {
        public string Gridcustomcolumnid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public int Userid { get; set; }
        public string Jsongridcustomizationdata { get; set; }
        public string Classname { get; set; }
    }
}
