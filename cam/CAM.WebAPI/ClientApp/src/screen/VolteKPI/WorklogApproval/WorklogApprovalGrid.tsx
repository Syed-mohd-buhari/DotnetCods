import React, { SetStateAction, useEffect, useState, useRef } from "react";
import { useSelector } from "react-redux";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/VolteKPI.css";
import {
  SelectFilterType,
  SelectGridType,
} from "../../../Hook/CommonRenderGrid/GridRender";
import { useAuth } from "../../../Hook/useAuth";
import { useFilterTableCrud } from "../../../Hook/useFilterTableCrud";
import {
  DataModalConfirm,
  RenderDetail,
  stateConfirm,
} from "../../../Model/Common";
import {
  VolteKPIWorklogDto,
  WorklogApprovalQueryObjectGrid,
} from "../../../Model/VolteKpi/WorklogApproval";
import { setNotification } from "../../../Redux/Action/NotificationAction";
import { GetWorklogApprovalDuplicates } from "../../../Redux/Action/VolteKPI/WorklogApproval/WorklogApprovalEditAction";
import {
  GetFilterColumWorklogApproval,
  GetWorklogApprovalGrid,
} from "../../../Redux/Action/VolteKPI/WorklogApproval/WorklogApprovalGridAction";
import { NotifyType } from "../../../Redux/Reducer/NotificationReducer";
import { RootState, rootStore } from "../../../Redux/Store/rootStore";
import ModalConfirm from "../../../Components/ModalConfirm";
import { calculateBodyWidths } from "../../../Utils/gridFunction";

