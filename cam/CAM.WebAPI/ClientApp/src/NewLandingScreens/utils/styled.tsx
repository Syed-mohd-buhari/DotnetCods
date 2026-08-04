import React from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import CustomCalendarIcon from "../../img/CustomCalendarIcon";
import { styled } from "@mui/material/styles";
import { Box, Typography } from "@mui/material";

export const DATEPICKER_GLOBAL_STYLES = `
  /* ── Popper wrapper: sit directly below the input, no extra offset ── */
  .psm-datepicker-popper {
    z-index: 9999 !important;
    padding-top: 4px !important;
  }
 
  /* ── Remove the default triangle arrow ── */
  .psm-datepicker-popper .react-datepicker__triangle {
    display: none !important;
  }
 
  /* ── Calendar container ── */
  .psm-datepicker-popper .react-datepicker {
    font-family: 'Vodafone Rg', sans-serif;
    border: 1px solid #E5E5E5;
    border-radius: 10px;
    box-shadow: 0px 4px 24px rgba(0,0,0,0.12);
    overflow: hidden;
    padding: 0;
    min-width: 280px;
  }
 
  /* ── Header bar ── */
  .psm-datepicker-popper .react-datepicker__header {
    background: #ffffff;
    border-bottom: 1px solid #F0F0F0;
    padding: 14px 16px 10px;
    border-radius: 0;
  }
 
  /* ── Month + year title ── */
  .psm-datepicker-popper .react-datepicker__current-month {
    font-family: 'Vodafone Rg', sans-serif;
    font-weight: 700;
    font-size: 15px;
    color: #0D0D0D;
    margin-bottom: 10px;
  }
 
  /* ── Navigation arrows ── */
  .psm-datepicker-popper .react-datepicker__navigation {
    top: 14px;
  }
  .psm-datepicker-popper .react-datepicker__navigation--previous {
    left: 14px;
  }
  .psm-datepicker-popper .react-datepicker__navigation--next {
    right: 14px;
  }
  .psm-datepicker-popper .react-datepicker__navigation-icon::before {
    border-color: #0D0D0D;
    border-width: 2px 2px 0 0;
    width: 8px;
    height: 8px;
  }
  .psm-datepicker-popper .react-datepicker__navigation:hover .react-datepicker__navigation-icon::before {
    border-color: #E60000;
  }
 
  /* ── Day-of-week header row ── */
  .psm-datepicker-popper .react-datepicker__day-names {
    display: flex;
    justify-content: space-around;
    padding: 0 8px;
  }
  .psm-datepicker-popper .react-datepicker__day-name {
    font-family: 'Vodafone Rg', sans-serif;
    font-size: 12px;
    font-weight: 600;
    color: #79797A;
    width: 34px;
    line-height: 34px;
    text-align: center;
    margin: 0;
  }
 
  /* ── Week rows ── */
  .psm-datepicker-popper .react-datepicker__month {
    margin: 6px 8px 10px;
  }
  .psm-datepicker-popper .react-datepicker__week {
    display: flex;
    justify-content: space-around;
  }
 
  /* ── Individual day cells ── */
  .psm-datepicker-popper .react-datepicker__day {
    font-family: 'Vodafone Rg', sans-serif;
    font-size: 13px;
    color: #0D0D0D;
    width: 34px;
    height: 34px;
    line-height: 34px;
    text-align: center;
    border-radius: 50%;
    margin: 2px 0;
    transition: background 0.15s, color 0.15s;
  }
  .psm-datepicker-popper .react-datepicker__day:hover {
    background: #FFE5E5;
    color: #E60000;
    border-radius: 50%;
  }
 
  /* ── Today highlight ── */
  .psm-datepicker-popper .react-datepicker__day--today {
    font-weight: 700;
    color: #E60000;
    background: transparent;
  }
  .psm-datepicker-popper .react-datepicker__day--today:hover {
    background: #FFE5E5;
  }
 
  /* ── Selected day ── */
  .psm-datepicker-popper .react-datepicker__day--selected,
  .psm-datepicker-popper .react-datepicker__day--keyboard-selected {
    background: #E60000 !important;
    color: #ffffff !important;
    border-radius: 50% !important;
    font-weight: 700;
  }
  .psm-datepicker-popper .react-datepicker__day--selected:hover,
  .psm-datepicker-popper .react-datepicker__day--keyboard-selected:hover {
    background: #C40000 !important;
  }
 
  /* ── Outside-month days ── */
  .psm-datepicker-popper .react-datepicker__day--outside-month {
    color: #C0C0C0;
  }
 
  /* ── Disabled days ── */
  .psm-datepicker-popper .react-datepicker__day--disabled {
    color: #D0D0D0 !important;
    cursor: not-allowed;
  }
  .psm-datepicker-popper .react-datepicker__day--disabled:hover {
    background: transparent !important;
  }
`;

