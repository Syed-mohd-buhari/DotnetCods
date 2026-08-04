using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models.Cross
{
    public partial class LcmOperationalContracts : AuditableEntity
    {
        public short Id { get; set; }
        public long LcmId { get; set; }
        public short OperationalContractId { get; set; }
        public LcmEngineering LcmEngineering { get; set; }
        public OperationalContract OperationalContract { get; set; }
    }
}
