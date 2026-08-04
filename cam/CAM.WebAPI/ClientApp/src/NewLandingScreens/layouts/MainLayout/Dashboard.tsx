import React, { useState, useCallback, memo } from "react";
import { Box } from "@mui/material";
import Sidebar from "./Sidebar";
import ProductSoftwareContainer from "../../pages/ProductSoftware/ProductSoftwareContainer";
import MainContent from "./MainContent";
import Header from "./Header";
import ProductHardwareContainer from "../../pages/ProductHardware/ProductHardwareContainer";

type Page = "dashboard" | "softwareProduct" | "hardwareProduct";

const MemoMainContent = memo(MainContent);

const Dashboard = (props: any) => {
  const [activePage, setActivePage] = useState<Page>("dashboard");
  const [majorId, setMajorId] = useState<any>(null);
  const [currentRole, setCurrentRole] = useState<string>(props.role);

  const handleSoftwareRedirect = useCallback((id: any) => {
    setActivePage("softwareProduct");
    setMajorId(id);
  }, []);

  const handleHardwareRedirect = useCallback((id: any) => {
    setActivePage("hardwareProduct");
    setMajorId(id);
  }, []);

  const handleNavigate = useCallback((page: string) => {
    setActivePage(page as Page);
    setMajorId(null);
  }, []);

  const handleRoleChange = useCallback((newRole: string) => {
    setCurrentRole((prev) => {
      if (prev === newRole) return prev;
      setActivePage("dashboard");
      setMajorId(null);
      return newRole;
    });
  }, []);

  return (
    <>
      <Box
        sx={{
          width: "100vw",
          height: "100vh",
          overflow: "hidden",
        }}
      >
        <Header role={currentRole} onNavigate={handleNavigate} />
        <Box
          sx={{
            position: "fixed",
            top: "92px",
            left: 0,
            right: 0,
            bottom: 0,
            display: "flex",
          }}
        >
          <Sidebar
            activeNav={activePage}
            role={currentRole}
            onNavigate={handleNavigate}
            onRoleChange={handleRoleChange}
          />
          <Box
            sx={{
              flex: 1,
              ml: "273px",
              overflow: "auto",
              background: "#F9FAFC",
              zIndex: 1,
            }}
          >
            {activePage === "softwareProduct" && (
              <ProductSoftwareContainer majorId={majorId} />
            )}
            {activePage === "hardwareProduct" && (
              <ProductHardwareContainer majorId={majorId} />
            )}
            {activePage === "dashboard" && (
              <MemoMainContent
                role={currentRole}
                activePage={activePage}
                onNavigate={handleNavigate}
                handleSoftwareRedirect={handleSoftwareRedirect}
                handleHardwareRedirect={handleHardwareRedirect}
              />
            )}
          </Box>
        </Box>
      </Box>
    </>
  );
};

export default Dashboard;
