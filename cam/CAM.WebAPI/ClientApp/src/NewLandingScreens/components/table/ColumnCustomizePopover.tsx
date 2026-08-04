import React, { useEffect, useMemo, useRef, useState } from "react";
import { Box, Divider, Typography, Popover } from "@mui/material";
import { FiEye, FiEyeOff } from "react-icons/fi";
import { TbSortAscending, TbSortDescending } from "react-icons/tb";
import LabelsDictionary from "../../../Constant/LabelsAndDescriptions.json";
import {
  CustomGridRender,
  RenderDetail,
  DataModalConfirm,
  stateConfirm,
} from "../../../Model/Common";
import { SaveGrid } from "../../../Model/CommonModels";
import {
  DeleteCustomGridRender,
  SaveCustomGridRender,
} from "../../../Redux/Action/Grid/SaveGridCustom";
import ModalConfirm from "../../../Components/ModalConfirm";
import { T } from "./CustomTable.tokens";

export type ColumnSortDirection = "asc" | "desc";

interface ColumnCustomizePopoverProps {
  open: boolean;
  anchorEl: HTMLElement | null;
  onClose: () => void;
  renderGrid: CustomGridRender | undefined;
  tab?: string;
  sortDirection?: ColumnSortDirection;
  onSortDirectionChange?: (dir: ColumnSortDirection) => void;
  /** Called after a successful Save or Reset Default so the parent can refetch columns/data. */
  onSaved?: () => void;
}

/**
 * Reusable "Sort and customise view" popover.
 * Reuses the same show/hide + order model and SaveCustomGridRender /
 * DeleteCustomGridRender APIs as SetupColumns, but with drag-and-drop
 * reordering instead of up/down buttons, and adds a (currently local-only,
 * no backend support yet) Ascending/Descending selector.
 */
