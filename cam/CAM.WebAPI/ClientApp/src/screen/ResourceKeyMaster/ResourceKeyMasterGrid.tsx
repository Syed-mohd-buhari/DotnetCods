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
import {
  ResourceKeyMasterDtoGrid,
  ResourceKeyMasterQueryObjectGrid,
} from "../../Model/ResourceKeyMaster";
import { GetFilterColumResourceKeyMaster } from "../../Redux/Action/ResourceKeyMaster/ResourceKeyMasterGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import { useAuth } from "./../../Hook/useAuth";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";

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
  };
  data: ResourceKeyMasterDtoGrid[] | undefined;
  pagination: ResourceKeyMasterQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const ResourceKeyMasterGrid: React.FC<Props> = (props) => {
  console.log("Tems ResourceKeyMasterGrid props", props);
  const [data, setData] = useState<ResourceKeyMasterDtoGrid[] | undefined>([]);
  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) =>
    state.resourceKeyMasterGridReducer.filter;
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
  } = useFilterTableCrud<ResourceKeyMasterQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumResourceKeyMaster,
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
                  item.propertyName == "plannedActivity"
                    ? SelectFilterType(
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
                        "Planned Activity",
                        undefined,
                        undefined,
                        true
                      )
                    : SelectFilterType(
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
                key={item.identityid}
              >
                {props.renderGrid
                  .filter((x) => x.show)
                  .sort((a, b) => a.order - b.order)
                  .map((td, i) =>
                    td.propertyName === "nodeType" ? (
                      <td
                        className=" "
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        {item.nodeType ?? "null"}
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
                <td className="actions"></td>
                {/* <td className="actions">
                  {!readonly && (
                    <Dropdown className="d-inline mx-2">
                      <Dropdown.Toggle id="dropdown-autoclose-true">
                        <ThreeDot/>
                      </Dropdown.Toggle>
 
                      <Dropdown.Menu>
                        {item.deleted ? (
                          <Dropdown.Item
                            onClick={() =>
                              props.action.Restore(item.networkElementAsIsId)
                            }
                          >
                            Restore
                          </Dropdown.Item>
                        ) : (
                          <>
                            <Dropdown.Item
                              onClick={() =>
                                props.action.EditNotDetail(
                                  item.networkElementAsIsId
                                )
                              }
                            >
                              Edit
                            </Dropdown.Item>
                            <Dropdown.Item
                              onClick={() =>
                                props.action.onDelete(
                                  item.networkElementAsIsId,
                                  item.orphan
                                )
                              }
                            >
                              Delete
                            </Dropdown.Item>
                          </>
                        )}
                      </Dropdown.Menu> 
                    </Dropdown>
                  )}
                </td> */}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ResourceKeyMasterGrid;
