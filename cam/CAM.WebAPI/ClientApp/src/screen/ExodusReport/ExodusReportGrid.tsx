import React, { SetStateAction, useEffect, useState, useRef } from "react";
import { Dropdown } from "react-bootstrap";
import { useSelector } from "react-redux";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { QueryObjectGrid, RenderDetail } from "../../Model/Common";
import { RootState } from "../../Redux/Store/rootStore";
import { useAuth } from "./../../Hook/useAuth";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";
import { ExodusDTO, ExodusGrid } from "../../Model/Report/Exodus";
import { GetFilterColumExodusReport } from "../../Redux/Action/Report/ExodusReportAction";

interface Props {
  action: {
    // onDelete(id: number | undefined, orphan?: boolean): any;
    // Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
  };
  data: ExodusGrid[] | undefined;
  pagination: ExodusDTO | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const ExodusReportGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<ExodusGrid[] | undefined>([]);
  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) =>
    state.exodusReportGridReducer.filter;
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
  } = useFilterTableCrud<ExodusDTO>(
    props.action.Filter,
    GetFilterColumExodusReport,
    props.pagination
  );

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
  const cleanValue = (v) => {
    return v === null || v === undefined ? "\u00A0" : v;
  };

  const mapProperty = (item: any, propertyName: string) => {
    const mapping: Record<string, string> = {
      lastModified: "lastModified", // adjust as needed
    };
    const key = mapping[propertyName] || propertyName;
    return item?.[key] ?? `--`;
  };
  return (
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
                    false,
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
            {data?.map((item, index) => {
              return (
                <tr className={`dati `} key={`${item.opCoId} + " " + ${index}`}>
                  {props.renderGrid
                    .filter((x) => x.show)
                    .sort((a, b) => a.order - b.order)
                    .map((td, i) => {
                      return SelectGridType(
                        cleanValue(item[td.propertyName]),
                        td.propertyName,
                        td.type,
                        "",
                        undefined,
                        undefined,
                        undefined,
                        i,
                        thRefs,
                        thRefss
                      );
                    })}
                  <td className="actions"></td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ExodusReportGrid;
