using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.Entita.Organisation;
using System;
using System.ComponentModel;
namespace CAM.DataTransferObjects.Entita.AspNetUser
{
    public class AspnetuserGridDto : AspnetOrganisationDto
    {
        [Default]
        [OrderGrid(Order = 1)]
        public int UserId { get; set; }
        [Default]
        [OrderGrid(Order = 2)]
        public string UserName { get; set; }
        [Default]
        [OrderGrid(Order = 3)]
        public string Email { get; set; }
        [Default]
        [OrderGrid(Order = 4)]
        public bool Active { get; set; }

    }
    public class AspnetOrganisationDto
    {
        [IgnoreGrid]
        public long OrganisationId { get; set; }
        [OrderGrid(Order = 5)]
        [Default]
        public string MainOrganisation { get; set; }

        [OrderGrid(Order = 6)]
        [Default]
        public string Practice { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        public string PracticeContact { get; set; }

        [OrderGrid(Order = 9)]
        [IgnoreGrid]
        [Default]
        public int MainOrganisationId { get; set; }

        [OrderGrid(Order = 10)]
        [IgnoreGrid]
        [Default]
        public int PracticeId { get; set; }

        [OrderGrid(Order = 11)]
        [IgnoreGrid]
        [Default]
        public int PracticeContactId { get; set; }

        [OrderGrid(Order = 14)]
        [Default]
        public string SubdomainResponsible { get; set; }

        [OrderGrid(Order = 15)]
        [IgnoreGrid]
        public int SubdomainResponsibleId { get; set; }
        [Default]
        [OrderGrid(Order = 17)]
        public bool IsSubDomainSpoc { get; set; }
        [Default]
        [OrderGrid(Order = 18)]
        public bool IsEduSpoc { get; set; }
        [Default]
        [OrderGrid(Order = 19)]
        public bool? IsDesigncontact { get; set; }
        [Default]
        [OrderGrid(Order = 20)]
        public string OpCo { get; set; }
        [Default]
        [OrderGrid(Order = 21)]
        public string Vertical { get; set; }
        [Default]
        [OrderGrid(Order = 22)]
        public string VerticalResponsible { get; set; }
        [Default]
        [OrderGrid(Order = 23)]
        public string Role { get; set; }

        [OrderGrid(Order = 24)]
        [DateRangeGrid]
        [DisplayName("Last Modified Date")]
        [Default]
        public virtual DateTime? LastModified { get; set; }
        [OrderGrid(Order = 25)]
        [MailTo]
        [Default]
        [DisplayName("Last Modified By")]
        public virtual string LastModifiedBy { get; set; }
    }
}