interface Props {
  action: {
    Edit(item: VolteKPIWorklogDto): any;
    Filter(
      obj: SetStateAction<WorklogApprovalQueryObjectGrid> | undefined
    ): any;
    setEnablePendingRequest(val: boolean): any;
    getDuplicates(item: VolteKPIWorklogDto, setConfirm: Function);
  };
  data: VolteKPIWorklogDto[];
  pagination: WorklogApprovalQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  enablePendingRequest: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const WorklogApprovalsGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<VolteKPIWorklogDto[]>([]);
  const [dataModified, setDataModified] = useState<VolteKPIWorklogDto>();
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);

  const getFiltersData = (state: RootState) =>
    state.worklogApprovalsGridReducer.filter;
  let filterData = useSelector(getFiltersData);
  const {
    filtriAttivi,
    isVisibleFiltri,
    setIsVisibleFiltri,
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
  } = useFilterTableCrud<WorklogApprovalQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumWorklogApproval,
    props.pagination
  );

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

  // //CHIAMATA AL PARENT AL CAMBIO FILTRI
  // useEffect(() => {
  // 	props.action.Filter(filtriAttivi);
  // }, [filtriAttivi]);
  const thRefs = useRef<(HTMLTableCellElement | null)[]>([]);

  const thRefss = (ref, index) => {
    if (index === 0) {
      thRefs.current[0] = ref;
      calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
    } else if (index !== 0 && index <= 3) {
      thRefs.current[index] = ref;
      calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
    }
  };

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

  const { KPIAdmin, admin } = useAuth();

  const change = (
    property: string,
    obj: VolteKPIWorklogDto,
    value?: string
  ) => {
    // const filteredObj: VolteKPIWorklogDto | undefined = data?.find((e) => e.volteKPIType === obj.volteKPIType && e.opCoId === obj.opCoId && e.volteKPIId == obj.volteKPIId && e.month === obj.month && e.year === obj.year);
    const filteredObj: VolteKPIWorklogDto | undefined = data?.find(
      (e) => e.volteKPIWorklogId === obj.volteKPIWorklogId
    );
    if (filteredObj) {
      if (property === "targetMonthlyValueNew") {
        // filteredObj.approveValueChangeProposal = true;
        if (obj.volteKPIType == 3) {
          // filteredObj[property] = value != undefined && value != "" && !isNaN(+value) ? +value / 100 : undefined;
          filteredObj[property] =
            value != undefined && value != "" && !isNaN(parseFloat(value))
              ? +value
              : undefined;
        } else {
          filteredObj[property] =
            value != undefined && value != "" && !isNaN(+value)
              ? +value
              : undefined;
        }
      }
      if (property === "approved") {
        if (filteredObj.approved) {
          filteredObj.approved = false;
          filteredObj.targetMonthlyValueNew = filteredObj.targetMonthlyValueOld;
        } else {
          filteredObj.targetMonthlyValueNew =
            filteredObj.targetMonthlyValueProposed;
          filteredObj.approved = true;
        }
      }

      const idxOfFilteredObj = data && data.indexOf(filteredObj);
      const newItems =
        data &&
        data.map((el, idx) => (idx !== idxOfFilteredObj ? el : filteredObj));
      setData(newItems);
    }
  };

  // const getMonthlyTargetValueNew = useCallback(
  // 	(item: VolteKPITargetApprovalDto) => (item.type === 3 && item.monthlyTargetNew !== undefined && item.monthlyTargetNew !== null ? `${item.monthlyTargetNew * 100}` : item.monthlyTargetNew),
  // 	[data]
  // );

  const Save = async (item: VolteKPIWorklogDto) => {
    let errorMessage: string | undefined = undefined;
    item.approved = item.approved ?? false;
    setDataModified(item);
    let duplicated = await GetWorklogApprovalDuplicates(item);

    if (!duplicated || props.enablePendingRequest) {
      if (item.approved) {
        if (
          item.targetMonthlyValueNew !== null &&
          item.targetMonthlyValueNew !== undefined
        ) {
          errorMessage =
            item.volteKPIType === 3
              ? checkForKpiThree(item.targetMonthlyValueNew)
              : checkForOtherKpi(item.targetMonthlyValueNew);
        } else {
          errorMessage = "Target Monthly Value (New) cannot be empty";
        }
        if (errorMessage !== undefined) {
          rootStore.dispatch(
            setNotification({
              message: errorMessage,
              notifyType: NotifyType.warning,
            })
          );
        } else {
          // if (item.volteKPIType === 3) item.targetMonthlyValueNew = (item.targetMonthlyValueNew ?? 0) / 100;
          props.action.Edit(item);
        }
      } else {
        props.action.Edit(item);
        return;
      }
    } else {
      setDataModified(item);
      setConfirm({
        title: "Pending Proposal",
        message: `For ${item.opCo} ${item.kpiIdName} ${item.monthYear} there are others pending proposals`,
        button: "Continue",
        item: 0,
        isOpen: true,
        actions: {
          cancel: () => setConfirm(stateConfirm),
          confirm: () => props.action.getDuplicates(item, setConfirm),
        },
      } as DataModalConfirm);
    }
  };

  useEffect(() => {
    if (dataModified != undefined && props.enablePendingRequest === true) {
      let copy = [...data] as VolteKPIWorklogDto[];
      let index = copy.findIndex(
        (x) => x.volteKPIWorklogId === dataModified.volteKPIWorklogId
      );
      if (index != -1) {
        copy[index].approved = dataModified.approved;
        copy[index].targetMonthlyValueNew = dataModified.targetMonthlyValueNew;
        setData(copy);
      }
    }
  }, [props.enablePendingRequest]);

  const checkForKpiThree = (value: number) => {
    if (value > 100) {
      return "Target Monthly Value (New) must be smaller than 100";
    }
    return undefined;
  };

  const checkForOtherKpi = (value) => {
    if (value >= 1000000) {
      return "Target Monthly Value (New) must be smaller than 1.000.000";
    }
  };

  const rtnPercentualSymbol = (property: string, value: string | number) => {
    switch (property) {
      case "targetMonthlyValueNew":
      case "targetMonthlyValueOld":
      case "targetMonthlyValueProposed":
      case "eoyTargetOld":
      case "eoyTargetNew":
      case "actualMonthlyValueOld":
      case "actualMonthlyValueNew":
        if (value != undefined && value != null) {
          return value.toString() + "%";
        } else {
          return value;
        }
      default:
        return value;
    }
  };

  return (
    <div className="listaApparatiContainer mx-0 col-12 p-0 d-flex justify-content-center">
      <ModalConfirm data={confirm}></ModalConfirm>
      <div className="col-12 mx-0 px-0 flex-row table-container">
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
                    props.enablePendingRequest,
                    undefined,
                    undefined,
                    undefined,
                    true
                  )
                )}
            </tr>
          </thead>
          <tbody>
            {data &&
              data?.map((item, i) => (
                <tr
                  className={`dati ${item.isStored ? "" : ""}`}
                  key={item.volteKPIWorklogId}
                >
                  {props.renderGrid
                    .sort((a, b) => a.order - b.order)
                    .filter((x) => x.show)
                    .map((td, i) =>
                      td.propertyName === "targetMonthlyValueNew" &&
                      !item.isStored ? (
                        <td
                          className=" "
                          key={item.volteKPIWorklogId + td.propertyName}
                          ref={(ref) => {
                            if (i === 0) thRefs.current[0] = ref;
                            else if (i <= 2) thRefs.current[i] = ref;
                          }}
                        >
                          <input
                            type="number"
                            readOnly={
                              KPIAdmin ? item.approved || item.isStored : true
                            }
                            onKeyPress={(e) => {
                              e = e || window.event;
                              var charCode =
                                typeof e.which == "undefined"
                                  ? e.keyCode
                                  : e.which;
                              var charStr = String.fromCharCode(charCode);
                              if (!charStr.match(/^[0-9]+$/))
                                e.preventDefault();
                            }}
                            className={`w-100 py-1 px-2 hideArrow mr-1 ${
                              item.approved || item.isStored
                                ? "disabledBackground"
                                : ""
                            }`}
                            style={{
                              outline: "none",
                              border: "1px solid #aaa",
                              borderRadius: "5px",
                            }}
                            onChange={(e) =>
                              change(
                                "targetMonthlyValueNew",
                                item,
                                e.target.value
                              )
                            }
                            value={item.targetMonthlyValueNew ?? ""}
                          />
                          {item.volteKPIType === 3 ||
                          item.volteKPIType === 4 ? (
                            <label
                              className="d-flex align-item-center mb-0"
                              style={{ fontSize: "17px" }}
                            >
                              %
                            </label>
                          ) : null}
                        </td>
                      ) : td.propertyName === "approved" ? (
                        <td
                          className=" "
                          key={item.volteKPIWorklogId + td.propertyName}
                          ref={(ref) => {
                            if (i === 0) thRefs.current[0] = ref;
                            else if (i <= 2) thRefs.current[i] = ref;
                          }}
                        >
                          {item.isStored && item.approved === null ? (
                            <label>---</label>
                          ) : (
                            <label className="switch">
                              <input
                                disabled={KPIAdmin ? item.isStored : true}
                                type="checkbox"
                                onChange={(e) => change("approved", item)}
                                className=""
                                checked={item.approved}
                              />
                              <span
                                className={`${
                                  item.approved === null
                                    ? "slider"
                                    : "customSlider "
                                } round`}
                              ></span>
                            </label>
                          )}
                        </td>
                      ) : td.propertyName === "isStored" ? (
                        !item.isStored ? (
                          <td
                            className=" "
                            ref={(ref) => {
                              if (i === 0) thRefs.current[0] = ref;
                              else if (i <= 2) thRefs.current[i] = ref;
                            }}
                          >
                            <div className="d-flex flex-row">
                              <button
                                disabled={!KPIAdmin}
                                className="  voda-bold btn btn-danger px-4 btnHeader"
                                onClick={() => Save(item)}
                                type="button"
                              >
                                Save
                              </button>
                            </div>
                          </td>
                        ) : item.isStored &&
                          (item.approved == undefined ||
                            item.approved == null) ? (
                          <td
                            className=" "
                            ref={(ref) => {
                              if (i === 0) thRefs.current[0] = ref;
                              else if (i <= 2) thRefs.current[i] = ref;
                            }}
                          >
                            No Approval Required
                          </td>
                        ) : (
                          <td
                            className=" "
                            ref={(ref) => {
                              if (i === 0) thRefs.current[0] = ref;
                              else if (i <= 2) thRefs.current[i] = ref;
                            }}
                          >
                            Completed
                          </td>
                        )
                      ) : (
                        SelectGridType(
                          item.volteKPIType === 3 || item.volteKPIType === 4
                            ? rtnPercentualSymbol(
                                td.propertyName,
                                item[td.propertyName]
                              )
                            : item[td.propertyName],
                          td.propertyName,
                          td.type,
                          undefined,
                          undefined,
                          undefined,
                          undefined,
                          i,
                          thRefs,
                          thRefss
                        )
                      )
                    )}
                </tr>
              ))}
          </tbody>
        </table>
      </div>
      {/* {data?.length == 0 && <div className="text-center mt-4">No Item Founded</div>} */}
    </div>
  );
};

export default WorklogApprovalsGrid;