if (typeof document !== "undefined") {
  const STYLE_ID = "psm-datepicker-styles";
  if (!document.getElementById(STYLE_ID)) {
    const style = document.createElement("style");
    style.id = STYLE_ID;
    style.textContent = DATEPICKER_GLOBAL_STYLES;
    document.head.appendChild(style);
  }
}

export const T = {
  red: "#E60000",
  black: "#0D0D0D",
  white: "#FFFFFF",
  shadow:
    "0px 0px 4.1px rgba(12,26,75,0.01),0px 2.097px 9.6px -0.699px rgba(50,50,71,0.05)",
};

export const INPUT_STYLE: React.CSSProperties = {
  width: "100%",
  fontSize: "14px",
  border: "1px solid #7E7E7E",
  borderRadius: "6px",
  padding: "8px 4px",
  outline: "none",
  background: "#fff",
};

export const LABEL_STYLE: React.CSSProperties = {
  fontSize: "14px",
  color: T.black,
  lineHeight: "28px",
  display: "flex",
  alignItems: "center",
  gap: "4px",
  marginBottom: "0px",
};

export const SELECT_STYLES = {
  control: (b: any, _state: any) => ({
    ...b,
    border: "1px solid #7E7E7E",
    borderRadius: "6px",
    minHeight: "38px",
    fontSize: "14px",
    boxShadow: "none",
    "&:hover": { border: "1px solid #7E7E7E" },
  }),
  menu: (b: any) => ({ ...b, zIndex: 9999 }),
  menuPortal: (b: any) => ({ ...b, zIndex: 9999 }),
};

export const REQ = (
  <span style={{ color: "#E30000", fontSize: "14px" }}>*</span>
);

export const ModalCard = styled(Box)({
  width: "90%",
  maxWidth: "1200px",
  maxHeight: "90vh",
  background: "#FFF",
  borderRadius: "0 12px",
  display: "flex",
  flexDirection: "column",
  gap: "0",
});

export const ModalHeader = styled(Box)({
  display: "flex",
  flexDirection: "column",
  gap: "0px",
  flexShrink: 0,
  background: "#FFFFFF",
  // zIndex: 10,
});

export const HeaderRow = styled(Box)({
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  alignItems: "center",
  gap: "10px",
  width: "100%",
  padding: "19.93px 24px",
});

export const ScrollBody = styled(Box)({
  flex: 1,
  overflowY: "auto",
  padding: "24px",
  gap: "24px",
  display: "flex",
  flexDirection: "column",
  "&::-webkit-scrollbar": { width: "6px" },
  "&::-webkit-scrollbar-track": { background: "transparent" },
  "&::-webkit-scrollbar-thumb": {
    background: "rgba(0,0,0,0.18)",
    borderRadius: "4px",
  },
});

export const ModalFooter = styled(Box)({
  display: "flex",
  justifyContent: "flex-end",
  gap: "12px",
  padding: "16px 24px",
  borderTop: "1px solid #E5E5E5",
  background: "#FFFFFF",
  position: "sticky",
  bottom: 0,
  // zIndex: 10,
});

export const SectionBox = styled(Box)({
  background: "#F4F6F9",
  borderRadius: "10px",
  padding: "16px",
  display: "flex",
  flexDirection: "column",
  gap: "16px",
});

export const ValidationLabel = styled("label")({
  fontSize: "12px",
  color: T.red,
  marginTop: "2px",
  display: "block",
});

export const FieldBox: React.FC<{
  width?: number | string;
  children: React.ReactNode;
}> = ({ width = 323, children }) => (
  <Box
    sx={{
      display: "flex",
      flexDirection: "column",
      gap: "4px",
      width,
      flexShrink: 0,
    }}
  >
    {children}
  </Box>
);

export const ToggleSwitch: React.FC<{
  checked: boolean;
  onChange?: (val: boolean) => void;
  disabled?: boolean;
  label: string;
  showToggle?: boolean; // when false, renders just the label (no switch)
}> = ({ checked, onChange, disabled = false, label, showToggle = true }) => (
  <Box sx={{ display: "flex", alignItems: "center", gap: "8px" }}>
    {showToggle && (
      <Box
        component="label"
        sx={{
          position: "relative",
          display: "inline-block",
          width: "40px",
          height: "22px",
          flexShrink: 0,
          marginBottom: 0,
        }}
      >
        <input
          type="checkbox"
          checked={checked}
          disabled={disabled}
          onChange={(e) => onChange?.(e.target.checked)}
          style={{ opacity: 0, width: 0, height: 0 }}
        />
        <Box
          component="span"
          sx={{
            position: "absolute",
            cursor: disabled ? "not-allowed" : "pointer",
            inset: 0,
            background: checked ? T.red : "#ccc",
            borderRadius: "34px",
            transition: "0.3s",
            opacity: disabled ? 0.5 : 1,
            "&::before": {
              content: '""',
              position: "absolute",
              height: "16px",
              width: "16px",
              left: checked ? "21px" : "3px",
              bottom: "3px",
              background: "#fff",
              borderRadius: "50%",
              transition: "0.3s",
            },
          }}
        />
      </Box>
    )}
    <Typography
      sx={{
        fontSize: "14px",
        color: T.black,
        opacity: disabled ? 0.7 : 1,
      }}
    >
      {label}
    </Typography>
  </Box>
);

