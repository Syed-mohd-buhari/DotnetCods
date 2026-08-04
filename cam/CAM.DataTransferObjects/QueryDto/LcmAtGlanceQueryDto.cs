
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;
using System;

namespace CAM.DataTransferObjects.QueryDto
{
    public class LcmAtGlanceQueryDto : QueryObject
    {
       
        public List<short> OpcoId { get; set; }
        public List<string> OpCoDescrption { get; set; }

        public List<short> ProductId { get; set; }

        public List<short> SupportedServicesId { get; set; }

        public List<short> PlannedActivityResourceRuleId { get; set; }

        public List<short> DeliveryStatusId { get; set; }

        public List<short> VerticalResponsibleId { get; set; }

        public DateTime? SelectedDate { get; set; }
       
        public List<short> ProductimportanceId { get; set; }
    }
}
