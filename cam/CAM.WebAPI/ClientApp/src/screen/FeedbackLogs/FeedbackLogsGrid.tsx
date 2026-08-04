import React, { SetStateAction, useEffect, useState, useRef } from "react";
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
import { RootState } from "../../Redux/Store/rootStore";
import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";
import {
  MdPreview,
  MdDelete,
  MdPublishedWithChanges,
  MdEdit,
} from "react-icons/md";
import { PiDownloadFill } from "react-icons/pi";
import { BiSolidEditAlt } from "react-icons/bi";
import { FaRegClone, FaRegEye } from "react-icons/fa";
import { FaClone } from "react-icons/fa6";
import {
  FeedbackLogsDtoGrid,
  FeedbackLogsQueryObjectGrid,
} from "../../Model/FeedbackLogs";
import { GetFilterColumFeedbackLogs } from "../../Redux/Action/FeedbackLogs/FeedbackLogsCommonAction";
import { Link } from "react-router-dom";
import { calculateBodyWidths } from "../../Utils/gridFunction";

interface Props {
  action: {
    EditNotDetail(id: number | undefined): any;
    EditAndDetail(
      id: number | undefined,
      idDetail: number | string | undefined
    ): any;
    Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
    onDownload(id: number, mode): any;
    closeModal(): any;
  };
  data: FeedbackLogsDtoGrid[] | undefined;
  pagination: FeedbackLogsQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

const FeedbackLogsGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<FeedbackLogsDtoGrid[] | undefined>([]);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const {
    tipologicaPermesso,
    VerifyIsInRole,
    KPIAdmin,
    KPIEditor,
    admin,
    simpleUser,
    readonly,
    role,
  } = useAuth();
  const getFiltersData = (state: RootState) =>
    state.FeedbackLogsGridReducer.filter;
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
  } = useFilterTableCrud<FeedbackLogsQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumFeedbackLogs,
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

  let firstIndex, secondIndex, thirdIndex;

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

  const DownloadReport = (item: any) => {
    props.action.onDownload(item.dynamicReportsId, "disaggregated");
  };

  const TrData = ({ item }) => {
    return (
      <tr
        className={`dati ${props.orphanColor && item.orphan ? "orphan" : null}`}
        key={item.dynamicReportId}
      >
        {props.renderGrid
          .filter((x) => x.show)
          .sort((a, b) => a.order - b.order)
          .map((td, i) =>
            SelectGridType(
              item[td.propertyName],
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
          )}
      </tr>
    );
  };
  return (
    <>
      <ModalConfirm data={confirm} showHyperLink={false} />
      <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
        <div className="mx-0 px-0 flex-row table-container">
          <table className="table-responsive table-thead-sticky">
            <thead>
              <tr className="intestazione">
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
              </tr>
            </thead>
            <tbody>
              {data?.map((item, index) => (
                <TrData key={item.dynamicReportId} item={item} />
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
};

export default FeedbackLogsGrid;
