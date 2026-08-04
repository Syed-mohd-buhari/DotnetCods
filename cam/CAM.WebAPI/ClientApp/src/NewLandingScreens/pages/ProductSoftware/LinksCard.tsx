import React from "react";
import { Box, Typography } from "@mui/material";
import { T } from "../../utils/styled";
import { safeNumber } from "../../../Hook/Common";
import { Link } from "react-router-dom";

export interface LinkItem {
  text: string;
  value?: string;
}

interface LinkSectionProps {
  title: string;
  items: LinkItem[];
  onItemClick?: (item: LinkItem) => void;
  emptyLabel?: string;
  minHeight?: string;
}

const LinkSection: React.FC<LinkSectionProps> = ({
  title,
  items,
  onItemClick,
  emptyLabel = "No Links",
  minHeight = "76px",
}) => {
  return (
    <Box
      sx={{
        background: "#F4F6F9",
        borderRadius: "10px",
        p: "8px",
        display: "flex",
        flexDirection: "column",
        gap: "16px",
      }}
    >
      <Box
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <Typography sx={{ fontWeight: 700, fontSize: "14px", color: T.black }}>
          {title}
        </Typography>
        <Box
          sx={{
            background: "rgba(0,0,0,0.2)",
            borderRadius: "2px",
            p: "6px",
            minWidth: "24px",
            height: "20px",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
          }}
        >
          <Typography
            sx={{
              color: "#000",
              lineHeight: "9px",
              fontSize: "12px",
            }}
          >
            {items?.length ?? 0}
          </Typography>
        </Box>
      </Box>

      <Box
        sx={{
          minHeight,
          gap: "8px",
          width: "100%",
          display: "flex",
          flexDirection: "row",
          flexWrap: "wrap",
          alignContent: "flex-start",
        }}
      >
        {items && items.length > 0 ? (
          items.map((item, idx) => (
            <Typography
              key={item.value ?? `${item.text}-${idx}`}
              component="span"
              onClick={(e: React.MouseEvent) => {
                e.stopPropagation();
                onItemClick?.(item);
              }}
              sx={{
                fontSize: "14px",
                color: "#00000080",
                textDecoration: "underline",
                textDecorationColor: "#00000080",
                textUnderlineOffset: "4px",
                whiteSpace: "nowrap",
                overflow: "hidden",
                textOverflow: "ellipsis",
                display: "block",
                cursor: "pointer",
                padding: "4px",
                height: "fit-content",
                borderRadius: "4px",
                backgroundColor: "transparent",
                transition: "background-color 300ms ease-out",
                "&:hover": {
                  backgroundColor: "#0000000d",
                },
              }}
            >
              <Link
                to={{
                  pathname: "/lcmengineering",
                }}
                state={{
                  lcmIds: item.value
                    ?.split(",")
                    ?.map((value) => safeNumber(value)),
                  tab: "overview",
                  prevPage: "overview",
                }}
              >
                {item.text}
              </Link>
            </Typography>
          ))
        ) : (
          <Typography sx={{ fontSize: "14px", color: "#00000080" }}>
            {emptyLabel}
          </Typography>
        )}
      </Box>
    </Box>
  );
};

export interface LinksCardProps {
  hardwareLinks: LinkItem[];
  servicesLinks: LinkItem[];
  onHardwareItemClick?: (item: LinkItem) => void;
  onServicesItemClick?: (item: LinkItem) => void;
}

const LinksCard: React.FC<LinksCardProps> = ({
  hardwareLinks,
  servicesLinks,
  onHardwareItemClick,
  onServicesItemClick,
}) => {
  return (
    <Box
      sx={{
        width: "437px",
        background: "#FFFFFF",
        borderRadius: "12px",
        boxShadow: T.shadow,
        padding: "0 0 16px",
        display: "flex",
        flexDirection: "column",
      }}
    >
      <Box
        sx={{
          p: "16px 18px 8px",
          borderBottom: "1px solid #E5E5E5",
          textAlign: "left",
        }}
      >
        <Typography sx={{ fontWeight: 700, fontSize: "16px", color: T.black }}>
          Links
        </Typography>
      </Box>

      <Box
        sx={{
          p: "8px",
          display: "flex",
          flexDirection: "column",
          gap: "8px",
        }}
      >
        <LinkSection
          title="Linked Services"
          items={servicesLinks}
          onItemClick={onServicesItemClick}
          minHeight="140px"
        />
        <LinkSection
          title="Linked Hardware"
          items={hardwareLinks}
          onItemClick={onHardwareItemClick}
          minHeight="76px"
        />
      </Box>
    </Box>
  );
};

export default LinksCard;
