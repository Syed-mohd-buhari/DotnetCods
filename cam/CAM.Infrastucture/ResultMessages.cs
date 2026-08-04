namespace CAM.Infrastucture
{
    public static class ResultMessages
    {
        //Add
        public static readonly string EntryAddSuccess = "Entry added successfully";
        public static readonly string EntryAddExists = "The entry specified already exists.";
        public static readonly string EntryAddExistsDeleted = "The entry specified is already present but it has been hidden. \nPlease contact an administrator to solve this issue.";
        public static readonly string EntryNotAdd = "An entry not added.";
        public static readonly string EntryNotFound = "An entry not found.";

        //Update
        public static readonly string EntryUpdateSuccess = "Entry updated successfully";
        public static readonly string EntryUpdateExists = "An entry equal to the one specified already exists.";
        public static readonly string EntryUpdateExistsDeleted = "An entry equal to the one specified is already present but it has been hidden. \nPlease contact an administrator to solve this issue.";
        public static readonly string EntryUpdateNotExists = "An entry to update was not exists.";
        public static readonly string EntryNotUpdate = "An entry not update.";

        public static readonly string EntryUpdateSuccessWithDuplicateAsset =
            "LCM already has assets with the same name. These assets not be migrated and marked as removed.";

        public static readonly string EntryAddUpdateSuccess = "Entry added or updated successfully";
        public static readonly string EntryAddUpdateFailed = "Entry failed to update";
        public static readonly string EntryAlreadyExists = "Entry already exists ";
        //Assets
        public static readonly string EntryLcmIdExists = "LCM found";
        public static readonly string EntryLcmIdNotExists = "LCM not found";
        public static readonly string EntryLcmIdNotExistsForOpcoDesignComponent = "LCM does not exist for this OPCO and DesignComponent";
        public static readonly string EntryLcmIdExistsWithDifferentStatus = "Kindly create a planned activity for updating the asset status ";
        public static readonly string EntryLcmIdParameterIssue = "Incorrect allocation of the request parameter";


        //Reset
        public static readonly string EntryResetSuccess = "Entry reseted successfully";
        public static readonly string EntryResetExists = "An entry equal to the one specified already exists.";
        public static readonly string EntryResetExistsDeleted = "An entry equal to the one specified is already present but it has been hidden. \nPlease contact an administrator to solve this issue.";
        public static readonly string EntryResetNotExists = "An entry to reset was not exists.";

        //Delete
        public static readonly string EntryDeleteSuccess = "Entry deleted successfully";
        public static readonly string EntryDeleteNotDeleted = "Entry not deleted";
        public static readonly string EntryDeleteNotExists = "The entry specified cannot be found.";
        public static readonly string EntryDeleteExistsDeleted = "The entry specified has been already deleted.";
        public static readonly string EntryDeleteNotOrphan = "The record is not orphaned.";
        public static readonly string EntryDeleteHasPlannedActivities = "The record has related planned activities.";
        public static readonly string EntryDeleteHasLinkedPlannedActivities = "The record has related planned activities with linked planned activitiy.";
        public static readonly string EntryDeleteHasActiveLCms = "This record is linked to LCM entities that are associated with the DesignComponentFamily";
        //public static readonly string EntryDeleteHasActiveLCms = "The record has related LCM entities.";
        //MultipleDCF 
        public static readonly string PlannedDCFEntryHasLinkedPlannedActivities = "The Planned DesignComponentFamilies have related planned activities within the same OpCo, Project, and Program combination.";
        //Other errors
        public static readonly string SystemError = "An internal server error occurred during the operation.\nPlease contact an administrator to solve this issue";
        public static readonly string NoPlannedActivity = "no planned activity was found";
        public static readonly string NoDesignComponent = "no design component was found";
        public static readonly string NoSystemType = "No System type was found";
        public static readonly string NoNetworkElement = "No Network Element As Planned was found in this Family";
        public static readonly string NoVolteKPI = "No VoLTE KPI was found";
        public static readonly string MailQueueInsert = "Insert Mail on queue success";
        public static readonly string MailQueueSended = "Mail sent with success";
        public static readonly string MailQueueErrorOnSend = "Error occurred on sending mail";
        public static readonly string MigrationNumberOfNodes = "Number of nodes of source LCMEngineering is less than number of nodes to migrate";
        public static readonly string AssetCategoryExists = "the specified Category already has an associated Asset Class";
        public static readonly string NoVolteKPIWorklog = "No VoLTE KPI Worklog was found or VoLTE KPI Worklog is already processed";
        public static readonly string VolteKPIWorklogExist = "There is a pending proposal for this VolteKPI";
        public static readonly string NoDeploymentStatus = "no deployment status was found";
        public static readonly string NoPlannedActivityManageMig = "No Planned Activity was found (Manage migration is applicable only for the planned activity in FSI status).";


        public static readonly string SubNetworkBoundariesEmptyLst = "Please Select At Least SubNetwork Boundary To be Able to Add New DC";

        //Auth errors
        public static readonly string AuthError = "The user is not authorized to access the application";



        public static readonly string ConfigurationGridSuccess = "Your configuration has been saved";

        public static readonly string GetInfoSuccess = "Get Info successfully";

        public static readonly string GetInfoNoFound = "Information Not Found";
        public static readonly string AnIssueOccurredMsg = "An issue occurred while fetching the data.";

        public static readonly string GetInfoDc = "designcomponent";
        public static readonly string GetInfoSt = "systemtype";

        public static readonly string ErrorValidation = "Check the fields entered";

        public static readonly string MajorSoftwareBuildAddExists = "The MajorSoftwareBuild specified already exists.";
        public static readonly string MajorSoftwareBuildAddExistsDeleted = "The MajorSoftwareBuild specified is already present but it has been hidden. \nPlease contact an administrator to solve this issue.";
        public static readonly string MajorHardwareBuildAddExists = "The MajorHardwareBuild specified already exists.";
        public static readonly string MajorHardwareBuildAddExistsDeleted = "The MajorHardwareBuild specified is already present but it has been hidden. \nPlease contact an administrator to solve this issue.";
        public static readonly string SystemTypeAddExists = "The SystemType specified already exists.";
        public static readonly string SystemTypeAddExistsDeleted = "The SystemType specified is already present but it has been hidden. \nPlease contact an administrator to solve this issue.";
        public static readonly string DesignComponentAddExists = "The DesignComponent specified already exists.";
        public static readonly string DesignComponentAddExistsDeleted = "The DesignComponent specified is already present but it has been hidden. \nPlease contact an administrator to solve this issue.";

        public static readonly string ThePlannedLcmNotExist = "The Planned LCM not Exist";

        public static readonly string NoDeploymentStatusExistForThePlannedActivity = "No Deployment Status Exist for The earliest Planned Activity";

        // Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow
        public static readonly string ModernizeAssetStatusInfo = "Please Add At Least One Planned/In-Commissioning Nodes For Modernize Flow";

        //Org Table 
        public static readonly string EduAndSubDomainSpocDisable = "Contact Is Associated With LCM,Please Check The LCM Before Making Changes  ";
        public static readonly string SubDomainSpocDisable = "Contact Is Associated With LCM {0},Please Check The LCM Before Making Changes  ";

        public static readonly string RelationShip = "You cannot delete this record, it has a relationship with other data.";
        public static readonly string DeactivityRelation = "You cannot deactivity this record, it has a relationship with other data.";



        //Import
        public static readonly string ImportSuccess = "Data imported Successfully";
        public static readonly string ImportFailed = "Data import Failed for these elements : ";
        public static readonly string NoFile = "Please try agian with valid file format";
        public static readonly string NoDataFoundInFile = "No data found in the file.";
        public static readonly string ImportFail = "Data import Failed ";

        // Log Erro 
        public static readonly string errorLogTracke = "Issue happen";

        //Vbom
        public static readonly string duplicateInfo = "Duplicate Info";
        public static readonly string duplicateCapacity = "Duplicate Capacity :";

        // Exodus - Platform migration
        public static readonly string NoDaAssetMigration = "No Da Asset Migration was found";
        public static readonly string EmptyDaAssetMigration = "Da Asset Migration is empty";
        public static readonly string DaAssetMustBeInservice = "Please check that all assets are not in In-Service status ";
        public static readonly string DaAssetIsRemovedStatus = "Asset is removed satus";
        public static readonly string DaTargetDCNotExists = "TargetDC Not Exists";
        //Vnf 
        public static readonly string VnfHardwareType = "Hardward Type -";
        public static readonly string VnfHardwareTypeLinkedClusterInfo = "is Linked with CNF CLuster Info";

        public static readonly string AddOrUpdateFailed = "Data not inserted or updated.";

        // Cluster Level LCM PA 
        public static readonly string NoInfraClusterRecord = "ClusterLevel record was found";
        public static readonly string NoClusterUpgradeRecord = "ClusterUpgrade record was found";

        public static readonly string NoModuleSelected = "Please select the module for the respective role";
    }
}
