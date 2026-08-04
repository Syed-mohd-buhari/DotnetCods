using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ArchivedPlannedActivityQueryDto: QueryObject
    {
        public List<long> PlannedActivityId { get; set; }
        public List<short> PlannedActivityResourceId { get; set; }
        public List<short> OpCo { get; set; }
        public List<long> DesignComponentFamilyName { get; set; }
        public List<long> OriginalDesignComponent { get; set; }
        public List<long> PlannedActivityDesignComponentId { get; set; }
        public List<string> ActivityDetails { get; set; }
        public List<string> Program { get; set; }
        public List<string> ProjectOwner { get; set; }
        public List<short> PlannedImplementationYear { get; set; }
        public List<bool> Archived { get; set; }
        public DateFilter StartDateValue { get; set; }
        public DateFilter PlannedCompletion { get; set; }

        public List<string> VerticalName { get; set; }
        public List<int> VerticalNameId { get; set; }
        #region Ticket 685 Dev - #674 SettingsUpdatePlanedActivity - Display  Planned Activity Associated/Linked Table Details  - Ex : LCM, DA, Assets
        public List<bool?> ForLcmLink { get; set; }


        public List<bool?> ForDesignAspectLink { get; set; }
        public List<bool?> ForServicePlanLink { get; set; }
        public bool? AddEditAssetFilter { get; set; }
        #endregion

        //Ticket 751 PPM Import

        public List<string> DeliveryProjectPpmId { get; set; }

        public List<long> PlannedBuildBagDescription { get; set; }
        public List<long> CurrentBuildBagDescription { get; set; }
        public DateFilter ModificationDate { get; set; }
    }
}
