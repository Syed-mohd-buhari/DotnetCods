import React, { useEffect, useRef, useState } from "react";
import { Box, InputBase, Tooltip, Typography } from "@mui/material";
import Skeleton from "@mui/material/Skeleton";
import { styled } from "@mui/material/styles";
import {
  FiChevronLeft,
  FiChevronRight,
  FiSearch,
  FiFilter,
  FiXCircle,
} from "react-icons/fi";

import { T } from "./CustomTable.tokens";
import { CustomTableProps, SortDirection } from "./CustomTable.types";
import DataRow from "./DataRow";
import ColumnFilterPopover, { FilterOptionItem } from "./ColumnFilterPopover";
import { MdOutlineFileDownload } from "react-icons/md";
import { AiOutlineEnter } from "react-icons/ai";
import ColumnCustomizePopover from "./ColumnCustomizePopover";

function buildStickyOffsets(
  cols: { key: string; sticky?: boolean; width?: number }[]
): Record<string, number> {
  const offsets: Record<string, number> = {};
  let acc = 0;
  for (const c of cols) {
    if (!c.sticky) break;
    offsets[c.key] = acc;
    acc += c.width ?? 160;
  }
  return offsets;
}

const TableCard = styled(Box)({
  background: T.white,
  borderRadius: "12px",
  boxShadow: T.shadow,
  display: "flex",
  flexDirection: "column",
  overflow: "hidden",
  width: "100%",
});

const ScrollArea = styled(Box)({
  overflowX: "auto",
  flex: 1,
  "&::-webkit-scrollbar": { height: "6px" },
  "&::-webkit-scrollbar-track": { background: "transparent" },
  "&::-webkit-scrollbar-thumb": {
    background: "rgba(0,0,0,0.15)",
    borderRadius: "4px",
  },
});

const PaginationRow = styled(Box)({
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
  gap: "24px",
  padding: "14px 24px",
  borderTop: `1px solid ${T.rowBorder}`,
  flexShrink: 0,
});

const PaginBtn = styled(Box)<{ disabled?: boolean }>(({ disabled }) => ({
  width: "28px",
  height: "28px",
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  cursor: disabled ? "not-allowed" : "pointer",
  opacity: disabled ? 0.35 : 1,
  borderRadius: "4px",
  "&:hover": { background: disabled ? "transparent" : "rgba(0,0,0,0.05)" },
}));

type FilterState = {
  columnKey: string;
  filterKey: string;
  label: string;
  anchorEl: HTMLElement;
};

