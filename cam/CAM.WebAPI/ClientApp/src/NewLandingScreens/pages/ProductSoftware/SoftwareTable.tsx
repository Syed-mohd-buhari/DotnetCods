import React, { useEffect, useMemo, useRef, useState } from "react";
import { Box } from "@mui/material";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../../../Redux/Store/rootStore";
import { FilterOptionItem } from "../../components/table/ColumnFilterPopover";
import {
  GetMajorSoftwareBuildGrid,
  GetFilterColumMajorSoftwareBuild,
} from "../../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildGridAction";
import setLoader from "../../../Redux/Action/LoaderAction";
import { useResourceTableCrud } from "../../../Hook/useResourceTableCrud";
import { useAuth } from "../../../Hook/useAuth";
import {
  MajorSoftwareBuildDtoGrid,
  MajorSoftwareBuildQueryObjectGrid,
} from "../../../Model/MajorSoftwareBuild";
import CustomTable from "../../components/table/CustomTable";
import {
  ColumnStyleRule,
  CustomColumnDef,
} from "../../components/table/CustomTable.types";
import { buildColumnsFromApi } from "../../components/table/CustomTable.utils";
import { GetMajorSoftwareBuildDownload } from "../../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildDownloadAction";
import { FilterValueDto } from "../../../Business/Common/CommonBusiness";
import { TbEdit } from "react-icons/tb";

