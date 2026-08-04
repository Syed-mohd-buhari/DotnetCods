import React, { useState, useRef, useContext, useEffect } from "react";
import { Box, Divider, Typography } from "@mui/material";
import { styled } from "@mui/material/styles";
import { useLocation, useNavigate } from "react-router-dom";
import {
  FiChevronRight,
  FiChevronDown,
  FiRefreshCw,
  FiCheck,
} from "react-icons/fi";
import { RootState } from "../../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
import switchIcon from "../../../img/tabler_switch-3.jpg";
import { DataModalConfirm, stateConfirm } from "../../../Model/Common";
import { OptionContext } from "../../../Context/MenuOptionContext";
import { removeAccessToken } from "../../../Redux/Action/AuthenticationAction";
import ModalConfirm from "../../../Components/ModalConfirm";
import newSwitchIcon from "../../../img/new_tabler_switch-3.png";
import { useAuth } from "../../../Hook/useAuth";

const SidebarWrapper = styled(Box)({
  display: "flex",
  flexDirection: "column",
  justifyContent: "space-between",
  alignItems: "center",
  padding: "24px 0",
  width: "273px",
  height: "calc(100vh - 92px)",
  background: "#F5F7FA",
  flexShrink: 0,
  position: "fixed",
  left: 0,
  zIndex: 100,
  overflowY: "auto",
});

const NavSection = styled(Box)({
  display: "flex",
  flexDirection: "column",
  gap: "4px",
  width: "233px",
});

const NavItem = styled(Box, {
  shouldForwardProp: (prop) => prop !== "active",
})<{ active?: boolean }>(({ active }) => ({
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  justifyContent: "space-between",
  padding: "0 12px",
  width: "233px",
  height: "48px",
  background: active ? "#FFFFFF" : "transparent",
  borderRadius: "5px",
  cursor: "pointer",
  position: "relative",
  "&:hover": { background: active ? "#FFFFFF" : "#ECEEF2" },
}));

const NavLabel = styled(Typography, {
  shouldForwardProp: (prop) => prop !== "active",
})<{ active?: boolean }>(({ active }) => ({
  fontWeight: active ? 700 : 400,
  fontSize: "16px",
  lineHeight: "32px",
  color: "#0D0D0D",
}));

const RedDot = styled(Box)({
  width: "8px",
  height: "8px",
  borderRadius: "50%",
  background: "#E60000",
  flexShrink: 0,
  marginLeft: "6px",
});

const FlyoutMenu = styled(Box)({
  position: "fixed",
  background: "#FFFFFF",
  borderRadius: "8px",
  boxShadow: "0px 4px 20px rgba(0,0,0,0.12)",
  minWidth: "200px",
  zIndex: 500,
  padding: "8px 0",
  overflow: "hidden",
});

const FlyoutItem = styled(Box)({
  padding: "10px 20px",
  cursor: "pointer",

  fontSize: "15px",
  color: "#0D0D0D",
  display: "flex",
  alignItems: "center",
  gap: "8px",
  "&:hover": { background: "#F5F7FA" },
});

const Avatar = styled(Box)({
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
  width: "40px",
  height: "40px",
  background: "#FFECC0",
  borderRadius: "9999px",
  flexShrink: 0,
  fontSize: "14px",
  fontWeight: 700,
  color: "#0D0D0D",
});

const UserMenu = styled(Box)({
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  padding: "12px 16px",
  gap: "12px",
  width: "253px",
  borderRadius: "10px",
  cursor: "pointer",
  "&:hover": { background: "#ECEEF2" },
});

const RoleSwitcher = styled(Box)({
  display: "inline-flex",
  flexDirection: "row",
  alignItems: "center",
  gap: "4px",
  cursor: "pointer",
  width: "fit-content",
  padding: "2px 8px",
  borderRadius: "12px",
  border: "1px solid #00000033",
  "&:hover": { background: "#F5F7FA" },
});