export const DateField: React.FC<{
  label: React.ReactNode;
  value: Date | null;
  onChange: (date: Date | null) => void;
  disabled?: boolean;
  placeholder?: string;
  width?: number | string;
  id?: string;
}> = ({
  label,
  value,
  onChange,
  disabled = false,
  placeholder = "DD/MM/YYYY",
  width = 211,
  id,
}) => (
  <FieldBox width={width}>
    <label style={LABEL_STYLE} id={id}>
      {label}
    </label>
    <Box
      sx={{
        display: "flex",
        alignItems: "center",
        border: "1px solid #7E7E7E",
        borderRadius: "6px",
        padding: "8px 4px",
        gap: "8px",
        background: disabled ? "#f5f5f5" : "#fff",
        opacity: disabled ? 0.7 : 1,
        height: "38px !important",
      }}
    >
      <CustomCalendarIcon
        className="alert-icon"
        sx={{
          color: "#E60000",
          "&:hover": {
            color: "#E60000 !important",
          },
          width: 30,
          height: 30,
        }}
      />
      <DatePicker
        selected={value}
        onChange={onChange}
        dateFormat="dd/MM/yyyy"
        placeholderText={placeholder}
        disabled={disabled}
        minDate={new Date(1980, 0, 1)}
        maxDate={new Date(2999, 0, 1)}
        customInput={
          <input
            style={{
              border: "none",
              outline: "none",
              fontSize: "14px",
              flex: 1,
              background: "transparent",
              width: "100%",
            }}
          />
        }
      />
    </Box>
  </FieldBox>
);

export const Overlay = styled(Box)({
  position: "fixed",
  inset: 0,
  background: "rgba(0,0,0,0.6)",
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  zIndex: 1300,
});

export const DeleteModalCard = styled(Box)({
  width: "90%",
  maxWidth: "480px",
  background: "#FFF",
  borderRadius: "12px",
  display: "flex",
  flexDirection: "column",
  overflow: "hidden",
});

export const CloseButton = styled(Box)({
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  width: "24px",
  height: "24px",
  cursor: "pointer",
  flexShrink: 0,
  "&:hover": { opacity: 0.7 },
});

export const OutlineBtn = styled(Box)<{ disabled?: boolean }>(
  ({ disabled }) => ({
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    padding: "8px 24px",
    border: "1px solid #000",
    borderRadius: "6px",
    cursor: disabled ? "not-allowed" : "pointer",
    minWidth: "100px",
    height: "40px",
    opacity: disabled ? 0.5 : 1,
    "&:hover": { background: disabled ? "transparent" : "#F5F5F5" },
  })
);

export const RedBtn = styled(Box)<{ disabled?: boolean }>(({ disabled }) => ({
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
  padding: "8px 24px",
  background: T.red,
  borderRadius: "6px",
  cursor: disabled ? "not-allowed" : "pointer",
  minWidth: "100px",
  height: "40px",
  color: "white",
  fontWeight: 600,
  opacity: disabled ? 0.6 : 1,
  "&:hover": { background: disabled ? T.red : "#C40000" },
}));

export const RedButton = styled(Box)({
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  padding: "8px 16px",
  gap: "8px",
  background: T.red,
  borderRadius: "10px",
  cursor: "pointer",
  "&:hover": { background: "#C40000" },
});

export const InfoCard = styled(Box)({
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  padding: "8px 10px",
  gap: "16px",
  background: T.white,
  boxShadow: T.shadow,
  borderRadius: "8px",
  cursor: "pointer",
  flex: 1,
  justifyContent: "space-between",
  height: "85px",
  minWidth: "375px",
  position: "relative",
  overflow: "hidden",

  "&::before": {
    content: '""',
    position: "absolute",
    top: 0,
    left: 0,
    width: "0%",
    height: "100%",
    background: "#E60000",
    transition: "width 0.2s ease",
    zIndex: 0,
  },
  "&:hover::before": { width: "100%" },
  "& > *": { position: "relative", zIndex: 1 },
  "&:hover .alert-label": { color: T.white },
  "&:hover svg": {
    color: "#FFFFFF !important",
  },
});
