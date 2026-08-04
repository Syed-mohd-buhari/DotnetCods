import React, { useEffect, useRef, useState } from "react";
import { Box, Typography } from "@mui/material";
import { createPortal } from "react-dom";
import { T, ACTIONS_TRIGGER_ATTR } from "./CustomTable.tokens";
import { RowActionsMenuProps, RowActionsMenuHandle } from "./CustomTable.types";

export const RowActionsMenu = React.forwardRef<
  RowActionsMenuHandle,
  RowActionsMenuProps
>(({ row, actions, open, setOpen }, ref) => {
  const [menuPos, setMenuPos] = useState({ top: 0, left: 0 });
  const triggerRef = useRef<HTMLDivElement>(null);
  const menuRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!open) return;
    const handler = (e: MouseEvent) => {
      if (
        menuRef.current &&
        !menuRef.current.contains(e.target as Node) &&
        triggerRef.current &&
        !triggerRef.current.contains(e.target as Node)
      )
        setOpen(false);
    };
    document.addEventListener("mousedown", handler);
    return () => document.removeEventListener("mousedown", handler);
  }, [open, setOpen]);

  useEffect(() => {
    if (!open) return;
    const handler = (e: KeyboardEvent) => {
      if (e.key === "Escape") setOpen(false);
    };
    document.addEventListener("keydown", handler);
    return () => document.removeEventListener("keydown", handler);
  }, [open, setOpen]);

  const positionAndOpen = (anchor: HTMLElement | DOMRect) => {
    const rect =
      anchor instanceof HTMLElement ? anchor.getBoundingClientRect() : anchor;

    const menuH = actions.length * 44 + 24;
    const menuW = 210;

    const rawTop =
      window.innerHeight - rect.bottom < menuH + 8
        ? rect.top - menuH - 4
        : rect.bottom + 4;
    const rawLeft =
      window.innerWidth - rect.left < menuW + 8
        ? rect.right - menuW
        : rect.left;

    const top = Math.min(Math.max(8, rawTop), window.innerHeight - menuH - 8);
    const left = Math.min(Math.max(8, rawLeft), window.innerWidth - menuW - 8);

    setMenuPos({ top, left });
    setOpen(true);
  };

  React.useImperativeHandle(ref, () => ({ positionAndOpen }));

  const openFromTrigger = (e: React.MouseEvent) => {
    e.stopPropagation();
    if (!triggerRef.current) return;
    positionAndOpen(triggerRef.current);
  };
  if (actions.length === 1) {
    const [action] = actions;
    return (
      <Box
        onClick={(e) => {
          e.stopPropagation();
          action.onClick(row);
        }}
        sx={{
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          width: "28px",
          height: "28px",
          borderRadius: "6px",
          cursor: "pointer",
          userSelect: "none",
          transition: "background 0.15s",
          color: action.color ?? "#6B7280",
          "&:hover": { background: "#ECEEF2" },
        }}
      >
        {action.icon ?? (
          <Typography sx={{ fontSize: "14px", color: action.color ?? T.black }}>
            {action.label}
          </Typography>
        )}
      </Box>
    );
  }

  return (
    <>
      <Box
        ref={triggerRef}
        {...{ [ACTIONS_TRIGGER_ATTR]: "true" }}
        onClick={openFromTrigger}
        sx={{
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          width: "28px",
          height: "28px",
          borderRadius: "6px",
          cursor: "pointer",
          userSelect: "none",
          transition: "background 0.15s",
          background: open ? "#ECEEF2" : "transparent",
          "&:hover": { background: "#ECEEF2" },
        }}
      >
        <Typography
          sx={{
            fontSize: "16px",
            color: "#6B7280",
            letterSpacing: "2px",
            lineHeight: 1,
            fontWeight: 700,
          }}
        >
          ···
        </Typography>
      </Box>

      {open &&
        createPortal(
          <Box
            ref={menuRef}
            sx={{
              position: "fixed",
              top: menuPos.top,
              left: menuPos.left,
              zIndex: 9999,
              background: T.white,
              borderRadius: "10px",
              boxShadow: T.menuShadow,
              border: "1px solid rgba(0,0,0,0.07)",
              minWidth: "210px",
              pt: "8px",
              overflow: "hidden",
            }}
          >
            {actions.map((action, i) => {
              const isDestructive =
                action.color === T.red || action.color === "#E60000";
              const showDivider = isDestructive && i > 0;
              return (
                <React.Fragment key={i}>
                  {showDivider && (
                    <Box
                      sx={{ height: "1px", background: "rgba(0,0,0,0.07)" }}
                    />
                  )}
                  <Box
                    onClick={(e) => {
                      e.stopPropagation();
                      setOpen(false);
                      action.onClick(row);
                    }}
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      gap: "10px",
                      px: "20px",
                      py: "11px",
                      cursor: "pointer",
                      transition: "background 0.12s",
                      "&:hover": { background: "#F5F7FA" },
                    }}
                  >
                    {action.icon && (
                      <Box
                        sx={{
                          display: "flex",
                          alignItems: "center",
                          color: action.color ?? T.black,
                          fontSize: "15px",
                          flexShrink: 0,
                        }}
                      >
                        {action.icon}
                      </Box>
                    )}
                    <Typography
                      sx={{
                        fontSize: "14px",
                        color: action.color ?? T.black,
                        lineHeight: "22px",
                      }}
                    >
                      {action.label}
                    </Typography>
                  </Box>
                </React.Fragment>
              );
            })}
          </Box>,
          document.body
        )}
    </>
  );
});

RowActionsMenu.displayName = "RowActionsMenu";
