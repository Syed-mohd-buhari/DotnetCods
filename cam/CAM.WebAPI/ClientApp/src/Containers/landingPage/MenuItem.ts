import { useAuth } from "../../Hook/useAuth";
import logo from "./skins/logo___transparent_1.png";
import "./landingPageHeader.css";

export {};
export const productAndDesignMenus = [
  {
    title: "Major Hardware Build",
    path: "/majorhardware",
    category: "Product Library",
  },
  {
    title: "Major Software Build",
    path: "/majorsoftware",
    category: "Product Library",
  },

  {
    title: "Component SW Build",
    path: "/componentswbuild",
    category: "Product Library",
  },

  { title: "System Type", path: "/systemtype", category: "Product Library" },

  {
    title: "System Verification Problems",
    path: "/systemverificationproblems",
    category: "Product Library",
  },

  {
    title: "NFVI Software Compatibility",
    path: "/nfvisoftwarecompatibility",
    category: "Product Library",
  },
  // {
  //   title: "BPT POC",
  //   path: "/bptpoc",
  //   category: "Product Library",
  // },
  {
    title: "Design Component",
    path: "/designcomponent",
    category: "Design Library",
  },

  {
    title: "Design Component Family",
    path: "/designcomponentfamily",
    category: "Design Library",
  },

  {
    title: "Subnetwork Boundary",
    path: "/subnetwork",
    category: "Design Library",
  },
];

export const useNetworkPlans = () => {
  const { admin, readonly, simpleUser } = useAuth();

  const networkPlans = [
    {
      title: "LCM Engineering",
      path: "/lcmengineering",
      category: "Network Plan",
    },
    {
      title: "Design Aspects",
      path: "/designAspect",
      category: "Network Plan",
    },
    {
      title: "Service Info",
      path: "/servicelevel",
      category: "Network Plan",
    },
    {
      title: "Assets",
      path: "/asplanned",
      category: "Network Plan",
    },
    {
      title: "Identities",
      path: "/Identities",
      category: "Network Plan",
    },
    {
      title: "Software Components Bag",
      path: "/componentswbag",
      category: "Bag Configuration",
    },

    {
      title: "Network Element",
      path: "/feedbackloop/networkelement",
      category: "Discovered Network Data",
    },
    {
      title: "Discovered Identities",
      path: "/feedbackloop/identity",
      category: "Discovered Network Data",
    },
    {
      title: "Hardware Configurations",
      path: "/feedbackloop/hardwareconfiguration",
      category: "Discovered Network Data",
    },
    {
      title: "Software Configurations",
      path: "/feedbackloop/softwareconfigurations",
      category: "Discovered Network Data",
    },
    {
      title: "Software Components",
      path: "/feedbackloop/softwarecomponent",
      category: "Discovered Network Data",
    },
    {
      title: "CNIS INFO",
      path: "/omc",
      category: "Discovered Network Data",
      bracket: "",
    },
    {
      title: "Network Element (As-Is)",
      path: "/asis",
      category: "Feedback Loop",
      bracket: "",
    },
    {
      title: "VBOM",
      path: "/virtualbom",
      category: "Infrastructure Management",
      bracket: "",
    },
    {
      title: "CBOM",
      path: "/cbom",
      category: "Infrastructure Management",
      bracket: "",
    },
    {
      title: "Cluster Info",
      path: "/clusterinfo",
      category: "Infrastructure Management",
      bracket: "",
    },
    {
      title: "Passthrough Data",
      path: "/passthroughdata",
      category: "Passthrough",
      bracket: "",
    },
  ];

  return networkPlans;
};

export default useNetworkPlans;
/////////////////////////////

