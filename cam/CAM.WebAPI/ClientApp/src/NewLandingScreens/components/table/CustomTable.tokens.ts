export const T = {
  red: "#E60000",
  black: "#0D0D0D",
  white: "#FFFFFF",
  headerText: "#525866",
  headerBg: "#F5F7FA",
  rowBorder: "rgba(0,0,0,0.06)",
  rowHover: "#F9F9FC",
  shadow:
    "0px 0px 4.1px rgba(12,26,75,0.01),0px 2.097px 9.6px -0.699px rgba(50,50,71,0.05)",
  menuShadow: "0px 8px 24px rgba(0,0,0,0.12), 0px 2px 6px rgba(0,0,0,0.08)",
};

export const ACTIONS_TRIGGER_ATTR = "data-row-actions-trigger";

export const DEFAULT_PILL_COLORS: Record<string, { bg: string; text: string }> =
  {
    Validate: { bg: "#E5F1FB", text: "#083774" },
    Plan: { bg: "#FFF3CD", text: "#7A5200" },
    Deploy: { bg: "#EDE9FE", text: "#4C1D95" },
    Assess: { bg: "#F3F4F6", text: "#374151" },
    Traditional: { bg: "#D1FAE5", text: "#065F46" },
    "EOM Review": { bg: "#FEE2E2", text: "#991B1B" },
    "Needs review": { bg: "#FEF3EB", text: "#853525" },
    "Pending validation": { bg: "#EDE9FE", text: "#4C1D95" },
    "On track": { bg: "#D1FAE5", text: "#065F46" },
    "One Track": { bg: "#E0F2FE", text: "#0369A1" },
    "At risk": { bg: "#FEE2E2", text: "#991B1B" },
    Approved: { bg: "#FEF3EB", text: "#854D0E" },
    "Pending feedback": { bg: "#F3F4F6", text: "#374151" },
  };

export const DEFAULT_DOT_COLORS: Record<string, string> = {
  High: "#E60000",
  Critical: "#E60000",
  Medium: "#F59E0B",
  Low: "#10B981",
  true: "#10B981",
  false: "#E60000",
};

export const AVATAR_PALETTE = [
  "#000000",
  "#1E3A5F",
  "#065F46",
  "#4C1D95",
  "#7A3F00",
];
