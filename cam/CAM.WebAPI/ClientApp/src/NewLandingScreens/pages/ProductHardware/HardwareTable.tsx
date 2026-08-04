import React, { useEffect, useState } from "react";
import { Box } from "@mui/material";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../../../Redux/Store/rootStore";
import { GetMajorHardwareBuildGrid } from "../../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildGridAction";
import setLoader from "../../../Redux/Action/LoaderAction";
import { useResourceTableCrud } from "../../../Hook/useResourceTableCrud";
import { useAuth } from "../../../Hook/useAuth";
import {
  MajorHardwareBuildDtoGrid,
  MajorHardwareBuildQueryObjectGrid,
} from "../../../Model/MajorHardwareBuild";
import CustomTable from "../../components/table/CustomTable";
import {
  ColumnStyleRule,
  CustomColumnDef,
} from "../../components/table/CustomTable.types";
import { buildColumnsFromApi } from "../../components/table/CustomTable.utils";
import { GetMajorHardwareBuildReport } from "../../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildDownloadAction";

const DEFAULT_QUERY: MajorHardwareBuildQueryObjectGrid = {
  majorHardwareBuildId: [],
  name: "",
  originalEquipmentManufacturer: [],
  hardwareSolution: [],
  platform: [],
  hardwareType: [],
  otherHardwareInfo: [],
  lastTimeBuyNew: undefined,
  lastTimeBuyUpgrades: undefined,
  lastTimeBuyExpansions: undefined,
  lastModified: undefined,
  endOfMaintenance: undefined,
  endOfsupport: undefined,
  vulnerabilityStatus: [],
  spareFieldsJson: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  buildConstruction: [],
  principalId: undefined,
  principalIdList: [],
  proprietaryHardware: [],
  deleted: false,
  orphan: false,
  lastModifiedBy: [],
};

interface HardwareTableProps {
  title?: string;
  refreshKey?: number;
  onRowClick: (row: MajorHardwareBuildDtoGrid) => void;
  onEdit: (row: MajorHardwareBuildDtoGrid) => void;
  onDelete: (row: MajorHardwareBuildDtoGrid) => void;
}

const HardwareTable: React.FC<HardwareTableProps> = ({
  title,
  refreshKey,
  onRowClick,
  onEdit,
  onDelete,
}) => {
  const [columns, setColumns] = useState<CustomColumnDef[]>([]);
  const [tableData, setTableData] = useState<MajorHardwareBuildDtoGrid[]>([]);

  const GridDto = useSelector(
    (state: RootState) =>
      state.majorHardwareBuildGridReducer.MajorHardwareBuildGridResult
  );
  const externalRefreshDto = useSelector(
    (state: RootState) => state.externalRefreshReducer.refresh
  );

  const { isPermesso, pageSize } = useAuth();
  const { query, setQuery } = useResourceTableCrud(
    DEFAULT_QUERY,
    isPermesso ? GetMajorHardwareBuildGrid : undefined
  );

  useEffect(() => {
    if (refreshKey === undefined || refreshKey === 0) return;
    setLoader("ADD", "GetMajorHardwareBuildGrid");
    GetMajorHardwareBuildGrid(query).then(() =>
      setLoader("REMOVE", "GetMajorHardwareBuildGrid")
    );
  }, [refreshKey]);

  useEffect(() => {
    if (externalRefreshDto !== true) return;
    GetMajorHardwareBuildGrid(query).then(() => {
      setLoader("REMOVE", "GetMajorHardwareBuildGrid");
      rootStore.dispatch({ type: "REFRESH", payload: false });
    });
  }, [externalRefreshDto]);

  useEffect(() => {
    if (!GridDto) return;

    setTableData(GridDto.items ?? []);
    setLoader("REMOVE", "GetMajorHardwareBuildGrid");

    if (!GridDto.gridRender?.render) return;

    const styleRules: ColumnStyleRule[] = [
      {
        propertyName: "originalEquipmentManufacturer",
        label: "Equipment Manufacturer",
        width: 200,
      },
      {
        propertyName: "platform",
        width: 200,
        renderAs: "link",
        onLinkClick: (row) => onRowClick(row as MajorHardwareBuildDtoGrid),
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
          onClick: (row) => onEdit(row as MajorHardwareBuildDtoGrid),
        },
        {
          label: "Delete",
          color: "#E60000",
          onClick: (row) => onDelete(row as MajorHardwareBuildDtoGrid),
        },
      ],
    };
    setColumns(
      buildColumnsFromApi(GridDto.gridRender.render, styleRules, [
        actionsColumn,
      ])
    );
  }, [GridDto]);

  const goToPage = (page: number) => {
    setTableData([]);
    const updated = { ...query, page };
    setQuery(updated);
    setLoader("ADD", "GetMajorHardwareBuildGrid");
    GetMajorHardwareBuildGrid(updated).then(() =>
      setLoader("REMOVE", "GetMajorHardwareBuildGrid")
    );
  };

  const handleSort = (key: string, dir: "asc" | "desc" | "none") => {
    const updated = {
      ...query,
      page: 1,
      sortBy: key,
      isSortAscending: dir === "asc",
    };
    setQuery(updated);
    setLoader("ADD", "GetMajorHardwareBuildGrid");
    GetMajorHardwareBuildGrid(updated).then(() =>
      setLoader("REMOVE", "GetMajorHardwareBuildGrid")
    );
  };
  const handleDownload = async () => {
    let result = await GetMajorHardwareBuildReport(query);
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
        data={tableData}
        totalItems={GridDto?.totalItems ?? 0}
        currentPage={query?.page ?? 1}
        pageSize={pageSize}
        onPageChange={goToPage}
        onSort={handleSort}
        onRowClick={onRowClick}
        isDownload={true}
        onDownload={handleDownload}
      />
    </Box>
  );
};

export default HardwareTable;
