namespace CAM.Enum
{
    public enum PlannedActivityResourceEnum
    {
        Not_applicable = 0,
        SwArchitectureUpgrade_SwMajorRelease = 1,
        HWRefresh = 2,
        HWReplace = 3,
        SystemRefresh = 4,
        HardwareUpgrade = 5,// Used in ReportHardwareManager file
        Refactor = 6, // Used in DesigncomponentFamilyLifecycle - CreateDCFLifecycleforLCMTransition()
        Release_Details_Unknown = 7,
        New_NFxI_Solution = 8, // Used in UI for LCM Planned Status based Select    
        Replace_Solution_Change_Equipment_Manufacturer = 9,// Replace Solution in Table -  Used in LCM Engineering Page - CreateNewLcmAndPlannedActivity()
        Modernize_System_incl_virtualization = 10,// Modernize System in Table -  Used in LCM Engineering Page - CreateNewLcmAndPlannedActivity()
        New_Solution = 11,// Used in LCM Engineering Page - GetModernizeAndReplacePAProperties()                
        New_System_HW_SW_Solution = 12, // Used in UI for LCM Planned Status based Select 
        Decommission_Service_Node = 13, // Used for Decommission Flow
        Modernize_Solution_Successor_Network = 14, //Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow
        No_PlannedActivity = 15, // Req  NO PA work Flow
        Component_Upgrade = 16, // Req Component upgrade work Flow
        Project_Plan = 17, // Req Multiple DCF Planned Activty work Flow
        Infra_Readiness = 18, // Req Exodus DA -PA Program Flow
        Platform_Migration = 19, // Req Exodus DA -PA Program Flow
        Decommision_Node = 20,// Req Decommision_Node Flow
        Decommission_Subnetwork = 21,// Req Decommission_Subnetwork Flow
        Hardware_Reduction = 22, // Req Hardware_Reduction Flow
        Hardware_Expansion = 23, // Req Hardware_Expansion Flow
        Traffic_Migration = 24, // Req Traffic_Migration Flow
        New_Feature_Redesign = 25, // Req New_Feature_Redesign Flow
        Rebalance_Traffic_Distribution = 26, // Req Rebalance_Traffic_Distribution Flow
        License_Management = 27, // Req License_Management Flow
        Add_Node = 28, // Req Add_Node Flow
        Initial_Network_Deployment = 29, // Req Initial_Network_Deployment Flow
        Emergency_Calls_over_IMS = 30, // Req Emergency_Calls_over_IMS Flow
        New_Roaming_Service = 31, // Req New_Roaming_Service Flow
        New_SIP_Interconnect = 32, // Req New_SIP_Interconnect Flow
        Ancillary_HW_Component_Upgrade = 33, // Req Ancillary_HW_Component_Upgrade Flow

        AddNewCluster = 34, // Req ClusterLevel PA Flow
        Add_Remove_Application_from_Cluster = 35, // Req ClusterLevel PA  Flow
        Upgrade_HardwareTypes = 36, // Req ClusterLevel PA  Flow
        Service_Planned = 37, // Req Service planned Flow
        No_PlannedActivity_For_DA = 38, // Req No PlannedActivity For DA Flow
    }
}