const useManageNetworkPlans = () => {
  const { admin, readonly, simpleUser } = useAuth();

  const managenetworkPlans = [
    {
      title: "LCM",
      path: "/lcmengineering",
      category: "Create Planned Activity",
    },
    {
      title: "Assets",
      path: "/asplanned",
      category: "Create Planned Activity",
    },
    {
      title: "Design Aspects",
      path: "/designAspect",
      category: "Create Planned Activity",
    },
    {
      title: "Service Info",
      path: "/servicelevel",
      category: "Create Planned Activity",
    },

    {
      title: "LCM - PA",
      path: "/plannedActivities/LCM",
      category: "Manage Planned Activities (PA)",
    },
    {
      title: "Assets - PA",
      path: "/plannedActivities/Asset",
      category: "Manage Planned Activities (PA)",
    },
    {
      title: "Design Aspects - PA",
      path: "/plannedActivities/DesignAspect",
      category: "Manage Planned Activities (PA)",
    },
    {
      title: "Service Level - PA",
      path: "/plannedActivities/ServiceLevel",
      category: "Manage Planned Activities (PA)",
    },
  ];

  return managenetworkPlans;
};

export { useManageNetworkPlans };
///////////////////////////////////////

const useReports = () => {
  const { admin, readonly, simpleUser, KPIAdmin, KPIEditor } = useAuth();

  const reports = [
    {
      title: "LCM Export",
      path: "/generatelcmdb",
      category: "LCM Export",
    },
    {
      title: "Disaggregated Reports",
      path: "/generatelcmdbR10",
      category: "LCM Export",
    },

    {
      title: "VAI Export",
      path: "/vaiexport",
      category: "LCM Export",
    },
    {
      title: "Planned Activity Tracker",
      path: "/plannedActivityTracker",
      category: "LCM Export",
    },
    {
      title: "TSR Report",
      path: "/tsrreport",
      category: "LCM Export",
    },
    {
      title: "FNT Report",
      path: "/fntreport",
      category: "LCM Export",
    },

    // {
    //   title: "Generate VoLTE Dashboard",
    //   path: "/generatevoltedashboard",
    //   disable: !admin && true,
    // },

    //{ title: "", path: "", disable: simpleUser && !admin ? true : false },
    // { title: "", path: "", disable: simpleUser && !admin ? true : false },
    // {
    //   title: "",
    //   path: "",
    //   disable: !admin || KPIAdmin ? true : false,
    // },
    // {
    //   title: "",
    //   path: "",
    //   disable: !admin || KPIAdmin || KPIEditor ? true : false,
    // },

    {
      title: "LCM @Glance",
      path: "/lcmatglance",
      category: "Graphical Reports",
    },
    {
      title: "Exodus @Glance (Under Development)",
      path: "/exodusatglance",
      category: "Graphical Reports",
    },
    {
      title: "Exodus-Asset Timeline (Under Development)",
      path: "/exodusassettimeline",
      category: "Graphical Reports",
    },
    {
      title: "Asset Overview by Market",
      path: "/assetoverviewbymarket",
      category: "Graphical Reports",
    },
    {
      title: "Network Visualizer (Under Development)",
      path: "/networkvisualizer",
      category: "Graphical Reports",
    },

    // {
    //   title: "LCM @Glance - V1",
    //   path: "/lcmatglance/v1",
    //   disable: admin ? false : true,
    // },

    {
      title: "PA Report",
      path: "/plannedactivityreport",
      category: "Graphical Reports",
    },
    {
      title: "Compatibility @Glance",
      path: "/nfvicreport",
      category: "Graphical Reports",
    },

    {
      title: "Asset Pivot By Location",
      path: "/assetpivotbylocation",
      category: "Grid Reports",
    },
    {
      title: "All - PA",
      path: "/plannedActivities/All",
      category: "Grid Reports",
    },
    {
      title: "User Defined Reports",
      path: "/genericreports",
      category: "Grid Reports",
    },
    {
      title: "BPT Report",
      path: "/bptreport",
      category: "Budget Planning",
    },
    {
      title: "Exodus Report",
      path: "/exodusreport",
      category: "Infrastructure Report",
    },

    // {
    //   title: KPIAdmin ? "VoLTE KPI Worklog & Approval" : "",
    //   path: "/targetmonthlyapprovals",
    //   category: KPIAdmin ? "Demand Tracking" : "",
    //   disable: KPIAdmin ? false : true,
    // },
    // {
    //   title: KPIAdmin || KPIEditor || readonly ? "VoLTE KPI Dashboard" : "",
    //   path: "/dashboard",
    //   category:
    //     !KPIAdmin && (KPIEditor || readonly) ? "Demand Tracking" : "",
    // },
    {
      title: "VoLTE KPI Worklog & Approval",
      path: "/targetmonthlyapprovals",
      category: "Demand Tracking",
    },
    {
      title: "VoLTE KPI Dashboard",
      path: "/dashboard",
      category: "Demand Tracking",
    },
    {
      title: "Generate VoLTE Dashboard",
      path: "/generatevoltedashboard",
      // disable: !admin || KPIAdmin  && true,
      category: "Demand Tracking",
    },
    {
      title: "VBOM Report",
      path: "/virtualbomreport",
      category: "Infrastructure Report",
      bracket: "",
    },
    {
      title: "CBOM Report",
      path: "/cbomreport",
      category: "Infrastructure Report",
      bracket: "",
    },
  ];

  return reports;
};

