import React, { useRef, useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";
import { styled } from "@mui/material/styles";
import { FiX, FiChevronUp, FiChevronDown, FiCheck } from "react-icons/fi";
import { createPortal } from "react-dom";

// ─── Types ────────────────────────────────────────────────────────────────────

interface OpenAction {
  id: string;
  productName: string;
  productUnderlined: boolean;
  description: string;
  eom: string;
  eos: string;
  country: string;
  countryFlag: string;
}

interface OpenActionsModalProps {
  open: boolean;
  onClose: () => void;
  upcomingActionList: any[];
  handleSelectedMonth: (value) => any;
  monthSelected: number;
  handleRedirect?: (id: any) => any;
}

const MONTH_OPTIONS = [
  { label: "1 Month", value: 1 },
  { label: "2 Months", value: 2 },
  { label: "3 Months", value: 3 },
];

// ─── Styled Components ────────────────────────────────────────────────────────

const Overlay = styled(Box)({
  position: "fixed",
  inset: 0,
  background: "rgba(0,0,0,0.6)",
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  zIndex: 1300,
});

// Modal card: fixed height, internal scroll only on cards list
const ModalCard = styled(Box)({
  display: "flex",
  flexDirection: "column",
  width: "563px",
  maxHeight: "85vh",
  background: "#FFFFFF",
  boxShadow: "0px -13px 26.4px rgba(12, 26, 75, 0.24)",
  borderRadius: "12px",
  overflow: "hidden", // clip children; only scroll region scrolls
});

// Sticky header — never scrolls
const ModalHeader = styled(Box)({
  display: "flex",
  flexDirection: "column",
  gap: "0px",
  flexShrink: 0,
  background: "#FFFFFF",
  zIndex: 10,
});

const HeaderRow = styled(Box)({
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  alignItems: "center",
  gap: "10px",
  width: "100%",
  padding: "19.93px",
});

const Divider = styled(Box)({
  width: "100%",
  height: "1px",
  background: "rgba(0, 0, 0, 0.2)",
  flexShrink: 0,
});

// Scrollable cards region
const ScrollBody = styled(Box)({
  flex: 1,
  overflowY: "auto",
  padding: "19.93px",
  display: "flex",
  flexDirection: "column",
  gap: "16px",
  "&::-webkit-scrollbar": { width: "6px" },
  "&::-webkit-scrollbar-track": { background: "transparent" },
  "&::-webkit-scrollbar-thumb": {
    background: "rgba(0,0,0,0.18)",
    borderRadius: "4px",
  },
});

const FilterButton = styled(Box)({
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  padding: "8px 16px",
  gap: "8px",
  background: "#F5F7FA",
  borderRadius: "10px",
  cursor: "pointer",
  position: "relative",
  "&:hover": { background: "#ECEEF2" },
});

const DropdownMenu = styled(Box)({
  position: "absolute",
  top: "calc(100% + 6px)",
  left: 0,
  width: "100%",
  background: "#FFFFFF",
  borderRadius: "10px",
  boxShadow: "0px 4px 20px rgba(0,0,0,0.12)",
  zIndex: 100,
  overflow: "hidden",
});

const DropdownItem = styled(Box)<{ selected?: boolean }>(({ selected }) => ({
  display: "flex",
  alignItems: "center",
  justifyContent: "space-between",
  padding: "10px 16px",
  cursor: "pointer",
  background: selected ? "#F0F0F3" : "#FFFFFF",
  "&:hover": { background: "#F5F7FA" },
}));

const CloseButton = styled(Box)({
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  width: "24px",
  height: "24px",
  cursor: "pointer",
  flexShrink: 0,
  "&:hover": { opacity: 0.7 },
});

const ActionCard = styled(Box)({
  display: "flex",
  flexDirection: "row",
  alignItems: "flex-start",
  padding: "16px",
  gap: "16px",
  width: "100%",
  background: "#F4F6F9",
  borderRadius: "5px",
  flexShrink: 0,
  boxSizing: "border-box",
});

const CountryBadge = styled(Box)({
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  padding: "4px 6px",
  gap: "4px",
  background: "#E5E5E5",
  borderRadius: "2px",
  flexShrink: 0,
});

const ActionButton = styled(Box)({
  display: "flex",
  flexDirection: "row",
  justifyContent: "center",
  alignItems: "center",
  padding: "5.5px 10px 6.5px",
  gap: "12px",
  width: "174px",
  height: "32px",
  background: "#FFFFFF",
  borderRadius: "8px",
  cursor: "pointer",
  "&:hover": { background: "#F5F7FA" },
});

// ─── Component ────────────────────────────────────────────────────────────────

const OpenActionsModal: React.FC<OpenActionsModalProps> = ({
  open,
  onClose,
  upcomingActionList,
  monthSelected,
  handleSelectedMonth,
  handleRedirect,
}) => {
  const overlayRef = useRef<HTMLDivElement>(null);
  const dropdownRef = useRef<HTMLDivElement>(null);
  const [activeTab, setActiveTab] = useState("Product Library");
  const [selectedMonth, setSelectedMonth] = useState(
    monthSelected
      ? MONTH_OPTIONS?.filter((res) => res.value === monthSelected)[0]
      : MONTH_OPTIONS[0]
  );
  const [dropdownOpen, setDropdownOpen] = useState(false);

  // Escape key closes modal
  useEffect(() => {
    const handleKey = (e: KeyboardEvent) => {
      if (e.key === "Escape") {
        if (dropdownOpen) setDropdownOpen(false);
        else onClose();
      }
    };
    if (open) document.addEventListener("keydown", handleKey);
    return () => document.removeEventListener("keydown", handleKey);
  }, [open, onClose, dropdownOpen]);

  useEffect(() => {
    if (monthSelected) {
      setSelectedMonth(
        monthSelected
          ? MONTH_OPTIONS?.filter((res) => res.value === monthSelected)[0]
          : MONTH_OPTIONS[0]
      );
    }
  }, [monthSelected]);

  // Click outside dropdown closes it
  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(e.target as Node)
      ) {
        setDropdownOpen(false);
      }
    };
    if (dropdownOpen)
      document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, [dropdownOpen]);

  if (!open) return null;

  return createPortal(
    <Overlay
      ref={overlayRef}
      onClick={(e) => {
        if (e.target === overlayRef.current) onClose();
      }}
    >
      <ModalCard>
        {/* ── Fixed Header ── */}
        <ModalHeader>
          <HeaderRow>
            <Typography
              sx={{
                fontFamily: "'Vodafone Rg', sans-serif",
                fontWeight: 700,
                fontSize: "18px",
                lineHeight: "28px",
                color: "#0D0D0D",
              }}
            >
              Upcoming actions
            </Typography>

            <Box sx={{ display: "flex", alignItems: "center", gap: "16px" }}>
              {/* Month filter dropdown */}
              <FilterButton ref={dropdownRef}>
                <Box
                  sx={{
                    display: "flex",
                    flexDirection: "row",
                    justifyContent: "space-between",
                    alignItems: "center",
                    width: "120px",
                    height: "28px",
                    gap: "8px",
                  }}
                  onClick={() => setDropdownOpen((p) => !p)}
                >
                  <Typography
                    sx={{
                      fontFamily: "'Vodafone Rg', sans-serif",
                      fontWeight: 400,
                      fontSize: "16px",
                      lineHeight: "28px",
                      color: "rgba(0,0,0,0.9)",
                      userSelect: "none",
                    }}
                  >
                    {selectedMonth.label}
                  </Typography>
                  {dropdownOpen ? (
                    <FiChevronUp
                      style={{
                        fontSize: "16px",
                        color: "rgba(0,0,0,0.9)",
                        flexShrink: 0,
                      }}
                    />
                  ) : (
                    <FiChevronDown
                      style={{
                        fontSize: "16px",
                        color: "rgba(0,0,0,0.9)",
                        flexShrink: 0,
                      }}
                    />
                  )}
                </Box>

                {dropdownOpen && (
                  <DropdownMenu>
                    {MONTH_OPTIONS.map((opt) => (
                      <DropdownItem
                        key={opt.value}
                        selected={opt.value === selectedMonth.value}
                        onClick={(e) => {
                          e.stopPropagation();
                          setSelectedMonth(opt);
                          handleSelectedMonth(opt);
                          setDropdownOpen(false);
                        }}
                      >
                        <Typography
                          sx={{
                            fontFamily: "'Vodafone Rg', sans-serif",
                            fontSize: "15px",
                            color:
                              opt.value === selectedMonth.value
                                ? "#E60000"
                                : "#0D0D0D",
                            fontWeight:
                              opt.value === selectedMonth.value ? 700 : 400,
                          }}
                        >
                          {opt.label}
                        </Typography>
                        {opt.value === selectedMonth.value && (
                          <FiCheck
                            style={{ fontSize: "14px", color: "#E60000" }}
                          />
                        )}
                      </DropdownItem>
                    ))}
                  </DropdownMenu>
                )}
              </FilterButton>

              <CloseButton onClick={onClose}>
                <FiX style={{ fontSize: "20px", color: "#000000" }} />
              </CloseButton>
            </Box>
          </HeaderRow>

          {/* Sticky divider */}
          <Divider />
        </ModalHeader>

        <ActionCard sx={{ padding: "10px 20px" }}>
          <ActionButton
            onClick={() => setActiveTab("Product Library")}
            sx={{
              background: activeTab === "Product Library" ? "#ffd7d7" : "white",
              color: activeTab === "Product Library" ? "#E60000" : "inherit",
              borderRadius: "4px !important",
              "&:hover": {
                background: "#ffd7d7",
                color: "#E60000",
              },
            }}
          >
            <Typography
              sx={{
                fontWeight: "bold",
                fontSize: "14px",
                lineHeight: "20px",
                letterSpacing: "-0.084px",
              }}
            >
              Product Library
            </Typography>
          </ActionButton>

          <ActionButton
            sx={{
              cursor: "not-allowed",
              background: activeTab === "LCM" ? "#E60000" : "white",
              color: activeTab === "LCM" ? "white" : "inherit",
              borderRadius: "4px !important",
              "&:hover": {
                background: "#0000001a",
                color: "white",
              },
            }}
          >
            <Typography
              sx={{
                fontWeight: 400,
                fontSize: "14px",
                lineHeight: "20px",
                letterSpacing: "-0.084px",
                cursor: "not-allowed",
                color: activeTab === "LCM" ? "white" : "inherit",
              }}
            >
              LCM
            </Typography>
          </ActionButton>
        </ActionCard>

        {/* ── Scrollable Action Cards ── */}
        <ScrollBody>
          {upcomingActionList?.map((action) => (
            <ActionCard key={action.id}>
              <Box
                sx={{
                  display: "flex",
                  flexDirection: "column",
                  justifyContent: "center",
                  alignItems: "center",
                  gap: "16px",
                  width: "100%",
                }}
              >
                {/* Title section */}
                <Box
                  sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "flex-start",
                    gap: "8px",
                    width: "100%",
                  }}
                >
                  {/* Product name + country badge */}
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "row",
                      justifyContent: "space-between",
                      alignItems: "flex-start",
                      width: "100%",
                      gap: "12px",
                      textAlign: "left",
                    }}
                  >
                    <Typography
                      sx={{
                        fontFamily: "'Vodafone Rg', sans-serif",
                        fontWeight: 400,
                        fontSize: "16px",
                        lineHeight: "22px",
                        color: "#0D0D0D",
                        flex: 1,
                      }}
                    >
                      {action.productUnderlined ? (
                        <>
                          <span
                            style={{
                              textDecoration: "underline",
                              textUnderlineOffset: "3px",
                              color: "rgba(0, 0, 0, 0.5)",
                              cursor: "pointer",
                            }}
                            onClick={() =>
                              handleRedirect && handleRedirect(action.majorId)
                            }
                          >
                            {action.productName}
                          </span>{" "}
                          {action.description}
                        </>
                      ) : (
                        <>
                          {action.description}{" "}
                          <span
                            style={{
                              textDecoration: "underline",
                              textUnderlineOffset: "3px",
                              color: "rgba(0, 0, 0, 0.5)",
                            }}
                          >
                            {action.productName}
                          </span>
                        </>
                      )}
                    </Typography>

                    <CountryBadge>
                      {/* <Typography sx={{ fontSize: "13px", lineHeight: 1 }}>
                        {action.countryFlag}
                      </Typography> */}
                      <Typography
                        sx={{
                          fontFamily: "'Vodafone Rg', sans-serif",
                          fontSize: "13px",
                          lineHeight: "20px",
                          color: "#757575",
                          whiteSpace: "nowrap",
                        }}
                      >
                        {action.country}
                      </Typography>
                    </CountryBadge>
                  </Box>

                  {/* EOM / EOS dates */}
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "row",
                      alignItems: "center",
                      gap: "24px",
                    }}
                  >
                    <Typography
                      sx={{
                        fontFamily: "'Vodafone Rg', sans-serif",
                        fontSize: "14px",
                        lineHeight: "26px",
                        color: "rgba(0,0,0,0.5)",
                      }}
                    >
                      EOM: {action.eom}
                    </Typography>
                    <Typography
                      sx={{
                        fontFamily: "'Vodafone Rg', sans-serif",
                        fontSize: "14px",
                        lineHeight: "26px",
                        color: "rgba(0,0,0,0.5)",
                      }}
                    >
                      EOS: {action.eos}
                    </Typography>
                  </Box>
                </Box>

                {/* Inner divider */}
                {/* <Box
                  sx={{
                    width: "100%",
                    height: "1px",
                    background: "rgba(0,0,0,0.2)",
                  }}
                /> */}

                {/* Take action button */}
                {/* <ActionButton>
                  <Typography
                    sx={{
                      fontFamily: "'Vodafone Rg', sans-serif",
                      fontWeight: 400,
                      fontSize: "14px",
                      lineHeight: "20px",
                      letterSpacing: "-0.084px",
                      color: "#000000",
                    }}
                  >
                    Take action
                  </Typography>
                </ActionButton> */}
              </Box>
            </ActionCard>
          ))}
        </ScrollBody>
      </ModalCard>
    </Overlay>,
    document.body // renders directly on body, bypasses all stacking contexts
  );
};

export default OpenActionsModal;
