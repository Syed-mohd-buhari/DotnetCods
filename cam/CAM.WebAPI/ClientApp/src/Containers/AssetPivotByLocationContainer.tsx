import React, { useEffect, useState } from "react";
import { MultiSelect } from "react-multi-select-component";
import { useSelector } from "react-redux";
import Select from "react-select";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useGenerateLcmDbHardware } from "../Hook/ReportLcmDatabaseOverrideHook/useGenerateLcmDbHardware";
import { useAuth } from "../Hook/useAuth";
import {
  PATReportDto,
  ReportPATQueryObjectGrid,
} from "../Model/Report/PlannedActivityTrackerModel";

import {
  GetFilterColumReportPAT,
  GetReportPATAllExports,
  GetReportPATGrid,
  GetSWOem,
  GetVendorsOfPredefinedFilter,
} from "../Redux/Action/Report/PlannedActivityTrackerAction";
import { RootState } from "../Redux/Store/rootStore";
import { dictionaryToArray } from "./../Hook/Dictionary";
import Paginate from "../Components/PaginationComponent";
import { downloadXLSX } from "../Hook/Common";
import setLoader from "../Redux/Action/LoaderAction";
import { useTheme } from "../Context/ThemeContext";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { ButtonGroup, Dropdown, ToggleButton } from "react-bootstrap";
import {
  GetAssetPivotByLocationExport,
  GetAssetPivotByLocationGrid,
  GetDropdownData,
  GetFilterColumnAssetPivotByLocation,
} from "../Redux/Action/Report/AssetPivotByLocationAction";
import AssetPivotByLocationTable from "../screen/AssetPivotByLocation/AssetPivotByLocationTable";
import {
  AssetPivotByLocationDto,
  AssetPivotByLocationQueryObjectGrid,
} from "../Model/Report/AssetPivotByLocationModel";
import { DropdownInputComponent } from "../Components/FormField";

export let paginationQuery = {
  year: new Date().getFullYear(),
};

const FORMAT_ENUM = {
  0: "ALL SW Versions",
  1: "BLUEPRINT NFVI",
  2: "BLUEPRINT NFCI",
  3: "Ericsson Virtualized Bundles",
  4: "Huwaei Virtualized Bundles",
  5: "Nokia Virtualized Bundles",
};

export let paginationQueryPAT: AssetPivotByLocationQueryObjectGrid = {
  opCo: [],
  dcId: [],
  hardwareType: [],
  deploymentStatus: [],
  paImplementaionYear: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
};

//commit
export const getColor = (text: string): string => {
  switch (text) {
    case "expired":
      return "red";
    case "on expiration":
      return "orange";
    case "on support":
      return "green";
    default:
      return "";
  }
};