export { useReports };
///////////////////////////////////////

const useManageTransformation = () => {
  const { admin, readonly, simpleUser, KPIAdmin, KPIEditor } = useAuth();

  const managetranformation = [
    {
      title: "Initialize New Product",
      path: "setIsVisibleModalInitializeNewProduct(true)",
      category: readonly ? "" : "Application Utilities",
    },
    {
      title: "Manage Migrations",
      path: "setIsVisibleModalManage(true)",
      category: readonly ? "" : "Application Utilities",
    },

    {
      title: "Update Planned Activity Status",
      path: "setIsVisibleModalStatus(true)",
      category: readonly ? "" : "Application Utilities",
    },
    {
      title: "Reconciliation",
      path: "/reconciliation",
      category: "Integration Utilities",
    },

    {
      title: "Delivery Tracking",
      path: "/deliveryTracking",
      category: "Integration Utilities",
    },
    {
      title: "Product Lifecycle Constraints",
      path: "setIsVisibleModalProductLifecycle(true)",
      category: "Product Lifecycle Constraints",
    },
  ];

  return managetranformation;
};

export { useManageTransformation };
////////////////////////////////////////////////////////
const useAdministration = () => {
  const { admin, readonly, simpleUser, KPIAdmin, KPIEditor } = useAuth();

  const administration = [
    {
      title: "Settings Update Planned Activity",
      path: "/settingsupdateplannedactivity",
      category: "Application Settings",
    },
    {
      title: "Configure Planned Activity Type",
      path: "/plannedactivitytype",
      category: "Application Settings",
    },
    {
      title: "SW App - VF Name",
      path: "/vodafoneName",
      category: "Application Settings",
    },
    {
      title: "General Settings",
      path: "/generalsettings",
      category: "Application Settings",
    },
    // {
    //   title: "General Settings", path: "/generalsettings", category: "Application Settings",
    // },
    // { title: "Glossary", path: "/glossary" },
    // {
    //   title: "Generic Report Definition",
    //   path: "/genericreporting",
    //   disable: !admin && true,
    // },

    // { title: "Refactor", path: "ResetForeignIndexConfirm()" },
    {
      title: "Resource Key Master",
      path: "/resourcekeymaster",
      category: "Resource Lifecycle",
    },
    {
      title: "DCF Life Cycle",
      path: "/dcflifecycle",
      category: "Resource Lifecycle",
    },

    {
      title: "Log Management",
      path: "/userLogLevel",
      category: "Application Management",
    },
    {
      title: "User Management",
      path: "/usermanagement",
      category: "Application Management",
    },
    // {
    //   title: "Organization Info",
    //   path: "/organizationinfo",
    //   category: "Application Management",
    // },

    {
      title: "Worklog & Approval",
      path: "/feedbackloop/audit",
      category: "Discovered Data",
    },
    {
      title: "Asset Mapping",
      path: "/assetmapinfo",
      category: "Discovered Data",
    },
  ];

  return administration;
};

export { useAdministration };

const useCustomisePage = () => {
  const { admin, readonly, simpleUser, KPIAdmin, KPIEditor } = useAuth();

  const customisepage = [
    {
      title: "Customize Page Size",
      path: "/customisedpagesize",
      category: "General Settings",
    },
    // Add more items as needed...
  ];

  return customisepage;
};

export { useCustomisePage };
