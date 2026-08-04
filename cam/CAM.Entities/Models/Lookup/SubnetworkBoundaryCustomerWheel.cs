using CAM.Entities.Models.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CAM.Entities.Models.Lookup
{
    public class SubnetworkBoundaryCustomerWheel : AuditableEntity
    {
        [Key]
        public long Id { get; set; }

        public long SubnetworkBoundaryId { get; set; }

        public int CustomerWheelId { get; set; }

        [ForeignKey(nameof(CustomerWheelId))]
        public virtual CustomerWheel CustomerWheel { get; set; }
        [ForeignKey(nameof(SubnetworkBoundaryId))]
        public virtual SubNetworkBoundary SubNetworkBoundary { get; set; }


    }
}
