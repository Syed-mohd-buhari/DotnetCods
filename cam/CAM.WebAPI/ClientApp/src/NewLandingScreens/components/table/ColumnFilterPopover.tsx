import React, { useEffect, useMemo, useState } from "react";
import {
  Box,
  Popover,
  InputBase,
  Checkbox,
  Typography,
  Button,
} from "@mui/material";
import { FiSearch, FiCalendar } from "react-icons/fi";
import { T } from "./CustomTable.tokens";
import { TbSortAscending, TbSortDescending } from "react-icons/tb";
import { ColumnSortDirection } from "./ColumnCustomizePopover";

export interface FilterOptionItem {
  key: string;
  label: string;
}

export interface ColumnFilterPopoverProps {
  open: boolean;
  anchorEl: HTMLElement | null;
  columnLabel: string;
  loadingOptions: boolean;
  options: FilterOptionItem[];
  initialSelected: string[];
  onClose: () => void;
  onApply: (selectedKeys: string[], isSortAscending?: boolean) => void;
  filterType?: "checkbox" | "date";
  /** Whether to show the ORDER (Ascending/Descending) section for this column. */
  showOrder?: boolean;
  /** true = ascending, false = descending, undefined = no order currently applied. */
  initialSortAscending?: boolean;
}

const dateInputSx = {
  boxSizing: "border-box" as const,
  display: "flex",
  alignItems: "center",
  padding: "4px 12px",
  gap: "8px",
  width: "100%",
  height: "36px",
  background: "#FFFFFF",
  border: "1px solid rgba(0,0,0,0.15)",
  borderRadius: "6px",
};

const orderButtonSx = (isSelected: boolean) => ({
  flex: 1,
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  height: "36px",
  borderRadius: "8px",
  cursor: "pointer",
  fontSize: "13px",
  fontWeight: 600,
  border: isSelected ? "1px solid #333333" : "1px solid rgba(0,0,0,0.15)",
  background: isSelected ? "#333333" : "#FFFFFF",
  color: isSelected ? "#FFFFFF" : "rgba(13,13,13,0.85)",
  transition: "background 0.15s, color 0.15s, border-color 0.15s",
  userSelect: "none" as const,
  gap: "2px",
});

