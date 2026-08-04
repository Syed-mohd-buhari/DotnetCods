using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models
{
    [Table("VolteKPI")]
    public class VolteKPI : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("VolteKPIId")]
        public long VolteKPIId { get; set; }

        public short OpCoId { get; set; }

        public short Month { get; set; }
        public short Year { get; set; }

        public decimal? KPIOneEoYTarget { get; set; }
        public decimal? KPIOneMonthlyTarget { get; set; }
        public decimal? KPIOneActualValue { get; set; }
        public decimal? KPIOneTargetValueChangeProposal { get; set; }
        public string KPIOneComment { get; set; }

        public decimal? KPITwoActualNumberOfRegisteredSubscribers { get; set; }
        public decimal? KPITwoActualNumberOfProvisionedSubscriber { get; set; }
        public string KPITwoComment { get; set; }

        public decimal? KPIThreeEoYTarget { get; set; }
        public decimal? KPIThreeMonthlyTarget { get; set; }
        public decimal? KPIThreeActualValue { get; set; }
        public decimal? KPIThreeTargetValueChangeProposal { get; set; }
        public string KPIThreeComment { get; set; }

        public decimal? KPIFourFinalTarget { get; set; }
        public decimal? KPIFourTargetMonthly { get; set; }
        public decimal? KPIFourActualMonthly { get; set; }
        public decimal? KPIFourTargetValueChangeProposal { get; set; }
        public short? KPIFourTargetDateMonth { get; set; }
        public short? KPIFourTargetDateYear { get; set; }
        public string KPIFourComment { get; set; }


        [ForeignKey(nameof(OpCoId))]
        [InverseProperty("Voltekpi")]
        public virtual OpCo OpCo { get; set; }


    }
}
