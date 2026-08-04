using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    public class SubnetworkBoundarySystemFunction: AuditableEntity
    {
        [Key]
        public long Id { get; set; }

        public long SubnetworkBoundaryId { get; set; }

        public short SystemFunctionId { get; set; }

        [ForeignKey(nameof(SystemFunctionId))]
        public virtual SystemFunction SystemFunction { get; set; }
        [ForeignKey(nameof(SubnetworkBoundaryId))]
        public virtual SubNetworkBoundary SubNetworkBoundary { get; set; }
    }
}
