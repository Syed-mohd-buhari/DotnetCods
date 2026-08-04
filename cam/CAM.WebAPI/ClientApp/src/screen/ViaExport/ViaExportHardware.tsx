import React, { SetStateAction, useEffect, useState, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { QueryObjectGrid, RenderDetail } from "../../Model/Common";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import { ViaExport, ViaExportQuery } from "../../Model/ViaExport/ViaExport";
import { GetFilterColumViaExportHardware } from "../../Redux/Action/ViaExport/ViaExportHardwareGridAction";
import { calculateBodyWidths } from "../../Utils/gridFunction";

interface Props {
  action: {
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
  };
  data: ViaExport[] | undefined;
  pagination: ViaExportQuery | undefined;
  renderGrid: RenderDetail[];
}

let firstIndex, secondIndex, thirdIndex;
const ViaExportHardware: React.FC<Props> = (props) => {
  const [data, setData] = useState<ViaExport[] | undefined>([]);
  const getFiltersData = (state: RootState) =>
    state.viaExportHardwareGridReducer.filter;
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
  } = useFilterTableCrud<ViaExportQuery>(
    props.action.Filter,
    GetFilterColumViaExportHardware,
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

  const optionValue = (data: string) => {
    if (data && data !== "" && data != null) {
      try {
        let teset = "";

        var dataObj = JSON.parse(data);
        for (var key in dataObj) {
          teset += `${key} : ${dataObj[key]}  </br> `;
        }
        return teset;
      } catch (error) {
        return data;
      }
    }
  };

  return (
    <div className="listaApparatiContainer mt-3 row mx-0 col-12 p-0 mb-3">
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
                    false,
                    item.colorHeader,
                    undefined,
                    undefined,
                    undefined,
                    undefined,
                    true
                  )
                )}
              <th className=" ">
                <div className="divFilter viaExports customWidth"></div>
              </th>
            </tr>
          </thead>
          <tbody>
            {data?.map((item, index) => (
              <tr className="dati" key={`HW${index}`}>
                {props.renderGrid
                  .sort((a, b) => a.order - b.order)
                  .filter((x) => x.show)
                  .map((td, i) =>
                    SelectGridType(
                      item[td.propertyName],
                      td.propertyName,
                      td.type,
                      "",
                      undefined,
                      item["eomStatus"],
                      "eomStatus",
                      i,
                      thRefs,
                      thRefss
                    )
                  )}
                <td className=" " style={{ minWidth: "100px" }}></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ViaExportHardware;
