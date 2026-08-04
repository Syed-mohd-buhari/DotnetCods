import React, { useState, useEffect, SetStateAction, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import {
  DesignComponentFamilyDtoGrid,
  DesignComponentFamilyQueryObjectGrid,
} from "../../Model/DesignComponentFamily";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import {
  GetFilterColumDesignComponentFamily,
  GetOpenImplementation,
  GetOpenDCS,
  GetOpenImplementationStatus,
} from "../../Redux/Action/DesignComponentFamily/DesignComponentFamilyGridAction";
import { Link } from "react-router-dom";

import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { RenderDetail } from "../../Model/Common";

import { Dropdown } from "react-bootstrap";
import ImplementationTable from "../../Containers/Lookup/Implementation";
import { Modal } from "react-bootstrap";
import DCS from "./../../Containers/Lookup/DCS";

import { useNavigate, useLocation } from "react-router-dom";
import ImplementationStatusTable from "../../Containers/Lookup/ImplementationStatusTable";
import { GetProductNameByVodafoneName } from "../../Redux/Action/LookUp/VodafoneName/VodafoneNameCommonAction";
import { toggleState } from "../../Hook/Common";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

interface Props {
  action: {
    onDelete(id: number | undefined, orphan?: boolean): any;
    Edit(id: number | undefined): any;
    Restore(id: number | undefined): any;
    Filter(
      obj: SetStateAction<DesignComponentFamilyQueryObjectGrid> | undefined
    ): any;
    setIsFiltriAttivati(value: boolean): any;
  };
  data: DesignComponentFamilyDtoGrid[] | undefined;
  pagination: DesignComponentFamilyQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
  readonly?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const DesignComponentFamilyGrid = (props) => {
  const [data, setData] = useState<DesignComponentFamilyDtoGrid[] | undefined>(
    []
  );

  const getFiltersData = (state: RootState) =>
    state.designComponentFamilyGridReducer.filter;
  let filterData = useSelector(getFiltersData);
  const {
    filtriAttivi,
    resetFilter,
    closeAll,
    setDateToChildren,
    orderBy,
    resetFilterDate,
    getFilters,
    updateCount,
    getFiltriAttivi,
    count,
    checkFilterinValue,
    checkFilterDateinValue,
    isVisibleFiltriString,
    setIsVisibleFiltriString,
    isFiltriAttivati,
  } = useFilterTableCrud<DesignComponentFamilyQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumDesignComponentFamily,
    props.pagination
  );

  let thRefs = useRef<Array<HTMLTableCellElement | null>>([
    null,
    null,
    null,
    null,
  ]); // Refs for th elements

  const thRefss = (ref, index) => {
    if (index === 0) {
      thRefs.current[0] = ref;
      calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
    } else if (index !== 0 && index <= 3) {
      thRefs.current[index] = ref;
      calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
    }
  };

  //CARICAMENTO INIZIALE
  useEffect(() => {
    setData(props.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, []);

  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, [props.data]);

  useEffect(() => {
    props.action.setIsFiltriAttivati(isFiltriAttivati);
  }, [isFiltriAttivati]);

  useEffect(() => {
    if (props.editIS) {
      openImplementationPopUp(props.editIS);
    }
  }, [props.editIS]);

  //CHIAMATA AL PARENT AL CAMBIO FILTRI
  useEffect(() => {
    props.action.Filter(filtriAttivi);
  }, [filtriAttivi]);

  const [dropdownStates, setDropdownStates] = useState({});

  const [dropdownPosition, setDropdownPosition] = useState<{
    left: number;
    top: number;
  }>({ left: 0, top: 0 });

  const dropdownRef = useRef<HTMLDivElement>(null);

  // Function to toggle dropdown state for a specific row
  const toggleDropdown = (rowId, implementation, isDeleted, orphan, event) => {
    setDropdownStates(() => ({
      rowId,
      implementation,
      isDeleted,
      orphan,
    }));
    const left = event.clientX + window.scrollX;
    const top = event.clientY + window.scrollY;

    const windowWidth = window.innerWidth + window.scrollX;
    const windowHeight = window.innerHeight + window.scrollY;

    const maxLeft = windowWidth - 500;
    const maxTop = windowHeight - 400;
    setDropdownPosition({
      left: Math.min(left, maxLeft),
      top: Math.min(top, maxTop),
    });
  };

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current) {
        if (
          event.target.tagName === "IMG" &&
          event.target.classList.contains("dropdown_trigger")
        ) {
          return;
        }
        setDropdownStates({});
      }
    };

    // Attach the event listener when the component mounts
    document.addEventListener("click", handleClickOutside);

    // Clean up the event listener when the component unmounts
    return () => {
      document.removeEventListener("click", handleClickOutside);
    };
  }, []);

  const thAction = {
    checkFilter: checkFilterinValue,
    settingVisibility: setIsVisibleFiltriString,
    resetFilter: resetFilter,
  };
  const actionFilterCK = {
    closeAll,
    updateCount,
    getFiltriAttivi,
    orderBy,
    getFilters,
  };
  const actionFilterDate = { closeAll, setDateToChildren, orderBy };
  const thActionDate = {
    checkFilter: checkFilterDateinValue,
    settingVisibility: setIsVisibleFiltriString,
    resetFilter: resetFilterDate,
  };

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  const [implementationTableData, setImplementationTableData] = useState<any>();
  const [dcsTableData, setDCSTableData] = useState<any>();
  const [implementationStatusData, seImplementationStatusData] =
    useState<any>();
  const [detailId, setDetailId] = useState(null);
  const [dcfId, setDcfId] = useState<number>(0);
  const [isVisibleVodafoneBubble, setIsVisibleVodafoneBubble] = useState(0);
  const navigate = useNavigate();
  const location: any = useLocation();
  const [relatedVodafoneNames, setRelatedVodafoneNames] = useState([]);

  //const [dataResponse, setDataResponse] = useState<DesignAspectDtoCreate>();

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE

  const returnLookup = () => {
    switch (isVisibleModalLookup) {
      case 1:
        return <ImplementationTable data={implementationTableData} />;
      case 2:
        return <DCS data={dcsTableData} />;
      case 3:
        return <ImplementationStatusTable data={implementationStatusData} />;
      default:
        return null;
    }
  };

  const openImplementationPopUp = (id: number) => {
    GetOpenImplementation(id).then((response) => {
      setImplementationTableData(response.data!);
      setIsVisibleModalLookup(1);
    });
  };

  const openImplementationStatusLookUp = (id: number) => {
    GetOpenImplementationStatus(id).then((response) => {
      seImplementationStatusData(response.data);
      setIsVisibleModalLookup(3);
    });
  };

  const openDCSLookup = (id: number) => {
    GetOpenDCS(id).then((response) => {
      setDCSTableData(response.data);
      setIsVisibleModalLookup(2);
    });
  };

  const openEditDesignAspects = async (id: number) => {
    if (id) {
      //sessionStorage.setItem("dcfId", id.toString());
      navigate({
        pathname: "/designAspect",
        search: `?dcfId=${id}`, // query string
      });
    }
  };
  const closeVodafoneBubble = () => {
    setIsVisibleVodafoneBubble(0);
  };
  const getRelatedVodafoneNames = async (vodafoneId, index) => {
    const result = await GetProductNameByVodafoneName(vodafoneId);
    setRelatedVodafoneNames(result);
    setIsVisibleVodafoneBubble(toggleState(index + 1, isVisibleVodafoneBubble));
  };

  return (
    <>
      <Dialog
        open={isVisibleModalLookup === 0 ? false : true}
        onClose={() => {
          setDcfId(0);
          props.onOpenImplementationPopUp(null);
          setIsVisibleModalLookup(0);
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            <h6 className="mt-2">
              {isVisibleModalLookup === 2
                ? "Preview Design Components for "
                : isVisibleModalLookup === 1
                ? "This Design Component Family is implemented in "
                : isVisibleModalLookup === 3
                ? "Further Implementation Details"
                : ""}
              <span
                dangerouslySetInnerHTML={{
                  __html:
                    isVisibleModalLookup === 2
                      ? implementationTableData?.dcfName
                      : "",
                }}
              ></span>
            </h6>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => {
            setDcfId(0);
            props.onOpenImplementationPopUp(null);
            setIsVisibleModalLookup(0);
          }}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          <>
            {returnLookup()}
            {/* <ImplementationTable data={implementationTableData} /> */}
            {isVisibleModalLookup === 1 && (
              <button
                className="voda-bold btn btn-danger px-4 fr mb-4 mt-4 mr-2"
                type="button"
                onClick={() => {
                  openImplementationStatusLookUp(dcfId ? dcfId : props?.editIS);
                }}
              >
                More Details
              </button>
            )}
            {isVisibleModalLookup === 3 && (
              <button
                className="voda-bold btn btn-danger px-4 fr mb-4 mt-4 cancel"
                style={{ marginRight: "10px", color: "#fff" }}
                type="button"
                onClick={() => setIsVisibleModalLookup(1)}
              >
                Back
              </button>
            )}
            {isVisibleModalLookup === 1 ||
            isVisibleModalLookup === 2 ||
            isVisibleModalLookup === 3 ? (
              <button
                className="voda-bold btn btn-link px-4 fr mb-4 mt-4 cancel"
                style={{ marginRight: "10px" }}
                type="button"
                onClick={() => {
                  setDcfId(0);
                  props.onOpenImplementationPopUp(null);
                  setIsVisibleModalLookup(0);
                }}
              >
                Close
              </button>
            ) : null}
          </>
        </DialogContent>
      </Dialog>

      <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
        <div
          className="mx-0 px-0 flex-row table-container"
          style={{ position: "relative" }}
        >
          {!props.readonly && dropdownStates["rowId"] && (
            <td className="actions">
              <Dropdown
                className="d-inline mx-2"
                show={dropdownStates["rowId"] ? true : false}
                ref={dropdownRef}
                style={
                  dropdownStates["rowId"]
                    ? {
                        position: "absolute",
                        top: `${dropdownPosition.top - 150}px`,
                        left: `${dropdownPosition.left}px`,
                        transform: "translate(-50%, -50%)",
                        zIndex: 9999,
                      }
                    : {
                        position: "absolute",
                        top: "0px",
                        left: "0px",
                        margin: "0px",
                        opacity: "0",
                      }
                }
              >
                <div
                  className={`${
                    dropdownStates["rowId"]
                      ? "dropdown-menu show"
                      : "dropdown-menu"
                  }`}
                >
                  {dropdownStates["isDeleted"] ? (
                    <Dropdown.Item
                      onClick={() =>
                        props.action.Restore(dropdownStates["rowId"])
                      }
                    >
                      Restore
                    </Dropdown.Item>
                  ) : (
                    <>
                      <Dropdown.Item
                        onClick={() => {
                          props.action.Edit(dropdownStates["rowId"]);
                        }}
                      >
                        Edit
                      </Dropdown.Item>
                      <Dropdown.Item
                        onClick={() => openDCSLookup(dropdownStates["rowId"]!)}
                      >
                        Associated Design Components
                      </Dropdown.Item>
                      <Dropdown.Item
                        disabled={!dropdownStates["implementation"]}
                        onClick={() => {
                          openImplementationPopUp(dropdownStates["rowId"]!);
                          setDcfId(dropdownStates["rowId"]!);
                        }}
                      >
                        Implementation Status
                      </Dropdown.Item>

                      <Dropdown.Item
                        as={Link}
                        disabled={!dropdownStates["implementation"]}
                        to={{
                          pathname: "/designAspect",
                          search: "dcfId=" + dropdownStates["rowId"],
                        }}
                        state={{
                          dcfId: dropdownStates["rowId"],
                          prevPage: "designcomponentfamily",
                        }}
                      >
                        Associated Design Aspects
                      </Dropdown.Item>

                      <Dropdown.Item
                        onClick={() =>
                          props.action.onDelete(
                            dropdownStates["rowId"],
                            dropdownStates["orphan"]
                          )
                        }
                      >
                        Delete
                      </Dropdown.Item>
                    </>
                  )}
                </div>
              </Dropdown>
            </td>
          )}
          <table className="table-responsive table-thead-sticky" tabIndex={-1}>
            <thead>
              <tr className="intestazione">
                {props.renderGrid
                  .sort((a, b) => a.order - b.order)
                  .filter((x) => x.show)
                  .map((item, i) =>
                    SelectFilterType(
                      item.propertyName,
                      item.type,
                      props.pagination?.isSortAscending,
                      filtriAttivi,
                      actionFilterDate,
                      props.pagination?.sortBy,
                      filterData,
                      count,
                      actionFilterCK,
                      thAction,
                      thActionDate,
                      isVisibleFiltriString,
                      undefined,
                      undefined,
                      undefined,
                      undefined,
                      undefined,
                      undefined,
                      true
                    )
                  )}
                {props.renderGrid && props.renderGrid.length ? (
                  <th className="customWidth"></th>
                ) : (
                  ""
                )}
              </tr>
            </thead>
            <tbody>
              {data &&
                data?.map((item, index) => (
                  <tr
                    className={`dati ${
                      props.orphanColor && item.orphan ? "orphan" : null
                    }`}
                    key={item.designComponentFamilyId}
                    onDoubleClick={(e) =>
                      toggleDropdown(
                        item.designComponentFamilyId,
                        item.implementation,
                        item.deleted,
                        item.orphan,
                        e
                      )
                    }
                  >
                    {props.renderGrid
                      .sort((a, b) => a.order - b.order)
                      .filter((x) => x.show)
                      .map((td, i) =>
                        td.propertyName === "gdprRelevant" &&
                        (item.gdprRelevant === null ||
                          item.gdprRelevant === undefined) ? (
                          SelectGridType(
                            "UNSPECIFIED",
                            td.propertyName,
                            td.type,
                            "",
                            undefined,
                            undefined,
                            undefined,
                            i,
                            thRefs,
                            thRefss
                          )
                        ) : td.propertyName === "vodafoneName" ? (
                          <td
                            className={`${
                              isVisibleVodafoneBubble == index + 1
                                ? "majorHardware hasModal"
                                : "majorHardware"
                            }`}
                            key={`${td.propertyName}${i}`}
                            ref={(ref) => {
                              if (i === 0) thRefs.current[0] = ref;
                              else if (i <= 2) thRefs.current[i] = ref;
                            }}
                          >
                            <div
                              className="majorHardware"
                              onClick={() =>
                                getRelatedVodafoneNames(
                                  item["vodafoneNameId"],
                                  index
                                )
                              }
                            >
                              {item?.vodafoneName != undefined ? (
                                <a
                                  className=""
                                  dangerouslySetInnerHTML={{
                                    __html: item.vodafoneName ?? "---",
                                  }}
                                ></a>
                              ) : (
                                "---"
                              )}
                            </div>
                            {isVisibleVodafoneBubble == index + 1 ? (
                              <div
                                className="bubbleMenu pl-2"
                                onMouseLeave={closeVodafoneBubble}
                              >
                                <div className="triangleBubbleTop"></div>
                                <div className="col-12 row mx-0 px-2 my-2">
                                  <nav className="nav flex-column">
                                    {item?.vodafoneName != undefined ? (
                                      <label className="mb-0  text-white">
                                        <ol>
                                          {relatedVodafoneNames &&
                                            relatedVodafoneNames?.map(
                                              (vdnames) => <li>{vdnames}</li>
                                            )}
                                        </ol>
                                      </label>
                                    ) : null}
                                  </nav>
                                </div>
                              </div>
                            ) : null}
                          </td>
                        ) : (
                          SelectGridType(
                            item[td.propertyName],
                            td.propertyName,
                            td.type,
                            "",
                            undefined,
                            undefined,
                            undefined,
                            i,
                            thRefs,
                            thRefss
                          )
                        )
                      )}
                    <td className="actions">
                      {!props.readonly && (
                        <div className="d-inline mx-2 cursor-pointer">
                          <img
                            className="dropdown_trigger"
                            src={require("../../img/options_dots.png")}
                            style={{ cursor: "pointer", padding: "10px" }}
                            onClick={(e) => {
                              toggleDropdown(
                                item.designComponentFamilyId,
                                item.implementation,
                                item.deleted,
                                item.orphan,
                                e
                              );
                            }}
                          />
                        </div>
                      )}
                    </td>
                  </tr>
                ))}
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
};

export default DesignComponentFamilyGrid;
