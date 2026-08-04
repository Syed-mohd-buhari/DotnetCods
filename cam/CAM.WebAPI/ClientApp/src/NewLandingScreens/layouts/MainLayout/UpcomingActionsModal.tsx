import React, { useRef, useEffect } from "react";
import { Box, Typography } from "@mui/material";
import { styled } from "@mui/material/styles";
import { FiX, FiChevronUp, FiClock } from "react-icons/fi";
import { createPortal } from "react-dom";

// ─── Types ────────────────────────────────────────────────────────────────────

interface SubAction {
  id: string;
  name: string;
  country: string;
  countryFlag: string;
  date: string;
}

interface ProductGroup {
  id: string;
  productName: string;
  subActions: SubAction[];
  currentDC: string;
  plannedDC: string;
  plannedCompletion: string;
}

interface UpcomingActionsModalProps {
  open: boolean;
  onClose: () => void;
}

// ─── Mock Data ────────────────────────────────────────────────────────────────

const upcomingGroups: ProductGroup[] = [
  {
    id: "1",
    productName: "Nokia AirScale RAN",
    subActions: [
      {
        id: "1a",
        name: "Budget planning",
        country: "UK",
        countryFlag: "🇬🇧",
        date: "March 2026",
      },
      {
        id: "1b",
        name: "Market research",
        country: "Germany",
        countryFlag: "🇩🇪",
        date: "June 2025",
      },
      {
        id: "1c",
        name: "User feedback analysis",
        country: "France",
        countryFlag: "🇫🇷",
        date: "September 2025",
      },
    ],
    currentDC: "Huawei SBC22.1 on other NFVI for interconnect",
    plannedDC: "Huawei SBC22.1 on other NFVI for interconnect",
    plannedCompletion: "28th February 2026",
  },
  {
    id: "2",
    productName: "ZTE Cloud RAN",
    subActions: [
      {
        id: "2a",
        name: "Infrastructure audit",
        country: "Spain",
        countryFlag: "🇪🇸",
        date: "April 2026",
      },
      {
        id: "2b",
        name: "Compliance review",
        country: "Italy",
        countryFlag: "🇮🇹",
        date: "July 2025",
      },
    ],
    currentDC: "ZTE Cloud 18.2 on NFVI interconnect",
    plannedDC: "ZTE Cloud 20.1 on upgraded NFVI",
    plannedCompletion: "15th June 2026",
  },
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

const ModalCard = styled(Box)({
  display: "flex",
  flexDirection: "column",
  alignItems: "flex-start",
  padding: "19.93px",
  gap: "19.93px",
  isolation: "isolate",
  width: "563px",
  maxHeight: "85vh",
  background: "#FFFFFF",
  boxShadow: "0px -13px 26.4px rgba(12, 26, 75, 0.24)",
  borderRadius: "12px",
  overflowY: "auto",
  "&::-webkit-scrollbar": { width: "6px" },
  "&::-webkit-scrollbar-track": { background: "transparent" },
  "&::-webkit-scrollbar-thumb": {
    background: "rgba(0,0,0,0.15)",
    borderRadius: "4px",
  },
});

const ModalHeader = styled(Box)({
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  alignItems: "center",
  gap: "10px",
  width: "100%",
  flexShrink: 0,
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
  "&:hover": { background: "#ECEEF2" },
});

const CloseButton = styled(Box)({
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  width: "24px",
  height: "24px",
  cursor: "pointer",
  "&:hover": { opacity: 0.7 },
});

const Divider = styled(Box)({
  width: "100%",
  height: "0px",
  border: "1px solid rgba(0, 0, 0, 0.2)",
  flexShrink: 0,
});

// Scrollable container for all product groups
const ScrollContainer = styled(Box)({
  display: "flex",
  flexDirection: "column",
  alignItems: "flex-start",
  gap: "20px",
  width: "100%",
  overflowY: "auto",
  borderRadius: "10px",
  flexShrink: 0,
  "&::-webkit-scrollbar": { width: "6px" },
  "&::-webkit-scrollbar-track": { background: "transparent" },
  "&::-webkit-scrollbar-thumb": {
    background: "rgba(0,0,0,0.15)",
    borderRadius: "4px",
  },
});

// Outer product card wrapper
const ProductCard = styled(Box)({
  display: "flex",
  flexDirection: "row",
  alignItems: "flex-start",
  padding: "16px",
  gap: "16px",
  width: "100%",
  background: "#F4F6F9",
  borderRadius: "10px",
  flexShrink: 0,
});

// Sub-action row with grey bg
const SubActionRow = styled(Box)({
  boxSizing: "border-box",
  display: "flex",
  flexDirection: "column",
  justifyContent: "center",
  alignItems: "center",
  padding: "8px",
  gap: "16px",
  width: "100%",
  background: "rgba(220, 221, 224, 0.5)",
  borderRadius: "6px",
});

const CountryBadge = styled(Box)({
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  gap: "4px",
  padding: "2px 6px",
  background: "#E5E5E5",
  borderRadius: "2px",
  flexShrink: 0,
  height: "21px",
});

const DateBadge = styled(Box)({
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  gap: "4px",
  flexShrink: 0,
});

const UpgradeButton = styled(Box)({
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

const MetaGrid = styled(Box)({
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  alignItems: "flex-start",
  gap: "24px",
  width: "100%",
});

const MetaColumn = styled(Box)({
  display: "flex",
  flexDirection: "column",
  justifyContent: "center",
  alignItems: "flex-start",
  gap: "13px",
  width: "148px",
});

// ─── Sub-action row component ─────────────────────────────────────────────────

const SubActionItem: React.FC<{ action: SubAction; buttonLabel: string }> = ({
  action,
  buttonLabel,
}) => (
  <SubActionRow>
    {/* Name row: label left, country+date right */}
    <Box
      sx={{
        display: "flex",
        flexDirection: "row",
        alignItems: "flex-start",
        gap: "16px",
        width: "100%",
      }}
    >
      <Box
        sx={{
          display: "flex",
          flexDirection: "row",
          alignItems: "center",
          gap: "24px",
          flex: 1,
        }}
      >
        <Typography
          sx={{
            fontFamily: "'Vodafone Rg', sans-serif",
            fontSize: "14px",
            lineHeight: "16px",
            color: "#0D0D0D",
          }}
        >
          {action.name}
        </Typography>
      </Box>

      {/* Right: country + date */}
      <Box
        sx={{
          display: "flex",
          flexDirection: "row",
          justifyContent: "flex-end",
          alignItems: "center",
          gap: "16px",
        }}
      >
        <CountryBadge>
          <Typography sx={{ fontSize: "13px" }}>
            {action.countryFlag}
          </Typography>
          <Typography
            sx={{
              fontFamily: "'Vodafone Rg', sans-serif",
              fontSize: "12px",
              lineHeight: "28px",
              color: "#757575",
            }}
          >
            {action.country}
          </Typography>
        </CountryBadge>

        <DateBadge>
          <FiClock style={{ fontSize: "12px", color: "#757575" }} />
          <Typography
            sx={{
              fontFamily: "'Vodafone Rg', sans-serif",
              fontSize: "12px",
              lineHeight: "28px",
              color: "#757575",
            }}
          >
            {action.date}
          </Typography>
        </DateBadge>
      </Box>
    </Box>

    {/* Upgrade button */}
    <UpgradeButton>
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
        {buttonLabel}
      </Typography>
    </UpgradeButton>
  </SubActionRow>
);

// ─── Main Component ────────────────────────────────────────────────────────────

const UpcomingActionsModal: React.FC<UpcomingActionsModalProps> = ({
  open,
  onClose,
}) => {
  const overlayRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleKey = (e: KeyboardEvent) => {
      if (e.key === "Escape") onClose();
    };
    if (open) document.addEventListener("keydown", handleKey);
    return () => document.removeEventListener("keydown", handleKey);
  }, [open, onClose]);

  if (!open) return null;

  return createPortal(
    <>
      <Overlay
        ref={overlayRef}
        onClick={(e) => {
          if (e.target === overlayRef.current) onClose();
        }}
      >
        <ModalCard>
          {/* Header */}
          <ModalHeader>
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
              <FilterButton>
                <Box
                  sx={{
                    display: "flex",
                    flexDirection: "row",
                    justifyContent: "space-between",
                    alignItems: "center",
                    width: "199px",
                    height: "28px",
                  }}
                >
                  <Typography
                    sx={{
                      fontFamily: "'Vodafone Rg', sans-serif",
                      fontWeight: 400,
                      fontSize: "16px",
                      lineHeight: "28px",
                      color: "rgba(0,0,0,0.9)",
                    }}
                  >
                    1 Month
                  </Typography>
                  <FiChevronUp
                    style={{ fontSize: "16px", color: "rgba(0,0,0,0.9)" }}
                  />
                </Box>
              </FilterButton>

              <CloseButton onClick={onClose}>
                <FiX style={{ fontSize: "20px", color: "#000000" }} />
              </CloseButton>
            </Box>
          </ModalHeader>

          {/* Divider */}
          <Divider />

          {/* Scrollable product groups */}
          <ScrollContainer>
            {upcomingGroups.map((group) => (
              <ProductCard key={group.id}>
                <Box
                  sx={{
                    display: "flex",
                    flexDirection: "column",
                    justifyContent: "center",
                    alignItems: "flex-start",
                    gap: "24px",
                    width: "100%",
                  }}
                >
                  {/* Product title */}
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "row",
                      alignItems: "center",
                      gap: "8px",
                      width: "100%",
                    }}
                  >
                    <Typography
                      sx={{
                        fontFamily: "'Vodafone Rg', sans-serif",
                        fontSize: "16px",
                        lineHeight: "18px",
                        color: "#0D0D0D",
                      }}
                    >
                      Past planned completion date for the product{" "}
                      <span style={{ textDecoration: "underline" }}>
                        {group.productName}
                      </span>
                    </Typography>
                  </Box>

                  {/* Sub-action rows */}
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "column",
                      alignItems: "flex-start",
                      gap: "12px",
                      width: "100%",
                    }}
                  >
                    {group.subActions.map((sub) => (
                      <SubActionItem
                        key={sub.id}
                        action={sub}
                        buttonLabel="Upgrade"
                      />
                    ))}
                  </Box>

                  {/* Metadata footer */}
                  <MetaGrid>
                    <MetaColumn>
                      <Typography
                        sx={{
                          fontFamily: "'Vodafone Rg', sans-serif",
                          fontSize: "12px",
                          lineHeight: "28px",
                          color: "#757575",
                        }}
                      >
                        Current DC
                      </Typography>
                      <Typography
                        sx={{
                          fontFamily: "'Vodafone Rg', sans-serif",
                          fontSize: "12px",
                          lineHeight: "14px",
                          color: "rgba(0,0,0,0.8)",
                        }}
                      >
                        {group.currentDC}
                      </Typography>
                    </MetaColumn>

                    <MetaColumn>
                      <Typography
                        sx={{
                          fontFamily: "'Vodafone Rg', sans-serif",
                          fontSize: "12px",
                          lineHeight: "28px",
                          color: "#757575",
                        }}
                      >
                        Planned DC
                      </Typography>
                      <Typography
                        sx={{
                          fontFamily: "'Vodafone Rg', sans-serif",
                          fontSize: "12px",
                          lineHeight: "14px",
                          color: "rgba(0,0,0,0.8)",
                        }}
                      >
                        {group.plannedDC}
                      </Typography>
                    </MetaColumn>

                    <MetaColumn>
                      <Typography
                        sx={{
                          fontFamily: "'Vodafone Rg', sans-serif",
                          fontSize: "12px",
                          lineHeight: "28px",
                          color: "#757575",
                        }}
                      >
                        Planned completion
                      </Typography>
                      <Typography
                        sx={{
                          fontFamily: "'Vodafone Rg', sans-serif",
                          fontSize: "12px",
                          lineHeight: "14px",
                          color: "rgba(0,0,0,0.8)",
                        }}
                      >
                        {group.plannedCompletion}
                      </Typography>
                    </MetaColumn>
                  </MetaGrid>
                </Box>
              </ProductCard>
            ))}
          </ScrollContainer>
        </ModalCard>
      </Overlay>
    </>,
    document.body // renders directly on body, bypasses all stacking contexts
  );
};

export default UpcomingActionsModal;
