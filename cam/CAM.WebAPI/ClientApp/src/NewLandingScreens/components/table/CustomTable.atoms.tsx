import React, { useEffect, useRef, useState } from "react";
import { Box, Typography } from "@mui/material";
import { createPortal } from "react-dom";
import { T, AVATAR_PALETTE } from "./CustomTable.tokens";

export const Dot: React.FC<{ color: string; size?: number }> = ({
  color,
  size = 8,
}) => (
  <Box
    sx={{
      width: size,
      height: size,
      borderRadius: "50%",
      background: color,
      flexShrink: 0,
    }}
  />
);

export const PillBadge: React.FC<{
  label: string;
  bg: string;
  color: string;
}> = ({ label, bg, color }) => (
  <Box
    sx={{
      display: "inline-flex",
      alignItems: "center",
      justifyContent: "center",
      px: "10px",
      py: "3px",
      borderRadius: "5px",
      background: bg,
      flexShrink: 0,
    }}
  >
    <Typography
      sx={{ fontSize: "13px", lineHeight: "18px", color, whiteSpace: "nowrap" }}
    >
      {label}
    </Typography>
  </Box>
);

export const AvatarCircle: React.FC<{ initials: string; idx: number }> = ({
  initials,
  idx,
}) => (
  <Box
    sx={{
      width: 28,
      height: 28,
      borderRadius: "50%",
      background: AVATAR_PALETTE[idx % AVATAR_PALETTE.length],
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
      flexShrink: 0,
    }}
  >
    <Typography
      sx={{ fontSize: "10px", fontWeight: 700, color: "#fff", lineHeight: 1 }}
    >
      {initials || "??"}
    </Typography>
  </Box>
);

export const CustomTooltip: React.FC<{
  text: string;
  children: React.ReactNode;
}> = ({ text, children }) => {
  const [show, setShow] = useState(false);
  const ref = useRef<HTMLDivElement>(null);
  const [pos, setPos] = useState({ top: 0, left: 0 });

  return (
    <>
      <Box
        ref={ref}
        sx={{
          position: "relative",
          display: "inline-flex",
          alignItems: "center",
          maxWidth: "100%",
        }}
        onMouseEnter={() => {
          if (ref.current) {
            const r = ref.current.getBoundingClientRect();
            setPos({ top: r.top - 40, left: r.left });
          }
          setShow(true);
        }}
        onMouseLeave={() => setShow(false)}
      >
        {children}
        {show &&
          text &&
          createPortal(
            <Box
              sx={{
                position: "fixed",
                top: pos.top,
                left: pos.left,
                background: "#1E2533",
                color: "#fff",
                borderRadius: "6px",
                px: "10px",
                py: "6px",
                zIndex: 9999,
                fontSize: "12px",
                pointerEvents: "none",
                boxShadow: "0 2px 8px rgba(0,0,0,0.2)",
                display: "flex",
                flexDirection: "column",
                gap: "4px",
              }}
            >
              {text.split("\n").map((t, i) => (
                <Typography
                  key={i}
                  sx={{ fontSize: "12px", color: "#fff", lineHeight: 1.5 }}
                >
                  {t}
                </Typography>
              ))}
              <Box
                sx={{
                  position: "absolute",
                  top: "100%",
                  left: "10px",
                  borderWidth: "5px",
                  borderStyle: "solid",
                  borderColor: "#1E2533 transparent transparent transparent",
                }}
              />
            </Box>,
            document.body
          )}
      </Box>
    </>
  );
};

export const TruncText: React.FC<{
  text: string;
  maxWidth: number;
  sx?: any;
}> = ({ text, maxWidth, sx }) => {
  const ref = useRef<HTMLSpanElement>(null);
  const [clipped, setClipped] = useState(false);

  useEffect(() => {
    if (ref.current)
      setClipped(ref.current.scrollWidth > ref.current.clientWidth);
  }, [text]);

  const node = (
    <Typography
      ref={ref}
      sx={{
        fontSize: "14px",
        color: "rgba(13,13,13,0.55)",
        lineHeight: "26px",
        whiteSpace: "nowrap",
        overflow: "hidden",
        textOverflow: "ellipsis",
        maxWidth: `${maxWidth}px`,
        ...sx,
      }}
    >
      {text || "—"}
    </Typography>
  );

  return clipped ? <CustomTooltip text={text}>{node}</CustomTooltip> : node;
};

export const SortIcon: React.FC<{ dir: "asc" | "desc" | "none" }> = ({
  dir,
}) => (
  <Box
    sx={{
      display: "flex",
      flexDirection: "column",
      gap: "1px",
      ml: "4px",
      flexShrink: 0,
    }}
  >
    <svg width="8" height="5" viewBox="0 0 8 5" fill="none">
      <path
        d="M1 4L4 1L7 4"
        stroke={dir === "asc" ? T.red : "#B0B7C3"}
        strokeWidth={dir === "asc" ? "1.8" : "1.2"}
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
    <svg width="8" height="5" viewBox="0 0 8 5" fill="none">
      <path
        d="M1 1L4 4L7 1"
        stroke={dir === "desc" ? T.red : "#B0B7C3"}
        strokeWidth={dir === "desc" ? "1.8" : "1.2"}
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  </Box>
);