const AssetPivotByLocation: React.FC = (props) => {
  //PAGE
  const { readonly, KPIAdmin, admin, isPermesso, pageSize } = useAuth();

  const [data, setData] = useState<
    AssetPivotByLocationDto | undefined | null
  >();
  const [dropdownData, setDropdownData] = useState<any>(null);
  const [macroXlS, setMacroXlS] = useState<boolean>(false);
  const [selected, setSelected] = useState<
    { label: string; value: number }[] | undefined
  >(undefined);

  const [opcoOptions, setOpcoOptions] = useState<any>();
  const [opcoSelected, setOpcoSelected] = useState<any>();
  const [hardwareTypeOptions, setHardwareTypeOptions] = useState<any>();
  const [hardwareTypeSelected, setHardwareTypeSelected] = useState<any>();
  const [deploymentStatusOptions, setDeploymentStatusOptions] = useState<any>();
  const [deploymentStatusSelected, setDeploymentStatusSelected] =
    useState<any>();

  const { query, setQuery, next, back } = useResourceTableCrud(
    {
      ...paginationQueryPAT,
    },
    isPermesso ? GetAssetPivotByLocationGrid : undefined
  );
  const Grid = (state: RootState) =>
    state.AssetPivotByLocationGridReducer.AssetPivotByLocationGridResult;
  const GridDto = useSelector(Grid);

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQueryPAT.pageSize = pageSize;
  }, [pageSize]);

  const getFiltersData = (state: RootState) =>
    state.AssetPivotByLocationGridReducer.filter;
  const filterData = useSelector(getFiltersData);

  const addExtraFilters = (): AssetPivotByLocationQueryObjectGrid => {
    let newQuery = { ...query } as AssetPivotByLocationQueryObjectGrid;

    newQuery.opCo = opcoSelected ? [opcoSelected.key] : [];
    newQuery.hardwareType = hardwareTypeSelected
      ? [hardwareTypeSelected.key]
      : [];
    newQuery.deploymentStatus = deploymentStatusSelected
      ? [deploymentStatusSelected.key]
      : [];

    newQuery.page = 1;

    return newQuery;
  };

  const getDropdownData = async () => {
    let result = await GetDropdownData();
    if (result) {
      setDropdownData(result);
      setOpcoOptions(
        result.data.opco.data?.map((item: any) => {
          return {
            key: item.key,
            value: item.value,
          } as { key: number; value: string };
        })
      );
      setDeploymentStatusOptions(
        result.data.deploymentStatus?.map((item: any) => {
          return {
            key: item.key,
            value: item.value,
          } as { key: number; value: string };
        })
      );
      setHardwareTypeOptions(result.data.hardwareType);
    }
  };

  const onViewReport = () => {
    setQuery(addExtraFilters());

    // GetAssetPivotByLocationGrid({
    //   ...addExtraFilters(),
    //   page: 1,
    // });
  };

  const { darkMode } = useTheme();

  const customStyles = {
    option: (provided, state) => ({
      ...provided,
      backgroundColor: darkMode ? "blue" : "white", // Change 'blue' to your desired background color
      color: darkMode ? "white" : "black", // Change 'white' to your desired text color
    }),
  };

  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null) {
      setData(GridDto);
    }
  }, [GridDto]);

  useEffect(() => {
    if (isPermesso) {
      getDropdownData();
      // GetAssetPivotByLocationGrid(paginationQueryPAT);
    }
  }, [isPermesso]);

  const onDownloadToXLS = async () => {
    let result = await GetAssetPivotByLocationExport({
      ...addExtraFilters(),
      pageSize: GridDto?.totalItems,
      page: 1,
    });
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  return (
    <div className="pageContainer">
      <div className="headerPage row mx-0 justify-content-between position-relative">
        <div>
          <h3 className="voda-bold">Asset Pivot By Location</h3>
        </div>
      </div>
      <div className="row mt-1 mx-0">
        <div className="col-3">
          <DropdownInputComponent
            label={"Please select an Opco"}
            //placeholderText="Select"
            labelCSS="mb-0 text-left"
            inputCSS="labelForm voda-bold mb-2"
            isSearchable={true}
            isClearable={true}
            value={opcoSelected}
            options={opcoOptions}
            onChange={(e: any) => setOpcoSelected(e)}
          />
        </div>
        <div className="col-3">
          <DropdownInputComponent
            label={"Hardware Type"}
            //placeholderText="Select"
            labelCSS="mb-0 text-left"
            inputCSS="labelForm voda-bold mb-2"
            isSearchable={true}
            isClearable={true}
            value={hardwareTypeSelected}
            options={hardwareTypeOptions}
            onChange={(e: any) => setHardwareTypeSelected(e)}
          />
        </div>
        <div className="col-3">
          <DropdownInputComponent
            label={"Deployment Status"}
            //placeholderText="Select"
            labelCSS="mb-0 text-left"
            inputCSS="labelForm voda-bold mb-2"
            isSearchable={true}
            isClearable={true}
            value={deploymentStatusSelected}
            options={deploymentStatusOptions}
            onChange={(e: any) => setDeploymentStatusSelected(e)}
          />
        </div>
        <div className="col-3 pt-4">
          <div className="row">
            <div
              className="col-12 px-2"
              style={{ alignContent: "center", justifyItems: "self-end" }}
            >
              <div className="d-flex patStyle">
                <button
                  className="btnHeader clearBtn br-20 voda-bold"
                  onClick={() => onViewReport()}
                >
                  View Report
                </button>
                <button
                  className="pl-2 btnHeader clearBtn br-20 voda-bold "
                  type="button"
                  onClick={() => onDownloadToXLS()}
                >
                  Download to XLS
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
      {data && (
        <>
          <AssetPivotByLocationTable
            data={data}
            //viewReport={viewReport}
            pagination={query}
            action={{
              Filter: setQuery,
              // Filter: (query: AssetPivotByLocationQueryObjectGrid) => {
              //   setQuery(query);
              //   //GetAssetPivotByLocationGrid(query);
              // },
            }}
          />

          <Paginate
            pagination={{
              page: query.page,
              pageSize: query.pageSize,
            }}
            totalItems={GridDto?.totalItems}
            actions={{ next: next, back: back }}
          />
        </>
      )}
    </div>
  );
};

//coment
export default AssetPivotByLocation;