const ColumnFilterPopover: React.FC<ColumnFilterPopoverProps> = ({
  open,
  anchorEl,
  columnLabel,
  loadingOptions,
  options,
  initialSelected,
  onClose,
  onApply,
  filterType,
  showOrder = true,
  initialSortAscending = undefined,
}) => {
  const [search, setSearch] = useState("");
  const [isFocused, setIsFocused] = useState(false);
  const [selected, setSelected] = useState<Set<string>>(
    () => new Set(initialSelected)
  );

  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");

  const [sortAscending, setSortAscending] = useState<boolean | undefined>(
    undefined
  );

  useEffect(() => {
    if (open) {
      setSelected(new Set(initialSelected));
      setSearch("");
      setSortAscending(initialSortAscending);
      if (filterType === "date") {
        setFromDate(initialSelected[0] ?? "");
        setToDate(initialSelected[1] ?? "");
      }
    }
  }, [open, anchorEl]);

  const filteredOptions = useMemo(
    () =>
      options.filter((o) =>
        o.label?.toLowerCase().includes(search?.toLowerCase())
      ),
    [options, search]
  );

  const allFilteredSelected =
    filteredOptions.length > 0 &&
    filteredOptions.every((o) => selected.has(o.key));

  const toggleOption = (key: string) => {
    setSelected((prev) => {
      const next = new Set(prev);
      if (next.has(key)) next.delete(key);
      else next.add(key);
      return next;
    });
  };

  const toggleSelectAll = () => {
    setSelected((prev) => {
      const next = new Set(prev);
      if (allFilteredSelected) {
        filteredOptions.forEach((o) => next.delete(o.key));
      } else {
        filteredOptions.forEach((o) => next.add(o.key));
      }
      return next;
    });
  };

  const handleSortDirection = (dir: "asc" | "desc") => {
    setSortAscending(dir === "asc");
  };

  const changeHandler = (e: any, target: string) => {
    const dateUpdated = new Date(e);
    const DateString = ` ${dateUpdated.getFullYear()}/${
      dateUpdated.getMonth() + 1
    }/${dateUpdated.getDate()}`;
    if (target === "from") {
      setFromDate(DateString);
    }

    if (target === "to") {
      setToDate(DateString);
    }
  };

  const handleOk = () => {
    if (filterType === "date") {
      onApply([fromDate, toDate], sortAscending);
    } else {
      onApply(Array.from(selected), sortAscending);
    }
    onClose();
  };

  return (
    <Popover
      open={open}
      anchorEl={anchorEl}
      onClose={onClose}
      anchorOrigin={{ vertical: "bottom", horizontal: "left" }}
      transformOrigin={{ vertical: "top", horizontal: "left" }}
      slotProps={{
        paper: {
          sx: {
            width: "280px",
            borderRadius: "10px",
            boxShadow: T.shadow,
            marginTop: "6px",
          },
        },
      }}
    >
      {showOrder && (
        <Box sx={{ padding: "16px 16px 0" }}>
          <Typography
            sx={{
              fontSize: "12px",
              fontWeight: 700,
              letterSpacing: "0.06em",
              color: "rgba(13,13,13,0.5)",
              marginBottom: "10px",
            }}
          >
            ORDER
          </Typography>

          <Box sx={{ display: "flex", gap: "10px", marginBottom: "16px" }}>
            <Box
              onClick={() => handleSortDirection("asc")}
              sx={orderButtonSx(sortAscending === true)}
            >
              <Typography
                sx={{
                  fontSize: "16px",
                  color: "inherit",
                  fontWeight: 700,
                }}
              >
                Ascending
              </Typography>
              <TbSortAscending
                size={20}
                color={sortAscending ? T.white : "#000"}
              />
            </Box>

            <Box
              onClick={() => handleSortDirection("desc")}
              sx={orderButtonSx(sortAscending === false)}
            >
              <Typography
                sx={{
                  fontSize: "16px",
                  color: "inherit",
                  fontWeight: 700,
                }}
              >
                Descending
              </Typography>
              <TbSortDescending
                size={20}
                color={!sortAscending ? T.white : "#000"}
              />
            </Box>
          </Box>
        </Box>
      )}

      {filterType === "date" ? (
        <Box sx={{ padding: showOrder ? "0 16px 16px" : "16px" }}>
          <Typography
            sx={{ fontSize: "13px", color: "rgba(13,13,13,0.6)", mb: "6px" }}
          >
            From
          </Typography>
          <Box sx={{ ...dateInputSx, mb: "16px" }}>
            <InputBase
              type="date"
              value={fromDate}
              onChange={(e) => setFromDate(e.target.value)}
              sx={{ width: "100%", fontSize: "14px" }}
            />
            <FiCalendar size={16} color="rgba(0,0,0,0.4)" />
          </Box>

          <Typography
            sx={{ fontSize: "13px", color: "rgba(13,13,13,0.6)", mb: "6px" }}
          >
            Up To
          </Typography>
          <Box sx={dateInputSx}>
            <InputBase
              type="date"
              value={toDate}
              onChange={(e) => setToDate(e.target.value)}
              sx={{ width: "100%", fontSize: "14px" }}
            />
            <FiCalendar size={16} color="rgba(0,0,0,0.4)" />
          </Box>
        </Box>
      ) : (
        <Box sx={{ padding: showOrder ? "0 16px 16px" : "16px" }}>
          <Typography
            sx={{
              fontSize: "12px",
              fontWeight: 700,
              letterSpacing: "0.06em",
              color: "rgba(13,13,13,0.5)",
              marginBottom: "10px",
            }}
          >
            FILTERS
          </Typography>

          <Box
            sx={{
              boxSizing: "border-box",
              display: "flex",
              alignItems: "center",
              padding: "4px 12px",
              gap: "8px",
              width: "100%",
              height: "36px",
              background: "#FFFFFF",
              border: `1px solid ${isFocused ? T.red : "rgba(0,0,0,0.15)"}`,
              borderRadius: "6px",
              marginBottom: "12px",
              ...(isFocused && { boxShadow: `0 0 0 1px ${T.red}` }),
            }}
          >
            <FiSearch size={15} color="rgba(0,0,0,0.4)" />
            <InputBase
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              onFocus={() => setIsFocused(true)}
              onBlur={() => setIsFocused(false)}
              placeholder="Search"
              sx={{
                width: "100%",
                fontSize: "14px",
                "& input::placeholder": {
                  color: "rgba(13,13,13,0.35)",
                  opacity: 1,
                },
              }}
            />
          </Box>

          <Box
            sx={{
              maxHeight: "220px",
              overflowY: "auto",
              display: "flex",
              flexDirection: "column",
              "&::-webkit-scrollbar": { width: "6px" },
              "&::-webkit-scrollbar-track": { background: "transparent" },
              "&::-webkit-scrollbar-thumb": {
                background: "rgba(0,0,0,0.2)",
                borderRadius: "4px",
              },
            }}
          >
            {loadingOptions ? (
              <Typography
                sx={{
                  fontSize: "14px",
                  color: "rgba(0,0,0,0.4)",
                  padding: "8px 4px",
                }}
              >
                Loading...
              </Typography>
            ) : filteredOptions.length === 0 ? (
              <Typography
                sx={{
                  fontSize: "14px",
                  color: "rgba(0,0,0,0.4)",
                  padding: "8px 4px",
                }}
              >
                No options found.
              </Typography>
            ) : (
              <>
                <Box
                  onClick={toggleSelectAll}
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    gap: "6px",
                    cursor: "pointer",
                    padding: "4px 0",
                  }}
                >
                  <Checkbox
                    size="small"
                    checked={allFilteredSelected}
                    sx={{
                      padding: "4px",
                      color: "rgba(0,0,0,0.3)",
                      "&.Mui-checked": { color: T.red },
                    }}
                  />
                  <Typography
                    sx={{ fontSize: "14px", fontWeight: 700, color: T.black }}
                  >
                    Select All
                  </Typography>
                </Box>

                {filteredOptions.map((opt) => (
                  <Box
                    key={opt.key}
                    onClick={() => toggleOption(opt.key)}
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      gap: "6px",
                      cursor: "pointer",
                      padding: "4px 0",
                    }}
                  >
                    <Checkbox
                      size="small"
                      checked={selected.has(opt.key)}
                      sx={{
                        padding: "4px",
                        color: "rgba(0,0,0,0.3)",
                        "&.Mui-checked": { color: T.red },
                      }}
                    />
                    <Typography
                      sx={{ fontSize: "14px", fontWeight: 700, color: T.black }}
                    >
                      {opt.label}
                    </Typography>
                  </Box>
                ))}
              </>
            )}
          </Box>
        </Box>
      )}

      <Box
        sx={{
          display: "flex",
          gap: "12px",
          padding: "12px 16px 16px",
          borderTop: "1px solid rgba(0,0,0,0.08)",
        }}
      >
        <Button
          onClick={onClose}
          fullWidth
          sx={{
            textTransform: "none",
            fontWeight: 700,
            color: T.black,
            border: "1px solid rgba(0,0,0,0.15)",
            borderRadius: "8px",
            "&:hover": { background: "rgba(0,0,0,0.03)" },
          }}
        >
          Close
        </Button>
        <Button
          onClick={handleOk}
          fullWidth
          sx={{
            textTransform: "none",
            fontWeight: 700,
            color: "#FFFFFF",
            background: T.red,
            borderRadius: "8px",
            "&:hover": { background: T.red },
          }}
        >
          OK
        </Button>
      </Box>
    </Popover>
  );
};

export default ColumnFilterPopover;
