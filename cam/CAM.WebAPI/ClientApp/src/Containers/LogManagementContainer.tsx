import React, { useState, useEffect, useCallback } from "react";
import "../Css/App.css";
import "../Css/index.css";
import UsersLogLevels from "./users-log-level/usersLogLevel";
import AuditTrailsContainer from "./AuditTrailsContainer";
import { Tab, Tabs } from "react-bootstrap";
import FeedbackLogsContainer from "./FeedbackLogsContainer";
import UserDefinedReportsLogsContainer from "./UserDefinedReportsLogsContainer";

const LogManagement = () => {
  const [keyTabs, setKeyTabs] = useState("ServerLogs");

  return (
    <div className="pageContainer">
      {/*<h3 className="voda-bold headerPage row mx-0">Log Management</h3>*/}
      <Tabs
        defaultActiveKey="Server Logs"
        id="logs"
        activeKey={keyTabs}
        onSelect={(x) => setKeyTabs(x || "")}
      >
        <Tab eventKey="ServerLogs" title="Server Logs">
          {keyTabs === "ServerLogs" && <UsersLogLevels />}
        </Tab>
        <Tab eventKey="FeedbackLogs" title="Feedback Logs">
          {keyTabs === "FeedbackLogs" && <FeedbackLogsContainer />}
        </Tab>
        <Tab eventKey="AuditTrails" title="Audit Trails">
          {keyTabs === "AuditTrails" && <AuditTrailsContainer />}
        </Tab>
        <Tab eventKey="UserDefinedReportLogs" title="User Defined Report Logs">
          {keyTabs === "UserDefinedReportLogs" && (
            <UserDefinedReportsLogsContainer />
          )}
        </Tab>
      </Tabs>
    </div>
  );
};

export default LogManagement;
