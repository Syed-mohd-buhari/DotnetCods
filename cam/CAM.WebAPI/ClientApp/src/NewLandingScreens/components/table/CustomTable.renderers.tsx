import React, { useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";
import {
  T,
  DEFAULT_PILL_COLORS,
  DEFAULT_DOT_COLORS,
} from "./CustomTable.tokens";
import { CustomColumnDef, RowActionsMenuHandle } from "./CustomTable.types";
import {
  Dot,
  PillBadge,
  AvatarCircle,
  CustomTooltip,
  TruncText,
} from "./CustomTable.atoms";
import { RowActionsMenu } from "./RowActionsMenu";
import { MdOutlineUpgrade } from "react-icons/md";

function resolveColor(
  value: string,
  map?: Record<string, { bg: string; text: string }>
) {
  return (
    map?.[String(value)] ??
    DEFAULT_PILL_COLORS[String(value)] ?? { bg: "#F3F4F6", text: "#374151" }
  );
}

function resolveDotColor(value: any, map?: Record<string, string>) {
  return map?.[String(value)] ?? DEFAULT_DOT_COLORS[String(value)] ?? "#9CA3AF";
}

function getEmailInitials(email: string) {
  const local = email.split("@")[0] ?? email;
  const parts = local.split(/[.\-_]+/).filter(Boolean);
  const initials = (
    (parts[0]?.[0] ?? "") + (parts[1]?.[0] ?? "")
  ).toUpperCase();
  return initials || local.slice(0, 2).toUpperCase();
}

export interface ActionsCellProps {
  col: CustomColumnDef;
  row: any;
  registerMenuRef: (handle: RowActionsMenuHandle | null) => void;
}

export const ActionsCell: React.FC<ActionsCellProps> = ({
  col,
  row,
  registerMenuRef,
}) => {
  const [open, setOpen] = useState(false);

  useEffect(() => {
    return () => registerMenuRef(null);
  }, []);

  if (!col.actions?.length) {
    return (
      <Box
        sx={{ display: "flex", alignItems: "center", justifyContent: "center" }}
      >
        <Typography
          sx={{
            fontSize: "18px",
            color: "#9CA3AF",
            letterSpacing: "1px",
            lineHeight: 1,
            cursor: "pointer",
            "&:hover": { color: T.black },
          }}
        >
          ···
        </Typography>
      </Box>
    );
  }

  return (
    <RowActionsMenu
      ref={(handle) => registerMenuRef(handle)}
      row={row}
      actions={col.actions}
      open={open}
      setOpen={setOpen}
    />
  );
};

export function builtInRender(
  col: CustomColumnDef,
  value: any,
  row: any,
  rowIdx: number,
  registerMenuRef?: (handle: RowActionsMenuHandle | null) => void
): React.ReactNode {
  const colWidth = col.width ?? 160;
  const innerW = colWidth - 24;
  const strVal = String(value ?? "");

  switch (col.renderAs) {
    case "link": {
      return (
        <CustomTooltip text={strVal}>
          <Typography
            component="span"
            onClick={(e: React.MouseEvent) => {
              e.stopPropagation();
              col.onLinkClick?.(row);
            }}
            sx={{
              fontSize: "14px",
              color: T.black,
              textDecoration: "underline",
              textDecorationColor: "rgba(0,0,0,0.3)",
              textUnderlineOffset: "3px",
              whiteSpace: "nowrap",
              overflow: "hidden",
              textOverflow: "ellipsis",
              display: "block",
              maxWidth: `${innerW}px`,
              cursor: "pointer",
              "&:hover": { color: T.red },
            }}
          >
            {strVal || "—"}
          </Typography>
        </CustomTooltip>
      );
    }

    case "boolean-dot": {
      const bool =
        value === true || value === "true" || value === 1 || value === "Yes";
      return (
        <Box sx={{ display: "flex", alignItems: "center", gap: "6px" }}>
          <Dot color={bool ? "#10B981" : T.red} />
          <Typography sx={{ fontSize: "14px", color: "rgba(13,13,13,0.7)" }}>
            {bool ? "Yes" : "No"}
          </Typography>
        </Box>
      );
    }

    case "dot-text": {
      return (
        <Box sx={{ display: "flex", alignItems: "center", gap: "6px" }}>
          <Dot color={resolveDotColor(strVal, col.dotColorMap)} />
          <Typography
            sx={{ fontSize: "14px", color: T.black, whiteSpace: "nowrap" }}
          >
            {strVal || "—"}
          </Typography>
        </Box>
      );
    }

    case "version-dot": {
      return (
        <Box sx={{ display: "flex", alignItems: "center", gap: "6px" }}>
          <Dot color="#10B981" />
          <Typography
            sx={{
              fontSize: "14px",
              color: "rgba(13,13,13,0.7)",
              whiteSpace: "nowrap",
            }}
          >
            {strVal || "—"}
          </Typography>
        </Box>
      );
    }

    case "version-updgrade-button": {
      const showUpgradeButton = col?.upgradeCondition
        ? !!col?.upgradeCondition(row)
        : true;

      return (
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            gap: "6px",
            justifyContent: "space-between",
          }}
        >
          <Typography
            sx={{
              fontSize: "14px",
              color: "rgba(13,13,13,0.7)",
              whiteSpace: "nowrap",
            }}
          >
            {strVal || "—"}
          </Typography>
          {showUpgradeButton && (
            <Box
              onClick={(e: React.MouseEvent) => {
                e.stopPropagation();
                col?.onUpgradeClick && col?.onUpgradeClick?.(row);
              }}
              sx={{
                width: "fit-content",
                height: 26,
                borderRadius: "3px",
                gap: "8px",
                opacity: 1,
                padding: "3px 5px",
                border: "1px solid #E5E5E5",
                display: "flex",
                alignItems: "center",
                cursor: "pointer",
                "&:hover": {
                  borderColor: "#10B981",
                  boxShadow:
                    "0 0 0 3px rgba(16, 185, 129, 0.15), 0 4px 12px rgba(16, 185, 129, 0.3)",
                },
              }}
            >
              <MdOutlineUpgrade color="#10B981" />
              <Typography
                sx={{
                  fontSize: "14px",
                  color: "rgba(13,13,13,0.7)",
                  whiteSpace: "nowrap",
                }}
              >
                {"Upgrade Version"}
              </Typography>
            </Box>
          )}
        </Box>
      );
    }

    case "pill": {
      const c = resolveColor(strVal, col.colorMap);
      return <PillBadge label={strVal || "—"} bg={c.bg} color={c.text} />;
    }

    case "avatar-name": {
      const parts = strVal.trim().split(/\s+/);
      const initials = (
        (parts[0]?.[0] ?? "") + (parts[1]?.[0] ?? "")
      ).toUpperCase();
      return (
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            gap: "8px",
            overflow: "hidden",
          }}
        >
          <AvatarCircle initials={initials} idx={0} />
          <TruncText text={strVal} maxWidth={innerW - 36} />
        </Box>
      );
    }

    case "avatar-email-stack": {
      const emails = strVal
        .split(/[,;]+/)
        .map((e) => e.trim())
        .filter(Boolean);

      if (emails.length === 0) {
        return (
          <Typography sx={{ fontSize: "14px", color: "rgba(13,13,13,0.4)" }}>
            —
          </Typography>
        );
      }

      if (emails.length === 1) {
        return (
          <CustomTooltip text={emails[0]}>
            <Box
              sx={{
                display: "flex",
                alignItems: "center",
                gap: "8px",
                overflow: "hidden",
              }}
            >
              <AvatarCircle initials={getEmailInitials(emails[0])} idx={0} />
              <Typography
                sx={{
                  fontSize: "14px",
                  color: "rgba(13,13,13,0.55)",
                  lineHeight: "26px",
                  whiteSpace: "nowrap",
                  overflow: "hidden",
                  textOverflow: "ellipsis",
                  maxWidth: `${innerW - 36}px`,
                }}
              >
                {emails[0] || "—"}
              </Typography>
            </Box>
          </CustomTooltip>
        );
      }

      const visibleCount = Math.min(emails.length, 3);
      const extraCount = emails.length - visibleCount;
      const visibleEmails = emails.slice(0, visibleCount);

      return (
        <CustomTooltip text={emails.join(", ")}>
          <Box sx={{ display: "flex", alignItems: "center" }}>
            {visibleEmails.map((email, i) => {
              const isOverflowSlot = extraCount > 0 && i === visibleCount - 1;
              return (
                <Box
                  key={`${email}-${i}`}
                  sx={{
                    width: 28,
                    height: 28,
                    borderRadius: "50%",
                    background: T.black,
                    color: T.white,
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    fontSize: "11px",
                    fontWeight: 600,
                    border: `2px solid ${T.white}`,
                    marginLeft: i === 0 ? 0 : "-8px",
                    zIndex: visibleCount - i,
                    flexShrink: 0,
                  }}
                >
                  {isOverflowSlot ? `+${extraCount}` : getEmailInitials(email)}
                </Box>
              );
            })}
          </Box>
        </CustomTooltip>
      );
    }

    case "email": {
      return (
        <Typography
          component="a"
          href={`mailto:${strVal}`}
          sx={{
            fontSize: "14px",
            color: T.black,
            textDecoration: "underline",
            textDecorationColor: "rgba(0,0,0,0.3)",
          }}
        >
          {strVal || "—"}
        </Typography>
      );
    }

    case "url": {
      const href = strVal.startsWith("http") ? strVal : `https://${strVal}`;
      return (
        <Typography
          component="a"
          href={href}
          target="_blank"
          rel="noopener noreferrer"
          sx={{ fontSize: "14px", color: T.red, textDecoration: "underline" }}
        >
          {strVal || "—"}
        </Typography>
      );
    }

    case "red-dot-badge": {
      return (
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            gap: "6px",
            overflow: "hidden",
          }}
        >
          <Dot color={T.red} size={8} />
          <TruncText
            text={strVal}
            maxWidth={innerW - 16}
            sx={{ color: T.black }}
          />
        </Box>
      );
    }
    case "dot-text-border-badge": {
      return (
        <Box
          sx={{
            width: "fit-content",
            height: 26,
            borderRadius: "3px",
            gap: "8px",
            opacity: 1,
            padding: "3px 5px",
            border: "1px solid #E5E5E5",
            display: "flex",
            alignItems: "center",
          }}
        >
          {strVal === "Live" || strVal === "On Support" ? (
            <Dot color={"#6DCD00"} size={12} />
          ) : (
            <Dot color={T.red} size={12} />
          )}
          <TruncText
            text={strVal}
            maxWidth={innerW - 16}
            sx={{ color: T.black }}
          />
        </Box>
      );
    }

    case "actions": {
      if (registerMenuRef) {
        return (
          <ActionsCell col={col} row={row} registerMenuRef={registerMenuRef} />
        );
      }
      return null;
    }

    default:
      return <TruncText text={strVal || "—"} maxWidth={innerW} />;
  }
}
