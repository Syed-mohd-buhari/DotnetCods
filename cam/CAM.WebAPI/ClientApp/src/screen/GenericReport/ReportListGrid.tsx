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
  GenericReportDtoGrid,
  GenericReportQueryObjectGrid,
} from "../../Model/GenericReport";
import { GetFilterColumGenericReport } from "../../Redux/Action/GenericReport/GenericReportCommonAction";
import { Link } from "react-router-dom";
import { calculateBodyWidths } from "../../Utils/gridFunction";
import { useTheme } from "../../Context/ThemeContext";

interface Props {
  action: {
    onDelete(id: number | undefined, name: string, orphan?: boolean): any;
    EditNotDetail(id: number | undefined): any;
    EditAndDetail(
      id: number | undefined,
      idDetail: number | string | undefined
    ): any;
    Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
    onPublish(id: number, name: string): any;
    onDownload(id: number, mode): any;
    onOverride(list: any): any;
    onCloneReport(details: any): any;
    previewReport(id: number | undefined, name: string): any;
    closeModal(): any;
  };
  data: GenericReportDtoGrid[] | undefined;
  pagination: GenericReportQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const GenericReportGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<GenericReportDtoGrid[] | undefined>([]);
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
    state.genericReportGridReducer.filter;
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
  } = useFilterTableCrud<GenericReportQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumGenericReport,
    props.pagination
  );

  const [selectAll, setSelectAll] = useState<boolean>(false);
  const [selectedRows, setSelectedRows] = useState<any>([]);
  const [isVisibleModalStatus, setIsVisibleModalStatus] =
    useState<boolean>(false);
  const { darkMode } = useTheme();

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

  const DeleteReport = (item: any) => {
    props.action.onDelete(item.dynamicReportsId, item.reportName);
  };
  const PreviewReport = (item: any) => {
    props.action.previewReport(item.dynamicReportsId, item.reportName);
  };
  const PublishReport = (item: any) => {
    props.action.onPublish(item.dynamicReportsId, item.reportName);
  };

  const DownloadReport = (item: any) => {
    props.action.onDownload(item.dynamicReportsId, "disaggregated");
  };

  const CloneReport = (item: any) => {
    props.action.onCloneReport(item);
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
        <td className="p-0">
          <div className="btn-group mr-2" role="group" aria-label="First group">
            <button
              type="button"
              className="btn pt-2 pb-2"
              data-toggle="tooltip"
              data-placement="top"
              title="View"
              onClick={() => PreviewReport(item)}
            >
              <FaRegEye color={`${darkMode ? "white" : ""}`} />
            </button>
            <button
              type="button"
              className="btn pt-2 pb-2"
              data-toggle="tooltip"
              data-placement="top"
              title="Download"
              onClick={() => {
                DownloadReport(item);
              }}
            >
              <PiDownloadFill color={`${darkMode ? "white" : ""}`} />
            </button>
            {admin && (
              <>
                <Link
                  to={{ pathname: `/genericreporting` }}
                  state={{ reportId: item.dynamicReportsId, isEdit: true }}
                >
                  {!readonly && (
                    <button
                      type="button"
                      className="btn pt-2 pb-2"
                      data-toggle="tooltip"
                      data-placement="top"
                      title="Edit"
                    >
                      <MdEdit color={`${darkMode ? "white" : ""}`} />
                    </button>
                  )}
                </Link>
                {!readonly && (
                  <button
                    type="button"
                    className={`btn pt-2 pb-2`}
                    data-toggle="tooltip"
                    data-placement="top"
                    title={"Clone"}
                    onClick={() => CloneReport(item)}
                  >
                    <FaClone color={`${darkMode ? "white" : ""}`} />
                  </button>
                )}
                {!readonly && (
                  <button
                    type="button"
                    className={`btn pt-2 pb-2 ${
                      item?.published === "Published" && "disabledCursor"
                    }`}
                    data-toggle="tooltip"
                    data-placement="top"
                    title={`${
                      item?.published === "Published"
                        ? "Report Published"
                        : "Publish"
                    }`}
                    disabled={item?.published === "Published" ? true : false}
                    onClick={() => {
                      PublishReport(item);
                    }}
                  >
                    <MdPublishedWithChanges
                      color={`${
                        item?.published === "Published"
                          ? "green"
                          : darkMode
                          ? "white"
                          : ""
                      }`}
                    />
                  </button>
                )}
                {!readonly && (
                  <button
                    type="button"
                    className="btn pt-2 pb-2"
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Delete"
                    onClick={() => {
                      DeleteReport(item);
                    }}
                  >
                    <MdDelete color="red" />
                  </button>
                )}
              </>
            )}
          </div>
        </td>
      </tr>
    );
  };
  return (
    <>
      <ModalConfirm data={confirm} showHyperLink={false} />
      <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
        <div className="mx-0 px-0 flex-row table-container">
          <table className="table-responsive table-thead-sticky" tabIndex={-1}>
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
                <th key={"action"}>
                  <div className="h-100 d-flex align-items-center divFilter w-16rem">
                    {SelectFilterType(
                      "genericReportAction",
                      1,
                      props.pagination?.isSortAscending,
                      filtriAttivi,
                      actionFilterDate,
                      props.pagination?.sortBy,
                      filterData,
                      count,
                      actionFilterCK,
                      thAction,
                      thActionDate,
                      "genericReportAction",
                      undefined,
                      undefined,
                      true
                    )}
                  </div>
                </th>
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

export default GenericReportGrid;
