import React, { SetStateAction, useEffect, useState, useRef } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import {
  DataModalConfirm,
  QueryObjectGrid,
  RenderDetail,
  stateConfirm,
} from "../../Model/Common";
import { AuditDtoGrid, AuditQueryObjectGrid } from "../../Model/Audit";
import { GetFilterColumAudit } from "../../Redux/Action/Audit/AuditGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";
import ModalAduitStatus from "./ModalAduitStatus";
import { calculateBodyWidths } from "../../Utils/gridFunction";
import TH from "../../Components/TableCrud/TableCrudTH";
interface Props {
  action: {
    onDelete(id: number | undefined, orphan?: boolean): any;
    EditNotDetail(id: number | undefined): any;
    EditAndDetail(
      id: number | undefined,
      idDetail: number | string | undefined
    ): any;
    Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
    onApprove(list: any): any;
    onReject(list: any): any;
    onOverride(list: any): any;
    closeModal(): any;
  };
  data: AuditDtoGrid[] | undefined;
  pagination: AuditQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}
let firstIndex, secondIndex, thirdIndex;

const AuditGrid: React.FC<Props> = (props) => {
  console.log("Tems AuditGrid props", props);
  const [data, setData] = useState<AuditDtoGrid[] | undefined>([]);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) => state.auditGridReducer.filter;
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
  } = useFilterTableCrud<AuditQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumAudit,
    props.pagination
  );

  const [selectAll, setSelectAll] = useState<boolean>(false);
  const [selectedRows, setSelectedRows] = useState<any>([]);
  const [isVisibleModalStatus, setIsVisibleModalStatus] =
    useState<boolean>(false);

  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, [props.data]);

  useEffect(() => {
    props.action.setIsFiltriAttivati(isFiltriAttivati);
  }, [isFiltriAttivati]);

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

  const [isVisibleMajorBubble, setIsVisibleMajorBubble] = useState(0);
  const closeBubble = () => {
    setIsVisibleMajorBubble(0);
  };

  const Cancel = (msg) => {
    setConfirm({
      title: "Warning!",
      message: msg,
      cancelText: "Ok",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => {
          setConfirm(stateConfirm);
        },
      },
    });
  };

  const toggleSelectAll = () => {
    if (!selectAll) {
      // setSelectedRows(data?.filter(item => item.status === "Auto_Approved").map((item)=> item.auditHistoryId));
      setSelectedRows(data?.map((item) => item.auditHistoryId));
    } else {
      setSelectedRows([]);
    }
  };

  const toggleRow = (id: any) => {
    if (selectedRows?.includes(id)) {
      // data?.filter((item)=>{
      //    if(item.auditHistoryId !== id && item.status !== "Auto_Approved"){
      setSelectedRows(selectedRows?.filter((rowId) => rowId !== id));
      // }
      // })
    } else {
      setSelectedRows([...selectedRows, id]);
    }
  };

  useEffect(() => {
    // const filteredRows = data?.filter(item => item.status !== "Auto_Approved").map((item)=> item.auditHistoryId)
    // filteredRows && data && data?.length !== 0 && data?.length - filteredRows?.length === selectedRows?.length ? setSelectAll(true) : setSelectAll(false);
    // console.log(data?.length ,filteredRows?.length)
    selectedRows && data?.length !== 0 && selectedRows?.length == data?.length
      ? setSelectAll(true)
      : setSelectAll(false);
  }, [selectedRows]);

  const handleApprove = () => {
    const check = data
      ?.filter(
        (item) =>
          selectedRows?.includes(item?.auditHistoryId) &&
          item.status?.toLocaleLowerCase() !== "auto_approved"
      )
      .map((item) => item.auditHistoryId);

    if (check && check?.length > 0) {
      Cancel(
        "Select only the record with status code 'Auto Approved' to Approve."
      );
    } else {
      props.action.onApprove(selectedRows);
    }
  };

  const handleReject = () => {
    const check = data
      ?.filter(
        (item) =>
          selectedRows?.includes(item?.auditHistoryId) &&
          (item.status?.toLocaleLowerCase() === "rejected" ||
            item.status?.toLocaleLowerCase() === "override")
      )
      .map((item) => item.auditHistoryId);

    if (check && check?.length > 0) {
      Cancel(
        "Select only the record with status code 'Auto Approved' & 'Approved' to Reject."
      );
    } else {
      props.action.onReject(selectedRows);
    }
  };

  const handleOverride = () => {
    const check = data
      ?.filter(
        (item) =>
          selectedRows?.includes(item?.auditHistoryId) &&
          item.status?.toLocaleLowerCase() === "auto_approved"
      )
      .map((item) => item.auditHistoryId);

    if (selectedRows?.length > 1 || check?.length === 0) {
      setIsVisibleModalStatus(false);
      Cancel(
        "Select only one record with status code 'Auto Approved' to Override."
      );
    } else {
      setIsVisibleModalStatus(true);
    }
  };

  return (
    <>
      <ModalConfirm data={confirm} showHyperLink={false} />

      {isVisibleModalStatus && (
        <ModalAduitStatus
          show={isVisibleModalStatus}
          modalType={"override"}
          data={data?.filter((item) =>
            selectedRows?.includes(item?.auditHistoryId)
          )}
          action={{
            closeModal: () => {
              setIsVisibleModalStatus(false);
            },
            Override: (item) => props.action.onOverride(item),
          }}
        />
      )}

      <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
        <div className="mx-0 px-0 flex-row table-container">
          <table className="table-responsive table-thead-sticky">
            <thead>
              <tr className="intestazione">
                {!readonly && (
                  <TH
                    propertyName=""
                    isVisibleFiltriString=""
                    freezeHeader={true}
                    renderChildren={true}
                  >
                    <input
                      type="checkbox"
                      className="pointer"
                      checked={selectAll}
                      onChange={toggleSelectAll}
                    ></input>
                  </TH>
                )}
                {props.renderGrid
                  .filter((x) => x.show)
                  .sort((a, b) => a.order - b.order)
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
              {data?.map((item, index) => (
                <tr
                  className={`dati ${
                    props.orphanColor && item.orphan ? "orphan" : null
                  }`}
                  key={item.auditHistoryId}
                >
                  {!readonly && (
                    <td
                      ref={(ref) => {
                        thRefs.current[0] = ref;
                      }}
                    >
                      {/* {item?.status !== "Auto_Approved"  ?  
                      <input
                        type="checkbox"
                        className="disabledCursor"
                        disabled
                        title={`Already this record is ${item?.status}`}
                      ></input>
                    : */}
                      <input
                        type="checkbox"
                        className="pointer"
                        checked={selectedRows?.includes(item?.auditHistoryId)}
                        onChange={() => toggleRow(item.auditHistoryId)}
                      ></input>
                      {/* } */}
                    </td>
                  )}
                  {props.renderGrid
                    .filter((x) => x.show)
                    .sort((a, b) => a.order - b.order)
                    .map((td, i) =>
                      td.propertyName === "nodeType" ? (
                        <td className=" ">{item.nodeType ?? "null"}</td>
                      ) : (
                        SelectGridType(
                          item[td.propertyName],
                          td.propertyName,
                          td.type,
                          undefined,
                          undefined,
                          undefined,
                          undefined,
                          i + 1,
                          thRefs,
                          thRefss
                        )
                      )
                    )}
                  <td className="actions"></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        <div className="col-12 justify-content-center mt-4 d-flex footerModal">
          {!readonly && (
            <button
              className={` voda-bold btn px-4 btnHeader cancel ${
                selectedRows?.length === 0 ? "disabledCursor" : "pointer"
              }`}
              onClick={() => handleApprove()}
              disabled={selectedRows?.length === 0 ? true : false}
              title="Select at least one record to Approve."
              type="button"
            >
              Approve
            </button>
          )}
          {!readonly && (
            <button
              className={` voda-bold btn px-4 btnHeader cancel ${
                selectedRows?.length === 0 ? "disabledCursor" : "pointer"
              }`}
              onClick={() => handleReject()}
              disabled={selectedRows?.length === 0 ? true : false}
              title="Select at least one record to Reject."
              type="button"
            >
              Reject
            </button>
          )}
          {!readonly && (
            <button
              className={` voda-bold btn px-4 btnHeader cancel ${
                selectedRows?.length === 0 ? "disabledCursor" : "pointer"
              }`}
              onClick={() => handleOverride()}
              disabled={selectedRows?.length === 0 ? true : false}
              title="Select only one record to Override."
              type="button"
            >
              Override
            </button>
          )}
          {/* <button
          className=" voda-bold btn btn-danger px-4 btnHeader"
          //onClick={() => props.action.closeModal(changed)}
          type="button"
        >
          Cancel
        </button> */}
        </div>
      </div>
    </>
  );
};

export default AuditGrid;
