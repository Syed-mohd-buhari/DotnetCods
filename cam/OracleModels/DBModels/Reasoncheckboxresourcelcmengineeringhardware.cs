using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Reasoncheckboxresourcelcmengineeringhardware
    {
        public long Lcmengineeringid { get; set; }
        public short Reasoncheckboxresourceid { get; set; }
        public decimal Id { get; set; }

        public virtual Lcmengineering Lcmengineering { get; set; }
        public virtual Reasoncheckboxresources Reasoncheckboxresource { get; set; }
    }
}
