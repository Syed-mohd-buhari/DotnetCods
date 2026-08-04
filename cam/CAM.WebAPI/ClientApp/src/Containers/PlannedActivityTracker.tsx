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
import PATReportTable from "../screen/PlannedActivityTracker/PATReport";
import { dictionaryToArray } from "./../Hook/Dictionary";
import Paginate from "../Components/PaginationComponent";
import { downloadXLSX } from "../Hook/Common";
import setLoader from "../Redux/Action/LoaderAction";
import { useTheme } from "../Context/ThemeContext";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { ButtonGroup, Dropdown, ToggleButton } from "react-bootstrap";

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

export let paginationQueryPAT: ReportPATQueryObjectGrid = {
  dcfId: [],
  opCoId: [],
  verticalNameId: [],
  vendorIds: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  buildConstruction: 0,
  isEosDateEnable: false,
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

const PlannedActivityTracker: React.FC = (props) => {
  //PAGE
  const { readonly, KPIAdmin, admin, isPermesso } = useAuth();

  const [data, setData] = useState<PATReportDto | undefined | null>();
  const [swoEm, setSwoEm] = useState<{ [key: string]: string }>({});
  const [macroXlS, setMacroXlS] = useState<boolean>(false);
  const [selected, setSelected] = useState<
    { label: string; value: number }[] | undefined
  >(undefined);

  const [filterType, setFilterType] = useState<number | undefined>(0);
  const [radioValue, setRadioValue] = useState(
    paginationQueryPAT?.isEosDateEnable ? "eos" : "eom"
  );

  const { query, setQuery, next, back } = useResourceTableCrud(
    {
      ...paginationQueryPAT,
      isEosDateEnable: radioValue === "eos" ? true : false,
    },
    isPermesso ? GetReportPATGrid : undefined
  );

  const Grid = (state: RootState) =>
    state.reportPATGridReducer.ReportPATGridResult;
  const GridDto = useSelector(Grid);

  const getFiltersData = (state: RootState) =>
    state.reportPATGridReducer.filter;
  const filterData = useSelector(getFiltersData);

  const addExtraFilters = (): ReportPATQueryObjectGrid => {
    let newQuery = { ...query } as ReportPATQueryObjectGrid;
    if (selected && selected?.length > 0) {
      newQuery.vendorIds = selected.map((item) => item.value);
    } else {
      newQuery.vendorIds = [];
    }

    if (filterType !== undefined) {
      newQuery.buildConstruction = filterType;
    } else {
      newQuery.buildConstruction = undefined;
    }

    return newQuery;
  };

  const getSwoEm = async () => {
    let result = await GetSWOem();
    setSwoEm(result?.SWOemResources!);
  };

  const InvocheDownload = async () => {
    let result = await GetReportPATAllExports({
      ...paginationQueryPAT,
      isEosDateEnable: radioValue === "eos" ? true : false,
      pageSize: GridDto?.totalItems,
    });
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      downloadXLSX(url, "PlannedActivityTrackerAll", result?.fileName);
    }
  };

  const onViewReport = () => {
    GetReportPATGrid({
      ...addExtraFilters(),
      page: 1,
      isEosDateEnable: radioValue === "eos" ? true : false,
    });
  };

  const setFilterSelected = (e: any) => {
    console.log("==> ", e);
    if (e) {
      setFilterType(e.key);
      GetVendorsOfPredefinedFilter(e.key).then((res) => {
        const selectedId = dictionaryToArray(res).map((item) => +item.value);
        const selectedList = dictionaryToArray(swoEm)
          .filter((item) => selectedId.includes(+item.key))
          .map((el) => ({ label: el.value, value: el.key }));

        console.log("selected list => ", selectedList);
        setSelected(selectedList);
      });
      return;
    }

    setFilterType(undefined);
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
      getSwoEm();
      // GetReportPATGrid(paginationQueryPAT);
    }
  }, [isPermesso]);

  useEffect(() => {
    if (
      filterData !== undefined &&
      filterData !== null &&
      macroXlS === true &&
      isPermesso
    ) {
      GetReportPATAllExports({
        ...addExtraFilters(),
        pageSize: GridDto?.totalItems,
        isEosDateEnable: radioValue === "eos" ? true : false,
        page: 1,
      })
        .then((res) => {
          if (res !== undefined) {
            let url = window.URL.createObjectURL(res.file);
            downloadXLSX(
              url,
              "PlannedActivityTracker",
              res?.fileName,
              filterData?.map((obj) => obj.text)
            );
            setMacroXlS(false);
          }
        })
        .catch((error) => {
          console.log(
            "An error occurred during getting GetReportPATAllExports api",
            error
          );
          setMacroXlS(false);
        });
    }
  }, [macroXlS, isPermesso]);

  const onDownloadToXLS = async () => {
    setLoader("ADD", "GetReportPATExports");
    try {
      await GetFilterColumReportPAT("productName", "", {
        ...addExtraFilters(),
        pageSize: GridDto?.totalItems,
        isEosDateEnable: radioValue === "eos" ? true : false,
        page: 1,
      });
      setLoader("REMOVE", "GetReportPATExports");
      setMacroXlS(true);
    } catch (error) {
      console.log("An error occurred during getting filter service api", error);
      setLoader("REMOVE", "GetReportPATExports");
      let result = await GetReportPATAllExports({
        ...addExtraFilters(),
        pageSize: GridDto?.totalItems,
        isEosDateEnable: radioValue === "eos" ? true : false,
        page: 1,
      });
      if (result !== undefined) {
        let url = window.URL.createObjectURL(result.file);
        downloadXLSX(
          url,
          "PlannedActivityTrackerAll",
          result?.fileName,
          undefined
        );
      }
    }
  };

  return (
    <div className="pageContainer">
      <div className="headerPage row mx-0 justify-content-between position-relative">
        <div>
          <h3 className="voda-bold">Planned Activity Tracker</h3>
        </div>

        {(KPIAdmin || admin) && (
          <div className="">
            <button
              className="px-4 btnHeader clearBtn br-20 voda-bold mrl-10"
              onClick={() => InvocheDownload()}
            >
              <img
                src={
                  darkMode
                    ? require("../img/MS Excel_white.png")
                    : require("../img/MS Excel_black.png")
                }
                className="excel"
              />
              Export All Data
            </button>
            <button
              className="px-4 btnHeader clearBtn br-20 voda-bold"
              type="button"
              onClick={() => window.location.reload()}
            >
              Refresh
            </button>
          </div>
        )}
      </div>
      <div className="row mt-1 mx-0">
        <div className="col-4 px-0">
          <div className="form-group col-12 pl-0">
            <label className="voda-bold w-100 text-left themeText mb-1">
              Please Select a Filter<span className="red">*</span>
            </label>
            <div className="d-flex text-left" style={{ color: "black" }}>
              <div className="w-100">
                <Select
                menuPosition={"fixed"}
                  // styles={customStyles}
                  options={dictionaryToArray(FORMAT_ENUM)}
                  value={dictionaryToArray(FORMAT_ENUM).filter(
                    (item) => item.key === filterType
                  )}
                  onChange={(e) => setFilterSelected(e)}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.value}
                  getOptionValue={(option) => option.key.toString()}
                />
              </div>
            </div>
          </div>
        </div>

        <div className="col-8">
          <div className="row">
            <div className="col-6 px-2">
              <div className="form-group col-12">
                <label className="w-100 voda-bold text-left themeText mb-1">
                  Export To Vendor<span className="red">*</span>
                </label>
                <div className="d-flex PA">
                  <MultiSelect
                    className="w-100"
                    options={dictionaryToArray(swoEm).map((item) => ({
                      value: item.key,
                      label: item.value,
                    }))}
                    value={selected ?? []}
                    onChange={setSelected}
                    labelledBy="Select"
                  />
                </div>
              </div>
            </div>
            <div
              className="col-6 px-2"
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
                <Dropdown className="pl-2 d-inline more-options grid-main-btn">
                  <Dropdown.Toggle id="dropdown-autoclose-inside">
                    More Options
                  </Dropdown.Toggle>

                  <Dropdown.Menu className="grid-main-btn">
                    {!readonly && (
                      <>
                        <ButtonGroup className="patBtnGroup">
                          {[
                            { name: "EOM", value: "eom" },
                            { name: "EOS", value: "eos" },
                          ].map((radio, idx) => (
                            <ToggleButton
                              key={idx}
                              id={`radio-${idx}`}
                              type="radio"
                              variant={
                                radioValue === radio.value
                                  ? "outline-primary"
                                  : "outline-secondary"
                              }
                              name="radio"
                              value={radio.value}
                              checked={radioValue === radio.value}
                              onChange={(e) => {
                                setQuery({
                                  ...addExtraFilters(),
                                  isEosDateEnable:
                                    e.currentTarget.value === "eos"
                                      ? true
                                      : false,
                                });
                                setRadioValue(e.currentTarget.value);
                              }}
                            >
                              {radio.name}
                            </ToggleButton>
                          ))}
                        </ButtonGroup>
                        <Dropdown.Item>Legend</Dropdown.Item>
                        <div className="bubbleMenuLegenda">
                          <div className="triangleBubbleTop-right"></div>
                          <div className="col-12 row mx-0 px-2 my-2">
                            <div className="w-100 mx-0 py-1 d-flex align-items-center">
                              <div className="legendaElement green"></div>
                              <span className="legendaElement">{`${
                                radioValue === "eom" ? "EOM" : "EOS"
                              } Active`}</span>
                            </div>
                            <div className="w-100 mx-0 py-1 d-flex align-items-center">
                              <div className="legendaElement yellow"></div>
                              <span className="legendaElement">
                                {`${
                                  radioValue === "eom" ? "EOM" : "EOS"
                                } Expired & Plan Available`}
                              </span>
                            </div>
                            <div className="w-100 mx-0 py-1 d-flex align-items-center">
                              <div className="legendaElement red"></div>
                              <span className="legendaElement">
                                {`${
                                  radioValue === "eom" ? "EOM" : "EOS"
                                } Expired & No Plan`}
                              </span>
                            </div>
                          </div>
                        </div>
                      </>
                    )}
                  </Dropdown.Menu>
                </Dropdown>
              </div>
            </div>
          </div>
        </div>
      </div>
      {data && (
        <>
          <PATReportTable
            data={data}
            //viewReport={viewReport}
            pagination={query}
            action={{
              Filter: (query: ReportPATQueryObjectGrid) => {
                setQuery(query);
                GetReportPATGrid(query);
              },
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
export default PlannedActivityTracker;
