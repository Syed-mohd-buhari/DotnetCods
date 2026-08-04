import React, { useState } from "react";
import { LineChart } from "@mui/x-charts/LineChart";
import { BarChart } from "@mui/x-charts/BarChart";
import { PieChart } from "@mui/x-charts/PieChart";
import { Gauge, gaugeClasses } from "@mui/x-charts/Gauge";
import {
  AiOutlineAppstore,
  AiOutlineSetting,
  AiOutlineCluster,
  AiOutlineSafety,
  AiOutlineSchedule,
  AiOutlineDeploymentUnit,
  AiOutlinePlusCircle,
  AiOutlineSearch,
} from "react-icons/ai";
import "./NewLandingPage.css";
import { useNavigate } from "react-router-dom";
import { MdCategory } from "react-icons/md";

const NewLandingPage: React.FC = () => {
  const navigate = useNavigate();
  // Line chart data
  const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun"];
  const networkData = [2500, 2800, 5000, 2000, 2200, 2800];
  const capacityData = [2200, 2500, 10000, 4800, 2300, 2100];
  const planningData = [2000, 2200, 2300, 1800, 1900, 2000];

  // Bar chart data
  const products = ["OEM1", "OEM2", "OEM3", "OEM4", "OEM5"];
  const productValues = [5, 10, 15, 20, 25];

  // Pie chart data
  const pieData = [
    { id: 0, value: 45, label: "Compliant", color: "#c41e3a" },
    { id: 1, value: 25, label: "SwUpgradePlanOk", color: "#888888" },
    { id: 2, value: 15, label: "Decommissioning", color: "#cccccc" },
    { id: 3, value: 15, label: "SwUpgradePlanNotOk", color: "#555555" },
  ];

  const quickLinks = [
    {
      icon: <MdCategory className="quick-link-icon" />,
      text: "Initialize New\nProduct",
      path: "/setIsVisibleModalInitializeNewProduct(true)",
    },
    {
      icon: <AiOutlineSchedule className="quick-link-icon" />,
      text: "LCM Engineering",
      path: "/lcmengineering",
    },
    {
      icon: <AiOutlineCluster className="quick-link-icon" />,
      text: "Design Aspects",
      path: "/designAspect",
    },
    {
      icon: <AiOutlineSetting className="quick-link-icon" />,
      text: "Update PA Status",
      path: "/setIsVisibleModalStatus(true)",
    },
    {
      text: "Manage Migrations",
      path: "/setIsVisibleModalManage(true)",
    },
    {
      text: "Major Hardware",
      path: "/majorhardware",
    },
    {
      text: "Major Software",
      path: "/majorsoftware",
    },
    {
      text: "System Type",
      path: "/systemtype",
    },
    {
      text: "Assets",
      path: "/asplanned",
    },
    {
      text: "LCM-PA",
      path: "/plannedActivities/LCM",
    },
    {
      text: "Asset-PA",
      path: "/plannedActivities/Asset",
    },
    {
      text: "Design Aspects-PA",
      path: "/plannedActivities/designAspect",
    },
    {
      text: "Planned Activities",
      path: "/plannedActivities",
    },
    {
      text: "Component SW Build",
      path: "/componentswbuild",
    },
    {
      text: "System Verification\nProblem",
      path: "/systemverificationproblems",
    },
    {
      text: "VBOM",
      path: "/virtualbom",
    },
    {
      text: "CBOM",
      path: "/cbom",
    },
    {
      text: "Cluster Info",
      path: "/clusterinfo",
    },
    {
      text: "NFVI Software\nCompatibility",
      path: "/nfvisoftwarecompatibility",
    },
    {
      text: "Passthrough Data",
      path: "/passthroughdata",
    },
    {
      text: "Design Component",
      path: "/designcomponent",
    },
    {
      text: "Design Component Family",
      path: "/designcomponentfamily",
    },
    {
      text: "Subnetwork Boundary",
      path: "/subnetwork",
    },
    {
      text: "Network Element\n(As-Is)",
      path: "/asis",
    },
    {
      text: "CNIS INFO",
      path: "/omc",
    },
    {
      text: "Identities",
      path: "/identities",
    },
    {
      text: "Software Components Bag",
      path: "/componentswbag",
    },
    {
      text: "Network Element",
      path: "/feedbackloop/networkelement",
    },
    {
      text: "Discovered Identities",
      path: "/feedbackloop/identity",
    },
    {
      text: "Hardware Configurations",
      path: "/feedbackloop/hardwareconfiguration",
    },
    {
      text: "Software Configurations",
      path: "/feedbackloop/softwareconfigurations",
    },
    {
      text: "Software Components",
      path: "/feedbackloop/softwarecomponent",
    },
    {
      text: "Reconciliation",
      path: "/reconciliation",
    },
    {
      text: "Delivery Tracking",
      path: "/deliveryTracking",
    },
    {
      text: "Product Lifecycle\nConstraints",
      path: "/setIsVisibleModalProductLifecycle(true)",
    },
    {
      text: "LCM Export",
      path: "/generatelcmdb",
    },
    {
      text: "Disaggregated Reports",
      path: "/generatelcmdbR10",
    },
    {
      text: "VAI Export",
      path: "/vaiexport",
    },
    {
      text: "Planned Activity\nTracker",
      path: "/plannedActivityTracker",
    },
    {
      text: "TSR Report",
      path: "/tsrreport",
    },
    {
      text: "FNT Report",
      path: "/fntreport",
    },
    {
      text: "LCM @Glance",
      path: "/lcmatglance",
    },
    {
      text: "Exodus @Glance (Under Development)",
      path: "/exodusatglance",
    },
    {
      text: "Exodus-Asset Timeline (Under Development)",
      path: "/exodusassettimeline",
    },
    {
      text: "Network Visualizer\n(Under Development)",
      path: "/networkvisualizer",
    },
    {
      text: "PA Report",
      path: "/plannedactivityreport",
    },
    {
      text: "Compatibility @Glance",
      path: "/nfvicreport",
    },
    {
      text: "Asset Overview\nby Market",
      path: "/assetoverviewbymarket",
    },
    {
      text: "Asset Pivot\nby Location",
      path: "/assetpivotbylocation",
    },
    {
      text: "All - PA",
      path: "/plannedActivities/All",
    },
    {
      text: "User Defined Reports",
      path: "/genericreports",
    },
    {
      text: "BPT Report",
      path: "/bptreport",
    },
    {
      text: "Exodus Report",
      path: "/exodusreport",
    },
    {
      text: "VBOM Report",
      path: "/virtualvbomreport",
    },
    {
      text: "CBOM Report",
      path: "/cbomreport",
    },
    {
      text: "Generate VoLTE Dashboard",
      path: "/generatevoltedashboard",
    },
  ];
  const [searchQuery, setSearchQuery] = useState("");
  const visibleCount = 12;

  const filteredLinks = quickLinks.filter((link) =>
    link.text.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const displayedLinks = filteredLinks.slice(0, visibleCount);

  const handleAddLink = () => {
    alert("Add new quick link functionality");
  };

  const handleMenuClick = (path: string, newTab: boolean = false) => {
    if (path.includes("/")) {
      if (newTab) {
        window.open(path, "_blank");
      } else {
        navigate(path);
      }
    } else {
      try {
        eval(path);
      } catch (error) {
        console.error("Error executing function:", error);
      }
    }
  };

  return (
    <div className="dashboard-container">
      {/* Quick Links Section */}
      <div className="quick-links-section">
        <header className="main-header">
          <div className="main-quick-title pl-0">Quick Links</div>
        </header>
        <div className="d-flex justify-between">
          <div className="quick-links-search w-100">
            <AiOutlineSearch className="quick-links-search-icon" />
            <input
              type="text"
              className="quick-links-search-input"
              placeholder="Search links..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
          </div>
          <div style={{ paddingLeft: "10px", paddingTop: "5px" }}>
            <button className="quick-links-footer-btn" onClick={handleAddLink}>
              <AiOutlinePlusCircle size={20} />
            </button>
          </div>
        </div>
        <div className="quick-links-grid">
          {displayedLinks.map((link, index) => (
            <div
              key={index}
              className="quick-link-card"
              onClick={(event) => {
                event.preventDefault();
                const newTab = link?.path?.includes("/")
                  ? event.ctrlKey || event.metaKey
                  : false;
                link?.path && handleMenuClick(link?.path, newTab);
              }}
            >
              {link.icon}
              <span className="quick-link-text">{link.text}</span>
            </div>
          ))}
        </div>
        {/* <div className="quick-links-footer">
          <button className="quick-links-footer-btn" onClick={handleAddLink}>
            <AiOutlinePlusCircle size={20} />
          </button>
        </div> */}
      </div>

      {/* Main Content */}
      <main className="main-content">
        <header className="main-header">
          <div className="main-title pl-0">WHAT'S GOING ON</div>
          <p className="main-subtitle pl-0">
            High-level summaries with Key Metrics & KPIs • Dashboards combining
            multiple views • Charts & Graphs
          </p>
        </header>

        {/* Top Charts */}
        <div className="charts-grid">
          {/* Network Performance Trends */}
          <div className="card">
            <h3 className="card-title">Planned Activity Tracker</h3>
            <div className="line-chart-container">
              <LineChart
                xAxis={[
                  {
                    scaleType: "point",
                    data: months,
                  },
                ]}
                yAxis={[
                  {
                    min: 0,
                    max: 10000,
                  },
                ]}
                series={[
                  {
                    data: networkData,
                    color: "#c41e3a",
                    showMark: true,
                  },
                  {
                    data: capacityData,
                    color: "#333333",
                    showMark: true,
                  },
                  {
                    data: planningData,
                    color: "#666666",
                    showMark: true,
                  },
                ]}
                height={220}
                grid={{ horizontal: true }}
              />
            </div>
            <div className="chart-legend">
              <div className="legend-item">
                <span className="legend-dot network"></span>
                <span>network</span>
              </div>
              <div className="legend-item">
                <span className="legend-dot capacity"></span>
                <span>capacity</span>
              </div>
              <div className="legend-item">
                <span className="legend-dot planning"></span>
                <span>planning</span>
              </div>
            </div>
          </div>

          {/* Product Distribution */}
          <div className="card">
            <h3 className="card-title">Asset Count</h3>
            <div className="bar-chart-container">
              <BarChart
                xAxis={[
                  {
                    scaleType: "band",
                    data: products,
                  },
                ]}
                yAxis={[
                  {
                    min: 0,
                    max: 30,
                  },
                ]}
                series={[
                  {
                    data: productValues,
                    color: "#c41e3a",
                  },
                ]}
                height={260}
                grid={{ horizontal: true }}
              />
            </div>
          </div>
        </div>

        {/* Bottom Charts */}
        <div className="bottom-charts-grid">
          {/* System Health Score */}
          <div className="card">
            <h3 className="card-title">LCM Compliance</h3>
            <div className="gauge-container">
              <Gauge
                value={72}
                startAngle={-90}
                endAngle={90}
                width={200}
                height={120}
                cornerRadius="50%"
                sx={{
                  [`& .${gaugeClasses.valueText}`]: {
                    fontSize: 36,
                    fontWeight: "bold",
                    transform: "translate(0px, -10px)",
                  },
                  [`& .${gaugeClasses.valueArc}`]: {
                    fill: "#c41e3a",
                  },
                  [`& .${gaugeClasses.referenceArc}`]: {
                    fill: "#e0e0e0",
                  },
                }}
                text={({ value }) => `${value}%`}
              />
              <span className="gauge-label">Overall Performance</span>
              <div className="gauge-legend">
                <div className="gauge-legend-item">
                  <span className="gauge-legend-dot active"></span>
                  <span>Active</span>
                </div>
                <div className="gauge-legend-item">
                  <span className="gauge-legend-dot remaining"></span>
                  <span>Remaining</span>
                </div>
              </div>
            </div>
          </div>
          {/* Task Status Overview */}
          <div className="card">
            <h3 className="card-title">VNF Compliance</h3>
            <div className="pie-chart-container">
              <PieChart
                series={[
                  {
                    data: pieData,
                    innerRadius: 30,
                    outerRadius: 100,
                    paddingAngle: 2,
                    cornerRadius: 6,
                    startAngle: -42,
                    endAngle: 330,
                    cx: 148,
                    cy: 135,
                  },
                ]}
              />
            </div>
          </div>
        </div>

        {/* Market Insights */}
        <div className="market-insights">
          <h3 className="insights-title">Feedback Loop</h3>
          <div className="insights-grid">
            <div className="insight-card">
              <p className="insight-value">25/01/2026</p>
              <p className="insight-label">Last Run</p>
            </div>
            <div className="insight-card">
              <p className="insight-value">13/06/2025</p>
              <p className="insight-label">OMC Updated Date</p>
            </div>
            <div className="insight-card">
              <p className="insight-value green">29/11/2025</p>
              <p className="insight-label">Export to Viddle</p>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
};

export default NewLandingPage;