const SwitchButton = styled(Box)({
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  justifyContent: "center",
  padding: "5.5px 10px",
  gap: "8px",
  width: "233px",
  height: "32px",
  background: "#FFFFFF",
  borderRadius: "8px",
  boxShadow: "inset 0px 0px 0px 1px #E1E4EA",
  cursor: "pointer",
  "&:hover": { background: "#F5F7FA" },
});

interface SidebarProps {
  activeNav: string;
  onNavigate: (page: string) => void;
  role: string;
  onRoleChange?: (role: string) => void;
}

const Sidebar: React.FC<SidebarProps> = ({
  activeNav,
  onNavigate,
  role,
  onRoleChange,
}) => {
  const location = useLocation();
  const navigate = useNavigate();
  const [productOpen, setProductOpen] = useState(false);
  const [reportingOpen, setReportingOpen] = useState(false);
  const [helpOpen, setHelpOpen] = useState(false);
  const [profileOpen, setProfileOpen] = useState(false);
  const [roleMenuOpen, setRoleMenuOpen] = useState(false);
  const productRef = useRef<HTMLDivElement>(null);
  const reportingRef = useRef<HTMLDivElement>(null);
  const helpRef = useRef<HTMLDivElement>(null);
  const profileRef = useRef<HTMLDivElement>(null);
  const roleRef = useRef<HTMLDivElement>(null);
  const userInfo = useSelector(
    (state: RootState) => state.autenticazione.aadResponse
  );
  const { role: roleList } = useAuth();
  const SWITCHABLE_ROLES = ["SW Product Owner", "HW Product Owner"];
  const filteredRoleList = (roleList ?? []).filter((r: string) =>
    SWITCHABLE_ROLES.includes(r)
  );
  const [selectedRole, setSelectedRole] = useState<string>(
    role ?? roleList?.[0] ?? ""
  );

  useEffect(() => {
    if (role) setSelectedRole(role);
  }, [role]);

  const { selectedOption, setSelectedOption } = useContext(OptionContext);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const CancelConfirm = () => {
    setConfirm(stateConfirm);
  };
  const goToHomepage = (clearMenu) => {
    if (location.pathname !== "/home" && clearMenu) {
      navigate("/");
    }
    if (clearMenu) setSelectedOption("");
  };
  const onLogout = () => {
    navigate("/");
    if (localStorage.getItem("TYPE") === "LOC") {
      goToHomepage(false);
    } else {
      goToHomepage(false);
    }
    removeAccessToken();
  };
  const getPos = (ref: React.RefObject<HTMLDivElement>) => {
    if (!ref.current) return { top: 0, left: 200 };
    const rect = ref.current.getBoundingClientRect();
    return { top: rect.top, left: rect.right + 2 };
  };

  const getPosAbove = (ref: React.RefObject<HTMLDivElement>) => {
    if (!ref.current) return { bottom: 0, left: 200 };
    const rect = ref.current.getBoundingClientRect();
    return {
      bottom: window.innerHeight - rect.top + 6,
      left: rect.left,
    };
  };

  const navItems = ["dashboard", "Design", "Network Planning", "Delivery"];
  const getInitials = (email: string) => {
    const username = email?.split("@")[0];
    const parts = username?.split(".");

    return (
      parts?.[0][0]?.toUpperCase() +
      (parts?.length > 1 ? parts?.[1][0]?.toUpperCase() : "")
    );
  };
  const LogOutConfirm = () => {
    setConfirm({
      title: "Confirm",
      message: "Are you sure you want to quit? Unsaved changes will be lost.",
      button: "Logout",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => CancelConfirm(),
        confirm: () => {
          setSelectedOption("");
          onLogout();
        },
      },
    });
  };

  const handleRoleSelect = (newRole: string) => {
    setSelectedRole(newRole);
    setRoleMenuOpen(false);
    onRoleChange?.(newRole);
  };

  return (
    <SidebarWrapper>
      <NavSection>
        {/* <NavItem
          active={activeNav === "dashboard"}
          onClick={() => onNavigate("dashboard")}
        >
          <NavLabel active={activeNav === "dashboard"}>Dashboard</NavLabel>
        </NavItem> */}
        <NavItem>
          <NavLabel>View All Portal</NavLabel>
        </NavItem>
        <Box
          ref={productRef}
          onMouseEnter={() => setProductOpen(true)}
          onMouseLeave={() => setProductOpen(false)}
          sx={{ position: "relative" }}
        >
          <NavItem
            active={
              activeNav === "softwareProduct" || activeNav === "hardwareProduct"
            }
          >
            <Box sx={{ display: "flex", alignItems: "center" }}>
              <NavLabel>Product</NavLabel>
            </Box>
            <FiChevronRight style={{ color: "#0D0D0D", fontSize: "16px" }} />
          </NavItem>

          {productOpen && (
            <FlyoutMenu
              style={{
                top: getPos(productRef).top,
                left: getPos(productRef).left,
              }}
            >
              {selectedRole === "SW Product Owner" && (
                <FlyoutItem onClick={() => onNavigate("softwareProduct")}>
                  Software
                </FlyoutItem>
              )}
              {selectedRole === "HW Product Owner" && (
                <FlyoutItem onClick={() => onNavigate("hardwareProduct")}>
                  Hardware
                </FlyoutItem>
              )}
            </FlyoutMenu>
          )}
        </Box>

        <Box
          ref={reportingRef}
          onMouseEnter={() => setReportingOpen(true)}
          onMouseLeave={() => setReportingOpen(false)}
          sx={{ position: "relative" }}
        >
          <NavItem active={activeNav === "userReport"}>
            <Box sx={{ display: "flex", alignItems: "center" }}>
              <NavLabel>Reporting</NavLabel>
            </Box>
            <FiChevronRight style={{ color: "#0D0D0D", fontSize: "16px" }} />
          </NavItem>

          {reportingOpen && (
            <FlyoutMenu
              style={{
                top: getPos(reportingRef).top,
                left: getPos(reportingRef).left,
              }}
            >
              <FlyoutItem onClick={() => navigate("/genericreporting")}>
                User Defined Report
              </FlyoutItem>
            </FlyoutMenu>
          )}
        </Box>

        {/* {["Design", "Network Planning", "Delivery"].map((item) => (
          <NavItem key={item} active={activeNav === item}>
            <NavLabel active={activeNav === item}>{item}</NavLabel>
          </NavItem>
        ))} */}

        {/* <NavItem>
          <NavLabel>Help</NavLabel>
          <FiChevronRight style={{ color: "#0D0D0D", fontSize: "16px" }} />
        </NavItem> */}
      </NavSection>
      <ModalConfirm data={confirm} />

      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          gap: "12px",
          width: "100%",
        }}
      >
        <Box
          ref={helpRef}
          onMouseEnter={() => setHelpOpen(true)}
          onMouseLeave={() => setHelpOpen(false)}
          sx={{ position: "relative", width: "233px", paddingTop: "12px" }}
        >
          <NavItem active={activeNav === "glossary"}>
            <Box sx={{ display: "flex", alignItems: "center" }}>
              <NavLabel>Help</NavLabel>
            </Box>
            <FiChevronRight style={{ color: "#0D0D0D", fontSize: "16px" }} />
          </NavItem>

          {helpOpen && (
            <FlyoutMenu
              style={{
                top: getPos(helpRef).top,
                left: getPos(helpRef).left,
              }}
            >
              <FlyoutItem onClick={() => navigate("/glossary")}>
                Glossary
              </FlyoutItem>
            </FlyoutMenu>
          )}
        </Box>
        <Box
          ref={profileRef}
          onMouseEnter={() => setProfileOpen(true)}
          onMouseLeave={() => setProfileOpen(false)}
          sx={{
            position: "relative",
            width: "100%",
            paddingTop: "12px",
            borderTop: "1px solid rgba(144.52, 144.52, 144.52, 0.50)",
          }}
        >
          <UserMenu>
            <Avatar>
              {userInfo ? getInitials(userInfo?.account.name) : ""}
            </Avatar>
            <Box
              sx={{
                display: "flex",
                flexDirection: "column",
                gap: "4px",
                flex: 1,
                minWidth: 0,
                textAlign: "left",
              }}
            >
              <Typography
                sx={{
                  fontWeight: 400,
                  fontSize: "14px",
                  lineHeight: "20px",
                  color: "#0E121B",
                  whiteSpace: "nowrap",
                  overflow: "hidden",
                  textOverflow: "ellipsis",
                }}
              >
                {userInfo?.account.name
                  .split("@")[0]
                  .trim()
                  .replace(/\./g, " ")
                  .replace(/(^\w|\.\s*\w)/g, function (char) {
                    return char.toUpperCase();
                  })}
              </Typography>

              <Box
                ref={roleRef}
                onClick={(e) => {
                  e.stopPropagation();
                  if (filteredRoleList?.length > 1) {
                    setRoleMenuOpen((o) => !o);
                    setProfileOpen(false);
                  }
                }}
                sx={{ position: "relative" }}
              >
                <RoleSwitcher>
                  <Typography
                    className="role-label"
                    sx={{
                      fontWeight: 400,
                      fontSize: "12px",
                      lineHeight: "16px",
                      color: "#525866",
                      whiteSpace: "nowrap",
                      overflow: "hidden",
                      textOverflow: "ellipsis",
                    }}
                  >
                    {selectedRole}
                  </Typography>
                  {filteredRoleList?.length > 1 && (
                    <FiChevronDown
                      style={{
                        color: "#525866",
                        fontSize: "13px",
                        flexShrink: 0,
                      }}
                    />
                  )}
                </RoleSwitcher>

                {roleMenuOpen && (
                  <FlyoutMenu
                    onMouseLeave={() => setRoleMenuOpen(false)}
                    style={{
                      bottom: getPosAbove(roleRef).bottom,
                      top: "auto",
                      left: getPosAbove(roleRef).left,
                    }}
                  >
                    {filteredRoleList?.map((r: string) => (
                      <FlyoutItem
                        key={r}
                        onClick={(e) => {
                          e.stopPropagation();
                          handleRoleSelect(r);
                        }}
                        sx={{
                          justifyContent: "space-between",
                          fontWeight: r === selectedRole ? 700 : 400,
                        }}
                      >
                        {r}
                        {r === selectedRole && (
                          <FiCheck
                            style={{ fontSize: "14px", color: "#0D0D0D" }}
                          />
                        )}
                      </FlyoutItem>
                    ))}
                  </FlyoutMenu>
                )}
              </Box>
            </Box>
            <FiChevronRight
              style={{ color: "#525866", fontSize: "16px", flexShrink: 0 }}
            />
          </UserMenu>

          {profileOpen && (
            <FlyoutMenu
              style={{
                bottom: "68px",
                top: "auto",
                left: getPos(profileRef).left,
              }}
            >
              {/* <FlyoutItem
                onClick={() => {
                  window.open("/usermanagement", "_blank");
                  // navigate("/usermanagement");
                }}
              >
                Role to Feature Map
              </FlyoutItem>
              <FlyoutItem
                onClick={() => {
                  window.open("/reports", "_blank");
                  // navigate("/reports");
                }}
              >
                Reports
              </FlyoutItem> */}
              <FlyoutItem
                onClick={() => {
                  LogOutConfirm();
                }}
              >
                Logout
              </FlyoutItem>
            </FlyoutMenu>
          )}
        </Box>

        <SwitchButton onClick={() => navigate("/home")}>
          <img src={newSwitchIcon} alt="" />
          <Typography
            sx={{
              fontWeight: 400,
              fontSize: "14px",
              color: "#525866",
            }}
          >
            Switch to Core Tems
          </Typography>
        </SwitchButton>
      </Box>
    </SidebarWrapper>
  );
};

export default Sidebar;
