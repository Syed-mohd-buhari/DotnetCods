using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.Organisation
{
    public class OrganisationDto : GridDtoBase
    {

        [OrderGrid(Order = 5)]
        [Default]
        public string MainOrganisation { get; set; }

        [OrderGrid(Order = 6)]
        [Default]
        public string Practice { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        public string VerticalResponsible { get; set; }

        [OrderGrid(Order = 7)]
        [Default]
        [IgnoreGrid]
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

        public int PracticeContactId { get; set; }

        [OrderGrid(Order = 20)]
        [IgnoreGrid]
        public int? VerticalResponsibleId { get; set; }


    }

    public class OrganisatioDtoGrid : OrganisationDto
    {
        [IgnoreGrid]
        public long OrganisationId { get; set; }
        [IgnoreGrid]
        public int? VerticalId { get; set; }
    }
}
