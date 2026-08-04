import React, { SetStateAction, useEffect, useState } from "react";
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
import ModalAduitStatus from "../Audit/ModalAduitStatus";
import { MdPreview, MdDelete, MdPublishedWithChanges } from "react-icons/md";
import { PiDownloadFill } from "react-icons/pi";
import { BiSolidEditAlt } from "react-icons/bi";
import { FaRegEye } from "react-icons/fa";
import {
  GenericReportDtoGrid,
  GenericReportQueryObjectGrid,
  GenericViewReportQueryObjectGrid,
} from "../../Model/GenericReport";
import { GetFilterColumGenericPreviewReport } from "../../Redux/Action/GenericReport/GenericReportCommonAction";

interface Props {
  action: {
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
    closeModal(): any;
  };
  data: GenericReportDtoGrid[] | undefined;
  pagination: GenericViewReportQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

const GenericReportGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<GenericReportDtoGrid[] | undefined>([]);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const { readonly } = useAuth();
  const getFiltersData = (state: RootState) =>
    state.genericPreviewReportGridReducer.filter;
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
  } = useFilterTableCrud<GenericViewReportQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumGenericPreviewReport,
    props.pagination
  );

  const [selectAll, setSelectAll] = useState<boolean>(false);
  const [selectedRows, setSelectedRows] = useState<any>([]);
  const [isVisibleModalStatus, setIsVisibleModalStatus] =
    useState<boolean>(false);

  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
  }, [props.data]);

  useEffect(() => {
    props.action.setIsFiltriAttivati(isFiltriAttivati);
  }, [isFiltriAttivati]);

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

  return (
    <>
      <ModalConfirm data={confirm} showHyperLink={false} />
      <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
        <div className="mx-0 px-0 py-3 flex-row">
          <table className="table-responsive">
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
                      false,
                      item.colorHeader ?? undefined,
                      false,
                      item?.updatedPropertyName === null ||
                        item.propertyName === item?.updatedPropertyName
                        ? undefined
                        : item?.updatedPropertyName
                    )
                  )}
              </tr>
            </thead>
            <tbody>
              {data?.map((item, index) => (
                <tr
                  className={`dati ${
                    props.orphanColor && item.orphan ? "orphan" : null
                  }`}
                  key={item.dynamicReportId}
                >
                  {props.renderGrid
                    .filter((x) => x.show)
                    .sort((a, b) => a.order - b.order)
                    .map((td, i) =>
                      SelectGridType(
                        item[td.propertyName],
                        td.propertyName,
                        td.type
                      )
                    )}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
};

export default GenericReportGrid;