const ColumnCustomizePopover: React.FC<ColumnCustomizePopoverProps> = ({
  open,
  anchorEl,
  onClose,
  renderGrid,
  tab,
  sortDirection = "asc",
  onSortDirectionChange,
  onSaved,
}) => {
  const [data, setData] = useState<RenderDetail[]>([]);
  const [changed, setChanged] = useState(false);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [localSortDirection, setLocalSortDirection] =
    useState<ColumnSortDirection>(sortDirection);
  const [dragOverIndex, setDragOverIndex] = useState<number | null>(null);
  const [isDragging, setIsDragging] = useState(false);
  const dragIndexRef = useRef<number | null>(null);
  const tabState = tab ?? "";

  useEffect(() => {
    if (!open) return;
    if (renderGrid) {
      const copy = renderGrid.render.map((x) => ({
        ...x,
        tab: x.tab ?? "",
        archive: x.archive ?? false,
        ignore: x.ignore ?? false,
      }));
      setData(copy);
    }
    setChanged(false);
    setLocalSortDirection(sortDirection);
    setConfirm(stateConfirm);
    setDragOverIndex(null);
    setIsDragging(false);
    dragIndexRef.current = null;
  }, [open, renderGrid]);

  const visibleRows = useMemo(
    () =>
      data
        .filter((x) => x.tab === tabState || x.tab === "")
        .sort((a, b) => a.order - b.order),
    [data, tabState]
  );

  const allSelected = useMemo(
    () => visibleRows.length > 0 && visibleRows.every((x) => x.show),
    [visibleRows]
  );

  const toSave = {
    className: renderGrid?.className,
    render: data,
  } as SaveGrid;

  const onToggleShow = (property: string | undefined) => {
    setChanged(true);
    setData((prev) => {
      const copy = [...prev];
      const index = copy.findIndex(
        (x) => x.propertyName === property && x.tab === tabState
      );
      if (index === -1) return prev;
      copy[index] = { ...copy[index], show: !copy[index].show, tab: tabState };
      return copy;
    });
  };

  const onSelectAllToggle = () => {
    setChanged(true);
    const next = !allSelected;
    setData((prev) =>
      prev.map((item) =>
        item.tab === tabState || item.tab === ""
          ? { ...item, show: next }
          : item
      )
    );
  };

  // ---- Drag and drop reordering ----
  const handleDragStart = (index: number) => {
    dragIndexRef.current = index;
    setIsDragging(true);
  };

  const handleDragOver = (
    e: React.DragEvent<HTMLDivElement>,
    index: number
  ) => {
    e.preventDefault();
    if (dragOverIndex !== index) setDragOverIndex(index);
  };

  const handleDragLeave = (index: number) => {
    setDragOverIndex((prev) => (prev === index ? null : prev));
  };

  const handleDrop = (index: number) => {
    const fromIndex = dragIndexRef.current;
    setDragOverIndex(null);
    dragIndexRef.current = null;
    setIsDragging(false);
    if (fromIndex === null || fromIndex === index) return;

    setChanged(true);
    setData((prev) => {
      const rows = [...visibleRows];
      const [moved] = rows.splice(fromIndex, 1);
      rows.splice(index, 0, moved);

      // Reassign sequential order (1..N) to the reordered visible rows only;
      // rows belonging to other tabs keep their own order numbering.
      const reOrderedMap = new Map(
        rows.map((r, i) => [r.propertyName, { ...r, order: i + 1 }])
      );

      return prev.map((item) =>
        reOrderedMap.has(item.propertyName)
          ? (reOrderedMap.get(item.propertyName) as RenderDetail)
          : item
      );
    });
  };

  const handleDragEnd = () => {
    dragIndexRef.current = null;
    setDragOverIndex(null);
    setIsDragging(false);
  };

  // ---- Footer actions ----
  const handleSave = () => {
    SaveCustomGridRender(toSave).then(() => {
      setChanged(false);
      onSaved?.();
      onClose();
    });
  };

  const handleResetDefault = () => {
    setConfirm({
      title: "Confirm",
      message:
        "Are you sure you want to delete your setup and restore to default?",
      button: "Reset",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => setConfirm(stateConfirm),
        confirm: () => {
          setConfirm(stateConfirm);
          DeleteCustomGridRender(renderGrid?.className ?? "").then(() => {
            setChanged(false);
            onSaved?.();
            onClose();
          });
        },
      },
    });
  };

  const handleCancel = () => {
    if (changed) {
      setConfirm({
        title: "Confirm",
        message: "Are you sure you want to quit? Unsaved changes will be lost",
        button: "Exit",
        item: 0,
        isOpen: true,
        actions: {
          cancel: () => setConfirm(stateConfirm),
          confirm: () => {
            setConfirm(stateConfirm);
            onClose();
          },
        },
      });
    } else {
      onClose();
    }
  };

  // NOTE: sort direction has no backend support yet — this only updates
  // local UI state so the control is ready to wire up once the API exists.
  const handleSortDirection = (dir: ColumnSortDirection) => {
    setLocalSortDirection(dir);
    onSortDirectionChange?.(dir);
  };

  return (
    <>
      <ModalConfirm data={confirm} />
      <Popover
        open={open}
        anchorEl={anchorEl}
        onClose={handleCancel}
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
        transformOrigin={{ vertical: "top", horizontal: "right" }}
        slotProps={{
          paper: {
            sx: {
              width: "278px",
              maxHeight: "80vh",
              padding: "20px",
              borderRadius: "10px",
              boxShadow: "0px 4px 37px rgba(0, 0, 0, 0.11)",
              display: "flex",
              flexDirection: "column",
              gap: "16px",
              mt: "8px",
            },
          },
        }}
      >
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            gap: "10px",
            maxHeight: "340px",
            overflowY: "auto",
            pr: "2px",
            mr: "-10px",
            "&::-webkit-scrollbar": { width: "6px" },
            "&::-webkit-scrollbar-thumb": {
              background: "rgba(0,0,0,0.15)",
              borderRadius: "4px",
            },
          }}
        >
          {visibleRows.map((item, index) => (
            <Box
              key={item.propertyName}
              draggable
              onDragStart={() => handleDragStart(index)}
              onDragOver={(e) => handleDragOver(e, index)}
              onDragLeave={() => handleDragLeave(index)}
              onDrop={() => handleDrop(index)}
              onDragEnd={handleDragEnd}
              sx={{
                display: "flex",
                flexDirection: "row",
                alignItems: "center",
                justifyContent: "space-between",
                width: "238px",
                height: "28px",
                px: "2px",
                borderRadius: "4px",
                cursor: isDragging ? "grabbing" : "grab",
                background:
                  dragOverIndex === index ? "rgba(0,0,0,0.05)" : "transparent",
                "&:hover .drag-handle": { opacity: 1 },
              }}
            >
              <Box
                sx={{
                  display: "flex",
                  alignItems: "center",
                  gap: "6px",
                  overflow: "hidden",
                  pointerEvents: "none",
                }}
              >
                <Typography
                  sx={{
                    fontFamily: "'Vodafone Rg', sans-serif",
                    fontSize: "16px",
                    lineHeight: "28px",
                    color: "#0D0D0D",
                    whiteSpace: "nowrap",
                    overflow: "hidden",
                    textOverflow: "ellipsis",
                  }}
                >
                  {item.propertyName && LabelsDictionary[item.propertyName]
                    ? LabelsDictionary[item.propertyName].Short
                    : item.propertyName}
                </Typography>
              </Box>

              <Box
                onClick={() => onToggleShow(item.propertyName)}
                sx={{
                  display: "flex",
                  cursor: "pointer",
                  width: "24px",
                  height: "24px",
                  alignItems: "center",
                  justifyContent: "center",
                  flexShrink: 0,
                }}
              >
                {item.show ? (
                  <FiEye size={18} color="#000" />
                ) : (
                  <FiEyeOff size={18} color="#000" />
                )}
              </Box>
            </Box>
          ))}
        </Box>

        <Divider sx={{ width: "238px", borderColor: "rgba(0,0,0,0.2)" }} />

        {/* <Box sx={{ display: "flex", flexDirection: "row", gap: "10px" }}>
          <Box
            onClick={() => handleSortDirection("asc")}
            sx={{
              display: "flex",
              alignItems: "center",
              gap: "10px",
              cursor: "pointer",
              width: "157px",
              height: "28px",
              color: localSortDirection === "asc" ? T.red : "#0D0D0D",
            }}
          >
            <Typography
              sx={{
                fontSize: "16px",
                color: "inherit",
                fontWeight: localSortDirection === "asc" ? 700 : 400,
              }}
            >
              Ascending
            </Typography>
            <TbSortAscending
              size={20}
              color={localSortDirection === "asc" ? T.red : "#000"}
            />
          </Box>

          <Box
            onClick={() => handleSortDirection("desc")}
            sx={{
              display: "flex",
              alignItems: "center",
              gap: "10px",
              cursor: "pointer",
              width: "157px",
              height: "28px",
              color: localSortDirection === "desc" ? T.red : "#0D0D0D",
            }}
          >
            <Typography
              sx={{
                fontSize: "16px",
                color: "inherit",
                fontWeight: localSortDirection === "desc" ? 700 : 400,
              }}
            >
              Descending
            </Typography>
            <TbSortDescending
              size={20}
              color={localSortDirection === "desc" ? T.red : "#000"}
            />
          </Box>
        </Box>

        <Divider sx={{ width: "238px", borderColor: "rgba(0,0,0,0.2)" }} /> */}

        <Box sx={{ display: "flex", justifyContent: "space-between" }}>
          <Typography
            onClick={onSelectAllToggle}
            sx={{
              fontSize: "14px",
              fontWeight: 700,
              color: T.black,
              cursor: "pointer",
              "&:hover": { textDecoration: "underline" },
            }}
          >
            {allSelected ? "Unselect All" : "Select All"}
          </Typography>
          <Typography
            onClick={handleResetDefault}
            sx={{
              fontSize: "14px",
              fontWeight: 700,
              color: T.black,
              cursor: "pointer",
              "&:hover": { textDecoration: "underline" },
            }}
          >
            Reset Default
          </Typography>
        </Box>

        <Box
          sx={{
            display: "flex",
            justifyContent: "flex-end",
            gap: "12px",
            pt: "12px",
            mt: "4px",
            borderTop: "1px solid rgba(0,0,0,0.1)",
          }}
        >
          <Box
            onClick={handleCancel}
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              px: "18px",
              height: "36px",
              border: "1px solid #000",
              borderRadius: "6px",
              cursor: "pointer",
              "&:hover": { background: "#F5F5F5" },
            }}
          >
            <Typography sx={{ fontSize: "14px", color: "#000" }}>
              Cancel
            </Typography>
          </Box>
          <Box
            onClick={handleSave}
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              px: "18px",
              height: "36px",
              background: T.red,
              borderRadius: "6px",
              cursor: "pointer",
              "&:hover": { background: "#C40000" },
            }}
          >
            <Typography sx={{ fontSize: "14px", color: "#FFF" }}>
              Save
            </Typography>
          </Box>
        </Box>
      </Popover>
    </>
  );
};

export default ColumnCustomizePopover;
