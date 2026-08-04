import React from "react";
import { FilterOptionItem } from "./ColumnFilterPopover";
import { CustomGridRender } from "../../../Model/Common";

export type SortDirection = "asc" | "desc" | "none";

export type CellRenderAs =
  | "text"
  | "link"
  | "boolean-dot"
  | "dot-text"
  | "version-dot"
  | "pill"
  | "avatar-name"
  | "actions"
  | "red-dot-badge"
  | "email"
  | "url"
  | "date"
  | "version-updgrade-button"
  | "dot-text-border-badge"
  | "avatar-email-stack";

export interface RowAction {
  label: string;
  icon?: React.ReactNode;
  color?: string;
  onClick: (row: any) => void;
}

export interface ColumnStyleRule {
  propertyName: string;
  renderAs?: CellRenderAs;
  width?: number;
  sticky?: boolean;
  sortable?: boolean;
  label?: string;
  colorMap?: Record<string, { bg: string; text: string }>;
  dotColorMap?: Record<string, string>;
  onLinkClick?: (row: any) => void;
  actions?: RowAction[];
  render?: (value: any, row: any, rowIdx: number) => React.ReactNode;
  upgradeCondition?: (row: any) => boolean;
  onUpgradeClick?: (row: any) => void;
}

export interface CustomColumnDef {
  key: string;
  label: string;
  width?: number;
  sticky?: boolean;
  sortable?: boolean;
  renderAs?: CellRenderAs;
  render?: (value: any, row: any, rowIdx: number) => React.ReactNode;
  colorMap?: Record<string, { bg: string; text: string }>;
  dotColorMap?: Record<string, string>;
  onLinkClick?: (row: any) => void;
  actions?: RowAction[];
  filterKey?: string;
  upgradeCondition?: (row: any) => boolean;
  onUpgradeClick?: (row: any) => void;
  filterVariant?: "checkbox" | "date";
}

export interface ApiGridRenderItem {
  propertyName: string;
  label?: string;
  show: boolean;
  order: number;
  type?: number;
  width?: number;
  sortable?: boolean;
}

export interface CustomTableProps {
  columns: CustomColumnDef[];
  data: any[];
  totalItems: number;
  currentPage: number;
  pageSize: number;
  onPageChange?: (page: number) => void;
  onSort?: (key: string, dir: SortDirection) => void;
  onRowClick?: (row: any) => void;
  onRowDoubleClick?: (row: any) => void;
  title?: string;
  fixColumn?: number;
  isDownload?: boolean;
  onDownload?: (row: any) => void;
  loading?: boolean;
  showSearch?: boolean;
  showSortCustomise?: boolean;
  searchValue?: string;
  onSearchChange?: (value: string) => void;
  onFetchFilterOptions?: (columnKey: string) => Promise<FilterOptionItem[]>;
  onApplyFilter?: (
    columnKey: string,
    selectedKeys: string[],
    isSortAscending?: boolean
  ) => void;
  currentSortBy?: string;
  currentSortAscending?: boolean;
  activeFilters?: Record<string, string[]>;
  highlightRowId?: any;
  gridRenderData?: CustomGridRender | undefined;
  onColumnsSaved?: () => void;
}

export interface RowActionsMenuProps {
  row: any;
  actions: RowAction[];
  open: boolean;
  setOpen: React.Dispatch<React.SetStateAction<boolean>>;
}

export interface RowActionsMenuHandle {
  positionAndOpen: (anchor: HTMLElement | DOMRect) => void;
}