const DEFAULT_QUERY: MajorSoftwareBuildQueryObjectGrid = {
  majorSoftwareBuildId: [],
  originalEquipmentManufacturer: [],
  softwareVersion: [],
  productNamesId: [],
  lastTimeBuyNew: undefined,
  lastTimeBuyUpgrades: undefined,
  lastTimeBuyExpansions: undefined,
  lastModified: undefined,
  endOfMaintenance: undefined,
  endOfsupport: undefined,
  generaAvailableDate: undefined,
  deliveryMethod: [],
  vulnerabilityStatus: [],
  operatingSystem: [],
  spareFieldsJson: [],
  globalSearchKeyword: "",
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

const SEARCH_DEBOUNCE_MS = 400;

interface SoftwareTableProps {
  title?: string;
  refreshKey?: number;
  onRowClick: (row: MajorSoftwareBuildDtoGrid) => void;
  onEdit: (row: MajorSoftwareBuildDtoGrid) => void;
  onDelete: (row: MajorSoftwareBuildDtoGrid) => void;
  onUpgrade: (row: MajorSoftwareBuildDtoGrid) => void;
  skipApiCall?: boolean;
  showSearch?: boolean;
  showSortCustomise?: boolean;
  highlightId?: number | string;
  onHighlightExpire?: () => void;
}

const SoftwareTable: React.FC<SoftwareTableProps> = ({
  title,
  refreshKey,
  onRowClick,
  onEdit,
  onDelete,
  onUpgrade,
  showSearch = true,
  showSortCustomise = true,
  highlightId,
  onHighlightExpire,
}) => {
  const [columns, setColumns] = useState<CustomColumnDef[]>([]);
  const [tableData, setTableData] = useState<MajorSoftwareBuildDtoGrid[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const searchDebounceRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const lastSearchedKeywordRef = useRef<string>("");

  const highlightIdRef = useRef<number | string | undefined>(highlightId);
  useEffect(() => {
    highlightIdRef.current = highlightId;
  }, [highlightId]);

  useEffect(() => {
    return () => {
      if (searchDebounceRef.current) {
        clearTimeout(searchDebounceRef.current);
      }
    };
  }, []);

  const clearHighlightIfDifferent = (row: any) => {
    const current = highlightIdRef.current;
    if (current === undefined || current === null) return;
    const rowId = row?.majorSoftwareBuildId ?? row?.id;
    if (rowId !== current) {
      onHighlightExpire?.();
    }
  };

  const handleRowClick = (row: MajorSoftwareBuildDtoGrid) => {
    clearHighlightIfDifferent(row);
    onRowClick(row);
  };
  const handleEdit = (row: MajorSoftwareBuildDtoGrid) => {
    clearHighlightIfDifferent(row);
    onEdit(row);
  };
  const handleDelete = (row: MajorSoftwareBuildDtoGrid) => {
    clearHighlightIfDifferent(row);
    onDelete(row);
  };
  const handleUpgrade = (row: MajorSoftwareBuildDtoGrid) => {
    clearHighlightIfDifferent(row);
    onUpgrade(row);
  };

  const handleRowDoubleClick = (row: MajorSoftwareBuildDtoGrid) => {
    handleEdit(row);
  };

  const GridDto = useSelector(
    (state: RootState) =>
      state.majorSoftwareBuildGridReducer.MajorSoftwareBuildGridResult
  );
  const externalRefreshDto = useSelector(
    (state: RootState) => state.externalRefreshReducer.refresh
  );

  const { isPermesso, pageSize } = useAuth();
  const {
    query,
    setQuery,
    loading: resourceLoading,
  } = useResourceTableCrud(
    DEFAULT_QUERY,
    isPermesso ? GetMajorSoftwareBuildGrid : undefined,
    false
  );

  useEffect(() => {
    setLoading(true);
  }, []);

  useEffect(() => {
    if (refreshKey === undefined || refreshKey === 0) return;
    setLoading(true);
    GetMajorSoftwareBuildGrid(query).then(() => setLoading(false));
  }, [refreshKey]);

  useEffect(() => {
    if (externalRefreshDto !== true) return;
    setLoading(true);
    GetMajorSoftwareBuildGrid(query).then(() => {
      setLoading(false);
      rootStore.dispatch({ type: "REFRESH", payload: false });
    });
  }, [externalRefreshDto]);

  useEffect(() => {
    if (!GridDto) return;

    setTableData(GridDto.items ?? []);
    setLoading(false);

    if (!GridDto.gridRender?.render) return;

    const styleRules: ColumnStyleRule[] = [
      {
        propertyName: "originalEquipmentManufacturer",
        label: "Equipment Manufacturer",
        width: 200,
      },
      {
        propertyName: "productName",
        width: 250,
        renderAs: "link",
        onLinkClick: (row) => handleRowClick(row as MajorSoftwareBuildDtoGrid),
      },
      {
        propertyName: "softwareVersion",
        width: 170,
        renderAs: "version-updgrade-button",
        onUpgradeClick: (row) =>
          handleUpgrade(row as MajorSoftwareBuildDtoGrid),
      },
      {
        propertyName: "designContact",
        label: "Software Product Owner",
        renderAs: "avatar-email-stack",
      },
      {
        propertyName: "criticalAssetType",
        label: "Software Type",
      },
      {
        propertyName: "isPlatform",
        width: 130,
        renderAs: "boolean-dot",
      },
      {
        propertyName: "deliveryMethod",
        width: 190,
        renderAs: "pill",
      },
      {
        propertyName: "lastModifiedBy",
        width: 200,
        renderAs: "avatar-name",
        label: "Owner",
      },
      {
        propertyName: "endOfMaintenanceValue",
        width: 180,
        label: "End of Maintenance",
      },
      {
        propertyName: "endOfsupportValue",
        width: 160,
        label: "End of Support",
      },
      {
        propertyName: "lastModifiedValue",
        width: 180,
        label: "Last Updated",
      },
    ];

    const actionsColumn: CustomColumnDef = {
      key: "__actions",
      label: "",
      width: 52,
      sortable: false,
      renderAs: "actions",
      actions: [
        {
          label: "Edit",
          icon: <TbEdit size={20} />,
          onClick: (row) => handleEdit(row as MajorSoftwareBuildDtoGrid),
        },
      ],
    };
    setColumns(
      buildColumnsFromApi(GridDto.gridRender.render, styleRules, [
        actionsColumn,
      ])
    );
  }, [GridDto]);

  const sortedTableData = useMemo(() => {
    if (highlightId === undefined || highlightId === null) return tableData;
    const idx = tableData.findIndex(
      (row: any) =>
        row.majorSoftwareBuildId === highlightId || row.id === highlightId
    );
    if (idx <= 0) return tableData;
    const copy = [...tableData];
    const [row] = copy.splice(idx, 1);
    copy.unshift(row);
    return copy;
  }, [tableData, highlightId]);

  const runQuery = (updated: MajorSoftwareBuildQueryObjectGrid) => {
    setTableData([]);
    setQuery(updated);
    setLoading(true);
    GetMajorSoftwareBuildGrid(updated).then(() => setLoading(false));
  };

  const goToPage = (page: number) => {
    runQuery({ ...query, page });
  };

  const handleFetchFilterOptions = async (
    columnKey: string
  ): Promise<FilterOptionItem[]> => {
    const res = await GetFilterColumMajorSoftwareBuild(columnKey, "", query);
    return (res?.filter ?? []).map((f: FilterValueDto) => ({
      key: f.value,
      label: f.text,
    }));
  };

  const handleApplyFilter = (
    columnKey: string,
    selectedKeys: string[],
    isSortAscending?: boolean
  ) => {
    const col = columns.find(
      (c) => c.key === columnKey || c.filterKey === columnKey
    );
    const sortPatch =
      isSortAscending !== undefined
        ? { sortBy: columnKey, isSortAscending }
        : {};

    if (col?.filterVariant === "date") {
      const [from, to] = selectedKeys;
      runQuery({
        ...query,
        ...sortPatch,
        page: 1,
        [`${columnKey}StartDate`]: from || undefined,
        [`${columnKey}EndDate`]: to || undefined,
        [`${columnKey}`]: {
          startDate: from || undefined,
          endDate: to || undefined,
        },
      } as MajorSoftwareBuildQueryObjectGrid);
      return;
    }

    runQuery({
      ...query,
      ...sortPatch,
      page: 1,
      [columnKey]: selectedKeys,
    } as MajorSoftwareBuildQueryObjectGrid);
  };

  const activeFilters = useMemo(() => {
    const map: Record<string, string[]> = {};
    Object.entries(query as Record<string, unknown>).forEach(([key, val]) => {
      if (Array.isArray(val) && val.length > 0) {
        map[key] = val as string[];
      }
    });
    return map;
  }, [query]);
  const handleSearchChange = (value: string) => {
    setSearchTerm(value);

    if (searchDebounceRef.current) {
      clearTimeout(searchDebounceRef.current);
    }

    searchDebounceRef.current = setTimeout(() => {
      const keyword = value.trim();

      if (keyword === lastSearchedKeywordRef.current) {
        return;
      }
      lastSearchedKeywordRef.current = keyword;

      setQuery({
        ...query,
        page: 1,
        globalSearchKeyword: keyword,
      } as MajorSoftwareBuildQueryObjectGrid);
    }, SEARCH_DEBOUNCE_MS);
  };

  const handleDownload = async () => {
    let result = await GetMajorSoftwareBuildDownload(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  return (
    <Box sx={{ margin: "24px" }}>
      <CustomTable
        title={title}
        columns={columns}
        data={sortedTableData}
        totalItems={GridDto?.totalItems ?? 0}
        currentPage={query?.page ?? 1}
        pageSize={pageSize}
        loading={loading || resourceLoading}
        onPageChange={goToPage}
        onRowClick={handleRowClick}
        onRowDoubleClick={handleRowDoubleClick}
        isDownload={true}
        onDownload={handleDownload}
        showSearch={showSearch}
        showSortCustomise={showSortCustomise}
        searchValue={searchTerm}
        onSearchChange={handleSearchChange}
        onFetchFilterOptions={handleFetchFilterOptions}
        onApplyFilter={handleApplyFilter}
        currentSortBy={query?.sortBy}
        currentSortAscending={query?.isSortAscending}
        activeFilters={activeFilters}
        highlightRowId={highlightId}
        gridRenderData={GridDto?.gridRender ?? undefined}
        onColumnsSaved={() => {
          setLoading(true);
          GetMajorSoftwareBuildGrid(query).then(() => setLoading(false));
        }}
      />
    </Box>
  );
};

export default SoftwareTable;
