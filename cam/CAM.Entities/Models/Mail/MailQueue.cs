using CAM.Entities.Models.Base;
using CAM.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.Mail
{
    [Table("MailQueue")]
    public class MailQueue : AuditableEntity
    {
        public MailQueue()
        {
            State = MailStateEnum.Insert;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MailId")]
        public long MailId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public MailStateEnum State { get; set; }
        public string ErrorMessage { get; set; }
        public int ApprovalUserId { get; set; }
        [ForeignKey(nameof(ApprovalUserId))]
        public ApplicationUser ApprovalUserEntity { get; set; }
    }
}