const CustomTable: React.FC<CustomTableProps> = ({
  columns,
  data,
  totalItems,
  currentPage,
  pageSize,
  onPageChange,
  onRowClick,
  onRowDoubleClick,
  title,
  isDownload,
  onDownload,
  loading,
  showSearch = true,
  showSortCustomise = true,
  searchValue,
  onSearchChange,
  onFetchFilterOptions,
  onApplyFilter,
  currentSortBy,
  currentSortAscending,
  activeFilters,
  highlightRowId,
  gridRenderData,
  onColumnsSaved,
}) => {
  const [sortPopoverAnchor, setSortPopoverAnchor] =
    useState<HTMLElement | null>(null);
  const totalPages = Math.max(1, Math.ceil(totalItems / pageSize));
  const startItem = totalItems === 0 ? 0 : (currentPage - 1) * pageSize + 1;
  const endItem = Math.min(currentPage * pageSize, totalItems);
  const isPrevDisabled = currentPage <= 1;
  const isNextDisabled = currentPage >= totalPages;
  const stickyOffsets = buildStickyOffsets(columns);

  const [isFocused, setIsFocused] = useState(false);

  const [filterState, setFilterState] = useState<FilterState | null>(null);
  const [filterOptionsCache, setFilterOptionsCache] = useState<
    Record<string, FilterOptionItem[]>
  >({});
  const [filterOptionsLoading, setFilterOptionsLoading] = useState<
    Record<string, boolean>
  >({});
  const handleFilterIconClick = (
    e: React.MouseEvent<HTMLElement>,
    col: {
      key: string;
      label?: string;
      filterKey?: string;
      filterVariant?: "checkbox" | "date";
    }
  ) => {
    e.stopPropagation();
    const filterKey = col.filterKey ?? col.key;
    setFilterState({
      columnKey: col.key,
      filterKey,
      label: col.label ?? col.key,
      anchorEl: e.currentTarget,
    });

    if (col.filterVariant === "date") return;

    if (!filterOptionsCache[filterKey] && onFetchFilterOptions) {
      setFilterOptionsLoading((prev) => ({ ...prev, [filterKey]: true }));
      onFetchFilterOptions(filterKey)
        .then((opts) => {
          setFilterOptionsCache((prev) => ({
            ...prev,
            [filterKey]: opts ?? [],
          }));
        })
        .catch((err) => {
          console.error("Filter options fetch error", err);
          setFilterOptionsCache((prev) => ({ ...prev, [filterKey]: [] }));
        })
        .finally(() => {
          setFilterOptionsLoading((prev) => ({ ...prev, [filterKey]: false }));
        });
    }
  };

  const closeFilterPopover = () => setFilterState(null);

  const handleClearFilter = (
    e: React.MouseEvent<HTMLElement>,
    col: { key: string; filterKey?: string }
  ) => {
    e.stopPropagation();
    const filterKey = col.filterKey ?? col.key;
    onApplyFilter?.(filterKey, []);
    if (filterState?.filterKey === filterKey) {
      closeFilterPopover();
    }
  };

  const isColumnFiltered = (key: string) =>
    (activeFilters?.[key]?.length ?? 0) > 0;

  const isRowHighlighted = (row: any) =>
    highlightRowId !== undefined &&
    highlightRowId !== null &&
    (row?.majorSoftwareBuildId === highlightRowId ||
      row?.id === highlightRowId);

  const skeletonRowCount = Math.min(pageSize || 6, 8);

  const searchInputRef = useRef<HTMLInputElement | null>(null);

  useEffect(() => {
    if (!loading && showSearch) {
      searchInputRef.current?.focus();
      const len = searchInputRef.current?.value.length ?? 0;
      searchInputRef.current?.setSelectionRange(len, len);
    }
  }, [loading]);

  return (
    <TableCard>
      <Box
        sx={{
          px: "24px",
          pt: "16px",
          pb: "12px",
          borderBottom: `1px solid ${T.rowBorder}`,
          flexShrink: 0,
          textAlign: "left",
          display: "flex",
          flexWrap: "wrap",
          justifyContent: "space-between",
          alignItems: "center",
          rowGap: "12px",
          columnGap: "16px",
        }}
      >
        {title && (
          <Typography
            sx={{
              fontWeight: 700,
              fontSize: "18px",
              lineHeight: "28px",
              color: T.black,
              order: 1,
            }}
          >
            {title}
          </Typography>
        )}

        {(showSearch || showSortCustomise) && (
          <Box
            sx={{
              display: "flex",
              flexWrap: "wrap",
              flexDirection: "row",
              alignItems: "center",
              gap: "16px",
              minHeight: "32px",
              order: { xs: 3, sm: 3, md: 3, lg: 2 },
              width: { xs: "100%", sm: "100%", md: "100%", lg: "auto" },
            }}
          >
            {showSearch && (
              <Box
                sx={{
                  boxSizing: "border-box",
                  display: "flex",
                  flexDirection: "row",
                  alignItems: "center",
                  padding: "4px 16px",
                  gap: "8px",
                  width: {
                    xs: "100%",
                    sm: "260px",
                    md: "260px",
                    lg: "200px",
                  },
                  maxWidth: "100%",
                  height: "32px",
                  background: "#FFFFFF",
                  border: `1px solid ${isFocused ? "#E60000" : "#7E7E7E"}`,
                  borderRadius: "6px",
                  ...(isFocused && {
                    boxShadow: "0 0 0 1px #E60000",
                  }),
                }}
              >
                <FiSearch size={16} color="#0D0D0D" />
                <InputBase
                  inputRef={searchInputRef}
                  autoFocus
                  value={searchValue ?? ""}
                  onChange={(e) => onSearchChange?.(e.target.value)}
                  onFocus={() => setIsFocused(true)}
                  onBlur={() => setIsFocused(false)}
                  placeholder={"Search software"}
                  sx={{
                    width: "100%",
                    fontWeight: 400,
                    fontSize: "16px",
                    lineHeight: "28px",
                    "& input::placeholder": {
                      color: "rgba(13, 13, 13, 0.3)",
                      opacity: 1,
                    },
                  }}
                />
              </Box>
            )}

            {showSortCustomise &&
              (loading ? (
                <Skeleton
                  variant="rounded"
                  width={200}
                  height={32}
                  sx={{ borderRadius: "8px" }}
                />
              ) : (
                <Box
                  sx={{
                    position: "relative",
                    width: {
                      xs: "100%",
                      sm: "260px",
                      md: "260px",
                      lg: "200px",
                    },
                    maxWidth: "100%",
                  }}
                >
                  <Box
                    onClick={(e) => setSortPopoverAnchor(e.currentTarget)}
                    sx={{
                      boxSizing: "border-box",
                      display: "flex",
                      flexDirection: "row",
                      alignItems: "center",
                      padding: "5.5px 10px 6.5px",
                      gap: "10px",
                      width: "100%",
                      height: "32px",
                      background: "#FFFFFF",
                      border: "1px solid rgba(0, 0, 0, 0.2)",
                      borderRadius: "8px",
                      cursor: "pointer",
                    }}
                  >
                    <AiOutlineEnter size={16} color="#525866" />
                    <Typography
                      sx={{
                        fontFamily: "'Vodafone Rg', sans-serif",
                        fontWeight: 400,
                        fontSize: "14px",
                        lineHeight: "20px",
                        letterSpacing: "-0.084px",
                        color: "#525866",
                        whiteSpace: "nowrap",
                      }}
                    >
                      {"Sort and customise view"}
                    </Typography>
                  </Box>
                </Box>
              ))}
          </Box>
        )}

        {isDownload &&
          (loading ? (
            <Skeleton
              variant="circular"
              width={20}
              height={20}
              sx={{ order: { xs: 2, sm: 2, md: 2, lg: 3 } }}
            />
          ) : (
            <Tooltip
              title="Download to Excel"
              slotProps={{
                tooltip: {
                  sx: {
                    backgroundColor: "black",
                    color: "white",
                    fontSize: "12px",
                    borderRadius: "6px",
                    px: 2,
                    py: 1,
                  },
                },
              }}
            >
              <Box
                onClick={onDownload}
                sx={{
                  cursor: "pointer",
                  order: { xs: 2, sm: 2, md: 2, lg: 3 },
                }}
              >
                <MdOutlineFileDownload size={20} />
              </Box>
            </Tooltip>
          ))}
      </Box>

      <ScrollArea>
        <table
          style={{
            borderCollapse: "collapse",
            tableLayout: "fixed",
            width: "max-content",
            minWidth: "100%",
          }}
        >
          <colgroup>
            {columns.map((c) => (
              <col key={c.key} style={{ width: c.width ?? 160 }} />
            ))}
          </colgroup>

          <thead>
            <tr>
              {columns.map((col) => {
                const isSticky = !!col.sticky;

                return (
                  <th
                    key={col.key}
                    style={{
                      width: col.width ?? 160,
                      minWidth: col.width ?? 160,
                      position: isSticky ? "sticky" : "relative",
                      left: isSticky ? stickyOffsets[col.key] : undefined,
                      zIndex: isSticky ? 3 : 1,
                      background: T.headerBg,
                      padding: "10px 12px",
                      borderBottom: "1px solid rgba(0,0,0,0.08)",
                      whiteSpace: "nowrap",
                    }}
                  >
                    {loading ? (
                      <Skeleton
                        variant="text"
                        width="70%"
                        height={22}
                        animation="wave"
                      />
                    ) : (
                      <Box
                        sx={{ display: "flex", alignItems: "center", gap: 0.5 }}
                      >
                        <Typography
                          sx={{
                            fontSize: "14px",
                            color: T.headerText,
                            fontWeight: 500,
                          }}
                        >
                          {col.label}
                        </Typography>

                        {col.sortable &&
                          (isColumnFiltered(col.filterKey ?? col.key) ? (
                            <Box
                              sx={{ cursor: "pointer", display: "flex" }}
                              onClick={(e) => handleClearFilter(e, col)}
                            >
                              <FiXCircle size={13} color={T.red} />
                            </Box>
                          ) : (
                            <Box
                              sx={{ cursor: "pointer", display: "flex" }}
                              onClick={(e) => handleFilterIconClick(e, col)}
                            >
                              <FiFilter size={13} color="#9CA3AF" />
                            </Box>
                          ))}
                      </Box>
                    )}
                  </th>
                );
              })}
            </tr>
          </thead>

          <tbody>
            {loading ? (
              Array.from({ length: skeletonRowCount }).map((_, i) => (
                <tr key={`skeleton-row-${i}`}>
                  {columns.map((col) => (
                    <td
                      key={col.key}
                      style={{
                        width: col.width ?? 160,
                        minWidth: col.width ?? 160,
                        padding: "0 12px",
                        height: "52px",
                        borderBottom: `1px solid ${T.rowBorder}`,
                        verticalAlign: "middle",
                      }}
                    >
                      <Skeleton
                        animation="wave"
                        variant="text"
                        width="80%"
                        height={20}
                      />
                    </td>
                  ))}
                </tr>
              ))
            ) : data.length === 0 ? (
              <tr>
                <td
                  colSpan={columns.length}
                  style={{ textAlign: "center", padding: "48px 0" }}
                >
                  <Typography
                    sx={{ fontSize: "14px", color: "rgba(0,0,0,0.4)" }}
                  >
                    No records found.
                  </Typography>
                </td>
              </tr>
            ) : (
              data.map((row, rowIdx) => (
                <DataRow
                  key={row?.majorSoftwareBuildId ?? row?.id ?? rowIdx}
                  row={row}
                  rowIdx={rowIdx}
                  columns={columns}
                  stickyOffsets={stickyOffsets}
                  onRowClick={onRowClick}
                  onRowDoubleClick={onRowDoubleClick}
                  highlighted={isRowHighlighted(row)}
                />
              ))
            )}
          </tbody>
        </table>
      </ScrollArea>

      <PaginationRow>
        {loading ? (
          <>
            <Skeleton variant="circular" width={28} height={28} />
            <Skeleton variant="text" width={220} height={20} />
            <Skeleton variant="circular" width={28} height={28} />
          </>
        ) : (
          <>
            <PaginBtn
              disabled={isPrevDisabled}
              onClick={() => !isPrevDisabled && onPageChange?.(currentPage - 1)}
            >
              <FiChevronLeft
                style={{
                  fontSize: "15px",
                  color: isPrevDisabled ? "#ccc" : "#7D7D7D",
                }}
              />
            </PaginBtn>
            <Typography
              sx={{
                fontSize: "14px",
                color: "#757575",
                whiteSpace: "nowrap",
              }}
            >
              {`Showing ${startItem}–${endItem} of ${totalItems} records`}
            </Typography>
            <PaginBtn
              disabled={isNextDisabled}
              onClick={() => !isNextDisabled && onPageChange?.(currentPage + 1)}
            >
              <FiChevronRight
                style={{
                  fontSize: "15px",
                  color: isNextDisabled ? "#ccc" : "#7D7D7D",
                }}
              />
            </PaginBtn>
          </>
        )}
      </PaginationRow>
      {filterState && (
        <ColumnFilterPopover
          open={!!filterState}
          anchorEl={filterState.anchorEl}
          columnLabel={filterState.label}
          filterType={
            columns.find((c) => c.key === filterState.columnKey)
              ?.filterVariant === "date"
              ? "date"
              : "checkbox"
          }
          loadingOptions={!!filterOptionsLoading[filterState.filterKey]}
          options={filterOptionsCache[filterState.filterKey] ?? []}
          initialSelected={activeFilters?.[filterState.filterKey] ?? []}
          showOrder={
            columns.find((c) => c.key === filterState.columnKey)?.sortable ??
            true
          }
          initialSortAscending={
            currentSortBy === filterState.filterKey
              ? currentSortAscending
              : undefined
          }
          onClose={closeFilterPopover}
          onApply={(selected, isSortAscending) =>
            onApplyFilter?.(filterState.filterKey, selected, isSortAscending)
          }
        />
      )}
      {gridRenderData && (
        <ColumnCustomizePopover
          open={!!sortPopoverAnchor}
          anchorEl={sortPopoverAnchor}
          onClose={() => setSortPopoverAnchor(null)}
          renderGrid={gridRenderData}
          onSaved={onColumnsSaved}
        />
      )}
    </TableCard>
  );
};

export default CustomTable;
