import React, { useState, useRef, useEffect, useCallback } from "react";
import {
  Box,
  InputBase,
  Button,
  Typography,
  Tabs,
  Tab,
  CircularProgress,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import { FiSearch, FiPlus, FiChevronUp, FiChevronDown } from "react-icons/fi";
import vfLogo from "../../../img/vfLogo.png";
import { rootStore } from "../../../Redux/Store/rootStore";
import ProductSoftwareModal from "../../pages/ProductSoftware/ProductSoftwareModal";
import ProductHardwareModal from "../../pages/ProductHardware/ProductHardwareModal";

const AiScanLineIcon: React.FC<{ style?: React.CSSProperties }> = ({
  style,
}) => (
  <svg
    width="20"
    height="20"
    viewBox="0 0 24 24"
    fill="none"
    xmlns="http://www.w3.org/2000/svg"
    style={style}
  >
    <path
      d="M2.625 18.5996V17C2.625 16.7929 2.79289 16.625 3 16.625C3.20711 16.625 3.375 16.7929 3.375 17V18.5996C3.375 19.1367 3.58801 19.6525 3.96777 20.0322C4.34753 20.412 4.86333 20.625 5.40039 20.625H7C7.20711 20.625 7.375 20.7929 7.375 21C7.375 21.2071 7.20711 21.375 7 21.375H5.40039C4.66441 21.375 3.95791 21.0829 3.4375 20.5625C2.91709 20.0421 2.625 19.3356 2.625 18.5996ZM20.625 18.5996V17C20.625 16.7929 20.7929 16.625 21 16.625C21.2071 16.625 21.375 16.7929 21.375 17V18.5996C21.375 19.3356 21.0829 20.0421 20.5625 20.5625C20.0421 21.0829 19.3356 21.375 18.5996 21.375H17C16.7929 21.375 16.625 21.2071 16.625 21C16.625 20.7929 16.7929 20.625 17 20.625H18.5996C19.1367 20.625 19.6525 20.412 20.0322 20.0322C20.412 19.6525 20.625 19.1367 20.625 18.5996ZM15 12.625C15.1721 12.625 15.3225 12.7422 15.3643 12.9092L15.709 14.29L17.0908 14.6357C17.2578 14.6775 17.375 14.8279 17.375 15C17.375 15.1721 17.2578 15.3225 17.0908 15.3643L15.709 15.709L15.3643 17.0908C15.3225 17.2578 15.1721 17.375 15 17.375C14.8279 17.375 14.6775 17.2578 14.6357 17.0908L14.29 15.709L12.9092 15.3643C12.7422 15.3225 12.625 15.1721 12.625 15C12.625 14.8279 12.7422 14.6775 12.9092 14.6357L14.29 14.29L14.6357 12.9092C14.7163 12.7146 14.8496 12.625 15 12.625ZM8.44238 6.68457C8.58724 6.1045 9.41276 6.1045 9.55762 6.68457L9.9082 8.09082L11.3154 8.44238C11.8957 8.58665 11.8955 9.41219 11.3145 9.55664L9.9082 9.9082L9.55762 11.3154C9.41276 11.8955 8.58724 11.8955 8.44238 11.3154L8.09082 9.9082L6.68457 9.55762C6.10473 9.41162 6.10457 8.58722 6.68457 8.44238L8.09082 8.09082L8.44238 6.68457ZM2.625 7V5.40039C2.625 4.66441 2.91709 3.95791 3.4375 3.4375C3.95791 2.91709 4.66441 2.625 5.40039 2.625H7C7.20711 2.625 7.375 2.79289 7.375 3C7.375 3.20711 7.20711 3.375 7 3.375H5.40039C4.86333 3.375 4.34753 3.58801 3.96777 3.96777C3.58801 4.34753 3.375 4.86333 3.375 5.40039V7C3.375 7.20711 3.20711 7.375 3 7.375C2.79289 7.375 2.625 7.20711 2.625 7ZM20.625 7V5.40039C20.625 4.86333 20.412 4.34753 20.0322 3.96777C19.6525 3.58801 19.1367 3.375 18.5996 3.375H17C16.7929 3.375 16.625 3.20711 16.625 3C16.625 2.79289 16.7929 2.625 17 2.625H18.5996C19.3356 2.625 20.0421 2.91709 20.5625 3.4375C21.0829 3.95791 21.375 4.66441 21.375 5.40039V7C21.375 7.20711 21.2071 7.375 21 7.375C20.7929 7.375 20.625 7.20711 20.625 7Z"
      fill="currentColor"
    />
  </svg>
);

const SendIcon: React.FC<{ color?: string }> = ({ color = "#B6B6B6" }) => (
  <svg
    width="18"
    height="18"
    viewBox="0 0 24 24"
    fill="none"
    xmlns="http://www.w3.org/2000/svg"
  >
    <path
      d="M22 2L11 13M22 2L15 22L11 13M22 2L2 9L11 13"
      stroke={color}
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    />
  </svg>
);

type SearchTab = "ai" | "software" | "hardware" | "systemtype";
interface AiMessage {
  role: "user" | "assistant";
  content: string;
}

const StyledHeader = styled(Box)({
  boxSizing: "border-box",
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  alignItems: "center",
  padding: "0 45px",
  gap: "48px",
  width: "100%",
  height: "92px",
  background: "#000000",
  position: "fixed",
  top: 0,
  left: 0,
  right: 0,
  zIndex: 300,
});

const LogoBox = styled("img")({
  width: "82px",
  height: "60px",
  objectFit: "contain",
  flexShrink: 0,
  cursor: "pointer",
});

const SearchSlot = styled(Box)({
  flex: 1,
  maxWidth: "620px",
  position: "relative",
  display: "flex",
  alignItems: "center",
  height: "48px",
});

const HeaderSearchBar = styled(Box)({
  boxSizing: "border-box",
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  padding: "8px 16px",
  gap: "8px",
  height: "48px",
  background: "#FFFFFF",
  border: "1px solid #7E7E7E",
  borderRadius: "6px",
  cursor: "text",
  width: "100%",
});

const AiBadge = styled(Box)({
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  justifyContent: "center",
  gap: "4px",
  padding: "2px 10px",
  width: "66px",
  height: "32px",
  border: "1px solid rgba(0,0,0,0.2)",
  borderRadius: "4px",
  flexShrink: 0,
});

const CreateNewButton = styled(Button)({
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  gap: "8px",
  padding: "8px 16px",
  background: "#E60000",
  borderRadius: "10px",
  color: "#ffffff",
  fontSize: "16px",
  fontWeight: 400,
  textTransform: "none",
  whiteSpace: "nowrap",
  flexShrink: 0,
  height: "44px",
  "&:hover": { background: "#CC0000" },
});

const DropdownMenu = styled(Box)({
  position: "absolute",
  top: "calc(100% + 8px)",
  right: 0,
  background: "#FFFFFF",
  borderRadius: "8px",
  boxShadow: "0px 4px 20px rgba(0,0,0,0.15)",
  minWidth: "160px",
  zIndex: 2000,
  overflow: "hidden",
  textAlign: "left",
});

const DropdownItem = styled(Box)({
  padding: "12px 20px",
  cursor: "pointer",
  fontSize: "14px",
  color: "#0D0D0D",
  borderBottom: "1px solid #F0F0F3",
  "&:last-child": { borderBottom: "none" },
  "&:hover": { background: "#F5F7FA" },
});

const StyledTabs = styled(Tabs)({
  borderBottom: "1px solid #E8E8E8",
  minHeight: "44px",
  "& .MuiTabs-indicator": { backgroundColor: "#E60000", height: "2px" },
});

const StyledTab = styled(Tab)({
  fontSize: "14px",
  fontWeight: 400,
  color: "#666666",
  textTransform: "none",
  minHeight: "44px",
  padding: "0",
  marginRight: "24px",
  minWidth: "auto",
  "&.Mui-selected": { color: "#0D0D0D", fontWeight: 700 },
});

const ResultItem = styled(Box)({
  padding: "14px 20px",
  borderBottom: "1px solid #F0F0F3",
  cursor: "pointer",
  fontSize: "14px",
  color: "#0D0D0D",
  "&:last-child": { borderBottom: "none" },
  "&:hover": { background: "#F5F7FA" },
});

const UserBubble = styled(Box)({
  background: "rgba(0, 0, 0, 0.1)",
  borderRadius: "6px",
  padding: "16px",
  width: "fit-content",
  fontSize: "16px",
  lineHeight: "28px",
  color: "#0D0D0D",
  alignItems: "flex-end",
  textAlign: "right" as const,
  boxSizing: "border-box" as const,
});

const AiBubble = styled(Box)({
  background: "rgba(230, 0, 0, 0.1)",
  borderRadius: "6px",
  padding: "16px",
  width: "fit-content",
  fontSize: "16px",
  lineHeight: "28px",
  color: "#0D0D0D",
  display: "flex",
  gap: "7px",
  alignItems: "flex-start",
  textAlign: "left" as const,
  boxSizing: "border-box" as const,
});

const MOCK_RESULTS: Record<SearchTab, string[]> = {
  ai: [],
  software: ["Result 1", "Result 2", "Result 3", "Result 4"],
  hardware: ["HW-01 Siemens 2232", "HW-02 Nokia Router", "HW-03 Access Point"],
  systemtype: ["Type A — Core", "Type B — Edge"],
};

const VodafoneHeader = (props: any) => {
  const [searchOpen, setSearchOpen] = useState(false);
  const [activeTab, setActiveTab] = useState<SearchTab>("ai");
  const [aiMessages, setAiMessages] = useState<AiMessage[]>([]);
  const [aiInput, setAiInput] = useState("");
  const [searchValue, setSearchValue] = useState("");
  const [aiLoading, setAiLoading] = useState(false);
  const [openCreateMenu, setOpenCreateMenu] = useState(false);
  const [openSoftwareModal, setOpenSoftwareModal] = useState(false);
  const [openHardwareModal, setOpenHardwareModal] = useState(false);
  const searchSlotRef = useRef<HTMLDivElement>(null);
  const dropdownRef = useRef<HTMLDivElement>(null);
  const panelInputRef = useRef<HTMLInputElement>(null);
  const bottomInputRef = useRef<HTMLInputElement>(null);
  const aiMessagesRef = useRef<HTMLDivElement>(null);
  const triggerGridRefresh = () => {
    rootStore.dispatch({ type: "REFRESH", payload: true });
  };
  const handleDocumentMouseDown = useCallback((e: MouseEvent) => {
    if (
      searchSlotRef.current &&
      !searchSlotRef.current.contains(e.target as Node)
    ) {
      setSearchOpen(false);
    }
  }, []);

  useEffect(() => {
    document.addEventListener("mousedown", handleDocumentMouseDown);
    return () =>
      document.removeEventListener("mousedown", handleDocumentMouseDown);
  }, [handleDocumentMouseDown]);

  useEffect(() => {
    const handleClick = (e: MouseEvent) => {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(e.target as Node)
      ) {
        setOpenCreateMenu(false);
      }
    };
    document.addEventListener("mousedown", handleClick);
    return () => document.removeEventListener("mousedown", handleClick);
  }, []);

  useEffect(() => {
    if (aiMessagesRef.current) {
      aiMessagesRef.current.scrollTop = aiMessagesRef.current.scrollHeight;
    }
  }, [aiMessages, aiLoading]);

  useEffect(() => {
    if (searchOpen) {
      const t = setTimeout(() => {
        if (activeTab === "ai") {
          bottomInputRef.current?.focus();
        } else {
          panelInputRef.current?.focus();
        }
      }, 60);
      return () => clearTimeout(t);
    }
  }, [searchOpen, activeTab]);

  const handleTabChange = (_: React.SyntheticEvent, val: SearchTab) => {
    setActiveTab(val);
  };

  const handleAiSend = () => {
    const text = aiInput.trim();
    if (!text) return;
    setAiMessages((prev) => [...prev, { role: "user", content: text }]);
    setAiInput("");
    setAiLoading(true);
    setTimeout(() => {
      setAiLoading(false);
      setAiMessages((prev) => [
        ...prev,
        {
          role: "assistant",
          content:
            "There were 30 Softwares installed on the Siemens 2322 hardware, 28 of them shared the same EOM date which was 23rd of August 2025 and the other 2 have the same date of 24th June 2026",
        },
      ]);
    }, 1300);
  };

  const hasConversation = aiMessages.length > 0 || aiLoading;

  const handleModalClose = () => {
    setOpenSoftwareModal(false);
    setOpenHardwareModal(false);
    triggerGridRefresh();
  };

  return (
    <>
      <StyledHeader>
        <LogoBox
          src={vfLogo}
          alt="Vodafone Logo"
          onClick={() => props.onNavigate?.("dashboard")}
        />

        <SearchSlot ref={searchSlotRef}>
          <HeaderSearchBar
            // onClick={() => setSearchOpen(true)}
            sx={{
              visibility: searchOpen ? "hidden" : "visible",
            }}
          >
            <FiSearch
              style={{ color: "#999999", fontSize: 20, flexShrink: 0 }}
            />
            <Typography
              sx={{
                flex: 1,
                fontSize: "16px",
                color: "rgba(13,13,13,0.3)",
                lineHeight: "28px",
                userSelect: "none",
                textAlign: "left",
                minWidth: 0,
                overflow: "hidden",
                textOverflow: "ellipsis",
              }}
            >
              Ask anything about TEMS - Under Construction
            </Typography>
            <AiBadge>
              <AiScanLineIcon style={{ color: "#B6B6B6" }} />
              <Typography
                sx={{
                  fontSize: "16px",
                  color: "rgba(13,13,13,0.3)",
                  lineHeight: "28px",
                }}
              >
                Ai
              </Typography>
            </AiBadge>
          </HeaderSearchBar>

          <Box
            sx={{
              position: "absolute",
              top: 0,
              left: 0,
              right: 0,
              display: "flex",
              flexDirection: "column",
              alignItems: "stretch",
              zIndex: 400,
              pointerEvents: searchOpen ? "all" : "none",
            }}
          >
            <Box
              sx={{
                background: "#FFFFFF",
                borderRadius: "10px",
                boxShadow: "0px 4px 37px rgba(0,0,0,0.11)",
                overflow: "hidden",
                maxHeight: searchOpen ? "600px" : "48px",
                opacity: searchOpen ? 1 : 0,
                transition:
                  "max-height 0.5s cubic-bezier(0.4,0,0.2,1), opacity 0.1s ease",
              }}
            >
              <Box
                sx={{
                  display: "flex",
                  flexDirection: "column",
                  padding: "16px",
                  gap: "16px",
                }}
              >
                <Box
                  sx={{
                    boxSizing: "border-box",
                    display: "flex",
                    flexDirection: "row",
                    alignItems: "center",
                    padding: "8px 16px",
                    gap: "8px",
                    height: "44px",
                    background: "#FFFFFF",
                    border: "1px solid #7E7E7E",
                    borderRadius: "6px",
                    cursor: activeTab === "ai" ? "text" : "text",
                  }}
                  onClick={() =>
                    activeTab === "ai" && bottomInputRef.current?.focus()
                  }
                >
                  <FiSearch
                    style={{ color: "#0D0D0D", fontSize: 20, flexShrink: 0 }}
                  />
                  {activeTab === "ai" ? (
                    <Typography
                      sx={{
                        flex: 1,
                        fontSize: "16px",
                        lineHeight: "28px",
                        color: "rgba(13,13,13,0.3)",
                        userSelect: "none",
                        textAlign: "left",
                      }}
                    >
                      Ask anything about TEMS
                    </Typography>
                  ) : (
                    <InputBase
                      inputRef={panelInputRef}
                      placeholder="Search…"
                      value={searchValue}
                      onChange={(e) => setSearchValue(e.target.value)}
                      onKeyDown={(e) => {
                        if (e.key === "Escape") setSearchOpen(false);
                      }}
                      sx={{
                        flex: 1,
                        fontSize: "16px",
                        color: "#0D0D0D",
                        "& .MuiInputBase-input": {
                          padding: 0,
                          "&::placeholder": {
                            color: "rgba(13,13,13,0.3)",
                            opacity: 1,
                          },
                        },
                      }}
                    />
                  )}
                </Box>

                <StyledTabs value={activeTab} onChange={handleTabChange}>
                  <StyledTab
                    value="ai"
                    label={
                      <Box
                        sx={{
                          display: "flex",
                          alignItems: "center",
                          gap: "6px",
                        }}
                      >
                        <AiScanLineIcon
                          style={{
                            color: activeTab === "ai" ? "#0D0D0D" : "#888",
                            width: 20,
                            height: 20,
                          }}
                        />
                        <span>Ai</span>
                      </Box>
                    }
                  />
                  <StyledTab value="software" label="Software" />
                  <StyledTab value="hardware" label="Hardware" />
                  <StyledTab value="systemtype" label="System type" />
                </StyledTabs>

                {activeTab === "ai" ? (
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "column",
                      gap: "16px",
                    }}
                  >
                    <Box
                      ref={aiMessagesRef}
                      sx={{
                        display: "flex",
                        flexDirection: "column",
                        gap: "20px",
                        maxHeight: "300px",
                        overflowY: "auto",
                        "&::-webkit-scrollbar": { width: "6px" },
                        "&::-webkit-scrollbar-track": {
                          background: "transparent",
                        },
                        "&::-webkit-scrollbar-thumb": {
                          background: "rgba(0,0,0,0.18)",
                          borderRadius: "4px",
                        },
                        ...(!hasConversation && {
                          minHeight: "80px",
                          justifyContent: "center",
                          alignItems: "center",
                        }),
                      }}
                    >
                      {!hasConversation && (
                        <Typography
                          sx={{
                            fontSize: "14px",
                            color: "rgba(13,13,13,0.35)",
                            textAlign: "center",
                          }}
                        >
                          Ask me anything about TEMS
                        </Typography>
                      )}
                      {aiMessages.map((msg, idx) =>
                        msg.role === "user" ? (
                          <UserBubble key={idx}>{msg.content}</UserBubble>
                        ) : (
                          <AiBubble key={idx}>
                            <Box
                              sx={{
                                flexShrink: 0,
                                mt: "2px",
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                width: 28,
                                height: 28,
                              }}
                            >
                              <AiScanLineIcon
                                style={{
                                  color: "#E60000",
                                  width: 20,
                                  height: 20,
                                }}
                              />
                            </Box>
                            <span>{msg.content}</span>
                          </AiBubble>
                        )
                      )}
                      {aiLoading && (
                        <AiBubble>
                          <Box
                            sx={{
                              flexShrink: 0,
                              mt: "2px",
                              width: 28,
                              height: 28,
                              display: "flex",
                              alignItems: "center",
                              justifyContent: "center",
                            }}
                          >
                            <AiScanLineIcon
                              style={{
                                color: "#E60000",
                                width: 20,
                                height: 20,
                              }}
                            />
                          </Box>
                          <CircularProgress
                            size={14}
                            sx={{ color: "#E60000", mt: "6px" }}
                          />
                        </AiBubble>
                      )}
                    </Box>

                    <Box
                      sx={{
                        boxSizing: "border-box",
                        display: "flex",
                        flexDirection: "row",
                        justifyContent: "space-between",
                        alignItems: "center",
                        padding: "11px 16px 10px",
                        width: "100%",
                        height: "43px",
                        background: "#FFFFFF",
                        border: "1px solid #7E7E7E",
                        borderRadius: "6px",
                      }}
                    >
                      <InputBase
                        inputRef={bottomInputRef}
                        placeholder="Type a message"
                        value={aiInput}
                        onChange={(e) => setAiInput(e.target.value)}
                        onKeyDown={(e) => {
                          if (e.key === "Enter") handleAiSend();
                        }}
                        sx={{
                          flex: 1,
                          fontSize: "17px",
                          color: "#0D0D0D",
                          "& .MuiInputBase-input": {
                            padding: 0,
                            "&::placeholder": {
                              color: "rgba(0,0,0,0.5)",
                              opacity: 1,
                            },
                          },
                        }}
                      />
                      <Box
                        onClick={handleAiSend}
                        sx={{
                          flexShrink: 0,
                          cursor: aiInput.trim() ? "pointer" : "default",
                          opacity: aiInput.trim() ? 1 : 0.5,
                          transition: "opacity 0.15s ease",
                          display: "flex",
                          alignItems: "center",
                          ml: "8px",
                        }}
                      >
                        <SendIcon color="#000000" />
                      </Box>
                    </Box>
                  </Box>
                ) : (
                  <Box sx={{ pb: "4px" }}>
                    {(MOCK_RESULTS[activeTab] || []).map((result, idx) => (
                      <ResultItem key={idx}>{result}</ResultItem>
                    ))}
                    {(MOCK_RESULTS[activeTab] || []).length === 0 && (
                      <Typography
                        sx={{
                          p: "24px",
                          fontSize: "14px",
                          color: "rgba(13,13,13,0.4)",
                          textAlign: "center",
                        }}
                      >
                        No results found
                      </Typography>
                    )}
                  </Box>
                )}
              </Box>
            </Box>
          </Box>
        </SearchSlot>

        <Box ref={dropdownRef} sx={{ position: "relative" }}>
          <CreateNewButton
            disableElevation
            onClick={() => setOpenCreateMenu((p) => !p)}
          >
            <FiPlus style={{ fontSize: 18 }} />
            Create new
            {openCreateMenu ? (
              <FiChevronUp style={{ fontSize: 16 }} />
            ) : (
              <FiChevronDown style={{ fontSize: 16 }} />
            )}
          </CreateNewButton>

          {openCreateMenu && (
            <DropdownMenu>
              {props?.role === "SW Product Owner" && (
                <DropdownItem
                  onClick={() => {
                    setOpenSoftwareModal(true);
                    setOpenCreateMenu(false);
                  }}
                >
                  Software
                </DropdownItem>
              )}
              {props?.role === "HW Product Owner" && (
                <DropdownItem
                  onClick={() => {
                    setOpenHardwareModal(true);
                    setOpenCreateMenu(false);
                  }}
                >
                  Hardware
                </DropdownItem>
              )}
            </DropdownMenu>
          )}
        </Box>
      </StyledHeader>

      <ProductSoftwareModal
        open={openSoftwareModal}
        onClose={handleModalClose}
        mode={"new"}
        action={{
          closeModal: handleModalClose,
          refresh: triggerGridRefresh,
        }}
      />
      <ProductHardwareModal
        open={openHardwareModal}
        onClose={handleModalClose}
        mode={"new"}
        action={{
          closeModal: handleModalClose,
          refresh: triggerGridRefresh,
        }}
      />
    </>
  );
};

export default VodafoneHeader;
